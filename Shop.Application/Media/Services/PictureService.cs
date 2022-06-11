using Shop.Application.Media.Settings;
using Shop.Domain.Media;
using SkiaSharp;

namespace Shop.Application.Media.Services
{
    public class PictureService : AbstractService<Picture>, IPictureService
    {
        private const string IMAGE_FOLDER = "images";
        private const string THUMB_FOLDER = "thumbs";
        private readonly MediaSetting _mediaSetting;
        private readonly IStorageService _storageService;

        public PictureService(
            ShopDbContext context,
            MediaSetting mediaSetting,
            IStorageService storageService) : base(context)
        {
            _mediaSetting = mediaSetting;
            _storageService = storageService;
        }

        public Stream Resize(SKBitmap image, SKEncodedImageFormat format, int size)
        {
            Guard.IsNotNull(image, nameof(image));
            Guard.IsGreaterThan(size, 0, nameof(size));

            int w, h;
            if (image.Width > image.Height)
            {
                w = size;
                h = (int)(image.Height * size / (double)image.Width);
            }
            else
            {
                h = size;
                w = (int)(image.Width * size / (double)image.Height);
            }

            using var resizeBitmap = image.Resize(new SKImageInfo(w, h), SKFilterQuality.Medium);
            using var cropImage = SKImage.FromBitmap(resizeBitmap);

            int quality = _mediaSetting.DefaultPictureQuality;
            if (quality <= 0)
                quality = 80;

            return cropImage.Encode(format, quality).AsStream();
        }

        public string GetPicturePath(string fileName)
            => _storageService.GetAbsolutePath(IMAGE_FOLDER, fileName);

        public string GetThumbPicturePath(string fileName)
            => _storageService.GetAbsolutePath(IMAGE_FOLDER, THUMB_FOLDER, fileName);

        public string GetPictureUrl(string fileName)
            => _storageService.GetUrl(Path.Combine(IMAGE_FOLDER, fileName));

        public string GetThumbPictureUrl(string fileName)
            => _storageService.GetUrl(Path.Combine(IMAGE_FOLDER, THUMB_FOLDER, fileName));

        public async Task<Picture> GetPictureByIdAsync(int id, bool tracked = false)
            => await Table.FindByIdAsync(id, tracked: tracked);

        public async Task<string> GetDefaultPictureUrlAsync(int size, PictureType type = PictureType.Entity)
        {
            Guard.IsGreaterThanOrEqualTo(size, 0, nameof(size));

            var fileName = type switch
            {
                PictureType.Avatar => _mediaSetting.DefaultPictureAvatar,
                _ => _mediaSetting.DefaultPictureEntity
            };

            var absoulutePath = GetPicturePath(fileName); 

            if (!await _storageService.Exist(absoulutePath))
                return string.Empty;

            if (size == 0)
                return GetPictureUrl(fileName);

            var extension = Path.GetExtension(absoulutePath);
            var thumbFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{size}{extension}";
            var thumbFilePath = GetThumbPicturePath(thumbFileName);

            if (!await _storageService.Exist(thumbFilePath))
            {
                using var mutex = new Mutex(false, thumbFileName);
                mutex.WaitOne();

                try
                {
                    var stream = await _storageService.GetStreamFromPathAsync(absoulutePath);
                    using var image = SKBitmap.Decode(stream);
                    var codec = SKCodec.Create(fileName);
                    await _storageService.SaveAsync(Resize(image, codec.EncodedFormat, size), thumbFilePath);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }

            return GetThumbPictureUrl(thumbFileName);
        }

        public async Task<string> GetPictureUrlAsync(int id, int size, bool showDefalutPicture = true, PictureType type = PictureType.Entity)
            => await GetPictureUrlAsync(await GetPictureByIdAsync(id), size, showDefalutPicture, type);

        public async Task<string> GetPictureUrlAsync(Picture picture, int size, bool showDefalutPicture = true, PictureType type = PictureType.Entity)
        {
            Guard.IsGreaterThanOrEqualTo(size, 0, nameof(size));

            if (picture == null)
                return showDefalutPicture ?
                    await GetDefaultPictureUrlAsync(size, type) :
                    string.Empty;

            string orginalFileName = $"{picture.Id}{picture.Extension}";
            string originalFilePath = GetPicturePath(orginalFileName);

            string thumbFileName;
            if (size == 0)
            {
                thumbFileName = $"{picture.Id}_{picture.Name}{picture.Extension}";
                var thumbFilePath = GetThumbPicturePath(thumbFileName);
                if (!await _storageService.Exist(thumbFilePath))
                {
                    using var mutex = new Mutex(false, thumbFileName);
                    mutex.WaitOne();
                    try
                    {
                        var stream = await _storageService.GetStreamFromPathAsync(originalFilePath);
                        await _storageService.SaveAsync(stream, thumbFilePath);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }

                return GetThumbPictureUrl(thumbFileName);
            }

            thumbFileName = $"{picture.Id}_{picture.Name}_{size}{picture.Extension}";
            var thumFilePath = GetThumbPicturePath(thumbFileName);
            if (!await _storageService.Exist(thumFilePath))
            {
                using var mutex = new Mutex(false, thumbFileName);
                try
                {
                    var stream = await _storageService.GetStreamFromPathAsync(originalFilePath);
                    using var image = SKBitmap.Decode(stream);
                    var codec = SKCodec.Create(thumbFileName);
                    await _storageService.SaveAsync(Resize(image, codec.EncodedFormat, size), thumFilePath);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }

            return GetThumbPictureUrl(thumbFileName);
        }

        public async Task<Picture> SaveAsync(IFormFile file)
        {
            Guard.IsNotNull(file, nameof(file));

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var name = file.FileName.Split('.')[0].NormalizeFileName();
                var extension = Path.GetExtension(file.FileName).ToLower();

                var picture = new Picture
                {
                    Name = name,
                    Size = file.Length,
                    Extension = extension,
                    MimeType = file.ContentType,
                };

                var stream = file.OpenReadStream();
                using var image = SKBitmap.Decode(stream);

                picture.Width = image.Width;
                picture.Height = image.Height;

                await Table.AddAsync(picture);
                await _context.SaveChangesAsync();

                var maxSize = _mediaSetting.DefaultMaximumPictureSize;
                if (maxSize > 0 && Math.Max(image.Width, image.Height) > maxSize)
                {
                    var codec = SKCodec.Create(file.FileName);
                    stream = Resize(image, codec.EncodedFormat, maxSize);
                }

                var fileName = $"{picture.Id}{picture.Extension}";
                var absolutePath = GetPicturePath(fileName);
                await _storageService.SaveAsync(stream, absolutePath, picture.MimeType);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return null;
        }

        public async Task DeleteAsync(Picture picture)
        {
            Guard.IsNotNull(picture, nameof(picture));

            Table.Remove(picture);

            var fileName = $"{picture.Id}{picture.Extension}";
            await _storageService.DeleteAsync(GetPicturePath(fileName));

            await DeleteThumbPictureAsync(picture);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteThumbPictureAsync(Picture picture)
        {
            Guard.IsNotNull(picture, nameof(picture));

            var pattern = $"{picture.Id}*.*";
            var directory = _storageService.GetAbsolutePath(IMAGE_FOLDER, THUMB_FOLDER);

            var files = _storageService.GetFiles(directory, pattern);

            foreach(var fileName in files)
            {
                var thumbFilePath = GetThumbPicturePath(fileName);

                await _storageService.DeleteAsync(thumbFilePath);
            }
        }

        public async Task SetNamePictureAsync(Picture picture, string newName)
        {
            Guard.IsNotNull(picture, nameof(picture));
            Guard.IsNotNullOrEmpty(newName, nameof(newName));

            if (picture.Name == newName)
                return;

            await DeleteThumbPictureAsync(picture);

            picture.Name = newName.NormalizeFileName();

            await _context.SaveChangesAsync();
        }
    }
}

using Shop.Application.Media.Settings;

namespace Shop.Infrastructure.Storage.Local
{
    public class LocalStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly MediaSetting _mediaSetting;
        private readonly IHttpContextAccessor _httpContext;
        public LocalStorageService(
            IWebHostEnvironment env, 
            MediaSetting mediaSetting,
            IHttpContextAccessor httpContext)
        {
            _env = env;
            _mediaSetting = mediaSetting;
            _httpContext = httpContext;
        }

        public string GetUrl(string path)
        {
            var httpContext = _httpContext.HttpContext;
            string host = string.Empty;
            
            if(httpContext != null)
            {
                var request = httpContext.Request;
                if(_mediaSetting.AutoGenerateAbsoluteUrl)
                {
                    host = string.Format("//{0}/{1}", request.Host, request.PathBase);
                }
                else
                {
                    host = request.PathBase;
                }
            }

            if (host.IsEmpty() || !host.EndsWith('/'))
                host += "/";

            string url = Path.Combine(host, path);

            return url;
        }

        public string GetAbsolutePath(params string[] paths)
        {
            List<string> segments = new();

            var root = _env.WebRootPath;
            if (paths.Length == 0)
                return root;

            if(!paths.Contains(root))    
                segments.Add(root);

            segments.AddRange(paths);

            return Path.Combine(segments.ToArray());
        }

        public async Task<bool> Exist(string path)
        {
            Guard.IsNotNullOrEmpty(path, nameof(path));

            return await Task.FromResult(File.Exists(path));
        }

        public IEnumerable<string> GetFiles(string directory, string pattern = "*", bool topDirectoryOnly = true)
        {
            Guard.IsNotNullOrEmpty(directory, nameof(directory));   
            Guard.IsNotNull(pattern, nameof(pattern));

            return Directory.GetFileSystemEntries(directory, pattern,
                new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    MatchCasing = MatchCasing.CaseInsensitive,
                    RecurseSubdirectories = !topDirectoryOnly,

                });
        }

        public async Task<Stream> GetStreamFromPathAsync(string path)
        {
            Guard.IsNotNullOrEmpty(path, nameof(path));

            using var ms = new MemoryStream(await File.ReadAllBytesAsync(path));

            return ms;
        }

        public async Task SaveAsync(Stream stream, string path, string mimeType = null)
        {
            Guard.IsNotNull(stream, nameof(stream));    
            Guard.IsNotNullOrEmpty(path, nameof(path));

            var dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fs);
        }

        public async Task DeleteAsync(string path)
        {
            Guard.IsNotNullOrEmpty(path, nameof(path));

            if (File.Exists(path))
                File.Delete(path);

            await Task.CompletedTask;
        }
    }
}

using Shop.Domain.Media;

namespace Shop.Application.Media.Services
{
    public interface IPictureService : IAbstractService<Picture>
    {
        Task<Picture> GetPictureByIdAsync(int id, bool tracked = false);

        Task<string> GetPictureUrlAsync(int id, int size, bool showDefalutPicture = true, PictureType type = PictureType.Entity);

        Task<string> GetPictureUrlAsync(Picture picture, int size, bool showDefalutPicture = true, PictureType type = PictureType.Entity);

        Task<Picture> SaveAsync(IFormFile file);

        Task DeleteAsync(Picture picture);

        Task SetNamePictureAsync(Picture picture, string newName);
    }
}

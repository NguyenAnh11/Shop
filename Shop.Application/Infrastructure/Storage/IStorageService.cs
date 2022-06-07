namespace Shop.Application.Infrastructure.Storage
{
    public interface IStorageService
    {
        string GetUrl(string path);

        string GetAbsolutePath(params string[] paths);

        Task<bool> Exist(string path);

        IEnumerable<string> GetFiles(string directory, string pattern = "*", bool topDirectoryOnly = true);

        Task<Stream> GetStreamFromPathAsync(string path);

        Task SaveAsync(Stream stream, string path, string mimeType = null);

        Task DeleteAsync(string path);
    }
}

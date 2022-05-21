namespace Shop.Application.Messages.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewPath);

        Task<string> RenderToStringAsync<T>(string viewPath, T model);
    }
}

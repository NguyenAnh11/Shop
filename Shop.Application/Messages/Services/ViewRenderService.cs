namespace Shop.Application.Messages.Services
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        public ViewRenderService(IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderToStringAsync(string viewPath)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using var sw = new StringWriter();
            var view = _viewEngine.GetView(null, viewPath, false);

            if (!view.Success)
                throw new ArgumentNullException($"{viewPath} does not match any avaliable view");

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

            var viewContext = new ViewContext(
                actionContext,
                view.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions());

            await view.View.RenderAsync(viewContext);

            return sw.ToString();
        }

        public async Task<string> RenderToStringAsync<T>(string viewPath, T model) 
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using var sw = new StringWriter();
            var view = _viewEngine.GetView(null, viewPath, false);

            if (!view.Success)
                throw new ArgumentNullException($"{viewPath} does not match any avaliable view");

            var viewDictionary = new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model,
            };

            var viewContext = new ViewContext(
                actionContext,
                view.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions());

            await view.View.RenderAsync(viewContext);

            return sw.ToString();
        }
    }
}

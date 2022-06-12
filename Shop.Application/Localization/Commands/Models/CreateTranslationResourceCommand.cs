namespace Shop.Application.Localization.Commands.Models
{
    public class CreateTranslationResourceCommand : IRequest<Response<int>>
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

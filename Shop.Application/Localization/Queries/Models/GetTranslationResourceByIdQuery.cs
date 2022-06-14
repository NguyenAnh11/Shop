using Shop.Application.Localization.Dtos;

namespace Shop.Application.Localization.Queries.Models
{
    public class GetTranslationResourceQuery  : IRequest<TranslationResourceDto>
    {
        public int Id { get; set; }
        public GetTranslationResourceQuery(int id)
        {
            Id = id;
        }
    }
}

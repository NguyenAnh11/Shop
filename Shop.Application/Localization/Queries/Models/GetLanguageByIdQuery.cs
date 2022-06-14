using Shop.Application.Localization.Dtos;

namespace Shop.Application.Localization.Queries.Models
{
    
    public class GetLanguageByIdQuery : IRequest<LanguageDto>
    {
        public int Id { get; set; }
        public GetLanguageByIdQuery(int id)
        {
            Id = id;
        }
    }
}

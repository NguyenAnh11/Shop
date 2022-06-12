using Shop.Application.Localization.Dtos;

namespace Shop.Application.Localization.Queries.Models
{
    
    public class GetLanguageQuery : IRequest<LanguageDto>
    {
        public int Id { get; set; }
        public GetLanguageQuery(int id)
        {
            Id = id;
        }
    }
}

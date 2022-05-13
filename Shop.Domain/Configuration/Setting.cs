using Shop.SharedKernel;

namespace Shop.Domain.Configuration
{
    public class Setting : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

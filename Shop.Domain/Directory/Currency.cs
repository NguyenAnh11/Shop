using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Directory
{
    public class Currency : BaseEntity, IClock
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public decimal Rate { get; set; }
        public int Round { get; set; }
        public CurrencyRoundRule CurrencyRound
        {
            get => (CurrencyRoundRule)Round;
            set => Round = (int)value;
        }
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
    }
}

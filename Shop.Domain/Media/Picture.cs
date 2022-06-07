using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Media
{
    public class Picture : BaseEntity, IClock
    {
        public string Name { get; set; }
        public long? Size { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string Alt { get; set; }
        public string Title { get; set; }
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
    }
}

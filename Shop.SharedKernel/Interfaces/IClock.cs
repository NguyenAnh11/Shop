namespace Shop.SharedKernel.Interfaces
{
    public interface IClock
    {
        DateTime CreateUtc { get; }
        DateTime? UpdateUtc { get; }
    }
}

namespace Shop.Application.Infrastructure.Models
{
    public interface IPagedList<T> : IList<T>
    {
        int Page { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPage { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        bool IsFristPage { get; }
        bool IsLastPage { get; }
    }
}

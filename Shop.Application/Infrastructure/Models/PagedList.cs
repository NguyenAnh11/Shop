namespace Shop.Application.Infrastructure.Models
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IList<T> source, int page, int pageSize, int? totalCount = null)
        {
            Page = page;
            PageSize = pageSize;

            TotalCount = totalCount ?? 0;
            TotalPage = TotalCount / PageSize;
            if (TotalCount / PageSize != 0)
                TotalPage += 1;

            HasPreviousPage = Page > 1;
            HasNextPage = Page < TotalPage;

            IsFristPage = Page == 1;
            IsLastPage = Page == TotalPage;

            AddRange(totalCount == null ? source.Skip((Page - 1) * PageSize).Take(PageSize) : source);
        }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public int TotalPage { get; init; }
        public bool HasPreviousPage { get; init; }
        public bool HasNextPage { get; init; }
        public bool IsFristPage { get; init; }
        public bool IsLastPage { get; init; }
    }
}

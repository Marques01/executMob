namespace Domain.Entities.Request
{
    public record Paginate<T>
    {
        public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();

        public int CurrentPage { get; init; }

        public int PageSize { get; init; }

        public int TotalPages { get; init; }

        public int TotalRows { get; init; }

        public Paginate()
        {
        }

        public Paginate(IEnumerable<T> items, int currentPage, int pageSize, int totalPages, int totalRows)
        {
            Items = items;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalRows = totalRows;
        }
    }
}

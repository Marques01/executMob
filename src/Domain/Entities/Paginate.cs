namespace Domain.Entities
{
    public record Paginate<T>
    {
        public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();

        public int CurrentPage { get; init; }

        public int PageSize { get; init; }

        public int TotalPages { get; init; }

        public int TotalRows { get; init; }

        public Paginate(IEnumerable<T> items, int currentPage, int pageSize, int totalRows)
        {
            Items = items;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalRows / pageSize);
            TotalRows = totalRows;
        }
    }
}

namespace ResearchAtlas.Application.Abstractions.Queries;

public sealed record PagedResult<T>(IReadOnlyCollection<T> Items, int TotalCount, PageRequest Page)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / Page.PageSize);

    public bool HasPreviousPage => Page.PageNumber > 1;

    public bool HasNextPage => Page.PageNumber < TotalPages;
}

namespace ResearchAtlas.Application.Abstractions.Queries;

public sealed record PageRequest(int PageNumber = 1, int PageSize = 20)
{
    public int Skip => (PageNumber - 1) * PageSize;
}

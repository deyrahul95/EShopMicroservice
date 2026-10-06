namespace BuildingBlock.Pagination;

public class PaginatedResult<TEntity>(
    int pageNumber,
    int pageSize,
    long totalCount,
    IEnumerable<TEntity> data)
    where TEntity : class
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public long TotalCount { get; } = totalCount;
    public IEnumerable<TEntity> Data { get; } = data;
}

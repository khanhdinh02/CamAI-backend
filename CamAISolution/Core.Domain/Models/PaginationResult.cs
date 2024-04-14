namespace Core.Domain.Models;

public class PaginationResult<T>
{
    public PaginationResult() { }

    public PaginationResult(IEnumerable<T> values, int pageIndex, int pageSize)
    {
        Values = values.Skip(pageSize * pageIndex).Take(pageSize).ToList();
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = Values.Count;
    }

    public IList<T> Values { get; set; } = new List<T>();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool IsValuesEmpty => Values.Count == 0;
}

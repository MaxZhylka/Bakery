namespace backend.Core.DTOs
{
  public class PaginatedResult<T>
  {
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    public int Total { get; set; }
  }
}
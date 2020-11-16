namespace DTO.ActionResult
{
    public class SearchResult<T>
    {
        public T[] Items { get; set; }
        public int TotalCount { get; set; }
    }
}

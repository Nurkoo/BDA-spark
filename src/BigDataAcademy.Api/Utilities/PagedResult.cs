namespace BigDataAcademy.Api.Utilities;

public class PagedResult<T>
{
    public required PagedCriteria PagedCriteria { get; set; }

    public required long Total { get; set; }

    public required int TotalPages { get; set; }

    public required T[] Items { get; set; }
}

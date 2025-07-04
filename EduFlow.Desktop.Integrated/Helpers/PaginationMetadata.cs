namespace EduFlow.Desktop.Integrated.Helpers;

public class PaginationMetadata
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}

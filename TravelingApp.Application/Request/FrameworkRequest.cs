namespace TravelingApp.Application.Request
{
    public class FrameworkRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? OrderBy { get; set; } = string.Empty;
        public bool OrderByAsc { get; set; } = true;
    }
}

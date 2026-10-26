namespace myfilms.Pagination;

public class FilmsParams
{
    const int maxPageSize = 20;
    public int PageNumber { get; set; }
    private int _pageSize;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize)? maxPageSize : value;
        }
    }
}
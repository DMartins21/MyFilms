namespace myfilms.Pagination;

public abstract class QueryStringParams
{
    const int maxPageSize = 30;
    public int PageNumber { get; set; }
    private int _pageSize = maxPageSize;

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
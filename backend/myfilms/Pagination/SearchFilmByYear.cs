namespace myfilms.Pagination;

public class SearchFilmByYear : QueryStringParams
{
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
}
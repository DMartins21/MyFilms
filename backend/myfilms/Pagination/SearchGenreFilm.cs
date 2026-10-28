namespace myfilms.Pagination;

public class SearchGenreFilm : QueryStringParams
{
    public List<string>? Genre { get; set; }
}
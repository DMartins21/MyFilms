using System.Linq.Expressions;
using myfilms.Models;
using myfilms.Pagination;

namespace myfilms.Repositories.Interfaces;

public interface IFilmeRepository : IRepository<Filme>
{
    Task<Filme> GetByFilmIdAsync(int id);
    Task<PagedList<Filme>> GetFilms(FilmsParams  filmsParams);
    Task<PagedList<Filme>> GetFilmsWithGenres(SearchGenreFilm  filmsGenreParams);
    Task <PagedList<Filme>> GetFilmsWithYear(SearchFilmByYear  filmsYearParams);
    Task<IEnumerable<Filme>> GetByTitleAsync(string title);
    Task<IEnumerable<Filme>> SearchAsync(Expression<Func<Filme, bool>> predicate);
    Task CreateRangeAsync(IEnumerable<Filme> films);
}
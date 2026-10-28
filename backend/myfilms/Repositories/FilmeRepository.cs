using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.NativeInterop;
using myfilms.Context;
using myfilms.Models;
using myfilms.Pagination;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class FilmeRepository : Repository<Filme>, IFilmeRepository
{
    public FilmeRepository(AppDbContext context) : base(context){}

    public async Task<PagedList<Filme>> GetFilms(FilmsParams filmsParams)
    {
        var filmes = await _context.Filmes.AsQueryable()
            .ToListAsync();
        var filmesOrdenados = PagedList<Filme>
            .ToPagedList(filmes.AsQueryable(), filmsParams.PageNumber, filmsParams.PageSize);
        
        return filmesOrdenados;
    }


    public async Task<PagedList<Filme>> GetFilmsWithGenres(SearchGenreFilm filmsGenreParams)
    {
        var generos = filmsGenreParams.Genre;
        if (generos == null || generos.Count == 0)
            return null;
        
        var allFilms = await _context.Filmes
            .ToListAsync();

        var films = allFilms.AsQueryable();

        films = films
            .Where(f => f.Genre!.Any(g => generos.Contains(g, StringComparer.OrdinalIgnoreCase)))
            .OrderBy(f => f.Id);
        
        var filmsQuery = PagedList<Filme>
            .ToPagedList(films, filmsGenreParams.PageNumber, filmsGenreParams.PageSize);
        
        return filmsQuery;
    }

    public async Task<PagedList<Filme>> GetFilmsWithYear(SearchFilmByYear filmByYearParams)
    {
        var allFilms = await _context.Filmes.ToListAsync();
        var films = allFilms.AsQueryable();
        if(filmByYearParams.From.HasValue && filmByYearParams.To.HasValue)
        {
            films = films
                .Where(f => f.ReleaseDate >= filmByYearParams.From && f.ReleaseDate <= filmByYearParams.To)
                .OrderBy(f => f.ReleaseDate);
        }
        else if (filmByYearParams.From.HasValue && !filmByYearParams.To.HasValue)
        {
            films = films
                .Where(f => f.ReleaseDate >= filmByYearParams.From)
                .OrderBy(f => f.ReleaseDate);
        }
        else if (!filmByYearParams.From.HasValue && filmByYearParams.To.HasValue)
        {
            films = films
                .Where(f => f.ReleaseDate <= filmByYearParams.To)
                .OrderByDescending(f => f.ReleaseDate);
        }
        var filmsQuery = PagedList<Filme>
            .ToPagedList(films, filmByYearParams.PageNumber, filmByYearParams.PageSize);
        
        return filmsQuery;
        
    }
    

    public async Task<Filme> GetByFilmIdAsync(int id)
    {
        var search = await _context.Filmes
            .FindAsync(id);
        return search;
    }

    public async Task<IEnumerable<Filme>> GetByTitleAsync(string title)
    {
        var query = await _context.Filmes
            .Where(f => f.Title.ToLower().Contains(title.ToLower()))
            .OrderBy(f => f.Id)
            .ToListAsync();
    
        return query;
    }

    public async Task<IEnumerable<Filme>> SearchAsync(Expression<Func<Filme, bool>> predicate)
    {
        var query = await _context.Filmes.Where(predicate).ToListAsync();
        return query;
    }

    public async Task CreateRangeAsync(IEnumerable<Filme> films)
    {
        await _context.Filmes.AddRangeAsync(films);
    }
}
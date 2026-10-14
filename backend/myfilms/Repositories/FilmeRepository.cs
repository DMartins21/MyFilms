using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using myfilms.Context;
using myfilms.Models;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class FilmeRepository : Repository<Filme>, IFilmeRepository
{
    public FilmeRepository(AppDbContext context) : base(context){}


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
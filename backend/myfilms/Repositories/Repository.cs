using Microsoft.EntityFrameworkCore;
using myfilms.Context;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class Repository<T> : IRepository<T> where T  : class
{
    protected readonly AppDbContext _context;
    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
        return await _context.Set<T>()
            .AsNoTracking().ToListAsync();
    }

    public async Task CreateAsync(T item)
    {
        await _context.Set<T>().AddAsync(item);
    }

    public async Task UpdateAsync(T item)
    {
        _context.Set<T>().Update(item);
    }

    public async Task DeleteAsync(int id)
    {
        var searchObj = await _context.Set<T>()
            .FindAsync(id);
        _context.Set<T>().Remove(searchObj);
    }
}
using myfilms.Context;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class UnityOfWork : IUnityOfWork
{
    private IFilmeRepository _filmeRepo;
    public AppDbContext _context;

    public UnityOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IFilmeRepository FilmeRepository
    {
        get { return _filmeRepo ?? new FilmeRepository(_context); }
    }

    public async void CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
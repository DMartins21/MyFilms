using myfilms.Context;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class UnityOfWork : IUnityOfWork
{
    private IFilmeRepository _filmeRepo;
    private IClientRepository _clientRepo;
    public AppDbContext _context;

    public UnityOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IFilmeRepository FilmeRepository
    {
        get { return _filmeRepo ?? new FilmeRepository(_context); }
    }
    
    public IClientRepository ClientRepository
    {
        get { return _clientRepo ?? new ClientRepository(_context); }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
namespace myfilms.Repositories.Interfaces;

public interface IUnityOfWork
{
    public IFilmeRepository  FilmeRepository { get; }
    public IClientRepository ClientRepository { get; }
    Task CommitAsync();
}
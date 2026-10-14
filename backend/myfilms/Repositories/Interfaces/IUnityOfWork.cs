namespace myfilms.Repositories.Interfaces;

public interface IUnityOfWork
{
    public IFilmeRepository  FilmeRepository { get; }
    void CommitAsync();
}
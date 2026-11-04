namespace myfilms.Repositories.Interfaces;

public interface IUnityOfWork
{
    public IFilmeRepository  FilmeRepository { get; }
    public IClientRepository ClientRepository { get; }
    public IBlogPostRepository BlogPostRepository { get; }
    Task CommitAsync();
}
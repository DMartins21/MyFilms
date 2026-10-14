namespace myfilms.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAsync();
    Task CreateAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteAsync(int id);
}
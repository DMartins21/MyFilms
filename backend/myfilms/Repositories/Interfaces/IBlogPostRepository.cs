using myfilms.Models;

namespace myfilms.Repositories.Interfaces;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    public Task<BlogPost> FindByIdAsync(int id);
}
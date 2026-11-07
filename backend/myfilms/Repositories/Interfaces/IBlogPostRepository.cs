using myfilms.DTOs;
using myfilms.Models;

namespace myfilms.Repositories.Interfaces;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    public Task<IEnumerable<BlogPostDTO>> GetAllPostsAsync();
    public Task<BlogPost> FindPostId(int idPost);
    public Task<BlogPost> FindByIdAsync(int id);
}
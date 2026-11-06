using Microsoft.EntityFrameworkCore;
using myfilms.Context;
using myfilms.Models;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(AppDbContext context) : base(context){}
    public async Task<BlogPost> FindByIdAsync(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        
        if (post == null)
            return null;
        
        return post;
    }
}
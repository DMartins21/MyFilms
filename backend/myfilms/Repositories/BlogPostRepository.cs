using Microsoft.EntityFrameworkCore;
using myfilms.Context;
using myfilms.DTOs;
using myfilms.DTOs.Mappings;
using myfilms.Models;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(AppDbContext context) : base(context){}

    public async Task<IEnumerable<BlogPostDTO>> GetAllPostsAsync()
    {
        var posts = await _context.BlogPosts
            .Select(p => new BlogPostDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                TypeContent = p.TypeContent,
                PublishedOn = p.PublishedOn,
                UpdatedOn = p.UpdatedOn,
                AuthorId = p.AuthorId,
            })
            .AsNoTracking()
            .ToListAsync();
        return posts;

    }
    
    public async Task<BlogPost> FindByIdAsync(int id)
    {
        var post = await _context.BlogPosts
            .Select(p => new BlogPostDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                TypeContent = p.TypeContent,
                PublishedOn = p.PublishedOn,
                UpdatedOn = p.UpdatedOn,
                AuthorId = p.AuthorId,
                Author = new ClientDTO()
                {
                    Name = p.Author.Name,
                    LastName = p.Author.LastName,
                    ProfilePictureUrl = p.Author.ProfilePictureUrl,
                    BirthDate = p.Author.BirthDate,
                }
            })
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        
        if (post == null)
            return null;
        
        return post.ToBlogPost();
    }

    public async Task<BlogPost> FindPostId(int idPost)
    {
        return await _context.BlogPosts.FindAsync(idPost);
    }
}
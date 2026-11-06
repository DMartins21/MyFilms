using myfilms.Models;

namespace myfilms.DTOs.Mappings;

public static class BlogPostDTOMappingExtensions
{
    public static BlogPostDTO? ToBlogPostDTO(this BlogPost post)
    {
        if(post is null)
            return null;
		
        var blogPostDto = new BlogPostDTO()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            TypeContent = post.TypeContent,
            PublishedOn = post.PublishedOn,
            AuthorId = post.AuthorId,
            Author = post.Author.ToClientDTO(),
        };
        
        return blogPostDto;
    }
    
    public static BlogPost? ToBlogPost(this BlogPostDTO postDto)
    {
        if(postDto is null)
            return null;
		
        var post = new BlogPost()
        {
            Id = postDto.Id,
            Title = postDto.Title,
            Content = postDto.Content,
            TypeContent = postDto.TypeContent,
            PublishedOn = postDto.PublishedOn,
            AuthorId = postDto.AuthorId,
            Author = postDto.Author.ToClient(),
        };
        
        return post;
    }
    
    public static IEnumerable<BlogPostDTO> ToPostDTOList(this IEnumerable<BlogPost> posts)
    {
        if (posts is null || !posts.Any())
            return new List<BlogPostDTO>();

        return posts.Select(post => new BlogPostDTO()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            TypeContent = post.TypeContent,
            PublishedOn = post.PublishedOn,
            AuthorId = post.AuthorId,
        }).ToList();

    }
}
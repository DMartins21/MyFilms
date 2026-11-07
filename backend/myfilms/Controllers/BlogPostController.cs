using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using myfilms.DTOs;
using myfilms.DTOs.Mappings;
using myfilms.Models;
using myfilms.Repositories.Interfaces;

namespace myfilms.Controllers;

[ApiController]
public class BlogPostController : ControllerBase
{
    private  IUnityOfWork _ofWork;
    private readonly IMemoryCache _cache;

    public BlogPostController(IUnityOfWork ofWork, IMemoryCache cache)
    {
        _ofWork = ofWork;
        _cache = cache;
    }

    private async Task<IEnumerable<BlogPostDTO>> PostsInMemory()
    {
        var posts = await _ofWork.BlogPostRepository.GetAllPostsAsync();
        
        if (posts is null || !posts.Any())
            return new List<BlogPostDTO>();
        
        var cacheOptions = new MemoryCacheEntryOptions
        {
            Size = 1,
            SlidingExpiration = TimeSpan.FromMinutes(3),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        _cache.Set("PostsInMemory", posts, cacheOptions);
        return posts;
    }

    private async Task<IEnumerable<BlogPostDTO>> GetPostsInMemory()
    {
        if (!_cache.TryGetValue("PostsInMemory", out IEnumerable<BlogPostDTO> posts))
        {
            posts = await PostsInMemory();
        }
        return posts;
    }
    
    [HttpGet,
     Route("[controller]/[action]")]
    public async Task<IActionResult> BlogPosts()
    {
        var posts = await GetPostsInMemory();
        if (posts is null || !posts.Any())
            return NoContent();
        // var postsDto = posts.ToPostDTOList();
        return Ok(posts);
    }

    [HttpGet,
     Route("[controller]/[action]/{idPost}")]
    public async Task<ActionResult> Post(int idPost)
    {
        var post = await _ofWork.BlogPostRepository.FindByIdAsync(idPost);
        if (post == null)
            return NotFound();
       
        var postDto = post.ToBlogPostDTO();

        return Ok(postDto);
    }
    
    [HttpPost,
     Authorize,
     Route("[controller]/[action]")]
    public async Task<ActionResult<BlogPostDTO>> NewPost([FromBody] BlogPostDTO postDto)
    {
        var author = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var infos = await _ofWork.ClientRepository.MyUserAsync(author);

        postDto.AuthorId = infos.Id;
        
        if (infos is null || infos.Id != postDto.AuthorId)
            return Forbid();
        
        postDto.PublishedOn = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        var post = postDto.ToBlogPost();
        
        await _ofWork.BlogPostRepository.CreateAsync(post);

        await _ofWork.ClientRepository.NewPostAsync(infos.UserId, post);
        
        await _ofWork.CommitAsync();
        
        _cache.Remove("PostsInMemory");
        
        return Created();
    }

    [HttpPut,
     Authorize,
     Route("[controller]/[action]/{idPost}")]
    public async Task<ActionResult<BlogPostDTO>> UpdatePost(int idPost, [FromBody] BlogPostDTO postDto)
    {
        var author = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(author is null)
            return Unauthorized();
        
        var infos = await _ofWork.ClientRepository.MyUserAsync(author);
        
        var post = await _ofWork.BlogPostRepository.FindPostId(idPost);
        
        if(post == null && post.AuthorId != infos.Id)
            return NotFound();
        
        post.Title = postDto.Title;
        post.Content = postDto.Content;
        post.TypeContent  = postDto.TypeContent;
        post.UpdatedOn = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        
        await _ofWork.BlogPostRepository.UpdateAsync(post);
        await  _ofWork.CommitAsync();
        
        _cache.Remove("PostsInMemory");
        
        return NoContent();

    }


    [HttpDelete,
     Authorize,
     Route("[controller]/[action]/{idPost}")]
    public async Task<ActionResult> DeletePost(int idPost)
    {
        var author = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (author == null)
            return Unauthorized();
        
        var authorId = await _ofWork.ClientRepository.MyUserAsync(author);
        
        var post = await _ofWork.BlogPostRepository.FindByIdAsync(idPost);
        if (post == null && post.AuthorId != authorId.Id)
            return NotFound();

        await _ofWork.BlogPostRepository.DeleteAsync(post.Id);
        await _ofWork.CommitAsync();
        
        _cache.Remove("PostsInMemory");
        
        return NoContent();
    }
    
}
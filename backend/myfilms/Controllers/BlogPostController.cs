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


    [HttpGet,
     Route("[controller]/[action]")]
    public async Task<ActionResult> BlogPosts()
    {
        var posts = await _ofWork.BlogPostRepository.GetAsync();
        if (posts is null || !posts.Any())
            return NoContent();
        var postsDto = posts.ToPostDTOList();
        return Ok(postsDto);
    }

    [HttpGet,
     Route("[controller]/[action]/{idPost}")]
    public async Task<ActionResult> Post(int idPost)
    {
        var post = await _ofWork.BlogPostRepository.FindByIdAsync(idPost);
        if (post == null)
            return NotFound();
        return Ok(post);
    }
    
    [HttpPost,
     Authorize,
     Route("[controller]/[action]")]
    public async Task<ActionResult<BlogPostDTO>> NewPost([FromBody] BlogPostDTO postDto)
    {
        var author = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var infos = _ofWork.ClientRepository.MyUserAsync(author).Result;
        
        if (infos is null || infos.Id != postDto.AuthorId)
            return Forbid();
        
        postDto.PublishedOn = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        var post = postDto.ToBlogPost();
        
        await _ofWork.BlogPostRepository.CreateAsync(post);

        await _ofWork.ClientRepository.NewPostAsync(infos.UserId, post);
        
        await _ofWork.CommitAsync();
        
        return Created();
    }

    [HttpPut,
     Authorize,
     Route("[controller]/[action]")]
    public async Task<ActionResult<BlogPostDTO>> UpdatePost(int idPost, [FromBody] BlogPostDTO postDto)
    {
        var author = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if(author is null)
            return Unauthorized();
        
        var infos = _ofWork.ClientRepository.MyUserAsync(author).Result;
        
        var post = await _ofWork.BlogPostRepository.FindByIdAsync(idPost);
        
        if(post == null && post.AuthorId != infos.Id)
            return NotFound();
        
        post.Title = postDto.Title;
        post.Content = postDto.Content;
        post.TypeContent  = postDto.TypeContent;
        
        await _ofWork.BlogPostRepository.UpdateAsync(post);
        await  _ofWork.CommitAsync();
        return NoContent();

    }


    [HttpDelete,
     Authorize,
     Route("[controller]/[action]")]
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
        return NoContent();
    }
    
}
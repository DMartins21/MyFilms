using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myfilms.Context;
using myfilms.DTOs;
using myfilms.DTOs.Mappings;
using myfilms.Models;

namespace myfilms.Controllers;

[ApiController,
Route("[controller]/[action]"),
Authorize]

public class ClientController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    
    public ClientController(AppDbContext context,  UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult> MyUser()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (user == null)
            return Unauthorized();

        var dbClient = await _context.Clients
            .FirstOrDefaultAsync(c => c.UserId == user);
        
        var clientDto = dbClient.ToClientDTO();
        
        return Ok(clientDto);
    }

    [HttpPost]
    public async Task<IActionResult> NewClient(ClientDTO client)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId is null)
            return Unauthorized();

        var exist = await _context.Clients
            .AnyAsync(c => c.UserId == userId);
        
        if(exist)
            return Conflict(new 
                {message = "user already exists",
                    Status = StatusCodes.Status409Conflict });

        var newClient = client.ToClient();
        newClient.UserId = userId;
        
        await _context.Clients.AddAsync(newClient);
        await _context.SaveChangesAsync();
        
        return Created();

    }

    [HttpPut]
    public async Task<IActionResult> ModifiedClient(ClientDTO clientDto)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return  Unauthorized();
        
        var user = await _context.Clients
            .FindAsync(userId);

        if (user == null)
            return NotFound();

        user.Name = clientDto.Name;
        user.LastName = clientDto.LastName;
        user.ProfilePictureUrl = clientDto.ProfilePictureUrl;
        user.BirthDate = clientDto.BirthDate;
        
        await _context.SaveChangesAsync();

        var client = user.ToClientDTO();
        
        return Ok($"The Client {client.Name} has been modified succesfully");
    }

    [HttpPost]
    public async Task<IActionResult> FavoritarFilme(int idFilme)
    { 
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (user == null) 
            return Unauthorized();
            
        var client = await _context.Clients
            .Include(c => c.FavoriteFilmes)
            .FirstOrDefaultAsync(c => c.UserId == user);
            

        var filme = await _context.Filmes.FindAsync(idFilme);

        if (filme == null) 
            return NotFound();
            
        if (client.FavoriteFilmes.Any(f=> f.Id == idFilme)) 
            return Conflict(new 
            {message = "Filme exist in favorite list", 
                Status = StatusCodes.Status409Conflict});
        
        client.FavoriteFilmes.Add(filme);
            
        await _context.SaveChangesAsync();

        return Ok($"The Film {filme.Title} has been added to favorite list");
    }

    [HttpGet]
    public async Task<IActionResult> MyFavorites()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (user == null)
            return Unauthorized();

        var favs = await _context.Clients
            .Include(c => c.FavoriteFilmes)
            .Where(c => c.UserId == user)
            .FirstOrDefaultAsync();

        var filmes = favs.FavoriteFilmes.ToFilmeDTOList();
        
        return Ok(filmes);
    }
}
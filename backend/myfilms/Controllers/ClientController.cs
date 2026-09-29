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
    public async Task<ActionResult> MyUser(string username)
    {
        var userExist = await _context.Clients.Where(c => c.User.UserName == username).FirstOrDefaultAsync();
        if (userExist == null)
            return NotFound();
        
        var clientDto = userExist.ToClientDTO();
        
        return Ok(clientDto);
    }

    [HttpPost]
    public async Task<IActionResult> NewClient(ClientDTO client, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return NotFound();
        
        var exist = await _context.Clients.Where(c => c.UserId == user.Id).FirstOrDefaultAsync();
        
        if(exist != null)
            return Conflict(new {message = "user already exists", Status = StatusCodes.Status409Conflict});

        var newClient = client.ToClient();
        newClient.UserId = user.Id;
        
        // var newClient = new Client()
        // {
        //     Name = client.Name,
        //     LastName = client.LastName,
        //     ProfilePictureUrl = client.ProfilePictureUrl,
        //     BirthDate = client.BirthDate,
        //     UserId = user.Id
        // };
        
        await _context.Clients.AddAsync(newClient);
        await _context.SaveChangesAsync();
        
        return Created();

    }

    [HttpPut]
    public async Task<IActionResult> ModifiedClient(ClientDTO clientDto, string name)
    {
        var clientExist = await _context.Clients.Where(c => c.Name  == name).FirstOrDefaultAsync();

        if (clientExist == null)
            return NotFound();

        clientExist.Name = clientDto.Name;
        clientExist.LastName = clientDto.LastName;
        clientExist.ProfilePictureUrl = clientDto.ProfilePictureUrl;
        clientExist.BirthDate = clientExist.BirthDate;
        
        await _context.SaveChangesAsync();

        var client = clientExist.ToClientDTO();
        
        return Ok($"The Client {name} has been modified succesfully to {client.Name}");
    }

    [HttpPost]
    public async Task<IActionResult> FavoritarFilme(int idClien, int idFilme)
    {
        try
        {
            var userExist = await _context.Clients
                .Include(c => c.FavoriteFilmes)
                .FirstOrDefaultAsync(c => c.Id == idClien);

            if (userExist == null)
                return NotFound();

            var filme = await _context.Filmes.FindAsync(idFilme);

            if (filme == null)
                return NotFound();

            var isDuplicated = await _context.Clients.AnyAsync(c=> c.Id == idClien && c.FavoriteFilmes.Any(f => f.Id == idFilme));

            if (isDuplicated)
                return Conflict(new {message = "Filme exist in favorite list", Status = StatusCodes.Status409Conflict});
            
            userExist.FavoriteFilmes.Add(filme);
            
            await _context.SaveChangesAsync();

            return Ok();
        }catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> MyFavorites(int id)
    {
        var user = await _context.Clients.FindAsync(id);
        if (user == null)
            return NotFound();
        var favs = await _context.Clients.Include(c => c.FavoriteFilmes).Where(c => c.Id == id).ToListAsync();
        
        return Ok(favs);
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myfilms.Context;
using myfilms.DTOs;
using myfilms.DTOs.Mappings;
using myfilms.Models;
using myfilms.Repositories.Interfaces;


namespace myfilms.Controllers;

[ApiController,
Route("[controller]/[action]"),
Authorize]

public class ClientController : ControllerBase
{
    private readonly IUnityOfWork _ofWork;
    
    public ClientController(IUnityOfWork ofWork)
    {
        _ofWork = ofWork;
    }

    [HttpGet]
    public async Task<ActionResult> MyUser()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (user == null)
            return Unauthorized();

        var searchProfile = await _ofWork.ClientRepository.MyProfileAsync(user);

        if (searchProfile == null)
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Profile not found" });
        
        return Ok(searchProfile);
    }

    [HttpPost]
    public async Task<IActionResult> NewClient(ClientDTO client)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId is null)
            return Unauthorized();

        var exist = await _ofWork.ClientRepository.MyUserAsync(userId);
        
        if(exist != null)
            return Conflict(new 
                {
                    message = "Profile already exists",
                    Status = StatusCodes.Status409Conflict 
                });

        var newClient = client.ToClient();
        newClient.UserId = userId;

        _ofWork.ClientRepository.CreateAsync(newClient);
        await _ofWork.CommitAsync();
        
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> ModifiedClient(ClientDTO clientDto)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return  Unauthorized();
        
        var user = await _ofWork.ClientRepository.MyUserAsync(userId);
        if (user == null)
            return NotFound(
                new
                {
                    message = "Profile Not Found, Please create a Profile",
                    StatusCode =  StatusCodes.Status404NotFound
                }
                );
  
        user.Name = clientDto.Name;
        user.LastName = clientDto.LastName;
        user.ProfilePictureUrl = clientDto.ProfilePictureUrl;
        user.BirthDate = clientDto.BirthDate;
        
        await _ofWork.ClientRepository.UpdateAsync(user);
        await _ofWork.CommitAsync();

        var client = user.ToClientDTO();
        
        return Ok(new
        {
            message = $"The Client {client.Name} has been modified succesfully"
        });
    }

    [HttpPatch]
    public async Task<IActionResult> ModfiedClient([FromBody] JsonPatchDocument<ClientDTO> pathDoc)
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (user == null)
            return Unauthorized();
        
        var profile = await _ofWork.ClientRepository.MyUserAsync(user);
        
        if (profile == null)
            return NotFound(
                new
                {
                    message = "Profile Not Found, Please create a Profile",
                    status = StatusCodes.Status404NotFound
                });

        var client = await _ofWork.ClientRepository.MyUserAsync(user);
        
        var clientUpdate = client.ToClientDTO();

        if (pathDoc != null && pathDoc.Operations != null)
            pathDoc.ApplyTo(clientUpdate, ModelState);
        
        if (!ModelState.IsValid || !TryValidateModel(clientUpdate))
            return ValidationProblem(ModelState);
        
        client.Name = clientUpdate.Name;
        client.ProfilePictureUrl = clientUpdate.ProfilePictureUrl;
        client.BirthDate = clientUpdate.BirthDate;
        client.LastName = clientUpdate.LastName;
        
        await _ofWork.CommitAsync();
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> FavoritarFilme(int idFilme)
    { 
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (user == null) 
            return Unauthorized();

        var filme = await _ofWork.FilmeRepository.GetByFilmIdAsync(idFilme);

        if (filme == null) 
            return NotFound();
        
        var add = await _ofWork.ClientRepository.FavoriteAsync(user, filme);
        
        if (!add) 
            return Conflict(new 
            {
                message = "Filme exist in favorite list", 
                Status = StatusCodes.Status409Conflict
            });
        
        await _ofWork.CommitAsync();

        return Ok(
            new
            {
                message = $"The Film {filme.Title} has been added to favorite list"
            });
    }

    [HttpGet]
    public async Task<IActionResult> MyFavorites()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (user == null)
            return Unauthorized();
        
        var clientFavs = await _ofWork.ClientRepository.GetFavsAsync(user);        

        if (clientFavs == null)
            return NoContent();
        
        var favs = clientFavs.FavoriteFilmes.ToFilmeDTOList();
        
        return Ok(favs);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveFilm(int idFilme)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();
        
        if (!await _ofWork.ClientRepository.UnfavoriteAsync(userId, idFilme))
            return NotFound(
                new
                {
                    message = "Your List is Empty or The Film not exist in favorite list",
                    Status = StatusCodes.Status404NotFound
                });
        
        await _ofWork.CommitAsync();
        
        return NoContent();
    }
}
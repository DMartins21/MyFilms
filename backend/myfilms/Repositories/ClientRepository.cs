using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using myfilms.Context;
using myfilms.DTOs;
using myfilms.DTOs.Mappings;
using myfilms.Models;
using myfilms.Repositories.Interfaces;

namespace myfilms.Repositories;

public class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(AppDbContext context) : base(context){}
    

    public async Task<Client?> MyUserAsync(string idUser)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(
                c => c.UserId == idUser
                );
    }

    public async Task<ClientDTO?> MyProfileAsync(string idUser)
    {
        return await _context.Clients
            .AsNoTracking()
            .Where(c => c.UserId == idUser)
            .Select(c => new ClientDTO
            {
                Name = c.Name,
                LastName = c.LastName,
                BirthDate = c.BirthDate,
                ProfilePictureUrl = c.ProfilePictureUrl,
            }).FirstOrDefaultAsync();
    }
    
    public async Task<Client> GetFavsAsync(string userId)
    {
        return await _context.Clients
            .Include(c => c.FavoriteFilmes)
            .Where(c => c.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> FavoriteAsync(string userId, Filme idFilm)
    {
        var clientFilms = await GetFavsAsync(userId);

        if (clientFilms.FavoriteFilmes.Any(f => f.Id == idFilm.Id))
            return false;
        clientFilms.FavoriteFilmes.Add(idFilm);

        return true;
    }
    
    public async Task<bool> UnfavoriteAsync(string userId, int idFilm)
    {

        var clientFilms = await GetFavsAsync(userId);
        if(clientFilms == null)
            return false;
        
        var search = clientFilms.FavoriteFilmes.FirstOrDefault(f => f.Id == idFilm);
        if(search == null)
            return false;
        
        clientFilms.FavoriteFilmes.Remove(search);
        
        return true;
    }
}
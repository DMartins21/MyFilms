using myfilms.DTOs;
using myfilms.Models;

namespace myfilms.Repositories.Interfaces;

public interface IClientRepository : IRepository<Client>
{
    Task<ClientDTO?> MyProfileAsync(string idUser);
    Task<Client?> MyUserAsync(string idUser);
    Task <Client> GetFavsAsync(string userId);
    Task<Client> GetPostsAsync(string userId);
    Task<bool> NewPostAsync(string userId, BlogPost idPost);
    Task<bool> FavoriteAsync(string userId, Filme idFilme);
    Task <bool> UnfavoriteAsync(string userId, int id);
}
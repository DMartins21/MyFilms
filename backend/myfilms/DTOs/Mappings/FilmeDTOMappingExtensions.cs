using myfilms.Models;
using myfilms.DTOs;

namespace myfilms.DTOs.Mappings;

public static class FilmeDTOMappingExtensions
{
    public static FilmeDTO? ToFilmeDTO(this Filme filme)
    {
        if(filme is null)
            return null;
		
        var filmeDto = new FilmeDTO()
        {
            Id = filme.Id,
            Title = filme.Title,
            Description = filme.Description,
            Genre = filme.Genre,
            ReleaseDate = filme.ReleaseDate,
            ImageUrl = filme.ImageUrl,
            ThumbnailUrl = filme.ThumbnailUrl
        };
        
        return filmeDto;
    }
    
    public static Filme? ToFilme(this FilmeDTO filmeDto)
    {
        if(filmeDto is null)
            return null;
		
        var filme = new Filme()
        {
            Id = filmeDto.Id,
            Title = filmeDto.Title,
            Description = filmeDto.Description,
            Genre = filmeDto.Genre,
            ReleaseDate = filmeDto.ReleaseDate,
            ImageUrl = filmeDto.ImageUrl,
            ThumbnailUrl = filmeDto.ThumbnailUrl
        };
        
        return filme;
    }

    public static IEnumerable<FilmeDTO> ToFilmeDTOList(this IEnumerable<Filme> filmes)
    {
        if (filmes is null || !filmes.Any())
            return new List<FilmeDTO>();

        return filmes.Select(filme => new FilmeDTO()
        {
            Id = filme.Id,
            Title = filme.Title,
            Description = filme.Description,
            Genre = filme.Genre,
            ReleaseDate = filme.ReleaseDate,
            ImageUrl = filme.ImageUrl,
            ThumbnailUrl = filme.ThumbnailUrl
        }).ToList();

    }
    
    public static IEnumerable<Filme> ToFilmeList(this IEnumerable<FilmeDTO> filmes)
    {
        if (filmes is null || !filmes.Any())
            return new List<Filme>();

        return filmes.Select(filme => new Filme()
        {
            Id = filme.Id,
            Title = filme.Title,
            Description = filme.Description,
            Genre = filme.Genre,
            ReleaseDate = filme.ReleaseDate,
            ImageUrl = filme.ImageUrl,
            ThumbnailUrl = filme.ThumbnailUrl
        }).ToList();

    }
}
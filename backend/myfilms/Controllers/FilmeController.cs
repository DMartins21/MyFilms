using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using myfilms.DTOs;
using myfilms.Models;
using myfilms.Context;
using myfilms.DTOs.Mappings;
using myfilms.Pagination;
using myfilms.Repositories.Interfaces;
using Newtonsoft.Json;


namespace myfilms.Controllers;

[EnableRateLimiting("api")]
[ApiController]
[Route("api/[controller]")]
public class FilmeController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private IUnityOfWork _ofWork;

    public FilmeController(IUnityOfWork uof, IMemoryCache cache)
    {
        _ofWork = uof;
        _cache = cache;
    }

    private async Task<List<Filme>> GetFilmesInDB()
    {
        var filmes = await _ofWork.FilmeRepository.GetAsync();
        
        _cache.Set("cache_Films", filmes, TimeSpan.FromMinutes(30));
        
        return filmes.ToList();
    }
    
    private async Task<IEnumerable<Filme>> FilmesEmCache()
    {
        if (!_cache.TryGetValue("cache_Films", out List<Filme>? filmes))
            filmes = await GetFilmesInDB();
        return filmes;
    }

    private ActionResult<IEnumerable<FilmeDTO>> FilmesDeletados()
    {
        if (_cache.TryGetValue("cache_FilmsDeleted", out List<FilmeDTO>? removedFilms))
        {
            return Ok(removedFilms);
        }

        return NoContent();
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FilmeDTO>>> GetFilme()
    {
        var filmes = await FilmesEmCache();
        
        if(filmes == null)
            return NoContent();
        
        var filmesDTO = filmes.ToFilmeDTOList();
        
        return Ok(filmesDTO);
    }

    [HttpGet("Pagination")]
    public async Task<ActionResult<FilmeDTO>> GetFilms([FromQuery] FilmsParams filmsParams)
    {
        var filmes = await _ofWork.FilmeRepository.GetFilms(filmsParams);

        var metadata = new
        {
            filmes.TotalCount,
            filmes.PageSize,
            filmes.CurrentPage,
            filmes.TotalPages,
            filmes.HasNextPage,
            filmes.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        
        var  filmesDTO = filmes.ToFilmeDTOList();
        return Ok(filmesDTO);
    }

    [HttpGet("FilmesRemovidos"), Authorize(Policy =  "AdminOnly")]
    public  ActionResult<IEnumerable<FilmeDTO>> GetFilmesRemovidos()
    {
        return FilmesDeletados();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Filme>> GetFilmeById(int id)
    {
        var search =  await FilmesEmCache();
        var filme = search.FirstOrDefault(f => f.Id == id);
        if(filme == null) 
            return NotFound();
        
        var filmeDTO = filme.ToFilmeDTO();
        
        return Ok(filmeDTO);
    }

    [HttpGet("titulo/{title}")]
    public async Task<ActionResult<IEnumerable<FilmeDTO>>> GetFilmeByTitle(string title)
    {
        var search = await _ofWork.FilmeRepository.GetByTitleAsync(title);
        
        if(!search.Any())
            return NoContent();

        var filmeDTO = search.ToFilmeDTOList();
        
        return Ok(filmeDTO);
    }

    [HttpPost("CreateFilm"),
     Route("/Create"), 
     Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FilmeDTO>> CreateFilme([FromBody]FilmeDTO filmeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var filme = filmeDto.ToFilme();

        await _ofWork.FilmeRepository.CreateAsync(filme);
        await _ofWork.CommitAsync();
        
        _cache.Remove("cache_Films");

        var created = filme.ToFilmeDTO();

        return Created($"api/Filme/{created.Id}", created);
    }

    
    [HttpPost("CreateFilms"),Route("/CreateFilms"), Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> PostInRange(List<FilmeDTO> filmesDTO)
    {
        var title = filmesDTO
            .Select(f => f.Title.ToLower())
            .ToList();
        
        var exist = await _ofWork.FilmeRepository
            .SearchAsync(f => title.Contains(f.Title.ToLower()));
        
        if (exist.Any())
            return Conflict(
                new
                {
                    message = $"The Film exists in Blog",
                    status = StatusCodes.Status409Conflict
                });
        
        var filmes = filmesDTO.ToFilmeList();

        await _ofWork.FilmeRepository.CreateRangeAsync(filmes);
        await _ofWork.CommitAsync();
        
        return Created();
    }

    
    [HttpPost("Restaurar/{titulo}"),Route("/RestaurarFilme"), Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FilmeDTO>> RestaurarFilme(string titulo, int? idFilme)
    {
        if (!_cache.TryGetValue("cache_FilmsDeleted", out List<FilmeDTO>? filmesRemoved) ||
            filmesRemoved == null || !filmesRemoved.Any())
            return NotFound("Não há filmes deletados na memória");
        
        if (filmesRemoved.Where(f => f.Title == titulo).Count() > 1)
        {
            if(idFilme == null)
                return Conflict(new
                {
                    mensagem = $"Existe mais de um Filme com o Titulo {titulo}",
                    options = filmesRemoved.Select(f => new { f.Id, f.Title })
                });

            var req = filmesRemoved.FirstOrDefault(f => f.Id == idFilme);

            if (req == null)
                return NotFound();

            var backFilme = req.ToFilme();
            
            backFilme.Id = 0;
            await _ofWork.FilmeRepository.CreateAsync(backFilme);
            await _ofWork.CommitAsync();
            
            _cache.Remove("cache_Films");
            
            filmesRemoved.Remove(req);
            _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
            
            return NoContent();
        }
        
        var filmeDTO = filmesRemoved.FirstOrDefault(f => f.Title == titulo);
        
        if (filmeDTO == null)
            return NotFound();
        
        var filme = filmeDTO.ToFilme();
        
        filme.Id = 0;
        await _ofWork.FilmeRepository.CreateAsync(filme);
        await _ofWork.CommitAsync();
        
        _cache.Remove("cache_Films");
        
        filmesRemoved.Remove(filmeDTO);
        _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
        
        return NoContent();
    }
    
    [HttpPut("Alterar/{id}"), Route("/AlterarTitulo") ,Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FilmeDTO>> AlterarDadosFilme(int id, [FromBody] FilmeDTO filmeAtualizado)
    {
        var res = await _ofWork.FilmeRepository.GetByFilmIdAsync(id);
        if (res == null)
            return NotFound("Filme não encontrado");
        
        res.Title = filmeAtualizado.Title;
        res.Description = filmeAtualizado.Description;
        res.ReleaseDate = filmeAtualizado.ReleaseDate;
        res.Genre = filmeAtualizado.Genre;
        res.ImageUrl = filmeAtualizado.ImageUrl;
        res.ThumbnailUrl = filmeAtualizado.ThumbnailUrl;

        _ofWork.FilmeRepository.UpdateAsync(res);
        await _ofWork.CommitAsync();

        _cache.Remove("cache_Films");
        return NoContent();
    }
    
    [HttpDelete("{id:int}"),Route("/DeleteFilm") ,Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FilmeDTO>> DeletarFilme(int id)
    {
        var res = await _ofWork.FilmeRepository.GetByFilmIdAsync(id);
        if (res == null)
            return NotFound();

        var filmeDTO = res.ToFilmeDTO();
        
        if(!_cache.TryGetValue("cache_FilmsDeleted", out List<FilmeDTO>? filmesRemoved))
            filmesRemoved = new List<FilmeDTO>();
        filmesRemoved!.Add(filmeDTO);
        _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));

        await _ofWork.FilmeRepository.DeleteAsync(id);
        await _ofWork.CommitAsync();
        
        _cache.Remove("cache_Films");
        return Ok($"Filme {filmeDTO.Title} Deletado com sucesso");
    }
}
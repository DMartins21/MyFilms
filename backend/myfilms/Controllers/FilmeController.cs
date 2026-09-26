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


namespace myfilms.Controllers;

[EnableRateLimiting("api")]
[ApiController]
[Route("api/[controller]")]
public class FilmeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    

    public FilmeController(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    private async Task<List<Filme>> GetFilmesInDB()
    {
        var filmes = await _context.Filmes.ToListAsync();
        
        _cache.Set("cache_Films", filmes, TimeSpan.FromMinutes(30));
        
        return filmes;
    }
    
    private async Task<IEnumerable<Filme>> FilmesEmCache()
    {
        if (!_cache.TryGetValue("cache_Films", out List<Filme>? filmes))
            filmes = await GetFilmesInDB();
        return filmes;
    }

    private ActionResult<IEnumerable<Filme>> FilmesDeletados()
    {
        if (_cache.TryGetValue("cache_FilmsDeleted", out List<Filme>? removedFilms))
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

    [HttpGet("FilmesRemovidos"), Authorize(Policy =  "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Filme>>> GetFilmesRemovidos()
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
        var filmes = await FilmesEmCache();
        
        var search = filmes.Where(f => f.Title.ToLower().Contains(title.ToLower()))
            .ToList().OrderBy(f => f.Id);
        
        if(!search.Any())
            return NoContent();

        var filmeDTO = search.ToFilmeDTOList();
        
        return Ok(filmeDTO);
    }

    [HttpPost, Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FilmeDTO>> PostFilme([FromBody]FilmeDTO filmeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var filme = filmeDto.ToFilme();
        
        await _context.Filmes.AddAsync(filme);
        
        await _context.SaveChangesAsync();
        
        _cache.Remove("cache_Films");
        
        return CreatedAtAction("GetFilme", new { id = filmeDto.Id }, filmeDto);
    }

    
    [HttpPost("postRange"), Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> PostInRange(List<FilmeDTO> filmesDTO)
    {
        var verify = await _context.Filmes.Select(f => f.Title).ToListAsync();

        var exist = await _context.Filmes.Where(f 
            => verify.Contains(f.Title))
            .Select(f => f.Title)
            .ToListAsync();

        if (exist.Any())
            return Conflict(
                new
                {
                    message = $"The Film exists in Blog",
                    status = StatusCodes.Status409Conflict
                });
        
        var filmes = filmesDTO.ToFilmeList();
        
        await _context.Filmes.AddRangeAsync(filmes);
        
        await _context.SaveChangesAsync();
        
        return Ok($"new films created with success!");
    }

    
    [HttpPost("restaurar/{titulo}"), Authorize(Policy = "AdminOnly")]
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
            
            req.Id = 0;
            _context.Filmes.Add(backFilme);
            await _context.SaveChangesAsync();
            
            _cache.Remove("cache_Films");
            
            filmesRemoved.Remove(req);
            _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
            
            return Ok($"The Film {req.Title} has been added to Blog");
        }
        
        var filmeDTO = filmesRemoved.FirstOrDefault(f => f.Title == titulo);
        
        if (filmeDTO == null)
            return NotFound();
        
        var filme = filmeDTO.ToFilme();
        
        filme.Id = 0;
        await _context.Filmes.AddAsync(filme);
        await _context.SaveChangesAsync();
        
        _cache.Remove("cache_Films");
        
        filmesRemoved.Remove(filmeDTO);
        _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
        
        return Ok($"The Film {filme.Title} has been added to Blog");
    }
    
    [HttpPut("alterarTitulo/{id}"), Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FilmeDTO>> AlterarDadosFilme(int id, [FromBody] FilmeDTO filmeAtualizado)
    {
        var res = await _context.Filmes.FindAsync(id);
        if (res == null)
            return NotFound("Filme não encontrado");
        
        res.Title = filmeAtualizado.Title;
        res.Description = filmeAtualizado.Description;
        res.ReleaseDate = filmeAtualizado.ReleaseDate;
        res.Genre = filmeAtualizado.Genre;
        res.ImageUrl = filmeAtualizado.ImageUrl;
        res.ThumbnailUrl = filmeAtualizado.ThumbnailUrl;

        _context.Filmes.Update(res);

        await _context.SaveChangesAsync();
        _cache.Remove("cache_Films");
        return Ok($"The Title {filmeAtualizado.Title} has been updated");
    }
    
    [HttpDelete("{id:int}"), Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<FilmeDTO>> DeletarFilme(int id)
    {
        var res = await _context.Filmes.FindAsync(id);
        if (res == null)
            return NotFound();

        var filmeDTO = res.ToFilmeDTO();
        
        if(!_cache.TryGetValue("cache_FilmsDeleted", out List<FilmeDTO>? filmesRemoved))
            filmesRemoved = new List<FilmeDTO>();
        filmesRemoved!.Add(filmeDTO);
        _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
        
        _context.Filmes.Remove(res);
        await _context.SaveChangesAsync();
        _cache.Remove("cache_Films");
        return Ok($"Filme {filmeDTO.Title} Deletado com sucesso");
    }
}
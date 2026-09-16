using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using myfilms.Models;
using myfilms.Context;


namespace myfilms.Controllers;
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
    public async Task<ActionResult<IEnumerable<Filme>>> GetFilme()
    {
        var filmes = await FilmesEmCache();
        
        if(filmes == null)
            return NoContent();
        
        return Ok(filmes);
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
        
        return Ok(filme);
    }

    [HttpGet("titulo/{title}")]
    public async Task<ActionResult<IEnumerable<Filme>>> GetFilmeByTitle(string title)
    {
        var filmes = await FilmesEmCache();
        var search = filmes.Where(f => f.Title.ToLower().Contains(title.ToLower()))
            .ToList().OrderBy(f => f.Id);
        if(!search.Any())
            return NoContent();
        return Ok(search);
    }

    [HttpPost, Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Filme>> PostFilme([FromBody]Filme filme)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _context.Filmes.AddAsync(filme);
        await _context.SaveChangesAsync();
        _cache.Remove("cache_Films");
        return CreatedAtAction("GetFilme", new { id = filme.Id }, filme);
    }

    
    [HttpPost("postRange"), Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> PostInRange(List<Filme> filmes)
    {
        await _context.Filmes.AddRangeAsync(filmes);
        await _context.SaveChangesAsync();
        return  Ok();
    }

    
    [HttpPost("restaurar/{titulo}"), Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Filme>> RestaurarFilme(string titulo, int? idFilme)
    {
        if (!_cache.TryGetValue("cache_FilmsDeleted", out List<Filme>? filmesRemoved) ||
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
            
            req.Id = 0;
            _context.Filmes.Add(req);
            await _context.SaveChangesAsync();
            
            _cache.Remove("cache_Films");
            
            filmesRemoved.Remove(req);
            _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
            
            return CreatedAtAction("GetFilme", new { id = req.Id }, req);
        }
        
        var filme = filmesRemoved.FirstOrDefault(f => f.Title == titulo);
        
        if (filme == null)
            return NotFound();
        
        filme.Id = 0;
        _context.Filmes.Add(filme);
        await _context.SaveChangesAsync();
        
        _cache.Remove("cache_Films");
        
        filmesRemoved.Remove(filme);
        _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
        
        return CreatedAtAction("GetFilme", new { id = filme.Id }, filme);
    }
    
    [HttpPut("alterarTitulo/{id}"), Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Filme>> AlterarDadosFilme(int id, [FromBody] Filme filmeAtualizado)
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
        
        _context.SaveChangesAsync();
        _cache.Remove("cache_Films");
        return Ok(res);
    }
    
    [HttpDelete("{id:int}"), Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<Filme>> DeletarFilme(int id)
    {
        var res = await _context.Filmes.FindAsync(id);
        if (res == null)
            return NotFound();
        
        if(!_cache.TryGetValue("cache_FilmsDeleted", out List<Filme>? filmesRemoved))
            filmesRemoved = new List<Filme>();
        filmesRemoved!.Add(res);
        _cache.Set("cache_FilmsDeleted", filmesRemoved, TimeSpan.FromDays(1));
        
        _context.Filmes.Remove(res);
        await _context.SaveChangesAsync();
        _cache.Remove("cache_Films");
        return Ok($"Filme {res.Title} Deletado com sucesso");
    }
}
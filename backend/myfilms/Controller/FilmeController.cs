using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using myfilms.Models;
using myfilms.Context;


namespace myfilms.Controller;
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

    private async Task<IEnumerable<Filme>> SalvarCache()
    {
        if (!_cache.TryGetValue("cache_Films", out List<Filme>? filmes))
        {
            filmes = await _context.Filmes.ToListAsync();
            _cache.Set("cache_Films", filmes, TimeSpan.FromMinutes(10));
        }
        return filmes.ToList();
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Filme>>> GetFilme()
    {
        var filmes=  await SalvarCache();
        return Ok(filmes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Filme>> GetFilmeById(int id)
    {
        var filme = await SalvarCache();
        var res = filme.Where(fil => fil.Id == id);
        if(filme == null) 
            return NotFound("Filme não localizado");
        return Ok(res);
    }

    [HttpGet("titulo/{title}")]
    public async Task<ActionResult<Filme>> GetFilmeByTitle(string title)
    {
        
        var result = await SalvarCache();
        if(result == null || result == Empty) 
            return NotFound($"Filme {title} não Encontrado");
        
        var filme = result.Where(film => film.Title == title || film.Title.Contains(title)).FirstOrDefault();
        return Ok(filme);
    }

    [HttpPost]
    public async Task<ActionResult<Filme>> PostFilme([FromBody]Filme filme)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _context.Filmes.AddAsync(filme);
        await _context.SaveChangesAsync();
        _cache.Remove("cache_Films");
        return CreatedAtAction("GetFilme", new { id = filme.Id }, filme);
    }

    [HttpPatch("alterarTitulo/{titulo}")]
    public async Task<ActionResult<Filme>> AlterarDadosFIlme(string titulo, [FromBody] Filme filmeAtualizado)
    {
        var res = await _context.Filmes.FirstOrDefaultAsync(
            fil => fil.Title == titulo || fil.Title.Contains(titulo)
            );
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

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Filme>> DeletarFilme(int id)
    {
        var res = await _context.Filmes.FindAsync(id);
        if (res == null)
            return NoContent();
        
        _context.Filmes.ExecuteDeleteAsync();
        _context.SaveChangesAsync();
        _cache.Remove("cache_Films");
        return Ok($"Filme {res.Title} Deletado com sucesso");
    }
}
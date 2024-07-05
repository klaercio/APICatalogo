using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> getCategoriasProdutos()
        {
            return _context.Categorias.Include(p  => p.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get() {

            try
            {
                var categorias = _context.Categorias.AsNoTracking().ToList();
                if (categorias is null)
                    return NotFound();

                return categorias;
            }
            catch
            (Exception erro)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um problema ao tratar a sua solicitação: {erro}");
            }

            
        }


        [HttpGet("{id:int}", Name = "ObterCategoria")]
        //Na rota está chegando como localhost/Categorias/id
        public ActionResult<Categoria> get(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

                if (categoria is null)
                    return NotFound("Categoria não encontrado");

                return Ok(categoria);
            }catch(Exception erro)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpPost]
        public ActionResult post(Categoria categoria) 
        {
            try
            {
                if (categoria is null)
                    BadRequest();

                Debug.WriteLine(categoria);

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }catch(Exception erro)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                    return BadRequest();

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (Exception erro) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema  ao tratar a sua solicitação");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

                if (categoria is null)
                    NotFound("Categoria não encontrada");

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return Ok(categoria);
            }catch(Exception erro)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao tratar a sua solicitação");
            }
        }
    }
}

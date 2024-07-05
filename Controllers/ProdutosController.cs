using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> get()
        {
            try
            {
                var produtos = _context.Produtos.ToList();

                if (produtos is null)
                    return NotFound();

                return produtos;
            }catch(Exception erro)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitãção");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produto> get(int id) {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);

                if (produto is null)
                    return BadRequest();

                return produto;
            }
            catch (Exception erro) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao tratar a sua solicitação");
            }
        }

        [HttpPost]
        public ActionResult post(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest();

                _context.Produtos.Add(produto);
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception erro) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao tratar a sua solicitação");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult put(int id, Produto produto) {
            try
            {
                if (id != produto.ProdutoId)
                    return BadRequest();

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception erro) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult delete(int id) {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);

                if (produto is null)
                    NotFound("Produto não localizado");

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception erro) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
            }
        }
    }
}

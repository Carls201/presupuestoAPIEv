using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaGastoController : ControllerBase
    {
        private readonly PresupuestoContext db;
        public CategoriaGastoController(PresupuestoContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Resp r = new();

            try
            {
                var catGastos = await db.CategoriaGastos.Select(x => new
                {
                    x.id_categoria,
                    x.nombre
                }).ToListAsync();

                if (catGastos.Any())
                {
                    r.Data = catGastos;
                    r.Success = true;
                    r.Message = "Los datos se han mostrado con exito";
                    return Ok(r);
                }
                r.Message = "No se han encotrado datos";
                r.Success = true;
                return Ok(r);
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                return BadRequest(r);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCategoriaGasto(CategoriaGasto categoriaGasto)
        {
            Resp r = new();
            if(categoriaGasto.nombre == "" || categoriaGasto.nombre == null)
            {
                r.Message = "Los campos no pueden quedar vacio";
                return BadRequest(r);
            }

            db.Add(categoriaGasto);
            await db.SaveChangesAsync();
            r.Message = "Se ha guardado con exito";
            r.Success = true;
            r.Data = categoriaGasto.id_categoria;
            return CreatedAtAction("Get", r, categoriaGasto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaGasto(int id)
        {
            Resp r = new();
            var catGasto = await db.CategoriaGastos.FindAsync(id);

            if(catGasto == null)
            {
                r.Message = "La categoria no existe";
                return BadRequest(r);
            }

            db.CategoriaGastos.Remove(catGasto);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Se ha eliminado con exito";
            return Ok(r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoriaGasto(int id, CategoriaGasto catGasto)
        {
            Resp r = new();
            var cat = await db.CategoriaGastos.Select(x => new
            {
                x.id_categoria,
                x.nombre
            }).FirstOrDefaultAsync(x => x.id_categoria == id);

            if(cat == null)
            {
                r.Message = "Categoria no encntrada";
                return BadRequest(r);
            }
            if(id != catGasto.id_categoria)
            {
                r.Message = "El id ingresado no coincide con el id de la categoria que desea modificar";
                return BadRequest(r);
            }

            db.CategoriaGastos.Update(catGasto);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Categoria modificada";
            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaGasto(int id)
        {
            Resp r = new();
            var catGasto = await db.CategoriaGastos.Select(x => new
            {
                x.id_categoria,
                x.nombre
            }).FirstOrDefaultAsync(x => x.id_categoria == id);

            if(catGasto == null)
            {
                r.Message = "Categoria con encontrada";
                return NotFound(r);
            }

            r.Success = true;
            r.Data = catGasto;
            return Ok(r);
        }

    }
}

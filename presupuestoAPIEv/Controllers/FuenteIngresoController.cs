using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuenteIngresoController : ControllerBase
    {
        private readonly PresupuestoContext db;
        public FuenteIngresoController(PresupuestoContext context)
        {
            db = context;
        }
        // GET
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Resp r = new();

            try
            {
                var fuenteIngreso = await db.FuenteIngresos.Select(x => new
                {
                    x.id_fuente,
                    x.nombre
                }).ToListAsync();

                if (fuenteIngreso.Any())
                {
                    r.Data = fuenteIngreso;
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

        // POST
        [HttpPost]
        public async Task<IActionResult> PostFuenteIngreso(FuenteIngreso fuenteIngreso)
        {
            Resp r = new();
            if (fuenteIngreso.nombre == "" || fuenteIngreso.nombre == null)
            {
                r.Message = "Los campos no pueden quedar vacio";
                return BadRequest(r);
            }

            db.Add(fuenteIngreso);
            await db.SaveChangesAsync();
            r.Message = "Se ha guardado con exito";
            r.Success = true;
            r.Data = fuenteIngreso.id_fuente;
            return CreatedAtAction("Get", r, fuenteIngreso);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuenteIngreso(int  id)
        {
            Resp r = new();
            var fIngreso = await db.FuenteIngresos.FindAsync(id);

            if (fIngreso == null)
            {
                r.Message = "La fuente de ingreso no existe";
                return BadRequest(r);
            }

            db.FuenteIngresos.Remove(fIngreso);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Se ha eliminado con exito";
            return Ok(r);
        }

        // EDIT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuenteIngreso(int id, FuenteIngreso fIngreso)
        {
            Resp r = new();
            var fIng = await db.FuenteIngresos.Select(x => new
            {
                x.id_fuente,
                x.nombre
            }).FirstOrDefaultAsync(x => x.id_fuente == id);

            if (fIng == null)
            {
                r.Message = "Fuente de ingreso no encontrada";
                return BadRequest(r);
            }
            if (id != fIngreso.id_fuente)
            {
                r.Message = "El id ingresado no coincide con el id de la fuente de ingreso que desea modificar";
                return BadRequest(r);
            }

            db.FuenteIngresos.Update(fIngreso);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Fuente de Ingreso modificada";
            return Ok(r);
        }

        // GET POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuenteIngreso(int id)
        {
            Resp r = new();
            var fIngreso = await db.FuenteIngresos.Select(x => new
            {
                x.id_fuente,
                x.nombre
            }).FirstOrDefaultAsync(x => x.id_fuente== id);

            if (fIngreso == null)
            {
                r.Message = "Fuente de ingreso no encontrada";
                return NotFound(r);
            }

            r.Success = true;
            r.Data = fIngreso;
            return Ok(r);
        }
    }
}

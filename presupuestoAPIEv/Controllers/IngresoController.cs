using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresoController : ControllerBase
    {
        private readonly PresupuestoContext db;
        public IngresoController(PresupuestoContext context)
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
                var ingreso = await db.Ingresos.Select(x => new
                {
                    x.id_ingreso,
                    x.id_usuario,
                    x.id_fuente,
                    x.monto
                }).ToListAsync();

                if (ingreso.Any())
                {
                    r.Data = ingreso;
                    r.Success = true;
                    r.Message = "Los ingresos se cargaron exitosamente";
                    return Ok(r);
                }

                r.Message = "No se encuentran datos";
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
        public async Task<IActionResult> PostIngreso(Ingreso ingreso)
        {
            Resp r = new();
            if (ingreso.id_usuario <= 0 || ingreso.id_fuente <= 0 || ingreso.monto <= 0
                || ingreso.id_usuario == null || ingreso.id_fuente == null || ingreso.monto == null)
            {
                r.Message = "Completa los campos vacios";
                return BadRequest(r);
            }

            var existeUsuario = await db.Usuarios.FirstOrDefaultAsync(x => x.id_usuario == ingreso.id_usuario);
            var existeFuente = await db.FuenteIngresos.FirstOrDefaultAsync(x => x.id_fuente == ingreso.id_fuente);

            if (existeUsuario == null || existeFuente == null)
            {
                r.Message = "La fuente o el usuario ingresado no existe";
                return BadRequest(r);
            }

            db.Ingresos.Add(ingreso);
            await db.SaveChangesAsync();
            r.Message = "Ingreso guardado";
            r.Success = true;
            r.Data = ingreso.id_ingreso;
            return CreatedAtAction("Get", r, ingreso);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngreso(int id)
        {
            Resp r = new();
            var ingreso = await db.Ingresos.FindAsync(id);

            if (ingreso == null)
            {
                r.Message = "El ingreso que desea eliminar no se encuentra";
                return BadRequest(r);
            }

            db.Ingresos.Remove(ingreso);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Ingreso eliminado";
            return Ok(r);
        }

        // EDIT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngreso(int id, Ingreso ingreso)
        {
            Resp r = new();
            var ingresox = await db.Ingresos.Select(x => new
            {
                x.id_ingreso,
                x.id_fuente,
                x.id_usuario,
                x.monto
            }).FirstOrDefaultAsync(x => x.id_ingreso== ingreso.id_ingreso
            );

            if (ingresox == null)
            {
                r.Message = "El ingreso que desea modificar no se encuentra";
                return BadRequest(r);
            }

            if (id != ingreso.id_ingreso)
            {
                r.Message = "El id ingresado no coincide con el id del ingreso que desea modificar";
                return BadRequest(r);
            }

            db.Ingresos.Update(ingreso);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "El ingreso se ha modificado con exito";
            return Ok(r);
        }

        // GET POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngreso(int id)
        {
            Resp r = new();
            var ingreso = await db.Ingresos.Select(x => new
            {
                x.id_ingreso,
                x.id_usuario,
                x.id_fuente,
                x.monto
            }).FirstOrDefaultAsync(x => x.id_ingreso == id);

            if (ingreso == null)
            {
                r.Message = $"No se encuentra el ingreso con id: {id}";
                return BadRequest(r);
            }

            r.Data = ingreso;
            r.Success = true;
            return Ok(r);
        }
    }
}

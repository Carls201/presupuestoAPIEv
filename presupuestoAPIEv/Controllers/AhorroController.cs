using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AhorroController : ControllerBase
    {
        private readonly PresupuestoContext db;
        public AhorroController(PresupuestoContext context)
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
                var ahorro = await db.Ahorros.Select(x => new
                {
                    x.id_ahorro,
                    x.id_usuario,
                    x.id_meta,
                    x.monto
                }).ToListAsync();

                if (ahorro.Any())
                {
                    r.Data = ahorro;
                    r.Success = true;
                    r.Message = "Los ahorros se cargaron exitosamente";
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
        public async Task<IActionResult> PostAhorro(Ahorro ahorro)
        {
            Resp r = new();
            if (ahorro.id_usuario <= 0 || ahorro.id_meta <= 0 || ahorro.monto <= 0
                || ahorro.id_usuario == null || ahorro.id_meta== null || ahorro.monto == null)
            {
                r.Message = "Completa los campos vacios";
                return BadRequest(r);
            }

            var existeUsuario = await db.Usuarios.FirstOrDefaultAsync(x => x.id_usuario == ahorro.id_usuario);
            var existeMeta = await db.MetaAhorros.FirstOrDefaultAsync(x => x.id_meta == ahorro.id_meta);

            if (existeUsuario == null || existeMeta == null)
            {
                r.Message = "La meta o el usario ingresado no existe";
                return BadRequest(r);
            }

            db.Ahorros.Add(ahorro);
            await db.SaveChangesAsync();
            r.Message = "Gasto guardado";
            r.Success = true;
            r.Data = ahorro.id_ahorro;
            return CreatedAtAction("Get", r, ahorro);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAhorro(int id)
        {
            Resp r = new();
            var ahorro = await db.Ahorros.FindAsync(id);

            if (ahorro == null)
            {
                r.Message = "El ahorro que desea eliminar no se encuentra";
                return BadRequest(r);
            }

            db.Ahorros.Remove(ahorro);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Ahorro eliminado";
            return Ok(r);
        }

        // EDIT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAhorro(int id, Ahorro ahorro)
        {
            Resp r = new();
            var ahorrox = await db.Ahorros.Select(x => new
            {
                x.id_ahorro,
                x.id_usuario,
                x.id_meta,
                x.monto
            }).FirstOrDefaultAsync(x => x.id_ahorro == ahorro.id_ahorro
            );

            if (ahorrox == null)
            {
                r.Message = "El ahorro que desea eliminar no se encuentra";
                return BadRequest(r);
            }

            if (id != ahorro.id_ahorro)
            {
                r.Message = "El id ingresado no coincide con el id del ahorro que desea eliminar";
                return BadRequest(r);
            }

            db.Ahorros.Update(ahorro);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "El ahorro se ha modificado con exito";
            return Ok(r);
        }

        // GET POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAhorro(int id)
        {
            Resp r = new();
            var ahorro = await db.Ahorros.Select(x => new
            {
                x.id_ahorro,
                x.id_usuario,
                x.id_meta,
                x.monto
            }).FirstOrDefaultAsync(x => x.id_ahorro== id);

            if (ahorro == null)
            {
                r.Message = $"No se encuentra el ahorro con id: {id}";
                return BadRequest(r);
            }

            r.Data = ahorro;
            r.Success = true;
            return Ok(r);
        }
    }
}

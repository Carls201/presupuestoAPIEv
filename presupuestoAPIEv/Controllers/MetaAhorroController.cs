using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaAhorroController : ControllerBase
    {
        private readonly PresupuestoContext db;
        public MetaAhorroController(PresupuestoContext context)
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
                var metaAhorro = await db.MetaAhorros.Select(x => new
                {
                    x.id_meta,
                    x.nombre
                }).ToListAsync();

                if (metaAhorro.Any())
                {
                    r.Data = metaAhorro;
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
        public async Task<IActionResult> PostMetaAhorro(MetaAhorro metaAhorro)
        {
            Resp r = new();
            if (metaAhorro.nombre == "" || metaAhorro.nombre == null)
            {
                r.Message = "Los campos no pueden quedar vacio";
                return BadRequest(r);
            }

            db.Add(metaAhorro);
            await db.SaveChangesAsync();
            r.Message = "Se ha guardado con exito";
            r.Success = true;
            r.Data = metaAhorro.id_meta;
            return CreatedAtAction("Get", r, metaAhorro);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetaAhorro(int id)
        {
            Resp r = new();
            var mAhorro = await db.MetaAhorros.FindAsync(id);

            if (mAhorro == null)
            {
                r.Message = "La meta no existe";
                return BadRequest(r);
            }

            db.MetaAhorros.Remove(mAhorro);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Se ha eliminado con exito";
            return Ok(r);
        }

        // EDIT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMetaAhorro(int id, MetaAhorro mAhorro)
        {
            Resp r = new();
            var meta = await db.MetaAhorros.Select(x => new
            {
                x.id_meta,
                x.nombre
            }).FirstOrDefaultAsync(x => x.id_meta== id);

            if (meta == null)
            {
                r.Message = "Meta no encontrada";
                return BadRequest(r);
            }
            if (id != mAhorro.id_meta)
            {
                r.Message = "El id ingresado no coincide con el id de la meta que desea modificar";
                return BadRequest(r);
            }

            db.MetaAhorros.Update(mAhorro);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Meta modificada";
            return Ok(r);
        }

        // GET POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMetaAhorro(int id)
        {
            Resp r = new();
            var metaAhorro = await db.MetaAhorros.Select(x => new
            {
                x.id_meta,
                x.nombre
            }).FirstOrDefaultAsync(x => x.id_meta == id);

            if (metaAhorro == null)
            {
                r.Message = "Meta con encontrada";
                return NotFound(r);
            }

            r.Success = true;
            r.Data = metaAhorro;
            return Ok(r);
        }
    }
}

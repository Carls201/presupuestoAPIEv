using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly PresupuestoContext db;
        public RolController(PresupuestoContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Resp r = new();
            try
            {
                var rols = await db.Rols.Select(a => new
                {
                    id = a.id_rol,
                    rol = a.rol
                }).ToListAsync();

                if (rols.Any())
                {
                    r.Data = rols;
                    r.Success = true;
                    r.Message = "Los datos se han mostrado con exito";
                    return Ok(r);
                }

                r.Message = "No existen registros";
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
        public async Task<IActionResult> PostRol(Rol rol)
        {
            Resp r = new();
            if (rol.rol == null || rol.rol == "")
            {
                r.Message = "Los campos no pueden quedar vacio";
                return BadRequest(r);
            }

            db.Rols.Add(rol);
            await db.SaveChangesAsync();
            r.Message = "Se ha guardado con exito";
            r.Success = true;
            r.Data = rol.rol;
            return CreatedAtAction("Get", r, rol);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            Resp r = new();
            var rol = await db.Rols.FindAsync(id);

            if (rol == null)
            {
                r.Message = "No existe el usuario";
                return BadRequest(r);
            }

            db.Rols.Remove(rol);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "El Rol se ha eliminado con exito";
            return Ok(r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            Resp r = new();
            var rolx = await db.Rols.Select(r => new
            {
                id = r.id_rol,
                rol = r.rol
            }).FirstOrDefaultAsync(x => x.id == id);

            if (rolx == null)
            {
                r.Message = "El rol que desea modificar no se encuentra";
                return BadRequest(r);
            }
            if (id != rol.id_rol)
            {
                r.Message = "El id que ingreso no coincide con el id del rol que desea modificar";
                return BadRequest(r);
            }

            db.Rols.Update(rol);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Rol editado con exito";
            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRol(int id)
        {
            Resp r = new();
            var rol = await db.Rols.Select(r => new
            {
                id = r.id_rol,
                rol = r.rol
            }).FirstOrDefaultAsync(x => x.id == id);

            if (rol == null)
            {
                r.Message = $"No se encuentra el rol con id: {id}";
                return NotFound(r);
            }
            r.Success = true;
            r.Data = rol;
            return Ok(r);
        }
    }
}

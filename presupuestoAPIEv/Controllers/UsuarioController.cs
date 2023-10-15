using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private PresupuestoContext db;
        public UsuarioController(PresupuestoContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Resp r = new();
            try
            {
                var usuarios = await db.Usuarios.Select(x => new
                {
                    id = x.id_usuario,
                    rol = x.id_rol,
                    nombre = x.nombre,
                    apellido = x.apellido,
                    edad = x.edad,
                    direccion = x.direccion
                }).ToListAsync();

                if (usuarios.Any())
                {
                    r.Data = usuarios;
                    r.Success = true;
                    r.Message = "Los datos se cargaron exitosamente";
                    return Ok(r);
                }

                r.Message = "No se encontraron datos";
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
        public async Task<IActionResult> PostUsuario(Usuario usuario)
        {
            Resp r = new();
            if (usuario.nombre == "" || usuario.apellido == "" || usuario.edad == 0 || usuario.direccion == "" || usuario.id_rol == 0)
            {
                r.Message = "Primero tiene que completar los campos vacios";
                return BadRequest(r);
            }

            var existeRol = await db.Rols.FirstOrDefaultAsync(x => x.id_rol == usuario.id_rol);
            if (existeRol == null)
            {
                r.Message = "El rol especificado no existe";
                return BadRequest(r);
            }

            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
            r.Message = "Usuario guardado";
            r.Success = true;
            r.Data = usuario.id_usuario;
            return CreatedAtAction("Get", r, usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            Resp r = new();
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                r.Message = "El usuario que desea eliminar no se encuentra";
                return BadRequest(r);
            }

            db.Usuarios.Remove(usuario);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Usuario eliminado";
            return Ok(r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            Resp r = new();
            var user = await db.Usuarios.Select(x => new
            {
                id = x.id_usuario,
                rol = x.id_rol,
                nombre = x.nombre,
                apellido = x.apellido,
                edad = x.edad,
                direccion = x.direccion
            }).FirstOrDefaultAsync(x => x.id == id);

            if (user == null)
            {
                r.Message = "El usario que desea modificar no se encuentra";
                return BadRequest(r);
            }
            if (id != usuario.id_usuario)
            {
                r.Message = "El id que ingreso no coincide con el id del usuario que desea modificar";
                return BadRequest(r);
            }

            db.Usuarios.Update(usuario);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Usuario editado";
            return Ok(r);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            Resp r = new();
            var usuario = await db.Usuarios.Select(x => new
            {
                id = x.id_usuario,
                rol = x.id_rol,
                nombre = x.nombre,
                apellido = x.apellido,
                edad = x.edad,
                direccion = x.direccion
            }).FirstAsync(x => x.id == id);

            if (usuario == null)
            {
                r.Message = $"No se encuentra el usuario de id: {id}";
                return NotFound(r);
            }
            r.Success = true;
            r.Data = usuario;
            return Ok(r);
        }
    }
}

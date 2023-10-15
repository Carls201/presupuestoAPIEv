using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using presupuestoAPIEv.Response;

namespace presupuestoAPIEv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GastoController : ControllerBase
    {
        private readonly PresupuestoContext db;
        public GastoController(PresupuestoContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Resp r = new();
            try
            {
                var gasto = await db.Gastos.Select(x => new
                {
                    x.id_gasto,
                    x.id_usuario,
                    x.id_categoria,
                    x.monto
                }).ToListAsync();

                if (gasto.Any())
                {
                    r.Data = gasto;
                    r.Success = true;
                    r.Message = "Los datos se cargaron exitosaente";
                    return Ok(r);
                }

                r.Message = "No se encuentran datos";
                r.Success = true;
                return Ok(r);

            }catch (Exception ex)
            {
                r.Message = ex.Message;
                return BadRequest(r);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostGasto(Gasto gasto)
        {
            Resp r = new();
            if (gasto.id_usuario <= 0 || gasto.id_categoria <= 0 || gasto.monto <= 0
                || gasto.id_usuario == null || gasto.id_categoria == null || gasto.monto == null)
            {
                r.Message = "Primero tiene que completar los campos vacios";
                return BadRequest(r);
            }
            
            var existeUsuario = await db.Usuarios.FirstOrDefaultAsync(x => x.id_usuario == gasto.id_usuario);
            var existeCategoria = await db.CategoriaGastos.FirstOrDefaultAsync(x => x.id_categoria == gasto.id_categoria);

            if (existeUsuario == null || existeCategoria == null)
            {
                r.Message = "La categoria o el usario ingresado no existe";
                return BadRequest(r);
            }

            db.Gastos.Add(gasto);
            await db.SaveChangesAsync();
            r.Message = "Gasto guardado";
            r.Success = true;
            r.Data = gasto.id_gasto;
            return CreatedAtAction("Get", r, gasto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGasto(int id)
        {
            Resp r = new();
            var gasto = await db.Gastos.FindAsync(id);

            if(gasto == null)
            {
                r.Message = "El gasto que desea eliminar no se encuentra";
                return BadRequest(r);
            }

            db.Gastos.Remove(gasto);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "Gasto eliminado";
            return Ok(r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGasto(int id, Gasto gasto)
        {
            Resp r = new();
            var gastox = await db.Gastos.Select(x => new
            {
                x.id_gasto,
                x.id_usuario,
                x.id_categoria,
                x.monto
            }).FirstOrDefaultAsync(x => x.id_gasto == gasto.id_gasto);

            if(gastox == null)
            {
                r.Message = "El gasto que desea eliminar no se encuentra";
                return BadRequest(r);
            }

            if(id != gasto.id_gasto)
            {
                r.Message = "El id ingresado no coincide con el id del gasto que desea eliminar";
                return BadRequest(r);
            }

            db.Gastos.Update(gasto);
            await db.SaveChangesAsync();
            r.Success = true;
            r.Message = "El gasto se ha modificado con exito";
            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGasto(int id)
        {
            Resp r = new();
            var gasto = await db.Gastos.Select(x => new
            {
                x.id_gasto,
                x.id_usuario,
                x.id_categoria,
                x.monto
            }).FirstOrDefaultAsync(x => x.id_gasto == id);

            if(gasto == null)
            {
                r.Message = $"No se encuentra el gasto con id: {id}";
                return BadRequest(r);
            }

            r.Data = gasto;
            r.Success = true;
            return Ok(r);
        }
    }
}

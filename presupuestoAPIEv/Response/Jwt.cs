using DB;
using System.Security.Claims;

namespace presupuestoAPIEv.Response
{
    public class Jwt
    {


        private PresupuestoContext db;
        public Jwt(PresupuestoContext context)
        {
            db = context;
        }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public int ExpireDay { get; set; }
        public static dynamic ValidarToken(ClaimsIdentity identity)
        {
            
            Resp r = new();
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    r.Message = "Token no valido";
                    return r;
                }
                var id = identity.Claims.Where(u => u.Type == "id_usuario").FirstOrDefault().Value;
                
                var user = db.Usuarios.Find(int.Parse(id));
                r.Success = true;
                r.Message = "Token valido";
                r.Data = user;
                return r;
            }
            catch (Exception ex)
            {
                r.Message = ex.ToString();
                return r;
            }
        }
    }
}

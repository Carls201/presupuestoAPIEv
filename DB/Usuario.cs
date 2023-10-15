using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_usuario {  get; set; }
        public int id_rol {  get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int edad {  get; set; }
        public string direccion {  get; set; }
        public string email { get; set; }
        public string pass {  get; set; }
    }
}

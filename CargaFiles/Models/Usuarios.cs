using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CargaFiles.Models
{
    public class Usuarios
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public Rol IdRol { get; set; }
    }
}
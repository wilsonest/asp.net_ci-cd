using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CargaFiles.Models
{
    public class Productos
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Id { get; set; }
        public Usuarios usuarios { get; set; }

    }
}
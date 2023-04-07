using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CargaFiles.Models
{
    public class Imagenes
    {
        public int Id{ get; set; }
        public string NombreImg { get; set; }
        public byte[] Imagen { get; set; }
        public int IdProducto { get; set; }
        public Products productos { get; set; }
    }
}
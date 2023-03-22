using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CargaFiles.Models
{
    public class Fotos
    {
        
        public int IdFotos { get; set;}
        public string NombreImg { get; set; }
        //[StringLength(50, ErrorMessage = "Maximum {2} characters exceeded")]
        public byte[] Imagen { get; set; }

        public Productos productos { get; set; }

    }
}
using CargaFiles.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CargaFiles.Controllers
{
    public class PrincipalController : Controller
    {
        static string cadena = "Data Source=ZIBOR-64517;Initial Catalog=DBARCHIVOS;Integrated Security=true";
        static List<Archivos> oLista = new List<Archivos>();
        // GET: Principal
        public ActionResult Index()
        {
            //obtener();
            oLista = new List<Archivos>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select * from archivos", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Archivos archivoE = new Archivos();

                        archivoE.IdArchivo = Convert.ToInt32(dr["IdArchivo"]);
                        archivoE.Nombre = dr["Nombre"].ToString();
                        archivoE.Archivo = dr["Archivo"] as byte[];
                        archivoE.Extension = dr["Extension"].ToString();
                        oLista.Add(archivoE);

                    }
                }
            }


            return View(oLista);
        }


        public ActionResult ver(int IdArchivo)
        {
            Archivos oArchivo = oLista.Where(a => a.IdArchivo == IdArchivo).FirstOrDefault();
            if (oArchivo == null)
            {
                return HttpNotFound();
            }

            return File(oArchivo.Archivo, "application/octet-stream");
        }
    }
}
using CargaFiles.Logica;
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
        static List<Products> oLista = new List<Products>();
        static List<Imagenes> img = new List<Imagenes>();
        // GET: Principal
        public ActionResult Index()
        {
            //obtener();
            oLista = new List<Products>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select * from Productos", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Products archivoE = new Products();

                        archivoE.IdProducto = Convert.ToInt32(dr["IdProducto"]);
                        archivoE.Nombre = dr["Nombre"].ToString();
                        archivoE.Descripcion = dr["Descripcion"].ToString();
                        archivoE.Id = Convert.ToInt32(dr["Id"]);
                        oLista.Add(archivoE);
                    }

                }
            }
            return View(oLista);
        }

        public ActionResult ver(int IdProducts)
        {
            Lo_Usuario lo = new Lo_Usuario();
            DataTable dt = lo.GetImagenes();
            DataRow row = dt.AsEnumerable().FirstOrDefault(r => r.Field<int>("IdProducto") == IdProducts);
            if (row != null)
            {
                byte[] imgc = (byte[])row["Imagen"];
                return File(imgc, "application/octet-stream");
            }
            return RedirectToAction("Index", "Principal");
        }
    }


}
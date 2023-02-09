using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using CargaFiles.Models;

namespace CargaFiles.Controllers
{
    public class ArchivoController : Controller
    {
        static string cadena = "Data Source=ZIBOR-64517;Initial Catalog=DBARCHIVOS;Integrated Security=true";
        static List<Archivos> oLista = new List<Archivos>();
        public ActionResult Subir()
        {
            return View();
        }

        public void obtener()
        {
            DataTable productos = new DataTable();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select * from archivos", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(productos);
                da.Dispose();
            }
        }

        // GET: Archivo
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

        // metodo post con parametor de ingreso que son el nombre y el archivo

        [HttpPost]
        public ActionResult SubirArchivo(string Nombre, HttpPostedFileBase Archivo)
        {

            //obtenemos la extension 
            string Extension = Path.GetExtension(Archivo.FileName);
            //string name = Path.GetFileName(Archivo.FileName);
            MemoryStream ms = new MemoryStream();
            Archivo.InputStream.CopyTo(ms);
            byte[] data = ms.ToArray();
            //coneccion a base de datos
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("insert into archivos(Nombre,Archivo,Extension) values(@nombre,@archivo,@extension)", oconexion);
                //cmd.Parameters.AddWithValue("@nombre", name);
                cmd.Parameters.AddWithValue("@nombre", Nombre + "." + Extension);
                cmd.Parameters.AddWithValue("@archivo", data);
                cmd.Parameters.AddWithValue("@extension", Extension);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Archivo");
        }

        //metodo para traer los bytes de la bd y convertilo a imagen para luego mostrarlo en el front
        public ActionResult Mostrar(int IdArchivo)
        {
            Archivos oArchivo = oLista.Where(a => a.IdArchivo == IdArchivo).FirstOrDefault();
            if (oArchivo == null)
            {
                return HttpNotFound();
            }

            return File(oArchivo.Archivo, "application/octet-stream");
        }


        [HttpPost]
        public FileResult DescargarArchivo(int IdArchivo)
        {
            Archivos oArchivo = oLista.Where(a => a.IdArchivo == IdArchivo).FirstOrDefault();
            string NombreCompleto = oArchivo.Nombre;
            var img = File(oArchivo.Archivo, "application/" + oArchivo.Extension.Replace(".", ""), NombreCompleto);
            return img;
        }



    }
}
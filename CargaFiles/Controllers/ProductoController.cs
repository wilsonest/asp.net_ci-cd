using CargaFiles.Logica;
using CargaFiles.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CargaFiles.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult CrearProducto()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearProducto(Productos prod, HttpPostedFileBase Archivo)
        {
            //int a = prod.usuarios.Id;
           
            if (prod.Nombre == null || prod.Descripcion == null)
            {
                return View("Error", new { message = "Falla al registrarte" });

            }
            else if (prod.Nombre != null)
            {
                FormsAuthentication.SetAuthCookie(prod.ToString(), false);
                int a = (int)Session["IdUsuario"];
                Session["Usuario"] = prod.Nombre;
                Productos objeto = new LO_Usuario().CrearProduct(prod, a);
                Imagen(Archivo);
                ViewBag.Message = "User registered successfully!";
                return RedirectToAction("Index", "Principal");
            }

            //Productos objeto = new LO_Usuario().CrearProduct(prod,a);
            //ViewBag.Message = "User registered successfully!";
            return RedirectToAction("Index", "Principal");
        }

        [HttpPost]
        public void Imagen(HttpPostedFileBase Archivo)
        {
            if (Archivo == null)
            {
                // return View("Error", new { message = "Falla al registrarte" });
                // No se puede retornar una vista desde un método void, se puede generar un error o registrar una excepción
                throw new Exception("Falla al registrarte");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(Archivo.ToString(), false);
                int a = (int)Session["IdUsuario"];
                string name = Path.GetFileName(Archivo.FileName);
                MemoryStream ms = new MemoryStream();
                Archivo.InputStream.CopyTo(ms);
                byte[] data = ms.ToArray();
                Productos prod = new LO_Usuario().GetProductos(a);
                Session["IdProducto"] = prod.IdProducto;
                int b = (int)Session["IdProducto"];
                new LO_Usuario().SubirImagen(b, data, name);
                // ViewBag.Message = "User registered successfully!";
                // No se puede usar ViewBag desde un método void
            }
        }
    }
}
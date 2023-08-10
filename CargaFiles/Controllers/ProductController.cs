using CargaFiles.Logica;
using CargaFiles.Models;
using CargaFiles.Permisos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CargaFiles.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [Authorize]
        public ActionResult CrearProduct()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CrearProduct(Products prod, List<HttpPostedFileBase> Archivos)
        {
            if (prod.Nombre == null || prod.Descripcion == null)
            {
                return View("Error", new { message = "Falla al registrarte" });
            }
            else if (prod.Nombre != null)
            {
                FormsAuthentication.SetAuthCookie(prod.ToString(), false);
                int a = (int)Session["IdUsuario"];
                Products objeto = new Lo_Usuario().CrearProducto(prod, a);
                Products producto = new Lo_Usuario().GetProductos(a);
                Session["IdProducto"] = producto.IdProducto;
                int b = (int)Session["IdProducto"];

                if (Archivos != null && Archivos.Count > 0)
                {
                    new Lo_Usuario().SubirImagen(b,Archivos);
                }
                ViewBag.Message = "User registered successfully!";
                return RedirectToAction("Index", "Principal");
            }
            return RedirectToAction("Index", "Principal");
        }

        public ActionResult InfoProdutcs(int idProducto, string descripcion)
        {
            Lo_Usuario lo = new Lo_Usuario();
            DataTable dt = lo.GetImagenes();
            List<string> imagenes = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                if ((int)row["IdProducto"] == idProducto)
                {
                    byte[] imagen = (byte[])row["Imagen"];
                    string base64String = Convert.ToBase64String(imagen);
                    string imgSrc = string.Format("data:image/jpeg;base64,{0}", base64String);
                    imagenes.Add(imgSrc);
                }
            }

            ViewBag.Descripcion = descripcion;
            ViewBag.IdProducto = idProducto;
            ViewBag.Imagenes = imagenes;

            return View();
        }



    }
}
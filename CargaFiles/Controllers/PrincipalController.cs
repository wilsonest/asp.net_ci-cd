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
        // GET: Principal
        public ActionResult Index()
        {
            Lo_Usuario lo = new Lo_Usuario();
            List<Products> productos = lo.GetAllProducts();
            return View(productos);
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
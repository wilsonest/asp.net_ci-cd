using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using CargaFiles.Models;
using CargaFiles.Logica;
using System.Web.Security;

namespace CargaFiles.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Inicio()
        
        {
            return View();
        }
        //los parametros los trae la vista
        [HttpPost]
        public ActionResult Inicio(string correo, string clave)
        {
            //mandamos los parametros al modelo y metodo
            Usuarios objeto = new LO_Usuario().EncontrarUsuario(correo, clave);

            if (objeto.Nombres != null)
            {


                FormsAuthentication.SetAuthCookie(objeto.Correo, false);

                Session["Usuario"] = objeto;

                return RedirectToAction("Index", "Archivo");
            }



            return View();
        }
    }
}
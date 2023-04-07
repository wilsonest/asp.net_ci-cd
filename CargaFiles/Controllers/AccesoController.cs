using CargaFiles.Logica;
using CargaFiles.Models;
using System.Web.Mvc;
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
            Usuarios objeto = new Lo_Usuario().EncontrarUsuario(correo, clave);

            //Session["Usuario"] = objeto.Nombre;
            if (objeto.Nombre != null)
            {
                FormsAuthentication.SetAuthCookie(objeto.ToString(), false);

                Session["UsuarioNombre"] = objeto.Nombre;
                Session["IdUsuario"] = objeto.Id;

                return RedirectToAction("Index", "Principal");
            }


            return View();
        }


        public ActionResult Registrar()

        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuarios user)
        {
            

            if (user.Clave == null || user.Nombre == null)
            {
                return View("Error", new { message = "Falla al registrarte" });

            }

            Usuarios objeto = new Lo_Usuario().Guardar(user);
            ViewBag.Message = "User registered successfully!";
            return RedirectToAction("Inicio", "Acceso");
        }
    }
}
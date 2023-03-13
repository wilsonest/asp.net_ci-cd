﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


using CargaFiles.Permisos;
using CargaFiles.Models;



namespace CargaFiles.Controllers
{
    //[Authorize] // si necesitamos restringir algunos campos o vistas a algun perfil
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
        //[Authorize]
        //[PermisosRol(Rol.Administrador)]
        public ActionResult About()
        {
            ViewBag.Message = "bienvenido a la pagina about";

            return View();
        }


        //[PermisosRol(Models.Rol.Administrador)]
        public ActionResult Contact()
        {
            ViewBag.Message = "bienvenido a la pagina contact";

            return View();
        }


        public ActionResult SinPermiso()
        {
            ViewBag.Message = "Usted no cuenta con permisos para ver esta pagina";

            return View();
        }

        public ActionResult CerrarSesion()
        {

            FormsAuthentication.SignOut();
            Session["Usuario"] = null;


            return RedirectToAction("Inicio", "Acceso");



        }
    }
}
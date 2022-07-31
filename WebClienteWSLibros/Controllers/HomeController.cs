using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebClienteWSLibros.ServiceReferenceLibros;

namespace WebClienteWSLibros.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (WebServiceLibrosSoapClient ws = new WebServiceLibrosSoapClient())
            {
                try
                {
                    List<ClassLibrosExt> ls = new List<ClassLibrosExt>();
                    ls = ws.Obtener().ToList();
                    return View(ls);
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                    return View("Index", ws.Obtener().ToList());
                }
            }
        }
        [HttpGet]
        public ActionResult Create() 
        {
            using(WebServiceLibrosSoapClient ws = new WebServiceLibrosSoapClient())
                {
                ViewBag.Categoria = new SelectList(ws.ObtenerCategoria().ToList(), "IdC", "NombreC");
                }
            return View();
        }
        [HttpPost]
        public ActionResult Create(ClassLibrosExt l) 
        {
            using (WebServiceLibrosSoapClient ws = new WebServiceLibrosSoapClient())
                {
                try
                {
                    ViewBag.Categoria = new SelectList(ws.ObtenerCategoria().ToList(), "IdC", "NombreC");
                    ws.Agregar(l);
                    TempData["resu"] = $"Se agregó un nuevo registro { l.Titulo}{ l.Autor}";
                    return View("Create");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                    return View("Create");
                }
                    
                }     
        }
        public ActionResult Edit(int id)
        {
            using (WebServiceLibrosSoapClient ws = new WebServiceLibrosSoapClient())
            {
                ClassLibrosExt ce = ws.IdObtener(id);
                ViewBag.Categoria = new SelectList(ws.ObtenerCategoria().ToList(), "IdC", "NombreC", ce.Categoria);
                return View("Edit", ws.IdObtener(id));
            }
        }
        [HttpPost]
        public ActionResult Edit(ClassLibrosExt l) 
        {
            using (WebServiceLibrosSoapClient ws = new WebServiceLibrosSoapClient())
            {
                try
                {
                    ws.Editar(l);
                    TempData["resu"] = $"Registro { l.Titulo} fue actualizado. ";
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
                return RedirectToAction("Index", ws.Obtener().ToList()); 
            }
        }
        public ActionResult Delete(int id) 
        {
            using (WebServiceLibrosSoapClient ws = new WebServiceLibrosSoapClient()) 
            {
                try
                {
                    ws.Eliminar(id);
                    TempData["resu"] = "Registro Eliminado";
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
                return RedirectToAction("Index", ws.Obtener().ToList());//
            }
        }
        public ActionResult Buscar(string valor)
        {
            using (WebServiceLibrosSoapClient ws = new WebServiceLibrosSoapClient())
            {
                return View("Index", ws.Buscar(valor).ToList());
            }
        }
    }
}
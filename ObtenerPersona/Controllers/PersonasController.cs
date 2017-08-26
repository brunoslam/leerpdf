using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ObtenerPersona.Models;

namespace ObtenerPersona.Controllers
{
    public class PersonasController : Controller
    {
        private PersonaPdfEntities db = new PersonaPdfEntities();

        // GET: Personas
        public ActionResult Index()
        {
            return View();
        }

        // GET: Personas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Rut,Region,Provincia,Ciudad")] Persona persona)
        {
            //List<Persona> listaPersonas = db.Persona.ToList();
            var query = (from p in db.Persona select p);
            if (persona.Nombre != null)
            {
                query = query.Where(p => p.Nombre.Contains(persona.Nombre));
            }
            if (persona.Rut != null)
            {
                query = query.Where(p => p.Rut.Contains(persona.Rut));
            }
            if (persona.Region != null)
            {
                query = query.Where(p => p.Region.Contains(persona.Region));
            }
            if (persona.Provincia != null)
            {
                query = query.Where(p => p.Provincia.Contains(persona.Provincia));
            }
            if (persona.Ciudad != null)
            {
                query = query.Where(p => p.Ciudad.Contains(persona.Ciudad));
            }

            //List<Persona> listaPersonas = db.Persona.Where(p => p.Nombre.Contains(persona.Nombre) && 
            //                                                  p.Rut.Contains(persona.Rut)).ToList();

            List<Persona> listaPersonas = query.ToList();
            return View("Index", listaPersonas);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Rut,Genero,Direccion,Circunscripcion,Region,Provincia,Ciudad")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Persona persona = db.Persona.Find(id);
            db.Persona.Remove(persona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

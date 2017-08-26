using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ConsultaPersona.Models;
using System.Web.OData;
namespace ConsultaPersona.Controllers
{
    public class PersonasController : ODataController
    {
        
        private PersonaPdfEntities db = new PersonaPdfEntities();

        // GET: api/Personas
        [Queryable]
        public IQueryable<Persona> GetPersona()
        {
            //return db.Persona.AsQueryable();
            return db.Persona.AsQueryable();
        }

        // GET: api/Personas/5
        [ResponseType(typeof(Persona))]
        public IHttpActionResult GetPersona(int id)
        {
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }

        // GET: api/Personas/5
        public IHttpActionResult GetPersonaByNombre(String id)
        {
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }


        // PUT: api/Personas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPersona(int id, Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != persona.Id)
            {
                return BadRequest();
            }

            db.Entry(persona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Personas
        [ResponseType(typeof(Persona))]
        public IHttpActionResult PostPersona(Persona persona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Persona.Add(persona);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PersonaExists(persona.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = persona.Id }, persona);
        }

        // DELETE: api/Personas/5
        [ResponseType(typeof(Persona))]
        public IHttpActionResult DeletePersona(int id)
        {
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return NotFound();
            }

            db.Persona.Remove(persona);
            db.SaveChanges();

            return Ok(persona);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonaExists(int id)
        {
            return db.Persona.Count(e => e.Id == id) > 0;
        }
    }
}
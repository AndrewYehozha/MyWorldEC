using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class PreferencesController : ApiController
    {
        private MyWorldECEntities2 db = new MyWorldECEntities2();

        // GET: api/Preferences
        public IQueryable<Preference> GetPreferences()
        {
            return db.Preferences;
        }

        // GET: api/Preferences/5
        [ResponseType(typeof(Preference))]
        public async Task<IHttpActionResult> GetPreference(int id)
        {
            Preference preference = await db.Preferences.FindAsync(id);
            if (preference == null)
            {
                return NotFound();
            }

            return Ok(preference);
        }

        // PUT: api/Preferences/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPreference(int id, Preference preference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != preference.Id)
            {
                return BadRequest();
            }

            db.Entry(preference).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreferenceExists(id))
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

        // POST: api/Preferences
        [ResponseType(typeof(Preference))]
        public async Task<IHttpActionResult> PostPreference(Preference preference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Preferences.Add(preference);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = preference.Id }, preference);
        }

        // DELETE: api/Preferences/5
        [ResponseType(typeof(Preference))]
        public async Task<IHttpActionResult> DeletePreference(int id)
        {
            Preference preference = await db.Preferences.FindAsync(id);
            if (preference == null)
            {
                return NotFound();
            }

            db.Preferences.Remove(preference);
            await db.SaveChangesAsync();

            return Ok(preference);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PreferenceExists(int id)
        {
            return db.Preferences.Count(e => e.Id == id) > 0;
        }
    }
}
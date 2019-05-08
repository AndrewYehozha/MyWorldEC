using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;
using WebApplication.Models.Request;

namespace WebApplication.Services
{
    public class PreferenceService
    {
        private MyWorldECEntities2 db = new MyWorldECEntities2();

        public async Task<List<Preference>> GetPreferences()
        {
            var preferences = await db.Preferences.ToListAsync();

            return preferences;
        }

        public async Task<Preference> GetPreference(int id)
        {
            var preference = await db.Preferences.FindAsync(id);

            return preference;
        }

        public async Task<List<Preference>> GetPreferenceByUserId(int userId)
        {
            var preferences = await db.Preferences.Where(p => p.UserId == userId).ToListAsync();

            return preferences;
        }

        public async Task<bool> CheckPreference(PreferenceRequest request)
        {
            var preferences = db.Preferences.Where(p => p.UserId == request.UserId && p.ServiceId == request.ServiceId);

            return preferences != null;
        }

        public async Task<List<Service>> GetServiceNoExistToPreferenceByUserId(int userId)
        {
            var preferenceByUser = await db.Preferences.Where(m => m.UserId == userId).Select(m => m.ServiceId).ToListAsync();

            var service = await db.Services.Where(m => !(preferenceByUser.Contains(m.Id))).ToListAsync();

            return service;
        }

        public async Task<Preference> GetPreferenceByUserIdAndServiceId(int userId, int serviceId)
        {
            var preference = await db.Preferences.Where(x => x.UserId == userId && x.ServiceId == serviceId).FirstOrDefaultAsync();

            return preference;
        }

        public async Task UpdatePreference(Preference preference)
        {
            db.Entry(preference).State = EntityState.Modified;

            await db.SaveChangesAsync();
        }

        public async Task<Preference> AddPreference(Preference preference)
        {
            var preferenc =  db.Preferences.Add(preference);
            await db.SaveChangesAsync();

            return preferenc;
        }

        public async Task DeletePreference(Preference preference)
        {
            db.Preferences.Remove(preference);
            await db.SaveChangesAsync();
        }
    }
}
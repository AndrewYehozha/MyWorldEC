using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class EntertainmentCenterService
    {
        private MyWorldECEntities db = new MyWorldECEntities();

        public async Task<IEnumerable<Entertainment_Centers>> GetEntertainment_Centers()
        {
            var entertainment_Centers = await db.Entertainment_Centers.ToListAsync();

            return entertainment_Centers;
        }

        public async Task<Entertainment_Centers> GetEntertainment_Center(int id)
        {
            var entertainment_Center = await db.Entertainment_Centers.FindAsync(id);
            
            return entertainment_Center;
        }

        public async Task UpdateEntertainment_Center(Entertainment_Centers entertainment_Center)
        {
            db.Entry(entertainment_Center).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Entertainment_Centers> AddEntertainment_Center(Entertainment_Centers entertainment_Center)
        {
            var result = db.Entertainment_Centers.Add(entertainment_Center);
            await db.SaveChangesAsync();

            return result;
        }

        public async Task DeleteEntertainment_Centers(int id)
        {
            var entertainment_Center = await db.Entertainment_Centers.FindAsync(id);

            db.Entertainment_Centers.Remove(entertainment_Center);
            await db.SaveChangesAsync();
        }

        private bool Entertainment_CentersExists(int id)
        {
            return db.Entertainment_Centers.Count(e => e.Id == id) > 0;
        }
    }
}
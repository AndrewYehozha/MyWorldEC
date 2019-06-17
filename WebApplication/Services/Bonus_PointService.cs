using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class Bonus_PointService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();

        public async Task<List<Bonus_Points>> GetBonus_Points()
        {
            var bonus_Points = await db.Bonus_Points.ToListAsync();

            return bonus_Points;
        }

        public async Task<Bonus_Points> GetBonus_Point(int id)
        {
            var bonus_Point = await db.Bonus_Points.Where(b => b.UserId == id).FirstOrDefaultAsync();

            return bonus_Point;
        }

        public async Task<List<Bonus_Points>> GetBonus_PointsByUserId(int userId)
        {
            var bonus_Points = await db.Bonus_Points.Where(b => b.UserId == userId).ToListAsync();

            return bonus_Points;
        }

        public async Task AddBonus_Point(Bonus_Points bonus_Points)
        {
            db.Bonus_Points.Add(bonus_Points);

            await db.SaveChangesAsync();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            this.Dispose(disposing);
        }
    }
}
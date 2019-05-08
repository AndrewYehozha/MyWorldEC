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
    public class RatingService
    {
        private MyWorldECEntities2 db = new MyWorldECEntities2();

        public async Task<IEnumerable<Rating>> GetRatings()
        {
            var ratings = await db.Ratings.ToListAsync();

            return ratings;
        }

        public async Task<Rating> GetRating(int id)
        {
            var rating = await db.Ratings.FindAsync(id);

            return rating;
        }

        public async Task<bool> CheckRating(RatingRequest request) {
            var rating = await db.Ratings.Where(x => x.Id == request.Id && x.ServiceId == request.ServiceId && x.UserId == request.UserId).FirstOrDefaultAsync();

            return rating != null;
        }

        public async Task<decimal?> AvgRating(int serviceId)
        {
            var ratingAvg = db.Ratings.Where(x => x.Id == serviceId).Select(r => r.Rating1).ToList<decimal?>();

            return ratingAvg.Average();
        }

        public async Task UpdateRating(Rating rating)
        {
            db.Entry(rating).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Rating> AddRating(Rating rating)
        {
            var result = db.Ratings.Add(rating);
            await db.SaveChangesAsync();

            return result;
        }

        public async Task DeleteRating(Rating rating)
        {
            db.Ratings.Remove(rating);
            await db.SaveChangesAsync();
        }
    }
}
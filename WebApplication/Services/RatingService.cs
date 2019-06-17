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
        private MyWorldECEntities4 db = new MyWorldECEntities4();

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
            var ratingAvg = await db.Ratings.Where(x => x.ServiceId == serviceId).Select(r => r.Rating1).DefaultIfEmpty(0).AverageAsync();

            return ratingAvg;
        }

        public async Task<decimal?> GetRatingByuserId(int serviceId, int userId)
        {
            var rating = await db.Ratings.Where(x => x.UserId == userId && x.ServiceId == serviceId).Select(r => r.Rating1).FirstOrDefaultAsync();

            return rating;
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
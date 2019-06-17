using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class Discount_CardService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();

        public async Task<List<Discount_Cards>> GetDiscount_Cards()
        {
            var discountCards = await db.Discount_Cards.ToListAsync();
            return discountCards;
        }

        public async Task<Discount_Cards> GetDiscount_Card(int id)
        {
            var discountCard = await db.Discount_Cards.FindAsync(id);
           
            return discountCard;
        }

        public async Task<List<Discount_Cards>> GetDiscount_CardsByUserID(int userId)
        {
            var discountCard = await db.Discount_Cards.Where(d => d.UserId == userId).ToListAsync();

            return discountCard;
        }

        public async Task<bool> CountDiscount_CardByNumberCard(int id, decimal numberCard)
        {
            var countDiscountCard = await db.Discount_Cards.Where(d => d.NumberCard == numberCard && d.Id != id).CountAsync();

            return countDiscountCard > 0;
        }

        public async Task UpdateDiscount_Card(Discount_Cards discount_Card)
        {
            db.Entry(discount_Card).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Discount_Cards> AddDiscount_Card(Discount_Cards discount_Card)
        {
            var result = db.Discount_Cards.Add(discount_Card);
            await db.SaveChangesAsync();

            return result;
        }

        public async Task DeleteDiscount_Card(Discount_Cards discount_Card)
        {
            db.Discount_Cards.Remove(discount_Card);
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
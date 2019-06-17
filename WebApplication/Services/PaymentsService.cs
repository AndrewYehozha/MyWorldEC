using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class PaymentsService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();

        public async Task<List<Payment>> GetPayments()
        {
            var payments = await db.Payments.ToListAsync();

            return payments;
        }

        public async Task<Payment> GetPayment(int id)
        {
           var payment = await db.Payments.FindAsync(id);
           
            return payment;
        }

        public async Task<List<Payment>> GetPaymentsByUserId(int userId)
        {
            var payments = await db.Payments.Where(p => p.UserId == userId).ToListAsync();

            return payments;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            var paymen = db.Payments.Add(payment);
            await db.SaveChangesAsync();

            return paymen;
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
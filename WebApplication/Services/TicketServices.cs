using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TicketService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();

        public async Task<List<Ticket>> GetTickets()
        {
            var tickets = await db.Tickets.ToListAsync();

            return tickets;
        }

        public async Task<Ticket> GetTicket(int id)
        {
            var ticket = await db.Tickets.Where(t => t.UserId == id).FirstOrDefaultAsync();

            return ticket;
        }

        public async Task<List<Ticket>> GetTicketByUserId(int userId)
        {
            var tickets = await db.Tickets.Where(t => t.UserId == userId).ToListAsync();

            return tickets;
        }

        public async Task AddTicket(Ticket ticket)
        {
            db.Tickets.Add(ticket);

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
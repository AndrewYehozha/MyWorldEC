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
using WebApplication.Models.Response;
using WebApplication.Services;
using System.IdentityModel.Tokens.Jwt;
namespace WebApplication.Controllers
{
    [Authorize]
    public class TicketController : ApiController
    {
        private TicketService _ticketService = new TicketService();

        // GET: api/Tickets1
        [ActionName("GetTickets")]
        [HttpGet]
        public async Task<object> GetTickets()
        {
            var tickets = await _ticketService.GetTickets();

            if (tickets == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Tickets not found");
            }

            var models = new List<TicketViewModel>();

            foreach (var ticket in tickets)
            {
                models.Add(GetTicketModel(ticket));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Tickets1/5
        [ActionName("GetTicket")]
        [HttpGet]
        public async Task<object> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicket(id);

            if (ticket == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Ticket not found");
            }

            var model = GetTicketModel(ticket);

            return JsonResults.Success(model);
        }

        // GET: api/Tickets1
        [ActionName("GetTicketByUserId")]
        [HttpGet]
        public async Task<object> GetTicketByUserId(int id)
        {
            var tickets = await _ticketService.GetTicketByUserId(id);

            if (tickets == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Tickets not found");
            }

            var models = new List<TicketViewModel>();

            foreach (var ticket in tickets)
            {
                models.Add(GetTicketModel(ticket));
            }

            return JsonResults.Success(models);
        }

        // Util
        private TicketViewModel GetTicketModel(Ticket ticket)
        {
            var model = new TicketViewModel
            {
                Id = ticket.Id,
                ServiceId = ticket.ServiceId,
                ServiceName = ticket.Service.Name,
                UserId = ticket.UserId,
                PreOrder_Date = ticket.PreOrder_Date,
                Price = ticket.Price,
                IsUse = ticket.IsUse
            };

            return model;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Models.Request;
using WebApplication.Models.Response;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Authorize]
    public class PaymentsController : ApiController
    {
        private PaymentsService _paymentsService = new PaymentsService();
        private Bonus_PointService _bonus_PointService = new Bonus_PointService();
        private TicketService _ticketService = new TicketService();
        private UserService _userService = new UserService();

        // GET: api/Payments
        [ActionName("GetPayments")]
        [HttpGet]
        public async Task<object> GetPayments()
        {
            var payments = await _paymentsService.GetPayments();

            if (payments == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Payments not found");
            }

            var models = new List<PaymentViewModel>();

            foreach (var payment in payments)
            {
                models.Add(GetPaymentModel(payment));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Payments/5
        [ActionName("GetPayment")]
        [HttpGet]
        public async Task<object> GetPayment(int id)
        {
            var payment = await _paymentsService.GetPayment(id);

            if (payment == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Payment not found");
            }

            var model = GetPaymentModel(payment);

            return JsonResults.Success(model);
        }

        // GET: api/Payments/5
        [ActionName("GetPaymentByUserId")]
        [HttpGet]
        public async Task<object> GetPaymentByUserId(int id)
        {
            var payments = await _paymentsService.GetPaymentsByUserId(id);

            if (payments == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "The user has no payments");
            }

            var models = new List<PaymentViewModel>();

            foreach (var payment in payments)
            {
                models.Add(GetPaymentModel(payment));
            }

            return JsonResults.Success(models);
        }

        // POST: api/Payments
        [ActionName("AddPayment")]
        [HttpPost]
        public async Task<object> AddPayment(PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var user = await _userService.GetUser((int)request.UserId);

                if (request.IsBonusPayment)
                {
                    decimal bonuceCoef = 5;

                    if (user.BonusScore < (request.Cost * bonuceCoef))
                    {
                        return JsonResults.Error(errorNum: 404, errorMessage: "Not enough bonus score");
                    }

                    user.BonusScore -= (int)(request.Cost * bonuceCoef);

                    var modelBonusPoint = new Bonus_Points
                    {
                        ServiceId = request.ServiceId,
                        UserId = request.UserId,
                        Amount = (request.Cost * bonuceCoef),
                        DateOfUse = DateTime.Now
                    };

                    await _bonus_PointService.AddBonus_Point(modelBonusPoint);
                } 
                else
                {
                    user.BonusScore += (int)request.Cost;

                    var modelPayment = new Payment
                    {
                        UserId = request.UserId,
                        Cost = request.Cost,
                        ServiceId = request.ServiceId,
                        Entert_CenterId = request.Entert_CenterId,
                        PaymentDate = DateTime.Now
                    };

                    await _paymentsService.AddPayment(modelPayment);
                }

                await _userService.UpdateUser(user);

                var modelTicket = new Ticket
                {
                    PreOrder_Date = DateTime.Now,
                    ServiceId = request.ServiceId,
                    UserId = request.UserId,
                    Price = request.Cost,
                    IsUse = false
                };

                await _ticketService.AddTicket(modelTicket);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // Util
        private PaymentViewModel GetPaymentModel(Payment payment)
        {
            var model = new PaymentViewModel
            {
                Id = payment.Id,
                Entert_CenterId = payment.Entert_CenterId,
                ServiceId = payment.ServiceId,
                UserId = payment.UserId,
                PaymentDate = payment.PaymentDate,
                Cost = payment.Cost
            };

            return model;
        }
    }
}
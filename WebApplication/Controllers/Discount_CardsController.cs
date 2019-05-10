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
using WebApplication.Models.Request;
using WebApplication.Models.Response;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Authorize]
    public class Discount_CardsController : ApiController
    {
        private Discount_CardService _discount_CardService = new Discount_CardService();

        // GET: api/Discount_Cards
        [ActionName("GetDiscount_Cards")]
        [HttpGet]
        public async Task<object> GetDiscount_Cards()
        {
            var discount_Cards = await _discount_CardService.GetDiscount_Cards();

            if (discount_Cards == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Discount Cards not found");
            }

            List<Discount_CardViewModel> models = new List<Discount_CardViewModel>();

            foreach (var discount_Card in discount_Cards)
            {
                models.Add(GetDiscount_CardModel(discount_Card));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Discount_Card/5
        [ActionName("GetDiscount_Card")]
        [HttpGet]
        public async Task<object> GetDiscount_Card(int id)
        {
            var discount_Card = await _discount_CardService.GetDiscount_Card(id);

            if (discount_Card == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Discount Card not found");
            }
            var model = GetDiscount_CardModel(discount_Card);

            return JsonResults.Success(model);
        }

        // PUT: api/Discount_Cards/5
        [ActionName("EditDiscount_Card")]
        [HttpPost]
        public async Task<object> EditDiscount_Card(Discount_CardRequest request)
        {
            if (request.NumberCard == 0)
            {
                return JsonResults.Error(400, "The Card Number is required");
            }

            if (request.NumberCard < 1000000000000000 || request.NumberCard > 9999999999999999)
            {
                return JsonResults.Error(400, "Invalid Card Number. (Card Number must be 16 numbers)");
            }

            try
            {
                var checkValidateCard = await _discount_CardService.CountDiscount_CardByNumberCard(request.Id, request.NumberCard);
                var discount_Card = await _discount_CardService.GetDiscount_Card(request.Id);

                if (checkValidateCard)
                {
                    return JsonResults.Error(302, "This discount card is already in the database");
                }

                if (discount_Card == null)
                {
                    return JsonResults.Error(404, "It`s discount card not found");
                }

                discount_Card.NumberCard = request.NumberCard;
                discount_Card.ServiceId = request.ServiceId;

                await _discount_CardService.UpdateDiscount_Card(discount_Card);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Discount_Cards
        [ActionName("AddDiscount_Card")]
        [HttpPost]
        public async Task<object> AddDiscount_Card(Discount_CardRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            var checkValidateCard = await _discount_CardService.CountDiscount_CardByNumberCard(request.Id, request.NumberCard);

            if (checkValidateCard)
            {
                return JsonResults.Error(302, "This discount card is already in the database");
            }

            try
            {
                var model = new Discount_Cards
                {
                    UserId = request.UserId,
                    NumberCard = request.NumberCard,
                    ServiceId = request.ServiceId
                };

                await _discount_CardService.AddDiscount_Card(model);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // DELETE: api/Discount_Cards/5
        [ActionName("DeleteDiscount_Card")]
        [HttpDelete]
        public async Task<object> DeleteDiscount_Card(int id)
        {
            var discount_Card = await _discount_CardService.GetDiscount_Card(id);

            if (discount_Card == null)
            {
                return JsonResults.Error();
            }

            await _discount_CardService.DeleteDiscount_Card(discount_Card);

            return JsonResults.Success();
        }

        // Util
        private Discount_CardViewModel GetDiscount_CardModel(Discount_Cards discount_Card)
        {
            var model = new Discount_CardViewModel
            {
                Id = discount_Card.Id,
                ServiceId = discount_Card.ServiceId,
                NumberCard = discount_Card.NumberCard,
                ServiceName = discount_Card.Service.Name
            };

            return model;
        }
    }
}
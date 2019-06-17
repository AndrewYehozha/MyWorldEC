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

namespace WebApplication.Controllers
{
    [Authorize]
    public class Bonus_PointController : ApiController
    {
        private Bonus_PointService _bonus_PointService = new Bonus_PointService();

        // GET: api/Bonus_Points
        [ActionName("GetBonus_Points")]
        [HttpGet]
        public async Task<object> GetBonus_Points()
        {
            var bonus_Points = await _bonus_PointService.GetBonus_Points();

            if (bonus_Points == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Tickets not found");
            }

            var models = new List<Bonus_PointViewModel>();

            foreach (var bonus_Point in bonus_Points)
            {
                models.Add(GetBonus_PointsModel(bonus_Point));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Bonus_Points/5
        [ActionName("GetBonus_Point")]
        [HttpGet]
        public async Task<object> GetBonus_Point(int id)
        {
            var bonus_Point = await _bonus_PointService.GetBonus_Point(id);

            if (bonus_Point == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Bonus_Point not found");
            }

            var model = GetBonus_PointsModel(bonus_Point);

            return JsonResults.Success(model);
        }

        // GET: api/Bonus_Points
        [ActionName("GetBonus_PointsByUserId")]
        [HttpGet]
        public async Task<object> GetBonus_PointsByUserId(int id)
        {
            var bonus_Points = await _bonus_PointService.GetBonus_PointsByUserId(id);

            if (bonus_Points == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Tickets not found");
            }

            var models = new List<Bonus_PointViewModel>();

            foreach (var bonus_Point in bonus_Points)
            {
                models.Add(GetBonus_PointsModel(bonus_Point));
            }

            return JsonResults.Success(models);
        }

        // Util
        private Bonus_PointViewModel GetBonus_PointsModel(Bonus_Points bonus_Points)
        {
            var model = new Bonus_PointViewModel
            {
                Id = bonus_Points.Id,
                ServiceId = bonus_Points.ServiceId,
                UserId = bonus_Points.UserId,
                DateOfUse = bonus_Points.DateOfUse,
                Amount = bonus_Points.Amount
            };

            return model;
        }
    }
}
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
    public class RatingsController : ApiController
    {
        RatingService _ratingService = new RatingService();

        // GET: api/Ratings
        [ActionName("GetRatings")]
        [HttpGet]
        public async Task<object> GetRatings()
        {
            var ratings = await _ratingService.GetRatings();

            if(ratings == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Ratings not found");
            }

            List<RatingViewModel> models = new List<RatingViewModel>();

            foreach (var rating in ratings)
            {
                models.Add(GetRatingModel(rating));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Ratings/5
        [ActionName("GetRating")]
        [HttpGet]
        public async Task<object> GetRating(int id)
        {
            var rating = await _ratingService.GetRating(id);

            if (rating == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Rating not found");
            }

            var model = GetRatingModel(rating);

            return JsonResults.Success(model);
        }

        // PUT: api/Ratings/5
        [ActionName("AddEditRating")]
        [HttpPost]
        public async Task<object> AddEditRating(RatingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var checkRating = await _ratingService.CheckRating(request);

                if (!checkRating)
                {
                    var model = new Rating
                    {
                        UserId = request.UserId,
                        ServiceId = request.ServiceId,
                        Rating1 = request.Rating1
                    };

                    await _ratingService.AddRating(model);

                    return JsonResults.Success(model);
                }

                var rating = await _ratingService.GetRating(request.Id);

                rating.Rating1 = request.Rating1;

                await _ratingService.UpdateRating(rating);

                return JsonResults.Success(rating);
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // Util
        private RatingViewModel GetRatingModel(Rating rating)
        {
            var model = new RatingViewModel
            {
                Id = rating.Id,
                ServiceId = rating.ServiceId,
                UserId = rating.UserId,
                Rating1 = rating.Rating1
            };

            return model;
        }
    }
}
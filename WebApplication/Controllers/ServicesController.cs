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
    public class ServicesController : ApiController
    {
        private ServicesService _servicesService = new ServicesService();
        private RatingService _ratingService = new RatingService();

        // GET: api/Services
        [ActionName("GetServices")]
        [HttpGet]
        public async Task<object> GetServices()
        {
            var services = await _servicesService.GetServices();

            if (services == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Services not found");
            }

            var models = new List<ServicesViewModel>();

            foreach (var service in services)
            {
                models.Add(GetServiceModel(service));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Services/5
        [ActionName("GetService")]
        [HttpGet]
        public async Task<object> GetService(int id)
        {
            var service = await _servicesService.GetService(id);

            if (service == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Service not found");
            }

            var model = GetServiceModel(service);

            return JsonResults.Success(model);
        }

        // GET: api/Services/5
        [ActionName("GetRecommendationService")]
        [HttpGet]
        public async Task<object> GetRecommendationService(int accountId, bool IsParking, bool IsChildrens, bool IsDiscCards)
        {
            var services = await _servicesService.GetRecommendationServices(accountId, IsParking, IsChildrens, IsDiscCards);

            if (services == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Services not found");
            }

            var models = new List<ServicesViewModel>();

            foreach (var service in services)
            {
                models.Add(await GetServiceModelWithRatings(service, accountId));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Services/5
        [ActionName("GetServiceByECenterId")]
        [HttpGet]
        public async Task<object> GetServiceByECenterId(int id)
        {
            var services = await _servicesService.GetServiceByECenterId(id);

            if (services == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Service not found");
            }

            var models = new List<ServicesViewModel>();

            foreach (var service in services)
            {
                models.Add(GetServiceModel(service));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Services/5
        [ActionName("ServiceView")]
        [HttpGet]
        public async Task<object> ServiceView(int id, int userId)
        {
            var service = await _servicesService.GetService(id);
            var categories = await _servicesService.GetCategoryToService(id);

            if (service == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Service not found");
            }

            var ratingAvg = await _ratingService.AvgRating(service.Id);
            var userRating = await _ratingService.GetRatingByuserId(service.Id, userId);

            if (ratingAvg == null)
            {
                ratingAvg = 0;
            }

            if (userRating == null)
            {
                userRating = 0;
            }

            var categoriesView = new List<CategoryViewModel>();

            foreach (var item in categories)
            {
                categoriesView.Add(new CategoryViewModel {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            var model = new ServiceCategoryViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Categories = categoriesView,
                Cost = service.Cost,
                Floor = service.Floor,
                Hall = service.Hall,
                AgeFrom = service.AgeFrom,
                Rating = ratingAvg != null ? (decimal)ratingAvg : 0,
                userRating = userRating
            };

            return JsonResults.Success(model);
        }

        // GET: api/Services/5
        [ActionName("GetServiceNoExistToDiscountCardByUserId")]
        [HttpGet]
        public async Task<object> GetServiceNoExistToDiscountCardByUserId(int id)
        {
            var services = await _servicesService.GetServiceNoExistToDiscountCardByUserId(id);

            if (services == null)
            {
                return JsonResults.Error();
            }

            var models = new List<ServicesViewModel>();

            foreach (var service in services)
            {
                models.Add(GetServiceModel(service));
            }

            return JsonResults.Success(models);
        }

        // PUT: api/Services/5
        [ActionName("EditService")]
        [HttpPost]
        public async Task<object> EditService(ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var service = await _servicesService.GetService(request.Id);

                if (service == null)
                {
                    return JsonResults.Error(404, "It`s service not found");
                }

                service.Name = request.Name;
                service.Description = request.Description;
                service.Cost = request.Cost;
                service.Floor = request.Floor;
                service.Hall = request.Hall;
                service.AgeFrom = request.AgeFrom;

                await _servicesService.UpdateService(service);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Services
        [ActionName("AddService")]
        [HttpPost]
        public async Task<object> AddService(ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var model = new Service
                {
                    Name = request.Name,
                    Description = request.Description,
                    Cost = request.Cost,
                    Floor = request.Floor,
                    Hall = request.Hall,
                    AgeFrom = request.AgeFrom
                };

                await _servicesService.AddService(model);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Services
        [ActionName("AddCategoryToService")]
        [HttpPost]
        public async Task<object> AddCategoryToService(Categories_Services request)
        {
            try
            {
                var isCategoryContain = await _servicesService.CheckCategoryInServices((int)request.IdCategories, (int)request.IdServices);

                if (isCategoryContain)
                {
                    return JsonResults.Error(406, "This service already has this category.");
                }

                await _servicesService.AddCategoryToService(request);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }
        
        // DELETE: api/Services/5
        [ActionName("DeleteService")]
        [HttpDelete]
        public async Task<object> DeleteService(int id)
        {
            var service = await _servicesService.GetService(id);

            if (service == null)
            {
                return JsonResults.Error();
            }

            await _servicesService.DeleteService(service);

            return JsonResults.Success();
        }

        // Util
        private ServicesViewModel GetServiceModel(Service service)
        {
            var model = new ServicesViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Cost = service.Cost,
                Floor = service.Floor,
                Hall = service.Hall,
                AgeFrom = service.AgeFrom
            };

            return model;
        }

        private async Task<ServicesViewModel> GetServiceModelWithRatings(Service service, int userId)
        {
            var categories = await _servicesService.GetCategoryToService(service.Id);
            var ratingAvg = await _ratingService.AvgRating(service.Id);
            var userRating = await _ratingService.GetRatingByuserId(service.Id, userId);
            var categoriesView = new List<CategoryViewModel>();

            foreach (var item in categories)
            {
                categoriesView.Add(new CategoryViewModel
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            var model = new ServiceCategoryViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Categories = categoriesView,
                Cost = service.Cost,
                Floor = service.Floor,
                Hall = service.Hall,
                AgeFrom = service.AgeFrom,
                Rating = ratingAvg != null ? (decimal)ratingAvg : 0,
                userRating = userRating
            };

            return model;
        }
    }
}
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
    public class ServicesController : ApiController
    {
        ServicesService _servicesService = new ServicesService();

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

            List<ServicesViewModel> models = new List<ServicesViewModel>();

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
        [ActionName("GetServiceByECenterId")]
        [HttpGet]
        public async Task<object> GetServiceByECenterId(int idEC)
        {
            var services = await _servicesService.GetServiceByECenterId(idEC);

            if (services == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Service not found");
            }

            List<ServicesViewModel> models = new List<ServicesViewModel>();

            foreach (var service in services)
            {
                models.Add(GetServiceModel(service));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Services/5
        [ActionName("ServiceView")]
        [HttpGet]
        public async Task<object> ServiceView(int idService)
        {
            var service = await _servicesService.GetService(idService);
            var categories = await _servicesService.GetCategoryToService(idService);

            if (service == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Service not found");
            }

            var model = new ServiceCategoryViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Categories = categories
            };

            return JsonResults.Success(model);
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
                    JsonResults.Error(404, "It`s service not found");
                }

                service.Name = request.Name;
                service.Description = request.Description;

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
                    Description = request.Description
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
                Description = service.Description
            };

            return model;
        }
    }
}
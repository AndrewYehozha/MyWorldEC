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
        ServicesService _servicesService;

        ServicesController(ServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        // GET: api/Services
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

        // PUT: api/Services/5
        [HttpPost]
        public async Task<object> EditService(ServiceRequest request)
        {
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
        [HttpPost]
        public async Task<object> AddService(ServiceRequest request)
        {
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

        // DELETE: api/Services/5
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
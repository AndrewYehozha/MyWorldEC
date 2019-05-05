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
    public class Entertainment_CentersController : ApiController
    {
        private EntertainmentCenterService _entertainmentCenterService = new EntertainmentCenterService();

        // GET: api/Entertainment_Centers
        [ActionName("GetEntertainment_Centers")]
        [HttpGet]
        public async Task<object> GetEntertainment_Centers()
        {
            var entertainmentCenters = await _entertainmentCenterService.GetEntertainment_Centers();

            if (entertainmentCenters == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Entertainment Centers not found");
            }

            List<Entertainment_CentersViewModel> models = new List<Entertainment_CentersViewModel>();

            foreach (var entertainmentCenter in entertainmentCenters)
            {
                models.Add(GetEntertainmentCenterModel(entertainmentCenter));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Entertainment_Centers/5
        [ActionName("GetEntertainment_Center")]
        [HttpGet]
        public async Task<object> GetEntertainment_Center(int id)
        {
            var entertainmentCenter = await _entertainmentCenterService.GetEntertainment_Center(id);

            if (entertainmentCenter == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: $"Entertainment Center with id [{id}] not found");
            }

            var model = GetEntertainmentCenterModel(entertainmentCenter);

            return JsonResults.Success(model);
        }

        // PUT: api/Entertainment_Centers/5
        [ActionName("EditEntertainment_Center")]
        [HttpPost]
        public async Task<object> EditEntertainment_Center(Entertainment_CentersRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var entertainment_Center = await _entertainmentCenterService.GetEntertainment_Center(request.Id);

                if (entertainment_Center == null)
                {
                    return JsonResults.Error(404, "It`s entertainment center not found");
                }

                entertainment_Center.Name = request.Name;
                entertainment_Center.Owner = request.Owner;
                entertainment_Center.Address = request.Address;
                entertainment_Center.Phone = request.Phone;
                entertainment_Center.Email = request.Email;
                entertainment_Center.IsParking = request.IsParking;

                await _entertainmentCenterService.UpdateEntertainment_Center(entertainment_Center);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Entertainment_Centers
        [ActionName("AddEntertainment_Center")]
        [HttpPost]
        public async Task<object> AddEntertainment_Center(Entertainment_CentersRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var model = new Entertainment_Centers
                {
                    Name = request.Name,
                    Owner = request.Owner,
                    Address = request.Address,
                    Phone = request.Phone,
                    Email = request.Email,
                    IsParking = request.IsParking
                };

                await _entertainmentCenterService.AddEntertainment_Center(model);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        [ActionName("AddServiceToEntertainment_Center")]
        [HttpPost]
        public async Task<object> AddServiceToEntertainment_Center(Services_Entertainment_Centers request)
        {
            try
            {
                var isServiceContain = await _entertainmentCenterService.CheckServicesInEC(request.Entertainment_CenterId, request.ServiceId);

                if (isServiceContain)
                {
                    return JsonResults.Error(406, "This entertainment center already has this service.");
                }

                await _entertainmentCenterService.AddServiceInEntertainment_Center(request);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // DELETE: api/Entertainment_Centers/5
        [ActionName("DeleteEntertainment_Center")]
        [HttpDelete]
        public async Task<object> DeleteEntertainment_Center(int id)
        {
            var entertainment_Centers = await _entertainmentCenterService.GetEntertainment_Center(id);

            if (entertainment_Centers == null)
            {
                return JsonResults.Error();
            }

            await _entertainmentCenterService.DeleteEntertainment_Centers(entertainment_Centers);

            return JsonResults.Success();
        }

        // Util
        private Entertainment_CentersViewModel GetEntertainmentCenterModel(Entertainment_Centers entertainment_Center)
        {
            var model = new Entertainment_CentersViewModel
            {
                Id = entertainment_Center.Id,
                Name = entertainment_Center.Name,
                Owner = entertainment_Center.Owner,
                Address = entertainment_Center.Address,
                Phone = entertainment_Center.Phone,
                Email = entertainment_Center.Email,
                IsParking = (bool)entertainment_Center.IsParking
            };

            return model;
        }
    }
}
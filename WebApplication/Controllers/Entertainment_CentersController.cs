﻿using System;
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

            var models = new List<Entertainment_CentersViewModel>();

            foreach (var entertainmentCenter in entertainmentCenters)
            {
                models.Add(GetEntertainmentCenterModel(entertainmentCenter));
            }

            return JsonResults.Success(models);
        }

        [ActionName("GetServicesToEC")]
        [HttpGet]
        public async Task<object> GetServicesToEC()
        {
            var servicesToEC = await _entertainmentCenterService.GetServicesToEC();

            if (servicesToEC == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Services in Malls not found");
            }

            var model = new List<ServicesToECResponse>();

            foreach (var serviceToEC in servicesToEC)
            {
                model.Add(new ServicesToECResponse
                {
                    Id = serviceToEC.Id,
                    Entertainment_CenterId = serviceToEC.Entertainment_CenterId,
                    ServiceId = serviceToEC.ServiceId,
                    Entertainment_Centers = (new Entertainment_CentersViewModel
                    {
                        Id = serviceToEC.Entertainment_Centers.Id,
                        Name = serviceToEC.Entertainment_Centers.Name,
                        Address = serviceToEC.Entertainment_Centers.Address,
                        Email = serviceToEC.Entertainment_Centers.Email,
                        IsParking = serviceToEC.Entertainment_Centers.IsParking,
                        Owner = serviceToEC.Entertainment_Centers.Owner,
                        Phone = serviceToEC.Entertainment_Centers.Phone
                    }),
                    Service = (new ServicesViewModel
                    {
                        Id = serviceToEC.Service.Id,
                        AgeFrom = serviceToEC.Service.AgeFrom,
                        Cost = serviceToEC.Service.Cost,
                        Description = serviceToEC.Service.Description,
                        Floor = serviceToEC.Service.Floor,
                        Hall = serviceToEC.Service.Hall,
                        Name = serviceToEC.Service.Name
                    })
                });
            }

            return JsonResults.Success(model);
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

        [ActionName("GetServiceToEC")]
        [HttpGet]
        public async Task<object> GetServiceToEC(int id)
        {
            var serviceToEC = await _entertainmentCenterService.GetServiceToEC(id);

            if (serviceToEC == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Service in Mall not found");
            }

            var model = new ServicesToECResponse
            {
                Id = serviceToEC.Id,
                Entertainment_CenterId = serviceToEC.Entertainment_CenterId,
                ServiceId = serviceToEC.ServiceId,
                Entertainment_Centers = (new Entertainment_CentersViewModel
                {
                    Id = serviceToEC.Entertainment_Centers.Id,
                    Name = serviceToEC.Entertainment_Centers.Name,
                    Address = serviceToEC.Entertainment_Centers.Address,
                    Email = serviceToEC.Entertainment_Centers.Email,
                    IsParking = serviceToEC.Entertainment_Centers.IsParking,
                    Owner = serviceToEC.Entertainment_Centers.Owner,
                    Phone = serviceToEC.Entertainment_Centers.Phone
                }),
                Service = (new ServicesViewModel
                {
                    Id = serviceToEC.Service.Id,
                    AgeFrom = serviceToEC.Service.AgeFrom,
                    Cost = serviceToEC.Service.Cost,
                    Description = serviceToEC.Service.Description,
                    Floor = serviceToEC.Service.Floor,
                    Hall = serviceToEC.Service.Hall,
                    Name = serviceToEC.Service.Name
                })
            };

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

        [ActionName("EditServiceToEC")]
        [HttpPost]
        public async Task<object> EditServiceToEC(Services_Entertainment_Centers request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var entertainment_Center = await _entertainmentCenterService.GetServiceToEC(request.Id);

                if (entertainment_Center == null)
                {
                    return JsonResults.Error(404, "It`s Service in EC not found");
                }

                entertainment_Center.ServiceId = request.ServiceId;

                await _entertainmentCenterService.UpdateServiceToEC(entertainment_Center);

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
        public async Task<object> AddServiceToEntertainment_Center(Services_Entertainment_CentersRequest request)
        {
            try
            {
                var isServiceContain = await _entertainmentCenterService.CheckServicesInEC(request.Entertainment_CenterId, request.ServiceId);

                if (isServiceContain)
                {
                    return JsonResults.Error(406, "This entertainment center already has this service.");
                }

                var model = new Services_Entertainment_Centers
                {
                    Entertainment_CenterId = request.Entertainment_CenterId,
                    ServiceId = request.ServiceId
                };

                await _entertainmentCenterService.AddServiceInEntertainment_Center(model);

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

        [ActionName("DeleteServiceToEC")]
        [HttpDelete]
        public async Task<object> DeleteServiceToEC(int id)
        {
            var entertainment_Centers = await _entertainmentCenterService.GetServiceToEC(id);

            if (entertainment_Centers == null)
            {
                return JsonResults.Error();
            }

            await _entertainmentCenterService.DeleteServiceToEC(entertainment_Centers);

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
                IsParking = entertainment_Center.IsParking
            };

            return model;
        }
    }
}
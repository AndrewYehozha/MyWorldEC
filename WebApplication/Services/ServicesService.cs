using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class ServicesService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();

        public async Task<IEnumerable<Service>> GetServices()
        {
            var services = await db.Services.ToListAsync();
            return services;
        }
        
        public async Task<Service> GetService(int id)
        {
            var service = await db.Services.FindAsync(id);
            return service;
        }

        public async Task<List<Service>> GetRecommendationServices(int accountId, bool IsParking, bool IsChildrens, bool IsDiscCards)
        {
            var paymentsServices = db.Payments.Where(x => x.UserId == accountId).Select(x => x.ServiceId.Value).Distinct().ToList();
            var categoriesServices = db.Categories.Join(db.Categories_Services, C => C.Id, CS => CS.IdServices, (C, CS) => new { CS.IdCategories, CS.IdServices.Value }).Where(x => paymentsServices.Contains(x.Value)).Select(x => x.IdCategories).Distinct().ToList();
            var recommendationServices = db.Services.Join(db.Categories_Services, S => S.Id, C => C.IdServices, (S, C) => new { S.Id, S.Name, S.Description, S.Cost, S.AgeFrom, S.Floor, S.Hall, S.Ratings, C.IdServices.Value }).Where(x => categoriesServices.Contains(x.Value));

            if (IsParking)
            {
               var parkingList = await db.Services.Join(db.Services_Entertainment_Centers, S => S.Id, SEC => SEC.ServiceId, (S, SEC) => new { S.Id, SEC.Entertainment_CenterId.Value })
                    .Join(db.Entertainment_Centers, SEC => SEC.Value, EC => EC.Id, (SEC, EC) => new { SEC.Id, EC.IsParking }).Where(x => x.IsParking).Select(x => x.Id).Distinct().ToListAsync();

                recommendationServices = recommendationServices.Where(x => parkingList.Contains(x.Id));
            }

            if(IsChildrens)
            {
                var childrenAge = db.Childrens.Join(db.Users, C => C.UserId, U => U.Id, (C, U) => new {U.Id, Age = ((DateTime.UtcNow - C.DateOfBirth).Days / 365) }).Where(x => x.Id == accountId).OrderBy(x => x.Age).FirstOrDefault().Age;

                recommendationServices = recommendationServices.Where(x => x.AgeFrom < childrenAge);
            }

            if (IsDiscCards)
            {
                var servicesDiscCards = db.Services.Join(db.Discount_Cards, S => S.Id, DC => DC.ServiceId, (S, DC) => new { ServiceID = S.Id, DC.UserId })
                    .Join(db.Users, DC => DC.UserId, U => U.Id, (DC, U) => new { DC.ServiceID, U.Id }).Where(x => x.Id == accountId).Select(x => x.ServiceID).Distinct();

                recommendationServices = recommendationServices.Where(x => servicesDiscCards.Contains(x.Id));
            }

            recommendationServices = recommendationServices.OrderByDescending(x => x.Ratings.Average(a => a.Rating1));

            var model = new List<Service>();

            foreach(var service in recommendationServices)
            {
                model.Add(new Service {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    Cost = service.Cost,
                    AgeFrom = service.AgeFrom,
                    Hall = service.Hall,
                    Floor = service.Floor
                });
            }

            return model.DistinctBy(x => x.Id).ToList();
        }

        public async Task<IEnumerable<Service>> GetServiceByECenterId(int idEC)
        {
            var serviceIds = await db.Services_Entertainment_Centers.Where(m => m.Entertainment_CenterId == idEC).Select(m => m.ServiceId).ToListAsync();

            var service = db.Services.Where(m => serviceIds.Contains(m.Id)).AsEnumerable();

            return service;
        }

        public async Task<IEnumerable<Category>> GetCategoryToService(int idService)
        {
            var categoriesIds = await db.Categories_Services.Where(m => m.IdServices == idService).Select(m => m.IdCategories).ToListAsync();

            var categories = db.Categories.Where(m => categoriesIds.Contains(m.Id)).AsEnumerable();

            return categories;
        }

        public async Task<IEnumerable<Service>> GetServiceNoExistToDiscountCardByUserId(int userId)
        {
            var discountCardsIds = await db.Discount_Cards.Where(m => m.UserId == userId).Select(m => m.ServiceId).ToListAsync();

            var service = db.Services.Where(m => !(discountCardsIds.Contains(m.Id))).AsEnumerable();

            return service;
        }

        public async Task UpdateService(Service service)
        {
            db.Entry(service).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Service> AddService(Service _service)
        {
            var service = db.Services.Add(_service);
            await db.SaveChangesAsync();

            return service;
        }

        public async Task AddCategoryToService(Categories_Services categories_Services)
        {
            var service = db.Categories_Services.Add(categories_Services);
            await db.SaveChangesAsync();
        }

        public async Task DeleteService(Service service)
        {
            db.Services.Remove(service);
            await db.SaveChangesAsync();
        }

        public async Task<bool> CheckCategoryInServices(int idCategorie, int idService)
        {
            var result = await db.Categories_Services.Where(m => m.IdCategories == idCategorie && m.IdServices == idService).FirstOrDefaultAsync();

            return result != null;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            this.Dispose(disposing);
        }
    }
}
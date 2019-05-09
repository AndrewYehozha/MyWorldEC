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
    }
}
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
        private MyWorldECEntities2 db = new MyWorldECEntities2();

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

        public async Task DeleteService(Service service)
        {
            db.Services.Remove(service);
            await db.SaveChangesAsync();
        }

        private bool ServiceExists(int id)
        {
            return db.Services.Count(e => e.Id == id) > 0;
        }
    }
}
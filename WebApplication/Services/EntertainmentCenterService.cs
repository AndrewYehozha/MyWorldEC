﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class EntertainmentCenterService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();
        
        public async Task<IEnumerable<Entertainment_Centers>> GetEntertainment_Centers()
        {
            var entertainment_Centers = await db.Entertainment_Centers.ToListAsync();

            return entertainment_Centers;
        }

        public async Task<IEnumerable<Services_Entertainment_Centers>> GetServicesToEC()
        {
            var servicesToEC = await db.Services_Entertainment_Centers.Include(c => c.Entertainment_Centers).Include(c => c.Service).ToListAsync();

            return servicesToEC;
        }

        public async Task<Services_Entertainment_Centers> GetServiceToEC(int id)
        {
            var servicesToEC = await db.Services_Entertainment_Centers.FindAsync(id);

            return servicesToEC;
        }

        public async Task<Entertainment_Centers> GetEntertainment_Center(int id)
        {
            var entertainment_Center = await db.Entertainment_Centers.FindAsync(id);
            
            return entertainment_Center;
        }

        public async Task UpdateEntertainment_Center(Entertainment_Centers entertainment_Center)
        {
            db.Entry(entertainment_Center).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task UpdateServiceToEC(Services_Entertainment_Centers serviceToEC)
        {
            db.Entry(serviceToEC).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Entertainment_Centers> AddEntertainment_Center(Entertainment_Centers entertainment_Center)
        {
            var result = db.Entertainment_Centers.Add(entertainment_Center);
            await db.SaveChangesAsync();

            return result;
        }

        public async Task AddServiceInEntertainment_Center(Services_Entertainment_Centers services_Entertainment_Centers)
        {
            var result = db.Services_Entertainment_Centers.Add(services_Entertainment_Centers);
            await db.SaveChangesAsync();
        }

        public async Task<bool> CheckServicesInEC(int idEC, int idService)
        {
            var result = await db.Services_Entertainment_Centers.Where(m => m.Entertainment_CenterId == idEC && m.ServiceId == idService).FirstOrDefaultAsync();

            return result != null;
        }

        public async Task DeleteEntertainment_Centers(Entertainment_Centers entertainment_Center)
        {
            db.Entertainment_Centers.Remove(entertainment_Center);
            await db.SaveChangesAsync();
        }

        public async Task DeleteServiceToEC(Services_Entertainment_Centers serviceToEC)
        {
            db.Services_Entertainment_Centers.Remove(serviceToEC);
            await db.SaveChangesAsync();
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
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class ChildrenService
    {
        private MyWorldECEntities2 db = new MyWorldECEntities2();

        public async Task<IEnumerable<Children>> GetChildrens()
        {
            var childrens = await db.Childrens.ToListAsync();

            return childrens;
        }

        public async Task<IEnumerable<Children>> GetChildrensByUserId(int id)
        {
            var childrens = await db.Childrens.Where(x => x.UserId == id).ToListAsync();

            return childrens;
        }

        public async Task<Children> GetChildren(int id)
        {
            var children = await db.Childrens.FindAsync(id);

            return children;
        }

        public async Task UpdateChildren(Children children)
        {
            db.Entry(children).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Children> AddChildren(Children children)
        {
            var result = db.Childrens.Add(children);
            await db.SaveChangesAsync();

            return result;
        }

        public async Task DeleteChildren(Children children)
        {
            db.Childrens.Remove(children);
            await db.SaveChangesAsync();
        }

        private bool ChildrenExists(int id)
        {
            return db.Childrens.Count(e => e.Id == id) > 0;
        }
    }
}
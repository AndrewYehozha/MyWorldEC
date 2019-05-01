using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class UserService
    {
        private MyWorldECEntities db = new MyWorldECEntities();

        public IEnumerable<User> GetUsers()
        {
            return db.Users.ToList();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await db.Users.FindAsync(id);

            return user;
        }

        public async Task UpdateUser(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<User> AddUser(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(User user)
        {
            db.Users.Remove(user);

            await db.SaveChangesAsync();
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}
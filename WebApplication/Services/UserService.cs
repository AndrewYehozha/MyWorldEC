using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class UserService
    {
        private MyWorldECEntities db = new MyWorldECEntities();

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await db.Users.ToListAsync();

            return users;
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
            var result = db.Users.Add(user);
            await db.SaveChangesAsync();

            return result;
        }

        public async Task DeleteUser(User user)
        {
            db.Users.Remove(user);

            await db.SaveChangesAsync();
        }

        public async Task<bool> CheckUserByEmailAsync(string email)
        {
            var user = await SearchAuthorizationUserAsync(email);

            return user != null;
        }

        public async Task<User> SearchAuthorizationUserAsync(string email)
        {
            var user = await db.Users.Where(m => m.Email == email).SingleOrDefaultAsync();

            return user;
        }

        public bool CheckUserCorrectPassword(string enteredPassword, string hashUserPassword)
        {
            return HashPassword(enteredPassword) == hashUserPassword ? true : false;
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }

        // Util
        public string HashPassword(string password)
        {
            var bytes = Encoding.Unicode.GetBytes(password);

            var CSP = new MD5CryptoServiceProvider();

            var byteHash = CSP.ComputeHash(bytes);

            var hash = new StringBuilder();

            foreach (byte b in byteHash)
            {
                hash.Append(string.Format("{0:x2}", b));
            }

            return hash.ToString();
        }
    }
}
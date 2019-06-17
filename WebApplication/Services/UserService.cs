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
using WebApplication.Models.Request;

namespace WebApplication.Services
{
    public class UserService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();

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

        public async Task ChangePassword(string newPassword, User user)
        {
            user.Password = HashPassword(newPassword);
            await db.SaveChangesAsync();
        }

        public async Task<bool> CheckUserByEmailAsync(string email)
        {
            var user = await db.Users.Where(m => m.Email == email).FirstOrDefaultAsync();

            return user != null;
        }

        public async Task<User> SearchAuthorizationUserAsync(string email)
        {
            var user = await db.Users.Where(m => m.Email == email || m.Phone == email).FirstOrDefaultAsync();

            return user;
        }

        public bool CheckUserCorrectPassword(string enteredPassword, string hashUserPassword)
        {
            return HashPassword(enteredPassword) == hashUserPassword ? true : false;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            this.Dispose(disposing);
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
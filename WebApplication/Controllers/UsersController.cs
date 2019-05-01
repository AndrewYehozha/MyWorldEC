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
using Newtonsoft.Json;
using System.Web.Http.Results;
using WebApplication.Services;
using WebApplication.Models.Response;

namespace WebApplication.Controllers
{
    public class UsersController : ApiController
    {
        private UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<object> GetUsers()
        {
            var users = _userService.GetUsers();

            if (users == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Users not found");
            }

            List<UserViewModel> models = new List<UserViewModel>();

            foreach (var user in users)
            {
                models.Add(GetUserModel(user));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Users/5
        [HttpGet]
        public async Task<object> GetUser(int id)
        {
            var user = await _userService.GetUser(id);

            if (user == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "User not found");
            }

            var model = GetUserModel(user);

            return JsonResults.Success(model);
        }

        // PUT: api/Users/5
        [HttpPost]
        public async Task<object> EditUser(User user)
        {
            try
            {
                var users = await _userService.GetUser(user.Id);

                if (users == null)
                {
                    JsonResults.Error(404, "It`s user not found");
                }

                users.FirstName = user.FirstName;
                users.LastName = user.LastName;
                users.Country = user.Country;
                users.City = user.City;
                users.Address = user.Address;
                users.Phone = user.Phone;
                users.Email = user.Email;
                users.Birsday = user.Birsday;

                await _userService.UpdateUser(user);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete]
        public async Task<object> DeleteUser(int id)
        {
            User user = await _userService.GetUser(id);

            await _userService.DeleteUser(user);

            return JsonResults.Success();
        }

        // Util
        private UserViewModel GetUserModel(User user)
        {
            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                City = user.City,
                Address = user.Address,
                Phone = user.Phone,
                Email = user.Email,
                Birsday = user.Birsday,
                DateRegistered = user.DateRegistered,
                IsBlocked = user.IsBlocked,
                IsAdministration = user.IsAdministration,
                BonusScore = user.BonusScore
            };

            return model;
        }
    }
}
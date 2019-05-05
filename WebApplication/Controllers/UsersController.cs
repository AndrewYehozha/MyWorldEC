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
using WebApplication.Models.Request;

namespace WebApplication.Controllers
{
    public class UsersController : ApiController
    {
        private UserService _userService = new UserService();

        // GET: api/Users/GetUsers
        [ActionName("GetUsers")]
        [HttpGet]
        public async Task<object> GetUsers()
        {
            var users = await _userService.GetUsers();

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

        // GET: api/Users/GetUser/5
        [ActionName("GetUser")]
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

        //[HttpGet]
        //public async Task<object> ViewUserProfile(int id)
        //{
        //    var user = await _userService.GetUser(id);

        //    if (user == null)
        //    {
        //        return JsonResults.Error(errorNum: 404, errorMessage: "User not found");
        //    }

        //    var model = GetUserModel(user);

        //    return JsonResults.Success(model);
        //}

        // PUT: api/Users/EditUser
        [ActionName("EditUser")]
        [HttpPost]
        public async Task<object> EditUser(UserEditRequest user)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

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

                await _userService.UpdateUser(users);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Users/Registration
        [ActionName("Registration")]
        [HttpPost]
        public async Task<object> Registration(UserRegistredRequest model)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var isUserContain = await _userService.CheckUserByEmailAsync(model.Email);

                if (isUserContain)
                {
                    return JsonResults.Error(401, "User is already registered");
                }

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = _userService.HashPassword(model.Password),
                    IsAdministration = false,
                    IsBlocked = false,
                    DateRegistered = DateTime.Now.Date,
                    BonusScore = 0
                };

                var newUser = await _userService.AddUser(user);

                return JsonResults.Success(new { newUser.Id});
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        [ActionName("Authorization")]
        [HttpPost]
        public async Task<object> Authorization(AuthorizationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            var user = await _userService.SearchAuthorizationUserAsync(model.Email);

            if (user == null)
            {
                return JsonResults.Error(402, "User not registered");
            }

            if (user.IsBlocked == true)
            {
                return JsonResults.Error(403, "The user is blocked");
            }

            try
            {
                var isCorrectPassword = _userService.CheckUserCorrectPassword(model.Password, user.Password);

                if (!isCorrectPassword)
                {
                    return JsonResults.Error(405, "Incorrect password");
                }

                return JsonResults.Success(new {user.Id, user.IsAdministration});
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // DELETE: api/Users/5
        [ActionName("DeleteUser")]
        [HttpDelete]
        public async Task<object> DeleteUser(int id)
        {
            User user = await _userService.GetUser(id);

            if(user == null)
            {
                JsonResults.Error();
            }

            await _userService.DeleteUser(user);

            return JsonResults.Success();
        }

        // Utils
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
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

    [Authorize]
    public class UsersController : ApiController
    {
        private UserService _userService = new UserService();
        private ChildrenService _childrenService = new ChildrenService();
        private Discount_CardService _discount_Card = new Discount_CardService();
        private PreferenceService _preferenceService = new PreferenceService();

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

            var models = new List<UserViewModel>();

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

        [ActionName("ViewUserProfile")]
        [HttpGet]
        public async Task<object> ViewUserProfile(int id)
        {
            var user = await _userService.GetUser(id);

            if (user == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "User not found");
            }

            var childrens = await _childrenService.GetChildrensByUserId(user.Id);
            var modelChildrens = new List<ChildrenViewModel>();

            foreach (var children in childrens)
            {
                modelChildrens.Add(new ChildrenViewModel {
                    Id = children.Id,
                    FirstName = children.FirstName,
                    LastName = children.LastName,
                    DateOfBirth = (DateTime)children.DateOfBirth
                });
            }

            var discountCards = await _discount_Card.GetDiscount_CardsByUserID(user.Id);
            var modelDiscountCards = new List<Discount_CardViewModel>();

            foreach (var discount_Card in discountCards)
            {
                modelDiscountCards.Add(new Discount_CardViewModel
                {
                    Id = discount_Card.Id,
                    ServiceId = discount_Card.ServiceId,
                    NumberCard = discount_Card.NumberCard,
                    ServiceName = discount_Card.Service.Name
                });
            }

            var preferences = await _preferenceService.GetPreferenceByUserId(user.Id);
            var modelPreferences = new List<PreferenceViewModel>();

            foreach (var preference in preferences)
            {
                modelPreferences.Add(new PreferenceViewModel {
                    Id = preference.Id,
                    UserId = (int)preference.UserId,
                    ServiceId = (int)preference.ServiceId,
                    ServiceName = preference.Service.Name
                });
            }

            var model = new UserViewProfile
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
                BonusScore = user.BonusScore,
                Childrens = modelChildrens,
                DiscountCards = modelDiscountCards,
                Preferences = modelPreferences
            };

            return JsonResults.Success(model);
        }

        [ActionName("AddUser")]
        [HttpPost]
        public async Task<object> AddUser(User request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var model = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = _userService.HashPassword(request.Password),
                    IsAdministration = request.IsAdministration,
                    IsBlocked = false,
                    DateRegistered = DateTime.Now.Date,
                    BonusScore = request.BonusScore
                };

                await _userService.AddUser(model);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

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
                    return JsonResults.Error(404, "It`s user not found");
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

        [ActionName("EditUserForAdmin")]
        [HttpPost]
        public async Task<object> EditUserForAdmin(User user)
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
                    return JsonResults.Error(404, "It`s user not found");
                }

                users.FirstName = user.FirstName;
                users.LastName = user.LastName;
                users.Country = user.Country;
                users.City = user.City;
                users.Address = user.Address;
                users.Phone = user.Phone;
                users.Email = user.Email;
                users.Birsday = user.Birsday;
                users.IsBlocked = user.IsBlocked;
                users.IsAdministration = user.IsAdministration;
                users.BonusScore = user.BonusScore;

                if (!String.IsNullOrEmpty(user.Password))
                {
                    users.Password = _userService.HashPassword(user.Password);
                }

                await _userService.UpdateUser(users);

                return JsonResults.Success();
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
            var user = await _userService.GetUser(id);

            if(user == null)
            {
                JsonResults.Error();
            }

            await _userService.DeleteUser(user);

            return JsonResults.Success();
        }

        [ActionName("ChangePassword")]
        [HttpPost]
        public async Task<object> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userService.GetUser(request.IdUser);

            if (user == null)
            {
                return JsonResults.Error(401, "User is already registered");
            }

            if (_userService.HashPassword(request.OldPassword) != user.Password)
            {
                return JsonResults.Error(402, "Incorrect password");
            }

            await _userService.ChangePassword(request.NewPassword, user);

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
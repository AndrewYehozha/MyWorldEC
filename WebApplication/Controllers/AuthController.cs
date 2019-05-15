using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication.Models;
using WebApplication.Models.Request;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class AuthController : ApiController
    {

        private UserService _userService = new UserService();


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

            if (user.IsBlocked)
            {
                return JsonResults.Error(403, "User is blocked");
            }

            try
            {
                var isCorrectPassword = _userService.CheckUserCorrectPassword(model.Password, user.Password);

                if (!isCorrectPassword)
                {
                    return JsonResults.Error(405, "Incorrect password");
                }

                string token = CreateToken($"{user.Id}.{user.Email}.{user.Password}");

                return JsonResults.Success(new { Token = token, UserId = user.Id, IsAdmin = user.IsAdministration });
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        [ActionName("AuthIOT")]
        [HttpPost]
        public async Task<object> AuthIOT(AuthorizationRequest model)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString();

                if (errorMessage == "The Email is required" || errorMessage == "Invalid Email")
                {
                    return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
                }

                return JsonResults.Error(402, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            var user = await _userService.SearchAuthorizationUserAsync(model.Email);

            if (user == null)
            {
                return JsonResults.Error(402, "User not registered");
            }

            if (!user.IsAdministration)
            {
                return JsonResults.Error(402, "You are not at administrator");
            }

            try
            {
                var isCorrectPassword = _userService.CheckUserCorrectPassword(model.Password, user.Password);

                if (!isCorrectPassword)
                {
                    return JsonResults.Error(402, "Incorrect password");
                }

                string token = CreateToken($"{user.Id}.{user.Email}.{user.Password}");

                return JsonResults.Success(new { Token = token});
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

                string token = CreateToken($"{newUser.Id}.{newUser.Email}.{newUser.Password}");

                return JsonResults.Success(new { Token = token, UserId = newUser.Id, IsAdmin = newUser.IsAdministration });
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        private string CreateToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);

            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "MyWorldEC", audience: "MyWorldEC",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}

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
using WebApplication.Models.Request;
using WebApplication.Models.Response;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class ChildrenController : ApiController
    {
        private ChildrenService _childrenService = new ChildrenService();

        // GET: api/Children
        [ActionName("GetChildrens")]
        [HttpGet]
        public async Task<object> GetChildrens()
        {
            var childrens = await _childrenService.GetChildrens();
            
            if(childrens == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Childrens not found");
            }

            List<ChildrenViewModel> models = new List<ChildrenViewModel>();

            foreach (var children in childrens)
            {
                models.Add(GetChildrenModel(children));
            }

            return JsonResults.Success(models);
        }

        [ActionName("GetChildrensByUserId")]
        [HttpGet]
        public async Task<object> GetChildrensByUserId(int userId)
        {
            var childrens = await _childrenService.GetChildrensByUserId(userId);

            if (childrens == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Childrens not found");
            }

            List<ChildrenViewModel> models = new List<ChildrenViewModel>();

            foreach (var children in childrens)
            {
                models.Add(GetChildrenModel(children));
            }

            return JsonResults.Success(models);
        }

        // GET: api/Children/5
        [ActionName("GetChildren")]
        [HttpGet]
        public async Task<object> GetChildren(int id)
        {
            var children = await _childrenService.GetChildren(id);

            if (children == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Childrens not found");
            }

            var model = GetChildrenModel(children);

            return JsonResults.Success(model);
        }

        // PUT: api/Children/5
        [ActionName("EditChildren")]
        [HttpPost]
        public async Task<object> EditChildren(ChildrenRequest model)
        {
            try
            {
                var children = await _childrenService.GetChildren(model.Id);

                if (children == null)
                {
                    JsonResults.Error(404, "It`s children center not found");
                }

                children.FirstName = model.FirstName;
                children.LastName = model.LastName;
                children.DateOfBirth = model.DateOfBirth;


                await _childrenService.UpdateChildren(children);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Children
        [ActionName("AddChildren")]
        [HttpPost]
        public async Task<object> AddChildren(ChildrenRequest request)
        {
            try
            {
                var model = new Children
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    UserId = request.UserId
                };

                await _childrenService.AddChildren(model);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // DELETE: api/Children/5
        [ActionName("DeleteChildren")]
        [HttpDelete]
        public async Task<object> DeleteChildren(int id)
        {
            var children = await _childrenService.GetChildren(id);

            if (children == null)
            {
                return JsonResults.Error();
            }

            await _childrenService.DeleteChildren(children);

            return JsonResults.Success();
        }

        // Util
        private ChildrenViewModel GetChildrenModel(Children children)
        {
            var model = new ChildrenViewModel
            {
                Id = children.Id,
                FirstName = children.FirstName,
                LastName = children.LastName,
                DateOfBirth = children.DateOfBirth
            };

            return model;
        }
    }
}
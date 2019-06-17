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
    [Authorize]
    public class CategoriesController : ApiController
    {
        private CategoryService _categoryService = new CategoryService();

        // GET: api/Categories
        [ActionName("GetCategories")]
        [HttpGet]
        public async Task<object> GetCategories()
        {
            var categories = await _categoryService.GetCategories();

            if (categories == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Categories not found");
            }

            var models = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                models.Add(GetCategoryModel(category));
            }

            return JsonResults.Success(models);
        }

        [ActionName("GetCategoriesServices")]
        [HttpGet]
        public async Task<object> GetCategoriesServices()
        {
            var categories = await _categoryService.GetCategories_Services();

            if (categories == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Categories in Service not found");
            }

            var model = new List<Categories_ServicesResponse>();

            foreach (var category_service in categories)
            {
                model.Add(new Categories_ServicesResponse {
                    Id = category_service.Id,
                    IdCategories = category_service.IdCategories,
                    IdServices = category_service.IdServices,
                    Category = (new CategoryViewModel {
                        Id = category_service.Category.Id,
                        Name = category_service.Category.Name
                    }),
                    Service = (new ServicesViewModel {
                        Id = category_service.Service.Id,
                        AgeFrom = category_service.Service.AgeFrom,
                        Cost = category_service.Service.Cost,
                        Description = category_service.Service.Description,
                        Floor = category_service.Service.Floor,
                        Hall = category_service.Service.Hall,
                        Name = category_service.Service.Name
                    })
                });
            }

            return JsonResults.Success(model);
        }

        [ActionName("GetCategoryService")]
        [HttpGet]
        public async Task<object> GetCategoryService(int id)
        {
            var categoryService = await _categoryService.GetCategoryService(id);

            if (categoryService == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Categories in Service not found");
            }

            var model = new Categories_ServicesResponse
            {
                    Id = categoryService.Id,
                    IdCategories = categoryService.IdCategories,
                    IdServices = categoryService.IdServices,
                    Category = (new CategoryViewModel
                    {
                        Id = categoryService.Category.Id,
                        Name = categoryService.Category.Name
                    }),
                    Service = (new ServicesViewModel
                    {
                        Id = categoryService.Service.Id,
                        AgeFrom = categoryService.Service.AgeFrom,
                        Cost = categoryService.Service.Cost,
                        Description = categoryService.Service.Description,
                        Floor = categoryService.Service.Floor,
                        Hall = categoryService.Service.Hall,
                        Name = categoryService.Service.Name
                    })
            };

            return JsonResults.Success(model);
        }

        // GET: api/Categories/5
        [ActionName("GetCategory")]
        [HttpGet]
        public async Task<object> GetCategory(int id)
        {
            var category = await _categoryService.GetCategory(id);

            if (category == null)
            {
                return JsonResults.Error(errorNum: 404, errorMessage: "Category not found");
            }

            var model = GetCategoryModel(category);

            return JsonResults.Success(model);
        }

        // GET: api/Categories/5
        [ActionName("GetCategoryNoExistToService")]
        [HttpGet]
        public async Task<object> GetCategoryNoExistToService(int id)
        {
            var categories = await _categoryService.GetCategoryNoExistToService(id);

            if (categories == null)
            {
                return JsonResults.Error();
            }

            var models = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                models.Add(GetCategoryModel(category));
            }

            return JsonResults.Success(models);
        }

        // PUT: api/Categories/5
        [ActionName("EditCategory")]
        [HttpPost]
        public async Task<object> EditCategory(CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var category = await _categoryService.GetCategory(request.Id);

                if (category == null)
                {
                    return JsonResults.Error(404, "It`s category not found");
                }

                category.Name = request.Name;

                await _categoryService.UpdateCategory(category);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        [ActionName("EditCategoryToService")]
        [HttpPost]
        public async Task<object> EditCategoryToService(Categories_Services request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var categoryToService = await _categoryService.GetCategoryService(request.Id);

                if (categoryToService == null)
                {
                    return JsonResults.Error(404, "It`s category not found");
                }

                categoryToService.IdCategories = request.IdCategories;

                await _categoryService.UpdateCategoryToService(categoryToService);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Categories
        [ActionName("AddCategory")]
        [HttpPost]
        public async Task<object> AddCategory(CategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var model = new Category
                {
                    Name = request.Name
                };

                await _categoryService.AddCategory(model);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // POST: api/Categories
        [ActionName("AddCategoryToService")]
        [HttpPost]
        public async Task<object> AddCategoryToService(Categories_Services request)
        {
            if (!ModelState.IsValid)
            {
                return JsonResults.Error(400, ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage.ToString());
            }

            try
            {
                var model = new Categories_Services
                {
                    IdCategories = request.IdCategories,
                    IdServices = request.IdServices
                };

                await _categoryService.AddCategoryToService(model);

                return JsonResults.Success();
            }
            catch (Exception ex)
            {
                return JsonResults.Error(400, ex.Message);
            }
        }

        // DELETE: api/Categories/5
        [ActionName("DeleteCategory")]
        [HttpDelete]
        public async Task<object> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategory(id);

            if (category == null)
            {
                return JsonResults.Error();
            }

            await _categoryService.DeleteCategory(category);

            return JsonResults.Success();
        }

        [ActionName("DeleteCategoryService")]
        [HttpDelete]
        public async Task<object> DeleteCategoryService(int id)
        {
            var categoryService = await _categoryService.GetCategoryService(id);

            if (categoryService == null)
            {
                return JsonResults.Error();
            }

            await _categoryService.DeleteCategoryService(categoryService);

            return JsonResults.Success();
        }

        // Util
        private CategoryViewModel GetCategoryModel(Category category)
        {
            var model = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return model;
        }
    }
}
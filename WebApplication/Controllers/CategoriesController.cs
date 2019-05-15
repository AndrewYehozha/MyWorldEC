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
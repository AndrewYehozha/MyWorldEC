using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class CategoryService
    {
        private MyWorldECEntities4 db = new MyWorldECEntities4();

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await db.Categories.ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<Category>> GetCategoryNoExistToService(int idService)
        {
            var categoriesIds = await db.Categories_Services.Where(m => m.IdServices == idService).Select(m => m.IdCategories).ToListAsync();

            var categories = db.Categories.Where(m => !(categoriesIds.Contains(m.Id))).AsEnumerable();

            return categories;
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await db.Categories.FindAsync(id);
            return category;
        }

        public async Task UpdateCategory(Category category)
        {
            db.Entry(category).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Category> AddCategory(Category category)
        {
            var result = db.Categories.Add(category);
            await db.SaveChangesAsync();

            return result;
        }

        public async Task DeleteCategory(Category category)
        {
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
        }
    }
}
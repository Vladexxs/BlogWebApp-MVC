using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using TestBlog.Models;
using TestBlog.Repositories;

namespace TestBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            BlogRepository blogRepository = new BlogRepository();
            var categories = await blogRepository.CategoryRepository.GetCategoriesAsync();
            return View(categories);
        }

        public async Task<IActionResult> Edit(int id)
        {
            BlogRepository repo = new BlogRepository();
            var category = await repo.CategoryRepository.GetCategoryAsync(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Category model)
        {
            BlogRepository repository = new BlogRepository();
            await repository.CategoryRepository.UpdateCategoryAsync(model);
            return RedirectToAction("Index");
        }

   
        public async Task<IActionResult> New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveNew(Category model)
        {

            BlogRepository repo = new BlogRepository();
            await repo.CategoryRepository.AddNewCategoryAsync(model);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int categoryId)
        {
            BlogRepository repo = new BlogRepository();
            await repo.CategoryRepository.DeleteCategoryAsync(categoryId);
            return RedirectToAction("Index");

        }

    }
}

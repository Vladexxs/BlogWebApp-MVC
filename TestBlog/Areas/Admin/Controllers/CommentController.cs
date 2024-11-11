using Microsoft.AspNetCore.Mvc;
using TestBlog.Repositories;

namespace TestBlog.Areas.Admin.Controllers
{
	public class CommentController : Controller
	{
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
			BlogRepository repo = new BlogRepository();
			var comments = await repo.CommentRepository.GetLastCommentsAsync();
			return View(comments);
		}

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            BlogRepository repo = new BlogRepository();
            await repo.CommentRepository.DeleteCommentAsync(id);
            return RedirectToAction("Index");
        }
    }
}

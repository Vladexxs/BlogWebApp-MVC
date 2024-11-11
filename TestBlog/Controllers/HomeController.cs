using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using TestBlog.Models;
using TestBlog.Repositories;
using TestBlog.Utils;
using X.PagedList;
namespace TestBlog.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			BlogRepository blogRepository = new BlogRepository();
			var posts = await blogRepository.PostRepository.GetLastPostsAsync(10);
			return View(posts);
		}



		public async Task<IActionResult> Posts(int categoryId, int page = 1)
		{
			BlogRepository blogRepository = new BlogRepository();
			if (categoryId == 0)
			{
				ViewBag.category = Constants.CreateOthersPostCategory();
				var posts = await blogRepository.PostRepository.GetOtherPostsAsync();
				var pagedPosts = await posts.ToPagedListAsync(page, 6);
				return View(pagedPosts);
			}
			else
			{
				var category = await blogRepository.CategoryRepository.GetCategoryAsync(categoryId);
				if (category == null)
				{
					return RedirectToAction("Index");
				}
				ViewBag.category = category;
				var posts = await blogRepository.PostRepository.GetPostsByCategoryAsync(categoryId);
				var pagedPosts = await posts.ToPagedListAsync(page, 6);
				return View(pagedPosts);
			}

		}



		public async Task<IActionResult> Post(int postId)
		{
			BlogRepository blogRepository = new BlogRepository();
			var post = await blogRepository.PostRepository.GetPostAsync(postId);

			post.Comments = await blogRepository.CommentRepository.GetCommentsByPostIdAsync(postId);
			return View(post);
		}
		public async Task<IActionResult> GetCommentByPostId(int PostId, string UserName, string Text)
		{
			var entity = new Comment
			{
				Text = Text,
				UserName = UserName,
				PostId = PostId,
				Date = DateTime.Now
			};
			BlogRepository repo = new BlogRepository();
			await repo.CommentRepository.AddNewCommentAsync(entity);

			return RedirectToAction("Post", new { postId = PostId });
		}

		public IActionResult AboutMe()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}



		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

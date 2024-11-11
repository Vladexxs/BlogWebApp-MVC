using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Reflection;
using TestBlog.Models;
using TestBlog.Repositories;
using TestBlog.Utils;
using X.PagedList;

namespace TestBlog.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class PostController : Controller
	{

		public async Task<IActionResult> Index()
		{
			BlogRepository repo = new BlogRepository();
			var categories = await repo.CategoryRepository.GetCategoriesAsync();
			var otherPostsCount = await repo.PostRepository.GetOtherPostsCountAsync();
			if (otherPostsCount > 0)
			{
				categories.Add(Constants.CreateOthersPostCategory());
			}
			return View(categories);
		}

		public async Task<IActionResult> Posts(int categoryId, int page = 1)
		{
			BlogRepository repo = new BlogRepository();
			var category = await repo.CategoryRepository.GetCategoryAsync(categoryId);
			if (category == null)
			{
				return RedirectToAction("Index");
			}
			var posts = await repo.PostRepository.GetPostsByCategoryAsync(categoryId);

		
			var pagedPosts = posts.ToPagedList(page, 3);

			ViewBag.category = category;
			return PartialView("_PostsPartial", pagedPosts);
		}

		public async Task<IActionResult> Create()
		{
			BlogRepository repo = new BlogRepository();
			var categories = await repo.CategoryRepository.GetCategoriesAsync();
			ViewBag.categories = categories;
			return PartialView("_CreatePostPartial", new Post());
		}


		public async Task<IActionResult> Save(Post post)
		{
			BlogRepository repo = new BlogRepository();

			ModelState.Remove("ShortDetail");
			ModelState.Remove("Detail");
			ModelState.Remove("ImageName");
			ModelState.Remove("PostImageFile");

			if (post.PostId == 0)
			{
				if (ModelState.IsValid == false)
				{
					return Json(new
					{
						Status = 0,
						Message = String.Join(",", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage).ToList()).ToList())
					});
				}
				if (post.PostImageFile == null)
				{
					return Json(new
					{
						Status = 0,
						Message = "Bir resim yükleyin!",
					});
				}
				var dir = Directory.GetCurrentDirectory();
				var fileName = post.PostImageFile.FileName;
				var ext = Path.GetExtension(post.PostImageFile.FileName).ToLower();
				if (ext == ".jpg" || ext == ".png" || ext == ".jpeg" || ext == ".bmp")
				{

				}
				else
				{
					return Json(new
					{
						Status = 0,
						Message = "Seçtiğiniz dosya formatı geçersiz!",
					});
				}

				var newImageName = DateTime.Now.ToString().Replace(".", "-").Replace(":", "-").Replace(" ", "-") + ext;

				var filePath = Path.Combine(dir, "wwwroot", "images", newImageName);
				using (var stream = System.IO.File.Create(filePath))
				{
					await post.PostImageFile.CopyToAsync(stream);
				}
				post.ImageName = newImageName;
				post.Date = DateTime.Now;
				await repo.PostRepository.AddNewPostAsync(post);
			}
			else
			{
				var data = await repo.PostRepository.GetPostAsync(post.PostId);
				if (data == null)
				{
					return Json(new
					{
						Status = 0,
						Message = "Gönderi Bulunamadı!",
					});
				}

				if (ModelState.IsValid == false)
				{
					return Json(new
					{
						Status = 0,
						Message = String.Join(",", ModelState.Values.SelectMany(x => x.Errors.Select(c => c.ErrorMessage).ToList()).ToList())
					});
				}

				if (post.PostImageFile != null)
				{
					//kod tekrarı başka bir class içerisine eklenmelidir
					var dir = Directory.GetCurrentDirectory();
					var fileName = post.PostImageFile.FileName;
					var ext = Path.GetExtension(post.PostImageFile.FileName).ToLower();
					if (ext == ".jpg" || ext == ".png" || ext == ".jpeg" || ext == ".bmp")
					{

					}
					else
					{
						return Json(new
						{
							Status = 0,
							Message = "Seçtiğiniz dosya formatı geçersiz!",
						});
					}

					var newImageName = DateTime.Now.ToString().Replace(".", "-").Replace(":", "-").Replace(" ", "-") + ext;

					var filePath = Path.Combine(dir, "wwwroot", "images", newImageName);
					using (var stream = System.IO.File.Create(filePath))
					{
						await post.PostImageFile.CopyToAsync(stream);
					}
					post.ImageName = newImageName;
				}
				else
				{
					post.ImageName = data.ImageName;
				}

				post.Date = data.Date;
				await repo.PostRepository.EditPostAsync(post);
			}

			return Json(new
			{
				Status = 1,
				Message = "İşlem Başarılı",
			});
		}

		public async Task<IActionResult> Edit(int postId)
		{
			BlogRepository repo = new BlogRepository();
			var post = await repo.PostRepository.GetPostAsync(postId);
			if (post == null)
			{
				throw new Exception("Gönderi Bulunamadı!");
			}
			var categories = await repo.CategoryRepository.GetCategoriesAsync();
			ViewBag.categories = categories;

			return PartialView("_CreatePostPartial", post);
		}


		[HttpPost]
		public async Task<IActionResult> Delete(int PostId)
		{
			BlogRepository repo = new BlogRepository();
			var post = await repo.PostRepository.GetPostAsync(PostId);
			if (post == null)
			{
				return Json(new
				{
					Status = 1,
					Message = "Gönderi Bulunamadı!"
				});
			}

			await repo.PostRepository.DeletePostAsync(PostId);

			return Json(new
			{
				Status = 0,
				Message = "Gönderi Silindi!"
			});
		}

	}
}
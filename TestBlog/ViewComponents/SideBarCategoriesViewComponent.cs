using Microsoft.AspNetCore.Mvc;
using TestBlog.Repositories;
using TestBlog.Utils;

namespace TestBlog.ViewComponents
{
	[ViewComponent(Name = "SideBarCategories")]
	public class SideBarCategoriesViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			BlogRepository repo = new BlogRepository();
			var categories = await repo.CategoryRepository.GetCategoriesAsync();
			var otherPostCount = await repo.PostRepository.GetOtherPostsCountAsync();
			if(otherPostCount > 0)
			{
				categories.Add(Constants.CreateOthersPostCategory());
			}
			return View(categories);
		}
	}


}

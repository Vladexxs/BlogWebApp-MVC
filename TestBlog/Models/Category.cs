using System.ComponentModel.DataAnnotations;

namespace TestBlog.Models
{
	public class Category
	{
        public int CategoryId { get; set; }

		[Display(Name ="Kategori adı")]
		[Required(ErrorMessage ="Kategori Adı Gerekli!")]
		public string? Name { get; set; }
    }
}

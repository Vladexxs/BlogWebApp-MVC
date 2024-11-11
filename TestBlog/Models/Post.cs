using System.ComponentModel.DataAnnotations;

namespace TestBlog.Models
{
	public class Post
	{
		public int PostId { get; set; }

		[Display(Name = "Gönderi Adı")]
		[Required(ErrorMessage ="Gönderi adı gerekli!")]
		[StringLength(50, ErrorMessage ="Gönderi adı naksimum 50 karakter olmalıdır!")]
		public string? Name { get; set; }

		[Display(Name = "Gönderi Kısa Adı")]
		[Required(ErrorMessage ="Gönderi Kısa Adı gerekli!")]
		[StringLength(150, ErrorMessage = "Gönderi adı naksimum 50 karakter olmalıdır!")]
		public string? ShortDetail { get; set; }

		[Display(Name ="Gönderi Detayı")]
		[Required(ErrorMessage = "Gönderi Detayı gerekli!")]
		[StringLength(4000, ErrorMessage = "Gönderi adı naksimum 50 karakter olmalıdır!")]
		public string? Detail { get; set; }

        public DateTime Date { get; set; }

        [Display(Name="Resim")]
		public string? ImageName { get; set; }

		[Display(Name ="Kategori")]
		[Required(ErrorMessage ="Kategori seçin!")]
		public int? CategoryId { get; set; }

        public IFormFile PostImageFile { get; set; }

        public List<Comment> Comments { get; set; } = new();


    }
}

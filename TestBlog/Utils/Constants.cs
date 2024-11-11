using Mono.TextTemplating;
using TestBlog.Models;

namespace TestBlog.Utils
{
	public static class Constants
	{
		public static string GetDateRemainingTime(DateTime date)
		{
			var now = DateTime.Now;
			TimeSpan span = now - date;
			if (span.TotalSeconds < 59)
			{
				return Convert.ToInt32(span.TotalSeconds) + " saniye önce";
			}
			else if (span.TotalMinutes < 59)
			{
				return Convert.ToInt32(span.TotalMinutes) + " dakika önce";
			}
			else if (span.TotalHours < 23)
			{
				return Convert.ToInt32(span.TotalHours) + " saat önce";
			}
			else if (span.TotalDays < 29)
			{
				return Convert.ToInt32(span.TotalDays) + " gün önce ";
			}
			return date.ToString("dd/MM/yyyy");
		}

		public static Category CreateOthersPostCategory()
		{
			var category = new Category()
			{
				CategoryId = 0,
				Name = "Diğer Gönderiler"
			};
			return category;
		}
	}
}

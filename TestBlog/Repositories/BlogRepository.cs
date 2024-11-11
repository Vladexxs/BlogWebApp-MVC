using System.Data.SqlClient;

namespace TestBlog.Repositories
{
    public class BlogRepository
    {
        public readonly SqlConnection _con;
        public BlogRepository()
        {
            _con = new SqlConnection("Server=MERDOYAZILIM;Database=MERTBLOG;User Id=sa;Password=341754;");
        }

        private CategoryRepository _categoryRepository;

        public CategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_con);
                }
                return _categoryRepository;
            }
        }


        private PostRepository _postRepository;

        public PostRepository PostRepository
        {
            get
            {
                if (_postRepository == null)
                {
                    _postRepository = new PostRepository(_con);
                }
                return _postRepository;
            }
        }

		private CommentRepository _commentRepository;

		public CommentRepository CommentRepository
		{
			get
			{
				if (_commentRepository == null)
				{
					_commentRepository = new CommentRepository(_con);
				}
				return _commentRepository; 
			}
		}

	}
}

using System.Data;
using System.Data.SqlClient;
using TestBlog.Models;

namespace TestBlog.Repositories
{
    public class PostRepository
    {
        private SqlConnection _con;
        public PostRepository(SqlConnection con)
        {
            _con = con;
        }
        public async Task<List<Post>> GetLastPostsAsync(int count)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            using (SqlCommand command = new SqlCommand("SELECT TOP [count] PostId, Name, ShortDetail, Detail, Date, ImageName, CategoryId " +
         "FROM Posts " +
         "ORDER BY Date DESC", _con))
            {
                command.CommandText = command.CommandText.Replace("[count]", count.ToString());
                List<Post> posts = new List<Post>();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var post = new Post
                            {
                                PostId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ShortDetail = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Detail = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Date = reader.GetDateTime(4),
                                ImageName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                            };
                            posts.Add(post);
                        }

                    }
                    reader.Close();
                    _con.Close();
                    return posts;
                }


            }

        }
        public async Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            const string queryString =
              "SELECT PostId, Name, ShortDetail, Detail, Date, ImageName, CategoryId " +
         "FROM Posts " +
         "WHERE CategoryId = @CategoryId " +
         "ORDER BY Date DESC";
            List<Post> postList = new List<Post>();
            using (SqlCommand command = new(queryString, _con))
            {
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var post = new Post
                        {
                            PostId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ShortDetail = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Detail = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Date = reader.GetDateTime(4),
                            ImageName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                        };
                        postList.Add(post);
                    }
                    reader.Close();
                    _con.Close();
                    return postList;
                }

            }

        }
        public async Task<List<Post>> GetOtherPostsAsync()
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            const string queryString =
              "SELECT PostId, Name, ShortDetail, Detail, Date, ImageName, CategoryId " +
         "FROM Posts " +
         "WHERE CategoryId is null " +
         "ORDER BY Date DESC";
            List<Post> postList = new List<Post>();
            using (SqlCommand command = new(queryString, _con))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var post = new Post
                        {
                            PostId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            ShortDetail = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Detail = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Date = reader.GetDateTime(4),
                            ImageName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                        };
                        postList.Add(post);
                    }
                    reader.Close();
                    _con.Close();
                    return postList;
                }

            }

        }
        public async Task<Post> GetPostAsync(int postId)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            const string queryString =
              "SELECT PostId, Name, ShortDetail, Detail, Date, ImageName, CategoryId " +
         "FROM Posts " +
         "WHERE PostId = @PostId ";

            using (SqlCommand command = new(queryString, _con))
            {
                command.Parameters.AddWithValue("@PostId", postId);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows == false)
                    {
                        reader.Close();
                        _con.Close();
                        return null;
                    }
                    await reader.ReadAsync();
                    var post = new Post
                    {
                        PostId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        ShortDetail = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        Detail = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Date = reader.GetDateTime(4),
                        ImageName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                    };
                    reader.Close();
                    _con.Close();
                    return post;
                }

            }

        }

        public async Task<int> GetOtherPostsCountAsync()
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            using (SqlCommand command = new SqlCommand("Select count(*) from Posts Where CategoryId is NULL", _con))
            {
                var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                _con.Close();
                return count;
            }
        }

        public async Task AddNewPostAsync(Post post)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            using (var cmd = new SqlCommand(@"INSERT INTO Posts (Name, ShortDetail, Detail, Date, ImageName, CategoryId) VALUES (@Name, @ShortDetail, @Detail, @Date, @ImageName, @CategoryId)", _con))
            {
                cmd.Parameters.AddWithValue("@Name", post.Name);
                cmd.Parameters.AddWithValue("@ShortDetail", post.ShortDetail??"");
                cmd.Parameters.AddWithValue("@Detail", post.Detail??"");
                cmd.Parameters.AddWithValue("@Date", post.Date);
                cmd.Parameters.AddWithValue("@ImageName", post.ImageName);
                cmd.Parameters.AddWithValue("@CategoryId", post.CategoryId);

                await cmd.ExecuteNonQueryAsync();
            }
            _con.Close();
        }

        public async Task EditPostAsync(Post post)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }

            using (SqlCommand cmd = new SqlCommand(@"UPDATE Posts SET 
        Name = @Name, 
        ShortDetail = @ShortDetail, 
        Detail = @Detail, 
        ImageName = @ImageName, 
        CategoryId = @CategoryId 
        WHERE PostId = @PostId", _con))
            {
                cmd.Parameters.AddWithValue("@Name", post.Name);
                cmd.Parameters.AddWithValue("@ShortDetail", post.ShortDetail??"");
                cmd.Parameters.AddWithValue("@Detail", post.Detail??"");
                cmd.Parameters.AddWithValue("@ImageName", post.ImageName);
                cmd.Parameters.AddWithValue("@CategoryId", post.CategoryId);
                cmd.Parameters.AddWithValue("@PostId", post.PostId);

                await cmd.ExecuteNonQueryAsync();
            }
            _con.Close();

        }

		public async Task DeletePostAsync(int postId)
		{
			if (_con.State != ConnectionState.Open)
			{
				await _con.OpenAsync();
			}

			using (var cmd = new SqlCommand("DELETE FROM Posts WHERE PostId = @PostId", _con))
			{
				cmd.Parameters.AddWithValue("@PostId", postId);

				await cmd.ExecuteNonQueryAsync();
               
			}

			_con.Close();
		}







	}
}

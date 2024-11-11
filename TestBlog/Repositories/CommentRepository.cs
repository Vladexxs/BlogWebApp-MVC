using System.Data;
using System.Data.SqlClient;
using TestBlog.Models;

namespace TestBlog.Repositories
{
	public class CommentRepository
	{
		private SqlConnection _con;

		public CommentRepository(SqlConnection con)
		{
			_con = con;
		}
		public async Task AddNewCommentAsync(Comment comment)
		{
			if (_con.State != ConnectionState.Open)
			{
				await _con.OpenAsync();
			}
			comment.Date = DateTime.Now;
			using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Comments (UserName, Text,Date, PostId) VALUES (@UserName, @Text,@Date, @PostId)", _con))
			{
				cmd.Parameters.AddWithValue("@UserName", comment.UserName);
				cmd.Parameters.AddWithValue("@Text", comment.Text);
				cmd.Parameters.AddWithValue("@Date", comment.Date);
				cmd.Parameters.AddWithValue("@PostId", comment.PostId);

				await cmd.ExecuteNonQueryAsync();
			}

			_con.Close();
		}

		public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
		{
			List<Comment> comments = new List<Comment>();

			if (_con.State != ConnectionState.Open)
			{
				await _con.OpenAsync();
			}

			using (SqlCommand cmd = new SqlCommand("SELECT CommentId, UserName, Text, Date, PostId FROM Comments WHERE PostId = @PostId", _con))
			{
				cmd.Parameters.AddWithValue("@PostId", postId);

				using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						Comment comment = new Comment
						{
							CommentId = reader.GetInt32(0),
							UserName = reader.GetString(1),
							Text = reader.IsDBNull(2) ? null : reader.GetString(2),
							Date = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
							PostId = reader.GetInt32(4)
						};

						comments.Add(comment);
					}

				}
			}

			_con.Close();

			return comments;
		}

		public async Task<List<Comment>> GetLastCommentsAsync()
		{
			List<Comment> comments = new List<Comment>();

			if (_con.State != ConnectionState.Open)
			{
				await _con.OpenAsync();
			}

			using (SqlCommand command = new SqlCommand("SELECT CommentId, UserName, Text, Date, PostId FROM Comments ORDER BY Date DESC", _con))
			{
				using (SqlDataReader reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						var comment = new Comment
						{
							CommentId = reader.GetInt32(0),
							UserName = reader.GetString(1),
							Text = reader.IsDBNull(2) ? null : reader.GetString(2),
							Date = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
							PostId = reader.GetInt32(4)
						};
						comments.Add(comment);
					}
				}
			}

			_con.Close();

			return comments;
		}

        public async Task DeleteCommentAsync(int id)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }

            using (SqlCommand cmd = new SqlCommand("DELETE FROM Comments WHERE CommentId = @CommentId", _con))
            {
                cmd.Parameters.AddWithValue("@CommentId", id);
                await cmd.ExecuteNonQueryAsync();
            }

            _con.Close();
        }

    }
}

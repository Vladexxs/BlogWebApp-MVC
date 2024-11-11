using System.Data;
using System.Data.SqlClient;
using TestBlog.Models;

namespace TestBlog.Repositories
{
    public class CategoryRepository
    {
        private SqlConnection _con;

        public CategoryRepository(SqlConnection con)
        {
            _con = con;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            if (_con.State != System.Data.ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            string queryString = "select CategoryId, Name from Categories";
            // Create the Command and Parameter objects.
            List<Category> categoryList = new List<Category>();
            using (SqlCommand command = new(queryString, _con))
            {
                //command.Parameters.AddWithValue("@Id", 5);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var category = new Category();
                            int Id = reader.GetInt32(0);
                            //reader.IsDBNull(1)
                            string categoryName = reader.GetString(1);
                            category.Name = categoryName;
                            category.CategoryId = Id;
                            categoryList.Add(category);
                        }
                    }
                    reader.Close();
                }
            }
            _con.Close();
            return categoryList;
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            using (SqlCommand command = new SqlCommand("select CategoryId, Name from Categories Where CategoryId=@CategoryId", _con))
            {
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                using (var rdr = await command.ExecuteReaderAsync())
                {
                    if (rdr.HasRows == false)
                    {
                        rdr.Close();
                        _con.Close();
                        return null;
                    }
                    await rdr.ReadAsync();
                    var category = new Category();
                    category.CategoryId = rdr.GetInt32(0);
                    category.Name = rdr.GetString(1);
                    rdr.Close();
                    _con.Close();
                    return category;
                }
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            using (SqlCommand cmd = new SqlCommand(@"UPDATE Categories
SET Name = @Name
where CategoryId = @CategoryId", _con))
            {
                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                cmd.Parameters.AddWithValue("@Name", category.Name);
                await cmd.ExecuteNonQueryAsync();
            }
			_con.Close();
		}

        public async Task AddNewCategoryAsync(Category category)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Categories (Name) VALUES (@Name)", _con))
            {
                cmd.Parameters.AddWithValue("@Name", category.Name);
                await cmd.ExecuteNonQueryAsync();
            }
			_con.Close();
		}

        public async Task DeleteCategoryAsync(int categoryId)
        {
            if (_con.State != ConnectionState.Open)
            {
                await _con.OpenAsync();
            }
            using (SqlCommand cmd = new SqlCommand(@"DELETE FROM Categories WHERE CategoryId = @CategoryId", _con))
            {
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                await cmd.ExecuteNonQueryAsync();
                cmd.CommandText = "UPDATE Posts SET CategoryId = null where CategoryId = @CategoryId";
                await cmd.ExecuteNonQueryAsync();
			}
            _con.Close();
        }

    }
}

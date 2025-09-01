using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.Database;
using OrderingSystem.Model;

namespace OrderingSystem.Repositories.Categories
{
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        public async Task<List<Category>> GetCategories()
        {
            List<Category> categories = new List<Category>();
            var db = MyDatabase.getInstance();
            try
            {

                var conn = await db.GetConnection();
                string query = @"
                                SELECT DISTINCT d.category_id, c.* FROM category c
                                INNER JOIN dishes d ON c.category_id = d.category_id 
                                WHERE category_type = 'Dishes'
                                ";
                var cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        Category cz = Category.Builder()
                        .SetCategoryID(reader.GetInt32("category_id"))
                       .SetCategoryName(reader.GetString("category_name"))
                       .SetCategoryType("Dish_Category")
                       .Build();
                        categories.Add(cz);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                await db.CloseConnection();
            }

            return categories;
        }
    }
}

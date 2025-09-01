using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using OrderingSystem.Database;
using OrderingSystem.Model;

namespace OrderingSystem.Repositories.Ingredients
{
    public class IngredientRepository : IIngredientRepository
    {
        public async Task<List<Ingredient>> getIngredientByMenu(Menu menu)
        {
            var db = MyDatabase.getInstance();
            var ingredients = new List<Ingredient>();
            try
            {
                var conn = await db.GetConnection();
                string query = @"
                                SELECT DISTINCT i.ingredient_id, i.ingredient_name, di.quantity FROM ingredients i
                                INNER JOIN dishes_ingredient di ON di.ingredient_id = i.ingredient_id
                                WHERE di.dishes_id = @id AND LOWER(i.removable) = 'yes';
                                ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", menu.MenuID);
                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    ingredients.Add(Ingredient.Builder()
                        .SetIngredientID(reader.GetInt32("ingredient_id"))
                        .SetQuantity(reader.GetInt32("quantity"))
                        .SetIngredientName(reader.GetString("ingredient_name"))
                        .Build());
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error get ingredient by menu", ex);
            }
            finally
            {
                await db.CloseConnection();
            }
            return ingredients;
        }
    }
}

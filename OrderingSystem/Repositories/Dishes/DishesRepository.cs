using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;
using OrderingSystem.Database;
using OrderingSystem.KioskApp.MenuBuilder;
using Dish = OrderingSystem.Model.Dish;

namespace OrderingSystem.KioskApp.Menus
{
    public class DishesRepository : IDishRepository
    {

        public async Task<List<Dish>> RetrieveDish()
        {
            List<Dish> mList = new List<Dish>();
            var db = MyDatabase.getInstance();

            try
            {
                var conn = await db.GetConnection();
                string query = @"SELECT * FROM x_retrieve_dishes WHERE isAvailable = 'Yes'";
                var command = new MySqlCommand(query, conn);
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        Dish m = (Dish)MenuBuilderFactory.BuildFromSQL(reader);
                        mList.Add(m);
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await db.CloseConnection();
            }
            return mList;
        }
    }
}

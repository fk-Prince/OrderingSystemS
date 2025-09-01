using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.Database;

namespace OrderingSystem.Repositories.BeverageDesserts
{
    public class BeverageDessertRepository : IBeverageDessertRepository
    {
        public async Task<List<Model.BeverageDesserts>> GetBeverage()
        {
            List<Model.BeverageDesserts> list = new List<Model.BeverageDesserts>();

            var db = MyDatabase.getInstance();

            try
            {
                var conn = await db.GetConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM x_retrieve_beverage_dessert WHERE menu_type = 'Beverage'", conn);
                MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Model.BeverageDesserts bd = Model.BeverageDesserts.Builder()
                        .SetMenuId(reader.GetInt32("menu_id"))
                        .SetMenuName(reader.GetString("menu_name"))
                        .SetDescription(reader.GetString("menu_description"))
                        .SetPrice(reader.GetDouble("price"))
                        .SetEstimatedTime(reader.GetTimeSpan("estimated_time"))
                        .SetCurrentlyMaxOrder(reader.GetInt32("max_order"))
                        .SetPurchaseQuantity(0)
                        .SetBDID(reader.GetInt32("beverage_dessert_id"))
                        .SetMenuType(reader.GetString("menu_type"))
                        .Build();
                    list.Add(bd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw new Exception("Xdxd");
            }
            finally
            {
                await db.CloseConnection();
            }
            return list;
        }

        public async Task<List<Model.BeverageDesserts>> GetDesert()
        {
            List<Model.BeverageDesserts> list = new List<Model.BeverageDesserts>();

            var db = MyDatabase.getInstance();

            try
            {
                var conn = await db.GetConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM x_retrieve_beverage_dessert WHERE menu_type = 'Dessert'", conn);
                MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Model.BeverageDesserts bd = Model.BeverageDesserts.Builder()
                        .SetMenuId(reader.GetInt32("menu_id"))
                        .SetMenuName(reader.GetString("menu_name"))
                        .SetDescription(reader.GetString("menu_description"))
                        .SetPrice(reader.GetDouble("price"))
                        .SetEstimatedTime(reader.GetTimeSpan("estimated_time"))
                        .SetCurrentlyMaxOrder(reader.GetInt32("max_order"))
                        .SetPurchaseQuantity(0)
                        .SetBDID(reader.GetInt32("beverage_dessert_id"))
                        .SetMenuType(reader.GetString("menu_type"))
                        .Build();
                    list.Add(bd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw new Exception("Xdxd");
            }
            finally
            {
                await db.CloseConnection();
            }
            return list;
        }
    }
}

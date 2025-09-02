using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.Database;
using OrderingSystem.Model;

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
                    string v = reader.GetString("pvid_name_stock_price");
                    List<Variant> vList = new List<Variant>();

                    string[] outerParts = v.Split(',');
                    foreach (string texts in outerParts)
                    {
                        string[] parts = texts.Trim().Split('|');
                        vList.Add(
                            Variant.Builder()
                            .SetVaraintId(int.Parse(parts[0].Trim()))
                            .SetVaraintName(parts[1].Trim())
                            .SetVaraintPrice(double.Parse(parts[3].Trim()))
                            .SetCurrentlyMaxOrder(int.Parse(parts[2].Trim()))
                            .Build()
                        );
                    }
                    Model.BeverageDesserts bd = Model.BeverageDesserts.Builder()
                        .SetMenuId(reader.GetInt32("menu_id"))
                        .SetMenuName(reader.GetString("menu_name"))
                        .SetDescription(reader.GetString("menu_description"))
                        .SetEstimatedTime(reader.GetTimeSpan("estimated_time"))
                        .SetVariant(vList)
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
                    string v = reader.GetString("pvid_name_stock_price");
                    List<Variant> vList = new List<Variant>();
                    string[] outerParts = v.Split(',');
                    foreach (string texts in outerParts)
                    {
                        string[] parts = texts.Trim().Split('|');
                        vList.Add(
                            Variant.Builder()
                            .SetVaraintId(int.Parse(parts[0].Trim()))
                            .SetVaraintName(parts[1].Trim())
                            .SetVaraintPrice(double.Parse(parts[3].Trim()))
                            .SetCurrentlyMaxOrder(int.Parse(parts[2].Trim()))
                            .Build()
                        );
                    }
                    Model.BeverageDesserts bd = Model.BeverageDesserts.Builder()
                        .SetMenuId(reader.GetInt32("other_menu_id"))
                        .SetMenuName(reader.GetString("menu_name"))
                        .SetDescription(reader.GetString("menu_description"))
                        .SetEstimatedTime(reader.GetTimeSpan("estimated_time"))
                        .SetVariant(vList)
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

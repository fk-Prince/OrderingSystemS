using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using Newtonsoft.Json;
using OrderingSystem.Database;
using OrderingSystem.Model;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.Repositories.Kiosk
{
    public class KioskRepository : IKioskRepository
    {

        public async Task<int> getMaxOrderAddon(List<Menu> cartList, int id)
        {
            var db = MyDatabase.getInstance();
            int maxOrder = 0;
            try
            {
                var conn = await db.GetConnection();
                using (var cmd = new MySqlCommand("GetMaxOrderForAddon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(cartList);
                    cmd.Parameters.AddWithValue("p_cart_json", json);
                    cmd.Parameters.AddWithValue("p_target_adds_id", id);

                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            if (await reader.ReadAsync())
                            {
                                maxOrder = reader.GetInt32("max_order");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception("Error calculat the maxorder");
            }
            finally
            {
                await db.CloseConnection();
            }

            return maxOrder;
        }

        public async Task<int> getMaxOrderMenu(List<Model.Menu> cartList, Menu menu)
        {

            var db = MyDatabase.getInstance();
            int maxOrder = 0;
            try
            {
                var conn = await db.GetConnection();
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    if (menu.MenuType.ToLower() == "dishes")
                        cmd.CommandText = "z_MaxOrderDishes";
                    else if (menu.MenuType.ToLower() == "appetizer")
                        cmd.CommandText = "z_MaxOrderAppetizer";
                    else if (menu.MenuType.ToLower() == "combo")
                        cmd.CommandText = "z_MaxOrderCombo";
                    else if (menu.MenuType.ToLower() == "addon")
                        cmd.CommandText = "z_MaxOrderAddon";
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(cartList);

                    cmd.Parameters.AddWithValue("p_cart_json", json);
                    if (menu.MenuType.ToLower() == "product")
                    {
                        cmd.Parameters.AddWithValue("p_target_product_id", menu.MenuID);
                    }
                    else if (menu.MenuType.ToLower() == "addon" && menu is Addon a)
                    {
                        cmd.Parameters.AddWithValue("p_target_menu_id", a.Addon_id);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("p_target_menu_id", menu.MenuID);

                    }
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            if (await reader.ReadAsync())
                            {
                                maxOrder = reader.GetInt32("max_order");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw new Exception("Error calculat the maxorder");
            }
            finally
            {
                await db.CloseConnection();
            }

            return maxOrder;

        }

        public async Task<int> getMaxOrderProduct(List<Menu> cartList, int productVariantID)
        {
            var db = MyDatabase.getInstance();
            int maxOrder = 0;

            try
            {
                var conn = await db.GetConnection();
                using (var cmd = new MySqlCommand("Z_MaxOrderProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(cartList);

                    cmd.Parameters.AddWithValue("p_cart_json", json);
                    cmd.Parameters.AddWithValue("p_target_product_id", productVariantID);


                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            if (await reader.ReadAsync())
                            {
                                maxOrder = reader.GetInt32("max_order");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error calculatong the maxorder");
            }
            finally
            {
                await db.CloseConnection();
            }

            return maxOrder;
        }

        public async Task ConfirmOrder(Order order)
        {
            var db = MyDatabase.getInstance();
            try
            {
                var conn = await db.GetConnection();
                string json = JsonConvert.SerializeObject(order.OrderList);
                Console.WriteLine(json);
                using (MySqlCommand cmd = new MySqlCommand("X_MakeAnOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_order_json", json);
                    if (order.CouponCode == null)
                    {
                        cmd.Parameters.AddWithValue("@p_coupon_code", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@p_coupon_code", order.CouponCode);
                    }
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw new Exception("Error confirming order " + ex.Message);
            }
            finally
            {
                await db.CloseConnection();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.Database;
using OrderingSystem.KioskApp.MenuBuilder;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApp.AddsOn
{
    public class AddonRepository : IAddonRepository
    {

        public async Task<List<Addon>> getAddsOnByMenu(int id)
        {
            List<Addon> l = new List<Addon>();
            var db = MyDatabase.getInstance();
            try
            {
                var conn = await db.GetConnection();
                string query = @"
                                SELECT * 
                                FROM x_retrieve_addon x
                                WHERE x.addon_id IN 
                                    (
                                        SELECT addon_id FROM addon_dishes WHERE addon_dishes.dishes_id = @dishes_id
                                    )
                            ";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dishes_id", id);
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            l.Add(MenuBuilderFactory.BuildFromSQL(reader) as Addon
                             );
                        }

                    }
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                await db.CloseConnection();
            }
            return l;
        }

        public async Task<List<Addon>> getAddsOn()
        {
            List<Addon> l = new List<Addon>();
            var db = MyDatabase.getInstance();
            try
            {
                var conn = await db.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM x_retrieve_addon", conn))
                {
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            l.Add(
                                 MenuBuilder.MenuBuilderFactory.BuildFromSQL(reader) as Addon
                             );
                        }

                    }
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                await db.CloseConnection();
            }
            return l;
        }
    }
}

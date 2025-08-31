using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.Database;
using OrderingSystem.KioskApp.MenuBuilder;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApp.Appetizers
{
    public class AppetizerRepository : IAppetizerRepository
    {
        public async Task<List<Appetizer>> GetAppetizers()
        {
            List<Appetizer> appList = new List<Appetizer>();
            var db = MyDatabase.getInstance();
            try
            {
                var conn = await db.GetConnection();
                var cmd = new MySqlCommand("SELECT * FROM x_retrieve_appetizer WHERE isAvailable = 'Yes'", conn);
                MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Appetizer app = (Appetizer)MenuBuilderFactory.BuildFromSQL(reader);
                    appList.Add(app);
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

            return appList;
        }
    }
}

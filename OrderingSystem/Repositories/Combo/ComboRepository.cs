using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.Database;
using OrderingSystem.KioskApp.MenuBuilder;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApp.Combos
{
    public class ComboRepository : IComboRepository
    {
        public async Task<List<Combo>> RetrieveCombo()
        {
            List<Combo> cList = new List<Combo>();
            var db = MyDatabase.getInstance();

            try
            {
                var conn = await db.GetConnection();
                var cmd = new MySqlCommand("SELECT * FROM x_retrieve_combo WHERE isAvailable = 'Yes'", conn);

                MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        Combo m = (Combo)MenuBuilderFactory.BuildFromSQL(reader);
                        cList.Add(m);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("xx");
            }
            finally
            {
                await db.CloseConnection();
            }
            return cList;
        }
    }
}

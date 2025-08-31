using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace OrderingSystem.Database
{
    public class MyDatabase
    {
        private static MyDatabase instance;
        private static readonly object lockObject = new object();
        private static readonly string driver = "server=localhost;user=root;pwd=root;database=smh;AllowUserVariables=true";
        private MySqlConnection conn;
        private SemaphoreSlim connectionSemaphore = new SemaphoreSlim(1, 1);
        private MyDatabase()
        {
        }

        public static MyDatabase getInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new MyDatabase();
                    }
                }
            }
            return instance;
        }

        public async Task<MySqlConnection> GetConnection()
        {
            await connectionSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (conn == null)
                {
                    conn = new MySqlConnection(driver);
                }


                while (conn.State == ConnectionState.Connecting)
                {

                }


                if (conn.State != ConnectionState.Open)
                {
                    await conn.OpenAsync();
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                //connectionSemaphore.Release();
            }


            return conn;

        }

        public SemaphoreSlim GetConnectionSemaphore()
        {
            return connectionSemaphore;
        }
        public async Task CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                await conn.CloseAsync();
                connectionSemaphore.Release();
            }

        }

    }
}

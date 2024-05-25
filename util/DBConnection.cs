
using System.Data.SqlClient;

namespace Ecommerce.util
{
    public static class DBConnection
    {
        private static string connectionString = null;
        public static SqlConnection GetConnection()
        {
            if (connectionString == null)
            {
                connectionString = PropertyUtil.GetPropertyString("connection.properties");
            }
            return new SqlConnection(connectionString);
        } 
    }
}

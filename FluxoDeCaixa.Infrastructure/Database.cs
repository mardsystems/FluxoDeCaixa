using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace FluxoDeCaixa
{
    public static class Database
    {
        public static DataTable ExecuteForTest(string sql)
        {
            using (var connection = new SqliteConnection("DataSource=myshared.db"))
            {
                connection.Open();

                try
                {
                    using (var command = new SqliteCommand(sql, connection))
                    {
                        var reader = command.ExecuteReader();

                        var table = new DataTable();

                        table.Load(reader);

                        return table;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}

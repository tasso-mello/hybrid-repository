namespace api.hybrid.repository.Core.Injections
{
    using System;
    using System.Data.SqlClient;
    using System.Text;
    using core.hybrid.repository.Utilities;
    using Microsoft.Extensions.DependencyInjection;
    public static class DbSeed
    {
        public static void AddDatabaseWithSeed(this IServiceCollection services, string connection)
        {
            var myConn = new SqlConnection("Server=localhost;Integrated security=True;database=master");

            SqlCommand myCommand = new SqlCommand(Queries.CreateDatabaseQuery(), myConn);

            myConn.Open();
            myCommand.ExecuteNonQuery();
            myConn.Close();

            AddTable(connection);
            AddSeed(connection);
        }

        private static void AddTable(string connection)
        {
            var myConn = new SqlConnection(connection);

            SqlCommand myCommand = new SqlCommand(Queries.AddTable(), myConn);

            myConn.Open();
            myCommand.ExecuteNonQuery();
            myConn.Close();
        }

        private static void AddSeed(string connection)
        {
            var myConn = new SqlConnection(connection);

            SqlCommand myCommand = new SqlCommand(Queries.AddInitialSeed(), myConn);

            myConn.Open();
            myCommand.ExecuteNonQuery();
            myConn.Close();
        }
    }
}

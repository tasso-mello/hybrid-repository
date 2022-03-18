namespace data.hybrid.repository.Service
{
    using data.hybrid.repository.Configurations;
    using Microsoft.Data.SqlClient;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDapperDbService
    {
        SqlConnection GetConnection();
    }

    public class DapperDbService : IDapperDbService
    {
        private readonly SqlConnection _connection;
        public DapperDbService(DapperConfigurations connection)
        {
            _connection = new SqlConnection(connection.SqlConnection);
        }
        public SqlConnection GetConnection()
            => _connection;
    }
}

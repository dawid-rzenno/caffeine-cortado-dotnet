using System.Data;
using Microsoft.Data.SqlClient;

namespace cortado;

public class DapperContext(IConfiguration configuration)
{
    public IDbConnection CreateConnection() => 
        new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
}
using System.Data;
using System.Data.SqlClient;
using Forum.Infrastructure;
using MySql.Data.MySqlClient;

namespace Forum.QueryServices.Dapper
{
    public abstract class BaseQueryService
    {
        protected IDbConnection GetConnection()
        {
            return new MySqlConnection(ConfigSettings.ForumConnectionString);
        }
    }
}
using System.Data.SqlClient;
using System.Linq;
using ECommon.Components;
using ECommon.Dapper;
using Forum.Domain.Accounts;
using Forum.Infrastructure;
using MySql.Data.MySqlClient;

namespace Forum.Domain.Dapper
{
    [Component]
    public class AccountIndexRepository : IAccountIndexRepository
    {
        public void Add(AccountIndex index)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Insert(new
                {
                    AccountId = index.AccountId,
                    AccountName = index.AccountName
                }, Constants.AccountIndexTable);
            }
        }

        public AccountIndex FindByAccountName(string accountName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var data = connection.QueryList(new { AccountName = accountName }, Constants.AccountIndexTable).SingleOrDefault();
                if (data != null)
                {
                    return new AccountIndex(data.AccountId as string, accountName);
                }
                return null;
            }
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConfigSettings.ForumConnectionString);
        }
    }
}
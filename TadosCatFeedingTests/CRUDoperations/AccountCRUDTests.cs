using NUnit.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TadosCatFeeding.CRUDoperations.Tests
{
    [TestFixture]
    public class AccountCRUDTests
    {
        [TestCaseSource("Accounts")]
        public void SuccessGetTests(Account account)
        {
            AccountCRUD accountCRUD = new AccountCRUD();
            accountCRUD.Create(account);

            var result = accountCRUD.Read(GetIdByName(account.Login));
            Assert.AreEqual(account.Login, result.objectReturned.Login);

            accountCRUD.Delete(GetIdByName(account.Login));
        }

        [TestCaseSource("Accounts")]
        public void SuccessCreateAndDeleteTests(Account account)
        {
            AccountCRUD accountCRUD = new AccountCRUD();

            var createResult = accountCRUD.Create(account);
            Assert.AreEqual(true, createResult.success);

            var deleteResult = accountCRUD.Delete(GetIdByName(account.Login));
            Assert.AreEqual(true, deleteResult.success);
        }

        static Account[] Accounts =
        {
            new Account { Login = "user141@gmail.com", Nickname = "user141", Password = "123456Aa", Role = "User" },
            new Account { Login = "user21231@gmail.com", Nickname = "user21231", Password = "123456Aa", Role = "User" },
            new Account { Login = "user31231@gmail.com", Nickname = "user31231", Password = "123456Aa", Role = "User" },
            new Account { Login = "user1234@gmail.com", Nickname = "user1234", Password = "123456Aa", Role = "User" },
            new Account { Login = "user5357@gmail.com", Nickname = "user5357", Password = "123456Aa", Role = "User" },
        };

        private IConfigurationRoot GetConnection()
            => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

        private int GetIdByName(string username)
        {
            using (SqlConnection connection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("CatFeedingDB").Value))
            {
                connection.Open();

                string sqlExpression = $"SELECT Id FROM Users WHERE Login = '{username}';";
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
            return 0;
        }
    }
}
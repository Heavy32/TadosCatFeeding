using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using TadosCatFeeding.CRUDoperations;

namespace TadosCatFeedingTests.ModelValidation
{
    public class AccountValidationTests
    {
        [TestCaseSource("Accounts")]
        public void ErrorInput(Account account)
        {
            //Arrange
            var context = new ValidationContext(account, null, null);

            //Act
            var result = Validator.TryValidateObject(account, context, null);

            //Assert
            Assert.IsFalse(result);
        }

        static Account[] Accounts =
        {
            new Account(),
            new Account { Login = "aaa@aaa.com", Nickname = "a", Password = "", Role = "User" },
            new Account { Login = "1", Nickname = "", Password = "123456Aa", Role = "User" },
            new Account { Login = "", Nickname = "", Password = "", Role = "User" },
            new Account { Login = ".com@sd", Nickname = "123123123a", Password = "123123123aaaaaAAa", Role = "" },
        };
    } 
}

using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using TadosCatFeeding.CRUDoperations;

namespace TadosCatFeedingTests.ModelValidation
{
    public class AccountValidationTests
    {
        [TestCaseSource("Accounts")]
        public void ErrorInput(UserModel account)
        {
            //Arrange
            var context = new ValidationContext(account, null, null);

            //Act
            var result = Validator.TryValidateObject(account, context, null);

            //Assert
            Assert.IsFalse(result);
        }

        static UserModel[] Accounts =
        {
            new UserModel(),
            new UserModel { Login = "aaa@aaa.com", Nickname = "a", Password = "", Role = "User" },
            new UserModel { Login = "1", Nickname = "", Password = "123456Aa", Role = "User" },
            new UserModel { Login = "", Nickname = "", Password = "", Role = "User" },
            new UserModel { Login = ".com@sd", Nickname = "123123123a", Password = "123123123aaaaaAAa", Role = "" },
        };
    } 
}

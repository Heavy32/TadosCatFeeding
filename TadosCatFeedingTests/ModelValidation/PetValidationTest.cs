using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using TadosCatFeeding.Models;

namespace TadosCatFeedingTests.ModelValidation
{
    [TestFixture]
    public class PetValidationTest
    {
        [TestCaseSource("Pets")]
        public void ErrorInput(PetModel pet)
        {
            //Arrange
            var context = new ValidationContext(pet, null, null);

            //Act
            var result = Validator.TryValidateObject(pet, context, null);

            //Assert
            Assert.IsFalse(result);
        }

        static PetModel[] Pets =
        {
            new PetModel {OwnerId = 3},
            new PetModel(),
            new PetModel {Name = ""},
            new PetModel {OwnerId = 0}
        };
    }
}

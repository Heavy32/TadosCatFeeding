using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using TadosCatFeeding.Models;

namespace TadosCatFeedingTests.ModelValidation
{
    [TestFixture]
    public class PetValidationTest
    {
        [TestCaseSource("Pets")]
        public void ErrorInput(Pet pet)
        {
            //Arrange
            var context = new ValidationContext(pet, null, null);

            //Act
            var result = Validator.TryValidateObject(pet, context, null);

            //Assert
            Assert.IsFalse(result);
        }

        static Pet[] Pets =
        {
            new Pet {OwnerId = 3},
            new Pet(),
            new Pet {Name = ""},
            new Pet {OwnerId = 0}
        };
    }
}

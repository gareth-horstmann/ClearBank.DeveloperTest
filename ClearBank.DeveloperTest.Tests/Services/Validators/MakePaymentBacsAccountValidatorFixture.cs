using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.Validators;

[TestFixture]
internal class MakePaymentBacsAccountValidatorFixture
{
    [Test]
    public void GivenAnAccount_WithBacsPaymentsAllowed_WhenValidateIsCalled_ThenReturnSuccessWithNoError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
        };
        var validator = new MakePaymentBacsAccountValidator();

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GivenAnAccount_WithBacsPaymentsNotAllowed_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps
        };
        var validator = new MakePaymentBacsAccountValidator();

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The account does not allow Bacs payments");
    }

    [Test]
    public void GivenANullAccount_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var validator = new MakePaymentBacsAccountValidator();

        // Act
        var result = validator.Validate((Account)null);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The Account instance is null");
    }
}

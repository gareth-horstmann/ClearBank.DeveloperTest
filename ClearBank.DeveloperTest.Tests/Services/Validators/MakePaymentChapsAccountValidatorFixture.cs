using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.Validators;

[TestFixture]
internal class MakePaymentChapsAccountValidatorFixture
{
    [Test]
    public void GivenALiveAccount_WithChapsPaymentsAllowed_WhenValidateIsCalled_ThenReturnSuccessWithNoError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
            Status = AccountStatus.Live
        };
        var validator = new MakePaymentChapsAccountValidator();

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GivenALiveAccount_WithChapsPaymentsNotAllowed_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs,
            Status = AccountStatus.Live
        };
        var validator = new MakePaymentChapsAccountValidator();

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The account does not allow Chaps payments");
    }

    [Test]
    public void GivenADisabledAccount_WithChapsPaymentsAllowed_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
            Status = AccountStatus.Disabled
        };
        var validator = new MakePaymentChapsAccountValidator();

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The account must have a status of Live");
    }

    [Test]
    public void GivenAnInboundPaymentsOnlyAccount_WithChapsPaymentsAllowed_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
            Status = AccountStatus.InboundPaymentsOnly
        };
        var validator = new MakePaymentChapsAccountValidator();

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The account must have a status of Live");
    }

    [Test]
    public void GivenANonLiveAccount_WithChapsPaymentsNotAllowed_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Status = AccountStatus.Disabled
        };
        var validator = new MakePaymentChapsAccountValidator();

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors[0].ErrorMessage.Should().Be("The account does not allow Chaps payments");
        result.Errors[1].ErrorMessage.Should().Be("The account must have a status of Live");
    }

    [Test]
    public void GivenANullAccount_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var validator = new MakePaymentChapsAccountValidator();

        // Act
        var result = validator.Validate((Account)null);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The Account instance is null");
    }
}

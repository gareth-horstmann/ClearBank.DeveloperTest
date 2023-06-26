using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Services.Validators;

[TestFixture]
internal class MakePaymentFasterPaymentsAccountValidatorFixture
{
    [Test]
    public void GivenAnAccount_WithFasterPaymentsAllowed_WithAccountBalanceGreaterThanPaymentAmount_WhenValidateIsCalled_ThenReturnSuccessWithNoError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Balance = 10
        };
        var arguments = new MakePaymentFasterPaymentsAccountValidatorArgs(9.99M); // One lower for the boundary
        var validator = new MakePaymentFasterPaymentsAccountValidator(arguments);

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GivenAnAccount_WithFasterPaymentsAllowed_WithAccountBalanceEqualToPaymentAmount_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Balance = 10
        };
        var arguments = new MakePaymentFasterPaymentsAccountValidatorArgs(10);
        var validator = new MakePaymentFasterPaymentsAccountValidator(arguments);

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GivenAnAccount_WithFasterPaymentsNotAllowed_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.Bacs
        };
        var arguments = new MakePaymentFasterPaymentsAccountValidatorArgs(0);
        var validator = new MakePaymentFasterPaymentsAccountValidator(arguments);

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The account does not allow FasterPayments payments");
    }

    [Test]
    public void GivenAnAccount_WithFasterPaymentsAllowed_WithAccountBalanceLessThanPaymentAmount_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Balance = 10
        };
        var arguments = new MakePaymentFasterPaymentsAccountValidatorArgs(10.01M); // One point higher for the boundary
        var validator = new MakePaymentFasterPaymentsAccountValidator(arguments);

        // Act
        var result = validator.Validate(account);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The account balance must be higher than the requested payment amount");
    }

    [Test]
    public void GivenANullAccount_WhenValidateIsCalled_ThenReturnFailureWithError()
    {
        // Arrange
        var arguments = new MakePaymentFasterPaymentsAccountValidatorArgs(0);
        var validator = new MakePaymentFasterPaymentsAccountValidator(arguments);

        // Act
        var result = validator.Validate((Account)null);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("The Account instance is null");
    }
}
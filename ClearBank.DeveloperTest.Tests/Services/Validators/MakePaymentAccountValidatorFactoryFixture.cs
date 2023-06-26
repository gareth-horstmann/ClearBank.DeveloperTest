using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace ClearBank.DeveloperTest.Tests.Services.Validators;

[TestFixture]
internal class MakePaymentAccountValidatorFactoryFixture
{
    [Test]
    public void GivenPaymentSchemeIsBacs_WhenGetValidatorIsCalled_ThenAnInstanceOfMakePaymentBacsAccountValidatorIsReturned()
    {
        // Arrange
        var factory = new MakePaymentAccountValidatorFactory();
        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.Bacs
        };

        // Act
        var result = factory.GetValidator(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<MakePaymentBacsAccountValidator>($"the payment scheme on the {nameof(MakePaymentRequest)} instance was {nameof(PaymentScheme.Bacs)}");
    }

    [Test]
    public void GivenPaymentSchemeIsFasterPayments_WhenGetValidatorIsCalled_ThenAnInstanceOfMakePaymentFasterPaymentsAccountValidatorIsReturned()
    {
        // Arrange
        var factory = new MakePaymentAccountValidatorFactory();
        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.FasterPayments
        };

        // Act
        var result = factory.GetValidator(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<MakePaymentFasterPaymentsAccountValidator>($"the payment scheme on the {nameof(MakePaymentRequest)} instance was {nameof(PaymentScheme.FasterPayments)}");
    }

    [Test]
    public void GivenPaymentSchemeIsChaps_WhenGetValidatorIsCalled_ThenAnInstanceOfMakePaymentChapsAccountValidatorIsReturned()
    {
        // Arrange
        var factory = new MakePaymentAccountValidatorFactory();
        var request = new MakePaymentRequest
        {
            PaymentScheme = PaymentScheme.Chaps
        };

        // Act
        var result = factory.GetValidator(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<MakePaymentChapsAccountValidator>($"the payment scheme on the {nameof(MakePaymentRequest)} instance was {nameof(PaymentScheme.Chaps)}");
    }

    [Test]
    public void GivenPaymentSchemeIsUnknown_WhenGetValidatorIsCalled_ThenAnArgumentOutOfRangeExceptionShouldBeThrown()
    {
        // Arrange
        var factory = new MakePaymentAccountValidatorFactory();
        var request = new MakePaymentRequest
        {
            PaymentScheme = (PaymentScheme)10
        };

        // Act
        var action = () => factory.GetValidator(request);

        // Assert
        var result = action.Should().Throw<ArgumentOutOfRangeException>("the payment scheme value is not supported by the method")
            .WithParameterName("PaymentScheme")
            .WithMessage("Not expecting payment scheme value: 10 (Parameter 'PaymentScheme')");
    }
}

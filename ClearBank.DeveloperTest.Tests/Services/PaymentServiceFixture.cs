using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests;

[TestFixture]
internal class PaymentServiceFixture
{
    private Account _account;
    private Mock<IAccountDataStore> _accountDataStoreMock;
    private Mock<IAccountDataStoreFactory> _accountDataStoreFactoryMock;
    private Mock<IValidator<Account>> _validatoryMock;
    private Mock<IMakePaymentAccountValidatorFactory> _makePaymentAccountValidatorFactoryMock;

    [SetUp]
    public void Setup()
    {
        _account = new Account { Balance = 10 };

        _accountDataStoreMock = new Mock<IAccountDataStore>();
        _accountDataStoreMock.Setup(s => s.GetAccount(It.IsAny<string>())).Returns(_account);

        _accountDataStoreFactoryMock = new Mock<IAccountDataStoreFactory>();
        _accountDataStoreFactoryMock.Setup(s => s.Build()).Returns(_accountDataStoreMock.Object);

        _validatoryMock = new Mock<IValidator<Account>>();

        _makePaymentAccountValidatorFactoryMock = new Mock<IMakePaymentAccountValidatorFactory>();
        _makePaymentAccountValidatorFactoryMock.Setup(s => s.GetValidator(It.IsAny<MakePaymentRequest>())).Returns(_validatoryMock.Object);
    }

    [Test]
    public void GivenAPaymentRequest_WhenMakePaymentIsCalled__WithNoValidationErrors_ThenResultIsSuccessAndAccountBalanceReduced()
    {
        // Arrange
        _validatoryMock.Setup(s => s.Validate(It.IsAny<Account>())).Returns(new ValidationResult());

        var service = new PaymentService(_accountDataStoreFactoryMock.Object, _makePaymentAccountValidatorFactoryMock.Object);
        var request = new MakePaymentRequest
        {
            Amount = 5
        };

        // Act
        var result = service.MakePayment(request);

        // Assert
        result.Success.Should().BeTrue();
        _account.Balance.Should().Be(5);
        _accountDataStoreMock.Verify(mock => mock.UpdateAccount(It.Is<Account>(updateAccount => updateAccount == _account)), Times.Once());
    }


    [Test]
    public void GivenAPaymentRequest_WhenMakePaymentIsCalled__WithValidationErrors_ThenResultIsFailureAndAccountBalanceNotReduced()
    {
        // Arrange
        _validatoryMock.Setup(s => s.Validate(It.IsAny<Account>())).Returns(new ValidationResult(new[] {new ValidationFailure("AnyProperty", "test error") }));

        var service = new PaymentService(_accountDataStoreFactoryMock.Object, _makePaymentAccountValidatorFactoryMock.Object);
        var request = new MakePaymentRequest
        {
            Amount = 5
        };

        // Act
        var result = service.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
        _account.Balance.Should().Be(10);
        _accountDataStoreMock.Verify(mock => mock.UpdateAccount(It.Is<Account>(updateAccount => updateAccount == _account)), Times.Never());
    }
}

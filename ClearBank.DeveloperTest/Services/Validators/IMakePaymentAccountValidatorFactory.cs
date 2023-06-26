using ClearBank.DeveloperTest.Types;
using FluentValidation;

namespace ClearBank.DeveloperTest.Services.Validators;

public interface IMakePaymentAccountValidatorFactory
{
    IValidator<Account> GetValidator(MakePaymentRequest request);
}
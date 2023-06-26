using ClearBank.DeveloperTest.Types;
using FluentValidation;

namespace ClearBank.DeveloperTest.Services.Validators;

internal interface IMakePaymentAccountValidatorFactory
{
    IValidator<Account> GetValidator(MakePaymentRequest request);
}
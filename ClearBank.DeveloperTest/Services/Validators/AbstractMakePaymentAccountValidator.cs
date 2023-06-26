using ClearBank.DeveloperTest.Types;
using FluentValidation;
using FluentValidation.Results;

namespace ClearBank.DeveloperTest.Services.Validators;

/// <summary>
/// Abstract account validator in the scope of make a payment
/// </summary>
/// <typeparam name="T">An account (or derived type)</typeparam>
/// <remarks>
///     All of the existing validation code checks for two things:
///         - Thath the account exists (i.e. not null)
///         - That the account supports the payment scheme
///
///     This class abstracts the validation behaviour to a common 
///     class as well as allowing the behaviour and validators to 
///     operate on derived account types. 
/// </remarks>
internal class AbstractMakePaymentAccountValidator<T> : AbstractValidator<T>
    where T : Account
{
    public AbstractMakePaymentAccountValidator(AllowedPaymentSchemes paymentSchemes)
    {
        RuleFor(account => account.AllowedPaymentSchemes)
            .Must(schemes => schemes.HasFlag(paymentSchemes))
            .WithMessage($"The account does not allow {paymentSchemes} payments");
    }

    protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
    {
        if (context.InstanceToValidate == null)
        {
            result.Errors.Add(new ValidationFailure("", $"The {typeof(T).Name} instance is null"));
            return false;
        }
        return true;
    }
}

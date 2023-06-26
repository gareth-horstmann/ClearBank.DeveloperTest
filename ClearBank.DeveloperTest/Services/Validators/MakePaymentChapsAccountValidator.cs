using ClearBank.DeveloperTest.Types;
using FluentValidation;

namespace ClearBank.DeveloperTest.Services.Validators;

internal class MakePaymentChapsAccountValidator : AbstractMakePaymentAccountValidator<Account>
{
    public MakePaymentChapsAccountValidator()
        : base(AllowedPaymentSchemes.Chaps)
    {
        RuleFor(account => account.Status)
            .Equal(AccountStatus.Live)
            .WithMessage($"The account must have a status of {nameof(AccountStatus.Live)}");
    }
}

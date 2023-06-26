using ClearBank.DeveloperTest.Types;
using FluentValidation;

namespace ClearBank.DeveloperTest.Services.Validators;

internal class MakePaymentFasterPaymentsAccountValidator : AbstractMakePaymentAccountValidator<Account>
{
    public MakePaymentFasterPaymentsAccountValidator(MakePaymentFasterPaymentsAccountValidatorArgs arguments)
        : base(AllowedPaymentSchemes.FasterPayments)
    {
        RuleFor(account => account.Balance)
            .GreaterThanOrEqualTo(arguments.Amount)
            .WithMessage("The account balance must be higher than the requested payment amount");
    }
}

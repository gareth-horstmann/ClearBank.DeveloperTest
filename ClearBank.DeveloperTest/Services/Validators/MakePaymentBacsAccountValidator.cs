using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Validators;

internal class MakePaymentBacsAccountValidator : AbstractMakePaymentAccountValidator<Account>
{
    public MakePaymentBacsAccountValidator()
        : base(AllowedPaymentSchemes.Bacs)
    {
    }
}

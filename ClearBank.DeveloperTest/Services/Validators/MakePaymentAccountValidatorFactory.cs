using ClearBank.DeveloperTest.Types;
using FluentValidation;
using System;

namespace ClearBank.DeveloperTest.Services.Validators;

/// <summary>
/// Factory class for the validators.
/// </summary>
/// <remarks>
///     This class returns an IValidator<Account> based on the payment scheme on the 
///     MakePaymentRequest request.  This abstracts the creation of the validators 
///     so that the consumer only cares that it will be getting a validator.  This 
///     makes the validation extensible so we can add additional validators without 
///     changing the calling code.
/// </remarks>
public class MakePaymentAccountValidatorFactory : IMakePaymentAccountValidatorFactory
{
    public IValidator<Account> GetValidator(MakePaymentRequest request)
    {
        return request.PaymentScheme switch
        {
            PaymentScheme.Bacs => new MakePaymentBacsAccountValidator(),
            PaymentScheme.FasterPayments => new MakePaymentFasterPaymentsAccountValidator(new MakePaymentFasterPaymentsAccountValidatorArgs(request.Amount)),
            PaymentScheme.Chaps => new MakePaymentChapsAccountValidator(),
            _ => throw new ArgumentOutOfRangeException(nameof(request.PaymentScheme), $"Not expecting payment scheme value: {request.PaymentScheme}"),
        };
    }
}

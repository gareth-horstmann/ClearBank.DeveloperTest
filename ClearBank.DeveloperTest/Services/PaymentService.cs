using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

// The accessibility could be hanged internal. This means that the payment service is only accessible
// externally through the IPaymentService contract.  If an IoC container is introduced, the services can
// be registered and accessed through the IoC container, which will completely hide the implementation
// details from the consumer.
public class PaymentService : IPaymentService
{
    private readonly IAccountDataStore _accountDataStore;
    private readonly IMakePaymentAccountValidatorFactory _makePaymentAccountValidatorFactory;

    public PaymentService(IAccountDataStoreFactory dataStoreFactory, IMakePaymentAccountValidatorFactory makePaymentAccountValidatorFactory)
    {
        // A factory is used to construct the data store here, but the data store should be injected
        // from an IoC container (please see remarks in the AccountDataStoreFactory class)
        _accountDataStore = dataStoreFactory.Build();
        _makePaymentAccountValidatorFactory = makePaymentAccountValidatorFactory;
    }

    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
        // There are no formal requirements for this method, and the task was to refactor
        // the existing implementation, so the creditor account is not considered in the
        // refactor.  The assumption is that the creditor could be a local account, or it
        // could be an account on a different system, which is out of scope.
        var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

        var validator = _makePaymentAccountValidatorFactory.GetValidator(request);
        var validationResult = validator.Validate(account);

        if (validationResult.IsValid)
        {
            account.Debit(request.Amount);
            _accountDataStore.UpdateAccount(account);
        }

        return new MakePaymentResult
        {
            Success = validationResult.IsValid
        };
    }
}

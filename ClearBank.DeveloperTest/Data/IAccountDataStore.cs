using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data;

internal interface IAccountDataStore
{
    Account GetAccount(string accountNumber);
    void UpdateAccount(Account account);
}


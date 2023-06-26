namespace ClearBank.DeveloperTest.Data;

internal interface IAccountDataStoreFactory
{
    IAccountDataStore Build();
}
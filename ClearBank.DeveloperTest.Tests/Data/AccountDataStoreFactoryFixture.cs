using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests.Data;

[TestFixture]
internal class AccountDataStoreFactoryFixture
{
    private Mock<IOptions<DatabaseConfiguration>> _dataBaseConfigurationOptionsMock;
    private MemoryCache _memoryCache;

    [SetUp]
    public void Setup()
    {
        _dataBaseConfigurationOptionsMock = new Mock<IOptions<DatabaseConfiguration>>();

        var options = new MemoryCacheOptions();
        var optionsMock = new Mock<IOptions<MemoryCacheOptions>>();
        optionsMock.Setup(s => s.Value).Returns(options);
        // An instance of MemoryCache is used here, instead of Mock<IMemoryCache>, because
        // the methods used on IMemoryCache are extension methods, which Moq cannot stub.
        _memoryCache = new MemoryCache(optionsMock.Object);
    }

    [Test]
    public void GivenDataStoreTypeIsBackup_WhenBuildIsCalled_ThenTheBackupAccountDataStoreShouldBeReturned()
    {
        // Arrange
        var dataBaseConfiguration = new DatabaseConfiguration { DataStoreType = DataStoreType.Backup };
        _dataBaseConfigurationOptionsMock.Setup(s => s.Value).Returns(dataBaseConfiguration);

        var factory = new AccountDataStoreFactory(_dataBaseConfigurationOptionsMock.Object, _memoryCache);

        // Act
        var result = factory.Build();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BackupAccountDataStore>("the configuration for the application has been set up to use the backup data store");
    }

    [Test]
    public void GivenDataStoreTypeIsLive_WhenBuildIsCalled_ThenTheLiveAccountDataStoreShouldBeReturned()
    {
        // Arrange
        var dataBaseConfiguration = new DatabaseConfiguration { DataStoreType = DataStoreType.Live };
        _dataBaseConfigurationOptionsMock.Setup(s => s.Value).Returns(dataBaseConfiguration);

        var factory = new AccountDataStoreFactory(_dataBaseConfigurationOptionsMock.Object, _memoryCache);

        // Act
        var result = factory.Build();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<AccountDataStore>("the configuration for the application has been set up to use the live data store");
    }

    [Test]
    public void GivenTheDataStoreHasAlreadyBeenBuilt_WhenBuildIsCalled_ThenReturnTheExistingDataStoreInstance()
    {
        // Arrange
        var dataBaseConfiguration = new DatabaseConfiguration { DataStoreType = DataStoreType.Live };
        _dataBaseConfigurationOptionsMock.Setup(s => s.Value).Returns(dataBaseConfiguration);

        var factory = new AccountDataStoreFactory(_dataBaseConfigurationOptionsMock.Object, _memoryCache);
        var firstCallResult = factory.Build();

        // Act
        var secondCallResult = factory.Build();

        // Assert
        firstCallResult.Should().NotBeNull();
        secondCallResult.Should().NotBeNull();
        secondCallResult.Should().BeSameAs(firstCallResult, "the exiting data store instance should be used");
    }
}

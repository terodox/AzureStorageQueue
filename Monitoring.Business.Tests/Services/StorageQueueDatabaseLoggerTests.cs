using AzureStorageQueues.Contracts;
using AzureStorageQueues.Entities;
using Monitoring.Business.Services;
using Monitoring.Business.Tests.Helpers;
using Monitoring.Entities.Contracts;
using Monitoring.Entities.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Monitoring.Business.Tests.Services
{
	/// <summary>
	/// Summary description for StorageQueueDatabaseLoggerTests
	/// </summary>
	[TestFixture]
	public class GivenAStorageQueueDatabaseLogger
	{
		private StorageQueueDatabaseLogger _sut;
		private Mock<IAzureStorageQueueService> _azureStorageQueueServiceMock;
		private Mock<IMonitoringContext> _monitoringContextMock;

		[SetUp]
		public void InitializeTest()
		{
			_azureStorageQueueServiceMock = MockAzureStorageQueueService();
			_monitoringContextMock =MockMonitoringContextHelper.MockMonitoringContext();
			_sut = new StorageQueueDatabaseLogger(_azureStorageQueueServiceMock.Object, _monitoringContextMock.Object);
		}

		[TearDown]
		public void CleanupTest()
		{
			_sut = null;
			_azureStorageQueueServiceMock = null;
			_monitoringContextMock = null;
		}

		#region Setup Mocks
		private Mock<IAzureStorageQueueService> MockAzureStorageQueueService()
		{
			var mock = new Mock<IAzureStorageQueueService>();

			mock.Setup(asqs => asqs.GetStorageQueues())
				.Returns(GenerateStorageQueueEnumerable().ToArray());

			return mock;
		}

		private IEnumerable<StorageQueue> GenerateStorageQueueEnumerable(int count = 3, int startingIndex = 0)
		{
			return Enumerable.Range(startingIndex, count)
				.Select(i => new StorageQueue(
					"Storage Queue" + i,
					new Uri("http://storage.queue" + i + ".com"),
					new Dictionary<string, string>(),
					i
					));
		}
		#endregion

		[Test]
		public void WhenInstatiatedIDoNotThrow() { }

		[Test]
		public void WhenLogAllStorageQueuesToDatabaseThenDoNotThrow()
		{
			_sut.LogAllStorageQueuesToDatabase();
		}
	}
}

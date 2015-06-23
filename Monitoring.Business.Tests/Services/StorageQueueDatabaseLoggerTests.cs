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
		private MockMonitoringContextInfo _monitoringContextMockInfo;

		[TearDown]
		public void CleanupTest()
		{
			_sut = null;
			_azureStorageQueueServiceMock = null;
			_monitoringContextMockInfo = null;
		}

		#region Initialization Helpers
		private void InitializeTestWithNewData()
		{
			_azureStorageQueueServiceMock = MockAzureStorageQueueServiceWithNewData();
			_monitoringContextMockInfo = MockMonitoringContextHelper.GenerateMockMonitoringContextInfo();
			_sut = new StorageQueueDatabaseLogger(
				_azureStorageQueueServiceMock.Object,
				_monitoringContextMockInfo.MonitoringContextMock.Object);
		}

		private void InitializeTestWithExistingData()
		{
			_azureStorageQueueServiceMock = MockAzureStorageQueueServiceWithExistingData();
			_monitoringContextMockInfo = MockMonitoringContextHelper.GenerateMockMonitoringContextInfo();
			_sut = new StorageQueueDatabaseLogger(
				_azureStorageQueueServiceMock.Object,
				_monitoringContextMockInfo.MonitoringContextMock.Object);
		}
		#endregion

		#region Setup Mocks
		private Mock<IAzureStorageQueueService> MockAzureStorageQueueServiceWithNewData()
		{
			var mock = new Mock<IAzureStorageQueueService>();

			mock.Setup(asqs => asqs.GetStorageQueues())
				.Returns(GenerateStorageQueueEnumerable().ToArray());

			return mock;
		}

		private Mock<IAzureStorageQueueService> MockAzureStorageQueueServiceWithExistingData()
		{
			var mock = new Mock<IAzureStorageQueueService>();

			mock.Setup(asqs => asqs.GetStorageQueues())
				.Returns(
					GenerateStorageQueueEnumerable(
						queueName: MockMonitoringContextHelper.FAKE_QUEUE_NAME,
						queueUri: MockMonitoringContextHelper.FAKE_QUEUE_URI)
					.ToArray());

			return mock;
		}

		private IEnumerable<StorageQueue> GenerateStorageQueueEnumerable(int count = 3, int startingIndex = 0, string queueName = "Storage Queue", string queueUri = "http://www.storagequeue{0}.com")
		{
			return Enumerable.Range(startingIndex, count)
				.Select(i => new StorageQueue(
					queueName + i,
					new Uri(string.Format(queueUri, i)),
					new Dictionary<string, string>(),
					i
					));
		}
		#endregion

		[Test]
		public void WhenInstatiatedIDoNotThrow() 
		{
			// Arrange + Act
			InitializeTestWithNewData();
		}

		[Test]
		public void WhenLogAllStorageQueuesToDatabaseThenDoNotThrow()
		{
			// Arrange 
			InitializeTestWithNewData();

			// Act
			_sut.LogAllStorageQueuesToDatabase();
		}

		[Test]
		public void WhenLogAllStorageQueuesToDatabaseAndTheDatabaseIsEmptyThenAllFoundStorageQueuesAreAdded()
		{
			// Arrange
			InitializeTestWithNewData();
			_monitoringContextMockInfo = MockMonitoringContextHelper.GenerateEmptyMockMonitoringContextInfo();
			_sut = new StorageQueueDatabaseLogger(
				_azureStorageQueueServiceMock.Object,
				_monitoringContextMockInfo.MonitoringContextMock.Object
				);

			// Act
			_sut.LogAllStorageQueuesToDatabase();

			// Assert
			_monitoringContextMockInfo.QueueDbSetMock.Verify(q => q.Add(It.IsAny<Queue>()), Times.Exactly(3));
		}

		[Test]
		public void WhenLogAllStorageQueuesToDatabaseAndTheDatabaseHasAllEntriesThenNoNewEntitiesAreAdded()
		{
			// Arrange
			InitializeTestWithExistingData();

			// Act
			_sut.LogAllStorageQueuesToDatabase();

			// Assert
			_monitoringContextMockInfo.QueueDbSetMock.Verify(q => q.Add(It.IsAny<Queue>()), Times.Never);
		}

		[Test]
		public void WhenLogAllStorageQueuesToDatabaseAndTheDatabaseHasAllEntriesThenAllEntitiesAreUpdated()
		{
			// Arrange
			InitializeTestWithExistingData();

			// Act
			_sut.LogAllStorageQueuesToDatabase();

			// Assert
			_monitoringContextMockInfo.QueueDbSetMock.Verify(q => q.Add(It.IsAny<Queue>()), Times.Exactly(3));
		}
	}
}

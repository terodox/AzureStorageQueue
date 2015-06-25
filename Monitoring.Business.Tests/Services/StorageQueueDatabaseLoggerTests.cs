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
		private Mock<IQueueBusiness> _queueBusinessMock;
		

		[TearDown]
		public void CleanupTest()
		{
			_sut = null;
			_azureStorageQueueServiceMock = null;
		}

		#region Initialization Helpers
		private void InitializeTestWithNewData()
		{
			_azureStorageQueueServiceMock = MockAzureStorageQueueServiceWithNewData();
			_queueBusinessMock = MockQueueBusiness();
			_sut = new StorageQueueDatabaseLogger(
				_azureStorageQueueServiceMock.Object,
				_queueBusinessMock.Object);
		}

		private void InitializeTestWithExistingData()
		{
			_azureStorageQueueServiceMock = MockAzureStorageQueueServiceWithExistingData();
			_queueBusinessMock = MockQueueBusiness();
			_sut = new StorageQueueDatabaseLogger(
				_azureStorageQueueServiceMock.Object,
				_queueBusinessMock.Object);
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
						queueUri: MockMonitoringContextHelper.FAKE_QUEUE_URI,
						queueSizeOffset: 10)
					.ToArray());

			return mock;
		}

		private IEnumerable<StorageQueue> GenerateStorageQueueEnumerable(
			int count = 3, 
			int startingIndex = 0, 
			string queueName = "Storage Queue", 
			string queueUri = "http://www.storagequeue{0}.com",
			int queueSizeOffset = 0)
		{
			return Enumerable.Range(startingIndex, count)
				.Select(i => new StorageQueue(
					queueName + i,
					new Uri(string.Format(queueUri, i)),
					new Dictionary<string, string>(),
					i + queueSizeOffset
					));
		}

		private Mock<IQueueBusiness> MockQueueBusiness()
		{
			var mock = new Mock<IQueueBusiness>();

			SetupFindByNameForRequiredIndices(mock);

			return mock;
		}

		private void SetupFindByNameForRequiredIndices(Mock<IQueueBusiness> mock)
		{
			Enumerable.Range(0, 3)
				.Select(index =>
				{
					mock
						.Setup(qb => qb.FindByName(It.Is<string>(s => s == MockMonitoringContextHelper.FAKE_QUEUE_NAME + index)))
						.Returns(GenerateQueueArrayForId(index));
					return index;
				})
				.ToArray();
		}

		private Queue[] GenerateQueueArrayForId(int index)
		{
			return new[] { 
					new Queue()
					{
						Id = 1,
						Name = MockMonitoringContextHelper.FAKE_QUEUE_NAME + index,
						Uri = MockMonitoringContextHelper.FAKE_QUEUE_URI + index,
						ItemCount = index,
						Created = DateTime.Now,
						Updated = DateTime.Now
					}
				};
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
			_queueBusinessMock = MockQueueBusiness();
			_sut = new StorageQueueDatabaseLogger(
				_azureStorageQueueServiceMock.Object,
				_queueBusinessMock.Object
				);

			// Act
			_sut.LogAllStorageQueuesToDatabase();

			// Assert
			_queueBusinessMock.Verify(q => q.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(3));
		}

		[Test]
		public void WhenLogAllStorageQueuesToDatabaseAndTheDatabaseHasAllEntriesThenNoNewEntitiesAreAdded()
		{
			// Arrange
			InitializeTestWithExistingData();

			// Act
			_sut.LogAllStorageQueuesToDatabase();

			// Assert
			_queueBusinessMock.Verify(q => q.Insert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
		}

		[Test]
		public void WhenLogAllStorageQueuesToDatabaseAndTheDatabaseHasAllEntriesThenAllEntitiesAreUpdated()
		{
			// Arrange
			InitializeTestWithExistingData();

			// Act
			_sut.LogAllStorageQueuesToDatabase();

			// Assert
			_queueBusinessMock.Verify(q => q.Update(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
		}
	}
}

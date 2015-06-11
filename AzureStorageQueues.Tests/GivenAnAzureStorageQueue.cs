using Microsoft.WindowsAzure.Storage;
using NUnit.Framework;
using System;
using System.Linq;


namespace AzureStorageQueues.Tests
{
	/// <summary>
	/// This class is a full on integration test with Azure.  The referenced account in VALID_STORAGE_ACCOUNT
	/// must contain the following queues for all tests to pass:
	/// - queue-image
	/// - queue-eoi
	/// </summary>
	[TestFixture]
	public class GivenAnAzureStorageQueue
	{
		private const string VALID_STORAGE_ACCOUNT = @"DefaultEndpointsProtocol=https;AccountName=sudevandy;AccountKey=AN8p+MRvGZaQbiu3o+evEZ2HFWc5zG7tgfGOU6nO3PGaV9mv3ifneVzXpdB4FpemqcksZYGAhUdrz7rByJl7pQ==";
		private const string INVALID_STORAGE_ACCOUNT = @"DefaultEndpointsProtocol=https;AccountName=fakeAccountName;AccountKey=ZmFrZUFjY291bnRLZXk=";

		#region Helpers
		private AzureStorageQueueService CreateAzureStorageQueueService(string storageAccountString)
		{
			return new AzureStorageQueueService(storageAccountString, 1);
		}
		#endregion

		[Test]
		public void WhenInstantiatedWithValidStorageConnectionStringThenItDoesNotThrow()
		{
			//Act + Assert
			Assert.DoesNotThrow(() => CreateAzureStorageQueueService(VALID_STORAGE_ACCOUNT));
		}

		[Test]
		public void WhenFetchingQueuesWithValidConnectionStringThenDoesNotThrow()
		{
			//Arrange
			var sut = CreateAzureStorageQueueService(VALID_STORAGE_ACCOUNT);

			//Act + Assert
			Assert.DoesNotThrow(() => sut.GetStorageQueues());
		}

		[Test]
		[Category("Long running")]
		public void WhenFetchingQueuesWithInvalidConnectionStringThenItThrows()
		{
			//Arrange
			var sut = CreateAzureStorageQueueService(INVALID_STORAGE_ACCOUNT);

			//Act + Assert
			Assert.Throws(typeof(StorageException), () => sut.GetStorageQueues());
		}

		[Test]
		public void WhenFetchingQueuesWithValidConnectionStringThenExpectedQueuesAreReturned()
		{
			//Arrange
			var sut = CreateAzureStorageQueueService(VALID_STORAGE_ACCOUNT);

			//Act
			var queues = sut.GetStorageQueues();
			var filteredQueues = queues
				.Where(q => q.Name == "queue-image" || q.Name == "queue-eoi")
				.ToArray();

			//Assert
			Assert.AreEqual(2, filteredQueues.Length);
		}
	}
}

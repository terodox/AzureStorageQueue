using AzureStorageQueues.Contracts;
using AzureStorageQueues.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageQueues
{
	public class AzureStorageQueueService : IAzureStorageQueueService
	{
		private readonly string _storageConnectionString;

		private CloudStorageAccount _storageAccount;
		private CloudQueueClient _queueServiceClient;


		public AzureStorageQueueService(string storageConnectionString, int timeoutInSeconds)
		{
			_storageConnectionString = storageConnectionString;

			// Order of operations is significant here, storage account must be initialized first
			_storageAccount = InitializeStorageAccount();
			_queueServiceClient = InitializeQueueClientService(timeoutInSeconds);
		}

		private CloudStorageAccount InitializeStorageAccount()
		{
			return CloudStorageAccount.Parse(_storageConnectionString);
		}

		private CloudQueueClient InitializeQueueClientService(int timeoutInSeconds)
		{
			var queueServiceClient = _storageAccount.CreateCloudQueueClient();
			queueServiceClient.DefaultRequestOptions.ServerTimeout = new TimeSpan(0, 0, timeoutInSeconds);
			return queueServiceClient;
		}

		public StorageQueue[] GetStorageQueues()
		{
			var queues = _queueServiceClient.ListQueues(queueListingDetails: QueueListingDetails.Metadata);
			var queueEntities = new List<StorageQueue>();
			foreach(var queue in queues)
			{
				var queueEntity = CreateAzureQueueEntity(queue.Name, queue.Uri, queue.Metadata, queue.ApproximateMessageCount);
				queueEntities.Add(queueEntity);
			}
			return queueEntities.ToArray();
		}

		private StorageQueue CreateAzureQueueEntity(string queueName, Uri uri, IDictionary<string, string> metadata, int? approximateMessageCount)
		{
			return new StorageQueue(queueName, uri, metadata, approximateMessageCount);
		}
	}
}

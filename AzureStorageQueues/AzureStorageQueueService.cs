using AzureStorageQueues.Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
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
		private string _storageAccountName;
		private string _storageAccountKey;

		public CloudStorageAccount _storageAccount;

		public AzureStorageQueueService(string storageAccountName, string storageAccountKey)
		{
			_storageAccountName = storageAccountName;
			_storageAccountKey = storageAccountKey;

			Initialize();

			var queueServiceClient = _storageAccount.CreateCloudQueueClient();
			var queueList = queueServiceClient.ListQueues(queueListingDetails: QueueListingDetails.Metadata);
		}

		private void Initialize()
		{
			var accountCredentials = new StorageCredentials(_storageAccountName, _storageAccountKey);
			_storageAccount = new CloudStorageAccount(accountCredentials, true);

			// Added a comment
		}
	}
}

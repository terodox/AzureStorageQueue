using AzureStorageQueues;
using AzureStorageQueues.Contracts;
using AzureStorageQueues.Entities;
using Monitoring.Entities.Context;
using Monitoring.Entities.Contracts;
using Monitoring.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Business.Services
{
	public class StorageQueueDatabaseLogger
	{
		private IAzureStorageQueueService _storageQueueService;
		private IQueueBusiness _queueBusiness;

		public StorageQueueDatabaseLogger(
			IAzureStorageQueueService storageQueueService, IQueueBusiness queueBusiness)
		{
			_storageQueueService = storageQueueService;
			_queueBusiness = queueBusiness;
		}

		public void LogAllStorageQueuesToDatabase()
		{
			var storageQueues = GetQueueInfo();

			HandleStorageQueues(storageQueues);
		}

		private void HandleStorageQueues(IEnumerable<StorageQueue> storageQueues)
		{
			foreach (var storageQueue in storageQueues)
			{
				var queueEntityQuery = _queueBusiness.FindByName(storageQueue.Name);
				if (queueEntityQuery.Count() == 0)
				{
					AddStorageQueueToDatabase(storageQueue);
				}
				else
				{
					UpdateStorageQueueInDatabase(queueEntityQuery.First().Id, storageQueue);
				}

			}
		}

		private void AddStorageQueueToDatabase(StorageQueue storageQueue)
		{
			_queueBusiness.Insert(GenerateQueueEntity(storageQueue));
		}

		private void UpdateStorageQueueInDatabase(int queueId, StorageQueue storageQueue)
		{
			_queueBusiness.Update(queueId, storageQueue.ApproximateMessageCount);
		}

		private IEnumerable<StorageQueue> GetQueueInfo()
		{
			return _storageQueueService.GetStorageQueues();
		}

		private Queue GenerateQueueEntity(StorageQueue storageQueue)
		{
			return new Queue()
			{
				Name = storageQueue.Name,
				Uri = PrepareUriForDatabase(storageQueue.Uri),
				ItemCount = storageQueue.ApproximateMessageCount,
				Created = DateTime.Now,
				Updated = DateTime.Now
			};
		}

		public string PrepareUriForDatabase(Uri uri)
		{
			return uri.ToString() ?? "";
		}
	}
}

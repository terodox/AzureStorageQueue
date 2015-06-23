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
		private IMonitoringContext _monitoringContext;

		public StorageQueueDatabaseLogger(IAzureStorageQueueService storageQueueService, IMonitoringContext monitoringContext)
		{
			_storageQueueService = storageQueueService;
			_monitoringContext = monitoringContext;
		}

		public void LogAllStorageQueuesToDatabase()
		{
			var storageQueues = GetQueueInfo();

			HandleStorageQueues(storageQueues, _monitoringContext);
			_monitoringContext.SaveChanges();
		}

		private void HandleStorageQueues(IEnumerable<StorageQueue> storageQueues, IMonitoringContext ctx)
		{
			foreach (var storageQueue in storageQueues)
			{
				var queueEntityQuery = ctx.Queues.Where(q => q.Name == storageQueue.Name);
				if (queueEntityQuery.Count() == 0)
				{
					AddStorageQueueToDatabase(ctx, storageQueue);
				}
				else
				{
					UpdateStorageQueueInDatabase(storageQueue, queueEntityQuery);
				}

			}
		}

		private void AddStorageQueueToDatabase(IMonitoringContext ctx, StorageQueue storageQueue)
		{
			ctx.Queues.Add(GenerateQueueEntity(storageQueue));
		}

		private void UpdateStorageQueueInDatabase(StorageQueue storageQueue, IQueryable<Queue> queueEntityQuery)
		{
			// Potentially need to abstract this update to a business class to allow for better testing of behavior...
			var queueEntity = queueEntityQuery.First();
			queueEntity.ItemCount = storageQueue.ApproximateMessageCount;
			queueEntity.Uri = PrepareUriForDatabase(storageQueue.Uri);
			queueEntity.Updated = DateTime.Now;
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

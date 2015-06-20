using AzureStorageQueues;
using AzureStorageQueues.Contracts;
using AzureStorageQueues.Entities;
using Monitoring.Entities.Context;
using Monitoring.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Console.Services
{
	public class StorageQueueDatabaseLogger
	{
		private IAzureStorageQueueService _storageQueueService;

		public StorageQueueDatabaseLogger(IAzureStorageQueueService storageQueueService)
		{
			_storageQueueService = storageQueueService;
		}

		public void LogAllStorageQueuesToDatabase()
		{
			var storageQueues = GetQueueInfo();

			using (var ctx = new MonitoringContext())
			{
				HandleStorageQueues(storageQueues, ctx);
				ctx.SaveChanges();
			}
		}

		private void HandleStorageQueues(IEnumerable<StorageQueue> storageQueues, MonitoringContext ctx)
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

		private void AddStorageQueueToDatabase(MonitoringContext ctx, StorageQueue storageQueue)
		{
			ctx.Queues.Add(GenerateQueueEntity(storageQueue));
		}

		private void UpdateStorageQueueInDatabase(StorageQueue storageQueue, IQueryable<Queue> queueEntityQuery)
		{
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

using AzureStorageQueues;
using AzureStorageQueues.Entities;
using Monitoring.Business.Services;
using Monitoring.Entities.Context;
using Monitoring.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var azureStorageQueueService = new AzureStorageQueueService(
				Constants.STORAGE_CONNECTION_STRING, 
				Constants.STORAGE_TIMEOUT_IN_SECONDS);

			using(var monitoringContext = new MonitoringContext())
			{
				var queueBusiness = new QueueBusiness(monitoringContext);
				var storageQueueLogger = new StorageQueueDatabaseLogger(azureStorageQueueService, queueBusiness);
				storageQueueLogger.LogAllStorageQueuesToDatabase();
			}
		}
	}
}

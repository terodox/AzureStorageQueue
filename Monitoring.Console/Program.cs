using AzureStorageQueues;
using AzureStorageQueues.Entities;
using Monitoring.Console.Services;
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
			var storageQueueLogger = new StorageQueueDatabaseLogger();
			storageQueueLogger.LogAllStorageQueuesToDatabase();
		}
	}
}

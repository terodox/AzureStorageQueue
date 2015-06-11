using AzureStorageQueues.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageQueues.Contracts
{
	public interface IAzureStorageQueueService
	{
		StorageQueue[] GetStorageQueues();
	}
}

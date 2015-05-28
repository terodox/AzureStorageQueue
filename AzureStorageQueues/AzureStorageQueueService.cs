using AzureStorageQueues.Contracts;
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

		public AzureStorageQueueService(string storageAccountName, string storageAccountKey)
		{
			_storageAccountName = storageAccountName;
			_storageAccountKey = storageAccountKey;

			Initialize();
		}

		private void Initialize()
		{
			throw new NotImplementedException();
		}
	}
}

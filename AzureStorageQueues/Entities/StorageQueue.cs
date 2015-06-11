using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageQueues.Entities
{
	public class StorageQueue
	{
		public string Name { get; private set; }
		public Uri Uri { get; private set; }
		public IDictionary<string, string> Metadata { get; private set; }
		public int ApproximateMessageCount { get; private set; }
		
		public StorageQueue(string queueName, Uri uri, IDictionary<string, string> metadata, int? approximateMessageCount)
		{
			Name = queueName;
			Uri = uri;
			Metadata = metadata;
			ApproximateMessageCount = approximateMessageCount ?? 0;
		}
	}
}

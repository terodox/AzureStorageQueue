using System;
using NUnit.Framework;

namespace AzureStorageQueues.Tests
{
	[TestFixture]
	public class GivenAnAzureStorageQueue
	{
		[Test]
		public void WhenAnAzureStorageQueueIsInstantiatedThenItDoesNotThrow()
		{
			Assert.DoesNotThrow(() => new AzureStorageQueueService("", ""));
		}
	}
}

using Monitoring.Entities.Contracts;
using Monitoring.Entities.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Business.Tests.Helpers
{
	public class MockMonitoringContextInfo
	{
		public Mock<IMonitoringContext> MonitoringContextMock { get; set; }

		// DbSets
		public Mock<IDbSet<Queue>> QueueDbSetMock { get; set; }
	}

	public static class MockMonitoringContextHelper
	{
		public const string FAKE_QUEUE_NAME = "Fake Queue ";
		public const string FAKE_QUEUE_URI = "http://storage.queue{0}.com";

		public static MockMonitoringContextInfo GenerateMockMonitoringContextInfo()
		{
			var mock = MockMonitoringContextInfo();
			var mockQueueDbSet = MockQueuesDbSet();

			mock.SetupGet(mc => mc.Queues)
				.Returns(mockQueueDbSet.Object);

			return new MockMonitoringContextInfo()
			{
				MonitoringContextMock = mock,
				QueueDbSetMock = mockQueueDbSet
			};
		}

		public static MockMonitoringContextInfo GenerateEmptyMockMonitoringContextInfo()
		{
			var mock = MockMonitoringContextInfo();
			var mockQueueDbSet = MockEmptyQueuesDbSet();

			mock.SetupGet(mc => mc.Queues)
				.Returns(mockQueueDbSet.Object);

			return new MockMonitoringContextInfo()
			{
				MonitoringContextMock = mock,
				QueueDbSetMock = mockQueueDbSet
			};
		}

		public static Mock<IMonitoringContext> MockMonitoringContextInfo()
		{
			var mock = new Mock<IMonitoringContext>();

			return mock;
		}

		public static Mock<IDbSet<Queue>> MockQueuesDbSet()
		{
			var queueQueryable = GenerateQueueEnumerable().AsQueryable();

			return GenerateMockIDbSet(queueQueryable);
		}

		public static Mock<IDbSet<Queue>> MockEmptyQueuesDbSet()
		{
			var queueQueryable = GenerateQueueEnumerable(0, 0).AsQueryable();

			return GenerateMockIDbSet(queueQueryable);
		}

		public static Mock<IDbSet<T>> GenerateMockIDbSet<T>(IQueryable<T> queryable) where T : class
		{
			var mock = new Mock<IDbSet<T>>();

			mock.As<IQueryable<T>>().Setup(m => m.Provider)
				.Returns(queryable.Provider);
			mock.As<IQueryable<T>>().Setup(m => m.Expression)
				.Returns(queryable.Expression);
			mock.As<IQueryable<T>>().Setup(m => m.ElementType)
				.Returns(queryable.ElementType);
			mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
				.Returns(queryable.GetEnumerator());

			return mock;
		}

		public static IEnumerable<Queue> GenerateQueueEnumerable(
			int count = 10, 
			int startingIndex = 0, 
			string queueName = FAKE_QUEUE_NAME, 
			string queueUri = FAKE_QUEUE_URI)
		{
			return Enumerable.Range(startingIndex, count)
				.Select(i => new Queue()
				{
					Created = new DateTime(2000, 1, 1, 0, 0, 0),
					Updated = new DateTime(2000, 1, 1, 0, 0, 0),
					Id = i,
					ItemCount = i,
					Name = queueName + i,
					Uri = string.Format(queueUri, i)
				});
		}
	}
}

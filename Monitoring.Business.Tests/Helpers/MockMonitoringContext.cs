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
	public static class MockMonitoringContextHelper
	{
		public static Mock<IMonitoringContext> MockMonitoringContext()
		{
			var mock = new Mock<IMonitoringContext>();
			var mockDbSet = MockQueuesDbSet();

			mock.SetupGet(mc => mc.Queues)
				.Returns(mockDbSet.Object);

			return mock;
		}

		public static Mock<IDbSet<Queue>> MockQueuesDbSet()
		{
			var queueQueryable = GenerateQueueEnumerable().AsQueryable();

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

		public static IEnumerable<Queue> GenerateQueueEnumerable(int count = 3, int startingIndex = 0)
		{
			return Enumerable.Range(startingIndex, count)
				.Select(i => new Queue()
				{
					Created = new DateTime(2000, 1, 1, 0, 0, 0),
					Updated = new DateTime(2000, 1, 1, 0, 0, 0),
					Id = i,
					ItemCount = i,
					Name = "Fake Queue " + i,
					Uri = "http://storage.queue" + i + ".com"
				});
		}
	}
}

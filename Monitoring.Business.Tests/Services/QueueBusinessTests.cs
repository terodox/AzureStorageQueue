using Monitoring.Business.Services;
using Monitoring.Business.Tests.Helpers;
using Monitoring.Entities.Entities;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monitoring.Business.Tests.Services
{
	[TestFixture]
	public class GivenAQueueBusiness
	{
		private QueueBusiness _sut;
		private MockMonitoringContextInfo _monitoringContextMock;

		#region Test Helpers
		public void InitializeQueueBusiness()
		{
			_monitoringContextMock = MockMonitoringContextHelper.GenerateMockMonitoringContextInfo();
			_sut = new QueueBusiness(_monitoringContextMock.MonitoringContextMock.Object);
		}
		#endregion

		[Test]
		public void WhenInitializedThenDoNotThrow()
		{
			Assert.DoesNotThrow(new TestDelegate(InitializeQueueBusiness));
		}

		[Test]
		public void WhenFindThenMonitoringContextReturnsExpectedValue()
		{
			// Arrange
			InitializeQueueBusiness();

			// Act
			var result = _sut.Find(1);

			// Assert
			Assert.AreEqual(1, result.Id);
		}

		[Test]
		public void WhenFindAndValueDoesntExistThenDoNotThrow()
		{
			// Arrange
			InitializeQueueBusiness();

			// Act
			var result = _sut.Find(1000);
		}

		[Test]
		public void WhenGetThenMonitoringContextReturnsExpectedValue()
		{
			// Arrange
			InitializeQueueBusiness();

			// Act
			var result = _sut.Find(1);

			// Assert
			Assert.AreEqual(1, result.Id);
		}

		[Test]
		public void WhenGEtAndValueDoesntExistThenThrow()
		{
			// Arrange
			InitializeQueueBusiness();

			// Act
			Assert.Throws<InvalidOperationException>(new TestDelegate(() => { _sut.Get(1000); }));
		}

		[Test]
		public void WhenFindByNameThenMonitoringContextReturnsExpectedValue()
		{
			// Arrange
			InitializeQueueBusiness();

			// Act
			var result = _sut.FindByName(MockMonitoringContextHelper.FAKE_QUEUE_NAME + "0");

			// Assert
			Assert.AreEqual(0, result[0].Id);
		}

		[Test]
		public void WhenFindByNameAndNameDoesntExistThenDoNotThrow()
		{
			// Arrange
			InitializeQueueBusiness();

			// Act
			var result = _sut.FindByName("I don't exist");
		}

		[Test]
		public void WhenInsertThenQueueDbSetAddIsCalled()
		{
			// Arrange
			var queueName = "ThisQueueName";
			InitializeQueueBusiness();

			// Act
			_sut.Insert(queueName, "http://Idontexist.com", 0);

			// Assert
			_monitoringContextMock.QueueDbSetMock
				.Verify(qdbs => qdbs.Add(It.Is<Queue>(q => q.Name == queueName)), Times.Once);
		}

		
	}
}
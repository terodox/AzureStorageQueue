using Monitoring.Business.Contracts;
using Monitoring.Entities.Contracts;
using Monitoring.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Business.Services
{
	public class QueueBusiness : IQueueBusiness
	{
		private IMonitoringContext _monitoringContext;

		public QueueBusiness(IMonitoringContext monitoringContext)
		{
			_monitoringContext = monitoringContext;
		}

		public Queue Find(int id)
		{
			return _monitoringContext.Queues.Where(q => q.Id == id).FirstOrDefault();
		}

		public Queue[] FindByName(string queueName)
		{
			return _monitoringContext.Queues.Where(q => q.Name == queueName).ToArray();
		}

		public Queue Get(int id)
		{
			return _monitoringContext.Queues.Where(q => q.Id == id).First();
		}

		public void Insert(Queue newEntity)
		{
			_monitoringContext.Queues.Add(newEntity);
			_monitoringContext.SaveChanges();
		}

		public void Delete(Queue entityToDelete)
		{
			_monitoringContext.Queues.Remove(entityToDelete);
			_monitoringContext.SaveChanges();
		}

		public void Update(int id, int itemCount)
		{
			var entity = Get(id);
			entity.ItemCount = itemCount;
			_monitoringContext.SaveChanges();
		}
	}
}

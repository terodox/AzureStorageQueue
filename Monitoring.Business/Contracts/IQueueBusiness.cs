using Monitoring.Business.Contracts;
using Monitoring.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Business.Services
{
	public interface IQueueBusiness : IBusiness<Queue>
	{
		Queue[] FindByName(string queueName);
		void Update(int id, int itemCount);
	}
}

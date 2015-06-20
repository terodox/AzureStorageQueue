using Monitoring.Entities.Contracts;
using Monitoring.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Entities.Context
{
	public class MonitoringContext : DbContext, IMonitoringContext
	{
		public MonitoringContext() : base("name=MonitoringContext") { }

		public IDbSet<Queue> Queues { get; set; }
	}
}

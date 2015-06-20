using Monitoring.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Monitoring.Entities.Contracts
{
	public interface IMonitoringContext
	{
		IDbSet<Queue> Queues { get; }
		int SaveChanges();
	}
}

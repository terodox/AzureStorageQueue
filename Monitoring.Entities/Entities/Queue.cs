using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Entities.Entities
{
	public class Queue : BaseEntity
	{
		public string Name { get; set; }
		public int ItemCount { get; set; }
		public string Uri { get; set; }
	}
}

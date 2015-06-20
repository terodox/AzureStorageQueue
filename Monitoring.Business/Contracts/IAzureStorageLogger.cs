using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Business.Contracts
{
	public interface IAzureStorageLogger
	{
		void LogAllStorageQueuesToDatabase();
	}
}

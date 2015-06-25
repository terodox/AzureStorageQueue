using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Business.Contracts
{
	public interface IBusiness<T> where T : class
	{
		T Find(int id);
		T Get(int id);
		void Delete(int id);
	}
}

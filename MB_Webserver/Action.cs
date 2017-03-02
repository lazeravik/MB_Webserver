using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBeePlugin
{
	class Action
	{
		public string data;
		public string datatype;

		public Action(string data, string datatype)
		{
			this.data = data;
			this.datatype = datatype;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MusicBeePlugin
{
	class Request
	{
		public string filename;
		
		public Request(string filename)
		{
			this.filename = filename;
		}


		public static Request GetRequest(NetworkStream stream)
		{
			StreamReader reader = new StreamReader(stream);
			String request = reader.ReadLine();
			if(request == null) return null;
			String[] tokens = request.Split(' ');   //Split the requets using spaces

			return new Request(tokens[1]);
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MusicBeePlugin
{
	class Response
	{
		private string status = "200 OK";
		private string mime = "text/html";
		private string data;


		private Response(string status, string mime, string data)
		{
			this.data = data;
		}


		private void Send(NetworkStream stream)
		{
			StreamWriter writer = new StreamWriter(stream);
			writer.WriteLine(string.Format(
				"{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n",
				HTTPServer.SERVER_VER,
				status,
				HTTPServer.SERVER_NAME,
				mime,
				data.Length
				));
			writer.WriteLine(data);
			writer.Flush();
		}


		public static void SendResponseFromRequest(Request request, NetworkStream stream)
		{
			String data = GetFiledataFromRequest(request.filename);
			Response response;
			if (data == null)
			{
				response = new Response("404", "text/html", "<h1>Sorry we got error!</h1><p>File does not exists!</p>");
			} else
			{
				response = new Response("200 OK", "text/html", data);
			}
			response.Send(stream);
		}

		private static string GetFiledataFromRequest(string filename)
		{
			if (string.IsNullOrEmpty(filename))
				return null;

			string plugindir = Environment.CurrentDirectory + "/Plugins/web/";
			filename = (filename == "/") ? "index.html" : filename;
			filename = plugindir + filename;

			if (File.Exists(filename))
			{
				String filedata = new StreamReader(filename).ReadToEnd();         //read the first line so the while loop does not end immidietly
				return filedata;
			}
			return null;
		}
	}
}

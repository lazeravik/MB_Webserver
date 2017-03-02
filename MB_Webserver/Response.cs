using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
	public enum RequestType
	{
		ACTION,
		DATA,
	}

	class Response
	{
		private string status = "200 OK";
		private string mime = "text/html";
		private string data;
		private static MusicBeeApiInterface mbapi;


		private Response(string status, string mime, string data)
		{
			this.status = status;
			this.mime = mime;
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


		public static void SendResponseFromRequest(Request request, NetworkStream stream, MusicBeeApiInterface mbapiinterface)
		{
			mbapi = mbapiinterface;
			String data = null;
			String datatype = "text/html";

			Response response;
			if (request != null)
			{
				Action tempaction = GetFiledataFromRequest(request.filename);
				data = tempaction.data;
				datatype = tempaction.datatype;
			}
			
			if (data == null)
			{
				response = new Response("404", "text/html", "<h1>Sorry we got error!</h1><p>"+mbapi.NowPlaying_GetFileTag(MetaDataType.TrackTitle)+"</p>");
			} else
			{
				response = new Response("200 OK", datatype, data);
			}
			response.Send(stream);
		}

		private static Action GetFiledataFromRequest(string urlparam)
		{
			if (string.IsNullOrEmpty(urlparam)) return null;

			string plugindir = Environment.CurrentDirectory + "/Plugins/web/";
			string[] urlparamArray = urlparam.TrimStart('/').TrimEnd('/').Split('/');

			if(urlparamArray[0] == "action" && 1 < urlparamArray.Length)
			{
				switch (urlparamArray[1])
				{
					case "toggleplay":
						mbapi.Player_PlayPause();
						return new Action("{isPlaying:"+mbapi.Player_GetPlayState()+"}", "text/json");
					default:
						break;
				}
				return new Action("{response:unknown}", "text/json");
			}
			else //if the requested data is not some type of action then return the page
			{
				if (urlparam == "/")
				{
					urlparam = "index.html";
				}

				urlparam = plugindir + urlparam;

				if (File.Exists(urlparam))
				{
					String filedata = new StreamReader(urlparam).ReadToEnd();         //read the first line so the while loop does not end immidietly
					return new Action(filedata, "text/html");
				}
			}

			return null;
		}
	}
}

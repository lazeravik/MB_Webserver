using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace MusicBeePlugin
{
	public class HTTPServer
	{
		private TcpListener listener;
		public const string SERVER_VER = "HTTP/1.0";
		public const string SERVER_NAME = "MusicBee Webserver";
		private string PluginDir;
		private Thread serverThread;

		public HTTPServer(int port)
		{
			PluginDir = Environment.CurrentDirectory + "/Plugins/web/";
			listener = new TcpListener(IPAddress.Any, port);
		}

		public void Start()
		{

			serverThread = new Thread(new ThreadStart(Run));
			serverThread.Start();
		}

		private void Run()
		{
			listener.Start();                           //Start listening for any upcoming connections

			while (true)
			{
				TcpClient client = listener.AcceptTcpClient();
				HandleClient(client);
				client.Close();
			}
		}

		private void HandleClient(TcpClient client)
		{
			using (StreamWriter writer = new StreamWriter(client.GetStream()))
			{
				Request request = Request.GetRequest(client.GetStream());
				Response.SendResponseFromRequest(request, client.GetStream());
			}
		}
	}
}

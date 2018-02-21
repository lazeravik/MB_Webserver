using MusicBeePlugin;
using Nancy.Hosting.Self;
using System;
using System.Threading;

namespace WebServer
{
    public class Server
    {
        private Thread serverThread = null;
        private NancyHost NancyHost = null;
        private HostConfiguration NancyHostConfig;
		private NancyBootstrapper NancyBootstrap;
        private int port;
        private bool isRunning;

        public int Port {
            get { return port; }
            set { port = value; }
        }

        public bool IsRunning {
            get { return isRunning; }
        }

        public Server()
        {
			Nancy.Json.JsonSettings.MaxJsonLength = int.MaxValue;
            NancyHostConfig = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true, },
                RewriteLocalhost = true,
            };
			NancyBootstrap = new NancyBootstrapper();
        }


        public void Start(int port = 1502)
        {
			this.port = port;
			serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            if (NancyHost != null)
            {
                NancyHost.Stop();
            }

            if (serverThread != null)
            {
                if (serverThread.IsAlive)
                    serverThread.Abort();
            }
        }

        private void Run()
        {
            isRunning = true;
            string url = string.Format("http://localhost:{0}/", port);
            NancyHost = new NancyHost(NancyBootstrap, NancyHostConfig, new Uri(url));
            NancyHost.Start();
        }

    }


}

using Fleck;
using MusicBeePlugin;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WebServer
{
    class WSServer
    {
        private Thread serverThread = null;
        private Thread wsThread = null;
        private WebSocketServer socketServer;
        private List<IWebSocketConnection> allSockets;
        private int port;
        private bool isRunning;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
        }

        public WSServer(int port = 1303)
        {
            allSockets = new List<IWebSocketConnection>();
        }

        public void Start(int port = 1303)
        {
			this.port = port;
			serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            if (socketServer != null)
            {
                allSockets.ForEach(s => s.Close());
                socketServer.Dispose();
            }

            if (wsThread != null)
            {
                if (wsThread.IsAlive)
                    wsThread.Abort();
            }

            if (serverThread != null)
            {
                if (serverThread.IsAlive)
                    serverThread.Abort();
            }
        }

        public void SendMessage(string message)
        {
            foreach (var socket in allSockets.ToList())
            {
                socket.Send(message);
            }
        }

        bool IsForced = false;

        private void Run()
        {
            isRunning = true;
            socketServer = new WebSocketServer(string.Format("ws://0.0.0.0:{0}", port));
            socketServer.ListenerSocket.NoDelay = true;
            socketServer.SupportedSubProtocols = new[] { "nowplaying_data" };

            wsThread = new Thread(new ThreadStart(RunTask));
            wsThread.Start();
            GenerateResponse.WsRef = this;

            socketServer.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    allSockets.Add(socket);
                    IsForced = true;
                };
                socket.OnClose = () =>
                {
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    if (message.Length > 0)
                    {
                        GenerateResponse.HandleResponse(message);
                    }
                };
            });
        }

        private void RunTask()
        {
            while (isRunning)
            {
                SendMessage(GenerateResponse.GetNowPlayingData(IsForced));
                IsForced = false;
                Thread.Sleep(500);
            }
        }

    }
}

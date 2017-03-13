using System;
using System.Windows;
using WebServer;

namespace MusicBeePlugin
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server httpServer;
        private WSServer websocketServer;
        private int port;
        private bool isSettingVisible = true;

        public MainWindow(Server server)
        {
            InitializeComponent();
            runningIndicator.Visibility = Visibility.Hidden;
            toggleSettingPanel();

            //Create server instances!
            httpServer = new Server();
            websocketServer = new WSServer();

        }

        private void StartSrever_Click(object sender, RoutedEventArgs e)
        {
            //@todo: also check for websocket server
            if (httpServer.IsRunning)
            {
                StopServer();
            }
            else
            {
                if (!string.IsNullOrEmpty(portInput.Text.Trim()))
                {
                    port = Convert.ToInt16(portInput.Text);
                    httpServer.Port = port;
                }

                httpServer.Start();
                websocketServer.Start();

                //Visual cues for server running or stopping
                runningIndicator.Visibility = Visibility.Visible;
                notRunningIndicator.Visibility = Visibility.Hidden;
                StartSrever.Content = "Stop Server";
            }
        }

        private void StopServer()
        {
            httpServer.Stop();
            websocketServer.Stop();

            //Visual cues for server running or stopping
            runningIndicator.Visibility = Visibility.Hidden;
            notRunningIndicator.Visibility = Visibility.Visible;
            StartSrever.Content = "Start Server";
        }

        #region setting panel ui stuff

        private void serverWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "If you close this window, the server will stop working. Are you sure you want to stop the server!",
                "Are you sure?",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                StopServer();
            }
        }

        private void MoveWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void toggleSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleSettingPanel();
        }

        private void toggleSettingPanel()
        {
            if (!isSettingVisible) //Show the settings
            {
                isSettingVisible = true;
                toggleSettingsBtn.Content = "Close Settings";
                Height = 250;
            }
            else
            {
                isSettingVisible = false;
                toggleSettingsBtn.Content = "Settings";
                Height = 84;
            }
        }

        #endregion
    }
}

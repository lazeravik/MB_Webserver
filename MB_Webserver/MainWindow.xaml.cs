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
		private Plugin plugin;

		public MainWindow(Plugin plugin)
		{
            InitializeComponent();
			this.plugin = plugin;

			LoadSavedSettings();
		}

		private void LoadSavedSettings()
		{
			portInput.Text = Properties.Settings.Default.httpPort;
			wsInput.Text = Properties.Settings.Default.wsPort;
			runOnStartup_checkbox.IsChecked = Properties.Settings.Default.runOnStartup;
		}


		#region setting panel ui stuff

		private void ServerWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void MoveWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }


		#endregion

		/// <summary>
		/// Get the new user inputted settings and save it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveSettingBtn_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(
				"Do you want save and restart the server?",
				"Are you sure?",
				MessageBoxButton.YesNo);


			if (result == MessageBoxResult.No) return;

			string newHttpPort = portInput.Text.Trim();
			string newWsPort = wsInput.Text.Trim();

			if (!string.IsNullOrEmpty(newHttpPort))
			{
				Properties.Settings.Default.httpPort = newHttpPort;
			}

			if(!string.IsNullOrEmpty(newWsPort))
			{
				Properties.Settings.Default.wsPort = newWsPort;
			}

			Properties.Settings.Default.runOnStartup = runOnStartup_checkbox.IsChecked.Value;

			Properties.Settings.Default.Save();

			plugin.RestartServer();
		}
	}
}

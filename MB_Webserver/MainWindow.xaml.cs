using System.Windows;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private HTTPServer server;
		public MusicBeeApiInterface mbInterface;

		public MainWindow(HTTPServer server, MusicBeeApiInterface mbInterface)
		{
			InitializeComponent();
			this.server = server;
			this.mbInterface = mbInterface;
			server.mbApiInterface = mbInterface;
			runningIndicator.Visibility = Visibility.Hidden;
		}

		private void StartSrever_Click(object sender, RoutedEventArgs e)
		{
			server.Start();
			runningIndicator.Visibility = Visibility.Visible;
			notRunningIndicator.Visibility = Visibility.Hidden;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			mbInterface.Player_PlayPause();
			MessageBox.Show(mbInterface.Player_GetPlayState().ToString());
		}
	}
}

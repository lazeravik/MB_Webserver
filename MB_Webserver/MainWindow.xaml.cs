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
		private MusicBeeApiInterface mbInterface;
		public MainWindow(HTTPServer server, MusicBeeApiInterface mbInterface)
		{
			InitializeComponent();
			this.server = server;
			this.mbInterface = mbInterface;
			runningIndicator.Visibility = Visibility.Hidden;
		}

		private void StartSrever_Click(object sender, RoutedEventArgs e)
		{
			server.Start();
			runningIndicator.Visibility = Visibility.Visible;
			notRunningIndicator.Visibility = Visibility.Hidden;
		}
	}
}

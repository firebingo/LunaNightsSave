using LunaNightsSave.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LunaNightsSave.Windows
{
	public enum VisiblePage
	{
		MainPage = 0
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public delegate void onWindowChanged(double width, double height);
		public static event onWindowChanged OnMainWindowSizeChanged;
		private List<Control> _pages;
		private VisiblePage _visiblePage;
		public VisiblePage VisiblePage
		{
			get { return _visiblePage; }
			set
			{
				_visiblePage = value;
				ChangePage();
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			_visiblePage = VisiblePage.MainPage;
			_pages = new List<Control>
			{
				mainPage
			};
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			WindowPlacementHandler.SetPlacement(new WindowInteropHelper(this).Handle);
			OnMainWindowSizeChanged?.Invoke(this.Width, this.Height);

			mainPage.Visibility = Visibility.Visible;
		}

		private void ChangePage()
		{
			foreach (var p in _pages)
			{
				p.Visibility = Visibility.Hidden;
			}
			switch (_visiblePage)
			{
				case VisiblePage.MainPage:
					mainPage.Visibility = Visibility.Visible;
					break;
			}
		}

		private void WindowClosing(object sender, CancelEventArgs e)
		{
			WindowPlacementHandler.GetPlacement(new WindowInteropHelper(this).Handle);
		}

		private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			OnMainWindowSizeChanged?.Invoke(this.Width, this.Height);
		}

		private void TopBarMouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void MinimizeClicked(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void CloseClicked(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void WindowActivated(object sender, EventArgs e)
		{
			topGridBorder.BorderBrush = Application.Current.Resources["TopBarBorderActivated"] as SolidColorBrush;
		}

		private void WindowDeactivated(object sender, EventArgs e)
		{
			topGridBorder.BorderBrush = Application.Current.Resources["TopBarBorderDeactivated"] as SolidColorBrush;
		}
	}
}

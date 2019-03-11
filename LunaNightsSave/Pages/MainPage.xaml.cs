using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LunaNightsSave.Pages
{
	/// <summary>
	/// Interaction logic for MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{	

		public MainPage()
		{
			InitializeComponent();

			//var l = File.ReadAllLines(@"C:\Users\matth\AppData\Local\touhou_luna_nights\game0.sav");
			//var newL = new List<string>();
			//foreach (var line in l)
			//{
			//	newL.Add(Encoding.UTF8.GetString(Convert.FromBase64String(line)));
			//}
			//newL.Dump();
		}

		private void HorizontalScrollMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (sender is ScrollViewer scrollviewer)
			{
				if (e.Delta > 0)
					scrollviewer.LineLeft();
				else
					scrollviewer.LineRight();
				e.Handled = true;
			}
		}
	}
}

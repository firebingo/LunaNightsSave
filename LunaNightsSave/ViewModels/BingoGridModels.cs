using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LunaNightsSave.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NightsSaveReader;

namespace LunaNightsSave.ViewModels
{
	public class BingoTileData : Control, INotifyPropertyChanged
	{
		public int X;
		public int Y;
		public BingoTileDef TileDef;
		

		private BitmapImage _imgSrc;
		public BitmapImage ImgSrc
		{
			get
			{
				if (_imgSrc == null)
					_imgSrc = GridHelper.GetImageForBingoType(TileDef?.Type ?? null, TileDef?.Data ?? -1);
				return _imgSrc;
			}
			set
			{
				_imgSrc = value;
				NotifyPropertyChanged("ImgSrc");
			}
		}
		private Visibility _imgVisible = Visibility.Hidden;
		public Visibility ImgVisible
		{
			get => _imgVisible;
			set
			{
				_imgVisible = value;
				NotifyPropertyChanged("ImgVisible");
			}
		}

		public Visibility TileVisible
		{
			get
			{
				return (TileDef == null || !TileDef.Hidden) ? Visibility.Visible : Visibility.Hidden;
			}
		}

		public SolidColorBrush TileHoverBorder
		{
			get
			{
				return (TileDef == null || TileDef.Enabled) ? Application.Current.Resources["BingoStrokeHoverBrush"] as SolidColorBrush : Application.Current.Resources["BingoStrokeBrush"] as SolidColorBrush;
			}
		}

		public double ImgWidth
		{
			get => Width - 4;
		}
		public double ImgHeight
		{
			get => Height - 4;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}

	public enum BingoTileType
	{
		Clock,
		Knife,
		Hp,
		Mp,
		Trash,
		Statue,
		Skill,
		Key,
		Upgrade
	}

	public static class BingoTileDefs
	{
		private static readonly string _tileDefPath = "AppData/BingoDef.json";
		public static BingoTileDef[,] Tiles;

		static BingoTileDefs()
		{
			try
			{
				if (File.Exists(_tileDefPath))
				{
					var jo = JObject.Parse(File.ReadAllText(_tileDefPath));
					Tiles = jo["Tiles"].ToObject<BingoTileDef[,]>();
					return;
				}
			}
			catch(Exception ex)
			{
				ErrorTracker.CurrentError = ex.Message;
				return;
			}
			
			Tiles = new BingoTileDef[0, 0];
		}
	}

	public class BingoTileDef
	{
		public BingoTileType Type;
		public Stage Stage;
		public int Data;
		public bool Hidden;
		public bool Enabled;
	}
}

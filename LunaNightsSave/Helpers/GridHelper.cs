using LunaNightsSave.ViewModels;
using NightsSaveReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LunaNightsSave.Helpers
{
	public static class GridHelper
	{
		public static Dictionary<string, BitmapImage> BingoImages = new Dictionary<string, BitmapImage>();
		public static DateTime LastSizeUpdate { get; private set; } = DateTime.UtcNow;
		private const int _baseGridWidth = 550;
		private static int _maxGridWidth = _baseGridWidth;
		private const int _baseGridHeight = 550;
		private static int _maxGridHeight = _baseGridHeight;
		private const int _baseTileWidth = 36;
		private static int _maxTileWidth = _baseTileWidth;
		private const int _baseTileHeight = 36;
		private static int _maxTileHeight = _baseTileHeight;

		/// <summary>
		/// Checks the passed window height and width and calculates new row and column max sizes
		/// so the grid fits the window.
		/// </summary>
		/// <param name="windowWidth"></param>
		/// <param name="windowHeight"></param>
		public static void CheckSizes(int windowWidth, int windowHeight, Grid iGrid, int gridWidth, int gridHeight, List<ColumnDefinition> columns, List<RowDefinition> rows)
		{
			LastSizeUpdate = DateTime.UtcNow;
			double ratio = 1.0;
			long baseSize = 1280 * 720;
			ratio = (double)(windowWidth * windowHeight) / baseSize;
			_maxGridWidth = (int)(_baseGridWidth * ratio);
			_maxTileWidth = (int)(_baseTileWidth * ratio);
			_maxGridHeight = _maxGridWidth;
			_maxTileHeight = _maxTileWidth;
			UpdateGridSizes(iGrid, gridWidth, gridHeight, columns, rows);
		}

		/// <summary>
		/// recalculates grid sizes and updates the current grid.
		/// </summary>
		public static void UpdateGridSizes(Grid iGrid, int gridWidth, int gridHeight, List<ColumnDefinition> columns, List<RowDefinition> rows)
		{
			if (iGrid != null)
			{
				CalculateGridSizes(iGrid, gridWidth, gridHeight);
				double colWidth = iGrid.Width / gridWidth;
				double rowHeight = iGrid.Height / gridHeight;
				foreach (var col in columns)
				{
					col.Width = new GridLength(colWidth);
				}
				foreach (var row in rows)
				{
					row.Height = new GridLength(rowHeight);
				}
				foreach (var child in iGrid.Children)
				{
					if (child is Control bu)
					{
						if (bu.DataContext is BingoTileData gbd)
						{
							gbd.Width = colWidth;
							gbd.Height = rowHeight;
						}
					}
				}
			}
		}

		/// <summary>
		/// Does the caluclations for grid and column widths so that they fit the grid sizes
		/// </summary>
		public static void CalculateGridSizes(Grid iGrid, int gridWidth, int gridHeight)
		{
			iGrid.Width = 0;
			iGrid.Height = 0;
			if (gridWidth > gridHeight)
			{
				iGrid.Width = gridWidth * _maxTileWidth > _maxGridWidth ? _maxGridWidth : gridWidth * _maxTileWidth;
				iGrid.Height = (iGrid.Width) * ((float)gridHeight / (float)gridWidth > 1 ? 1 : (float)gridHeight / (float)gridWidth);
			}
			else if (gridWidth < gridHeight)
			{
				iGrid.Height = gridHeight * _maxTileHeight > _maxGridHeight ? _maxGridHeight : gridHeight * _maxTileHeight;
				iGrid.Width = (iGrid.Height) * ((float)gridWidth / (float)gridHeight > 1 ? 1 : (float)gridWidth / (float)gridHeight);
			}
			else
			{
				iGrid.Width = gridWidth * _maxTileWidth > _maxGridWidth ? _maxGridWidth : gridWidth * _maxTileWidth;
				iGrid.Height = gridHeight * _maxTileHeight > _maxGridHeight ? _maxGridHeight : gridHeight * _maxTileHeight;
			}
		}

		public static BitmapImage GetImageForBingoType(BingoTileType? type, int Data)
		{
			try
			{
				switch (type)
				{
					case BingoTileType.Clock:
						if (!BingoImages.ContainsKey("Clock"))
							BingoImages.Add("Clock", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoClock.png")));
						return BingoImages["Clock"];
					case BingoTileType.Knife:
						if (!BingoImages.ContainsKey("Knife"))
							BingoImages.Add("Knife", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoKnife.png")));
						return BingoImages["Knife"];
					case BingoTileType.Hp:
						if (!BingoImages.ContainsKey("Hp"))
							BingoImages.Add("Hp", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoHp.png")));
						return BingoImages["Hp"];
					case BingoTileType.Mp:
						if (!BingoImages.ContainsKey("Mp"))
							BingoImages.Add("Mp", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoMp.png")));
						return BingoImages["Mp"];
					case BingoTileType.Trash:
						if (!BingoImages.ContainsKey("Trash"))
							BingoImages.Add("Trash", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoTrash.png")));
						return BingoImages["Trash"];
					case BingoTileType.Key:
						{
							switch (Data)
							{
								case (int)Keys.Red:
									if (!BingoImages.ContainsKey("KeyRed"))
										BingoImages.Add("KeyRed", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoKeyRed.png")));
									return BingoImages["KeyRed"];
								case (int)Keys.Yellow:
									if (!BingoImages.ContainsKey("KeyYellow"))
										BingoImages.Add("KeyYellow", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoKeyYellow.png")));
									return BingoImages["KeyYellow"];
								case (int)Keys.Green:
									if (!BingoImages.ContainsKey("KeyGreen"))
										BingoImages.Add("KeyGreen", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoKeyGreen.png")));
									return BingoImages["KeyGreen"];
								case (int)Keys.Blue:
									if (!BingoImages.ContainsKey("KeyBlue"))
										BingoImages.Add("KeyBlue", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoKeyBlue.png")));
									return BingoImages["KeyBlue"];
								case (int)Keys.Purple:
									if (!BingoImages.ContainsKey("KeyPurple"))
										BingoImages.Add("KeyPurple", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoKeyPurple.png")));
									return BingoImages["KeyPurple"];
							}
							break;
						}
					case BingoTileType.Statue:
						{
							switch (Data)
							{
								case (int)Statues.Amethyst:
									if (!BingoImages.ContainsKey("SAmethyst"))
										BingoImages.Add("SAmethyst", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGemAmethyst.png")));
									return BingoImages["SAmethyst"];
								case (int)Statues.Turquoise:
									if (!BingoImages.ContainsKey("STurquoise"))
										BingoImages.Add("STurquoise", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGemTurquoise.png")));
									return BingoImages["STurquoise"];
								case (int)Statues.Topaz:
									if (!BingoImages.ContainsKey("STopaz"))
										BingoImages.Add("STopaz", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGemTopaz.png")));
									return BingoImages["STopaz"];
								case (int)Statues.Ruby:
									if (!BingoImages.ContainsKey("SRuby"))
										BingoImages.Add("SRuby", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGemRuby.png")));
									return BingoImages["SRuby"];
								case (int)Statues.Sapphire:
									if (!BingoImages.ContainsKey("SSapphire"))
										BingoImages.Add("SSapphire", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGemSapphire.png")));
									return BingoImages["SSapphire"];
								case (int)Statues.Emerald:
									if (!BingoImages.ContainsKey("SEmerald"))
										BingoImages.Add("SEmerald", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGemEmerald.png")));
									return BingoImages["SEmerald"];
								case (int)Statues.Diamond:
									if (!BingoImages.ContainsKey("SDiamond"))
										BingoImages.Add("SDiamond", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGemDiamond.png")));
									return BingoImages["SDiamond"];
							}
							break;
						}
					case BingoTileType.Upgrade:
						{
							switch (Data)
							{
								case (int)Upgrades.Slide:
									if (!BingoImages.ContainsKey("USlide"))
										BingoImages.Add("USlide", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoSlide.png")));
									return BingoImages["USlide"];
								case (int)Upgrades.Double:
									if (!BingoImages.ContainsKey("UDouble"))
										BingoImages.Add("UDouble", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoDouble.png")));
									return BingoImages["UDouble"];
								case (int)Upgrades.Grip:
									if (!BingoImages.ContainsKey("UGrip"))
										BingoImages.Add("UGrip", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoGrip.png")));
									return BingoImages["UGrip"];
								case (int)Upgrades.Screw:
									if (!BingoImages.ContainsKey("UScrew"))
										BingoImages.Add("UScrew", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoScrew.png")));
									return BingoImages["UScrew"];
							}
							break;
						}
				}
			}
			catch (Exception ex)
			{
				ErrorTracker.CurrentError = ex.Message;
			}

			if (!BingoImages.ContainsKey("Default"))
				BingoImages.Add("Default", new BitmapImage(new Uri("pack://application:,,,/AppData/Images/BingoDefault.png")));
			return BingoImages["Default"];
		}
	}
}

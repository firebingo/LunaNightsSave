using LunaNightsDataWin;
using LunaNightsSave.Function;
using LunaNightsSave.Helpers;
using LunaNightsSave.ViewModels;
using Microsoft.Win32;
using Newtonsoft.Json;
using NightsSaveReader;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace LunaNightsSave.Pages
{
	/// <summary>
	/// Interaction logic for MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{
		private SaveEditor _saveEditor;
		private ImagePacker _imagePacker;
		private List<Control> UpgradeGridControls = new List<Control>();
		private readonly SynchronizationContext _syncContext;
		private string _loadPath = string.Empty;
		private string _save0Path = string.Empty;
		private readonly SolidColorBrush _save0Color = new SolidColorBrush(Color.FromRgb(123, 215, 55));
		private string _save1Path = string.Empty;
		private readonly SolidColorBrush _save1Color = new SolidColorBrush(Color.FromRgb(255, 97, 117));
		private string _save2Path = string.Empty;
		private readonly SolidColorBrush _save2Color = new SolidColorBrush(Color.FromRgb(0, 119, 199));
		private System.Timers.Timer _autoSaveTimer;

		public MainPageModel PageModel;

		public MainPage()
		{
			InitializeComponent();

			_syncContext = SynchronizationContext.Current;

			PageModel = new MainPageModel(_syncContext);
			MainGrid.DataContext = PageModel;

			_loadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "touhou_luna_nights");
			if (!Directory.Exists(_loadPath))
				Directory.CreateDirectory(_loadPath);
			_save0Path = Path.Combine(_loadPath, "game0.sav");
			_save1Path = Path.Combine(_loadPath, "game1.sav");
			_save2Path = Path.Combine(_loadPath, "game2.sav");

			_saveEditor = new SaveEditor();
			_imagePacker = new ImagePacker(_syncContext);

			ErrorTracker.UpdateError += ErrorTrackerUpdateError;

			Task.Run(ProcessCurrentSaves);
		}

		private async Task ProcessCurrentSaves()
		{
			_syncContext.Post((s) => { ErrorTracker.CurrentError = "Checking Saves"; }, null);

			List<SaveSelectionModel> sav = new List<SaveSelectionModel>();
			try
			{
				if (File.Exists(_save0Path))
				{
					await _saveEditor.LoadSave(_save0Path, false);
					sav.Add(new SaveSelectionModel(0, $"Save 0 - Time: {_saveEditor.Save.SaveDate.ToString(SaveLines.DATE_FORMAT)} - Stage: {((int)SaveHelpers.StageToStageDisplay(_saveEditor.Save.Stage)).ToString()}", _save0Color));
				}
				else
					sav.Add(new SaveSelectionModel(0, $"Save 0 - New Game", _save0Color));

				if (File.Exists(_save1Path))
				{
					await _saveEditor.LoadSave(_save1Path, false);
					sav.Add(new SaveSelectionModel(1, $"Save 1 - Time: {_saveEditor.Save.SaveDate.ToString(SaveLines.DATE_FORMAT)} - Stage: {((int)SaveHelpers.StageToStageDisplay(_saveEditor.Save.Stage)).ToString()}", _save1Color));
				}
				else
					sav.Add(new SaveSelectionModel(1, $"Save 1 - New Game", _save1Color));

				if (File.Exists(_save2Path))
				{
					await _saveEditor.LoadSave(_save2Path, false);
					sav.Add(new SaveSelectionModel(2, $"Save 2 - Time: {_saveEditor.Save.SaveDate.ToString(SaveLines.DATE_FORMAT)} - Stage: {((int)SaveHelpers.StageToStageDisplay(_saveEditor.Save.Stage)).ToString()}", _save2Color));
				}
				else
					sav.Add(new SaveSelectionModel(2, $"Save 2 - New Game", _save2Color));
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => { ErrorTracker.CurrentError = ex.Message; }, null);
			}

			_syncContext.Post((s) =>
			{
				PageModel.Saves.Clear();
				foreach (var i in sav)
				{
					PageModel.Saves.Add(i);
				}
				ErrorTracker.CurrentError = string.Empty;
			}, null);

			_autoSaveTimer = new System.Timers.Timer()
			{
				AutoReset = false,
				Interval = Config.Instance.ConfigObject.AutoSaveIntervalMs
			};
			_autoSaveTimer.Elapsed += AutoSaveTimerElapsed;
			_autoSaveTimer.Start();
		}

		private void SaveSelectChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ComboBox cb)
			{
				var loadPath = string.Empty;
				switch (PageModel.Saves[cb.SelectedIndex].Index)
				{
					default:
					case 0:
						loadPath = _save0Path;
						break;
					case 1:
						loadPath = _save1Path;
						break;
					case 2:
						loadPath = _save2Path;
						break;
				}

				Task.Run(() => LoadSave(loadPath));
			}
		}

		private async Task LoadSave(string path)
		{
			await _saveEditor.LoadSave(path, true);
			await PageModel.SaveInfo.SyncFromSave(_saveEditor.Save);
			_syncContext.Post((s) =>
			{
				BuildBingoGrid();
			}, null);
		}

		//TODO: Maybe make another parent element so all this can be done without locking the UI thread until the end
		private void BuildBingoGrid()
		{
			//remove the old grid if there is one
			if (BingoGrid.Children.Count > 0)
				BingoGrid.Children.Clear();

			var gridColumns = new List<ColumnDefinition>();
			var gridRows = new List<RowDefinition>();

			GridHelper.CalculateGridSizes(BingoGrid, BingoTileDefs.Width, BingoTileDefs.Height);
			BingoGrid.HorizontalAlignment = HorizontalAlignment.Center;
			BingoGrid.VerticalAlignment = VerticalAlignment.Center;
			double colWidth = BingoGrid.Width / BingoTileDefs.Width;
			double rowHeight = BingoGrid.Height / BingoTileDefs.Height;

			for (int x = 0; x < BingoTileDefs.Width; ++x)
			{
				ColumnDefinition column = new ColumnDefinition();
				gridColumns.Add(column);
			}
			foreach (var col in gridColumns)
			{
				col.Width = new GridLength(colWidth);
				BingoGrid.ColumnDefinitions.Add(col);
			}
			for (int y = 0; y < BingoTileDefs.Height; ++y)
			{
				RowDefinition row = new RowDefinition();
				gridRows.Add(row);
			}
			foreach (var row in gridRows)
			{
				row.Height = new GridLength(rowHeight);
				BingoGrid.RowDefinitions.Add(row);
			}

			var defs = BingoTileDefs.Tiles;

			if (FindResource("BingoTileTemplate") is ControlTemplate tileTemplate)
			{
				BingoTileDef def = null;
				UpgradeGridControls.Clear();
				for (int x = 0; x < BingoTileDefs.Width; ++x)
				{
					for (int y = 0; y < BingoTileDefs.Height; ++y)
					{
						BingoTileData tileData = new BingoTileData();
						if (defs.GetLength(0) > x && defs.GetLength(1) > y)
						{
							def = defs[x, y];
							tileData.TileDef = def;
						}
						Control gridTile = new Control
						{
							Template = tileTemplate
						};
						tileData.Height = rowHeight;
						tileData.Width = colWidth;
						tileData.X = x;
						tileData.Y = y;
						tileData.ImgVisible = IsBingoTileVisible(tileData);
						gridTile.DataContext = tileData;
						Grid.SetRow(gridTile, y);
						Grid.SetColumn(gridTile, x);
						BingoGrid.Children.Add(gridTile);
						if (tileData.TileDef != null && tileData.TileDef.Type == BingoTileType.Upgrade)
							UpgradeGridControls.Add(gridTile);
					}
				}
			}
			else
				ErrorTracker.CurrentError = "Could not find bingo tile template";
		}

		private Visibility IsBingoTileVisible(BingoTileData data)
		{
			if (data.TileDef == null)
				return Visibility.Hidden;

			try
			{
				switch (data.TileDef.Type)
				{
					case BingoTileType.Hp:
						if (PageModel.SaveInfo.HpUpgradesInv.Contains((int)SaveHelpers.StageToHpUpgrades(data.TileDef.Stage)))
							return Visibility.Visible;
						break;
					case BingoTileType.Mp:
						if (PageModel.SaveInfo.MpUpgradesInv.Contains((int)SaveHelpers.StageToMpUpgrades(data.TileDef.Stage)))
							return Visibility.Visible;
						break;
					case BingoTileType.Clock:
						if (PageModel.SaveInfo.ClockUpgradesInv.Contains((int)SaveHelpers.StageToClockUpgrades(data.TileDef.Stage)))
							return Visibility.Visible;
						break;
					case BingoTileType.Knife:
						if (PageModel.SaveInfo.KnifeUpgradesInv.Contains((int)SaveHelpers.StageToKnifeUpgrades(data.TileDef.Stage)))
							return Visibility.Visible;
						break;
					case BingoTileType.Trash:
						if (PageModel.SaveInfo.TrashCans.Contains(data.TileDef.Data))
							return Visibility.Visible;
						break;
					case BingoTileType.Statue:
						if (PageModel.SaveInfo.Statues.Contains(data.TileDef.Data))
							return Visibility.Visible;
						break;
					case BingoTileType.Skill:
						break;
					case BingoTileType.Key:
						if (PageModel.SaveInfo.Keys.Contains(data.TileDef.Data))
							return Visibility.Visible;
						break;
					case BingoTileType.Upgrade:
						if (PageModel.SaveInfo.Upgrades.Contains(data.TileDef.Data))
							return Visibility.Visible;
						break;
				}
			}
			catch (Exception ex)
			{
				ErrorTracker.CurrentError = ex.Message;
			}

			return Visibility.Hidden;
		}

		private void ErrorTrackerUpdateError(string error)
		{
			PageModel.Error = error;
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

		private void BingoGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (_saveEditor != null && _saveEditor.Save != null && sender is Grid gr && gr?.Children != null && gr.Children.Count > 0)
			{
				foreach (var child in gr.Children)
				{
					if (child is Control co && co.IsMouseOver && co.DataContext is BingoTileData td)
					{
						ProcessBingoTileFlip(td);
					}
				}
			}
		}

		private void ProcessBingoTileFlip(BingoTileData data)
		{
			//TODO: Mp and Hp need inventory definitions in save editor
			switch (data.TileDef.Type)
			{
				case BingoTileType.Hp:
					var hu = (int)SaveHelpers.StageToHpUpgrades(data.TileDef.Stage);
					if (PageModel.SaveInfo.HpUpgradesInv.Contains(hu))
					{
						data.ImgVisible = Visibility.Hidden;
						PageModel.SaveInfo.HpUpgradesInv.Remove(hu);
						PageModel.SaveInfo.HpUpgrades--;
					}
					else
					{
						data.ImgVisible = Visibility.Visible;
						PageModel.SaveInfo.HpUpgradesInv.Add(hu);
						PageModel.SaveInfo.HpUpgrades++;
					}
					break;
				case BingoTileType.Mp:
					var mu = (int)SaveHelpers.StageToMpUpgrades(data.TileDef.Stage);
					if (PageModel.SaveInfo.MpUpgradesInv.Contains(mu))
					{
						data.ImgVisible = Visibility.Hidden;
						PageModel.SaveInfo.MpUpgradesInv.Remove(mu);
						PageModel.SaveInfo.MpUpgrades--;
					}
					else
					{
						data.ImgVisible = Visibility.Visible;
						PageModel.SaveInfo.MpUpgradesInv.Add(mu);
						PageModel.SaveInfo.MpUpgrades++;
					}
					break;
				case BingoTileType.Clock:
					var cu = (int)SaveHelpers.StageToClockUpgrades(data.TileDef.Stage);
					if (PageModel.SaveInfo.ClockUpgradesInv.Contains(cu))
					{
						data.ImgVisible = Visibility.Hidden;
						PageModel.SaveInfo.ClockUpgradesInv.Remove(cu);
						PageModel.SaveInfo.ClockUpgrades--;
					}
					else
					{
						data.ImgVisible = Visibility.Visible;
						PageModel.SaveInfo.ClockUpgradesInv.Add(cu);
						PageModel.SaveInfo.ClockUpgrades++;
					}
					break;
				case BingoTileType.Knife:
					var ku = (int)SaveHelpers.StageToKnifeUpgrades(data.TileDef.Stage);
					if (PageModel.SaveInfo.KnifeUpgradesInv.Contains(ku))
					{
						data.ImgVisible = Visibility.Hidden;
						PageModel.SaveInfo.KnifeUpgradesInv.Remove(ku);
						PageModel.SaveInfo.KnifeUpgrades--;
					}
					else
					{
						data.ImgVisible = Visibility.Visible;
						PageModel.SaveInfo.KnifeUpgradesInv.Add(ku);
						PageModel.SaveInfo.KnifeUpgrades++;
					}
					break;
				case BingoTileType.Trash:
					var t = data.TileDef.Data;
					if (PageModel.SaveInfo.TrashCans.Contains(t))
					{
						data.ImgVisible = Visibility.Hidden;
						PageModel.SaveInfo.TrashCans.Remove(t);
					}
					else
					{
						data.ImgVisible = Visibility.Visible;
						PageModel.SaveInfo.TrashCans.Add(t);
					}
					break;
				case BingoTileType.Statue:
					var s = data.TileDef.Data;
					if (PageModel.SaveInfo.Statues.Contains(s))
					{
						data.ImgVisible = Visibility.Hidden;
						PageModel.SaveInfo.Statues.Remove(s);
					}
					else
					{
						data.ImgVisible = Visibility.Visible;
						PageModel.SaveInfo.Statues.Add(s);
					}
					break;
				case BingoTileType.Key:
					var k = data.TileDef.Data;
					if (PageModel.SaveInfo.Keys.Contains(k))
					{
						data.ImgVisible = Visibility.Hidden;
						PageModel.SaveInfo.Keys.Remove(k);
					}
					else
					{
						data.ImgVisible = Visibility.Visible;
						PageModel.SaveInfo.Keys.Add(k);
					}
					break;
				case BingoTileType.Upgrade:
					{
						HandleUpgradeToggle(data);
						break;
					}
			}
			Task.Run(() => SyncViewModelToSave(false));
		}

		private void HandleUpgradeToggle(BingoTileData data)
		{
			var u = data.TileDef.Data;
			switch (u)
			{
				case (int)Upgrades.Slide:
					if (PageModel.SaveInfo.Upgrades.Contains(u))
					{
						foreach (var c in UpgradeGridControls)
						{
							if (c.DataContext is BingoTileData td)
								td.ImgVisible = Visibility.Hidden;
						}
						PageModel.SaveInfo.Upgrades.Clear();
					}
					else
					{
						foreach (var c in UpgradeGridControls)
						{
							if (c.DataContext is BingoTileData td)
							{
								if (td.TileDef.Data == ((int)Upgrades.Slide))
									td.ImgVisible = Visibility.Visible;
								else
									td.ImgVisible = Visibility.Hidden;
							}
						}
						PageModel.SaveInfo.Upgrades.Clear();
						PageModel.SaveInfo.Upgrades.Add((int)Upgrades.Slide);
					}
					break;
				case (int)Upgrades.Double:
				case (int)Upgrades.Grip:
				case (int)Upgrades.Screw:
				case (int)Upgrades.Dash:
					if (PageModel.SaveInfo.Upgrades.Contains(u))
					{
						foreach (var c in UpgradeGridControls)
						{
							if (c.DataContext is BingoTileData td)
							{
								if (td.TileDef.Data >= u)
									td.ImgVisible = Visibility.Hidden;
							}
						}
						PageModel.SaveInfo.Upgrades.Clear();
						for (var i = 0; i < u; ++i)
						{
							PageModel.SaveInfo.Upgrades.Add(i);
						}
					}
					else
					{
						foreach (var c in UpgradeGridControls)
						{
							if (c.DataContext is BingoTileData td)
							{
								if (td.TileDef.Data <= u)
									td.ImgVisible = Visibility.Visible;
								else
									td.ImgVisible = Visibility.Hidden;
							}
						}
						PageModel.SaveInfo.Upgrades.Clear();
						for (var i = 0; i <= u; ++i)
						{
							PageModel.SaveInfo.Upgrades.Add(i);
						}
					}
					break;
			}
		}

		private void AutoSaveTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (PageModel != null && PageModel.SaveInfo.SaveLoaded && Config.Instance.ConfigObject.AutoSave)
				Task.Run(() => SyncViewModelToSave(true));
			_autoSaveTimer.Start();
		}

		private async Task SyncViewModelToSave(bool writeSave)
		{
			try
			{
				await PageModel.SaveInfo.SyncToSave(ref _saveEditor.Save);
				if (writeSave)
					await _saveEditor.WriteSave();
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => ErrorTracker.CurrentError = ex.Message, null);
			}
		}

		private void SaveButtonClicked(object sender, RoutedEventArgs e)
		{
			Task.Run(() => SyncViewModelToSave(true));
		}

		private void UnpackImagesClicked(object sender, RoutedEventArgs e)
		{
			try
			{
				var inFile = string.Empty;
				var outDir = string.Empty;
				OpenFileDialog inFileDiag = new OpenFileDialog
				{
					Title = "Select Luna Nights data.win file",
					DefaultExt = "win",
					Filter = "Data.win (*.win)|*.win",
					Multiselect = false
				};
				if (inFileDiag.ShowDialog() == true)
					inFile = inFileDiag.FileName;

				if (string.IsNullOrWhiteSpace(inFile))
					return;

				VistaFolderBrowserDialog outDirDiag = new VistaFolderBrowserDialog()
				{
					UseDescriptionForTitle = true,
					Description = "Select output folder for images"
				};
				if (outDirDiag.ShowDialog() == true)
					outDir = outDirDiag.SelectedPath;

				if (string.IsNullOrWhiteSpace(inFile) || string.IsNullOrWhiteSpace(outDir))
					return;

				Task.Run(() => _imagePacker.UnpackImagesFromData(inFile, outDir));
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => ErrorTracker.CurrentError = ex.Message, null);
			}
		}

		private void PackImagesClicked(object sender, RoutedEventArgs e)
		{
			try
			{
				string[] files = null;
				string outFile = string.Empty;
				OpenFileDialog inFileDiag = new OpenFileDialog
				{
					Title = "Select PNG images to pack",
					DefaultExt = "png",
					Filter = "PNG Image (*.png)|*.png",
					Multiselect = true
				};

				if (inFileDiag.ShowDialog() == true)
					files = inFileDiag.FileNames;

				if (files == null || files.Length == 0)
					return;

				OpenFileDialog outFileDiag = new OpenFileDialog
				{
					Title = "Select Luna Nights data.win file",
					DefaultExt = "win",
					Filter = "Data.win (*.win)|*.win",
					Multiselect = false
				};
				if (outFileDiag.ShowDialog() == true)
					outFile = outFileDiag.FileName;

				if (string.IsNullOrWhiteSpace(outFile) || files == null || files.Length == 0)
					return;

				Task.Run(() => _imagePacker.PackImagesIntoData(files, outFile));
			}
			catch(Exception ex)
			{
				_syncContext.Post((s) => ErrorTracker.CurrentError = ex.Message, null);
			}
		}
	}
}

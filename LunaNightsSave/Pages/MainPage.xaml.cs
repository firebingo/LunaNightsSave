using LunaNightsSave.Helpers;
using LunaNightsSave.ViewModels;
using NightsSaveReader;
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
		private readonly SynchronizationContext _syncContext;
		private string _loadPath = string.Empty;
		private string _save0Path = string.Empty;
		private readonly SolidColorBrush _save0Color = Brushes.Red;
		private string _save1Path = string.Empty;
		private readonly SolidColorBrush _save1Color = Brushes.Green;
		private string _save2Path = string.Empty;
		private readonly SolidColorBrush _save2Color = Brushes.Blue;

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
					sav.Add(new SaveSelectionModel(0, $"Save 0 - Time: {_saveEditor.Save.SaveDate.ToString(SaveEditor.DateFormat)} - Stage: {((int)SaveHelpers.StageToStageDisplay(_saveEditor.Save.Stage)).ToString()}", _save0Color));
				}
				else
					sav.Add(new SaveSelectionModel(0, $"Save 0 - New Game", _save0Color));

				if (File.Exists(_save1Path))
				{
					await _saveEditor.LoadSave(_save1Path, false);
					sav.Add(new SaveSelectionModel(1, $"Save 1 - Time: {_saveEditor.Save.SaveDate.ToString(SaveEditor.DateFormat)} - Stage: {((int)SaveHelpers.StageToStageDisplay(_saveEditor.Save.Stage)).ToString()}", _save1Color));
				}
				else
					sav.Add(new SaveSelectionModel(1, $"Save 1 - New Game", _save1Color));

				if (File.Exists(_save2Path))
				{
					await _saveEditor.LoadSave(_save2Path, false);
					sav.Add(new SaveSelectionModel(2, $"Save 2 - Time: {_saveEditor.Save.SaveDate.ToString(SaveEditor.DateFormat)} - Stage: {((int)SaveHelpers.StageToStageDisplay(_saveEditor.Save.Stage)).ToString()}", _save2Color));
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

			GridHelper.CalculateGridSizes(BingoGrid, SaveEditor.BingoWidth, SaveEditor.BingoHeight);
			BingoGrid.HorizontalAlignment = HorizontalAlignment.Center;
			BingoGrid.VerticalAlignment = VerticalAlignment.Center;
			double colWidth = BingoGrid.Width / SaveEditor.BingoWidth;
			double rowHeight = BingoGrid.Height / SaveEditor.BingoHeight;

			for (int x = 0; x < SaveEditor.BingoWidth; ++x)
			{
				ColumnDefinition column = new ColumnDefinition();
				gridColumns.Add(column);
			}
			foreach (var col in gridColumns)
			{
				col.Width = new GridLength(colWidth);
				BingoGrid.ColumnDefinitions.Add(col);
			}
			for (int y = 0; y < SaveEditor.BingoHeight; ++y)
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
				for (int x = 0; x < SaveEditor.BingoWidth; ++x)
				{
					for (int y = 0; y < SaveEditor.BingoHeight; ++y)
					{
						
						BingoTileData tileData = new BingoTileData();
						if (defs.Rank > x && defs.GetLength(x) > y)
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
				}
			}
			catch(Exception ex)
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
			switch(data.TileDef.Type)
			{
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
			}
			Task.Run(SyncViewModelToSave);
		}

		private async Task SyncViewModelToSave()
		{
			await PageModel.SaveInfo.SyncToSave(ref _saveEditor.Save);
			if (PageModel.AutoSave)
				await _saveEditor.WriteSave();
		}
	}
}

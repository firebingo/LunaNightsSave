using LunaNightsSave.Function;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LunaNightsSave.ViewModels
{
	public class SaveSelectionModel
	{
		public int Index { get; set; }
		public string Value { get; set; }
		public SolidColorBrush Color { get; set; }

		public SaveSelectionModel(int i, string v, SolidColorBrush c)
		{
			Index = i;
			Value = v;
			Color = c;
		}
	}

	public class MainPageModel : INotifyPropertyChanged
	{
		private readonly SynchronizationContext _syncContext;
		public SaveInfoViewModel SaveInfo { get; set; }

		private ObservableCollection<SaveSelectionModel> _saves;
		public ObservableCollection<SaveSelectionModel> Saves
		{
			get => _saves;
			set
			{
				_saves = value;
				NotifyPropertyChanged("Saves");
			}
		}

		private string _error;
		public string Error
		{
			get => _error;
			set
			{
				_error = value;
				NotifyPropertyChanged("Error");
			}
		}

		private bool _autoSave = false;
		public bool AutoSave
		{
			get => _autoSave;
			set
			{
				_autoSave = value;
				Config.Instance.ConfigObject.AutoSave = _autoSave;
				NotifyPropertyChanged("AutoSave");
			}
		}

		public MainPageModel(SynchronizationContext syncContext)
		{
			_syncContext = syncContext;
			Saves = new ObservableCollection<SaveSelectionModel>();
			Error = string.Empty;
			AutoSave = Config.Instance.ConfigObject.AutoSave;
			SaveInfo = new SaveInfoViewModel(syncContext);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}

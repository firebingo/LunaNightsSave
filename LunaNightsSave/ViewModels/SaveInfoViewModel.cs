using LunaNightsSave.Helpers;
using NightsSaveReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LunaNightsSave.ViewModels
{
	public class SaveInfoViewModel : INotifyPropertyChanged
	{
		private readonly SynchronizationContext _syncContext;

		public bool HasSlide { get; set; }
		public bool HasDouble { get; set; }
		public bool HasGrip { get; set; }
		public bool HasScrew { get; set; }
		public bool HasRedKey { get; set; }
		public bool HasYellowKey { get; set; }
		public bool HasGreenKey { get; set; }
		public bool HasBlueKey { get; set; }
		public bool HasPurpleKey { get; set; }
		public int Exp { get; set; }
		public int Gold { get; set; }
		public int Amethyst { get; set; }
		public int Turquoise { get; set; }
		public int Topaz { get; set; }
		public int Ruby { get; set; }
		public int Sapphire { get; set; }
		public int Emerald { get; set; }
		public int Diamond { get; set; }
		public int Stage { get; set; }
		public int PosX { get; set; }
		public int PosY { get; set; }
		public int HpUpgrades { get; set; }
		public int MpUpgrades { get; set; }
		public int KnifeUpgrades { get; set; }
		public int ClockUpgrades { get; set; }
		public ObservableCollection<int> KnifeUpgradesInv { get; set; }
		public ObservableCollection<int> ClockUpgradesInv { get; set; }
		public DateTime SaveDate { get; set; }
		public ObservableCollection<int> TrashCans { get; set; }
		public int KnifeUpgradesBought { get; set; }
		public int ClockUpgradesBought { get; set; }

		private readonly object _lockObject = new object();

		public SaveInfoViewModel(SynchronizationContext syncContext)
		{
			_syncContext = syncContext;
			KnifeUpgradesInv = new ObservableCollection<int>();
			ClockUpgradesInv = new ObservableCollection<int>();
			TrashCans = new ObservableCollection<int>();
		}

		public Task SyncFromSave(NightsSaveReader.LunaNightsSave save)
		{
			try
			{
				lock (_lockObject)
				{
					HasSlide = save.HasSlide;
					HasDouble = save.HasDouble;
					HasGrip = save.HasGrip;
					HasScrew = save.HasScrew;

					HasRedKey = save.HasRedKey;
					HasYellowKey = save.HasYellowKey;
					HasGreenKey = save.HasGreenKey;
					HasBlueKey = save.HasBlueKey;
					HasPurpleKey = save.HasPurpleKey;

					Exp = save.Exp;
					Gold = save.Gold;

					Amethyst = save.Amethyst;
					Turquoise = save.Turquoise;
					Topaz = save.Topaz;
					Ruby = save.Ruby;
					Sapphire = save.Sapphire;
					Emerald = save.Emerald;
					Diamond = save.Diamond;

					Stage = (int)save.Stage;
					PosX = save.PosX;
					PosY = save.PosY;

					HpUpgrades = save.HpUpgrades;
					MpUpgrades = save.MpUpgrades;

					KnifeUpgrades = save.KnifeUpgrades;
					ClockUpgrades = save.ClockUpgrades;

					KnifeUpgradesBought = save.KnifeUpgradesBought;
					ClockUpgradesBought = save.ClockUpgradesBought;

					KnifeUpgradesInv.Clear();
					foreach (var k in save.KnifeUpgradesInv)
					{
						KnifeUpgradesInv.Add((int)k);
					}

					ClockUpgradesInv.Clear();
					foreach (var c in save.ClockUpgradesInv)
					{
						ClockUpgradesInv.Add((int)c);
					}

					TrashCans.Clear();
					foreach (var t in save.TrashCans)
					{
						TrashCans.Add((int)t);
					}

					SaveDate = save.SaveDate;
				}
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => { ErrorTracker.CurrentError = ex.Message; }, null);
			}

			_syncContext.Post((s) => { NotifyPropertyChanged("*"); }, null);

			return Task.CompletedTask;
		}

		public Task SyncToSave(ref NightsSaveReader.LunaNightsSave save)
		{
			try
			{
				lock (_lockObject)
				{
					save.HasSlide = HasSlide;
					save.HasDouble = HasDouble;
					save.HasGrip = HasGrip;
					save.HasScrew = HasScrew;

					save.HasRedKey = HasRedKey;
					save.HasYellowKey = HasYellowKey;
					save.HasGreenKey = HasGreenKey;
					save.HasBlueKey = HasBlueKey;
					save.HasPurpleKey = HasPurpleKey;

					save.Exp = Exp;
					save.Gold = Gold;

					save.Amethyst = Amethyst;
					save.Turquoise = Turquoise;
					save.Topaz = Topaz;
					save.Ruby = Ruby;
					save.Sapphire = Sapphire;
					save.Emerald = Emerald;
					save.Diamond = Diamond;

					if (Enum.IsDefined(typeof(Stage), Stage))
						save.Stage = (Stage)Stage;
					save.PosX = PosX;
					save.PosY = PosY;

					save.HpUpgrades = HpUpgrades;
					save.MpUpgrades = MpUpgrades;

					save.KnifeUpgrades = KnifeUpgrades;
					save.ClockUpgrades = ClockUpgrades;

					save.KnifeUpgradesBought = KnifeUpgradesBought;
					save.ClockUpgradesBought = ClockUpgradesBought;

					save.KnifeUpgradesInv.Clear();
					foreach (var k in KnifeUpgradesInv)
					{
						if (Enum.IsDefined(typeof(KnifeUpgrades), k))
							save.KnifeUpgradesInv.Add((KnifeUpgrades)k);
					}

					save.ClockUpgradesInv.Clear();
					foreach (var c in ClockUpgradesInv)
					{
						if (Enum.IsDefined(typeof(ClockUpgrades), c))
							save.ClockUpgradesInv.Add((ClockUpgrades)c);
					}

					save.TrashCans.Clear();
					foreach (var t in TrashCans)
					{
						if (Enum.IsDefined(typeof(TrashCans), t))
							save.TrashCans.Add((TrashCans)t);
					}

					save.SaveDate = SaveDate;
				}
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => { ErrorTracker.CurrentError = ex.Message; }, null);
			}

			return Task.CompletedTask;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}

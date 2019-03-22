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

		private bool _saveLoaded;
		public bool SaveLoaded
		{
			get => _saveLoaded;
			set
			{
				_saveLoaded = value;
				NotifyPropertyChanged("SaveLoaded");
			}
		}

		public bool HasSlide { get; set; }
		public bool HasDouble { get; set; }
		public bool HasGrip { get; set; }
		public bool HasScrew { get; set; }
		public bool HasRedKey { get; set; }
		public bool HasYellowKey { get; set; }
		public bool HasGreenKey { get; set; }
		public bool HasBlueKey { get; set; }
		public bool HasPurpleKey { get; set; }
		private double _exp;
		public double Exp
		{
			get => _exp;
			set
			{
				_exp = value;
				NotifyPropertyChanged("Exp");
			}
		}
		private int _gold;
		public int Gold
		{
			get => _gold;
			set
			{
				_gold = value;
				NotifyPropertyChanged("Gold");
			}
		}

		private int _amethyst;
		public int Amethyst
		{
			get => _amethyst;
			set
			{
				_amethyst = value;
				NotifyPropertyChanged("Amethyst");
				NotifyPropertyChanged("AmethystMod");
				NotifyPropertyChanged("AmethystModString");
			}
		}
		public double AmethystMod
		{
			get
			{
				return 0.000075 * Amethyst;
			}
		}
		public string AmethystModString
		{
			get
			{
				var mod = AmethystMod;
				return string.Format("{0:+0.####;-0.####;+0}", mod);
			}
		}

		private int _turquoise;
		public int Turquoise
		{
			get => _turquoise;
			set
			{
				_turquoise = value;
				NotifyPropertyChanged("Turquoise");
			}
		}
		private int _topaz;
		public int Topaz
		{
			get => _topaz;
			set
			{
				_topaz = value;
				NotifyPropertyChanged("Topaz");
			}
		}
		private int _ruby;
		public int Ruby
		{
			get => _ruby;
			set
			{
				_ruby = value;
				NotifyPropertyChanged("Ruby");
			}
		}
		private int _sapphire;
		public int Sapphire
		{
			get => _sapphire;
			set
			{
				_sapphire = value;
				NotifyPropertyChanged("Sapphire");
			}
		}
		private int _emerald;
		public int Emerald
		{
			get => _emerald;
			set
			{
				_emerald = value;
				NotifyPropertyChanged("Emerald");
			}
		}
		private int _diamond;
		public int Diamond
		{
			get => _diamond;
			set
			{
				_diamond = value;
				NotifyPropertyChanged("Diamond");
			}
		}

		private int _stage;
		public int Stage
		{
			get => _stage;
			set
			{
				_stage = value;
				NotifyPropertyChanged("Stage");
			}
		}
		private int _posx;
		public int PosX
		{
			get => _posx;
			set
			{
				_posx = value;
				NotifyPropertyChanged("PosX");
			}
		}
		private int _posy;
		public int PosY
		{
			get => _posy;
			set
			{
				_posy = value;
				NotifyPropertyChanged("PosY");
			}
		}

		private int _hpUpgrades;
		public int HpUpgrades
		{
			get => _hpUpgrades;
			set
			{
				_hpUpgrades = value;
				NotifyPropertyChanged("HpUpgrades");
				NotifyPropertyChanged("MaxHp");
			}
		}
		public int MaxHp
		{
			get => 100 + (HpUpgrades * 10);
		}
		private int _mpUpgrades;
		public int MpUpgrades
		{
			get => _mpUpgrades;
			set
			{
				_mpUpgrades = value;
				NotifyPropertyChanged("MpUpgrades");
				NotifyPropertyChanged("MaxMp");
			}
		}
		public int MaxMp
		{
			get => 100 + (MpUpgrades * 10);
		}

		private int _knifeUpgrades;
		public int KnifeUpgrades
		{
			get => _knifeUpgrades;
			set
			{
				_knifeUpgrades = value;
				NotifyPropertyChanged("KnifeUpgrades");
				NotifyPropertyChanged("MaxKnives");
			}
		}
		public int MaxKnives
		{
			get => 18 + (KnifeUpgrades * 3);
		}

		private int _clockUpgrades;
		public int ClockUpgrades
		{
			get => _clockUpgrades;
			set
			{
				_clockUpgrades = value;
				NotifyPropertyChanged("ClockUpgrades");
				NotifyPropertyChanged("MaxClock");
			}
		}
		public int MaxClock
		{
			get => 85 + (ClockUpgrades * 15);
		}

		private int _knifeUpgradesBought;
		public int KnifeUpgradesBought
		{
			get => _knifeUpgradesBought;
			set
			{
				_knifeUpgradesBought = value;
				NotifyPropertyChanged("KnifeUpgradesBought");
			}
		}
		private int _clockUpgradesBought;
		public int ClockUpgradesBought
		{
			get => _clockUpgradesBought;
			set
			{
				_clockUpgradesBought = value;
				NotifyPropertyChanged("ClockUpgradesBought");
			}
		}

		public ObservableCollection<int> KnifeUpgradesInv { get; set; }
		public ObservableCollection<int> ClockUpgradesInv { get; set; }
		public DateTime SaveDate { get; set; }
		public ObservableCollection<int> TrashCans { get; set; }

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
			_saveLoaded = false;
			_syncContext.Post((s) => { NotifyPropertyChanged("SaveLoaded"); }, null);

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

					_amethyst = save.Amethyst;
					_turquoise = save.Turquoise;
					_topaz = save.Topaz;
					_ruby = save.Ruby;
					_sapphire = save.Sapphire;
					_emerald = save.Emerald;
					_diamond = save.Diamond;

					_stage = (int)save.Stage;
					_posx = save.PosX;
					_posy = save.PosY;

					_hpUpgrades = save.HpUpgrades;
					_mpUpgrades = save.MpUpgrades;

					_knifeUpgrades = save.KnifeUpgrades;
					_clockUpgrades = save.ClockUpgrades;

					_knifeUpgradesBought = save.KnifeUpgradesBought;
					_clockUpgradesBought = save.ClockUpgradesBought;

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

					_saveLoaded = true;
				}
			}
			catch (Exception ex)
			{
				_syncContext.Post((s) => { ErrorTracker.CurrentError = ex.Message; }, null);
			}

			_syncContext.Post((s) => { NotifyPropertyChanged(string.Empty); }, null);

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

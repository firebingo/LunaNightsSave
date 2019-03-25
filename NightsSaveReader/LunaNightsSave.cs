using System;
using System.Collections.Generic;

namespace NightsSaveReader
{
	public enum UpgradeLevel
	{
		None,
		Slide,
		Double,
		Grip,
		Screw
	}

	public enum Stage
	{
		NewGame = 0,
		Opening = 1,
		Title = 2,
		SaveSelect = 3,
		BlackScreen = 4,
		Stage1 = 5,
		Stage2 = 6,
		Stage3 = 7,
		Stage4 = 8,
		Stage5 = 9,
		StageExtra = 10,
		Credits = 11
	}

	public enum StageDisplay
	{
		NewGame = -4,
		Opening = -3,
		Title = -2,
		SaveSelect = -1,
		BlackScreen = 0,
		Stage1 = 1,
		Stage2 = 2,
		Stage3 = 3,
		Stage4 = 4,
		Stage5 = 5,
		StageExtra = 6,
		Credits = 7
	}

	//TODO: Get the actual numbers for these
	public enum TrashCans
	{
		S1_1 = 100221,
		S1_2 = 100220,
		S1_3 = 100219,
		S2_1 = 100273,
		S2_2 = 100272,
		S2_3 = 100275,
		S3_1 = 100468,
		S3_2 = 100594,
		S3_3 = 100472,
		S4_1 = 100619,
		S4_2 = 100782,
		S4_3 = 100684,
		S5_1 = 100818,
		S5_2 = 100816,
		S5_3 = 100814
	}

	public enum HpUpgrades
	{
		S1 = 1,
		S2 = 2,
		S3 = 3,
		S4 = 4,
		S5 = 5
	}

	public enum MpUpgrades
	{
		S1 = 1,
		S2 = 2,
		S3 = 3,
		S4 = 4,
		S5 = 5
	}

	public enum KnifeUpgrades
	{
		S1 = 1,
		S2 = 2,
		S3 = 3,
		S4 = 4,
		S5 = 5
	}

	public enum ClockUpgrades
	{
		S1 = 1,
		S2 = 2,
		S3 = 3,
		S4 = 4,
		S5 = 5
	}

	public enum Statues
	{
		Amethyst = 0,
		Turquoise = 1,
		Topaz = 2,
		Ruby = 3,
		Sapphire = 4,
		Emerald = 5,
		Diamond = 6
	}

	public enum Keys
	{
		Red = 0,
		Yellow = 1,
		Green = 2,
		Blue = 3,
		Purple = 4
	}

	public enum Upgrades
	{
		Slide = 0,
		Double = 1,
		Grip = 2,
		Screw = 3
	}

	public class LunaNightsSave
	{
		/// <summary>
		/// What upgrades you have
		/// These have to be sequential due to the way the game checks them on load
		/// Setting this does not add them to the bingo card or remove them from the map
		/// see Upgrades
		/// </summary>
		public UpgradeLevel UpgradeLevel = UpgradeLevel.None;
		/// <summary>
		/// What upgrades you have picked up.
		/// Fills bingo card and removes from map but does not provide upgrade.
		/// </summary>
		public List<Upgrades> Upgrades = new List<Upgrades>();
		public double Exp = 0;
		public int Gold = 0;
		public int Amethyst = 0;
		public int Turquoise = 0;
		public int Topaz = 0;
		public int Ruby = 0;
		public int Sapphire = 0;
		public int Emerald = 0;
		public int Diamond = 0;
		/// <summary>
		/// The stage you are on
		/// If you want to start from the start of the game use Stage.Opening
		/// </summary>
		public Stage Stage = Stage.Opening;
		/// <summary>
		/// Your X position in the current stage
		/// 0 is left
		/// </summary>
		public int PosX = 0;
		/// <summary>
		/// Your Y position in the current stage
		/// 1 is top
		/// </summary>
		public int PosY = 0;
		/// <summary>
		/// Count of Hp Upgrades
		/// Each gives 10 Hp, starting from 0 = 100
		/// </summary>
		public int HpUpgrades = 0;
		/// <summary>
		/// Count of Mp Upgrades
		/// Each gives 10 Mp, starting from 0 = 100
		/// </summary>
		public int MpUpgrades = 0;
		/// <summary>
		/// Count of time stop knife upgrades
		/// Each gives 3, starting from 0 = 18
		/// </summary>
		public int KnifeUpgrades = 0;
		/// <summary>
		/// Count of time stop time upgrades
		/// Each gives 15, starting from 0 = 85
		/// </summary>
		public int ClockUpgrades = 0;
		/// <summary>
		/// What hp upgrades you have picked up.
		/// Fills bingo card and removes from map but does not provide upgrade.
		/// </summary>
		public List<HpUpgrades> HpUpgradesInv = new List<HpUpgrades>();
		/// <summary>
		/// What mp upgrades you have picked up.
		/// Fills bingo card and removes from map but does not provide upgrade.
		/// </summary>
		public List<MpUpgrades> MpUpgradesInv = new List<MpUpgrades>();
		/// <summary>
		/// What knife upgrades you have picked up.
		/// Fills bingo card and removes from map but does not provide upgrade.
		/// </summary>
		public List<KnifeUpgrades> KnifeUpgradesInv = new List<KnifeUpgrades>();
		/// <summary>
		/// What clock upgrades you have picked up.
		/// Fills bingo card and removes from map but does not provide upgrade.
		/// </summary>
		public List<ClockUpgrades> ClockUpgradesInv = new List<ClockUpgrades>();
		/// <summary>
		/// What gem statues you have broken.
		/// </summary>
		public List<Statues> Statues = new List<Statues>();
		/// <summary>
		/// What keys you have picked up.
		/// Fills bingo card and provides the effect of the key.
		/// </summary>
		public List<Keys> Keys = new List<Keys>();
		/// <summary>
		/// The date the save was last saved
		/// Game uses local time for this
		/// </summary>
		public DateTime SaveDate = DateTime.Now;
		/// <summary>
		/// A list of trashcans that have been dunked
		/// </summary>
		public List<TrashCans> TrashCans = new List<TrashCans>();
		/// <summary>
		/// Count of knife upgrades bought.
		/// Does not give upgrade. Just used for out of stock count at shop.
		/// </summary>
		public int KnifeUpgradesBought = 0;
		/// <summary>
		/// Count of clock upgrades bought.
		/// Does not give upgrade. Just used for out of stock count at shop.
		/// </summary>
		public int ClockUpgradesBought = 0;
	}
}

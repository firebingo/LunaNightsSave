using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NightsSaveReader.SaveLines;

namespace NightsSaveReader
{
	public class SaveEditor
	{
		public LunaNightsSave Save;
		private string _currentPath;

		/// <summary>
		/// Loads a save from a file
		/// </summary>
		/// <param name="path">The path to the save file</param>
		/// <param name="createIfNotExist">If true will create a new save with the give path/name if it does not already exist</param>
		/// <returns></returns>
		public async Task LoadSave(string path, bool createIfNotExist)
		{
			Save = new LunaNightsSave();
			_currentPath = path;

			if (!File.Exists(_currentPath))
			{
				if (createIfNotExist)
					await CreateSave(_currentPath);
				else
					throw new IOException($"File at \"{_currentPath}\" does not exist");
			}

			var lines = await LoadLines(_currentPath);

			if (lines.Count != LINE_COUNT)
				throw new Exception("Save file may be corrupted");

			//Inventory Hp upgrades
			if (lines[HP_UPGRADE_S1] == HP_UPGRADE_S1.ToString())
				Save.HpUpgradesInv.Add(HpUpgrades.S1);
			if (lines[HP_UPGRADE_S2] == HP_UPGRADE_S2.ToString())
				Save.HpUpgradesInv.Add(HpUpgrades.S2);
			if (lines[HP_UPGRADE_S3] == HP_UPGRADE_S3.ToString())
				Save.HpUpgradesInv.Add(HpUpgrades.S3);
			if (lines[HP_UPGRADE_S4] == HP_UPGRADE_S4.ToString())
				Save.HpUpgradesInv.Add(HpUpgrades.S4);
			if (lines[HP_UPGRADE_S5] == HP_UPGRADE_S5.ToString())
				Save.HpUpgradesInv.Add(HpUpgrades.S5);

			//Inventory Mp upgrades
			if (lines[MP_UPGRADE_S1] == MP_UPGRADE_S1.ToString())
				Save.MpUpgradesInv.Add(MpUpgrades.S1);
			if (lines[MP_UPGRADE_S2] == MP_UPGRADE_S2.ToString())
				Save.MpUpgradesInv.Add(MpUpgrades.S2);
			if (lines[MP_UPGRADE_S3] == MP_UPGRADE_S3.ToString())
				Save.MpUpgradesInv.Add(MpUpgrades.S3);
			if (lines[MP_UPGRADE_S4] == MP_UPGRADE_S4.ToString())
				Save.MpUpgradesInv.Add(MpUpgrades.S4);
			if (lines[MP_UPGRADE_S5] == MP_UPGRADE_S5.ToString())
				Save.MpUpgradesInv.Add(MpUpgrades.S5);

			//Inventory Knife upgrades
			if (lines[KNIFE_UPGRADE_S1] == KNIFE_UPGRADE_S1.ToString())
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S1);
			if (lines[KNIFE_UPGRADE_S2] == KNIFE_UPGRADE_S2.ToString())
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S2);
			if (lines[KNIFE_UPGRADE_S3] == KNIFE_UPGRADE_S3.ToString())
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S3);
			if (lines[KNIFE_UPGRADE_S4] == KNIFE_UPGRADE_S4.ToString())
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S4);
			if (lines[KNIFE_UPGRADE_S5] == KNIFE_UPGRADE_S5.ToString())
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S5);

			//Inventory clock upgrades
			if (lines[CLOCK_UPGRADE_S1] == CLOCK_UPGRADE_S1.ToString())
				Save.ClockUpgradesInv.Add(ClockUpgrades.S1);
			if (lines[CLOCK_UPGRADE_S2] == CLOCK_UPGRADE_S2.ToString())
				Save.ClockUpgradesInv.Add(ClockUpgrades.S2);
			if (lines[CLOCK_UPGRADE_S3] == CLOCK_UPGRADE_S3.ToString())
				Save.ClockUpgradesInv.Add(ClockUpgrades.S3);
			if (lines[CLOCK_UPGRADE_S4] == CLOCK_UPGRADE_S4.ToString())
				Save.ClockUpgradesInv.Add(ClockUpgrades.S4);
			if (lines[CLOCK_UPGRADE_S5] == CLOCK_UPGRADE_S5.ToString())
				Save.ClockUpgradesInv.Add(ClockUpgrades.S5);

			//Inventory Upgrades
			if (lines[UPGRADES_SLIDE] == UPGRADES_SLIDE.ToString())
				Save.Upgrades.Add(Upgrades.Slide);
			if (lines[UPGRADES_DOUBLE] == UPGRADES_DOUBLE.ToString())
				Save.Upgrades.Add(Upgrades.Double);
			if (lines[UPGRADES_GRIP] == UPGRADES_GRIP.ToString())
				Save.Upgrades.Add(Upgrades.Grip);
			if (lines[UPGRADES_SCREW] == UPGRADES_SCREW.ToString())
				Save.Upgrades.Add(Upgrades.Screw);

			//Keys
			if (lines[KEY_RED] == KEY_RED.ToString())
				Save.Keys.Add(Keys.Red);
			if (lines[KEY_YELLOW] == KEY_YELLOW.ToString())
				Save.Keys.Add(Keys.Yellow);
			if (lines[KEY_GREEN] == KEY_GREEN.ToString())
				Save.Keys.Add(Keys.Green);
			if (lines[KEY_BLUE] == KEY_BLUE.ToString())
				Save.Keys.Add(Keys.Blue);
			if (lines[KEY_PURPLE] == KEY_PURPLE.ToString())
				Save.Keys.Add(Keys.Purple);

			//Position in stage
			int.TryParse(lines[POS_X], out Save.PosX);
			int.TryParse(lines[POS_Y], out Save.PosY);
			Enum.TryParse(lines[STAGE], out Save.Stage);

			//hp and mp upgrades
			int.TryParse(lines[HP_UPGRADES], out Save.HpUpgrades);
			int.TryParse(lines[MP_UPGRADES], out Save.MpUpgrades);

			//xp
			double.TryParse(lines[EXP], out Save.Exp);

			//File save date
			Save.SaveDate = DateTime.ParseExact(lines[SAVE_DATE], DATE_FORMAT, CultureInfo.CurrentCulture);

			//Count of upgrades
			int.TryParse(lines[KNIFE_UPGRADES], out Save.KnifeUpgrades);
			int.TryParse(lines[CLOCK_UPGRADES], out Save.ClockUpgrades);

			//Gems
			int.TryParse(lines[AMETHYST], out Save.Amethyst);
			int.TryParse(lines[TURQUOISE], out Save.Turquoise);
			int.TryParse(lines[TOPAZ], out Save.Topaz);
			int.TryParse(lines[RUBY], out Save.Ruby);
			int.TryParse(lines[SAPPHIRE], out Save.Sapphire);
			int.TryParse(lines[EMERALD], out Save.Emerald);
			int.TryParse(lines[DIAMOND], out Save.Diamond);

			//Upgrade level
			Enum.TryParse(lines[UPGRADE_LEVEL], out Save.UpgradeLevel);

			//Gold
			int.TryParse(lines[GOLD], out Save.Gold);

			//TrashCans
			//TODO: Do this when I figure out all the ids
			var trashLines = lines[TRASH_CANS].Split(',');
			foreach (var l in trashLines)
			{
				if (Enum.TryParse(l, out TrashCans can))
					Save.TrashCans.Add(can);
			}

			//Bought Upgrades
			int.TryParse(lines[CLOCK_UPGRADES_BOUGHT], out Save.ClockUpgradesBought);
			int.TryParse(lines[KNIFE_UPGRADES_BOUGHT], out Save.KnifeUpgradesBought);

			//Gem Statues
			if (lines[STATUE_AMETHYST] == STATUE_AMETHYST.ToString())
				Save.Statues.Add(Statues.Amethyst);
			if (lines[STATUE_TURQUOISE] == STATUE_TURQUOISE.ToString())
				Save.Statues.Add(Statues.Turquoise);
			if (lines[STATUE_TOPAZ] == STATUE_TOPAZ.ToString())
				Save.Statues.Add(Statues.Topaz);
			if (lines[STATUE_RUBY] == STATUE_RUBY.ToString())
				Save.Statues.Add(Statues.Ruby);
			if (lines[STATUE_SAPPHIRE] == STATUE_SAPPHIRE.ToString())
				Save.Statues.Add(Statues.Sapphire);
			if (lines[STATUE_EMERALD] == STATUE_EMERALD.ToString())
				Save.Statues.Add(Statues.Emerald);
			if (lines[STATUE_DIAMOND] == STATUE_DIAMOND.ToString())
				Save.Statues.Add(Statues.Diamond);
		}

		/// <summary>
		/// Writes the save out to the file
		/// </summary>
		/// <param name="useNowDate">If true saves the file's date as the current system time and not the time in the save</param>
		/// <returns></returns>
		public async Task WriteSave(bool useNowDate = false)
		{
			var lines = await LoadLines(_currentPath);

			//Inventory Hp upgrades
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S1))
				lines[HP_UPGRADE_S1] = HP_UPGRADE_S1.ToString();
			else
				lines[HP_UPGRADE_S1] = ZERO.ToString();
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S2))
				lines[HP_UPGRADE_S2] = HP_UPGRADE_S2.ToString();
			else
				lines[HP_UPGRADE_S2] = ZERO.ToString();
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S3))
				lines[HP_UPGRADE_S3] = HP_UPGRADE_S3.ToString();
			else
				lines[HP_UPGRADE_S3] = ZERO.ToString();
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S4))
				lines[HP_UPGRADE_S4] = HP_UPGRADE_S4.ToString();
			else
				lines[HP_UPGRADE_S4] = ZERO.ToString();
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S5))
				lines[HP_UPGRADE_S5] = HP_UPGRADE_S5.ToString();
			else
				lines[HP_UPGRADE_S5] = ZERO.ToString();

			//Inventory Mp upgrades
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S1))
				lines[MP_UPGRADE_S1] = MP_UPGRADE_S1.ToString();
			else
				lines[MP_UPGRADE_S1] = ZERO.ToString();
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S2))
				lines[MP_UPGRADE_S2] = MP_UPGRADE_S2.ToString();
			else
				lines[MP_UPGRADE_S2] = ZERO.ToString();
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S3))
				lines[MP_UPGRADE_S3] = MP_UPGRADE_S3.ToString();
			else
				lines[MP_UPGRADE_S3] = ZERO.ToString();
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S4))
				lines[MP_UPGRADE_S4] = MP_UPGRADE_S4.ToString();
			else
				lines[MP_UPGRADE_S4] = ZERO.ToString();
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S5))
				lines[MP_UPGRADE_S5] = MP_UPGRADE_S5.ToString();
			else
				lines[MP_UPGRADE_S5] = ZERO.ToString();

			//Inventory knife upgrades
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S1))
				lines[KNIFE_UPGRADE_S1] = KNIFE_UPGRADE_S1.ToString();
			else
				lines[KNIFE_UPGRADE_S1] = ZERO.ToString();
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S2))
				lines[KNIFE_UPGRADE_S2] = KNIFE_UPGRADE_S2.ToString();
			else
				lines[KNIFE_UPGRADE_S2] = ZERO.ToString();
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S3))
				lines[KNIFE_UPGRADE_S3] = KNIFE_UPGRADE_S3.ToString();
			else
				lines[KNIFE_UPGRADE_S3] = ZERO.ToString();
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S4))
				lines[KNIFE_UPGRADE_S4] = KNIFE_UPGRADE_S4.ToString();
			else
				lines[KNIFE_UPGRADE_S4] = ZERO.ToString();
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S5))
				lines[KNIFE_UPGRADE_S5] = KNIFE_UPGRADE_S5.ToString();
			else
				lines[KNIFE_UPGRADE_S5] = ZERO.ToString();

			//Inventory clock upgrades
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S1))
				lines[CLOCK_UPGRADE_S1] = CLOCK_UPGRADE_S1.ToString();
			else
				lines[CLOCK_UPGRADE_S1] = ZERO.ToString();
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S2))
				lines[CLOCK_UPGRADE_S2] = CLOCK_UPGRADE_S2.ToString();
			else
				lines[CLOCK_UPGRADE_S2] = ZERO.ToString();
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S3))
				lines[CLOCK_UPGRADE_S3] = CLOCK_UPGRADE_S3.ToString();
			else
				lines[CLOCK_UPGRADE_S3] = ZERO.ToString();
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S4))
				lines[CLOCK_UPGRADE_S4] = CLOCK_UPGRADE_S4.ToString();
			else
				lines[CLOCK_UPGRADE_S4] = ZERO.ToString();
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S5))
				lines[CLOCK_UPGRADE_S5] = CLOCK_UPGRADE_S5.ToString();
			else
				lines[CLOCK_UPGRADE_S5] = ZERO.ToString();

			//Inventory Upgrades
			if (Save.Upgrades.Contains(Upgrades.Slide))
				lines[UPGRADES_SLIDE] = UPGRADES_SLIDE.ToString();
			else
				lines[UPGRADES_SLIDE] = ZERO.ToString();
			if (Save.Upgrades.Contains(Upgrades.Double))
				lines[UPGRADES_DOUBLE] = UPGRADES_DOUBLE.ToString();
			else
				lines[UPGRADES_DOUBLE] = ZERO.ToString();
			if (Save.Upgrades.Contains(Upgrades.Grip))
				lines[UPGRADES_GRIP] = UPGRADES_GRIP.ToString();
			else
				lines[UPGRADES_GRIP] = ZERO.ToString();
			if (Save.Upgrades.Contains(Upgrades.Screw))
				lines[UPGRADES_SCREW] = UPGRADES_SCREW.ToString();
			else
				lines[UPGRADES_SCREW] = ZERO.ToString();

			//Keys
			if(Save.Keys.Contains(Keys.Red))
				lines[KEY_RED] = KEY_RED.ToString();
			else
				lines[KEY_RED] = ZERO.ToString();
			if (Save.Keys.Contains(Keys.Yellow))
				lines[KEY_YELLOW] = KEY_YELLOW.ToString();
			else
				lines[KEY_YELLOW] = ZERO.ToString();
			if (Save.Keys.Contains(Keys.Green))
				lines[KEY_GREEN] = KEY_GREEN.ToString();
			else
				lines[KEY_GREEN] = ZERO.ToString();
			if (Save.Keys.Contains(Keys.Blue))
				lines[KEY_BLUE] = KEY_BLUE.ToString();
			else
				lines[KEY_BLUE] = ZERO.ToString();
			if (Save.Keys.Contains(Keys.Purple))
				lines[KEY_PURPLE] = KEY_PURPLE.ToString();
			else
				lines[KEY_PURPLE] = ZERO.ToString();

			//Position in stage
			lines[POS_X] = Save.PosX.ToString();
			lines[POS_Y] = Save.PosY.ToString();
			lines[STAGE] = ((int)Save.Stage).ToString();

			//hp and mp upgrades
			lines[HP_UPGRADES] = Save.HpUpgrades.ToString();
			lines[MP_UPGRADES] = Save.MpUpgrades.ToString();

			//xp
			lines[EXP] = Save.Exp.ToString();

			//File save date
			if(useNowDate)
				lines[SAVE_DATE] = DateTime.Now.ToString(DATE_FORMAT, CultureInfo.CurrentCulture);
			else
				lines[SAVE_DATE] = Save.SaveDate.ToString(DATE_FORMAT, CultureInfo.CurrentCulture);

			//Count of upgrades
			lines[KNIFE_UPGRADES] = Save.KnifeUpgrades.ToString();
			lines[CLOCK_UPGRADES] = Save.ClockUpgrades.ToString();

			//Gems
			lines[AMETHYST] = Save.Amethyst.ToString();
			lines[TURQUOISE] = Save.Turquoise.ToString();
			lines[TOPAZ] = Save.Topaz.ToString();
			lines[RUBY] = Save.Ruby.ToString();
			lines[SAPPHIRE] = Save.Sapphire.ToString();
			lines[EMERALD] = Save.Emerald.ToString();
			lines[DIAMOND] = Save.Diamond.ToString();

			//Upgrade level
			lines[UPGRADE_LEVEL] = ((int)Save.UpgradeLevel).ToString();

			//Gold
			lines[GOLD] = Save.Gold.ToString();

			//TrashCans
			var cans = Save.TrashCans.Select(x => ((int)x).ToString());
			lines[TRASH_CANS] = $"{(cans.Count() == 0 ? ZERO.ToString() : $"{string.Join(",", cans)},")}";

			//Bought Upgrades
			lines[CLOCK_UPGRADES_BOUGHT] = Save.ClockUpgradesBought.ToString();
			lines[KNIFE_UPGRADES_BOUGHT] = Save.KnifeUpgradesBought.ToString();

			//Gem Statues
			if (Save.Statues.Contains(Statues.Amethyst))
				lines[STATUE_AMETHYST] = STATUE_AMETHYST.ToString();
			else
				lines[STATUE_AMETHYST] = ZERO.ToString();
			if (Save.Statues.Contains(Statues.Turquoise))
				lines[STATUE_TURQUOISE] = STATUE_TURQUOISE.ToString();
			else
				lines[STATUE_TURQUOISE] = ZERO.ToString();
			if (Save.Statues.Contains(Statues.Topaz))
				lines[STATUE_TOPAZ] = STATUE_TOPAZ.ToString();
			else
				lines[STATUE_TOPAZ] = ZERO.ToString();
			if (Save.Statues.Contains(Statues.Ruby))
				lines[STATUE_RUBY] = STATUE_RUBY.ToString();
			else
				lines[STATUE_RUBY] = ZERO.ToString();
			if (Save.Statues.Contains(Statues.Sapphire))
				lines[STATUE_SAPPHIRE] = STATUE_SAPPHIRE.ToString();
			else
				lines[STATUE_SAPPHIRE] = ZERO.ToString();
			if (Save.Statues.Contains(Statues.Emerald))
				lines[STATUE_EMERALD] = STATUE_EMERALD.ToString();
			else
				lines[STATUE_EMERALD] = ZERO.ToString();
			if (Save.Statues.Contains(Statues.Diamond))
				lines[STATUE_DIAMOND] = STATUE_DIAMOND.ToString();
			else
				lines[STATUE_DIAMOND] = ZERO.ToString();

			await WriteLines(_currentPath, lines.ToArray());
		}

		/// <summary>
		/// Will create a new save at the given path
		/// </summary>
		/// <param name="path">the path to create the save at</param>
		/// <returns></returns>
		public async Task CreateSave(string path)
		{
			_currentPath = path;
			var lines = new string[LINE_COUNT];
			for(var i = 0; i < LINE_COUNT; ++i)
			{
				lines[i] = ZERO.ToString();
			}
			lines[STAGE] = ((int)Stage.Opening).ToString();
			byte[] bytes = null;
			for (var i = 0; i < LINE_COUNT; ++i)
			{
				bytes = Encoding.UTF8.GetBytes(lines[i]);
				lines[i] = Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None);
			}

			File.Delete(_currentPath);
			File.Create(_currentPath);

			await WriteLines(_currentPath, lines);
		}

		private async Task<List<string>> LoadLines(string path)
		{
			var lines = new List<string>();

			using (var file = new StreamReader(path))
			{
				string line = null;
				while ((line = await file.ReadLineAsync()) != null)
				{
					lines.Add(Encoding.UTF8.GetString(Convert.FromBase64String(line)));
				}
			}

			return lines;
		}

		private async Task WriteLines(string path, string[] lines)
		{
			using (var writer = new StreamWriter(path))
			{
				for (var i = 0; i < LINE_COUNT; ++i)
				{
					await writer.WriteLineAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(lines[i]), Base64FormattingOptions.None));
				}
			}
		}
	}
}

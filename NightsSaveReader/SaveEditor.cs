using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightsSaveReader
{
	public class SaveEditor
	{
		private static readonly int _lineCount = 1000;
		public static readonly string DateFormat = "HH:mm dd/MM/yyyy";
		public static readonly int BingoWidth = 9;
		public static readonly int BingoHeight = 7;
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

			if (lines.Count != _lineCount)
				throw new Exception("Save file may be corrupted");

			//Inventory Hp upgrades
			if (lines[20] == "20")
				Save.HpUpgradesInv.Add(HpUpgrades.S1);
			if (lines[21] == "21")
				Save.HpUpgradesInv.Add(HpUpgrades.S2);
			if (lines[22] == "22")
				Save.HpUpgradesInv.Add(HpUpgrades.S3);
			if (lines[23] == "23")
				Save.HpUpgradesInv.Add(HpUpgrades.S4);
			if (lines[24] == "24")
				Save.HpUpgradesInv.Add(HpUpgrades.S5);

			//Inventory Mp upgrades
			if (lines[40] == "40")
				Save.MpUpgradesInv.Add(MpUpgrades.S1);
			if (lines[41] == "41")
				Save.MpUpgradesInv.Add(MpUpgrades.S2);
			if (lines[42] == "42")
				Save.MpUpgradesInv.Add(MpUpgrades.S3);
			if (lines[43] == "43")
				Save.MpUpgradesInv.Add(MpUpgrades.S4);
			if (lines[44] == "44")
				Save.MpUpgradesInv.Add(MpUpgrades.S5);

			//Inventory Knife upgrades
			if (lines[60] == "60")
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S1);
			if (lines[61] == "61")
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S2);
			if (lines[62] == "62")
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S3);
			if (lines[63] == "63")
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S4);
			if (lines[64] == "64")
				Save.KnifeUpgradesInv.Add(KnifeUpgrades.S5);

			//Inventory clock upgrades
			if (lines[70] == "70")
				Save.ClockUpgradesInv.Add(ClockUpgrades.S1);
			if (lines[71] == "71")
				Save.ClockUpgradesInv.Add(ClockUpgrades.S2);
			if (lines[72] == "72")
				Save.ClockUpgradesInv.Add(ClockUpgrades.S3);
			if (lines[73] == "73")
				Save.ClockUpgradesInv.Add(ClockUpgrades.S4);
			if (lines[74] == "74")
				Save.ClockUpgradesInv.Add(ClockUpgrades.S5);

			//Inventory Upgrades
			if (lines[80] == "80")
				Save.Upgrades.Add(Upgrades.Slide);
			if (lines[81] == "81")
				Save.Upgrades.Add(Upgrades.Double);
			if (lines[82] == "82")
				Save.Upgrades.Add(Upgrades.Grip);
			if (lines[83] == "83")
				Save.Upgrades.Add(Upgrades.Screw);

			//Keys
			if (lines[90] == "90")
				Save.Keys.Add(Keys.Red);
			if (lines[91] == "91")
				Save.Keys.Add(Keys.Yellow);
			if (lines[92] == "92")
				Save.Keys.Add(Keys.Green);
			if (lines[93] == "93")
				Save.Keys.Add(Keys.Blue);
			if (lines[94] == "94")
				Save.Keys.Add(Keys.Purple);

			//Position in stage
			int.TryParse(lines[100], out Save.PosX);
			int.TryParse(lines[101], out Save.PosY);
			Enum.TryParse(lines[102], out Save.Stage);

			//hp and mp upgrades
			int.TryParse(lines[104], out Save.HpUpgrades);
			int.TryParse(lines[105], out Save.MpUpgrades);

			//xp
			double.TryParse(lines[106], out Save.Exp);

			//File save date
			Save.SaveDate = DateTime.ParseExact(lines[107], DateFormat, CultureInfo.CurrentCulture);

			//Count of upgrades
			int.TryParse(lines[108], out Save.KnifeUpgrades);
			int.TryParse(lines[109], out Save.ClockUpgrades);

			//Gems
			int.TryParse(lines[110], out Save.Amethyst);
			int.TryParse(lines[111], out Save.Turquoise);
			int.TryParse(lines[112], out Save.Topaz);
			int.TryParse(lines[113], out Save.Ruby);
			int.TryParse(lines[114], out Save.Sapphire);
			int.TryParse(lines[115], out Save.Emerald);
			int.TryParse(lines[116], out Save.Diamond);

			//Upgrade level
			Enum.TryParse(lines[117], out Save.UpgradeLevel);

			//Gold
			int.TryParse(lines[118], out Save.Gold);

			//TrashCans
			//TODO: Do this when I figure out all the ids
			var trashLines = lines[120].Split(',');
			foreach (var l in trashLines)
			{
				if (Enum.TryParse(l, out TrashCans can))
					Save.TrashCans.Add(can);
			}

			//Bought Upgrades
			int.TryParse(lines[134], out Save.ClockUpgradesBought);
			int.TryParse(lines[135], out Save.KnifeUpgradesBought);

			//Gem Statues
			if (lines[250] == "250")
				Save.Statues.Add(Statues.Amethyst);
			if (lines[251] == "251")
				Save.Statues.Add(Statues.Turquoise);
			if (lines[252] == "252")
				Save.Statues.Add(Statues.Topaz);
			if (lines[253] == "253")
				Save.Statues.Add(Statues.Ruby);
			if (lines[254] == "254")
				Save.Statues.Add(Statues.Sapphire);
			if (lines[255] == "255")
				Save.Statues.Add(Statues.Emerald);
			if (lines[256] == "256")
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
				lines[20] = "20";
			else
				lines[20] = "0";
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S2))
				lines[21] = "21";
			else
				lines[21] = "0";
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S3))
				lines[22] = "22";
			else
				lines[22] = "0";
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S4))
				lines[23] = "23";
			else
				lines[23] = "0";
			if (Save.HpUpgradesInv.Contains(HpUpgrades.S5))
				lines[24] = "24";
			else
				lines[24] = "0";

			//Inventory Mp upgrades
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S1))
				lines[40] = "40";
			else
				lines[40] = "0";
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S2))
				lines[41] = "41";
			else
				lines[41] = "0";
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S3))
				lines[42] = "42";
			else
				lines[42] = "0";
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S4))
				lines[43] = "43";
			else
				lines[43] = "0";
			if (Save.MpUpgradesInv.Contains(MpUpgrades.S5))
				lines[44] = "44";
			else
				lines[44] = "0";

			//Inventory knife upgrades
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S1))
				lines[60] = "60";
			else
				lines[60] = "0";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S2))
				lines[61] = "61";
			else
				lines[61] = "0";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S3))
				lines[62] = "62";
			else
				lines[62] = "0";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S4))
				lines[63] = "63";
			else
				lines[63] = "0";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S5))
				lines[64] = "64";
			else
				lines[64] = "0";

			//Inventory clock upgrades
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S1))
				lines[70] = "70";
			else
				lines[70] = "0";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S2))
				lines[71] = "71";
			else
				lines[71] = "0";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S3))
				lines[72] = "72";
			else
				lines[72] = "0";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S4))
				lines[73] = "73";
			else
				lines[73] = "0";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S5))
				lines[74] = "74";
			else
				lines[74] = "0";

			//Inventory Upgrades
			if (Save.Upgrades.Contains(Upgrades.Slide))
				lines[80] = "80";
			else
				lines[80] = "0";
			if (Save.Upgrades.Contains(Upgrades.Double))
				lines[81] = "81";
			else
				lines[81] = "0";
			if (Save.Upgrades.Contains(Upgrades.Grip))
				lines[82] = "82";
			else
				lines[82] = "0";
			if (Save.Upgrades.Contains(Upgrades.Screw))
				lines[83] = "83";
			else
				lines[83] = "0";

			//Keys
			if(Save.Keys.Contains(Keys.Red))
				lines[90] = "90";
			else
				lines[90] = "0";
			if (Save.Keys.Contains(Keys.Yellow))
				lines[91] = "91";
			else
				lines[91] = "0";
			if (Save.Keys.Contains(Keys.Green))
				lines[92] = "92";
			else
				lines[92] = "0";
			if (Save.Keys.Contains(Keys.Blue))
				lines[93] = "93";
			else
				lines[93] = "0";
			if (Save.Keys.Contains(Keys.Purple))
				lines[94] = "94";
			else
				lines[94] = "0";

			//Position in stage
			lines[100] = Save.PosX.ToString();
			lines[101] = Save.PosY.ToString();
			lines[102] = ((int)Save.Stage).ToString();

			//hp and mp upgrades
			lines[104] = Save.HpUpgrades.ToString();
			lines[105] = Save.MpUpgrades.ToString();

			//xp
			lines[106] = Save.Exp.ToString();

			//File save date
			if(useNowDate)
				lines[107] = DateTime.Now.ToString(DateFormat, CultureInfo.CurrentCulture);
			else
				lines[107] = Save.SaveDate.ToString(DateFormat, CultureInfo.CurrentCulture);

			//Count of upgrades
			lines[108] = Save.KnifeUpgrades.ToString();
			lines[109] = Save.ClockUpgrades.ToString();

			//Gems
			lines[110] = Save.Amethyst.ToString();
			lines[111] = Save.Turquoise.ToString();
			lines[112] = Save.Topaz.ToString();
			lines[113] = Save.Ruby.ToString();
			lines[114] = Save.Sapphire.ToString();
			lines[115] = Save.Emerald.ToString();
			lines[116] = Save.Diamond.ToString();

			//Upgrade level
			lines[117] = ((int)Save.UpgradeLevel).ToString();

			//Gold
			lines[118] = Save.Gold.ToString();

			//TrashCans
			var cans = Save.TrashCans.Select(x => ((int)x).ToString());
			lines[120] = $"{(cans.Count() == 0 ? "0" : $"{string.Join(",", cans)},")}";

			//Bought Upgrades
			lines[134] = Save.ClockUpgradesBought.ToString();
			lines[135] = Save.KnifeUpgradesBought.ToString();

			//Gem Statues
			if (Save.Statues.Contains(Statues.Amethyst))
				lines[250] = "250";
			else
				lines[250] = "0";
			if (Save.Statues.Contains(Statues.Turquoise))
				lines[251] = "251";
			else
				lines[251] = "0";
			if (Save.Statues.Contains(Statues.Topaz))
				lines[252] = "252";
			else
				lines[252] = "0";
			if (Save.Statues.Contains(Statues.Ruby))
				lines[253] = "253";
			else
				lines[253] = "0";
			if (Save.Statues.Contains(Statues.Sapphire))
				lines[254] = "254";
			else
				lines[254] = "0";
			if (Save.Statues.Contains(Statues.Emerald))
				lines[255] = "255";
			else
				lines[255] = "0";
			if (Save.Statues.Contains(Statues.Diamond))
				lines[256] = "256";
			else
				lines[256] = "0";

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
			var lines = new string[_lineCount];
			for(var i = 0; i < _lineCount; ++i)
			{
				lines[i] = "0";
			}
			lines[102] = ((int)Stage.Opening).ToString();
			byte[] bytes = null;
			for (var i = 0; i < _lineCount; ++i)
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
				for (var i = 0; i < _lineCount; ++i)
				{
					await writer.WriteLineAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(lines[i]), Base64FormattingOptions.None));
				}
			}
		}
	}
}

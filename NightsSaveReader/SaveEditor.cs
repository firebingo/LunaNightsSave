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

			//Inventory knife upgrades
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
				Save.HasSlide = true;
			if (lines[81] == "81")
				Save.HasDouble = true;
			if (lines[82] == "82")
				Save.HasGrip = true;
			if (lines[83] == "83")
				Save.HasScrew = true;

			//Keys
			if (lines[90] == "90")
				Save.HasRedKey = true;
			if (lines[91] == "91")
				Save.HasYellowKey = true;
			if (lines[92] == "92")
				Save.HasGreenKey = true;
			if (lines[93] == "93")
				Save.HasBlueKey = true;
			if (lines[94] == "94")
				Save.HasPurpleKey = true;

			//Position in stage
			int.TryParse(lines[100], out Save.PosX);
			int.TryParse(lines[101], out Save.PosY);
			Enum.TryParse(lines[102], out Save.Stage);

			//hp and mp upgrades
			int.TryParse(lines[104], out Save.HpUpgrades);
			int.TryParse(lines[105], out Save.MpUpgrades);

			//xp
			int.TryParse(lines[106], out Save.Exp);

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

			}

			//Bought Upgrades
			int.TryParse(lines[134], out Save.ClockUpgradesBought);
			int.TryParse(lines[135], out Save.KnifeUpgradesBought);
		}

		public async Task WriteSave()
		{
			var lines = await LoadLines(_currentPath);

			//Inventory knife upgrades
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S1))
				lines[60] = "60";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S2))
				lines[61] = "61";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S3))
				lines[62] = "62";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S4))
				lines[63] = "63";
			if (Save.KnifeUpgradesInv.Contains(KnifeUpgrades.S5))
				lines[64] = "64";

			//Inventory clock upgrades
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S1))
				lines[70] = "70";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S2))
				lines[71] = "71";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S3))
				lines[72] = "72";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S4))
				lines[73] = "73";
			if (Save.ClockUpgradesInv.Contains(ClockUpgrades.S5))
				lines[74] = "74";

			//Inventory Upgrades
			if (Save.HasSlide)
				lines[80] = "80";
			if (Save.HasDouble)
				lines[81] = "81";
			if (Save.HasGrip)
				lines[82] = "82";
			if (Save.HasScrew)
				lines[83] = "83";

			//Keys
			if (Save.HasRedKey)
				lines[90] = "90";
			if (Save.HasYellowKey)
				lines[91] = "91";
			if (Save.HasGreenKey)
				lines[92] = "92";
			if (Save.HasBlueKey)
				lines[93] = "93";
			if (Save.HasPurpleKey)
				lines[94] = "94";

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
			lines[115] = Save.Diamond.ToString();

			//Upgrade level
			lines[117] = ((int)Save.UpgradeLevel).ToString();

			//Gold
			lines[118] = Save.Gold.ToString();

			//TrashCans
			//TODO: Do this when I figure out all the ids
			//var cans = Save.TrashCans.Select(x => ((int)x).ToString());
			//lines[120] = $"{(cans.Count() == 0 ? "0" : $"{string.Join(",", cans)},")}";

			//Bought Upgrades
			lines[134] = Save.ClockUpgradesBought.ToString();
			lines[135] = Save.KnifeUpgradesBought.ToString();

			await WriteLines(_currentPath, lines.ToArray());
		}

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
					await writer.WriteLineAsync(lines[i]);
				}
			}
		}
	}
}

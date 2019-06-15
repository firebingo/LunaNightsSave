using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NightsSaveReader
{
	/// <summary>
	/// Static line number definitions for the save file. Can be modified and loaded from "AppData/SaveLines.json"
	/// </summary>
	public static class SaveLines
	{
		private static readonly string LinesPath = @"AppData/SaveLines.json";
		static SaveLines()
		{
			try
			{
				if (!File.Exists(LinesPath))
					return;

				JObject lines = null;
				using (StreamReader file = File.OpenText(LinesPath))
				{
					using (JsonTextReader reader = new JsonTextReader(file))
					{
						lines = (JObject)JToken.ReadFrom(reader);
					}
				}

				if (lines == null)
					return;

				if (lines.TryParseJsonProperty("zero", out int i))
					ZERO = i;

				if (lines.TryParseJsonProperty("lineCount", out i))
					LINE_COUNT = i;

				if (lines.TryParseJsonProperty("dateFormat", out string s))
					DATE_FORMAT = s;

				if (lines.TryParseJsonProperty("hpUpgradeS1", out i))
					HP_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("hpUpgradeS2", out i))
					HP_UPGRADE_S2 = i;
				if (lines.TryParseJsonProperty("hpUpgradeS3", out i))
					HP_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("hpUpgradeS4", out i))
					HP_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("hpUpgradeS5", out i))
					HP_UPGRADE_S1 = i;

				if (lines.TryParseJsonProperty("mpUpgradeS1", out i))
					MP_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("mpUpgradeS2", out i))
					MP_UPGRADE_S2 = i;
				if (lines.TryParseJsonProperty("mpUpgradeS3", out i))
					MP_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("mpUpgradeS4", out i))
					MP_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("mpUpgradeS5", out i))
					MP_UPGRADE_S1 = i;

				if (lines.TryParseJsonProperty("knifeUpgradeS1", out i))
					KNIFE_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("knifeUpgradeS2", out i))
					KNIFE_UPGRADE_S2 = i;
				if (lines.TryParseJsonProperty("knifeUpgradeS3", out i))
					KNIFE_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("knifeUpgradeS4", out i))
					KNIFE_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("knifeUpgradeS5", out i))
					KNIFE_UPGRADE_S1 = i;

				if (lines.TryParseJsonProperty("clockUpgradeS1", out i))
					CLOCK_UPGRADE_S1 = i;
				if (lines.TryParseJsonProperty("clockUpgradeS2", out i))
					CLOCK_UPGRADE_S2 = i;
				if (lines.TryParseJsonProperty("clockUpgradeS3", out i))
					CLOCK_UPGRADE_S3 = i;
				if (lines.TryParseJsonProperty("clockUpgradeS4", out i))
					CLOCK_UPGRADE_S4 = i;
				if (lines.TryParseJsonProperty("clockUpgradeS5", out i))
					CLOCK_UPGRADE_S5 = i;

				if (lines.TryParseJsonProperty("upgradesSlide", out i))
					UPGRADES_SLIDE = i;
				if (lines.TryParseJsonProperty("upgradesDouble", out i))
					UPGRADES_DOUBLE = i;
				if (lines.TryParseJsonProperty("upgradesGrip", out i))
					UPGRADES_GRIP = i;
				if (lines.TryParseJsonProperty("upgradesScrew", out i))
					UPGRADES_SCREW = i;
				if (lines.TryParseJsonProperty("upgradesDash", out i))
					UPGRADES_DASH = i;

				if (lines.TryParseJsonProperty("keyRed", out i))
					KEY_RED = i;
				if (lines.TryParseJsonProperty("keyYellow", out i))
					KEY_YELLOW = i;
				if (lines.TryParseJsonProperty("keyGreen", out i))
					KEY_GREEN = i;
				if (lines.TryParseJsonProperty("keyBlue", out i))
					KEY_BLUE = i;
				if (lines.TryParseJsonProperty("keyPurple", out i))
					KEY_PURPLE = i;
				if (lines.TryParseJsonProperty("iceMagatama", out i))
					ICE_MAGATAMA = i;

				if (lines.TryParseJsonProperty("posX", out i))
					POS_X = i;
				if (lines.TryParseJsonProperty("posY", out i))
					POS_Y = i;
				if (lines.TryParseJsonProperty("stage", out i))
					STAGE = i;

				if (lines.TryParseJsonProperty("hpUpgrades", out i))
					HP_UPGRADES = i;
				if (lines.TryParseJsonProperty("mpUpgrades", out i))
					MP_UPGRADES = i;

				if (lines.TryParseJsonProperty("exp", out i))
					EXP = i;

				if (lines.TryParseJsonProperty("saveDate", out i))
					SAVE_DATE = i;

				if (lines.TryParseJsonProperty("knifeUpgrades", out i))
					KNIFE_UPGRADES = i;
				if (lines.TryParseJsonProperty("clockUpgrades", out i))
					CLOCK_UPGRADES = i;

				if (lines.TryParseJsonProperty("amethyst", out i))
					AMETHYST = i;
				if (lines.TryParseJsonProperty("turquoise", out i))
					TURQUOISE = i;
				if (lines.TryParseJsonProperty("topaz", out i))
					TOPAZ = i;
				if (lines.TryParseJsonProperty("ruby", out i))
					RUBY = i;
				if (lines.TryParseJsonProperty("sapphire", out i))
					SAPPHIRE = i;
				if (lines.TryParseJsonProperty("emerald", out i))
					EMERALD = i;
				if (lines.TryParseJsonProperty("diamond", out i))
					DIAMOND = i;

				if (lines.TryParseJsonProperty("upgradeLevel", out i))
					UPGRADE_LEVEL = i;

				if (lines.TryParseJsonProperty("gold", out i))
					GOLD = i;

				if (lines.TryParseJsonProperty("trashCans", out i))
					TRASH_CANS = i;

				if (lines.TryParseJsonProperty("clockUpgradesBought", out i))
					CLOCK_UPGRADES_BOUGHT = i;
				if (lines.TryParseJsonProperty("knifeUpgradesBought", out i))
					KNIFE_UPGRADES_BOUGHT = i;

				if (lines.TryParseJsonProperty("statueAmethyst", out i))
					STATUE_AMETHYST = i;
				if (lines.TryParseJsonProperty("statueTurquoise", out i))
					STATUE_TURQUOISE = i;
				if (lines.TryParseJsonProperty("statueTopaz", out i))
					STATUE_TOPAZ = i;
				if (lines.TryParseJsonProperty("statueRuby", out i))
					STATUE_RUBY = i;
				if (lines.TryParseJsonProperty("statueSapphire", out i))
					STATUE_SAPPHIRE = i;
				if (lines.TryParseJsonProperty("statueEmerald", out i))
					STATUE_EMERALD = i;
				if (lines.TryParseJsonProperty("statueDiamond", out i))
					STATUE_DIAMOND = i;
			}
			catch { }
		}

		public static readonly int ZERO = 0;

		public static readonly int LINE_COUNT = 1000;

		public static readonly string DATE_FORMAT = "HH:mm dd/MM/yyyy";

		public static readonly int HP_UPGRADE_S1 = 20;
		public static readonly int HP_UPGRADE_S2 = 21;
		public static readonly int HP_UPGRADE_S3 = 22;
		public static readonly int HP_UPGRADE_S4 = 23;
		public static readonly int HP_UPGRADE_S5 = 24;

		public static readonly int MP_UPGRADE_S1 = 40;
		public static readonly int MP_UPGRADE_S2 = 41;
		public static readonly int MP_UPGRADE_S3 = 42;
		public static readonly int MP_UPGRADE_S4 = 43;
		public static readonly int MP_UPGRADE_S5 = 44;

		public static readonly int KNIFE_UPGRADE_S1 = 60;
		public static readonly int KNIFE_UPGRADE_S2 = 61;
		public static readonly int KNIFE_UPGRADE_S3 = 62;
		public static readonly int KNIFE_UPGRADE_S4 = 63;
		public static readonly int KNIFE_UPGRADE_S5 = 64;

		public static readonly int CLOCK_UPGRADE_S1 = 70;
		public static readonly int CLOCK_UPGRADE_S2 = 71;
		public static readonly int CLOCK_UPGRADE_S3 = 72;
		public static readonly int CLOCK_UPGRADE_S4 = 73;
		public static readonly int CLOCK_UPGRADE_S5 = 74;

		public static readonly int UPGRADES_SLIDE = 80;
		public static readonly int UPGRADES_DOUBLE = 81;
		public static readonly int UPGRADES_GRIP = 82;
		public static readonly int UPGRADES_SCREW = 83;
		public static readonly int UPGRADES_DASH = 84;

		public static readonly int KEY_RED = 90;
		public static readonly int KEY_YELLOW = 91;
		public static readonly int KEY_GREEN = 92;
		public static readonly int KEY_BLUE = 93;
		public static readonly int KEY_PURPLE = 94;
		public static readonly int ICE_MAGATAMA = 95;

		public static readonly int POS_X = 100;
		public static readonly int POS_Y = 101;
		public static readonly int STAGE = 102;

		public static readonly int HP_UPGRADES = 104;
		public static readonly int MP_UPGRADES = 105;

		public static readonly int EXP = 106;

		public static readonly int SAVE_DATE = 107;

		public static readonly int KNIFE_UPGRADES = 108;
		public static readonly int CLOCK_UPGRADES = 109;

		public static readonly int AMETHYST = 110;
		public static readonly int TURQUOISE = 111;
		public static readonly int TOPAZ = 112;
		public static readonly int RUBY = 113;
		public static readonly int SAPPHIRE = 114;
		public static readonly int EMERALD = 115;
		public static readonly int DIAMOND = 116;

		public static readonly int UPGRADE_LEVEL = 117;

		public static readonly int GOLD = 118;

		public static readonly int TRASH_CANS = 120;

		public static readonly int CLOCK_UPGRADES_BOUGHT = 134;
		public static readonly int KNIFE_UPGRADES_BOUGHT = 135;

		public static readonly int STATUE_AMETHYST = 250;
		public static readonly int STATUE_TURQUOISE = 251;
		public static readonly int STATUE_TOPAZ = 252;
		public static readonly int STATUE_RUBY = 253;
		public static readonly int STATUE_SAPPHIRE = 254;
		public static readonly int STATUE_EMERALD = 255;
		public static readonly int STATUE_DIAMOND = 256;
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NightsSaveReader
{
	public static class SaveHelpers
	{

		public static StageDisplay StageToStageDisplay(Stage s)
		{
			switch (s)
			{
				case Stage.NewGame:
					return StageDisplay.NewGame;
				case Stage.Opening:
					return StageDisplay.Opening;
				case Stage.Title:
					return StageDisplay.Title;
				case Stage.SaveSelect:
					return StageDisplay.SaveSelect;
				case Stage.BlackScreen:
					return StageDisplay.BlackScreen;
				case Stage.Stage1:
					return StageDisplay.Stage1;
				case Stage.Stage2:
					return StageDisplay.Stage2;
				case Stage.Stage3:
					return StageDisplay.Stage3;
				case Stage.Stage4:
					return StageDisplay.Stage4;
				case Stage.Stage5:
					return StageDisplay.Stage5;
				case Stage.StageExtra:
					return StageDisplay.StageExtra;
				case Stage.Credits:
					return StageDisplay.Credits;
				default:
					return StageDisplay.NewGame;
			}
		}

		public static HpUpgrades StageToHpUpgrades(Stage s)
		{
			switch (s)
			{
				case Stage.Stage1:
					return HpUpgrades.S1;
				case Stage.Stage2:
					return HpUpgrades.S2;
				case Stage.Stage3:
					return HpUpgrades.S3;
				case Stage.Stage4:
					return HpUpgrades.S4;
				case Stage.Stage5:
					return HpUpgrades.S5;
				default:
					return HpUpgrades.S1;
			}
		}

		public static MpUpgrades StageToMpUpgrades(Stage s)
		{
			switch (s)
			{
				case Stage.Stage1:
					return MpUpgrades.S1;
				case Stage.Stage2:
					return MpUpgrades.S2;
				case Stage.Stage3:
					return MpUpgrades.S3;
				case Stage.Stage4:
					return MpUpgrades.S4;
				case Stage.Stage5:
					return MpUpgrades.S5;
				default:
					return MpUpgrades.S1;
			}
		}

		public static ClockUpgrades StageToClockUpgrades(Stage s)
		{
			switch(s)
			{
				case Stage.Stage1:
					return ClockUpgrades.S1;
				case Stage.Stage2:
					return ClockUpgrades.S2;
				case Stage.Stage3:
					return ClockUpgrades.S3;
				case Stage.Stage4:
					return ClockUpgrades.S4;
				case Stage.Stage5:
					return ClockUpgrades.S5;
				default:
					return ClockUpgrades.S1;
			}
		}

		public static KnifeUpgrades StageToKnifeUpgrades(Stage s)
		{
			switch (s)
			{
				case Stage.Stage1:
					return KnifeUpgrades.S1;
				case Stage.Stage2:
					return KnifeUpgrades.S2;
				case Stage.Stage3:
					return KnifeUpgrades.S3;
				case Stage.Stage4:
					return KnifeUpgrades.S4;
				case Stage.Stage5:
					return KnifeUpgrades.S5;
				default:
					return KnifeUpgrades.S1;
			}
		}
	}
}

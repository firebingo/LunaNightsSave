using LunaNightsSave.Helpers;

namespace LunaNightsSave.Models
{
	public class ConfigModel
	{
		public WINDOWPLACEMENT WindowPlacement;
		public bool AutoSave;
		public int AutoSaveIntervalMs;

		public ConfigModel()
		{
			AutoSave = false;
			AutoSaveIntervalMs = 5000;
		}
	}
}

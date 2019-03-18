using LunaNightsSave.Helpers;

namespace LunaNightsSave.Models
{
	public class ConfigModel
	{
		public WINDOWPLACEMENT WindowPlacement;
		public bool AutoSave;

		public ConfigModel()
		{
			AutoSave = false;
		}
	}
}

using System.Threading.Tasks;

namespace LunaNightsSave.Helpers
{
	public static class ErrorTracker
	{
		public delegate void OnUpdate(string error);
		public static event OnUpdate UpdateError;

		private static string _currentError;
		/// <summary>
		/// Call this from a SynchronizationContext preferably
		/// </summary>
		public static string CurrentError
		{
			get
			{
				return _currentError;
			}
			set
			{
				_currentError = value;
				UpdateError?.Invoke(_currentError);
			}
		}

		public static async Task DelayClearError(string msg, int ms)
		{
			await Task.Delay(ms);
			if (CurrentError == msg)
				CurrentError = string.Empty;
		}
	}
}

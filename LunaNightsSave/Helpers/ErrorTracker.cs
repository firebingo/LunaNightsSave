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
	}
}

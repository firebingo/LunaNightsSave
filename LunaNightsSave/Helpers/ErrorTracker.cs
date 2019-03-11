namespace LunaNightsSave.Helpers
{
	public static class ErrorTracker
	{
		public delegate void OnUpdate(string error);
		public static event OnUpdate UpdateError;

		private static string _currentError;
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

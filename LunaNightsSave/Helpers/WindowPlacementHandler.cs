using System;
using System.Runtime.InteropServices;
using LunaNightsSave.Function;

namespace LunaNightsSave.Helpers
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct WINDOWPLACEMENT
	{
		public int length;
		public int flags;
		public int showCmd;
		public POINT minPosition;
		public POINT maxPosition;
		public RECT normalPosition;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
	}

	// POINT structure required by WINDOWPLACEMENT structure
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int X;
		public int Y;

		public POINT(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public static class WindowPlacementHandler
	{
		[DllImport("user32.dll")]
		private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll")]
		private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

		private const int SW_SHOWNORMAL = 1;
		private const int SW_SHOWMINIMIZED = 2;

		public static void SetPlacement(IntPtr windowHandle)
		{
			WINDOWPLACEMENT placement;

			try
			{
				placement = Config.Instance.ConfigObject.WindowPlacement;
				//If the top and bottom are both 0 it probably means the config file had it wiped and we want to go back to defaults.
				if (placement.normalPosition.Bottom == 0 && placement.normalPosition.Top == 0)
					return;

				placement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
				placement.flags = 0;
				placement.showCmd = (placement.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : placement.showCmd);
				SetWindowPlacement(windowHandle, ref placement);
			}
			catch (InvalidOperationException) { }
		}

		public static void GetPlacement(IntPtr windowHandle)
		{
			WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
			GetWindowPlacement(windowHandle, out placement);

			Config.Instance.ConfigObject.WindowPlacement = placement;
		}
	}
}

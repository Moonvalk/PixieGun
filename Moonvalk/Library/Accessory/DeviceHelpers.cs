using Godot;

namespace Moonvalk.Accessory
{
	/// <summary>
	/// Helper functions for detecting devices.
	/// </summary>
	public static class DeviceHelpers
	{
		/// <summary>
		/// All available device parsing states.
		/// </summary>
		public enum DeviceState
		{
			None,
			Desktop,
			Mobile,
			HTML5,
		}

		/// <summary>
		/// Stores the current device state.
		/// </summary>
		public static DeviceState CurrentState { get; private set; } = DeviceState.None;

		/// <summary>
		/// Determines if the device is mobile or desktop based on operating system.
		/// </summary>
		/// <returns>Returns true if the device is determined mobile.</returns>
		public static bool IsDeviceMobile()
		{
			setDeviceState();
			return CurrentState == DeviceState.Mobile;
		}

		/// <summary>
		/// Determines if the device is mobile or desktop based on operating system.
		/// </summary>
		/// <returns>Returns true if the device is determined mobile.</returns>
		public static bool IsDeviceHTML5()
		{
			setDeviceState();
			return CurrentState == DeviceState.HTML5;
		}

		/// <summary>
		/// Determines if the device is mobile or desktop based on operating system.
		/// </summary>
		/// <returns>Returns true if the device is determined mobile.</returns>
		public static bool IsDeviceDesktop()
		{
			setDeviceState();
			return CurrentState == DeviceState.Desktop;
		}

		/// <summary>
		/// Helper for determining the current device.
		/// </summary>
		private static void setDeviceState()
		{
			if (CurrentState != DeviceState.None)
			{
				return;
			}
			string os = OS.GetName();
			if (os == "Windows" || os == "OSX" || os == "macOS" || os == "Linux" || os == "Server" || os == "UWP")
			{
				CurrentState = DeviceState.Desktop;
			}
			else if (os == "HTML5" || os == "Web")
			{
				CurrentState = DeviceState.HTML5;
			}
			else if (os == "Android" || os == "iOS" || os == "BlackBerry 10")
			{
				CurrentState = DeviceState.Mobile;
			}
		}
	}
}

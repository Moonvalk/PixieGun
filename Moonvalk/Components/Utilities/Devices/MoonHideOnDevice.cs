using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Components {
	/// <summary>
	/// Component that is responsible for showing or hiding Nodes based on device type.
	/// </summary>
	public class MoonHideOnDevice : Node {
		/// <summary>
		/// All nodes to show or hide based on device.
		/// </summary>
		[Export] protected NodePath[] p_elementsToHideOnMobile { get; set; }

		/// <summary>
		/// True to hide these elements on mobile.
		/// </summary>
		[Export] public bool HideOnMobile { get; protected set; } = true;

		/// <summary>
		/// True to hide these elements on HTML5 web.
		/// </summary>
		[Export] public bool HideOnHTML5 { get; protected set; } = false;

		/// <summary>
		/// True to hide these elements on desktop.
		/// </summary>
		[Export] public bool HideOnDesktop { get; protected set; } = false;

		/// <summary>
		/// Called when object is first initialized.
		/// </summary>
		public override void _Ready() {
			bool isPlatform = true;
			if (this.HideOnDesktop && DeviceHelpers.IsDeviceDesktop()) {
				isPlatform = false;
			}
			if (this.HideOnHTML5 && DeviceHelpers.IsDeviceHTML5()) {
				isPlatform = false;
			}
			if (this.HideOnMobile && DeviceHelpers.IsDeviceMobile()) {
				isPlatform = false;
			}
			foreach (NodePath path in this.p_elementsToHideOnMobile) {
				Control control = this.GetNode<Control>(path);
				control.Visible = isPlatform;
				control.SetProcess(isPlatform);
			}
		}
	}
}

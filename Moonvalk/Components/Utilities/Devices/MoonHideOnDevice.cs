using Godot;
using Moonvalk.Accessory;

namespace Moonvalk.Components
{
    /// <summary>
    /// Component that is responsible for showing or hiding Nodes based on device type.
    /// </summary>
    public class MoonHideOnDevice : Node
    {
        /// <summary>
        /// All nodes to show or hide based on device.
        /// </summary>
        [Export] protected NodePath[] PElementsToHideOnMobile { get; set; }

        /// <summary>
        /// True to hide these elements on mobile.
        /// </summary>
        [Export] public bool HideOnMobile { get; protected set; } = true;

        /// <summary>
        /// True to hide these elements on HTML5 web.
        /// </summary>
        [Export] public bool HideOnHtml5 { get; protected set; }

        /// <summary>
        /// True to hide these elements on desktop.
        /// </summary>
        [Export] public bool HideOnDesktop { get; protected set; }

        /// <summary>
        /// Called when object is first initialized.
        /// </summary>
        public override void _Ready()
        {
            var isPlatform = !(HideOnDesktop && DeviceHelpers.IsDeviceDesktop());

            if (HideOnHtml5 && DeviceHelpers.IsDeviceHtml5()) isPlatform = false;

            if (HideOnMobile && DeviceHelpers.IsDeviceMobile()) isPlatform = false;

            foreach (var path in PElementsToHideOnMobile)
            {
                var control = GetNode<Control>(path);
                control.Visible = isPlatform;
                control.SetProcess(isPlatform);
            }
        }
    }
}
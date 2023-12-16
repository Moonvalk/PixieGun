using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Animation {
	/// <summary>
	/// A grouping of parameters which change how MoonTweens operate.
	/// </summary>
	[RegisteredType(nameof(MoonTweenParams), "", nameof(Resource))]
	public class MoonTweenParams : Resource {
		/// <summary>
		/// The duration in seconds of the Tween.
		/// </summary>
		[Export] public float Duration { get; set; } = 1f;

		/// <summary>
		/// A delay in seconds before the Tween begins its animation.
		/// </summary>
		[Export] public float Delay { get; set; } = 0f;

		/// <summary>
		/// An Easing Type applied to the Tween. This value is used to find a easing method applied to Tween logic.
		/// </summary>
		[Export] public Easing.Types EasingType { get; set; } = Easing.Types.None;

		/// <summary>
		/// The Easing Function applied to the Tween.
		/// </summary>
		public EasingFunction EasingFunction { get; set; } = Easing.Cubic.InOut;
	}
}
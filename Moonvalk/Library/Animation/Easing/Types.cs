
namespace Moonvalk.Animation {
	/// <summary>
	/// Supplies Easing functions that affect how a value traverses from a start to an end point.
	/// </summary>
	public static partial class Easing {
		/// <summary>
		/// All Easing Types available. Types can be parsed into usable EasingFunctions by passing to the <cref="Easing.Functions"/> map.
		/// </summary>
		public enum Types {
			BackIn,
			BackOut,
			BackInOut,
			BounceIn,
			BounceOut,
			BounceInOut,
			CircularIn,
			CircularOut,
			CircularInOut,
			CubicIn,
			CubicOut,
			CubicInOut,
			ElasticIn,
			ElasticOut,
			ElasticInOut,
			ExponentialIn,
			ExponentialOut,
			ExponentialInOut,
			Linear,
			QuadraticIn,
			QuadraticOut,
			QuadraticInOut,
			QuarticIn,
			QuarticOut,
			QuarticInOut,
			QuinticIn,
			QuinticOut,
			QuinticInOut,
			SinusoidalIn,
			SinusoidalOut,
			SinusoidalInOut,
			None,
		}
	}
}

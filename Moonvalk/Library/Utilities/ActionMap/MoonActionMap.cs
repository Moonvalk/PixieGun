using System;

namespace Moonvalk.Utilities {
	/// <summary>
	/// Container representing an action map based on enum / state.
	/// </summary>
	/// <typeparam name="EnumType">An enumerator used for tracking events.</typeparam>
	public class MoonActionMap<EnumType> : BaseMoonActionMap<EnumType, Action> {
		/// <summary>
		/// Runs all actions for the specified event / enum value.
		/// </summary>
		/// <param name="event_">The event / enum value to add new actions for.</param>
		/// <param name="clearAfterRun_">Flag that when true will clear all actions stored for this event
		/// after execution.</param>
		public MoonActionMap<EnumType> Run(
			EnumType event_,
			bool clearAfterRun_ = false
		) {
			for (int index = 0; index < this.Events[event_].Value.Count; index++) {
				this.Events[event_].Value[index]?.Invoke();
			}
			if (clearAfterRun_) {
				this.Clear(event_);
			}
			return this;
		}
	}

	/// <summary>
	/// Container representing an action map based on enum / state that takes a parameter.
	/// </summary>
	/// <typeparam name="EnumType">An enumerator used for tracking events.</typeparam>
	/// <typeparam name="ParamType">The type of parameter actions will require.</typeparam>
	public class MoonActionMap<EnumType, ParamType> : BaseMoonActionMap<EnumType, Action<ParamType>> {
		/// <summary>
		/// Runs all actions for the specified event / enum value.
		/// </summary>
		/// <param name="event_">The event / enum value to add new actions for.</param>
		/// <param name="param_">Value that will be sent to all stored actions as a parameter.</param>
		/// <param name="clearAfterRun_">Flag that when true will clear all actions stored for this event
		/// after execution.</param>
		public MoonActionMap<EnumType, ParamType> Run(
			EnumType event_,
			ParamType param_ = default(ParamType),
			bool clearAfterRun_ = false
		) {
			for (int index = 0; index < this.Events[event_].Value.Count; index++) {
				this.Events[event_].Value[index]?.Invoke(param_);
			}
			if (clearAfterRun_) {
				this.Clear(event_);
			}
			return this;
		}
	}

	/// <summary>
	/// Container representing an action map based on enum / state that takes two parameters.
	/// </summary>
	/// <typeparam name="EnumType">An enumerator used for tracking events.</typeparam>
	/// <typeparam name="ParamType">The first type of parameter actions will require.</typeparam>
	/// <typeparam name="ParamType2">The second type of parameter actions will require.</typeparam>
	public class MoonActionMap<EnumType, ParamType, ParamType2> : BaseMoonActionMap<EnumType, Action<ParamType, ParamType2>> {
		/// <summary>
		/// Runs all actions for the specified event / enum value.
		/// </summary>
		/// <param name="event_">The event / enum value to add new actions for.</param>
		/// <param name="param_">Value that will be sent to all stored actions as a parameter.</param>
		/// <param name="param2_">Value that will be sent to all stored actions as a parameter.</param>
		/// <param name="clearAfterRun_">Flag that when true will clear all actions stored for this event
		/// after execution.</param>
		public MoonActionMap<EnumType, ParamType, ParamType2> Run(
			EnumType event_,
			ParamType param_ = default(ParamType),
			ParamType2 param2_ = default(ParamType2),
			bool clearAfterRun_ = false
		) {
			for (int index = 0; index < this.Events[event_].Value.Count; index++) {
				this.Events[event_].Value[index]?.Invoke(param_, param2_);
			}
			if (clearAfterRun_) {
				this.Clear(event_);
			}
			return this;
		}
	}
}

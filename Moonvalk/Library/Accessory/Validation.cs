using System;

namespace Moonvalk.Accessory
{
	/// <summary>
	/// Static helpers to assist with validating conditions.
	/// </summary>
	public static class Validation
	{
		/// <summary>
		/// Delegate representing a check that returns a boolean value.
		/// </summary>
		/// <returns>Returns true if the check has passed.</returns>
		public delegate bool Check();

		/// <summary>
		/// Appends a conditional validation check to the provided action.
		/// </summary>
		/// <param name="validationCheck_">The conditional check that must be made.</param>
		/// <param name="action_">The action that will be returned if the condition returns true.</param>
		/// <returns>Returns a new action that validates a conditional before running the provided action.</returns>
		public static Action IfTrue(Check validationCheck_, Action action_)
		{
			return () =>
			{
				if (validationCheck_.Invoke()) {
					action_.Invoke();
				}
			};
		}
	}
}

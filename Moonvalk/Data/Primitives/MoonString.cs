using Godot;
using Moonvalk.Resources;

namespace Moonvalk.Data {
	/// <summary>
	/// Container used for pairing a title with string data.
	/// </summary>
	[RegisteredType(nameof(MoonString), "", nameof(Resource))]
	public class MoonString : MoonValue<string> {
		/// <summary>
		/// Validates and returns the resource path if it exists. If not found an error will be thrown.
		/// </summary>
		public string AsPath {
			get {
				if (!ResourceLoader.Exists(this.Value)) {
					GD.Print("LOAD ERROR - Resource " + this.Name + ":" + this.ResourceName + " not found at path " + this.Value);
					return null;
				}
				return this.Value;
			}
		}
	}
}

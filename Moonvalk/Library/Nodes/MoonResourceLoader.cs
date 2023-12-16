// #define __DEBUG

using System;
using Godot;
using Moonvalk.Accessory;
using Moonvalk.Utilities;

namespace Moonvalk.Resources {
	/// <summary>
	/// Helper for loading resources. This manages polling for status as Godot loads new items and
	/// provides a callback for utilizing resources when they are complete.
	/// </summary>
	public static class MoonResourceLoader {
		/// <summary>
		/// The default poll rate in seconds. This is how often finalization checks will occur when resources are
		/// actively being loaded.
		/// </summary>
		private const float _defaultPollRate = 0.05f;

		/// <summary>
		/// Attempts to load a resource at the provided file path. Once loading is completed the provided callback will be
		/// invoked with the new resource as a parameter.
		/// </summary>
		/// <typeparam name="ResourceType">The type of resource to be loaded.</typeparam>
		/// <param name="path_">The path within the file system where this resource is located.</param>
		/// <param name="onLoad_">A callback to be executed once a successful load is complete.</param>
		/// <param name="pollRate_">The rate at which in seconds polling will be done.</param>
		/// <param name="initialPollDelay_">An initial polling delay in seconds.</param>
		public static void Load<ResourceType>(
			string path_,
			Action<ResourceType> onLoad_ = null,
			float? pollRate_ = null,
			float? initialPollDelay_ = null
		) where ResourceType : Resource {
			Action load = () => {
				ResourceInteractiveLoader loader = ResourceLoader.LoadInteractive(path_);
				#if (__DEBUG)
					GD.Print("Started loading resource at path: " + path_);
				#endif

				MoonResourceLoader.pollLoader(loader, onLoad_, pollRate_ ?? _defaultPollRate, initialPollDelay_ ?? 0f);
			};
			if (DeviceHelpers.IsDeviceHTML5()) {
				load();
				return;
			}
			System.Threading.Thread thread = new System.Threading.Thread(() => {
				load();
			});
			thread.Start();
		}

		/// <summary>
		/// Called to poll an active resource loader after a duration of time has passed. This will recursively
		/// call itself until a successful load is complete.
		/// </summary>
		/// <typeparam name="ResourceType">The type of resource to be loaded.</typeparam>
		/// <param name="loader_">The loader being used to pull new resources.</param>
		/// <param name="onLoad_">A callback to be executed once a successful load is complete.</param>
		/// <param name="pollRate_">The adjusted rate at which in seconds polling will be done following an initial delay.</param>
		/// <param name="pollDelay_">A delay in seconds before the next poll will occur.</param>
		private static void pollLoader<ResourceType>(
			ResourceInteractiveLoader loader_,
			Action<ResourceType> onLoad_,
			float pollRate_,
			float pollDelay_
		) where ResourceType : Resource {
			MoonTimer.Wait(pollDelay_, () => {
				#if (__DEBUG)
					GD.Print("Polling load for status...");
				#endif
				Error error = loader_.Poll();
				if (error == Error.FileEof) {
					ResourceType resource = (ResourceType)loader_.GetResource();
					loader_.Dispose();
					#if (__DEBUG)
						GD.Print("Resource loaded: " + resource);
					#endif
					onLoad_?.Invoke(resource);
				} else if (error == Error.Ok) {
					MoonResourceLoader.pollLoader(loader_, onLoad_, pollRate_, pollRate_);
				} else {
					GD.Print("Resource load failure: " + error);
				}
			});
		}
	}
}

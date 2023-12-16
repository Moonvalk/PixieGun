#if (TOOLS)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Moonvalk.Resources
{
	/// <summary>
	/// This is a plugin for registering custom C# Resources within the Godot Registry.
	/// </summary>
	[Tool]
	public class ResourceRegistryPlugin : EditorPlugin
	{
		#region Data Fields
		/// <summary>
		/// A list of all custom types.
		/// </summary>
		private readonly List<string> _customTypes = new List<string>();

		/// <summary>
		/// Stores reference to the refresh button within the Godot editor to load registered types.
		/// </summary>
		public Button RefreshButton { get; protected set; }
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this plugin enters the main tree (on tool load).
		/// </summary>
		public override void _EnterTree() {
			// Initialize a new refresh button and slot it in the toolbar container.
			RefreshButton = new Button();
			RefreshButton.Text = "Build Resources";

			AddControlToContainer(CustomControlContainer.Toolbar, RefreshButton);
			RefreshButton.Icon = RefreshButton.GetIcon("Reload", "EditorIcons");
			RefreshButton.Connect("pressed", this, nameof(onRefreshPressed));

			ResourceRegistrySettings.Init();
			RefreshCustomClasses();
			GD.PushWarning("You may change any setting for the C# Registry Plugin in Project -> ProjectSettings -> General -> ResourceRegistryPlugin");
		}

		/// <summary>
		/// Called when this plugin exits the main tree (on tool unload).
		/// </summary>
		public override void _ExitTree() {
			unregisterCustomClasses();
			RemoveControlFromContainer(CustomControlContainer.Toolbar, RefreshButton);
			RefreshButton.QueueFree();
		}
		#endregion

		/// <summary>
		/// Called to refresh registered classes on user request.
		/// </summary>
		public void RefreshCustomClasses()
		{
			GD.Print("\nRefreshing Registered Resources...");
			unregisterCustomClasses();
			registerCustomClasses();
		}

		/// <summary>
		/// Called to register all custom types.
		/// </summary>
		protected void registerCustomClasses()
		{
			this._customTypes.Clear();

			File file = new File();
			foreach (Type type in getCustomRegisteredTypes())
			{
				if (type.IsSubclassOf(typeof(Resource)))
				{
					addRegisteredType(type, nameof(Resource), file);
				}
				else
				{
					addRegisteredType(type, nameof(Node), file);
				}
			}	
		}
		
		/// <summary>
		/// Called to add a new registered type to the Godot registry.
		/// </summary>
		/// <param name="type_">The type being added.</param>
		/// <param name="defaultBaseTypeName_">The default inherited type in the case none is provided.</param>
		/// <param name="file_">The file being written.</param>
		protected void addRegisteredType(Type type_, string defaultBaseTypeName_, File file_)
		{
			RegisteredTypeAttribute attribute = (RegisteredTypeAttribute)Attribute.GetCustomAttribute(type_, typeof(RegisteredTypeAttribute));
			String path = findClassPath(type_);
			if (path == null && !file_.FileExists(path)) return;

			Script script = GD.Load<Script>(path);
			if (script == null) return;

			string baseTypeName = (attribute.BaseType == "" ? defaultBaseTypeName_ : attribute.BaseType);
			ImageTexture icon = null;
			string iconPath = attribute.IconPath;
			
			if (iconPath == "")
			{
				Type baseType = type_.BaseType;
				while (baseType != null)
				{
					RegisteredTypeAttribute baseTypeAttribute = (RegisteredTypeAttribute)Attribute.GetCustomAttribute(baseType, typeof(RegisteredTypeAttribute));
					if (baseTypeAttribute != null && baseTypeAttribute.IconPath != "")
					{
						iconPath = baseTypeAttribute.IconPath;
						break;
					}
					
					baseType = baseType.BaseType;
				}
			}
			
			if (iconPath != "")
			{
				if (file_.FileExists(iconPath))
				{
					Texture rawIcon = ResourceLoader.Load<Texture>(iconPath);
					if (rawIcon != null)
					{
						Image image = rawIcon.GetData();
						int length = (int) Mathf.Round(16 * GetEditorInterface().GetEditorScale());
						image.Resize(length, length);
						icon = new ImageTexture();
						icon.CreateFromImage(image);
					}
					else
					{
						GD.PushError($"Could not load the icon for the registered type \"{type_.FullName}\" at path \"{path}\".");
					}
				}
				else
				{
					GD.PushError($"The icon path of \"{path}\" for the registered type \"{type_.FullName}\" does not exist.");
				}
			}

			AddCustomType($"{ResourceRegistrySettings.ClassPrefix}{type_.Name}", baseTypeName, script, icon);
			this._customTypes.Add($"{ResourceRegistrySettings.ClassPrefix}{type_.Name}");
			GD.Print($"Registered custom type: {type_.Name} -> {path}");
		}

		/// <summary>
		/// Finds a matching path for the requested type.
		/// </summary>
		/// <param name="type_">The type to find.</param>
		/// <returns>Returns a string matching the path of the requested type.</returns>
		protected static string findClassPath(Type type_)
		{
			switch (ResourceRegistrySettings.SearchType)
			{
				case ResourceRegistrySettings.ResourceSearchType.Recursive:
					return findClassPathRecursive(type_);
				case ResourceRegistrySettings.ResourceSearchType.Namespace:
					return findClassPathNamespace(type_);
				default:
					GD.PushError($"ResourceSearchType {ResourceRegistrySettings.SearchType} not implemented!");
					return "";
			}
		}

		/// <summary>
		/// Finds the matching class path by searching namespaces.
		/// </summary>
		/// <param name="type_">The type to find a path for.</param>
		/// <returns>Returns the matching path for the type.</returns>
		protected static string findClassPathNamespace(Type type_)
		{
			foreach (string dir in ResourceRegistrySettings.ResourceScriptDirectories)
			{
				string filePath = $"{dir}/{type_.Namespace?.Replace(".", "/") ?? ""}/{type_.Name}.cs";
				File file = new File();
				if (file.FileExists(filePath)) return filePath;
			}
			
			return null;
		}

		/// <summary>
		/// Finds the matching class path by searching recursively for type name.
		/// </summary>
		/// <param name="type_">The type to find a path for.</param>
		/// <returns>Returns the matching path for the type.</returns>
		protected static string findClassPathRecursive(Type type_)
		{
			foreach (string directory in ResourceRegistrySettings.ResourceScriptDirectories)
			{
				string fileFound = findClassPathRecursiveHelper(type_, directory);
				if (fileFound != null) return fileFound;
			}
			
			return null;
		}

		/// <summary>
		/// Helper method called to recursively search for type paths.
		/// </summary>
		/// <param name="type_">The type to find a path for.</param>
		/// <param name="directory_">The directory to search.</param>
		/// <returns>Returns the matching path, when found.</returns>
		protected static string findClassPathRecursiveHelper(Type type_, string directory_)
		{
			Directory dir = new Directory();

			if (dir.Open(directory_) == Error.Ok)
			{
				dir.ListDirBegin();
				while (true)
				{
					var fileOrDirName = dir.GetNext();
					
					// Skips hidden files like .
					if (fileOrDirName == "")
					{
						break;
					}
					else if (fileOrDirName.BeginsWith("."))
					{
						continue;
					}
					else if (dir.CurrentIsDir())
					{
						string foundFilePath = findClassPathRecursiveHelper(type_, dir.GetCurrentDir() + "/" + fileOrDirName);
						
						if (foundFilePath != null)
						{
							dir.ListDirEnd();
							return foundFilePath;
						}
					}
					else if (fileOrDirName == $"{type_.Name}.cs")
					{
						return dir.GetCurrentDir() + "/" + fileOrDirName;
					}
				}
			}
			
			return null;
		}

		/// <summary>
		/// Gets all valid custom registered types.
		/// </summary>
		/// <returns>All valid custom registered types.</returns>
		protected static IEnumerable<Type> getCustomRegisteredTypes()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(ResourceRegistryPlugin));
			
			return assembly.GetTypes()
				.Where(t => !t.IsAbstract 
					&& Attribute.IsDefined(t, typeof(RegisteredTypeAttribute)) 
					&& (t.IsSubclassOf(typeof(Node)) || t.IsSubclassOf(typeof(Resource)))
				);
		}

		/// <summary>
		/// Called to unregister all custom Resource types from the Godot registry.
		/// </summary>
		protected void unregisterCustomClasses()
		{
			foreach (string script in this._customTypes)
			{
				RemoveCustomType(script);
				GD.Print($"Unregister custom resource: {script}");
			}
			this._customTypes.Clear();
		}

		/// <summary>
		/// Called when the refresh button is pressed.
		/// </summary>
		protected void onRefreshPressed()
		{
			RefreshCustomClasses();
		}
	}
}
#endif
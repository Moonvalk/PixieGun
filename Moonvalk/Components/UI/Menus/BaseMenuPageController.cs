using System;
using System.Collections.Generic;
using Godot;
using Moonvalk.Animation;
using Moonvalk.UI;
using Moonvalk.Utilities;

namespace Moonvalk.Components.UI
{
	/// <summary>
	/// Base class for a menu page controller. This handles all elements displayed on any
	/// particular page found within a menu scene.
	/// </summary>
	public class BaseMenuPageController : Control
	{
		#region Data Fields
		/// <summary>
		/// Paths to all page elements that will be animated on/off the screen.
		/// </summary>
		[Export] protected NodePath[] p_elements { get; set; }

		/// <summary>
		/// Delay between each element as they are introduced.
		/// </summary>
		[Export] public float ElementIntroDelay { get; protected set; } = 0.05f;

		/// <summary>
		/// Reference to all button objects found on this page.
		/// </summary>
		public List<MoonButton> Buttons { get; protected set; }
		
		/// <summary>
		/// Reference to all elements found on this page.
		/// </summary>
		public Control[] Elements { get; protected set; }

		/// <summary>
		/// Flag that is true when the page is actively displayed.
		/// </summary>
		public bool IsPageDisplayed { get; protected set; }

		/// <summary>
		/// Stores index of the last requested page to ensure events are only emitted once.
		/// </summary>
		public int RequestedPage { get; protected set; } = -1;

		/// <summary>
		/// Event emitted when a button object is focused.
		/// </summary>
		[Signal] public delegate void OnButtonFocus();

		/// <summary>
		/// Event emitted when a button object is pressed.
		/// </summary>
		[Signal] public delegate void OnButtonPress();

		/// <summary>
		/// Event emitted when an element is animated on/off screen.
		/// </summary>
		[Signal] public delegate void OnAnimateElement();

		/// <summary>
		/// Event emitted when a request has been made to display a new page.
		/// </summary>
		/// <param name="index_">The page index that was requested.</param>
		[Signal] public delegate void OnDisplayPage(int index_);

		/// <summary>
		/// Event emitted when a request has been made to close the menu.
		/// </summary>
		[Signal] public delegate void OnExitMenu();
		#endregion

		#region Godot Events
		/// <summary>
		/// Called when this object is first initialized.
		/// </summary>
		public override void _Ready()
		{
			Buttons = this.GetAllComponents<MoonButton>(typeof(BaseRootMenu));
			for (int index = 0; index < Buttons.Count; index++)
			{
				Buttons[index].Connect(nameof(MoonButton.OnFocusEnter), this, nameof(handleButtonFocus));
				Buttons[index].Connect("pressed", this, nameof(handleButtonPress));
			}

			Elements = new Control[p_elements.Length];
			for (int index = 0; index < Elements.Length; index++)
			{
				Elements[index] = GetNode<Control>(p_elements[index]);
				Elements[index].CenterPivot();
				Elements[index].Visible = false;
				Elements[index].RectScale = Vector2.Zero;
			}
			
			this.Wait(0.1f, () =>
			{
				displayElements(true);
			});
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Called to emit a request to show a different page.
		/// </summary>
		/// <param name="index_">The page index to request.</param>
		/// <returns>Returns true if the page request is unique, false if the request was already made.</returns>
		public bool DisplayPage(int index_)
		{
			if (RequestedPage == index_) return false;

			RequestedPage = index_;
			EmitSignal(nameof(OnDisplayPage), index_);
			return true;
		}

		/// <summary>
		/// Called to emit a request to close the root menu.
		/// </summary>
		public void ExitMenu()
		{
			EmitSignal(nameof(OnExitMenu));
		}

		/// <summary>
		/// Called to hide this page by animating elements away.
		/// </summary>
		/// <param name="onComplete_">Action to be executed when the animation is complete.</param>
		public virtual void HidePage(Action onComplete_)
		{
			displayElements(false, onComplete_);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Handles emitting a button focus event.
		/// </summary>
		protected void handleButtonFocus()
		{
			EmitSignal(nameof(OnButtonFocus));
		}

		/// <summary>
		/// Handles emitting a button press event.
		/// </summary>
		protected void handleButtonPress()
		{
			EmitSignal(nameof(OnButtonPress));
		}

		/// <summary>
		/// Called to animate elements on or off of the page.
		/// </summary>
		/// <param name="flag_">True when elements should be introduced, false to hide.</param>
		/// <param name="onComplete_">An optional action to be called when the animation is done.</param>
		protected void displayElements(bool flag_, Action onComplete_ = null)
		{
			EasingFunction easing = (flag_ ? (EasingFunction)Easing.Back.Out : Easing.Cubic.InOut);
			float duration = (flag_ ? 0.75f : 0.25f);
			Vector2 size = (flag_ ? Vector2.One : Vector2.Zero);
			
			float delay = 0.5f;
			for (int index = 0; index < Elements.Length; index++)
			{
				int currentIndex = index;
				if (flag_) delay += currentIndex * ElementIntroDelay;

				MoonTweenVec2 animation = Elements[currentIndex].ScaleTo(size, new MoonTweenParams
				{
					Duration = duration, EasingFunction = easing, Delay = delay,
				}, false);
				
				if (currentIndex == Elements.Length - 1)
				{
					IsPageDisplayed = flag_;
					animation.OnComplete(onComplete_);
				}
				
				animation.OnStart(() => 
				{
					if (flag_) IsPageDisplayed = flag_;

					Elements[currentIndex].Visible = true;
					EmitSignal(nameof(OnAnimateElement));
				}).Start();
			}
		}
		#endregion
	}
}

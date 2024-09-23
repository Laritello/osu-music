using Osu.Music.Common.Enums;
using Osu.Music.Services.Hotkeys;
using System.ComponentModel;

namespace Osu.Music.Services.Events
{
	public class GlobalKeyboardHookEventArgs(GlobalKeyboardHook.LowLevelKeyboardInputEvent keyboardData, KeyboardState keyboardState) : HandledEventArgs
	{
		public KeyboardState KeyboardState { get; } = keyboardState;
		public GlobalKeyboardHook.LowLevelKeyboardInputEvent KeyboardData { get; } = keyboardData;
	}
}

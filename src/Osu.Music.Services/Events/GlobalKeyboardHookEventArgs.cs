using Osu.Music.Common.Enums;
using Osu.Music.Services.Hotkeys;
using System.ComponentModel;

namespace Osu.Music.Services.Events
{
    public class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        public KeyboardState KeyboardState { get; private set; }
        public GlobalKeyboardHook.LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        public GlobalKeyboardHookEventArgs(GlobalKeyboardHook.LowLevelKeyboardInputEvent keyboardData, KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}

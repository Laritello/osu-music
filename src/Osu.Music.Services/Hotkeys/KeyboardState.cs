using System;
using System.Collections.Generic;
using System.Text;

namespace Osu.Music.Services.Hotkeys
{
    public enum KeyboardState
    {
        KeyDown = 0x0100,
        KeyUp = 0x0101,
        SysKeyDown = 0x0104,
        SysKeyUp = 0x0105
    }
}

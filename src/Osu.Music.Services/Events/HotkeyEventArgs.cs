using Osu.Music.Common.Enums;

namespace Osu.Music.Services.Events
{
	public class HotkeyEventArgs(HotkeyType type)
	{
		public HotkeyType Type { get; } = type;
	}
}

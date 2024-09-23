using Osu.Music.Common.Models;
using System;

namespace Osu.Music.Services.Events
{
	public class BeatmapEventArgs(Beatmap beatmap) : EventArgs
	{
		public Beatmap Beatmap { get; } = beatmap;
	}
}

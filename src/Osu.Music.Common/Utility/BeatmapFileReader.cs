using osu_database_reader;
using osu_database_reader.Components.Events;
using osu_database_reader.TextFiles;
using System;
using System.Collections.Generic;
using System.IO;

namespace Osu.Music.Common.Utility
{
    public static class BeatmapFileReader
    {
        public static BeatmapFile Read(Stream stream)
        {
            var file = new BeatmapFile();

            using var r = new StreamReader(stream);
            if (!int.TryParse(r.ReadLine()?.Replace("osu file format v", string.Empty), out file.FileFormatVersion))
                throw new Exception("Not a valid beatmap"); //very simple check, could be better

            BeatmapSection? bs;
            while ((bs = r.ReadUntilSectionStart()) != null)
            {
                switch (bs.Value)
                {
                    case BeatmapSection.Events:
                        file.Events.AddRange(r.ReadEvents());
                        break;
                    default:
                        r.ReadUntilSectionStart();
                        break;
                }
            }

            return file;
        }

        private static BeatmapSection? ReadUntilSectionStart(this StreamReader sr)
        {
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(str)) continue;

                string stripped = str.TrimStart('[').TrimEnd(']');
                if (!Enum.TryParse(stripped, out BeatmapSection a))
                    continue;
                return a;
            }

            //we reached an end of stream
            return null;
        }

        private static IEnumerable<EventBase> ReadEvents(this StreamReader sr)
        {
            string line;
            while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                if (!line.StartsWith("//"))
                    yield return EventBase.FromString(line);
        }
    }
}

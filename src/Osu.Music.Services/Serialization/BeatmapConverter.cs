using Osu.Music.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Osu.Music.Services.Serialization
{
    public static class BeatmapConverter
    {
        public static Beatmap Deserialize(string path)
        {
            Beatmap beatmap = new Beatmap();

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (line.Equals("[TimingPoints]"))
                    {
                        break;
                    }
                    else if (!line.Contains(":"))
                    {
                        continue;
                    }
                    else
                    {
                        string[] fields = line.Split(":");
                        SetProperty(beatmap, fields[0], fields[1]);
                    }
                }
            }

            return beatmap;
        }

        private static void SetProperty(Beatmap beatmap, string propertyName, string propertyValue)
        {
            if (propertyValue == null)
                return;

            // In some cases after separator ':' property contains one space before it's value
            // which brakes logic for audio path etc.
            if (propertyValue.Length > 0)
                propertyValue = propertyValue[0].Equals(' ') ? propertyValue.Substring(1) : propertyValue;

            var property = beatmap.GetType().GetProperty(propertyName);
            
            if (property != null)
            {
                switch (Type.GetTypeCode(property.PropertyType))
                {
                    case TypeCode.Int32:
                        property.SetValue(beatmap, int.Parse(propertyValue));
                        break;
                    case TypeCode.String:
                        property.SetValue(beatmap, propertyValue);
                        break;
                }
            }
            
        }
    }
}

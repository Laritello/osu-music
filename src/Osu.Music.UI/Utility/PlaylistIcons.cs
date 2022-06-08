using MaterialDesignThemes.Wpf;
using System.Collections.Generic;

namespace Osu.Music.UI.Utility
{
    public static class PlaylistIcons
    {
        public static List<PackIconKind> GetIcons()
        {
            return new List<PackIconKind>()
            {
                PackIconKind.PlaylistMusic,
                PackIconKind.NightSky,
                PackIconKind.ZodiacAquarius,
                PackIconKind.ZodiacAries,
                PackIconKind.ZodiacCancer,
                PackIconKind.ZodiacCapricorn,
                PackIconKind.ZodiacGemini,
                PackIconKind.ZodiacLeo,
                PackIconKind.ZodiacLibra,
                PackIconKind.ZodiacPisces,
                PackIconKind.ZodiacSagittarius,
                PackIconKind.ZodiacScorpio,
                PackIconKind.ZodiacTaurus,
                PackIconKind.ZodiacVirgo,
            };
        }
    }
}

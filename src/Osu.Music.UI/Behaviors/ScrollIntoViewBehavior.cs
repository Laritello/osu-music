using Osu.Music.Common.Models;
using System.Windows;
using System.Windows.Controls;

namespace Osu.Music.UI.Behaviors
{
    public class ScrollBeatmapIntoViewBehavior
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached(
            "Target",
            typeof(Beatmap),
            typeof(ScrollBeatmapIntoViewBehavior),
            new UIPropertyMetadata(new PropertyChangedCallback(TargetChanged)));

        public static Beatmap GetTarget(DependencyObject obj)
        {
            return (Beatmap)obj.GetValue(TargetProperty);
        }

        public static void SetTarget(DependencyObject obj, Beatmap value)
        {
            obj.SetValue(TargetProperty, value);
        }

        private static void TargetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target == null || e.NewValue == null)
                return;

            var listView = (ListView)target;
            var item = (Beatmap)e.NewValue;

            listView.ScrollIntoView(item);
            listView.SelectedItem = item;
        }
    }
}

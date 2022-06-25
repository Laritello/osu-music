using Osu.Music.Services.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Osu.Music.UI.UserControls
{
    public partial class BeatmapProgressBar : UserControl
    {
        public static readonly DependencyProperty TotalProperty = DependencyProperty.Register("Total", typeof(TimeSpan), typeof(BeatmapProgressBar), new PropertyMetadata(TimeSpan.Zero));
        public TimeSpan Total
        {
            get => (TimeSpan)GetValue(TotalProperty);
            set => SetValue(TotalProperty, value);
        }

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(TimeSpan), typeof(BeatmapProgressBar), new PropertyMetadata(TimeSpan.Zero));
        public TimeSpan Current
        {
            get => (TimeSpan)GetValue(CurrentProperty);
            set => SetValue(CurrentProperty, value);
        }

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(BeatmapProgressBar), new PropertyMetadata(0.0));
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public static readonly RoutedEvent ProgressChangedEvent = EventManager.RegisterRoutedEvent("ProgressChanged", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(BeatmapProgressBar));

        public event RoutedEventHandler ProgressChanged
        {
            add { AddHandler(ProgressChangedEvent, value); }
            remove { RemoveHandler(ProgressChangedEvent, value); }
        }

        public BeatmapProgressBar()
        {
            InitializeComponent();
        }

        private void SliderArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double max = ActualWidth;
            double current = e.GetPosition(this).X;

            double progress = Math.Max(Math.Min(current / max, 1), 0);

            TimeSpan result = new TimeSpan(0, 0, Convert.ToInt32(Total.TotalSeconds * progress));

            BeatmapProgressBarProgressChangedEventArgs eventArgs = new BeatmapProgressBarProgressChangedEventArgs(ProgressChangedEvent, result);
            RaiseEvent(eventArgs);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Osu.Music.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для BeatmapProgressBar.xaml
    /// </summary>
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

        public BeatmapProgressBar()
        {
            InitializeComponent();
        }
    }
}

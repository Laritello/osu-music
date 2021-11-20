using Osu.Music.Services.Hotkeys;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для HotkeyListItem.xaml
    /// </summary>
    public partial class HotkeyListItem : UserControl
    {
        public static readonly DependencyProperty InEditProperty = DependencyProperty.Register("InEdit", typeof(bool), typeof(HotkeyListItem), new PropertyMetadata(false));
        public bool InEdit
        {
            get => (bool)GetValue(InEditProperty);
            set => SetValue(InEditProperty, value);
        }

        public static readonly DependencyProperty SelectedHotkeyProperty = DependencyProperty.Register("SelectedHotkey", typeof(HotkeyListItem), typeof(HotkeyListItem), new PropertyMetadata(null));
        public HotkeyListItem SelectedHotkey
        {
            get => (HotkeyListItem)GetValue(SelectedHotkeyProperty);
            set => SetValue(SelectedHotkeyProperty, value);
        }

        public HotkeyListItem()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return LabelName.Text;
        }
    }
}

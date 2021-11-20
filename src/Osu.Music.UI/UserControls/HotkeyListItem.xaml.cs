using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using System.Windows;
using System.Windows.Controls;

namespace Osu.Music.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для HotkeyListItem.xaml
    /// </summary>
    public partial class HotkeyListItem : UserControl
    {
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(HotkeyType), typeof(HotkeyListItem), new PropertyMetadata(null));
        public HotkeyType Type
        {
            get => (HotkeyType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty CombinationProperty = DependencyProperty.Register("Combination", typeof(KeyCombination), typeof(HotkeyListItem), new PropertyMetadata(null));
        public KeyCombination Combination
        {
            get => (KeyCombination)GetValue(CombinationProperty);
            set => SetValue(CombinationProperty, value);
        }

        public static readonly DependencyProperty InEditProperty = DependencyProperty.Register("InEdit", typeof(bool), typeof(HotkeyListItem), new PropertyMetadata(false));
        public bool InEdit
        {
            get => (bool)GetValue(InEditProperty);
            set => SetValue(InEditProperty, value);
        }

        public static readonly DependencyProperty SelectedHotkeyProperty = DependencyProperty.Register("SelectedHotkey", typeof(HotkeyType?), typeof(HotkeyListItem), new PropertyMetadata(null));
        public HotkeyType? SelectedHotkey
        {
            get => (HotkeyType?)GetValue(SelectedHotkeyProperty);
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

using Osu.Music.Services.Updates;
using Osu.Music.ViewModels;
using Prism.Commands;
using Squirrel;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Osu.Music.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DelegateCommand MinimizeCommand { get; private set; }
        public DelegateCommand<Button> MaximizeOrRestoreCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            MinimizeCommand = new DelegateCommand(Minimize);
            MaximizeOrRestoreCommand = new DelegateCommand<Button>(MaximizeOrRestore);
            CloseCommand = new DelegateCommand(Close);
        }

        private void Minimize() => WindowState = WindowState.Minimized;

        private void MaximizeOrRestore(Button maximize)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            maximize.Visibility = WindowState == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
        }

#if (!DEBUG)
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
			try
			{
				var dc = (MainWindowViewModel)DataContext;
				dc.Updater = new GitHubUpdater();

				if (dc != null)
				{
					var manager = await UpdateManager.GitHubUpdateManager("https://github.com/Laritello/osu-music");
					dc.Updater.Manager = manager;
					dc.Updater.CheckForUpdates();
				}
			}
			catch
			{
				// Ignore updater fail
			}
        }
#else
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
#endif

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
        }

        public static IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_GETMINMAXINFO)
            {
                // We need to tell the system what our size should be when maximized. Otherwise it will cover the whole screen,
                // including the task bar.
                MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

                // Adjust the maximized size and position to fit the work area of the correct monitor
                IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    MONITORINFO monitorInfo = new MONITORINFO
                    {
                        cbSize = Marshal.SizeOf(typeof(MONITORINFO))
                    };
                    GetMonitorInfo(monitor, ref monitorInfo);

                    RECT rcWorkArea = monitorInfo.rcWork;
                    RECT rcMonitorArea = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                }

                Marshal.StructureToPtr(mmi, lParam, true);
            }

            return IntPtr.Zero;
        }

        private const int WM_GETMINMAXINFO = 0x0024;

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
    }
}

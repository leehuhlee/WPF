using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Dialog
{
    public enum WindowDockPosition
    {
        Undocked = 0,
        Left = 1,
        Right = 2,
        TopBottom = 3,
        TopLeft = 4,
        TopRight = 5,
        BottomLeft = 6,
        BottomRight = 7,
    }


    public class WindowResizer
    {
        #region Private Members
        private Window mWindow;

        private Rect mScreenSize = new Rect();

        private int mEdgeTolerance = 8;

        private DpiScale? mMonitorDpi;

        private IntPtr mLastScreen;

        private WindowDockPosition mLastDock = WindowDockPosition.Undocked;
        #endregion

        #region DLL Imports
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);
        #endregion

        #region Public Events
        public event Action<WindowDockPosition> WindowDockChanged = (dock) => { };
        #endregion

        #region Public Properties
        public Rectangle CurrentMonitorSize { get; set; } = new Rectangle();
        #endregion

        #region Constructor
        public WindowResizer(Window window)
        {
            mWindow = window;

            // Listen out for source initialized to setup
            mWindow.SourceInitialized += Window_SourceInitialized;

            // Monitor for edge docking
            mWindow.SizeChanged += Window_SizeChanged;
            mWindow.LocationChanged += Window_LocationChanged;
        }
        #endregion

        #region Initialize
        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            // Get the handle of this window
            var handle = (new WindowInteropHelper(mWindow)).Handle;
            var handleSource = HwndSource.FromHwnd(handle);

            // If not found, end
            if (handleSource == null)
                return;

            // Hook into it's Windows messages
            handleSource.AddHook(WindowProc);
        }
        #endregion

        #region Edge Docking
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Window_SizeChanged(null, null);
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Cannot calculate size until we know monitor scale
            if (mMonitorDpi == null)
                return;

            // Get window rectangle
            var top = mWindow.Top;
            var left = mWindow.Left;
            var bottom = top + mWindow.Height;
            var right = left + mWindow.Width;

            // Get window position/size in device pixels
            var windowTopLeft = new Point(left * mMonitorDpi.Value.DpiScaleX, top * mMonitorDpi.Value.DpiScaleX);
            var windowBottomRight = new Point(right * mMonitorDpi.Value.DpiScaleX, bottom * mMonitorDpi.Value.DpiScaleX);

            // Check for edges docked
            var edgedTop = windowTopLeft.Y <= (mScreenSize.Top + mEdgeTolerance) && windowTopLeft.Y >= (mScreenSize.Top - mEdgeTolerance);
            var edgedLeft = windowTopLeft.X <= (mScreenSize.Left + mEdgeTolerance) && windowTopLeft.X >= (mScreenSize.Left - mEdgeTolerance);
            var edgedBottom = windowBottomRight.Y >= (mScreenSize.Bottom - mEdgeTolerance) && windowBottomRight.Y <= (mScreenSize.Bottom + mEdgeTolerance);
            var edgedRight = windowBottomRight.X >= (mScreenSize.Right - mEdgeTolerance) && windowBottomRight.X <= (mScreenSize.Right + mEdgeTolerance);

            // Get docked position
            var dock = WindowDockPosition.Undocked;

            // Left docking
            if (edgedTop && edgedBottom && edgedLeft)
                dock = WindowDockPosition.Left;
            // Right docking
            else if (edgedTop && edgedBottom && edgedRight)
                dock = WindowDockPosition.Right;
            // Top/bottom
            else if (edgedTop && edgedBottom)
                dock = WindowDockPosition.TopBottom;
            // Top-left
            else if (edgedTop && edgedLeft)
                dock = WindowDockPosition.TopLeft;
            // Top-right
            else if (edgedTop && edgedRight)
                dock = WindowDockPosition.TopRight;
            // Bottom-left
            else if (edgedBottom && edgedLeft)
                dock = WindowDockPosition.BottomLeft;
            // Bottom-right
            else if (edgedBottom && edgedRight)
                dock = WindowDockPosition.BottomRight;

            // None
            else
                dock = WindowDockPosition.Undocked;

            // If dock has changed
            if (dock != mLastDock)
                // Inform listeners
                WindowDockChanged(dock);

            // Save last dock position
            mLastDock = dock;
        }
        #endregion

        #region Windows Message Pump
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                // Handle the GetMinMaxInfo of the Window
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }
        #endregion

        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            // Get the point position to determine what screen we are on
            GetCursorPos(out POINT lMousePosition);

            // Now get the current screen
            var lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            var lPrimaryScreen = MonitorFromPoint(new POINT(0,0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

            // Try and get the current screen information
            var lCurrentScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lCurrentScreen, lCurrentScreenInfo) == false)
                return;

            // Try and get the primary screen information
            var lPrimaryScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
                return;

            // If this has changed from the last one, update the transform
            if (lCurrentScreen != mLastScreen || mMonitorDpi == null)
                mMonitorDpi = VisualTreeHelper.GetDpi(mWindow);

            // Store last know screen
            mLastScreen = lCurrentScreen;

            // Get work area sizes and rations
            var currentX = lCurrentScreenInfo.rcWork.Left - lCurrentScreenInfo.rcMonitor.Left;
            var currentY = lCurrentScreenInfo.rcWork.Top - lCurrentScreenInfo.rcMonitor.Top;
            var currentWidth = (lCurrentScreenInfo.rcWork.Right - lCurrentScreenInfo.rcWork.Left);
            var currentHeight = (lCurrentScreenInfo.rcWork.Bottom - lCurrentScreenInfo.rcWork.Top);
            var currentRatio = (float)currentWidth / (float)currentHeight;

            var primaryX = lPrimaryScreenInfo.rcWork.Left - lPrimaryScreenInfo.rcMonitor.Left;
            var primaryY = lPrimaryScreenInfo.rcWork.Top - lPrimaryScreenInfo.rcMonitor.Top;
            var primaryWidth = (lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left);
            var primaryHeight = (lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top);
            var primaryRatio = (float)primaryWidth / (float)primaryHeight;

            // Get min/max structure to fill with information
            var lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // NOTE: rcMonitor is the monitor size
            //       rcWork is the available screen size (so the area inside the taskbar start menu for example)

            // Size size limits (used by Windows when maximized)
            // relative to 0,0 being the current screens top-left corner
            //
            //  - Position
            lMmi.ptMaxPosition.X = currentX;
            lMmi.ptMaxPosition.Y = currentY;
            //
            // - Size
            lMmi.ptMaxSize.X = currentWidth;
            lMmi.ptMaxSize.Y = currentHeight;

            //
            // BUG: 
            // NOTE: I've noticed a bug which I think is Windows itself
            //       If your non-primary monitor has a greater width than your primary
            //       (or possibly due to the screen ratio's being different)
            //       then setting the max X on the monitor to the correct value causes
            //       it to scale wrong. 
            //
            //       The fix seems to be to set the max width only (height is fine)
            //       to that of the primary monitor, not the current monitor
            //        
            //       However, 1 pixel different and the size goes totally wrong again
            //       so the fix doesn't work when the taskbar is on the left or right
            //

            // Set monitor size
            CurrentMonitorSize = new Rectangle(lMmi.ptMaxPosition.X, lMmi.ptMaxPosition.Y, lMmi.ptMaxSize.X + lMmi.ptMaxPosition.X, lMmi.ptMaxSize.Y + lMmi.ptMaxPosition.Y);

            // Set min size
            var minSize = new Point(mWindow.MinWidth * mMonitorDpi.Value.DpiScaleX, mWindow.MinHeight * mMonitorDpi.Value.DpiScaleX);
            lMmi.ptMinTrackSize.X = (int)minSize.X;
            lMmi.ptMinTrackSize.Y = (int)minSize.Y;

            // Store new size
            mScreenSize = new Rect(lCurrentScreenInfo.rcWork.Left, lCurrentScreenInfo.rcWork.Top, lMmi.ptMaxSize.X, lMmi.ptMaxSize.Y);

            // Now we have the max size, allow the host to tweak as needed
            Marshal.StructureToPtr(lMmi, lParam, true);
        }
    }

    #region DLL Helper Structures
    enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        public Rectangle rcMonitor = new Rectangle();
        public Rectangle rcWork = new Rectangle();
        public int dwFlags = 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int Left, Top, Right, Bottom;

        public Rectangle(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
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
    };

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
    #endregion
}
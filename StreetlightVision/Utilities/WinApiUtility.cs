using StreetlightVision.Extensions;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace StreetlightVision.Utilities
{
    public static class WinApiUtility
    {
        #region kernel32.dll extern

        /// <summary>
        /// This function retrieves the current system date
        /// and time expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="lpSystemTime">[out] Pointer to a SYSTEMTIME structure to
        /// receive the current system date and time.</param>
        [DllImport("kernel32.dll")]
        public extern static void GetSystemTime(ref SystemTime lpSystemTime);

        /// <summary>
        /// This function sets the current system date
        /// and time expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="lpSystemTime">[in] Pointer to a SystemTime structure that
        /// contains the current system date and time.</param>
        [DllImport("kernel32.dll")]
        public extern static uint SetSystemTime(ref SystemTime lpSystemTime);

        #endregion

        #region user32.dll extern

        [DllImport("user32.dll")]
        extern static bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        extern static bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        extern static void mouse_event(MouseEvent dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        extern static void keybd_event(VK_KeyCode bVk, VK_ScanCode bScan, VK_KeyEvent dwFlags, int dwExtraInfo);

        #endregion

        /// <summary>
        /// Set current cursor position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void SetCursor(int x, int y)
        {
            SetCursorPos(x, y);
        }

        /// <summary>
        /// Set current cursor position
        /// </summary>
        /// <param name="point"></param>
        public static void SetCursor(Point point)
        {
            SetCursorPos(point.X, point.Y);
        }

        /// <summary>
        /// Get current cursor position
        /// </summary>
        /// <returns></returns>
        public static Point GetCursor()
        {
            Point lpPoint;
            GetCursorPos(out lpPoint);

            return lpPoint;
        }

        /// <summary>
        /// Simulate left click on real cursor
        /// </summary>
        /// <param name="p"></param>
        public static void LeftMouseClick(Point p, bool resetCursorPosToDefault = true)
        {
            SetCursorPos(p.X, p.Y);
            Wait.ForMilliseconds(100);
            mouse_event(MouseEvent.LeftDown | MouseEvent.LeftUp, p.X, p.Y, 0, 0);
            Wait.ForMilliseconds(100);
            if (resetCursorPosToDefault) SetCursorPos(0, 0);
            Wait.ForSeconds(1);
        }

        public static void LeftMouseClick()
        {
            Wait.ForMilliseconds(100);
            var p = GetCursor();
            mouse_event(MouseEvent.LeftDown | MouseEvent.LeftUp, p.X, p.Y, 0, 0);
            Wait.ForSeconds(1);
        }

        /// <summary>
        /// Simulate right click on real cursor
        /// </summary>
        /// <param name="p"></param>
        public static void RightMouseClick(Point p, bool resetCursorPosToDefault = true)
        {
            SetCursorPos(p.X, p.Y);
            Wait.ForMilliseconds(100);
            mouse_event(MouseEvent.RightDown | MouseEvent.RightUp, p.X, p.Y, 0, 0);
            Wait.ForMilliseconds(100);
            if (resetCursorPosToDefault) SetCursorPos(0, 0);
            Wait.ForSeconds(1);
        }

        /// <summary>
        /// Mouse double click at a specific point
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="scrollValue"></param>
        public static void MouseDbClick(Point pos, bool resetCursorPosToDefault = true)
        {
            SetCursorPos(pos.X, pos.Y);
            mouse_event(MouseEvent.LeftDown | MouseEvent.LeftUp, pos.X, pos.Y, 0, 0);
            Wait.ForMilliseconds(50);
            mouse_event(MouseEvent.LeftDown | MouseEvent.LeftUp, pos.X, pos.Y, 0, 0);
            if (resetCursorPosToDefault) SetCursorPos(0, 0);
            Wait.ForSeconds(1);
        }

        /// <summary>
        /// Drag and drop with real Cursor
        /// </summary>
        /// <param name="p"></param>
        public static void DragAndDrop(Point start, Point end, bool resetCursorPosToDefault = true)
        {
            SetCursorPos(start.X, start.Y);
            Wait.ForMilliseconds(500);

            mouse_event(MouseEvent.LeftDown, start.X, start.Y, 0, 0);
            Wait.ForMilliseconds(500);

            mouse_event(MouseEvent.Move, end.X - start.X, end.Y - start.Y, 0, 0);
            Wait.ForMilliseconds(500);

            SetCursorPos(end.X, end.Y);
            Wait.ForMilliseconds(500);

            mouse_event(MouseEvent.LeftUp, end.X, end.Y, 0, 0);
            Wait.ForMilliseconds(500);
            if (resetCursorPosToDefault) SetCursorPos(0, 0);
            Wait.ForSeconds(1);
        }

        /// <summary>
        /// Mouse scroll at a specific point
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="scrollValue"></param>
        public static void MouseScroll(Point pos, int scrollValue = 120, bool resetCursorPosToDefault = true)
        {
            SetCursorPos(pos.X, pos.Y);
            Wait.ForMilliseconds(100);

            //mouse_event(MouseEvent.LeftDown, pos.X, pos.Y, 0, 0);
            //Wait.ForMilliseconds(100);

            mouse_event(MouseEvent.Wheel, pos.X, pos.Y, scrollValue, 0);
            Wait.ForMilliseconds(100);

            //mouse_event(MouseEvent.LeftUp, pos.X, pos.Y, 0, 0);
            //Wait.ForMilliseconds(100);

            if (resetCursorPosToDefault)
            {
                SetCursorPos(0, 0);
            }
        }   

        public static void KeyDown(VK_KeyCode key)
        {
            KeyEvent(key, VK_KeyEvent.None);
        }

        public static void KeyUp(VK_KeyCode key)
        {
            KeyEvent(key, VK_KeyEvent.Up);
        }

        public static void KeyPress(VK_KeyCode key)
        {
            KeyEvent(key, VK_KeyEvent.None);
            KeyEvent(key, VK_KeyEvent.Up);
        }

        private static void KeyEvent(VK_KeyCode key, VK_KeyEvent keyEvent)
        {
            keybd_event(key, GetScanCode(key), keyEvent, 0);
        }

        private static VK_ScanCode GetScanCode(VK_KeyCode key)
        {
            switch(key)
            {
                case VK_KeyCode.SHIFT:
                case VK_KeyCode.LSHIFT:
                    return VK_ScanCode.LSHIFT;
                case VK_KeyCode.RSHIFT:
                    return VK_ScanCode.RSHIFT;
                case VK_KeyCode.CTRL:
                case VK_KeyCode.LCTRL:
                case VK_KeyCode.RCTRL:
                    return VK_ScanCode.LCTRL;
                case VK_KeyCode.ALT:
                    return VK_ScanCode.LALT;
                case VK_KeyCode.ENTER:
                    return VK_ScanCode.ENTER;
                case VK_KeyCode.ESC:
                    return VK_ScanCode.ESC;
                case VK_KeyCode.TAB:
                    return VK_ScanCode.TAB;
                case VK_KeyCode.Num0:
                    return VK_ScanCode.Num0;
                case VK_KeyCode.Num1:
                    return VK_ScanCode.Num1;
                case VK_KeyCode.Num2:
                    return VK_ScanCode.Num2;
                case VK_KeyCode.Num3:
                    return VK_ScanCode.Num3;
                case VK_KeyCode.Num4:
                    return VK_ScanCode.Num4;
                case VK_KeyCode.Num5:
                    return VK_ScanCode.Num5;
                case VK_KeyCode.Num6:
                    return VK_ScanCode.Num6;
                case VK_KeyCode.Num7:
                    return VK_ScanCode.Num7;
                case VK_KeyCode.Num8:
                    return VK_ScanCode.Num8;
                case VK_KeyCode.Num9:
                    return VK_ScanCode.Num9;
                case VK_KeyCode.A:
                    return VK_ScanCode.A;
                case VK_KeyCode.B:
                    return VK_ScanCode.B;
                case VK_KeyCode.C:
                    return VK_ScanCode.C;
                case VK_KeyCode.D:
                    return VK_ScanCode.D;
                case VK_KeyCode.E:
                    return VK_ScanCode.E;
                case VK_KeyCode.F:
                    return VK_ScanCode.F;
                case VK_KeyCode.G:
                    return VK_ScanCode.G;
                case VK_KeyCode.H:
                    return VK_ScanCode.H;
                case VK_KeyCode.I:
                    return VK_ScanCode.I;
                case VK_KeyCode.J:
                    return VK_ScanCode.J;
                case VK_KeyCode.K:
                    return VK_ScanCode.K;
                case VK_KeyCode.L:
                    return VK_ScanCode.L;
                case VK_KeyCode.M:
                    return VK_ScanCode.M;
                case VK_KeyCode.N:
                    return VK_ScanCode.N;
                case VK_KeyCode.O:
                    return VK_ScanCode.O;
                case VK_KeyCode.P:
                    return VK_ScanCode.P;
                case VK_KeyCode.Q:
                    return VK_ScanCode.Q;
                case VK_KeyCode.R:
                    return VK_ScanCode.R;
                case VK_KeyCode.S:
                    return VK_ScanCode.S;
                case VK_KeyCode.T:
                    return VK_ScanCode.T;
                case VK_KeyCode.U:
                    return VK_ScanCode.U;
                case VK_KeyCode.V:
                    return VK_ScanCode.V;
                case VK_KeyCode.W:
                    return VK_ScanCode.W;
                case VK_KeyCode.X:
                    return VK_ScanCode.X;
                case VK_KeyCode.Y:
                    return VK_ScanCode.Y;
                case VK_KeyCode.Z:
                    return VK_ScanCode.Z;
            }

            return VK_ScanCode.NONE;
        }

        #region Get color from pixel

        [DllImport("user32.dll")]
        extern static IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        extern static IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        extern static Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        extern static uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        public static Color GetPixelColorInTestWindow(int x, int y)
        {
            var testBrowserHandle = FindWindow(IntPtr.Zero, WebDriverContext.CurrentDriver.Title);
            IntPtr hdc = GetDC(testBrowserHandle);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);

            return color;
        }

        #endregion
    }

    [Flags]
    public enum MouseEvent : int
    {
        Absolute = 0x8000,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        Move = 0x0001,
        RightDown = 0x0008,
        RightUp = 0x0010,
        Wheel = 0x0800,
        XDown = 0x0080,
        XUp = 0x0100,
        HWheel = 0x1000,
    }

    [Flags]
    public enum VK_ScanCode : int
    {       
        NONE = 0,
        LSHIFT = 0xAA,
        RSHIFT = 0xB6,
        LCTRL = 0x9D,
        LALT = 0xB8,
        ESC = 0x81,
        ENTER = 0x9C,
        TAB = 0x8F,
        A = 0x9E,
        B = 0xB0,
        C = 0xAE,
        D = 0xA0,
        E = 0x92,
        F = 0xA1,
        G = 0xA2,
        H = 0xA3,
        I = 0x97,
        J = 0xA4,
        K = 0xA5,
        L = 0xA6,
        M = 0xB2,
        N = 0xB1,
        O = 0x98,
        P = 0x99,
        Q = 0x90,
        R = 0x93,
        S = 0x9F,
        T = 0x94,
        U = 0x96,
        V = 0xAF,
        W = 0x91,
        X = 0xAD,
        Y = 0x95,
        Z = 0xAC,
        Num0 = 0x82,
        Num1 = 0x83,
        Num2 = 0x84,
        Num3 = 0x85,
        Num4 = 0x86,
        Num5 = 0x87,
        Num6 = 0x88,
        Num7 = 0x89,
        Num8 = 0x8A,
        Num9 = 0x8B,
    }

    [Flags]
    public enum VK_KeyEvent : int
    {
        None = 0,
        Down = 0x0001,
        Up = 0x0002
    }

    [Flags]
    public enum VK_KeyCode : byte
    {
        ENTER = 0x0D,
        SHIFT = 0x10,
        LSHIFT = 0xA0,
        RSHIFT = 0xA1,
        CTRL = 0x11,
        LCTRL = 0xA2,
        RCTRL = 0xA3,
        ALT = 0x12,
        ESC = 0x1B,
        TAB = 0x09,
        Num0 = 0x30,
        Num1 = 0x31,
        Num2 = 0x32,
        Num3 = 0x33,
        Num4 = 0x34,
        Num5 = 0x35,
        Num6 = 0x36,
        Num7 = 0x37,
        Num8 = 0x38,
        Num9 = 0x39,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x39,
        F11 = 0x7A,
        F12 = 0x7B,
        LEFT = 0x25,
        UP = 0x26,
        RIGHT = 0x27,
        DOWN = 0x28
    }

    /// <summary> This structure represents a date and time. </summary>
    public struct SystemTime
    {
        public ushort wYear, wMonth, wDayOfWeek, wDay, wHour, wMinute, wSecond, wMilliseconds;
    }
}

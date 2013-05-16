using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Rimto
{
    class UiHelpers
    {
        internal enum WM
        {
            NULL = 0x0000,
            CREATE = 0x0001,
            DESTROY = 0x0002,
            MOVE = 0x0003,
            SIZE = 0x0005,
            ACTIVATE = 0x0006,
            SETFOCUS = 0x0007,
            KILLFOCUS = 0x0008,
            ENABLE = 0x000A,
            SETREDRAW = 0x000B,
            SETTEXT = 0x000C,
            GETTEXT = 0x000D,
            GETTEXTLENGTH = 0x000E,
            PAINT = 0x000F,
            CLOSE = 0x0010,
            QUERYENDSESSION = 0x0011,
            QUIT = 0x0012,
            QUERYOPEN = 0x0013,
            ERASEBKGND = 0x0014,
            SYSCOLORCHANGE = 0x0015,
            SHOWWINDOW = 0x0018,
            CTLCOLOR = 0x0019,
            WININICHANGE = 0x001A,
            SETTINGCHANGE = 0x001A,
            ACTIVATEAPP = 0x001C,
            SETCURSOR = 0x0020,
            MOUSEACTIVATE = 0x0021,
            CHILDACTIVATE = 0x0022,
            QUEUESYNC = 0x0023,
            GETMINMAXINFO = 0x0024,

            WINDOWPOSCHANGING = 0x0046,
            WINDOWPOSCHANGED = 0x0047,

            CONTEXTMENU = 0x007B,
            STYLECHANGING = 0x007C,
            STYLECHANGED = 0x007D,
            DISPLAYCHANGE = 0x007E,
            GETICON = 0x007F,
            SETICON = 0x0080,
            NCCREATE = 0x0081,
            NCDESTROY = 0x0082,
            NCCALCSIZE = 0x0083,
            NCHITTEST = 0x0084,
            NCPAINT = 0x0085,
            NCACTIVATE = 0x0086,
            GETDLGCODE = 0x0087,
            SYNCPAINT = 0x0088,
            NCMOUSEMOVE = 0x00A0,
            NCLBUTTONDOWN = 0x00A1,
            NCLBUTTONUP = 0x00A2,
            NCLBUTTONDBLCLK = 0x00A3,
            NCRBUTTONDOWN = 0x00A4,
            NCRBUTTONUP = 0x00A5,
            NCRBUTTONDBLCLK = 0x00A6,
            NCMBUTTONDOWN = 0x00A7,
            NCMBUTTONUP = 0x00A8,
            NCMBUTTONDBLCLK = 0x00A9,

            SYSKEYDOWN = 0x0104,
            SYSKEYUP = 0x0105,
            SYSCHAR = 0x0106,
            SYSDEADCHAR = 0x0107,
            COMMAND = 0x0111,
            SYSCOMMAND = 0x0112,

            MOUSEMOVE = 0x0200,
            LBUTTONDOWN = 0x0201,
            LBUTTONUP = 0x0202,
            LBUTTONDBLCLK = 0x0203,
            RBUTTONDOWN = 0x0204,
            RBUTTONUP = 0x0205,
            RBUTTONDBLCLK = 0x0206,
            MBUTTONDOWN = 0x0207,
            MBUTTONUP = 0x0208,
            MBUTTONDBLCLK = 0x0209,
            MOUSEWHEEL = 0x020A,
            XBUTTONDOWN = 0x020B,
            XBUTTONUP = 0x020C,
            XBUTTONDBLCLK = 0x020D,
            MOUSEHWHEEL = 0x020E,
            PARENTNOTIFY = 0x0210,

            CAPTURECHANGED = 0x0215,
            POWERBROADCAST = 0x0218,
            DEVICECHANGE = 0x0219,

            ENTERSIZEMOVE = 0x0231,
            EXITSIZEMOVE = 0x0232,

            IME_SETCONTEXT = 0x0281,
            IME_NOTIFY = 0x0282,
            IME_CONTROL = 0x0283,
            IME_COMPOSITIONFULL = 0x0284,
            IME_SELECT = 0x0285,
            IME_CHAR = 0x0286,
            IME_REQUEST = 0x0288,
            IME_KEYDOWN = 0x0290,
            IME_KEYUP = 0x0291,

            NCMOUSELEAVE = 0x02A2,

            TABLET_DEFBASE = 0x02C0,
            //WM_TABLET_MAXOFFSET = 0x20,

            TABLET_ADDED = TABLET_DEFBASE + 8,
            TABLET_DELETED = TABLET_DEFBASE + 9,
            TABLET_FLICK = TABLET_DEFBASE + 11,
            TABLET_QUERYSYSTEMGESTURESTATUS = TABLET_DEFBASE + 12,

            CUT = 0x0300,
            COPY = 0x0301,
            PASTE = 0x0302,
            CLEAR = 0x0303,
            UNDO = 0x0304,
            RENDERFORMAT = 0x0305,
            RENDERALLFORMATS = 0x0306,
            DESTROYCLIPBOARD = 0x0307,
            DRAWCLIPBOARD = 0x0308,
            PAINTCLIPBOARD = 0x0309,
            VSCROLLCLIPBOARD = 0x030A,
            SIZECLIPBOARD = 0x030B,
            ASKCBFORMATNAME = 0x030C,
            CHANGECBCHAIN = 0x030D,
            HSCROLLCLIPBOARD = 0x030E,
            QUERYNEWPALETTE = 0x030F,
            PALETTEISCHANGING = 0x0310,
            PALETTECHANGED = 0x0311,
            HOTKEY = 0x0312,
            PRINT = 0x0317,
            PRINTCLIENT = 0x0318,
            APPCOMMAND = 0x0319,
            THEMECHANGED = 0x031A,

            DWMCOMPOSITIONCHANGED = 0x031E,
            DWMNCRENDERINGCHANGED = 0x031F,
            DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

            GETTITLEBARINFOEX = 0x033F,

            #region Windows 7
            DWMSENDICONICTHUMBNAIL = 0x0323,
            DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,
            #endregion

            USER = 0x0400,

            TRAYMOUSEMESSAGE = 0x800, //WM_USER + 1024
            APP = 0x8000,
        }

        [Flags]
        internal enum WS_EX : uint
        {
            None = 0,
            DLGMODALFRAME = 0x00000001,
            NOPARENTNOTIFY = 0x00000004,
            TOPMOST = 0x00000008,
            ACCEPTFILES = 0x00000010,
            TRANSPARENT = 0x00000020,
            MDICHILD = 0x00000040,
            TOOLWINDOW = 0x00000080,
            WINDOWEDGE = 0x00000100,
            CLIENTEDGE = 0x00000200,
            CONTEXTHELP = 0x00000400,
            RIGHT = 0x00001000,
            LEFT = 0x00000000,
            RTLREADING = 0x00002000,
            LTRREADING = 0x00000000,
            LEFTSCROLLBAR = 0x00004000,
            RIGHTSCROLLBAR = 0x00000000,
            CONTROLPARENT = 0x00010000,
            STATICEDGE = 0x00020000,
            APPWINDOW = 0x00040000,
            LAYERED = 0x00080000,
            NOINHERITLAYOUT = 0x00100000,
            LAYOUTRTL = 0x00400000,
            COMPOSITED = 0x02000000,
            NOACTIVATE = 0x08000000,
            OVERLAPPEDWINDOW = (WINDOWEDGE | CLIENTEDGE),
            PALETTEWINDOW = (WINDOWEDGE | TOOLWINDOW | TOPMOST),
        }

        private enum WINDOWTHEMEATTRIBUTETYPE : uint
        {
            WTA_NONCLIENT = 1,
        }

        [Flags]
        private enum WTNCA : uint
        {
            NODRAWCAPTION = 0x00000001,
            NODRAWICON = 0x00000002,
            NOSYSMENU = 0x00000004,
            NOMIRRORHELP = 0x00000008,
            VALIDBITS = NODRAWCAPTION | NODRAWICON | NOMIRRORHELP | NOSYSMENU,
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct WTA_OPTIONS
        {
            public const uint Size = 8;

            [FieldOffset(0)]
            public WTNCA dwFlags;

            [FieldOffset(4)]
            public WTNCA dwMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Margins
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        static extern bool DwmIsCompositionEnabled();

        [DllImport("uxtheme.dll", PreserveSig = false)]
        private static extern void SetWindowThemeAttribute([In] IntPtr hwnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins pMarInset);

        public static void ExtendFrameIntoClientArea(Window window)
        {
            try
            {
                bool compositionEnabled = DwmIsCompositionEnabled();

                if (compositionEnabled)
                {
                    IntPtr hwnd = new WindowInteropHelper(window).Handle;

                    if (hwnd != IntPtr.Zero)
                    {
                        var hwndSource = HwndSource.FromHwnd(hwnd);

                        window.Background = Brushes.Transparent;
                        hwndSource.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

                        var margins = new Margins { cxLeftWidth = -1, cxRightWidth = -1, cyTopHeight = -1, cyBottomHeight = -1 };
                        DwmExtendFrameIntoClientArea(hwndSource.Handle, ref margins);
                    }
                    else
                    {
                        throw new InvalidOperationException("Window's handle is not valid.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("DWM Composition disabled.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void SetWindowThemeAttribute(Window window, bool showCaption, bool showIcon, bool showSysMenu)
        {
            try
            {
                bool compositionEnabled = DwmIsCompositionEnabled();

                if (compositionEnabled)
                {
                    IntPtr hwnd = new WindowInteropHelper(window).Handle;

                    if (hwnd != IntPtr.Zero)
                    {
                        var options = new WTA_OPTIONS
                        {
                            dwMask = (WTNCA.NODRAWCAPTION | WTNCA.NODRAWICON)
                        };

                        if (!showCaption)
                        {
                            options.dwFlags |= WTNCA.NODRAWCAPTION;
                        }

                        if (!showIcon)
                        {
                            options.dwFlags |= WTNCA.NODRAWICON;
                        }

                        if (!showSysMenu)
                        {
                            options.dwFlags |= WTNCA.NOSYSMENU;
                        }

                        SetWindowThemeAttribute(hwnd, WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT, ref options, WTA_OPTIONS.Size);
                    }
                    else
                    {
                        throw new InvalidOperationException("Window's handle is not valid.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("DWM Composition disabled.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void AttachWndProcHook(Window window, HwndSourceHook MessageHandler)
        {
            try
            {
                IntPtr hwnd = new WindowInteropHelper(window).Handle;

                if (hwnd != IntPtr.Zero)
                {
                    var hwndSource = HwndSource.FromHwnd(hwnd);

                    hwndSource.AddHook(new HwndSourceHook(MessageHandler));
                }
                else
                {
                    throw new InvalidOperationException("Window's handle is not valid.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;

namespace Rimto
{
    public partial class RimtoWindow
    {
        private IntPtr MessageHandler(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            IntPtr rc = IntPtr.Zero;

            UiHelpers.WM wmMsg = (UiHelpers.WM)msg;

            if (wmMsg == UiHelpers.WM.NCCALCSIZE)
            {
                handled = true;
            }
            else if (wmMsg == UiHelpers.WM.NCHITTEST)
            {
                const int HTCAPTION = 2;
                const int HTCLIENT = 1;
                const int HTCLOSE = 20;

                var hwndSource = HwndSource.FromHwnd(hWnd);
                var window = hwndSource.RootVisual as RimtoWindow;

                if (window != null)
                {
                    int x = lParam.ToInt32() & 0x0000FFFF;
                    int y = (int)((lParam.ToInt32() & 0xFFFF0000) >> 16);
                    Point origPos = new Point(x, y);
                    Point winPos = window.PointFromScreen(origPos);

                    // TODO: normal caption hit test
                    // var visual = VisualTreeHelper.HitTest(window, winPos).VisualHit as System.Windows.Controls.TextBlock;*/
                    
                    if (
                            winPos.X > 140 &&
                            winPos.X < window.ActualWidth - SystemParameters.WindowCaptionButtonWidth - SystemParameters.BorderWidth * 3 &&
                            winPos.Y < SystemParameters.WindowCaptionButtonHeight * 2
                       )
                    {
                        rc = (IntPtr)HTCAPTION;
                    }
                    else if (
                            winPos.X > window.ActualWidth - SystemParameters.WindowCaptionButtonWidth - SystemParameters.BorderWidth * 3 &&
                            winPos.Y < SystemParameters.WindowCaptionButtonHeight
                       )
                    {
                        rc = (IntPtr)HTCLOSE;
                    }
                    else
                    {
                        rc = (IntPtr)HTCLIENT;
                    }

                    handled = true;
                }
                else
                {
                    throw new InvalidOperationException("Window's handle is not valid.");
                }
            }

            return rc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ServerWF {
    public class WIN32 {

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {

            public Int32 x;

            public Int32 y;

        }//POINT

        [DllImport("user32.dll")]
        public static extern IntPtr GetCursor();

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO {

            public Int32 cbSize;

            public Int32 flags;

            public IntPtr hCursor;

            public POINT ptScreenPos;

        }//CURSORINFO

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(ref CURSORINFO pci);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd); //GetDC(IntPtr.Zero);
        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC); //ReleaseDC(IntPtr.Zero, ...);
        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hwnd, IntPtr lpRect, bool bErase); //InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);

        [DllImport("gdi32.dll")]
        public static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject([In] IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect {

            public int left;
            public int top;
            public int right;
            public int bottom;

        }//Rect

        public enum MapType : uint {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static string GetCharFromKey(int keyValue) {

            char ch = '\0';

            int virtualKey = keyValue;
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result) {
                case -1:
                    break;
                case 0:
                    break;
                case 1: {
                        ch = stringBuilder[0];
                        break;
                    }
                default: {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            
            return ch == '\0' ? "" : ch.ToString();

        }//GetCharFromKey

    }//WIN32

}//ServerWF

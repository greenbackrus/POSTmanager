using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace POSTmanager.helpers
{
    internal static class User32Helper
    {
        const int WM_SETTEXT = 0x000C;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;

        private static class User32Lib 
        {
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
        }

        private static bool IsValidHandle(IntPtr hWnd)
        {
            return hWnd != IntPtr.Zero;
        }

        public static IntPtr GetWindowByTitle(String Title)
        {
            IntPtr hWindow = User32Lib.FindWindow(null, Title);
            return hWindow;
        }

        public static IntPtr GetElement(IntPtr ParentWindow, IntPtr ChildWindow, string ElementClass, string ElementName)
        {
            IntPtr El = IntPtr.Zero;
            int Attempt = 0;

            // TODO: refactor
            El = User32Lib.FindWindowEx(ParentWindow, IntPtr.Zero, ElementClass, ElementName);
            while (!IsValidHandle(El) && Attempt < 3) 
            {
                Task.Delay(1500).GetAwaiter().GetResult();
                El = User32Lib.FindWindowEx(ParentWindow, IntPtr.Zero, ElementClass, ElementName);
                Attempt++;
            };

            return El;
        }

        public static void SetElementText(IntPtr Element, String Text) 
        {
            User32Lib.SendMessage((IntPtr)Element, WM_SETTEXT, IntPtr.Zero, Text);
        }

        public static void ClickElement(IntPtr Element) 
        {
            GeneralHelper.Wait(1500);
            User32Lib.SendMessage((IntPtr)Element, WM_LBUTTONDOWN, IntPtr.Zero, null);
            GeneralHelper.Wait(500);
            User32Lib.SendMessage((IntPtr)Element, WM_LBUTTONUP, IntPtr.Zero, null);
        }
    }
}

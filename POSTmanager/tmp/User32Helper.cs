using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;

namespace POSTmanager.Helpers
{
    internal static class User32Helper
    {
        const int WM_SETTEXT = 0x000C;
        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;
        // fix case sensitivity issue

        private static class User32Lib
        {
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindow(string ipClassName, string ipWindowName);
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
        }

        private static bool IsValidHandle(IntPtr hWnd)
        {
            return hWnd != IntPtr.Zero;
        }

        public static IntPtr GetWindowWithTitleVariants(List<string> titles)
        {
            foreach (var title in titles )
            {
                IntPtr hWindow = User32Lib.FindWindow(null, title);
                if (IsValidHandle(hWindow))
                {
                    return hWindow;
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr GetElementByName(IntPtr parentWindow, string elementClass, string elementName)
        {
            IntPtr el = User32Lib.FindWindowEx(parentWindow, IntPtr.Zero, elementClass, elementName);
            if (IsValidHandle(el))
            {
                return el;
            }
            return IntPtr.Zero;
        }

        public static IntPtr GetElementWithNameVariants(IntPtr parentWindow, string elementClass, List<string> elementNames)
        {
            foreach (var elementName in elementNames)
            {
                IntPtr el = User32Lib.FindWindowEx(parentWindow, IntPtr.Zero, elementClass, elementName);
                if (IsValidHandle(el))
                {
                    return el;
                }
            }
            return IntPtr.Zero;
        }

        public static void SetElementText(IntPtr element, String text)
        {
            User32Lib.SendMessage((IntPtr)element, WM_SETTEXT, IntPtr.Zero, text);
        }

        public static void ClickElement(IntPtr element)
        {
            GeneralHelper.Wait(1500);
            User32Lib.SendMessage((IntPtr)element, WM_LBUTTONDOWN, IntPtr.Zero, null);
            GeneralHelper.Wait(500);
            User32Lib.SendMessage((IntPtr)element, WM_LBUTTONUP, IntPtr.Zero, null);
        }
    }
}

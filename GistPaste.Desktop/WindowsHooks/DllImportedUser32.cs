using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GistPaste.Desktop.WindowsHooks
{
    class DllImportedUser32 : IUser32
    {
        public IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam)
        {
            return User32DllImports.CallNextHookEx(hhk, nCode, wParam, lParam);
        }

        public IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId)
        {
            return User32DllImports.SetWindowsHookEx(idHook, lpfn, hMod, dwThreadId);
        }

        public bool UnhookWindowsHookEx(IntPtr hhk)
        {
            return User32DllImports.UnhookWindowsHookEx(hhk);
        }
    }
}

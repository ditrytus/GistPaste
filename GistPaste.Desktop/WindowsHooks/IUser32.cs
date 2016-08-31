using System;

namespace GistPaste.Desktop.WindowsHooks
{
    public interface IUser32
    {
        IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        bool UnhookWindowsHookEx(IntPtr hhk);
        IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    }
}

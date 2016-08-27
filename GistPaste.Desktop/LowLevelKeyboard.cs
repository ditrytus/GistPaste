using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GistPaste.Desktop
{
    class LowLevelKeyboard : IDisposable
    {
        private const int WH_KEYBOARD_LL = 13;

        private static IntPtr hookID = IntPtr.Zero;
        private ISubject<KeyboardEvent> keyStrokeSubject;
        private LowLevelKeyboardProc callback;
        public IObservable<KeyboardEvent> KeyboardEvents
        {
            get
            {
                return keyStrokeSubject;
            }
        }

        public LowLevelKeyboard()
        {
            callback = HookCallback;
            keyStrokeSubject = new Subject<KeyboardEvent>();
            hookID = SetHook(callback);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && new[] { LowLevelKeyboardMessages.WM_KEYDOWN, LowLevelKeyboardMessages.WM_KEYUP}.Select(m => (IntPtr)m).Contains(wParam))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                keyStrokeSubject.OnNext(new KeyboardEvent(KeyInterop.KeyFromVirtualKey(vkCode), (LowLevelKeyboardMessages)wParam));
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}

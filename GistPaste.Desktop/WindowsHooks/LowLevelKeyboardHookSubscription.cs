using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GistPaste.Desktop
{
    class LowLevelKeyboardHookSubscription : IDisposable
    {
        private static IntPtr hookID = IntPtr.Zero;
        private LowLevelKeyboardProc callback;
        private IObserver<LowLevelKeyboardMessage> observer;

        public LowLevelKeyboardHookSubscription(IObserver<LowLevelKeyboardMessage> observer)
        {
            this.observer = observer;
            callback = HookCallback;
            hookID = SetHook(callback);            
        }

        public void Dispose()
        {
            User32.UnhookWindowsHookEx(hookID);
            observer.OnCompleted();
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return User32.SetWindowsHookEx(HookTypes.WH_KEYBOARD_LL, proc, Kernel32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= HookCallbackCodes.HC_ACTION)
            {
                observer.OnNext(new LowLevelKeyboardMessage(nCode, wParam, lParam));
            }

            return User32.CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}

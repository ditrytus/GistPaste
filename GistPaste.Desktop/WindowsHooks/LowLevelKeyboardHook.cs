using GistPaste.Desktop.WindowsHooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GistPaste.Desktop
{
    public class LowLevelKeyboardHook : IObservable<LowLevelKeyboardMessage>
    {
        private readonly List<LowLevelKeyboardHookSubscription> subscriptions = new List<LowLevelKeyboardHookSubscription>();
        private readonly LowLevelKeyboardProc callback;
        private IntPtr hookId = IntPtr.Zero;
        private IUser32 user32;
        private IKernel32 kernel32;

        public LowLevelKeyboardHook(IUser32 user32, IKernel32 kernel32)
        {
            this.user32 = user32;
            this.kernel32 = kernel32;
            callback = HookCallback;
        }

        public IDisposable Subscribe(IObserver<LowLevelKeyboardMessage> observer)
        {
            if (!subscriptions.Any())
            {
                hookId = SetHook(callback);
            }
            var subscription = new LowLevelKeyboardHookSubscription(this, observer);
            subscriptions.Add(subscription);
            return subscription;
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return user32.SetWindowsHookEx(HookTypes.WH_KEYBOARD_LL, proc, kernel32.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= HookCallbackCodes.HC_ACTION)
            {
                var message = new LowLevelKeyboardMessage(nCode, wParam, lParam);
                subscriptions.ForEach(s => s.Push(message));
            }

            return user32.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private void Unsubscribe(LowLevelKeyboardHookSubscription subscription)
        {
            subscriptions.Remove(subscription);
            if (!subscriptions.Any())
            {
                user32.UnhookWindowsHookEx(hookId);
            }
        }

        class LowLevelKeyboardHookSubscription : IDisposable
        {
            private LowLevelKeyboardHook hook;
            private IObserver<LowLevelKeyboardMessage> observer;

            public LowLevelKeyboardHookSubscription(LowLevelKeyboardHook hook, IObserver<LowLevelKeyboardMessage> observer)
            {
                this.observer = observer;
                this.hook = hook;
            }

            public void Dispose()
            {
                hook.Unsubscribe(this);
            }

            internal void Push(LowLevelKeyboardMessage message)
            {
                observer.OnNext(message);
            }
        }
    }
}

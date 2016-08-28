using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GistPaste.Desktop
{
    class LowLevelKeyboardHook : IObservable<LowLevelKeyboardMessage>
    {
        private List<LowLevelKeyboardHookSubscription> subscriptions;
        private static IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc callback;

        public LowLevelKeyboardHook()
        {
            subscriptions = new List<LowLevelKeyboardHookSubscription>();
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
                var message = new LowLevelKeyboardMessage(nCode, wParam, lParam);
                subscriptions.ForEach(s => s.Push(message));
            }

            return User32.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private void Unsubscribe(LowLevelKeyboardHookSubscription subscription)
        {
            subscriptions.Remove(subscription);
            if (!subscriptions.Any())
            {
                User32.UnhookWindowsHookEx(hookId);
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

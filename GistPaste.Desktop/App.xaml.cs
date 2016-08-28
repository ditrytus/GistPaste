using System;
using System.Windows;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace GistPaste.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IDisposable keyboardHook;

        public App()
        {
            keyboardHook = new LowLevelKeyboardHook()
                .Subscribe(m =>
                {
                    Debug.WriteLine(KeyInterop.KeyFromVirtualKey(Marshal.ReadInt32(m.LParam)));
                });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            keyboardHook.Dispose();
            base.OnExit(e);
        }
    }
}

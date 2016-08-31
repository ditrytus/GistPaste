using System;
using System.Windows;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using GistPaste.Desktop.WindowsHooks;

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
            keyboardHook = new LowLevelKeyboardHook(new DllImportedUser32(), new DllImportedKernel32())
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

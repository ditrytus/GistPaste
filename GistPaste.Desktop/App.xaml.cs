using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reactive.Linq;
using System.Diagnostics;

namespace GistPaste.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IDisposable globalKeyboard;
        IDisposable writeKeyDescription;

        public App()
        {
            var gk = new LowLevelKeyboard();
            globalKeyboard = gk;
            writeKeyDescription = gk.KeyboardEvents.Subscribe(keyEvent => Debug.WriteLine($"{keyEvent.Key}, {keyEvent.Message}"));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            writeKeyDescription.Dispose();
            globalKeyboard.Dispose();
            base.OnExit(e);
        }
    }
}

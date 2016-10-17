using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GistPaste.Desktop.ViewModels
{
    public class TaskbarViewModel : ReactiveObject
    {
        private readonly ReactiveCommand<object> quitCommand;
        public ReactiveCommand<object> QuitCommand => quitCommand;

        public TaskbarViewModel()
        {
            quitCommand = ReactiveCommand.Create();
            quitCommand.Subscribe(_ => Application.Current.Shutdown());
        }
    }
}

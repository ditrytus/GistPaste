using GistPaste.Desktop.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GistPaste.Desktop.Controls
{
    /// <summary>
    /// Interaction logic for Taskbar.xaml
    /// </summary>
    public partial class Taskbar : IViewFor<TaskbarViewModel>
    {
        public Taskbar()
        {
            InitializeComponent();

            ViewModel = new TaskbarViewModel();

            this.WhenActivated(d =>
            { 
                d(this.BindCommand(
                    this.ViewModel,
                    m => m.QuitCommand,
                    v => v.ExitMenuItem,
                    nameof(ExitMenuItem.Click)));
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (TaskbarViewModel)value; }
        }

        public TaskbarViewModel ViewModel
        {
            get { return (TaskbarViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(TaskbarViewModel), typeof(Taskbar));
    }
}

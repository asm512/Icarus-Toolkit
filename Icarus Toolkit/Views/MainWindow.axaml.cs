using Avalonia.Controls;
using Avalonia.Media;
using Icarus_Toolkit.ViewModels;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Icarus_Toolkit.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            (this.DataContext as MainWindowViewModel).ConfirmPath();
        }
    }
}

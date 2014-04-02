using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Sparrow.Chart.Demos
{
    public class AboutCommand : ICommand
    {
        Sparrow.Chart.Demos.View.About about;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            about = new View.About();
            about.Owner = (Window)parameter;
            about.ShowDialog();
            
        }
    }
}

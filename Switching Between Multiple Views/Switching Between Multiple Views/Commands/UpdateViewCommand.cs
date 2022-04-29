using Switching_Between_Multiple_Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Switching_Between_Multiple_Views.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewmodel;
        public event EventHandler? CanExecuteChanged;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewmodel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter.ToString() == "Home")
            {
                viewmodel.SelectedViewModel = new HomeViewModel();
            }
            else if(parameter.ToString() == "Account")
            {
                viewmodel.SelectedViewModel = new AccountViewModel();
            }
        }
    }
}

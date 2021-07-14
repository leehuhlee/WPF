using Dialog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dialog
{
    public class UIManager : IUIManager
    {
        public Task ShowMessage(MessageBoxDialogViewModel viewModel)
         {
            return new DialogMessageBox().ShowDialog(viewModel);
        }
    }
}

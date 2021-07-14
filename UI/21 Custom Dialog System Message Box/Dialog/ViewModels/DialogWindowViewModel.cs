using Dialog.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dialog
{
    public class DialogWindowViewModel : WindowViewModel
    {
        #region Public Properties
        public string Title { get; set; }

        public Control Content { get; set; }
        #endregion

        #region Contructor
        public DialogWindowViewModel(Window window) : base(window) 
        {
            WindowMinimumWidth = 250;
            WindowMinimumHeight = 100;

            TitleHeight = 30;
        }
        #endregion
    }
}

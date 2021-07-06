using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfTreeView
{
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new DirectoryStructureViewModel();
        }
        #endregion
    }
}

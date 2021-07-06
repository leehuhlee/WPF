using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTreeView
{
    public class DirectoryStructureViewModel : BaseViewModel
    {
        #region Public Properties
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }
        #endregion

        #region Constructor
        public DirectoryStructureViewModel()
        {
            var children = DirectoryStructure.GetLogicalDrives();
            this.Items = new ObservableCollection<DirectoryItemViewModel>(DirectoryStructure.GetLogicalDrives().Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive)));
        }
        #endregion
    }
}

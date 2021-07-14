using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dialog.Core
{
    public class BaseDialogViewModel : BaseViewModel
    {
        public string Title { get; set; }
    }
}

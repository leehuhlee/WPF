using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Core
{
    public class MessageBoxDialogDesignModel : MessageBoxDialogViewModel
    {
        #region Singleton
        public static MessageBoxDialogDesignModel Instance => new MessageBoxDialogDesignModel();
        #endregion

        #region Contructor
        public MessageBoxDialogDesignModel()
        {
            OkText = "OK";
            Message = "Design time message are fun :)";
        }
        #endregion
    }
}

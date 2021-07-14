using Dialog.Core;

namespace Dialog
{
    public class BasePopupViewModel : BaseViewModel
    {
        #region Public Properties
        public string BubbleBackground { get; set; }

        public ElementHorizontalAlignment ArrowAlignment { get; set; }

        public BaseViewModel Content { get; set; }
        #endregion

        #region Constructor
        public BasePopupViewModel()
        {
            // Set default values
            // TODO: Move colors into Core and make use of it here
            BubbleBackground = "ffffff";
            ArrowAlignment = ElementHorizontalAlignment.Left;
        }
        #endregion
    }
}

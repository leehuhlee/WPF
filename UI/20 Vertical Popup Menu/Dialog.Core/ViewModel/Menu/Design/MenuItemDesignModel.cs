namespace Dialog.Core
{
    public class MenuItemDesignModel : MenuItemViewModel
    {
        #region Singleton
        public static MenuItemDesignModel Instance => new MenuItemDesignModel();
        #endregion

        #region Constructor
        public MenuItemDesignModel()
        {
            Text = "Hello World";
            Icon = IconType.File;
        }
        #endregion
    }
}

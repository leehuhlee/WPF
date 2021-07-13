namespace Dialog.Core
{
    public class ChatListItemDesignModel : ChatListItemViewModel
    {
        #region Singleton
        public static ChatListItemDesignModel Instance => new ChatListItemDesignModel();
        #endregion

        #region Constructor
        public ChatListItemDesignModel()
        {
            Initials = "LM";
            Name = "Luke";
            Message = "This chat app is awesome! I bet it will be fast too";
            ProfilePictureRGB = "3099c5";
        }
        #endregion
    }
}

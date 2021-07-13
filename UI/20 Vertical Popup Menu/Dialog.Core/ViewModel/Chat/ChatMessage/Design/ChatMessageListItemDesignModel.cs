using System;

namespace Dialog.Core
{
    public class ChatMessageListItemDesignModel : ChatMessageListItemViewModel
    {
        #region Singleton
        public static ChatMessageListItemDesignModel Instance => new ChatMessageListItemDesignModel();
        #endregion

        #region Constructor
        public ChatMessageListItemDesignModel()
        {
            Initials = "LM";
            SenderName = "Luke";
            Message = "Some design time visual text";
            ProfilePictureRGB = "3099c5";
            SentByMe = true;
            MessageSentTime = DateTimeOffset.UtcNow;
            MessageReadTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(1.3));
        }
        #endregion
    }
}

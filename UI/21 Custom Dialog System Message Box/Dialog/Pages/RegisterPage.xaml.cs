using Dialog.Core;
using System.Security;

namespace Dialog
{
    public partial class RegisterPage : BasePage<RegisterViewModel>, IHavePassword
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        public SecureString SecurePassword => PasswordText.SecurePassword;
    }
}

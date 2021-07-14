using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dialog.Core
{
    public class RegisterViewModel : BaseViewModel
    {
        #region Public Properties
        public string Email { get; set; }

        public bool RegisterIsRunning { get; set; }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }
        #endregion

        #region Constructor
        public RegisterViewModel()
        {
            // Create commands
            RegisterCommand = new RelayParameterizedCommand(async (parameter) => await RegisterAsync(parameter));
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }
        #endregion

        public async Task RegisterAsync(object parameter)
        {
            await RunCommandAsync(() => RegisterIsRunning, async () =>
            {
                await Task.Delay(5000);
            });
        }

        public async Task LoginAsync()
        {
            // Go to register page?
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }
    }
}

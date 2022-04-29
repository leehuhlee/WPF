using System.ComponentModel;

namespace Switching_Between_Multiple_Views.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string parameterName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(parameterName));
        }
    }
}
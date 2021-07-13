using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Chrome
{
    public class PasswordBoxProperties
    {
        public static readonly DependencyProperty MornitorPasswordProperty = 
            DependencyProperty.RegisterAttached("MornitorPassword", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false, OnMonitorPasswordChanged));

        private static void OnMonitorPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = (d as PasswordBox);

            if (passwordBox == null)
                return;

            if((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
        }

        public static void SetMornitorPassword(PasswordBox element, bool value)
        {
            element.SetValue(MornitorPasswordProperty, value);
        }

        public static bool GetMornitorPassword(PasswordBox element)
        {
            return (bool)element.GetValue(MornitorPasswordProperty);
        }

        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(PasswordBoxProperties), new PropertyMetadata(false));

        private static void SetHashText(PasswordBox element)
        {
            element.SetValue(HasTextProperty, element.SecurePassword.Length > 0);
        }

        public static bool GetHasText(PasswordBox element)
        {
            return (bool)element.GetValue(HasTextProperty);
        }
    }
}

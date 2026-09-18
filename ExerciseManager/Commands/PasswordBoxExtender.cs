using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ExerciseManager.Commands
{
    public static class PasswordBoxExtender
    {
        public static readonly DependencyProperty EncryptedPasswordProperty = DependencyProperty.RegisterAttached(
            "EncryptedPassword",
            typeof(SecureString),
            typeof(PasswordBoxExtender),
            new PropertyMetadata(OnEncryptedPasswordChanged));

        private static void OnEncryptedPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if( e.NewValue == null)
               return;
            PasswordBox passwordBox = d as PasswordBox;
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SetEncryptedPassword((DependencyObject)sender, ((PasswordBox)sender).SecurePassword);
        }

        public static SecureString GetEncryptedPassword(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(EncryptedPasswordProperty);
        }
        public static readonly DependencyProperty EncryptedPasswordRepeatedProperty = DependencyProperty.RegisterAttached(
            "EncryptedPasswordRepeated",
            typeof(SecureString),
            typeof(PasswordBoxExtender),
            new PropertyMetadata(OnEncryptedPasswordRepeatedChanged));

        private static void OnEncryptedPasswordRepeatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;
            PasswordBox passwordBox = d as PasswordBox;
            passwordBox.PasswordChanged -= PasswordBox_PasswordRepeatedChanged;
            passwordBox.PasswordChanged += PasswordBox_PasswordRepeatedChanged;
        }

        private static void PasswordBox_PasswordRepeatedChanged(object sender, RoutedEventArgs e)
        {
            SetEncryptedPasswordRepeated((DependencyObject)sender, ((PasswordBox)sender).SecurePassword);
        }
        public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordProperty, value);
        }

        public static SecureString GetEncryptedPasswordRepeated(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(EncryptedPasswordRepeatedProperty);
        }

        public static void SetEncryptedPasswordRepeated(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordRepeatedProperty, value);
        }
    }
}

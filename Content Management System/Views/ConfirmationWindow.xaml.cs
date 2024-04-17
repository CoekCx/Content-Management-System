using System;
using System.Windows;
using System.Windows.Input;

namespace Content_Management_System.Views
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow(string msg, string ttl = "")
        {
            InitializeComponent();
            message.Content = msg;
            if (!String.IsNullOrEmpty(ttl))
            {
                title.Content = ttl;
            }
        }

        // Buttons
        private void buttonClose_No(object sender, RoutedEventArgs e)
        {
            Dashboard.UserChoice = false;
            this.Close();
        }

        private void buttonClose_Yes(object sender, RoutedEventArgs e)
        {
            Dashboard.UserChoice = true;
            this.Close();
        }

        // Window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}

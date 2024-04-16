using System;
using System.Windows;
using System.Windows.Input;

namespace Content_Management_System.Views
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow(string msg, string ttl = "")
        {
            InitializeComponent();
            message.Content = msg;
            if (!String.IsNullOrEmpty(ttl))
            {
                title.Content = ttl;
            }
        }

        // Buttons
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
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

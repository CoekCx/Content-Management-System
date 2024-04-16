using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Content_Management_System.Models;
using Content_Management_System.Services;
using Content_Management_System.Views;

namespace Content_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static User LoggedInUser { get; set; } = null;
        public static User SelectedUser { get; set; } = null;
        public static List<User> Users { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ReadUsers();
            LoggedInUser = new User();
        }

        // ComboBoxes
        private void cmbProfile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            labelProfileError.Content = "";

            try
            {
                if (!string.IsNullOrEmpty(cmbProfile.SelectedItem.ToString()))
                {
                    SelectedUser = Users.FirstOrDefault(x => x.Name == cmbProfile.SelectedItem.ToString()) ?? new User();
                }
            }
            catch (Exception ex) { }
        }

        // TextBoxes
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                buttonLogin_Click(sender, e);
            }
        }

        // Buttons
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (!Validate()) return;

            // Update data
            ReadUsers();
            var selectedUser = Users.FirstOrDefault(x => x.Name == cmbProfile.SelectedItem.ToString()) ?? new User();
            if (String.IsNullOrEmpty(selectedUser.Name))
            {
                labelProfileError.Content = "Nonexistant profile selected!";
                return;
            }
            labelProfileError.Content = "";

            // Autentification
            if (Autentificator.Autentificate(selectedUser, pswBoxPassword.Password))
            {
                LoggedInUser = selectedUser;
                labelPasswordError.Content = "";
                this.Hide();
                new MessageWindow($"Signed in as {selectedUser.Name}", "Login Success").ShowDialog();
                new Dashboard().ShowDialog();
                this.Show();
            }
            else
            {
                labelPasswordError.Content = "Incorrect password!";
            }
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

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

        // Helpers
        private bool Validate()
        {
            var isValid = true;

            if (cmbProfile.SelectedItem == null)
            {
                labelProfileError.Content = "Choose a profile!";
                isValid = false;
            }
            else
            {
                labelProfileError.Content = "";
            }
            if (String.IsNullOrEmpty(pswBoxPassword.Password))
            {
                labelPasswordError.Content = "Enter password!";
                isValid = false;
            }
            else
            {
                labelPasswordError.Content = "";
            }

            return isValid;
        }

        private void ReadUsers()
        {
            Users = ProfileManager.LoadUsers();
            var names = Users.Select(x => x.Name).ToList() ?? new List<string>();
            cmbProfile.ItemsSource = names;
        }
    }
}

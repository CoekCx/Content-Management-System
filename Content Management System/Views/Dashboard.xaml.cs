using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Content_Management_System.Models;
using Content_Management_System.Services;
using Content_Management_System.Persistance;
using System.Collections.Generic;
using System.Linq;

namespace Content_Management_System.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private static DataIO serializer = new DataIO();
        public static BindingList<Weapon> Weapons { get; set; }
        public Dashboard()
        {
            Weapons = serializer.DeSerializeObject<BindingList<Weapon>>("weapons.xml");
            if (Weapons == null)
            {
                Weapons = new BindingList<Weapon>();
            }

            DataContext = this;
            InitializeComponent();

            Icon = ContentManager.GetIcon();
            Login();
        }

        private void Login()
        {
            try
            {
                this.Hide();
                new MainWindow().ShowDialog();
                UpdateUI();
                this.Show();
            }
            catch (Exception) { }
        }

        private void UpdateUI()
        {
            ButtonsPanel.Visibility = States.CurrentlyLoggedInUsersType == Models.UserType.Visitor ? Visibility.Collapsed : Visibility.Visible;
        }

        // Buttons
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonNewEntry_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new NewEntry().ShowDialog();
            this.Show();
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void SignOut(object sender, RoutedEventArgs e)
        {
            Login();
        }

        // Window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_Closing(object sender, CancelEventArgs e) => SaveWeapons();

        // Helper
        public static void SaveWeapons() => serializer.SerializeObject<BindingList<Weapon>>(Weapons, "weapons.xml");

        public static List<string> GetWeaponNames() => Weapons.Select(x => x.Name).ToList();
    }
}

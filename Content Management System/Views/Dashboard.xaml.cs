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
        public static bool UserChoice { get; set; }
        public Dashboard()
        {
            Weapons = serializer.DeSerializeObject<BindingList<Weapon>>("weapons.xml");
            if (Weapons == null)
            {
                Weapons = new BindingList<Weapon>();
            }
            // Check for invalid images
            foreach (var weapon in Weapons)
            {
                if (!File.Exists(weapon.ImgPath)) weapon.ImgPath = ContentManager.GetImageNotFoundPlaceholderPath();
            }
            SaveWeapons();

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
            if (States.CurrentlyLoggedInUsersType == Models.UserType.Visitor)
            {
                ButtonsPanel.Visibility = Visibility.Collapsed;
                HideCheckboxColumn();
            }
            else
            {
                ButtonsPanel.Visibility = Visibility.Visible;
                ShowCheckboxColumn();
            }
        }

        // Buttons
        private void buttonClick_Inspect(object sender, RoutedEventArgs e)
        {
            var name = (sender as Button).Tag;
            var weaponIndex = Weapons.ToList().FindIndex(x => x.Name == name);
            var weapon = Weapons.FirstOrDefault(x => x.Name == name);
            this.Hide();
            Window window = States.CurrentlyLoggedInUsersType != Models.UserType.Visitor ? new NewEntry(weaponIndex) : new Inspect(weapon);
            window.ShowDialog();
            this.Show();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            // Get selected weapons
            var selectedWeapons = Weapons.Where(w => w.IsSelected).ToList();

            if (selectedWeapons.Count == 0) return;

            this.Hide();
            new ConfirmationWindow("Are you sure you want to\ndelete these weapons?", "Confirmation").ShowDialog();
            if (!UserChoice)
            {
                this.Show();
                return;
            }

            // Remove selected weapons from the list
            foreach (var weapon in selectedWeapons)
            {
                if (File.Exists(weapon.FilePath)) File.Delete(weapon.FilePath);
                Weapons.Remove(weapon);
            }

            // Save changes
            SaveWeapons();

            // Clear checkbox selection
            foreach (var weapon in Weapons)
            {
                weapon.IsSelected = false;
            }

            new MessageWindow("Deleted weapons successfully!", "Success").ShowDialog();
            this.Show();
        }

        private void buttonNewEntry_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new NewEntry().ShowDialog();
            this.Show();
        }

        private void SignOut(object sender, RoutedEventArgs e) => Login();

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        // Window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_Closing(object sender, CancelEventArgs e) => SaveWeapons();

        // Helper
        public static void SaveWeapons()
        {
            Weapons.ToList().ForEach(weapon => weapon.IsSelected = false);
            serializer.SerializeObject<BindingList<Weapon>>(Weapons, "weapons.xml");
        }

        public static List<string> GetWeaponNames() => Weapons.Select(x => x.Name).ToList();

        private void HideCheckboxColumn()
        {
            // Find the checkbox column by its DataGridTemplateColumn.CellTemplate
            foreach (var column in weaponsDataGrid.Columns)
            {
                if (column is DataGridTemplateColumn templateColumn)
                {
                    var checkBox = (templateColumn.CellTemplate.LoadContent() as CheckBox);
                    if (checkBox != null)
                    {
                        // Hide the column
                        templateColumn.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void ShowCheckboxColumn()
        {
            // Find the checkbox column by its DataGridTemplateColumn.CellTemplate
            foreach (var column in weaponsDataGrid.Columns)
            {
                if (column is DataGridTemplateColumn templateColumn)
                {
                    var checkBox = (templateColumn.CellTemplate.LoadContent() as CheckBox);
                    if (checkBox != null)
                    {
                        // Show the column
                        templateColumn.Visibility = Visibility.Visible;
                    }
                }
            }
        }
    }
}

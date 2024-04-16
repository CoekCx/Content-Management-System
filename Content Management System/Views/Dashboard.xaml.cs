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

namespace Content_Management_System.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private DataIO serializer = new DataIO();
        public static BindingList<Weapon> weapons { get; set; }
        public Dashboard()
        {
            weapons = serializer.DeSerializeObject<BindingList<Weapon>>("weapons.xml");
            if (weapons == null)
            {
                weapons = new BindingList<Weapon>();
            }

            DataContext = this;
            InitializeComponent();

            if (MainWindow.LoggedInUser.Type == Models.UserType.Visitor) ButtonsPanel.Visibility = Visibility.Collapsed;

            Icon = new BitmapImage(new Uri(ContentManager.GetImagePath("cms.ico"))); // Set app icon
        }

        // Buttons
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonNewEntry_Click(object sender, RoutedEventArgs e)
        {

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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            serializer.SerializeObject<BindingList<Weapon>>(weapons, "weapons.xml");
        }
    }
}

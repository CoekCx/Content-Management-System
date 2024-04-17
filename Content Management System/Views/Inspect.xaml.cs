using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Content_Management_System.Models;
using Content_Management_System.Services;

namespace Content_Management_System.Views
{
    /// <summary>
    /// Interaction logic for Inspect.xaml
    /// </summary>
    public partial class Inspect : Window
    {
        public Weapon InspectWeapon { get; set; }
        public Inspect(Weapon weapon)
        {
            InitializeComponent();
            InspectWeapon = weapon;
            loadDataToView();
        }

        private void loadDataToView()
        {
            textBoxName.Text = InspectWeapon.Name;

            textBoxCredits.Text = InspectWeapon.Credits.ToString();

            if (File.Exists(InspectWeapon.FilePath))
            {
                TextRange range;
                FileStream fStream;

                range = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd);
                fStream = new FileStream(InspectWeapon.FilePath, FileMode.OpenOrCreate);
                range.Load(fStream, System.Windows.DataFormats.Rtf);

                fStream.Close();
            }

            if (File.Exists(InspectWeapon.ImgPath))
            {
                ImagePreview.Source = new BitmapImage(new Uri(InspectWeapon.ImgPath));
            }
            else
            {
                ImagePreview.Source = ContentManager.GetPlaceholderImage();
            }
        }

        // Buttons
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
    }
}

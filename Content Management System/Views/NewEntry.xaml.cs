using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Content_Management_System.Models;
using Content_Management_System.Services;

namespace Content_Management_System.Views
{
    /// <summary>
    /// Interaction logic for NewEntry.xaml
    /// </summary>
    public partial class NewEntry : Window
    {
        public Weapon TempWeapon { get; set; }
        List<double> fontSizes = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        public bool EditingMode { get; set; }
        public string InitWeaponName { get; set; } = "";
        public int WeaponIndex { get; set; }

        public NewEntry()
        {
            InitializeComponent();
            EditingMode = false;
            TempWeapon = new Weapon();
            Icon = ContentManager.GetIcon();

            // Name
            textBoxName.Text = "Enter weapon name";
            textBoxName.Foreground = Brushes.LightSlateGray;
            textBoxName.BorderThickness = new Thickness(1);
            textBoxName.BorderBrush = Brushes.Gray;

            // Credits
            textBoxCredits.Text = "Enter credits amount";
            textBoxCredits.Foreground = Brushes.LightSlateGray;
            textBoxCredits.BorderThickness = new Thickness(1);
            textBoxCredits.BorderBrush = Brushes.Gray;

            // Image
            ImagePreview.Source = ContentManager.GetPlaceholderImage();

            // RTB
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = fontSizes;
        }

        public NewEntry(int weaponIndex)
        {
            InitializeComponent();
            EditingMode = true;
            WeaponIndex = weaponIndex;
            var weapon = Dashboard.Weapons[weaponIndex];
            TempWeapon = new Weapon(weapon);
            InitWeaponName = weapon.Name;
            Icon = ContentManager.GetIcon();
            loadDataToView();

            // RTB
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = fontSizes;
        }

        // Validation
        public bool validate()
        {
            bool result = true;

            // Name
            if(!validateName()) result = false;

            // Credits
            if(!validateCredits()) result = false;

            // Image
            if(!validateImage()) result = false;
            
            // RTB
            if(!validateRTB()) result = false;

            return result;
        }

        private bool validateName()
        {
            var existingNames = Dashboard.GetWeaponNames();

            if (textBoxName.Text.Trim().Equals(String.Empty) || textBoxName.Text.Trim().Equals("Enter weapon name"))
            {
                labelNameError.Content = "Enter valid value into field";
                textBoxName.BorderBrush = Brushes.Red;
                return false;
            }
            else if (existingNames.Where(x => x != InitWeaponName).Contains(textBoxName.Text.Trim()))
            {
                labelNameError.Content = "Weapon with that name already exists";
                textBoxName.BorderBrush = Brushes.Red;
                return false;
            }

            labelNameError.Content = String.Empty;
            textBoxName.BorderBrush = Brushes.Gray;
            TempWeapon.Name = textBoxName.Text.Trim();
            return true;
        }

        private bool validateCredits()
        {
            if (textBoxCredits.Text.Trim().Equals(String.Empty) || textBoxCredits.Text.Trim().Equals("Enter credits amount") || !int.TryParse(textBoxCredits.Text.Trim(), out int x))
            {
                labelCreditsError.Content = "Enter valid value into field";
                textBoxCredits.BorderBrush = Brushes.Red;
                return false;
            }

            labelCreditsError.Content = String.Empty;
            textBoxCredits.BorderBrush = Brushes.Gray;
            TempWeapon.Credits = int.Parse(textBoxCredits.Text.Trim());
            return true;
        }

        private bool validateImage()
        {
            if (!File.Exists(TempWeapon.ImgPath))
            {
                labelImageError.Content = "Invalid image\nselected";
                return false;
            }
            
            labelImageError.Content = String.Empty;
            return true;
        }

        private bool validateRTB()
        {
            string text = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            if (text.Trim().Equals(""))
            {
                labelRTBError.Content = "Enter a valid\ndescription";
                return false;
            }
            
            labelRTBError.Content = "";
            return true;
        }

        // Name
        private void textBoxName_GotFocus(object sender, RoutedEventArgs e)
        {

            if (textBoxName.Text.Trim().Equals("Enter weapon name"))
            {
                textBoxName.Text = "";
                textBoxName.Foreground = Brushes.Black;
            }

        }

        private void textBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            validateName();
            if (textBoxName.Text.Trim().Equals(string.Empty))
            {
                textBoxName.Text = "Enter weapon name";
                textBoxName.Foreground = Brushes.LightSlateGray;
            }
        }

        // Creadits
        private void textBoxCredits_GotFocus(object sender, RoutedEventArgs e)
        {

            if (textBoxCredits.Text.Trim().Equals("Enter credits amount"))
            {
                textBoxCredits.Text = "";
                textBoxCredits.Foreground = Brushes.Black;
            }

        }

        private void textBoxCredits_LostFocus(object sender, RoutedEventArgs e)
        {
            validateCredits();
            if (textBoxCredits.Text.Trim().Equals(string.Empty))
            {
                textBoxCredits.Text = "Enter credits amount";
                textBoxCredits.Foreground = Brushes.LightSlateGray;
            }
        }

        // Image
        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = openFileDialog.FileName; // openFileDialog.FileName returns path to chosen file
                ImagePreview.Source = new BitmapImage(new Uri(selectedPath));
                TempWeapon.ImgPath = selectedPath;
            }
            validateImage();
        }

        // RTB
        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null && !rtbEditor.Selection.IsEmpty)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
            }
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null && !rtbEditor.Selection.IsEmpty)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.SelectedItem);
            }
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var pickedColor = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                if (!rtbEditor.Selection.IsEmpty)
                    rtbEditor.Selection.ApplyPropertyValue(ForegroundProperty, pickedColor);
                btnColor.Background = pickedColor;
            }
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Bold
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            // Italic
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            // Underline
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            // Font family
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;

            // Font size
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.SelectedItem = temp;

            // Color
            temp = rtbEditor.Selection.GetPropertyValue(Inline.ForegroundProperty);
            if (temp == DependencyProperty.UnsetValue)
                btnColor.Background = Brushes.Black;
            else if (!temp.Equals(btnColor.Background))
                btnColor.Background = (Brush)temp;

            // Word count
            TextRange textRange = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            int wordCount = countWords(textRange.Text);
            tbWordCount.Text = $"Words: {wordCount}";

            validateRTB();
        }

        // Buttons
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!validate()) return;
            if (!saveXamlPackage()) return;

            if (EditingMode)
            {
                UpdateWeapons();
                Dashboard.SaveWeapons();
                this.Hide();
                new MessageWindow("Successfully added weapon!", "Success").ShowDialog();
                this.Close();
                return;
            }

            TempWeapon.CreatedOnDate = DateTime.Now;
            Dashboard.Weapons.Add(TempWeapon);
            Dashboard.SaveWeapons();
            this.Hide();
            new MessageWindow("Successfully added weapon!", "Success").ShowDialog();
            this.Close();
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

        // Helper
        private int countWords(string text)
        {
            int wordCount = 0, index = 0;

            // skip whitespace until first word
            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            return wordCount;
        }
        
        private bool saveXamlPackage() // Saves rtb content and the file path
        {
            TempWeapon.SetFilePath();
            string fileName = TempWeapon.FilePath;

            try
            {
                if (fileName != string.Empty)
                {
                    var myFile = File.Create(fileName);
                    myFile.Close();
                    rtbEditor.SelectAll();
                    FileStream file = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                    rtbEditor.Selection.Save(file, System.Windows.DataFormats.Rtf);
                    file.Close();
                    return true;
                }
                else
                {
                    new MessageWindow("An error occured while\nsaving the file!", "Error").ShowDialog();
                    return false;
                }
            }
            catch (Exception)
            {
                new MessageWindow("An error occured while\nsaving the file!", "Error").ShowDialog();
                return false;
            }
        }

        private void loadDataToView()
        {
            textBoxName.Text = TempWeapon.Name;

            textBoxCredits.Text = TempWeapon.Credits.ToString();

            if (validateImage())
            {
                try
                {
                    ImagePreview.Source = new BitmapImage(new Uri(TempWeapon.ImgPath));
                }
                catch
                {
                    labelImageError.Content = "Failed to load\nimage";
                }
            }
            else {
                labelImageError.Content = "Image not found";
            }

            if (File.Exists(TempWeapon.FilePath))
            {
                TextRange range;
                FileStream fStream;

                range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                fStream = new FileStream(TempWeapon.FilePath, FileMode.OpenOrCreate);
                range.Load(fStream, System.Windows.DataFormats.Rtf);

                fStream.Close();
            }
            else
            {
                labelRTBError.Content = "Couldn't load\ndescription";
            }
        }

        private void UpdateWeapons()
        {
            Dashboard.Weapons.RemoveAt(WeaponIndex);
            Dashboard.Weapons.Insert(WeaponIndex, TempWeapon);
        }
    }
}

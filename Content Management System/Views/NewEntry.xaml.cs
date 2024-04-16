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

        public NewEntry()
        {
            InitializeComponent();
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

        // Validation method
        public bool validate()
        {
            bool result = true;

            // Name
            if (textBoxName.Text.Trim().Equals(String.Empty) || textBoxName.Text.Trim().Equals("Enter weapon name"))
            {
                result = false;
                labelNameError.Content = "Enter valid value into field";
                textBoxName.BorderBrush = Brushes.Red;
            }
            else
            {
                labelNameError.Content = String.Empty;
                textBoxName.BorderBrush = Brushes.Gray;
            }

            // Credits
            if (textBoxCredits.Text.Trim().Equals(String.Empty) || textBoxCredits.Text.Trim().Equals("Enter credits amount") || !int.TryParse(textBoxCredits.Text.Trim(), out int x))
            {
                result = false;
                labelCreditsError.Content = "Enter valid value into field";
                textBoxCredits.BorderBrush = Brushes.Red;
            }
            else
            {
                labelCreditsError.Content = String.Empty;
                textBoxCredits.BorderBrush = Brushes.Gray;
            }

            // Image
            if (!File.Exists(TempWeapon.ImgPath))
            {
                result = false;
                labelImageError.Content = "Invalid image\nselected";
            }
            else
            {
                labelImageError.Content = String.Empty;
            }

            // RTB
            string text = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            if (text.Trim().Equals(""))
            {
                result = false;
                labelRTBError.Content = "Enter a valid\ndescription";
            }
            else
            {
                labelRTBError.Content = "";
            }

            return result;
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
            validate();
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
            validate();
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
            validate();
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

            validate();
        }

        // Buttons
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (validate()) Close();
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
    }
}

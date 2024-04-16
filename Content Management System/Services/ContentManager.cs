using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Content_Management_System.Services
{
    public static class ContentManager
    {
        private static string outPutDirectory { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
        private static string assetsFolderPath { get; set; } = Path.Combine(outPutDirectory.Remove(outPutDirectory.Length - 25), "Assets\\");
        private static string imagesFolderPath { get; set; } = Path.Combine(assetsFolderPath, "Images\\");
        private static string descriptionsFolderPath { get; set; } = Path.Combine(assetsFolderPath, "Descriptions\\");

        public static string GetImagePath(string imgName) => Path.Combine(imagesFolderPath, imgName);
        public static BitmapImage GetImage(string imgName) => new BitmapImage(new Uri(GetImagePath(imgName)));
        public static BitmapImage GetPlaceholderImage() => new BitmapImage(new Uri(GetImagePath("placeholder.png")));
        public static BitmapImage GetIcon() => new BitmapImage(new Uri(GetImagePath("cms.ico")));
        public static string GetDescriptionPath(string descName) => Path.Combine(descriptionsFolderPath, descName);
    }
}

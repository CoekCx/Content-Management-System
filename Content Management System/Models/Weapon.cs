using Content_Management_System.Services;
using System;
using System.IO;

namespace Content_Management_System.Models
{
    public class Weapon
    {
        public string Name { get; set; }
        public int Credits { get; set; }
        public string ImgPath { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedOnDate { get; set; }

        public Weapon()
        {
            Name = "";
            Credits = 0;
            ImgPath = "";
            FilePath = "";
            CreatedOnDate = new DateTime();
        }
        public Weapon(string name, int credits, string imgPath, DateTime createdOnDate)
        {
            Name = name;
            Credits = credits;
            ImgPath = imgPath;
            FilePath = GetFilePath();
            CreatedOnDate = createdOnDate;
        }
        public Weapon(Weapon weapon)
        {
            Name = weapon.Name;
            Credits = weapon.Credits;
            ImgPath = weapon.ImgPath;
            FilePath = weapon.FilePath;
            CreatedOnDate = weapon.CreatedOnDate;
        }

        public void copyWeapon(Weapon weapon)
        {
            Name = weapon.Name;
            Credits = weapon.Credits;
            ImgPath = weapon.ImgPath;
            FilePath = weapon.FilePath;
            CreatedOnDate = weapon.CreatedOnDate;
        }
        public void SetFilePath()
        {
            // Generate file path based on name
            FilePath = ContentManager.GetDescriptionPath($"{Name}.rtf");

            // Create a Uri object from the file path
            Uri uri = new Uri(FilePath);

            // Get the local file system path without the "file://" prefix
            FilePath = uri.LocalPath;
        }

        private string GetFilePath()
        {
            return ContentManager.GetDescriptionPath($"{Name}.rtf");
        }
    }
}

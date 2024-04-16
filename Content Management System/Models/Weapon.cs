using Content_Management_System.Services;
using System;

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
        public Weapon(string name, int credits, string imgPath, string filePath, DateTime createdOnDate)
        {
            Name = name;
            Credits = credits;
            ImgPath = imgPath;
            FilePath = filePath;
            CreatedOnDate = createdOnDate;
        }
    }
}

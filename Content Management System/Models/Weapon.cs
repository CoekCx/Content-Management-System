using System;

namespace Content_Management_System.Models
{
    public class Weapon
    {
        public string Name { get; set; }
        public int Credits { get; set; }
        public WeaponType Type { get; set; }
        public string ImgPath { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedOnDate { get; set; }

        public Weapon(string name, int credits, WeaponType type, string imgPath, string filePath, DateTime createdOnDate)
        {
            Name = name;
            Credits = credits;
            Type = type;
            ImgPath = imgPath;
            FilePath = filePath;
            CreatedOnDate = createdOnDate;
        }
    }

    public enum WeaponType
    {
        Sidearm,
        SMG,
        Shotgun,
        Rifle,
        Sniper,
        MachineGun
    }
}

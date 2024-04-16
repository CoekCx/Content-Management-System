using Content_Management_System.Services;

namespace Content_Management_System.Models
{
    public class User
    {
        public string Name { get; set; }
        public byte[] Token { get; set; }

        public User()
        {
            Name = "";
            Token = new byte[] { };
        }
        public User(string name, string password)
        {
            Name = name;
            Token = Autentificator.Hash(password);
        }
    }
}

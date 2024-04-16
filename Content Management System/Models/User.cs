using Content_Management_System.Services;

namespace Content_Management_System.Models
{
    public class User
    {
        public string Name { get; set; }
        public byte[] Token { get; set; }
        public UserType Type { get; set; }

        public User()
        {
            Name = "";
            Token = new byte[] { };
            Type = UserType.Visitor;
        }
        public User(string name, string password, UserType type)
        {
            Name = name;
            Token = Autentificator.Hash(password);
            Type = type;
        }
    }

    public enum UserType
    {
        Admin,
        Visitor
    }
}

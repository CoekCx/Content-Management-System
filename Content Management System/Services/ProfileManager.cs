using Content_Management_System.Models;
using Content_Management_System.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Content_Management_System.Services
{
    /// <summary>
    /// The `ProfileManager` class is responsible for managing users' profiles, including loading and saving profiles.
    /// </summary>
    public static class ProfileManager
    {
        /// <summary>
        /// Loads the users' profiles from the `profiles.json` file.
        /// If the file doesn't exist, it creates a new list of profiles and saves it to the file.
        /// In case of any error while loading the profiles, it logs a warning and tryes to read the profiles in old format.
        /// </summary>
        public static List<User> LoadUsers()
        {
            // If not profiles file, try to reset and return the default users
            if (!File.Exists("profiles.json"))
            {
                var resetSuccesfull = ResetUsers();

                if (resetSuccesfull)
                    return LoadUsers();
                return new List<User> { };
            }

            try
            {
                using (StreamReader r = new StreamReader("profiles.json"))
                {
                    string json = r.ReadToEnd();
                    var Users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                    return Users;
                }
            }
            // If a problem occurs while reading the data, try to reset and return the default users
            catch (Exception ex)
            {
                new MessageWindow(ex.Message, "Error").ShowDialog();

                var resetSuccesfull = ResetUsers();

                if (resetSuccesfull)
                    return LoadUsers();
                return new List<User> { };
            }
        }

        /// <summary>
        /// Resets the users profiles and saves an empty list of profiles to the `profiles.json` file.
        /// </summary>
        private static bool ResetUsers()
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter("profiles.json"))
                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    var serializer = new JsonSerializer();
                    var defaultUsers = new List<User>
                    {
                        new User("Admin", "1234"),
                        new User("Visitor", "1234")
                    };
                    serializer.Serialize(jsonWriter, defaultUsers);
                }

                return true;
            }
            catch (Exception ex)
            {
                new MessageWindow(ex.Message, "Error").ShowDialog();
                return false;
            }
        }
    }
}

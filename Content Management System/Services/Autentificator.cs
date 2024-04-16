using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Content_Management_System.Models;

namespace Content_Management_System.Services
{
    internal static class Autentificator
    {
        /// <summary>
        /// This method checks if the given password matches the user's stored token.
        /// The password is converted to an MD5 hash and compared to the user's token.
        /// If they match, the method returns true, otherwise false.
        /// </summary>
        public static bool Autentificate(User user, string password)
        {
            string sSourceData = password;
            byte[] tmpSource;
            byte[] tmpHash;
            byte[] userHash = user.Token;

            //Create a byte array from source data
            tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);

            //Compute hash based on source data
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            var psw = ByteArrayToString(tmpHash);

            bool areEqual = false;
            if (userHash.Length == tmpHash.Length)
            {
                int i = 0;
                while ((i < userHash.Length) && (userHash[i] == tmpHash[i]))
                {
                    i += 1;
                }
                if (i == userHash.Length)
                {
                    areEqual = true;
                }
            }

            return areEqual;
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        /// <summary>
        /// Computes the hash value of a given string using the MD5 algorithm.
        /// </summary>
        /// <param name="text">The input string to be hashed.</param>
        /// <returns>A byte array representing the hash value of the input string.</returns>
        public static byte[] Hash(string text)
        {
            return new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
        }
    }
}

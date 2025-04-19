using System.Security.Cryptography;
using System.Text;

namespace T1PR2_Client.Tools
{
    /// <summary>
    /// This class contains security-related utility methods.
    /// It provides methods for encrypting passwords and other sensitive data.
    /// It is used to enhance the security of the application by providing encryption functionalities.
    /// It includes methods for hashing passwords using SHA256 algorithm.
    /// </summary>
    public static class SecurityTools
    {
        /// <summary>
        /// Encrypts a password using SHA256 hashing algorithm.
        /// This method takes a password as input and returns the hashed password as a string.
        /// It uses SHA256 algorithm to create a hash of the password.
        /// The hashed password is a fixed-length string that represents the original password.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncriptPassword(string password)
        {
            SHA256 hash = SHA256.Create();
            return hash.ComputeHash(Encoding.UTF8.GetBytes(password)).ToString();
        }
    }
}

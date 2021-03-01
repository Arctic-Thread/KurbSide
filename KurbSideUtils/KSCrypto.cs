using System;
using System.Security.Cryptography;
using System.Text;

namespace KurbSideUtils
{
    /// <summary>
    /// Less complicated (and slightly less secure) utilities for 
    /// passwords used internally in the application. The more secure
    /// account password is still required.
    /// </summary>
    /// <remarks>
    /// Seth VanNiekerk
    /// TODO Re-visit in future
    /// </remarks>
    class KSCrypto
    {
        /// <summary>
        /// Create a SHA256 hash from a string
        /// </summary>
        /// <param name="userInputRaw">raw string to be hashed</param>
        /// <returns>SHA256 encoded hash</returns>
        public static string HashPassword(string userInputRaw)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(userInputRaw)));
        }

        /// <summary>
        /// Compares / hashes input against already hashed input.
        /// Do not use to compare ASP.Net passwords, there are built in methods for this
        /// </summary>
        /// <param name="userInputRaw">raw input string to be compared</param>
        /// <param name="hashedPassword">already hashed input to be compared against</param>
        /// <returns>true=match/false=no match</returns>
        public static bool ComparePassword(string userInputRaw, string hashedPassword)
        {
            return HashPassword(userInputRaw) == hashedPassword;
        }
    }
}

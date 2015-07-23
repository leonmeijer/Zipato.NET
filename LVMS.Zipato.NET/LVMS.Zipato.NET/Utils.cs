using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLCrypto;

namespace LVMS.Zipato
{
    internal class Utils
    {
        internal static string GetToken(string password, string nonce)
        {
            return CalculateSha1Hash(string.Format("{0}{1}", nonce, CalculateSha1Hash(password)));
        }

        private static string CalculateSha1Hash(string input)
        {            
            // step 1, calculate MD5 hash from input
            var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = hasher.HashData(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        ///// <summary>
        ///// Compute hash for string encoded as UTF8
        ///// </summary>
        ///// <param name="s">String to be hashed</param>
        ///// <returns>40-character hex string</returns>
        //internal static string SHA1HashStringForUTF8String(string s)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(s);

        //    var sha1 = SHA1.Create();
        //    byte[] hashBytes = sha1.ComputeHash(bytes);

        //    return HexStringFromBytes(hashBytes);
        //}

        ///// <summary>
        ///// Convert an array of bytes to a string of hex digits
        ///// </summary>
        ///// <param name="bytes">array of bytes</param>
        ///// <returns>String of hex digits</returns>
        //internal static string HexStringFromBytes(byte[] bytes)
        //{
        //    var sb = new StringBuilder();
        //    foreach (byte b in bytes)
        //    {
        //        var hex = b.ToString("x2");
        //        sb.Append(hex);
        //    }
        //    return sb.ToString();
        //}
    }
}

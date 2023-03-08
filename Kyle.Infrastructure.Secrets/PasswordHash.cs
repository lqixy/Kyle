using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Infrastructure.Secrets
{
    public class PasswordHash
    {
        public static string CreateHash(string password)
        {
            RNGCryptoServiceProvider provider = new();
            var array = new byte[24];
            provider.GetBytes(array);
            var inArray = PBKDF2(password, array, 1000, 24);
            return 1000 + ":" + Convert.ToBase64String(array) + ":" + Convert.ToBase64String(inArray);
        }


        public static bool ValidatePassword(string password, string correctHash)
        {
            var separator = new char[1] { ':' };
            var array = correctHash.Split(separator);
            var iterations = int.Parse(array[0]);
            var salt = Convert.FromBase64String(array[1]);
            var array2 = Convert.FromBase64String(array[2]);
            var b = PBKDF2(password, salt, iterations, array2.Length);
            return SlowEquals(array2, b);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint num = (uint)(a.Length ^ b.Length);
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                num |= (uint)(a[i] ^ b[i]);
            }

            return num == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int interations, int outputBytes)
        {
            return Rfc2898DeriveBytes.Pbkdf2(password, salt, interations, HashAlgorithmName.SHA256, outputBytes);
        }

    }
}

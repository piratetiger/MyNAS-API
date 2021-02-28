using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyNAS.Service.Helper
{
    public static class EncryptHelper
    {
        private const string strPermutation = "__MyNAS$";

        private const Int32 bytePermutation1 = 0x19;
        private const Int32 bytePermutation2 = 0x59;
        private const Int32 bytePermutation3 = 0x17;
        private const Int32 bytePermutation4 = 0x41;

        public static string Encrypt(string data, string permutation = strPermutation)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data), permutation));
        }

        public static string Decrypt(string data, string permutation = strPermutation)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data), permutation));
        }

        private static byte[] Encrypt(byte[] date, string permutation = strPermutation)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(permutation,
            new byte[] { bytePermutation1,
                         bytePermutation2,
                         bytePermutation3,
                         bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(date, 0, date.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        private static byte[] Decrypt(byte[] date, string permutation = strPermutation)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(permutation,
            new byte[] { bytePermutation1,
                         bytePermutation2,
                         bytePermutation3,
                         bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(date, 0, date.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
    }
}
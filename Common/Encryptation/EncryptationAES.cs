using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Encryptation
{
    public static class EncryptationAES
    {
        public static string GenerarKey()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Encriptar(string texto, string key)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = 256;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.ECB;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.Key = ASCIIEncoding.UTF8.GetBytes(key);
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(texto);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherText);
        }

        public static string Desencriptar(string texto, string key)
        {
            try
            {
                RijndaelManaged aesEncryption = new RijndaelManaged();
                aesEncryption.KeySize = 256;
                aesEncryption.BlockSize = 128;
                aesEncryption.Mode = CipherMode.ECB;
                aesEncryption.Padding = PaddingMode.PKCS7;
                aesEncryption.Key = ASCIIEncoding.UTF8.GetBytes(key);
                ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
                byte[] encryptedBytes = Convert.FromBase64CharArray(texto.ToCharArray(), 0, texto.Length);
                return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

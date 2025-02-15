using System.Security.Cryptography;
using System.Text;

namespace ProjectJWTeCommerce.Utility
{
    
        public class EncodeDecode
        {
            private static string key = "12345678901234567890123456789012";
            public static string Encrypt(string text)
            {

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32)); // Ensure 32-byte key
                    aes.IV = new byte[16]; // Default IV (should be stored securely)

                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(text);
                        }
                        return (Convert.ToBase64String(ms.ToArray()));
                    }
                }



            }

            public static string Decrypt(string encryptedText)
            {

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
                    aes.IV = new byte[16];

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

        }
    
}

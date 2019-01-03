using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegoCrypto
{
    public class AESdecrypter
    {
        private FormMain mainForm;
        public AESdecrypter(FormMain mainForm)
        {
            this.mainForm = mainForm;
        }

        public byte[] DecryptedBytes(byte[] encodedBytes, byte[] encryptionKey, byte[] IV)
        {
            byte[] bytes = new byte[0];
            
            using (Aes aesAlg = Aes.Create("AES"))
            {
                aesAlg.BlockSize = mainForm.UserPrefs.BlockSize;
                aesAlg.KeySize = mainForm.UserPrefs.KeySize;
                aesAlg.Key = encryptionKey;
                aesAlg.IV = IV;
                aesAlg.Mode = mainForm.UserPrefs.CipherMode;
                aesAlg.Padding = PaddingMode.Zeros;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(encryptionKey, IV);

                // Create the streams used for decryption.
                using (MemoryStream memoryStreamDecrypt = new MemoryStream(encodedBytes))
                {
                    try
                    {
                        using (CryptoStream cryptoStreamDecrypt = new CryptoStream(memoryStreamDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            cryptoStreamDecrypt.Read(encodedBytes, 0, encodedBytes.Count());
                            cryptoStreamDecrypt.Close();
                        }
                        bytes = memoryStreamDecrypt.ToArray();
                    }
                    catch (CryptographicException e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }

            if (bytes.Length != encodedBytes.Length)
            {
                Console.WriteLine("Decoded bytes: " + bytes.Length + " from " + encodedBytes.Length + " !!! ");
            }
            return bytes;
        }
    }
}

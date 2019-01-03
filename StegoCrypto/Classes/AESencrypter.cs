using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    public class AESencrypter
    {
        // Fields
        private List<byte> rawFileWithHeader;
        private byte[] initializationVector;
        private byte[] encryptedFile;
        private FormMain mainForm;

        public byte[] InitializationVector
        {
            get
            {
                return initializationVector;
            }
        }

        // Constructor
        public AESencrypter(byte[] fileHeader, byte[] fileContents, FormMain mainForm)
        {
            PopulateRawFileByteList(fileHeader, fileContents);
            this.mainForm = mainForm;
        }

        // Concatenate the file contents onto the header.
        private void PopulateRawFileByteList(byte[] fileHeader, byte[] fileContents)
        {
            rawFileWithHeader = new List<byte>();
            foreach (byte b in fileHeader)
            {
                rawFileWithHeader.Add(b);
            }

            foreach (byte b in fileContents)
            {
                rawFileWithHeader.Add(b);
            }
        }

        public byte[] EncryptBytes()
        {
            try
            {
                // Create a new instance of the Aes class.  
                // This generates a new key and initialization vector (IV).
                using (Aes myAes = Aes.Create())
                {
                    // Encrypt the string to an array of bytes.
                    myAes.BlockSize = mainForm.UserPrefs.BlockSize;
                    myAes.KeySize = mainForm.UserPrefs.KeySize;
                    myAes.Key = mainForm.EncryptionKey;
                    myAes.Mode = mainForm.UserPrefs.CipherMode;
                    myAes.Padding = PaddingMode.Zeros;

                    initializationVector = myAes.IV;

                    Console.WriteLine("AES Encrypter created the folowing IV.");
                    foreach (byte b in initializationVector)
                    {
                        Console.WriteLine("IV byte: " + b.ToString());
                    }

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream memoryStreamEncrypt = new MemoryStream())
                    {
                        using (CryptoStream cryptoStreamEncrypt = new CryptoStream(memoryStreamEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStreamEncrypt.Write(rawFileWithHeader.ToArray(), 0, rawFileWithHeader.Count);
                        }
                        encryptedFile = memoryStreamEncrypt.ToArray();
                    }
                }

                // Return the encrypted bytes from the memory stream.
                Console.WriteLine("Encrypted file is " + encryptedFile.Length + " bytes long.");
                return encryptedFile;
            
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return null;
            }
        }
    }
}

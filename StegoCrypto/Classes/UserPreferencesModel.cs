using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    class UserPreferences
    {
        // The private fields
        private byte[] salt;
        private int iterations;
        private int blockSize;
        private int keySize;
        private CipherMode cipherMode;
        private string[] prefsSaveFile;

        // The public properties
        public byte[] Salt
        {
            get
            {
                return salt;
            }

            set
            {
                salt = value;
            }
        }

        public int Iterations
        {
            get
            {
                return iterations;
            }

            set
            {
                iterations = value;
            }
        }

        public int BlockSize
        {
            get
            {
                return blockSize;
            }

            set
            {
                blockSize = value;
            }
        }

        public int KeySize
        {
            get
            {
                return keySize;
            }

            set
            {
                keySize = value;
            }
        }

        public CipherMode CipherMode
        {
            get
            {
                return cipherMode;
            }

            set
            {
                cipherMode = value;
            }
        }

        // Default Constructor
        public UserPreferences()
        {
            salt = new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 };
            iterations = 300;
            blockSize = 128;
            keySize = 128;
            cipherMode = CipherMode.CBC;
        }

        // Constructor for custom settings.
        public UserPreferences(byte[] salt, int iterations, int blockSize, int keySize, CipherMode cipherMode)
        {
            this.salt = salt;
            this.iterations = iterations;
            this.blockSize = blockSize;
            this.keySize = keySize;
            this.cipherMode = cipherMode;
        }

        public void LoadPrefs()
        {
            if (File.Exists("preferences.dat"))
            {
                prefsSaveFile = File.ReadAllLines("preferences.dat");

                for (int i = 0; i < prefsSaveFile.Length; i++)
                {
                    //Console.WriteLine(prefsSaveFile[i]);
                    switch (i)
                    {
                        case 0:
                            string[] saltBytes = prefsSaveFile[i].Split('|');
                            //Console.WriteLine("Found " + saltBytes.Length + " salt bytes in file.");

                            for (int j = 0; j < saltBytes.Length - 1; j++)
                            {
                                // Set the active salt from file.
                                salt[j] = Byte.Parse(saltBytes[j]);
                                //Console.WriteLine("Salt byte from file: " + saltBytes[j] + ". Parsed byte: " + salt[j]);
                            }
                            break;
                        case 1:
                            this.iterations = int.Parse(prefsSaveFile[i]);
                            break;
                        case 2:
                            this.blockSize = int.Parse(prefsSaveFile[i]);
                            break;
                        case 3:
                            this.keySize = int.Parse(prefsSaveFile[i]);
                            break;
                        case 4:
                            string mode = prefsSaveFile[i].ToString();
                            switch (mode)
                            {
                                case "CBC":
                                    this.cipherMode = CipherMode.CBC;
                                    break;
                                case "CFB":
                                    this.cipherMode = CipherMode.CFB;
                                    break;
                                case "CTS":
                                    this.cipherMode = CipherMode.CTS;
                                    break;
                                case "ECB":
                                    this.cipherMode = CipherMode.ECB;
                                    break;
                                case "OFB":
                                    this.cipherMode = CipherMode.OFB;
                                    break;
                            }
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Could not finde preferences files.");
            }
            
        }

        public void SavePrefs()
        {
            Console.WriteLine("Writing preferences file.");

            // Get settings from all controls.

            string saltBytes = "";

            foreach (byte b in salt)
            {
                saltBytes += b.ToString() + "|";
            }

            string[] lines = { saltBytes.ToString(), iterations.ToString(), blockSize.ToString(), keySize.ToString(), cipherMode.ToString() };
            File.WriteAllLines("preferences.dat", lines);

        }
    }
}

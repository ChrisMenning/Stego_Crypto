using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The Password Handler accepts a password string and returns an encryption key as a bit array.
    /// Some code from Stack Overflow: https://stackoverflow.com/questions/17195969/generating-aes-256-bit-key-value
    /// </summary>
    public class PasswordHandler
    {
        /// <summary>
        /// A reference to the main form.
        /// </summary>
        private FormMain main;

        /// <summary>
        /// The standard salt this program will always use.
        /// </summary>
        private byte[] Salt;

        /// <summary>
        /// A string for temporarily storing the password.
        /// </summary>
        private string password;

        /// <summary>
        /// A byte array for temporarily storing the encryption key.
        /// </summary>
        private byte[] encryptionKey;

        /// <summary>
        /// Initializes a new instance of the PasswordHandler class.
        /// </summary>
        /// <param name="pw">The password String</param>
        /// <param name="main">A reference to the main form</param>
        public PasswordHandler(string pw, FormMain main)
        {
            Salt = main.UserPrefs.Salt;

            this.main = main;
            this.Password = pw;
            this.encryptionKey = this.CreateKey(pw);
            this.main.EncryptionKey = this.encryptionKey;

            Console.WriteLine("PWHandler created the following Encryption Key:");
            foreach (byte b in this.main.EncryptionKey)
            {
                Console.WriteLine("Key bye:" + b.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                this.password = value;
            }
        }

        /// <summary>
        /// Gets or sets the EncryptionKey
        /// </summary>
        public byte[] EncryptionKey
        {
            get
            {
                return this.encryptionKey;
            }

            set
            {
                this.encryptionKey = value;
            }
        }

        /// <summary>
        /// Returns a byte array to be used as encryption key.
        /// </summary>
        /// <param name="password">The password as a string</param>
        /// <returns>A byte array</returns>
        public byte[] CreateKey(string password)
        {
            int Iterations = main.UserPrefs.Iterations;
            var keyGenerator = new Rfc2898DeriveBytes(password, Salt, Iterations);
            return keyGenerator.GetBytes(main.UserPrefs.KeySize / 8);
        }
    }
}

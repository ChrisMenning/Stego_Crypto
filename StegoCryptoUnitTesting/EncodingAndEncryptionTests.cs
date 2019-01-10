using System.Drawing;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoCrypto;
using System.Diagnostics;

namespace StegoCryptoUnitTesting
{
    [TestClass]
    public class EncodingAndEncryptionTests
    {
        [TestMethod]
        public void TestBitmapEncoder()
        {
            // ARRANGE
            BitmapEncoder bmpE = new BitmapEncoder();
            byte[] file = new byte[] { 10, 249, 12, 1, 180, 29, 2, 78, 45, 12, 12, 13, 69, 45, 78, 111 };
            byte[] IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            // ACT
            Bitmap encBMP;
            Task.Run(async () => { encBMP = await bmpE.EncodedBitmap(file, IV);
                // ASSERT
                Assert.AreNotEqual(bmpE, encBMP);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestBMPEncoderAndBMPDecoder()
        {
            // ARRANGE
            BitmapEncoder bmpE = new BitmapEncoder();
            byte[] file = new byte[] { 10, 249, 12, 1, 180, 29, 2, 78, 45, 12, 12, 13, 69, 45, 78, 111 };
            byte[] IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            byte[] bothTogether = new byte[file.Length + IV.Length];
            for (int i = 0; i < IV.Length; i++)
            {
                bothTogether[i] = IV[i];
            }

            for (int i = 0; i < file.Length; i++)
            {
                bothTogether[i + IV.Length] = file[i];
            }

            BitmapDecoder bmpD = new BitmapDecoder(false);
            Bitmap encBMP;
            byte[] bytesFromImage;
            byte[] onlyRelevantBytes;
            // ACT

            Task.Run(async () =>
            {
                encBMP = await bmpE.EncodedBitmap(file, IV);
                bytesFromImage = await bmpD.BytesFromImage(encBMP);

                // Trim excess off end of bytesFromImage.
                onlyRelevantBytes = new byte[bothTogether.Length];
                for (int i = 0; i < bothTogether.Length; i++)
                {
                    onlyRelevantBytes[i] = bytesFromImage[i];
                }
                Trace.WriteLine("XXXXX Comparing " + onlyRelevantBytes.Length + "|" + bothTogether.Length);
                // ASSERT
                for (int i = 0; i < bothTogether.Length; i++)
                {
                    Assert.AreEqual(onlyRelevantBytes[i], bothTogether[i]);

                }

            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TestEncryption()
        {
            // ARRANGE
            // Create a new mainform which also creates a new UserPreferences object, where encryption settings are stored.
            FormMain mainForm = new FormMain();
            PasswordHandler pwh = new PasswordHandler("password", mainForm);
            FileInformation fi = new FileInformation();
            AESencrypter aesEnc = new AESencrypter(fi.GenerateFileInfoHeader(), fi.FileContents, mainForm);
            // ACT
            byte[] encryptedFile = aesEnc.EncryptBytes();
            // ASSERT
            Assert.AreNotEqual(fi.FileContents, encryptedFile);
        }

        [TestMethod]
        public void TestEncryptionAndDecryption()
        {
            // ARRANGE
            FormMain mainForm = new FormMain();
            PasswordHandler pwh = new PasswordHandler("password", mainForm);
            FileInformation fi = new FileInformation();
            byte[] fileHeader = fi.InfoHeader;

            AESencrypter aesEnc = new AESencrypter(fileHeader, fi.FileContents, mainForm);
            AESdecrypter aesDec = new AESdecrypter(mainForm);

            // ACT
            byte[] encryptedFile = aesEnc.EncryptBytes();
            byte[] decryptedFile = aesDec.DecryptedBytes(encryptedFile, mainForm.EncryptionKey, aesEnc.InitializationVector);
            HeaderParser hp = new HeaderParser();
            byte[] parsedDecrypted = hp.fileContentsWithoutHeader(decryptedFile);

            // ASSERT
            for (int i = 0; i < fi.FileContents.Length; i++)
            {
                Assert.AreEqual(fi.FileContents[i], parsedDecrypted[i]);
            }
           
        }
    }
}

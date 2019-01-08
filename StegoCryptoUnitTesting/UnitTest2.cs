using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoCrypto;

namespace StegoCryptoUnitTesting
{
    [TestClass]
    public class UnitTest2
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
            });
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

            BitmapDecoder bmpD = new BitmapDecoder();

            // ACT
            Bitmap encBMP;
            byte[] bytesFromImage;
            Task.Run(async () => {
                encBMP = await bmpE.EncodedBitmap(file, IV);
                bytesFromImage = await bmpD.BytesFromImage(encBMP);

                // Trim excess off end of bytesFromImage.
                byte[] onlyRelevantBytes = new byte[bothTogether.Length];
                for (int i = 0; i < bothTogether.Length; i++)
                {
                    onlyRelevantBytes[i] = bytesFromImage[i];
                }

                // ASSERT
                Assert.AreEqual(onlyRelevantBytes, bothTogether);
            });
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
            Trace.Write("test");
            // ARRANGE
            FormMain mainForm = new FormMain();
            PasswordHandler pwh = new PasswordHandler("password", mainForm);
            FileInformation fi = new FileInformation();
            byte[] fileHeader = fi.InfoHeader;
            Trace.WriteLine("Raw file plus header is " + (fileHeader.Length + fi.FileContents.Length) + " bytes long.");

            AESencrypter aesEnc = new AESencrypter(fileHeader, fi.FileContents, mainForm);
            AESdecrypter aesDec = new AESdecrypter(mainForm);
            // ACT
            byte[] encryptedFile = aesEnc.EncryptBytes();
            Trace.WriteLine("Encrypted file is " + encryptedFile.Length + " bytes long.");
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

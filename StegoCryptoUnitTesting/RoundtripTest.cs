using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoCrypto;
using System.Drawing;
using System.Threading.Tasks;

namespace StegoCryptoUnitTesting
{
    [TestClass]
    public class RoundtripTest
    {
        [TestMethod]
        public void TestRoundtrip()
        {
            // ARRANGE
            // =======
            // File
            FileInformation fi = new FileInformation();
            byte[] sourceFile = fi.FileContents;

            // Encryption and Decryption
            FormMain mainForm = new FormMain(); // MainForm also holds user preferences in memory.
            PasswordHandler pwh = new PasswordHandler("password", mainForm);
            AESencrypter aesEnc = new AESencrypter(fi.GenerateFileInfoHeader(), fi.FileContents, mainForm);
            AESdecrypter aesDec = new AESdecrypter(mainForm);
            byte[] encryptedFile;
            byte[] decryptedFile;

            // Bitmap Encoder and Decoder
            BitmapEncoder bmpEncoder = new BitmapEncoder();
            BitmapDecoder bmpDecoder = new BitmapDecoder(false);
            Bitmap encodedBitmap;
            byte[] bytesFromImage;

            // Header Parser
            HeaderParser hp = new HeaderParser();
            byte[] parsedDecrypted;

            // ACT
            // ===
            Task.Run(async () => {
                // Encrypt the file.
                encryptedFile = aesEnc.EncryptBytes();

                // Encode the encrypted file into the bitmap.
                encodedBitmap = await bmpEncoder.EncodedBitmap(encryptedFile, aesEnc.InitializationVector);

                // Retrieve the encrypted bytes back out of the bitmap.
                bytesFromImage = await bmpDecoder.BytesFromImage(encodedBitmap);

                // Decrypt the bytes pulled from the image.
                decryptedFile = aesDec.DecryptedBytes(bytesFromImage, mainForm.EncryptionKey, aesEnc.InitializationVector);

                // Parse the header from the decrypted file.
                parsedDecrypted = hp.fileContentsWithoutHeader(decryptedFile);

                //hp.fileContentsWithoutHeader(aesDec.DecryptedBytes(aesEnc.EncryptBytes(), mainForm.EncryptionKey, aesEnc.InitializationVector))

                // ASSERT
                // ======
                for (int i = 0; i < fi.FileContents.Length; i++)
                {
                    // Assert that the bytes that went in are the same as the bytes that came out.
                    Assert.AreEqual(fi.FileContents[i], parsedDecrypted[i]);
                }
            }).GetAwaiter().GetResult();
        }
    }
}

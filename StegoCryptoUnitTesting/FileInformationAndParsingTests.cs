using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StegoCrypto;

namespace StegoCryptoUnitTesting
{
    [TestClass]
    public class HeaderTests
    {
        [TestMethod]
        public void TestFileNameLengthParsing()
        {
            // ARRANGE
            FileInformation fi = new FileInformation();

            // ACT
            // Parse the filename length from the header.
            byte[] NameLength = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                NameLength[i] = fi.InfoHeader[i];
            }

            int parsedFileNameLength = BitConverter.ToInt32(NameLength, 0);

            // ASSERT
            Assert.AreEqual(fi.FileName.Length, parsedFileNameLength);
        }

        [TestMethod]
        public void TestFileNameParsing()
        {
            // ARRANGE
            FileInformation fi = new FileInformation();
            byte[] NameLength = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                NameLength[i] = fi.InfoHeader[i];
            }

            int parsedFileNameLength = BitConverter.ToInt32(NameLength, 0);

            // ACT
            // Parse the filename from the header.
            byte[] NameAsBytes = new byte[parsedFileNameLength];
            int counter = 0;
            for (int i = 4; i < parsedFileNameLength + 4; i++)
            {
                NameAsBytes[counter] = fi.InfoHeader[i];
                counter++;
            }

            string parsedName = System.Text.Encoding.UTF8.GetString(NameAsBytes);

            // ASSERT
            Assert.AreEqual(parsedName, fi.FileName);
        }

        [TestMethod]
        public void TestFileSizeParsing()
        {
            // ARRANGE
            FileInformation fi = new FileInformation();
            byte[] NameLength = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                NameLength[i] = fi.InfoHeader[i];
            }

            int parsedFileNameLength = BitConverter.ToInt32(NameLength, 0);

            byte[] NameAsBytes = new byte[parsedFileNameLength];
            int counter = 0;
            for (int i = 4; i < parsedFileNameLength + 4; i++)
            {
                NameAsBytes[counter] = fi.InfoHeader[i];
                counter++;
            }

            string parsedName = System.Text.Encoding.UTF8.GetString(NameAsBytes);

            // ACT
            // Parse the number of bytes in the original file from the header.
            int position = (4 + parsedFileNameLength);
            byte[] NumFileBytes = new byte[4];
            counter = 0;
            for (int i = position; i < position + 4; i++)
            {
                NumFileBytes[counter] = fi.InfoHeader[i];
                counter++;
            }
            int parsedFileSize = BitConverter.ToInt32(NumFileBytes, 0);

            // ASSERT
            Assert.AreEqual(parsedFileSize, fi.FileContents.Length);
        }
    }
}

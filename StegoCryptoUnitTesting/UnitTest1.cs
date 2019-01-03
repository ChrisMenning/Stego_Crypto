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
            // Arrange
            FileInformation fi = new FileInformation();

            // Act
            // Parse the filename length from the header.
            Console.WriteLine("Unit testing. Parsing NameLength bytes.");
            byte[] NameLength = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                NameLength[i] = fi.InfoHeader[i];
                Console.WriteLine(i + ": " + NameLength[i].ToString());
            }

            int parsedFileNameLength = BitConverter.ToInt32(NameLength, 0);
            Console.WriteLine("Parsed file name length: " + parsedFileNameLength);

            // Assert
            Assert.AreEqual(fi.FileName.Length, parsedFileNameLength);
        }

        [TestMethod]
        public void TestFileNameParsing()
        {
            // Arrange
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
                Console.WriteLine(i + ": " + NameAsBytes[counter]);
                counter++;
            }

            string parsedName = System.Text.Encoding.UTF8.GetString(NameAsBytes);
            Console.WriteLine("Parsed file name is: " + parsedName);
            Console.WriteLine("Compare to : " + fi.FileName);

            // ASSERT
            Assert.AreEqual(parsedName, fi.FileName);
        }

        [TestMethod]
        public void TestFileSizeParsing()
        {
            // Arrange
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
                Console.WriteLine(i + ": " + NameAsBytes[counter]);
                counter++;
            }

            string parsedName = System.Text.Encoding.UTF8.GetString(NameAsBytes);

            // ACT
            // Parse the number of bytes in the original file from the header.
            Console.WriteLine("Parsing number of bytes in file size from Header.");
            int position = (4 + parsedFileNameLength);
            byte[] NumFileBytes = new byte[4];
            counter = 0;
            for (int i = position; i < position + 4; i++)
            {
                NumFileBytes[counter] = fi.InfoHeader[i];
                Console.WriteLine(i + ": " + NumFileBytes[counter]);
                counter++;
            }
            int parsedFileSize = BitConverter.ToInt32(NumFileBytes, 0);
            Console.WriteLine("Parsed File size is: " + parsedFileSize);
            Console.WriteLine("Compare to: " + fi.FileContents.Length);

            // ASSERT
            Assert.AreEqual(parsedFileSize, fi.FileContents.Length);
        }
    }
}

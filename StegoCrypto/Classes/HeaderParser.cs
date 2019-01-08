using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegoCrypto
{
    public class HeaderParser
    {
        private string fileName;
        private byte[] fileContents;

        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }

        public byte[] FileContents
        {
            get
            {
                return fileContents;
            }

            set
            {
                this.fileContents = value;
            }
        }

        public HeaderParser()
        {

        }

        public byte[] fileContentsWithoutHeader(byte[] decryptedBytesWithHeader)
        {
            byte[] bytes;
            List<byte> fileContents = new List<byte>();
                
            // Position begins 
            int position = 0;
      
            // Get nameLength.
            byte[] NameLength = new byte[4];
            

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    NameLength[i] = decryptedBytesWithHeader[i];
                }

                int parsedFileNameLength = BitConverter.ToInt32(NameLength, 0);

                byte[] NameAsBytes = new byte[parsedFileNameLength];
                int counter = 0;
                for (int i = 4; i < parsedFileNameLength + 4; i++)
                {
                    NameAsBytes[counter] = decryptedBytesWithHeader[i];
                    //Console.WriteLine(i + ": " + NameAsBytes[counter]);
                    counter++;
                }
                // Set filename.
                this.fileName = System.Text.Encoding.UTF8.GetString(NameAsBytes);

                // Parse the number of bytes in the original file from the header.
                Console.WriteLine("Parsing number of bytes in file size from Header.");
                position = (4 + parsedFileNameLength);
                byte[] NumFileBytes = new byte[4];
                counter = 0;
                for (int i = position; i < position + 4; i++)
                {
                    NumFileBytes[counter] = decryptedBytesWithHeader[i];
                    Console.WriteLine(i + ": " + NumFileBytes[counter]);
                    counter++;
                }
                int parsedFileSize = BitConverter.ToInt32(NumFileBytes, 0);
                Console.WriteLine("Parsed File size is: " + parsedFileSize);

                // Get contents.
                position += 4;
                for (int i = position; i < position + parsedFileSize; i++)
                {
                    fileContents.Add(decryptedBytesWithHeader[i]);
                }
                Console.WriteLine("Retrieved " + fileContents.Count() + " bytes.");

                bytes = fileContents.ToArray();

                var str = System.Text.Encoding.Default.GetString(bytes);
                System.IO.File.WriteAllText("Bar.txt", str);

                return bytes;
            }
            catch
            {
                MessageBox.Show("Failed to extract original file. Either password or encryption is wrong.");
                
            }
            return null;
      
            
        }
    }
}

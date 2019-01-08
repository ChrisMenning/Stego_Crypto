using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{ 
    public class FileInformation
    {
        // This object is for generating a file metadata header that will be used for parsing out the file after decryption.
        private string fileName;
        private byte[] fileContents;
        private byte[] infoHeader;

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
                fileContents = value;
            }
        }

        public byte[] InfoHeader
        {
            get
            {
                return infoHeader;
            }

            set
            {
                infoHeader = value;
            }
        }

        // The fileName Length as a byte array.
        public byte[] fileNameLength()
        {
            return BitConverter.GetBytes(fileName.Length);
        }

        // The fileContents Length as a byte array.
        public byte[] fileContentsLength()
        {
            return BitConverter.GetBytes(fileContents.Length); 
        }


        // Default Constructor
        public FileInformation(string fileName, byte[] fileContents)
        {
            this.fileName = fileName;
            this.fileContents = fileContents;
            this.infoHeader = GenerateFileInfoHeader();
        }

        // Constructor for Unit Testing
        public FileInformation()
        {
            this.fileName = "TestDummyFile.tsv";
            this.fileContents = Properties.Resources.Oculus_VRC_Test_Plan_Mobile_1;
            this.infoHeader = GenerateFileInfoHeader();
        }

        // Generate the file info header.
        public byte[] GenerateFileInfoHeader()
        {
            List<byte> header = new List<byte>();

            // First 4 bytes: Add the length of the filename to the header. 
            Console.WriteLine("File Name Length is stored in " + fileNameLength().Length + " bytes. \n they are...");
            foreach (byte b in fileNameLength())
            {
                header.Add(b);
                Console.WriteLine(b.ToString());
            }

            // Next n bytes: Store the filename in the header.
            byte[] fileNameBytes = Encoding.UTF8.GetBytes(fileName);

            Console.WriteLine("FileName is stored in " + fileNameBytes.Count() + " bytes. \n they are...");

            foreach (byte b in fileNameBytes)
            {
                header.Add(b);
                Console.WriteLine(b.ToString());
            }

            // Next 4 bytes: Add the length of the file contents (this corresponds to file size in bytes) to the header.
            Console.WriteLine("An int representing the number of bytes in the file contents is stored in " + fileContentsLength().Length + " bytes. They are...");

            foreach (byte b in fileContentsLength())
            {
                header.Add(b);
                Console.WriteLine(b.ToString());
            }

            // Don't append the actual file contents to the header though.

            // Finally, return the byte[].
            byte[] ret = new byte[header.Count()];
            for (int i = 0; i < ret.Count(); i++)
            {
                ret[i] = header[i];
            }

            header.Clear(); // Dispose of the header list.
            return ret;
        }
    }
}

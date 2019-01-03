using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    public class BitmapDecoder
    {
        private StringBuilder binaryFromImage;

        // Constructor
        public BitmapDecoder()
        {
            binaryFromImage = new StringBuilder();
        }

        // Bytes from Image
        public async Task<byte[]> BytesFromImage(Bitmap encoded)
        {
            byte[] bytes;

            PleaseWait pwForm = new PleaseWait();
            pwForm.progress.Maximum = encoded.Height;
            pwForm.Show();
            pwForm.Refresh();

            Console.WriteLine("Looping through all pixels...");
            int testCounter = 0;
            // Loop through each pixel of the encoded image.
            for (int row = 0; row < encoded.Height; row++)
            {
                for (int column = 0; column < encoded.Width; column++)
                {
                    // Pull the last bit out of each color channel and concatenate them onto the ones and zeros.
                    Color pixelColor = encoded.GetPixel(column, row);
                    await GetLastBitOfEachColorChannel(pixelColor);

                    testCounter++;
                }
                pwForm.progress.Value = row;
                pwForm.Refresh();
            }
            Console.WriteLine("Checked " + testCounter + " pixels, which should be able to store " + testCounter * 4 + " bits, or " + testCounter / 2 + " bytes.");
            Console.WriteLine("Finished looping through pixels. Found " + binaryFromImage.Length + " bits");

            // Convert string of 1s and 0s to byte[].
            Task<byte[]> bytesFromBinaryString = BytesFromBinaryString();
            bytes = await bytesFromBinaryString;

            pwForm.Close();
            return bytes;
        }

        private async Task<bool> GetLastBitOfEachColorChannel(Color color)
        {
            Color pixelColor = color;
            this.binaryFromImage.Append(pixelColor.A % 2);
            this.binaryFromImage.Append(pixelColor.R % 2);
            this.binaryFromImage.Append(pixelColor.G % 2);
            this.binaryFromImage.Append(pixelColor.B % 2);

            return true;
        }

        public async Task<byte[]> BytesFromBinaryString( )
        {
            // Very large files run out of memory on this next line.
            string binary = binaryFromImage.ToString();

            byte[] bytes;
            int numOfBytes = binaryFromImage.Length / 8;
            bytes = new byte[numOfBytes];

            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(binary.Substring(8 * i, 8), 2);
            }
            Console.WriteLine("Converted bits to " + bytes.Count() + " bytes.");

            return bytes;
        }
    }
}

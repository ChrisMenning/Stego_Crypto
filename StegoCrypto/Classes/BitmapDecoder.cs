using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoCrypto
{
    public class BitmapDecoder
    {
        private bool[] binaryFromImage;
        private byte[] rgbValues;

        // Constructor
        public BitmapDecoder()
        {
        }

        // Bytes from Image
        public byte[] BytesFromImage(Bitmap encoded)
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, encoded.Width, encoded.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                encoded.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                encoded.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int numOfbytes = Math.Abs(bmpData.Stride) * encoded.Height;
            rgbValues = new byte[numOfbytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, numOfbytes);

            // Create a place to store the bytes.
            binaryFromImage = new bool[encoded.Height * encoded.Width * 4];
            byte[] byteArray;

            PleaseWait pwForm = new PleaseWait();
            pwForm.progress.Maximum = encoded.Height;
            pwForm.Show();
            pwForm.Refresh();

            int h = encoded.Height;

            Console.WriteLine("Looping through all pixels...");


            for (int i = 0; i < (rgbValues.Length - 4); i +=4)
            {
                binaryFromImage[i] = ToBool(rgbValues[i + 3] % 2);
                binaryFromImage[i + 1] = ToBool(rgbValues[i + 2] % 2);
                binaryFromImage[i + 2] = ToBool(rgbValues[i + 1] % 2);
                binaryFromImage[i + 3] = ToBool(rgbValues[i] % 2);
            }


            // Loop through each pixel of the encoded image.
          //  for (int row = 0; row < h; row++)
          //  {
          //      for (int column = 0; column < encoded.Width; column++)
          //      {
          //          // Pull the last bit out of each color channel and concatenate them onto the ones and zeros.
          //          Color pixelColor = encoded.GetPixel(column, row);
          //          GetLastBitOfEachColorChannel(pixelColor);
          //
          //          testCounter++;
          //      }
          //      pwForm.progress.Value = row;
          //      pwForm.Refresh();
          //  }
            //Console.WriteLine("Checked " + testCounter + " pixels, which should be able to store " + testCounter * 4 + " bits, or " + testCounter / 2 + " bytes.");
            //Console.WriteLine("Finished looping through pixels. Found " + binaryFromImage.Count + " bits");

            // Convert string of 1s and 0s to byte[].
            BitArray ba = new BitArray(binaryFromImage.ToArray());
            byteArray = ToByteArray(ba);

            pwForm.Close();
            // Unlock the bits.
            encoded.UnlockBits(bmpData);
            return byteArray;
        }

    //    private void GetLastBitOfEachColorChannel(Color color)
    //    {
    //        Color pixelColor = color;
    //        this.binaryFromImage.Add(ToBool(pixelColor.A % 2));
    //        this.binaryFromImage.Add(ToBool(pixelColor.R % 2));
    //        this.binaryFromImage.Add(ToBool(pixelColor.G % 2));
    //        this.binaryFromImage.Add(ToBool(pixelColor.B % 2));
    //    }

        public bool ToBool(int value)
        {
            if (value == 1)
                return true;
            else
                return false;
        }

        // Method for converting BitArray to Byte[] by David Brown: http://geekswithblogs.net/dbrown/archive/2009/04/05/convert-a-bitarray-to-byte-in-c.aspx
        public byte[] ToByteArray(BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }
    }
}

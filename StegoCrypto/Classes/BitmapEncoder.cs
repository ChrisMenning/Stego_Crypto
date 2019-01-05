using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StegoCrypto
{
    public class BitmapEncoder
    {
        // the private fields.
        private readonly Bitmap rawBitmap;
        private Bitmap encodedImage;
        private Color pixelColor;
        private Color sanitizedColor;
        private BitArray OnesAndZeros;
        private int newA;
        private int newR;
        private int newG;
        private int newB;

        // The constructor
        public BitmapEncoder(Bitmap rawBitmap)
        {
            this.rawBitmap = rawBitmap;
        }

        // The main method that returns an encoded bitmap.
        public Bitmap EncodedBitmap(byte[] file, byte[] IV)
        {
            PleaseWait pwForm = new PleaseWait();

            // By setting the new encodedImage to being the same as the rawImage, non-encoded pixels do not need to be set again.
            this.encodedImage = new Bitmap(this.rawBitmap);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, this.encodedImage.Width, this.encodedImage.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                this.encodedImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                this.encodedImage.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * this.encodedImage.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Declare a counter.
            int counter = 0;
            int h = this.rawBitmap.Height;
            int w = this.rawBitmap.Width;

            // Prepend IV onto file and convert them both to a BitArray.
            OnesAndZeros = GetOnesAndZeros(IV, file);

            // Update the progress bar.
            pwForm.progress.Maximum = h;
            pwForm.Show();
            pwForm.Refresh();

            // Loop through every row and every column of pixels in the original image.
            for (int row = 0; row < h; row++)
            {
                // Only set a new pixel value until the message length runs out. 
                // There's no sense in setting the pixel value to its original value again.
                if (counter < OnesAndZeros.Length)
                {
                    for (int column = 0; column < w; column++)
                    {
                        {
                            if (counter + 3 < OnesAndZeros.Length)
                            {
                                rgbValues[counter] = (byte)((rgbValues[counter] - (rgbValues[counter] % 2)) + ToInt(OnesAndZeros[counter + 3]));
                                rgbValues[counter + 1] = (byte)((rgbValues[counter + 1] - (rgbValues[counter + 1] % 2)) + ToInt(OnesAndZeros[counter + 2]));
                                rgbValues[counter + 2] = (byte)((rgbValues[counter + 2] - (rgbValues[counter + 2] % 2)) + ToInt(OnesAndZeros[counter + 1]));
                                rgbValues[counter + 3] = (byte)((rgbValues[counter + 3] - (rgbValues[counter + 3] % 2)) + ToInt(OnesAndZeros[counter]));

                                counter += 4;
                            }
                        }
                    }
                    pwForm.progress.Invoke(pwForm.myDelegate);
                    pwForm.progress.Value = row;
                }
                else
                {
                    pwForm.progress.Value = h;
                }
            }

            pwForm.Close();

            if (OnesAndZeros.Length / 8 > (w * h / 2))
                Console.WriteLine("******************************* \n MESSAGE TRUNCATED WHILE WRITING TO BITMAP!!!");

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            this.encodedImage.UnlockBits(bmpData);
            return this.encodedImage;
        }

        // Prepend IV onto File as BitArray
        private BitArray GetOnesAndZeros(byte[] IV, byte[] file)
        {
            List<byte> bothTogether = new List<byte>();
            
            foreach (byte b in IV)
            {
                bothTogether.Add(Reverse(b));
            }

            foreach (byte b in file)
            {
                bothTogether.Add(Reverse(b));
            }

            OnesAndZeros = new BitArray(bothTogether.ToArray());
            return OnesAndZeros;
        }

        // Convert bool to int
        public int ToInt(bool value)
        {
            if (value == true)
                return 1;
            else
                return 0;
        }

        // Reverses bits in a byte https://softwarejuancarlos.com/2013/05/05/byte_bits_reverse/
        public static byte Reverse(byte inByte)
        {
            byte result = 0x00;

            for (byte mask = 0x80; Convert.ToInt32(mask) > 0; mask >>= 1)
            {
                // shift right current result
                result = (byte)(result >> 1);

                // tempbyte = 1 if there is a 1 in the current position
                var tempbyte = (byte)(inByte & mask);
                if (tempbyte != 0x00)
                {
                    // Insert a 1 in the left
                    result = (byte)(result | 0x80);
                }
            }

            return (result);
        }
    }
}

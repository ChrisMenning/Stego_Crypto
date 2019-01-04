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
        private Bitmap rawBitmap;
        private Bitmap encodedImage;
        private Color pixelColor;
        private Color sanitizedColor;
        private BitArray OnesAndZeros;
        private static Mutex mut = new Mutex();
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
            this.encodedImage = this.rawBitmap;

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
                            Color c = MakeNewPixel(column, row, counter);
                            this.encodedImage.SetPixel(column, row, c);
                            counter += 4;
                        }
                    }
                    pwForm.progress.Value = row;
                }
                else
                {
                    pwForm.progress.Value = h;
                }
            }

            Console.WriteLine("Finished stuffing " + OnesAndZeros.Length / 8 + " bytes, including 16 bit IV." );
            pwForm.Close();

            if (OnesAndZeros.Length / 8 > (w * h / 2))
                Console.WriteLine("******************************* \n MESSAGE TRUNCATED WHILE WRITING TO BITMAP!!!");
            return this.encodedImage;
        }

        private Color MakeNewPixel(int col, int r, int count)
        {
            int row = r;
            int column  = col;
            int counter = count;

            // Sanitize the original pixel
            // Get the precise color value of the pixel at this row and this column.
            pixelColor = this.rawBitmap.GetPixel(column, row);

            // Now create a copy of the pixelColor, but with the Least Significant Bit of each color cleared out.
            sanitizedColor = Color.FromArgb(
                    pixelColor.A - (pixelColor.A % 2),
                    pixelColor.R - (pixelColor.R % 2),
                    pixelColor.G - (pixelColor.G % 2),
                    pixelColor.B - (pixelColor.B % 2));

            // Assign each bit from OnesAndZeros to each color channel.
            if (counter + 3 < OnesAndZeros.Length)
            {
                // Encode Nibble to Pixel
                // Next, declare a newR, newG, and newB consisting of the sanitized value, plus a bit from the byteString.
                newA = sanitizedColor.A + ToInt(OnesAndZeros[counter]);
                newR = sanitizedColor.R + ToInt(OnesAndZeros[counter + 1]);
                newG = sanitizedColor.G + ToInt(OnesAndZeros[counter + 2]);
                newB = sanitizedColor.B + ToInt(OnesAndZeros[counter + 3]);

                return Color.FromArgb(newA, newR, newG, newB);
            }
            else
            {
                // Otherwise give the image a pixel equal to the sanitized pixel value.
                return pixelColor;
            }
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

            Console.WriteLine("Ones and zeros is " + OnesAndZeros.Length /8 + " bytes long.");

            // test that IV is encoded correctly.
            string newByte = "";
            for (int j = 0; j < 8; j++)
            {
                newByte += ToInt(OnesAndZeros[j]).ToString();
            }
            Console.WriteLine("OnesAndZeros IV byte: " + newByte);

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

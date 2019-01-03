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
        private Bitmap rawBitmap;
        private Bitmap encodedImage;
        private Color pixelColor;
        private Color sanitizedColor;
        StringBuilder OnesAndZeros;
        private static Mutex mut = new Mutex();
        int newA;
        int newR;
        int newG;
        int newB;

        public BitmapEncoder(Bitmap rawBitmap)
        {
            this.rawBitmap = rawBitmap;
        }

        // Encode bits using lock and unlock. (This is still slow, and inaccurate.)
        public Bitmap EncodedBitMapUsingLocking(byte[] IV, byte[]file)
        {
            // Create a new bitmap.
            Bitmap bmp = new Bitmap(this.rawBitmap);
            using (var g = Graphics.FromImage(this.rawBitmap))
            {
                // Lock the bitmap's bits.  
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData =
                    bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
    
                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;
    
                // Declare an array to hold the bytes of the bitmap.
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                byte[] argbValues = new byte[bytes];
    
                // Copy the RGB values into the array.
                System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, bytes);
                
                PleaseWait pwForm = new PleaseWait();
                pwForm.progress.Maximum = argbValues.Length;
                pwForm.Show();
                pwForm.Refresh();
    
                OnesAndZeros = SBPrependIVOntoFileAsStringBuilder(IV, file);
                int sanitized;
                int bit;
                byte encoded;
                // DO MODIFICATION
                for (int counter = 0; counter < argbValues.Length; counter++)
                {
                    // Sanitize the byte, clearing out it's Least Significant Bit.
                    if (counter < OnesAndZeros.Length)
                    {
                        //Console.WriteLine("Original byte: " + argbValues[counter]);
                        sanitized = argbValues[counter] - (argbValues[counter] % 2);
                        //Console.WriteLine("Sanitized byte: " + sanitized);
                        bit = int.Parse(OnesAndZeros[counter].ToString());
                        encoded = (byte)(sanitized + bit);
                        //Console.WriteLine("encoded byte: " + encoded);
                        argbValues[counter] = encoded;
                    }
                    pwForm.progress.Value = counter;
                }
                pwForm.Close();
    
                Console.WriteLine("Bytes from first two pixels: ");
                for (int i = 0; i < 8; i++)
                {
                    Console.WriteLine("LSB: " + (argbValues[i] % 2));
                }
    
                // Copy the ARGB values back to the bitmap
                System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, bytes);
    
                // Unlock the bits.
                bmp.UnlockBits(bmpData);
    
                // Draw the modified image.
                g.DrawImage(bmp, 0, 0);
            }
    
            return bmp;
        }

        // 
        public async Task<Bitmap> EncodedBitmap(byte[] file, byte[] IV)
        {
            PleaseWait pwForm = new PleaseWait();

            // Convert IV to string of 1s and 0s.
            Task<StringBuilder> OZs = PrependIVOntoFileAsStringBuilder(IV, file);

            // Declare a bitmap for encoding the image. Make it the same width and height as the original.
            //this.encodedImage = new Bitmap(this.rawBitmap.Width, this.rawBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            this.encodedImage = this.rawBitmap;

            // Consider completely different approach using UnlockBits https://docs.microsoft.com/en-us/dotnet/api/system.drawing.bitmap.lockbits?view=netframework-4.7.2
            //  using (var g = Graphics.FromImage(this.encodedImage))
            //  { }

            //======
            // Declare a counter.
           int counter = 0;
           int h = this.rawBitmap.Height;
           int w = this.rawBitmap.Width;
           
           OnesAndZeros = await OZs;
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

            // Now, in the new bitmap image, set the pixel value to be the same as the original pixelColor, plus
            // a bit from our long string of bytes, for each color channel.
            if (counter + 3 < OnesAndZeros.Length)
            {
                // Encode Nibble to Pixel
                // Next, declare a newR, newG, and newB consisting of the sanitized value, plus a bit from the byteString.
                
                newA = sanitizedColor.A + int.Parse(OnesAndZeros[counter].ToString());
                newR = sanitizedColor.R + int.Parse(OnesAndZeros[counter + 1].ToString());
                newG = sanitizedColor.G + int.Parse(OnesAndZeros[counter + 2].ToString());
                newB = sanitizedColor.B + int.Parse(OnesAndZeros[counter + 3].ToString());

                return Color.FromArgb(newA, newR, newG, newB);
                //this.encodedImage.SetPixel(column, row, Color.FromArgb(newA, newR, newG, newB));
            }
            else
            {
                // Otherwise give the image a pixel equal to the sanitized pixel value.
                return pixelColor;
            }
        }

        private async Task<StringBuilder> PrependIVOntoFileAsStringBuilder(byte[] IV, byte[] file)
        {
            OnesAndZeros = new StringBuilder();

            foreach (byte b in IV)
            {
                OnesAndZeros.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            // Convert byte[] to string of 1s and 0s and add on to list..
            foreach (byte b in file)
            {
                OnesAndZeros.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return OnesAndZeros;
        }

        private StringBuilder SBPrependIVOntoFileAsStringBuilder(byte[] IV, byte[] file)
        {
            OnesAndZeros = new StringBuilder();

            foreach (byte b in IV)
            {
                OnesAndZeros.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            // Convert byte[] to string of 1s and 0s and add on to list..
            foreach (byte b in file)
            {
                OnesAndZeros.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return OnesAndZeros;
        }

       

    }
}

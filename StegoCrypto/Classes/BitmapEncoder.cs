using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly Bitmap theBitmap;
        private byte[] rgbValues;
        private BitArray OnesAndZeros;
        BackgroundWorker bgWorker = new BackgroundWorker();
        PleaseWait pwForm;
        int h;
        int w;

        // The constructor
        public BitmapEncoder(Bitmap rawBitmap)
        {
            this.theBitmap = rawBitmap;

            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            bgWorker.DoWork +=
                new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged +=
                new ProgressChangedEventHandler(
            bgWorker_ProgressChanged);

            bgWorker.WorkerReportsProgress = true;
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int counter = 0;
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
                    bgWorker.ReportProgress(row / h);
                }
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void bgWorker_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            pwForm.progress.Increment(e.ProgressPercentage);
        }

        // The main method that returns an encoded bitmap.
        public Bitmap EncodedBitmap(byte[] file, byte[] IV)
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, this.theBitmap.Width, this.theBitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                this.theBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                this.theBitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * this.theBitmap.Height;
            rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // cache the bitmaps height and width.

            h = this.theBitmap.Height;
            w = this.theBitmap.Width;

            // Prepend IV onto file and convert them both to a BitArray.
            OnesAndZeros = GetOnesAndZeros(IV, file);

            // Loop through every row and every column of pixels in the original image.
            var t = new Thread(() => BeginWorkerAndProgress());
            t.Start();
            t.Join();

            if (OnesAndZeros.Length / 8 > (w * h / 2))
                Console.WriteLine("******************************* \n MESSAGE TRUNCATED WHILE WRITING TO BITMAP!!!");

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            this.theBitmap.UnlockBits(bmpData);
            return this.theBitmap;
        }

        private void BeginWorkerAndProgress()
        {
            // Update the progress bar.
            pwForm = new PleaseWait();
            pwForm.progress.Maximum = h;
            pwForm.Show();
            pwForm.Refresh();
            bgWorker.RunWorkerAsync();
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

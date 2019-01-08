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
        byte[] rgbValues;
        private BitArray OnesAndZeros;
        BackgroundWorker bgWorker;
        PleaseWait pwForm;

        // The constructor.
        public BitmapEncoder(Bitmap rawBitmap)
        {
            this.theBitmap = rawBitmap;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
        }

        protected void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("BGworker stared.");
            int counter = 0;
            int length = rgbValues.Length;
            for (int i = 0; i < rgbValues.Length - 4; i += 4)
            {
                if (i + 3 < OnesAndZeros.Length)
                {
                    rgbValues[i] = (byte)((rgbValues[i] - (rgbValues[i] % 2)) + ToInt(OnesAndZeros[i + 3]));
                    rgbValues[i + 1] = (byte)((rgbValues[i + 1] - (rgbValues[i + 1] % 2)) + ToInt(OnesAndZeros[i + 2]));
                    rgbValues[i + 2] = (byte)((rgbValues[i + 2] - (rgbValues[i + 2] % 2)) + ToInt(OnesAndZeros[i + 1]));
                    rgbValues[i + 3] = (byte)((rgbValues[i + 3] - (rgbValues[i + 3] % 2)) + ToInt(OnesAndZeros[i]));

                    if (i % 1000 == 0)
                    {
                        bgWorker.ReportProgress(i);
                    }
                    counter += 4;
                }
            }
            Console.WriteLine("BGWorker Completed.");
        }

        protected void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        protected void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pwForm.progress.Value = e.ProgressPercentage;
        }

        // The main method that returns an encoded bitmap.
        public async Task<Bitmap> EncodedBitmap(byte[] file, byte[] IV)
        {
            pwForm = new PleaseWait();

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, this.theBitmap.Width, this.theBitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                this.theBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                this.theBitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int numOfBytes = Math.Abs(bmpData.Stride) * this.theBitmap.Height;
            rgbValues = new byte[numOfBytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, numOfBytes);

            // Prepend IV onto file and convert them both to a BitArray.
            OnesAndZeros = GetOnesAndZeros(IV, file);

            // Update the progress bar.
            pwForm.progress.Maximum =  numOfBytes;
            pwForm.Show();
            pwForm.Refresh();

            // Do work, and even though it's a background worker, wait until it's complete.
            var result = await bgWorker.RunWorkerTaskAsync();

            pwForm.Close();

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, numOfBytes);

            // Unlock the bits.
            this.theBitmap.UnlockBits(bmpData);
            return this.theBitmap;
        }

        private void OverwriteWithNewBytes(int startIndex, int stop)
        {
            throw new NotImplementedException();
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

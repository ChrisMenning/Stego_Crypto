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
            int amountDone = 0;
            for (int i = 0; i < rgbValues.Length; i++)
            {
                if (counter + 3 < OnesAndZeros.Length)
                {
                    rgbValues[counter] = (byte)((rgbValues[counter] - (rgbValues[counter] % 2)) + ToInt(OnesAndZeros[counter + 3]));
                    rgbValues[counter + 1] = (byte)((rgbValues[counter + 1] - (rgbValues[counter + 1] % 2)) + ToInt(OnesAndZeros[counter + 2]));
                    rgbValues[counter + 2] = (byte)((rgbValues[counter + 2] - (rgbValues[counter + 2] % 2)) + ToInt(OnesAndZeros[counter + 1]));
                    rgbValues[counter + 3] = (byte)((rgbValues[counter + 3] - (rgbValues[counter + 3] % 2)) + ToInt(OnesAndZeros[counter]));

                    if ((i % rgbValues.Length) / 10 == 0)
                    {
                        Console.WriteLine("amount done: " + amountDone);
                        amountDone++;
                        bgWorker.ReportProgress(amountDone * 10);
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
            int bytes = Math.Abs(bmpData.Stride) * this.theBitmap.Height;
            rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Declare a counter.
            int h = this.theBitmap.Height;
            int w = this.theBitmap.Width;

            // Prepend IV onto file and convert them both to a BitArray.
            OnesAndZeros = GetOnesAndZeros(IV, file);

            // Update the progress bar.
            pwForm.progress.Maximum =  100;
            pwForm.Show();
            pwForm.Refresh();

            var result = await bgWorker.RunWorkerTaskAsync();

            pwForm.Close();

            if (OnesAndZeros.Length / 8 > (w * h / 2))
                Console.WriteLine("******************************* \n MESSAGE TRUNCATED WHILE WRITING TO BITMAP!!!");

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

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

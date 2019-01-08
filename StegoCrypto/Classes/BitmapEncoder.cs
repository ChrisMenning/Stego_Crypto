using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;

namespace StegoCrypto
{
    public class BitmapEncoder
    {
        // the private fields.
        private readonly Bitmap theBitmap;
        byte[] argbValues;
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
            int length = argbValues.Length;

            // Starting at 0, loop through every 4th byte of ARGB values, and in groups of 4, set the least significant bit (LSB) 
            // to 0 and then change the LSB to the bit from the OnesAndZeros bitarray that was made from the input file.
            for (int i = 0; i < length - 4; i += 4)
            {
                if (i + 3 < OnesAndZeros.Length)
                {
                    // Note: This used to be a more readable set of GetPixel and SetPixel commands, but this way is much faster.
                    argbValues[i] = (byte)((argbValues[i] - (argbValues[i] % 2)) + ToInt(OnesAndZeros[i + 3]));
                    argbValues[i + 1] = (byte)((argbValues[i + 1] - (argbValues[i + 1] % 2)) + ToInt(OnesAndZeros[i + 2]));
                    argbValues[i + 2] = (byte)((argbValues[i + 2] - (argbValues[i + 2] % 2)) + ToInt(OnesAndZeros[i + 1]));
                    argbValues[i + 3] = (byte)((argbValues[i + 3] - (argbValues[i + 3] % 2)) + ToInt(OnesAndZeros[i]));

                    if (i % 1000 == 0)
                    {
                        bgWorker.ReportProgress(i);
                    }
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

            pwForm.Show();
            pwForm.Refresh();

            // Prepend IV onto file and convert them both to a BitArray.
            OnesAndZeros = GetOnesAndZeros(IV, file);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, this.theBitmap.Width, this.theBitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                this.theBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                this.theBitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int numOfBytes = Math.Abs(bmpData.Stride) * this.theBitmap.Height;
            argbValues = new byte[numOfBytes];

            // Update the progress bar.
            pwForm.progress.Maximum = numOfBytes;
            pwForm.progress.Value = 100;

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numOfBytes);

            // Do work, and even though it's a background worker, wait until it's complete.
            var result = await bgWorker.RunWorkerTaskAsync();

            pwForm.Close();

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numOfBytes);

            // Unlock the bits.
            this.theBitmap.UnlockBits(bmpData);
            return this.theBitmap;
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

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StegoCrypto
{
    public class BitmapDecoder
    {
        private bool[] binaryFromImage;
        private int height; 
        private byte[] argbValues;
        BackgroundWorker bgWorker;
        PleaseWait pwForm;
        private bool useWaitForm;

        // Constructor
        public BitmapDecoder()
        {
            useWaitForm = true;
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
        }

        public BitmapDecoder(bool useWaitForm)
        {
            useWaitForm = false;
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
        }

        protected void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Starting at 0, loop through every 4th byte of RGB values, and in groups of 4, get the least significant bit (LSB) 
            // and assign it to the binaryfromImage array.

            for (int i = 0; i < argbValues.Length - 4; i += 4)
            {
                binaryFromImage[i + 3] = ToBool(argbValues[i] % 2);
                binaryFromImage[i + 2] = ToBool(argbValues[i + 1] % 2);
                binaryFromImage[i + 1] = ToBool(argbValues[i + 2] % 2);
                binaryFromImage[i] = ToBool(argbValues[i + 3] % 2);

                if (i % 1000 == 0)
                {
                    bgWorker.ReportProgress(i);
                }
            }
        }

        protected void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgWorker.Dispose();
        }

        protected void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (useWaitForm)
                pwForm.progress.Value = e.ProgressPercentage;
        }

        // Bytes from Image
        public async Task<byte[]> BytesFromImage(Bitmap encodedBitmap)
        {
            Bitmap encoded = encodedBitmap;
            binaryFromImage = new bool[(encoded.Width * encoded.Height) * 4];
            byte[] bytesDecodedFromImage;

            // Cache the bitmap's height.
            height = encoded.Height;

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, encoded.Width, encoded.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                encoded.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                encoded.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            GC.Collect();
            // Declare an array to hold the bytes of the bitmap.
            int numOfBytes = Math.Abs(bmpData.Stride) * height;
            argbValues = new byte[numOfBytes];            

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numOfBytes);

            if (useWaitForm == true)
            {
                pwForm = new PleaseWait();
                pwForm.progress.Maximum = numOfBytes;
                pwForm.Show();
                pwForm.Refresh();
            }

            // Do work, and even though it's a background worker, wait until it's complete.
            var result = await bgWorker.RunWorkerTaskAsync();

            if (useWaitForm == true)
                pwForm.progress.Value = pwForm.progress.Maximum - 10;

            // Convert string of 1s and 0s to byte[].
            BitArray ba = new BitArray(binaryFromImage.ToArray());
            bytesDecodedFromImage = ToByteArray(ba);

            if (useWaitForm == true)
                pwForm.progress.Value = pwForm.progress.Maximum;

            // Unlock the bits.
            //encoded.UnlockBits(bmpData);

            if (useWaitForm == true)
            {
                pwForm.Close();
                pwForm.Dispose();
            }

            // Finally, return the bytes.
            return bytesDecodedFromImage;
        }

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

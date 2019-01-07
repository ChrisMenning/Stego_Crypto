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
    public class BitmapDecoder
    {
        private bool[] binaryFromImage;
        private byte[] rgbValues;
        BackgroundWorker bgWorker = new BackgroundWorker();
        PleaseWait pwForm;

        // Constructor
        public BitmapDecoder()
        {
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
        { }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { }
        private void bgWorker_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            pwForm.progress.Increment(e.ProgressPercentage);
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

            pwForm = new PleaseWait();
            pwForm.progress.Maximum = encoded.Height;
            pwForm.Show();
            pwForm.Refresh();

            int h = encoded.Height;

            Console.WriteLine("Looping through all pixels...");
        //    var t = new Thread(() => BeginWorkerAndProgress());
        //    t.Start();
        //    t.Join();

            for (int i = 0; i < (rgbValues.Length - 4); i +=4)
            {
                binaryFromImage[i] = ToBool(rgbValues[i + 3] % 2);
                binaryFromImage[i + 1] = ToBool(rgbValues[i + 2] % 2);
                binaryFromImage[i + 2] = ToBool(rgbValues[i + 1] % 2);
                binaryFromImage[i + 3] = ToBool(rgbValues[i] % 2);
            }

            // Convert string of 1s and 0s to byte[].
            BitArray ba = new BitArray(binaryFromImage.ToArray());
            byteArray = ToByteArray(ba);

            pwForm.Close();
            // Unlock the bits.
            encoded.UnlockBits(bmpData);
            return byteArray;
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

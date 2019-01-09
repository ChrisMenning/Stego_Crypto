using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace StegoCrypto
{
    public partial class GenerateFractal : Form
    {
        // Private fields
        private FormMain mainForm;
        private int squareSize;
        private Bitmap newFractal;
        private double realC;
        private double imaginaryC;
        private double zoomLevel;
        private double xOffset;
        private double yOffset;
        private int amountDone;
        private byte[] argbValues;

        // Public Properties
        public double RealC { get { return realC; } set { realC = value; } } 
        public double ImaginaryC { get { return imaginaryC; } set { imaginaryC = value; } }

        // Form Constructor
        public GenerateFractal(FormMain mainForm, int squareSize)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.squareSize = squareSize;
            this.AcceptButton = buttonAccept;

            progressBar1.Maximum = (squareSize * squareSize) / 2;
            this.zoomLevel = 1;
        }

        // Background Worker's main work method.
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            newFractal = JuliaSet();
        }

        // Background Worker's progress reporting thread.
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Increment(amountDone);
            labelPercent.Text = e.ProgressPercentage + "%";
        }

        // Method triggered when background worker completes work.
        private void bgWorker_RunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Image = newFractal;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            buttonAccept.Enabled = true;
            buttonGenerate.Enabled = true;
            progressBar1.Value = 0;
        }

        // First of four threads for rendering a section of the fractal.
        private Thread firstQuarter()
        {
            var t = new Thread(() => JuliaSetSection(0, squareSize / 4));
            return t;
        }

        // Second of four threads for rendering a section of the fractal.
        private Thread secondQuarter()
        {
            var t = new Thread(() => JuliaSetSection(squareSize / 4, squareSize / 2));
            return t;
        }

        // Third of four threads for rendering a section of the fractal.
        private Thread thirdQuarter()
        {
            var t = new Thread(() => JuliaSetSection(squareSize / 2, Convert.ToInt32(squareSize * 0.75)));
            return t;
        }

        // Fourth of four threads for rendering a section of the fractal.
        private Thread fourthQuarter()
        {
            var t = new Thread(() => JuliaSetSection(Convert.ToInt32(squareSize * 0.75), squareSize));
            return t;
        }

        // Validate the input values and give the background worker the go-ahead to begin the work.
        private void MakeJuliaSet()
        {
            ValidateOffsets();
            ValidateC();
            buttonGenerate.Enabled = false;

            backgroundWorker1.RunWorkerAsync();
        }

        // Returns the finished fractal Bitmap when called. Background worker calls this.
        private Bitmap JuliaSet()
        {
            Bitmap bmp = new Bitmap(squareSize, squareSize);
            amountDone = 0;

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            argbValues = new byte[bytes];

            // Divide the workload of drawing the Julia Set across four threads, each drawing 1/4 of the image.
            Thread ThreadOne = firstQuarter();
            Thread ThreadTwo = secondQuarter();
            Thread ThreadThree = thirdQuarter();
            Thread ThreadFour = fourthQuarter();

            Console.WriteLine("Starting four threads.");
            ThreadOne.Start();
            ThreadTwo.Start();
            ThreadThree.Start();
            ThreadFour.Start();
            Console.WriteLine("Four threads started.");

            ThreadOne.Join(); Console.WriteLine("Thread One Joined.");
            ThreadTwo.Join(); Console.WriteLine("Thread Two Joined.");
            ThreadThree.Join(); Console.WriteLine("Thread Three Joined.");
            ThreadFour.Join(); Console.WriteLine("Thread Four Joined.");

            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, bytes);

            // Unlock the bits and return the image.
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        // This method adapted from https://lodev.org/cgtutor/juliamandelbrot.html
        private void JuliaSetSection(int startIndex, int stopIndex)
        {
            Console.WriteLine("Starting a Julia quarter at " + startIndex + " on Thread " + Thread.CurrentThread.ManagedThreadId);
            //each iteration, it calculates: new = old*old + c, where c is a constant and old starts at current pixel
            double newReal, newIm, oldReal, oldIm;   //real and imaginary parts of new and old
            double zoom = zoomLevel, moveX = xOffset, moveY = yOffset; //you can change these to zoom and change position
            int maxIterations = 360; //after how much iterations the function should stop

            //pick some values for the constant c, this determines the shape of the Julia Set           

            int counter = startIndex * squareSize * 4;
            //loop through every pixel
            for (int y = startIndex; y < stopIndex; y++)
            {
                for (int x = 0; x < squareSize; x++)
                {
                    //calculate the initial real and imaginary part of z, based on the pixel location and zoom and position values
                    newReal = 1.5 * (x - squareSize / 2) / (0.5 * zoom * squareSize) + moveX;
                    newIm = (y - squareSize / 2) / (0.5 * zoom * squareSize) + moveY;
                    //i will represent the number of iterations
                    int i;
                    //start the iteration process
                    for (i = 0; i < maxIterations; i++)
                    {
                        //remember value of previous iteration
                        oldReal = newReal;
                        oldIm = newIm;
                        //the actual iteration, the real and imaginary part are calculated
                        newReal = oldReal * oldReal - oldIm * oldIm + RealC;
                        newIm = 2 * oldReal * oldIm + ImaginaryC;
                        //if the point is outside the circle with radius 2: stop
                        if ((newReal * newReal + newIm * newIm) > 4) break;
                    }

                    double val = i % maxIterations;
                    int r, g, b;

                    HsvToRgb(val, 1, val, out r, out g, out b);

                    // Note: For some reason, the byte order needs to be reversed here.
                    argbValues[counter + 3] = 255;
                    argbValues[counter + 2] = (byte)r;
                    argbValues[counter + 1] = (byte)g;
                    argbValues[counter + 0] = (byte)b;
                    counter += 4;
                }
                if (!backgroundWorker1.CancellationPending)
                {
                    amountDone++;
                    double percentDone = ((double)amountDone / (double)squareSize) * 100;
                    backgroundWorker1.ReportProgress((int)percentDone);
                }
            }
            Console.WriteLine("Thread Complete from " + startIndex + " to " + stopIndex);
        }

        // HSV to RGB by Chris Hulbert http://www.splinter.com.au/converting-hsv-to-rgb-colour-using-c/
        private void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color
                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color
                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color
                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color
                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.
                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    default:
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = (int)R * 10;
            g = (int)G * 10;
            b = (int)B * 10;
        }

        private void ValidateOffsets()
        {
            try
            {
                double.TryParse(textBoxXOffset.Text, out xOffset);

                if (xOffset > 1)
                {
                    xOffset = 1;
                }
                else if (xOffset < -1)
                {
                    xOffset = -1;
                }
                textBoxXOffset.Text = xOffset.ToString();
            }
            catch
            {
                MessageBox.Show("Offset value must be a valid decimal.");
            }

            try
            {
                double.TryParse(textBoxYOffset.Text, out yOffset);

                if (yOffset > 1)
                {
                    yOffset = 1;
                }
                else if (yOffset < -1)
                {
                    yOffset = -1;
                }
                textBoxYOffset.Text = yOffset.ToString();
            }
            catch
            {
                MessageBox.Show("Offset value must be a valid decimal.");
            }

        }

        private void ValidateC()
        {
            try
            {
                double rC = 0;
                double.TryParse(textBoxC.Text, out rC);
                RealC = rC;
            }
            catch
            {
                MessageBox.Show("Real C must be a decimal.");
                textBoxC.Text = "-0.7";
                RealC = -0.7;
            }

            try
            {
                double imC = 0;
                double.TryParse(textBoxCim.Text, out imC);
                ImaginaryC = imC;
            }
            catch
            {
                MessageBox.Show("Imaginary C must be a decimal.");
                textBoxCim.Text = "0.27015";
                ImaginaryC = 0.27015;
            }

            Console.WriteLine("C: " + RealC + " ImC: " + ImaginaryC);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            MakeJuliaSet();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            mainForm.OriginalImage = newFractal;
            mainForm.PicBoxOrig.Image = mainForm.OriginalImage;
            mainForm.PicBoxOrig.SizeMode = PictureBoxSizeMode.Zoom;
            mainForm.EstimatedStorageCap.Text = (((newFractal.Height * newFractal.Width) / 2) - 64).ToString();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxZoom_TextChanged(object sender, EventArgs e)
        {
            zoomLevel = 0;
            try {
                double.TryParse(textBoxZoom.Text, out zoomLevel);
            }
            catch {
                MessageBox.Show("Zoom level must be a decimal.");
            }

            if (zoomLevel < -300)
            {
                textBoxZoom.Text = "-300";
            }
            else if (zoomLevel > 300)
            {
                textBoxZoom.Text = "300";
            }
        }

        // A set of interesting values for different looking Julia Set fractals.
        private void comboBoxPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selection = comboBoxPreset.SelectedItem.ToString();
            switch (selection)
            {
                case "1":
                    textBoxC.Text = "-0.7";
                    textBoxCim.Text = "0.27015";
                    break;
                case "2":
                    textBoxC.Text = "-0.70176";
                    textBoxCim.Text = "0.265";
                    break;
                case "3":
                    textBoxC.Text = "-0.391";
                    textBoxCim.Text = "-0.59";
                    break;
                case "4":
                    textBoxC.Text = "-0.70176";
                    textBoxCim.Text = "-0.3842";
                    break;
                case "5":
                    textBoxC.Text = "-0.835";
                    textBoxCim.Text = "0.2321";
                    break;
                case "6":
                    textBoxC.Text = "-0.8";
                    textBoxCim.Text = "0.156";
                    break;
                case "7":
                    textBoxC.Text = "-0.7885";
                    textBoxCim.Text = "0.1385";
                    break;
                case "8":
                    textBoxC.Text = "0.285";
                    textBoxCim.Text = "0.01";
                    break;
                case "9":
                    textBoxC.Text = "0.285";
                    textBoxCim.Text = "0.0132";
                    break;
                case "Custom":
                    break;
                default:
                    textBoxC.Text = "-0.7";
                    textBoxCim.Text = "0.27015";
                    break;
            }
        }
    }
}

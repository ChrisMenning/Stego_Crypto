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
        Thread[] threads;

        private double startingHue;
        private double startingSaturation;
        private double startingValue;

        private bool useHighContrast;

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
            GetStartColor();

            progressBar1.Maximum = (squareSize * squareSize) / 2;
            this.zoomLevel = 1;
            useHighContrast = false;
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

        // Validate the input values and give the background worker the go-ahead to begin the work.
        private void MakeJuliaSet()
        {
            ValidateOffsets();
            ValidateC();
            buttonGenerate.Enabled = false;

            bgWorker.RunWorkerAsync();
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

            // Create as many threads as there are logical processors.
            threads = new Thread[Environment.ProcessorCount];
            int lastEndIndex = 0;
            for (int i = 0; i < threads.Length; i++)
            {
                int startIndex = 0;
                int endIndex = 0;
                    
                if (i == 0)
                {
                    startIndex = 0;
                }
                else
                {
                    startIndex = lastEndIndex;
                }
                     
                endIndex = Convert.ToInt32(squareSize * ((double)(i + 1) / (double)threads.Length));
                Console.WriteLine("Creating thread for section: " + startIndex + " to " + endIndex);
             
                threads[i] = new Thread(() => JuliaSetSection(startIndex, endIndex));
                lastEndIndex = endIndex;
            }
            Console.WriteLine("Created " + threads.Length + " threads.");
             
            foreach (Thread t in threads)
            {
                t.Start();
            }
             
            foreach (Thread t in threads)
            {
                t.Join();
                Console.WriteLine("Thread " + t.ManagedThreadId + " joined.");
            }
            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, bytes);

            // Unlock the bits and return the image.
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        // This method adapted from https://lodev.org/cgtutor/juliamandelbrot.html
        private void JuliaSetSection(int startIndex, int stopIndex)
        {
            Console.WriteLine("Starting a Julia section at " + startIndex + " on Thread " + Thread.CurrentThread.ManagedThreadId);
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

                    double hue = i % maxIterations + startingHue;
                    double val = startingValue;

                    if (useHighContrast)
                    {
                        val = i % maxIterations;
                    }
                    else
                    {
                        if (i < maxIterations)
                        {
                            val = 255;
                        }
                        else
                        {
                            val = 0;
                        }
                    }
                    int r;
                    int g;
                    int b;
                    if (useHighContrast)
                    {
                        HsvToRgb(hue, startingSaturation, val, out r, out g, out b);
                    }
                    else
                    {
                        Color C = ColorFromHSV(hue, startingSaturation, val);
                        r = C.R;
                        g = C.G;
                        b = C.B;
                    }

                    // Note: For some reason, the byte order needs to be reversed here.
                    argbValues[counter + 3] = 255;
                    argbValues[counter + 2] = (byte)r;
                    argbValues[counter + 1] = (byte)g;
                    argbValues[counter + 0] = (byte)b;
                    counter += 4;
                }
                if (!bgWorker.CancellationPending)
                {
                    amountDone++;
                    double percentDone = ((double)amountDone / (double)squareSize) * 100;
                    bgWorker.ReportProgress((int)percentDone);
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

        // https://stackoverflow.com/users/12971/greg
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        // https://stackoverflow.com/users/12971/greg
        public static Color ColorFromHSV(double h, double s, double val)
        {
            double hue = h;
            double saturation = s;
            double value = val;
         
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);
            
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
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

        private void buttonChooseStartingColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Color startColor = colorDialog1.Color;
            ColorToHSV(startColor, out startingHue, out startingSaturation, out startingValue);
            labelColor.BackColor = startColor;
        }

        private void GetStartColor()
        {
            Color startColor = labelColor.BackColor;
            ColorToHSV(startColor, out startingHue, out startingSaturation, out startingValue);
        }

        private void radioButtonHighContrast_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHighContrast.Checked)
                useHighContrast = true;
        }

        private void radioButtonSmooth_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSmooth.Checked)
                useHighContrast = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegoCrypto
{
    public partial class GenerateFractal : Form
    {
        public delegate void RefreshBar();
        public RefreshBar delegateRefresh;

        private FormMain mainForm;
        private int squareSize;
        private Bitmap newFractal;
        private double realC;
        private double imaginaryC;
        private double zoomLevel;
        private double xOffset;
        private double yOffset;

        public double RealC { get { return realC; } set { realC = value; } } 
        public double ImaginaryC { get { return imaginaryC; } set { imaginaryC = value; } }

        public GenerateFractal(FormMain mainForm, int squareSize)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.squareSize = squareSize;
            progressBar1.Maximum = squareSize;
            this.zoomLevel = 1;
            this.AcceptButton = buttonAccept;
            delegateRefresh = new RefreshBar(progressBar1.Refresh);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            MakeJuliaSet();
        }

        private async void MakeJuliaSet()
        {
            ValidateOffsets();
            ValidateC();
            Task<Bitmap> js = JuliaSet();
            newFractal = await js;
            pictureBox1.Image = newFractal;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            buttonAccept.Enabled = true;
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

        // This method adapted from https://lodev.org/cgtutor/juliamandelbrot.html
        private async Task<Bitmap> JuliaSet()
        {
            progressBar1.Maximum = (squareSize * squareSize) / 2;
            progressBar1.Show();

            buttonGenerate.Enabled = false;
            Bitmap bmp = new Bitmap(squareSize, squareSize);

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            //each iteration, it calculates: new = old*old + c, where c is a constant and old starts at current pixel
            double cRe, cIm;           //real and imaginary part of the constant c, determinate shape of the Julia Set
            double newRe, newIm, oldRe, oldIm;   //real and imaginary parts of new and old
            double zoom = zoomLevel, moveX = xOffset, moveY = yOffset; //you can change these to zoom and change position
            int maxIterations = 512; //after how much iterations the function should stop

            //pick some values for the constant c, this determines the shape of the Julia Set           
            cIm = ImaginaryC;
            cRe = RealC;
            int counter = 0;
            //loop through every pixel
            for (int y = 0; y < squareSize; y++)
            {
                for (int x = 0; x < squareSize; x++)
                {
                    //calculate the initial real and imaginary part of z, based on the pixel location and zoom and position values
                    newRe = 1.5 * (x - squareSize / 2) / (0.5 * zoom * squareSize) + moveX;
                    newIm = (y - squareSize / 2) / (0.5 * zoom * squareSize) + moveY;
                    //i will represent the number of iterations
                    int i;
                    //start the iteration process
                    for (i = 0; i < maxIterations; i++)
                    {
                        //remember value of previous iteration
                        oldRe = newRe;
                        oldIm = newIm;
                        //the actual iteration, the real and imaginary part are calculated
                        newRe = oldRe * oldRe - oldIm * oldIm + cRe;
                        newIm = 2 * oldRe * oldIm + cIm;
                        //if the point is outside the circle with radius 2: stop
                        if ((newRe * newRe + newIm * newIm) > 4) break;
                    }

                    double val = i % maxIterations;
                    int r, g, b;
                    HsvToRgb(val, 1, val, out r, out g, out b);
                    // Note: For some reason, the byte order needs to be reversed here.
                    rgbValues[counter + 3] = 255;
                    rgbValues[counter + 2] = (byte)r;
                    rgbValues[counter + 1] = (byte)g;
                    rgbValues[counter + 0] = (byte)b;

                    counter += 4;
                }

                this.Invoke(delegateRefresh);
                progressBar1.Increment(y);
            }
            progressBar1.Value = 0;

            for (int i = 0; i < 16; i++)
            {
                Console.WriteLine("Color byte: " + rgbValues[i]);
            }

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            buttonGenerate.Enabled = true;
            return bmp;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            mainForm.OriginalImage = newFractal;
            mainForm.PicBoxOrig.Image = mainForm.OriginalImage;
            mainForm.PicBoxOrig.SizeMode = PictureBoxSizeMode.Zoom;
            mainForm.EstimatedStorageCap.Text = ((((newFractal.Height * newFractal.Width) / 2) - 64)).ToString();
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
                MessageBox.Show("Zoom level must be an integer.");
            }

            if (zoomLevel < 1)
            {
                textBoxZoom.Text = "1";
            }
            else if (zoomLevel > 512)
            {
                textBoxZoom.Text = "512";
            }
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

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

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
                    textBoxC.Text = "-0.8";
                    textBoxCim.Text = "0.156";
                    break;
                case "3":
                    textBoxC.Text = "-0.7885";
                    textBoxCim.Text = "0.1385";
                    break;
                case "4":
                    textBoxC.Text = "-0.70176";
                    textBoxCim.Text = "0.265";
                    break;
                case "5":
                    textBoxC.Text = "0.285";
                    textBoxCim.Text = "0.0132";
                    break;
                case "6":
                    textBoxC.Text = "-0.391";
                    textBoxCim.Text = "-0.59";
                    break;
                case "Custom":
                    break;
                default:
                    textBoxC.Text = "-0.7";
                    textBoxCim.Text = "0.27015";
                    break;
            }
        }

        private void textBoxXOffset_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxYOffset_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}

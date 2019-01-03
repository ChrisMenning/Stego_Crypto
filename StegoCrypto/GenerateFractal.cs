﻿using System;
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
        // This code adapted from https://lodev.org/cgtutor/juliamandelbrot.html
        private FormMain mainForm;
        private int squareSize;
        private Bitmap newFractal;
        //Color[] colors;

        public GenerateFractal(FormMain mainForm, int squareSize)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.squareSize = squareSize;
            progressBar1.Maximum = squareSize;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            newFractal = JuliaSet();
            pictureBox1.Image = newFractal;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            buttonAccept.Enabled = true;
        }

    //    private void MakeColorSet(int maxIterations)
    //    {
    //        int cubeRoot = 8;
    //        Console.WriteLine("Cubic root of fractal iterations: " + cubeRoot);
    //        colors = new Color[maxIterations];
    //
    //        int index = 1;
    //        for (int r = 1; r <= cubeRoot; r++)
    //        {
    //            for (int g = 1; g <= cubeRoot; g++)
    //            {
    //                for (int b = 1; b <= cubeRoot; b++)
    //                {
    //                    colors[index] = Color.FromArgb(255, (r * 32) - 1, (g * 32) - 1, Math.Abs((index / 2) - 1));
    //                    index++;
    //                }
    //            }
    //        }
    //        Console.WriteLine("CREATED " + index + "colors.");
    //    }

        private Bitmap JuliaSet()
        {
            buttonGenerate.Enabled = false;
            Random rand = new Random();

            int randomChannel = rand.Next(0, 3);
            int[] colors = new int[3];

            Bitmap bmp = new Bitmap(squareSize, squareSize);
            progressBar1.Maximum = (squareSize * squareSize) / 2;
            progressBar1.Show();

            //each iteration, it calculates: new = old*old + c, where c is a constant and old starts at current pixel
            double cRe, cIm;           //real and imaginary part of the constant c, determinate shape of the Julia Set
            double newRe, newIm, oldRe, oldIm;   //real and imaginary parts of new and old
            double zoom = rand.Next(1, 300), moveX = rand.NextDouble() * (0.1 - -0.1) + -0.1, moveY = rand.NextDouble() * (0.1 - -0.1) + -0.1; //you can change these to zoom and change position
            //double zoom = 1, moveX = 0, moveY = 0; //you can change these to zoom and change position
            Color color; //the RGB color value for the pixel
            int maxIterations = 512; //after how much iterations the function should stop

            //pick some values for the constant c, this determines the shape of the Julia Set
            cRe = -0.7;
            cIm = 0.27015;
            // cRe = rand.Next(-1, 1) / 10;
            // cIm = rand.Next(-1, 1) / 10;

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

                    // make brightness black if maxIterations reached

                    for (int j = 0; j < 3; j++)
                    {
                        colors[j] = i % 255;
                    }


                    if (i < maxIterations)
                    {
                        colors[randomChannel] = 255 - (i % 255);
                    }
                    else
                    {
                        colors[randomChannel] = 0;
                    }

                    color = Color.FromArgb(250, colors[0], colors[1], colors[2]);

                    //  if (i < maxIterations)
                    //  {
                    //      color = colors[i];
                    //  }
                    //  else
                    //      color = colors[0];

                    //draw the pixel
                    bmp.SetPixel(x, y, color);
                }
                progressBar1.Increment(y);
            }
            progressBar1.Value = 0;
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
    }
    
}

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
    public partial class ImageTooSmall : Form
    {
        private FormMain mainForm;
        int neededSquare;

        public ImageTooSmall(FormMain mainForm, int neededSquare)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.neededSquare = neededSquare;
        }

        private void buttonGenerateFractal_Click(object sender, EventArgs e)
        {
            GenerateFractal gf = new GenerateFractal(mainForm, neededSquare);
            gf.Show();
            this.Close();

        }

        private void buttonOpenAnotherImage_Click(object sender, EventArgs e)
        {
            mainForm.SelectImageForHiding();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

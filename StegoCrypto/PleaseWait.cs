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
    public partial class PleaseWait : Form
    {
        public ProgressBar progress;
        public PleaseWait()
        {
            InitializeComponent();
            this.progress = progressBar1;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}

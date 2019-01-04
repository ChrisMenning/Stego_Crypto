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
    public partial class FormPassword : Form
    {
        private FormMain mainform;
        private PasswordHandler pwHandler;

        public FormPassword(FormMain mainform)
        {
            InitializeComponent();
            this.mainform = mainform;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AcceptButton = buttonOK;
        }

        public PasswordHandler PwHandler
        {
            get
            {
                return pwHandler;
            }

            set
            {
                pwHandler = value;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            pwHandler = new PasswordHandler(textBoxPassword.Text, mainform);
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

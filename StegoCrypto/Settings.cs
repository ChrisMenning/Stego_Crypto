﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegoCrypto
{
    public partial class Settings : Form
    {
        FormMain mainForm;
        UserPreferences prefs;
        public Settings(FormMain mainForm)
        {
            this.mainForm = mainForm;

            InitializeComponent();
            prefs = mainForm.UserPrefs;
            prefs.LoadPrefs();

            PopulateControlsFromPreferencesFile();
            this.StartPosition = FormStartPosition.CenterParent;

            CreateToolTips();
        }

        private void CreateToolTips()
        {
            ToolTip ttSalt = new ToolTip();
            ttSalt.IsBalloon = true;
            ttSalt.SetToolTip(labelSalt, "Random bytes for hashing the password.");

            ToolTip ttIterations = new ToolTip();
            ttIterations.IsBalloon = true;
            ttIterations.SetToolTip(labelIterations, "The number of times the password is hashed against the salt.");

            ToolTip ttKeySize = new ToolTip();
            ttKeySize.IsBalloon = true;
            ttKeySize.SetToolTip(labelKeySize, "The encryption key generated by the salt-and-hash \n" +
                                                "and used in both encryption and decryption can be \n" +
                                                "128, 192, or 256 bits long.");

            ToolTip ttCipherMode = new ToolTip();
            ttCipherMode.IsBalloon = true;
            ttCipherMode.SetToolTip(labelCipherMode, "AES has five different cipher modes, or algorithms.");
        }

        private void PopulateControlsFromPreferencesFile()
        {
            textBoxSalt0.Text = prefs.Salt[0].ToString();
            textBoxSalt1.Text = prefs.Salt[1].ToString();
            textBoxSalt2.Text = prefs.Salt[2].ToString();
            textBoxSalt3.Text = prefs.Salt[3].ToString();
            textBoxSalt4.Text = prefs.Salt[4].ToString();
            textBoxSalt5.Text = prefs.Salt[5].ToString();
            textBoxSalt6.Text = prefs.Salt[6].ToString();
            textBoxSalt7.Text = prefs.Salt[7].ToString();

            textBoxIterations.Text = prefs.Iterations.ToString();
            comboBoxBlockSize.SelectedItem = prefs.BlockSize.ToString();
            comboBoxKeySize.SelectedItem = prefs.KeySize.ToString();
            comboBoxCipherMode.SelectedItem = prefs.CipherMode.ToString();

            DescribeCipherMode();
        }

        private void DescribeCipherMode()
        {
            switch (comboBoxCipherMode.SelectedItem.ToString())
            {
                case "CBC":
                    textBoxCipherModeDetails.Text = "The Cipher Block Chaining (CBC) mode introduces feedback. Before each plain text block is encrypted, it is combined with the cipher text of the previous block by a bitwise exclusive OR operation. This ensures that even if the plain text contains many identical blocks, they will each encrypt to a different cipher text block. The initialization vector is combined with the first plain text block by a bitwise exclusive OR operation before the block is encrypted. If a single bit of the cipher text block is mangled, the corresponding plain text block will also be mangled. In addition, a bit in the subsequent block, in the same position as the original mangled bit, will be mangled.";
                    break;
                case "CFB":
                    textBoxCipherModeDetails.Text = "The Cipher Feedback (CFB) mode processes small increments of plain text into cipher text, instead of processing an entire block at a time. This mode uses a shift register that is one block in length and is divided into sections. For example, if the block size is 8 bytes, with one byte processed at a time, the shift register is divided into eight sections. If a bit in the cipher text is mangled, one plain text bit is mangled and the shift register is corrupted. This results in the next several plain text increments being mangled until the bad bit is shifted out of the shift register. The default feedback size can vary by algorithm, but is typically either 8 bits or the number of bits of the block size. You can alter the number of feedback bits by using the FeedbackSize property. Algorithms that support CFB use this property to set the feedback.";
                    break;
                case "CTS":
                    textBoxCipherModeDetails.Text = "The Cipher Text Stealing (CTS) mode handles any length of plain text and produces cipher text whose length matches the plain text length. This mode behaves like the CBC mode for all but the last two blocks of the plain text.";
                    break;
                case "ECB":
                    textBoxCipherModeDetails.Text = "The Electronic Codebook(ECB) mode encrypts each block individually.Any blocks of plain text that are identical and in the same message, or that are in a different message encrypted with the same key, will be transformed into identical cipher text blocks.Important: This mode is not recommended because it opens the door for multiple security exploits.If the plain text to be encrypted contains substantial repetition, it is feasible for the cipher text to be broken one block at a time.It is also possible to use block analysis to determine the encryption key.Also, an active adversary can substitute and exchange individual blocks without detection, which allows blocks to be saved and inserted into the stream at other points without detection.";
                    break;
                case "OFB":
                    textBoxCipherModeDetails.Text = "The Output Feedback (OFB) mode processes small increments of plain text into cipher text instead of processing an entire block at a time. This mode is similar to CFB; the only difference between the two modes is the way that the shift register is filled. If a bit in the cipher text is mangled, the corresponding bit of plain text will be mangled. However, if there are extra or missing bits from the cipher text, the plain text will be mangled from that point on.";
                    break;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ApplySettingsFromControls();
            prefs.SavePrefs();
            this.Close();
        }

        private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            mainForm.UserPrefs = new UserPreferences();
            prefs.LoadPrefs();
            Thread.Sleep(1);
            mainForm.UserPrefs.SavePrefs();
            Thread.Sleep(1);
            prefs.LoadPrefs();
            PopulateControlsFromPreferencesFile();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplySettingsFromControls();
            prefs.SavePrefs();
        }

        private void ApplySettingsFromControls()
        {
            mainForm.UserPrefs.Salt[0] = Byte.Parse(textBoxSalt0.Text);
            mainForm.UserPrefs.Salt[1] = Byte.Parse(textBoxSalt1.Text);
            mainForm.UserPrefs.Salt[2] = Byte.Parse(textBoxSalt2.Text);
            mainForm.UserPrefs.Salt[3] = Byte.Parse(textBoxSalt3.Text);
            mainForm.UserPrefs.Salt[4] = Byte.Parse(textBoxSalt4.Text);
            mainForm.UserPrefs.Salt[5] = Byte.Parse(textBoxSalt5.Text);
            mainForm.UserPrefs.Salt[6] = Byte.Parse(textBoxSalt6.Text);
            mainForm.UserPrefs.Salt[7] = Byte.Parse(textBoxSalt7.Text);

            mainForm.UserPrefs.Iterations = Int32.Parse(textBoxIterations.Text);
            mainForm.UserPrefs.BlockSize = Int32.Parse(comboBoxBlockSize.Text);
            mainForm.UserPrefs.KeySize = Int32.Parse(comboBoxKeySize.Text);

            string mode = comboBoxCipherMode.SelectedItem.ToString();
            switch (mode)
            {
                case "CBC":
                    mainForm.UserPrefs.CipherMode = CipherMode.CBC;
                    break;
                case "CFB":
                    mainForm.UserPrefs.CipherMode = CipherMode.CFB;
                    break;
                case "CTS":
                    mainForm.UserPrefs.CipherMode = CipherMode.CTS;
                    break;
                case "ECB":
                    mainForm.UserPrefs.CipherMode = CipherMode.ECB;
                    break;
                case "OFB":
                    mainForm.UserPrefs.CipherMode = CipherMode.OFB;
                    break;
            }
        }

        private void textBoxSalt0_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt0);
        }

        private void KeepValueInByteRange(TextBox tb)
        {
            int saltByte = 0;
            if (int.TryParse(tb.Text, out saltByte))
            {
                if (saltByte < 0)
                {
                    saltByte = 0;
                    textBoxSalt0.Text = "0";
                }
                else if (saltByte > 255)
                {
                    saltByte = 255;
                    textBoxSalt0.Text = "255";
                }
            }
        }

        private void textBoxSalt1_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt1);

        }

        private void textBoxSalt2_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt2);

        }

        private void textBoxSalt3_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt3);

        }

        private void textBoxSalt4_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt4);

        }

        private void textBoxSalt5_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt5);

        }

        private void textBoxSalt6_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt6);

        }

        private void textBoxSalt7_TextChanged(object sender, EventArgs e)
        {
            KeepValueInByteRange(textBoxSalt7);
        }

        private void comboBoxCipherMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            DescribeCipherMode();

            string mode = comboBoxCipherMode.SelectedItem.ToString();
            switch (mode)
            {
                case "CTS":
                    buttonOK.Enabled = false;
                    buttonApply.Enabled = false;
                    break;
                case "OFB":
                    buttonOK.Enabled = false;
                    buttonApply.Enabled = false;
                    break;
                default:
                    buttonOK.Enabled = true;
                    buttonApply.Enabled = true;
                    break;
            }
        }
    }
}

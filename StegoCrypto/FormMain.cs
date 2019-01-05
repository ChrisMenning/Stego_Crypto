using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StegoCrypto
{
    public partial class FormMain : Form
    {
        // The private fields
        private byte[] encryptionKey;
        private FileInformation fi;
        private Image originalImage;
        private AESencrypter aes;
        private byte[] decodedBytes;
        private Bitmap encImg;
        private FormPassword fp;
        private UserPreferences userPrefs;
        private PictureBox picBoxOrig;
        private Label estimatedStorageCap;

        int headerLength;
        private int estimatedBytes;

        // The Main Form's constructor
        public FormMain()
        {
            InitializeComponent();
            encryptionKey = new byte[0];
            userPrefs = new UserPreferences();
            userPrefs.LoadPrefs();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.PicBoxOrig = pictureBoxOriginalImage;
            this.estimatedStorageCap = labelStorageCapacity;
        }

        // The Properties
        public byte[] EncryptionKey
        {
            get
            {
                return encryptionKey;
            }
            set
            {
                this.encryptionKey = value;
            }
        }

        internal UserPreferences UserPrefs
        {
            get
            {
                return userPrefs;
            }

            set
            {
                userPrefs = value;
            }
        }

        public Image OriginalImage
        {
            get
            {
                return originalImage;
            }

            set
            {
                originalImage = value;
            }
        }

        public PictureBox PicBoxOrig
        {
            get
            {
                return picBoxOrig;
            }

            set
            {
                picBoxOrig = value;
            }
        }

        public Label EstimatedStorageCap
        {
            get
            {
                return estimatedStorageCap;
            }

            set
            {
                estimatedStorageCap = value;
            }
        }

        private void OpenFileToHide()
        {
            openFileDialogSourceFile.Filter = "All files (*.*)|*.*";
            openFileDialogSourceFile.FileName = "";

            if (openFileDialogSourceFile.ShowDialog() == DialogResult.OK)
            {
                buttonSelectRawImage.Enabled = true;
                imageToEncodeToolStripMenuItem.Enabled = true;
                generateFractalToolStripMenuItem.Enabled = true;

                byte[] sourceFile = File.ReadAllBytes(openFileDialogSourceFile.FileName);
                fi = new FileInformation(openFileDialogSourceFile.SafeFileName, sourceFile);

                labelFileInfo.Text = fi.FileName + "\n" + fi.FileName.Length + " characters in file name. \n" + fi.FileContents.Length + " bytes in file. ";

                // Calculate header size.
                int headerLength = (16 + 4 + 4 + (fi.FileName.Length * 8));

                estimatedBytes = (((fi.FileContents.Length) + headerLength));
                labelEstEncSize.Text = estimatedBytes.ToString();
                fp = new FormPassword(this);
                fp.Show();
                buttonGenerateFractal.Enabled = true;
            }
        }

        public void SelectImageForHiding()
        {
            labelStorageCapacity.ForeColor = Color.Black;

            openFileDialogSourceFile.Filter = "Image files (*.png, *.jpg, *.jpeg, *.jpe, *.jfif, *.gif) | *.png; *.jpg; *.jpeg; *.jpe; *.jfif; *.gif;";
            openFileDialogSourceFile.FileName = "";

            if (openFileDialogSourceFile.ShowDialog() == DialogResult.OK)
            {
                originalImage = Image.FromFile(openFileDialogSourceFile.FileName);
                pictureBoxOriginalImage.Image = originalImage;
                pictureBoxOriginalImage.SizeMode = PictureBoxSizeMode.Zoom;

                // Measure total pixels
                int totalPixels = originalImage.Width * originalImage.Height;

                int storageCapacity = ((totalPixels / 2) - headerLength);
                labelStorageCapacity.Text = storageCapacity + " bytes";

                // Check that image is large enough to fit
                if (estimatedBytes > storageCapacity)
                {
                    labelStorageCapacity.ForeColor = Color.DarkRed;
                    // Ensure that fractal's encodable bytes are divisible by 128.
                    int pixelsNeeded = estimatedBytes * 2;
                    //int pixelsNeededToNearest128 = roundUp(pixelsNeeded, 128);
                    double SquareSize = Math.Sqrt(pixelsNeeded) + 1;
                    //int SquareToNearest128 = roundUp((int)SquareSize, 128);

                    ImageTooSmall its = new ImageTooSmall(this, (int)SquareSize);
                    its.ShowDialog();
                    buttonHideFile.Enabled = true;
                    hideFileInImageToolStripMenuItem.Enabled = true;
                }
                else
                {
                    buttonHideFile.Enabled = true;
                    hideFileInImageToolStripMenuItem.Enabled = true;
                }
            }
        }

        int roundUp(int numToRound, int multiple)
        {
            if (multiple == 0)
                return numToRound;

            int remainder = numToRound % multiple;

            if (remainder == 0)
                return numToRound;

            return numToRound + multiple - remainder;
        }

        private void CallFractalMaker()
        {
            Console.WriteLine("Need to hide " + estimatedBytes + " estimated bytes");

            // Ensure that fractal's encodable bytes are divisible by 128.
            int pixelsNeeded = estimatedBytes * 2;
            //int pixelsNeededToNearest128 = roundUp(pixelsNeeded, 128);
            int SquareSize = (int)Math.Sqrt(pixelsNeeded) + 16;
            //int SquareToNearest128 = roundUp((int)SquareSize, 128);

            GenerateFractal gf = new GenerateFractal(this, SquareSize);
            gf.ShowDialog();
            buttonHideFile.Enabled = true;
            hideFileInImageToolStripMenuItem.Enabled = true;
        }

        private async void HideFile()
        {
            aes = new AESencrypter(fi.InfoHeader, fi.FileContents, this);
            BitmapEncoder bmEnc = new BitmapEncoder(new Bitmap(originalImage));
            byte[] bytes = aes.EncryptBytes();
            // MessageBox.Show("Attempting to stuff " + bytes.Length + " bytes into " + (originalImage.Width * originalImage.Height) / 2 + " bytes of space.");
            if (bytes.Length > (originalImage.Width * originalImage.Height) / 2)
            {
                MessageBox.Show("WARNING: It looks like the file is too large to fit in the image.");
            }
            Bitmap bmp = bmEnc.EncodedBitmap(bytes, aes.InitializationVector);

            openFileDialogSourceFile.Filter = "PNG files (*.png | *.png;";

            saveFileDialog.FileName = "image.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void OpenImageToDecode()
        {
            if (openFileDialogSourceFile.ShowDialog() == DialogResult.OK)
            {
                Image encodedImage = Image.FromFile(openFileDialogSourceFile.FileName);
                pictureBoxEncodedImage.Image = encodedImage;
                pictureBoxEncodedImage.SizeMode = PictureBoxSizeMode.Zoom;

                encImg = (Bitmap)encodedImage;

                fp = new FormPassword(this);
                fp.ShowDialog();

                buttonRetrieveFile.Enabled = true;
                retrieveFileFromImageToolStripMenuItem.Enabled = true;
            }
        }

        // The buttons
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileToHide();
        }

        private void buttonSelectRawImage_Click(object sender, EventArgs e)
        {
            SelectImageForHiding();
        }

        private void buttonHideFile_Click(object sender, EventArgs e)
        {
            HideFile();
        }

        private void buttonOpenImage_Click(object sender, EventArgs e)
        {
            OpenImageToDecode();
        }

        private void buttonGenerateFractal_Click(object sender, EventArgs e)
        {
            CallFractalMaker();
        }

        private void buttonRetrieveFile_Click(object sender, EventArgs e)
        {
            RetrieveFile();
        }

        private void RetrieveFile()
        {
            encryptionKey = fp.PwHandler.EncryptionKey;

            BitmapDecoder dec = new BitmapDecoder();
            decodedBytes = dec.BytesFromImage(encImg);

            Console.WriteLine("Retrieved " + decodedBytes.Count() + " bytes from image.");

            // Get IV from first 16 bytes.
            byte[] IV = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                IV[i] = decodedBytes[i];
            }
            Console.WriteLine("Retrieved IV." + IV);
            foreach (byte b in IV)
            {
                Console.WriteLine("IV Byte: " + b.ToString());
            }

            // Trim IV off encryptedbytes.
            List<byte> stillEncryptedBytes = new List<byte>();
            for (int i = 16; i < decodedBytes.Count(); i++)
            {
                stillEncryptedBytes.Add(decodedBytes[i]);
            }

            byte[] bytes = stillEncryptedBytes.ToArray();
            Console.WriteLine("Trimmed IV off of encryped bytes.");

            // Ensure that byte array is divisible by 128.
            int bytesToNearest128 = roundUp(bytes.Length, 128);
            byte[] newBytes = new byte[bytesToNearest128];
            for (int i = 0; i < bytes.Length; i++)
            {
                newBytes[i] = bytes[i];
            }
            bytes = newBytes;

            // Decrypt
            Console.WriteLine("Decrypting.");

            AESdecrypter aesdec = new AESdecrypter(this);

            byte[] decryptedBytes = aesdec.DecryptedBytes(bytes, encryptionKey, IV);
            Console.WriteLine("Decrypted " + decryptedBytes.Count() + " from image.");

            // Parse the header 
            HeaderParser hp = new HeaderParser();
            byte[] file = hp.fileContentsWithoutHeader(decryptedBytes);
            string fileName = hp.FileName;
            labelFileInfo2.Text = ("Original file name: " + fileName + "\n Bytes: " + file.Length);
            this.Refresh();

            Console.WriteLine("Saving " + fileName);

            saveFileDialog.FileName = fileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog.FileName, file);

                if (checkBoxOpenAfterSave.Checked)
                    Process.Start(saveFileDialog.FileName);
            }
        }

        private void radioButtonHideFile_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonHideFile.Checked)
            {
                groupBoxHide.Enabled = true;
                groupBoxShow.Enabled = false;
            }
            else
            {
                groupBoxHide.Enabled = false;
                groupBoxShow.Enabled = true;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings(this);
            settings.ShowDialog();
        }

        private void fileToEncryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            radioButtonHideFile.Checked = true;
            this.Refresh();
            OpenFileToHide(); 
        }

        private void imageToEncodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            radioButtonHideFile.Checked = true;
            this.Refresh();
            SelectImageForHiding();
        }

        private void generateFractalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CallFractalMaker();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.Show();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            encryptionKey = new byte[0];
            fi = null;
            originalImage = null;
            decodedBytes = null;
            encImg = null;
            picBoxOrig.Image = null;
            pictureBoxEncodedImage.Image = null;
            EstimatedStorageCap.Text = "";
            labelFileInfo.Text = "";
            labelFileInfo2.Text = "";

            buttonSelectRawImage.Enabled = false;
            buttonGenerateFractal.Enabled = false;
            buttonHideFile.Enabled = false;
            buttonRetrieveFile.Enabled = false;
            hideFileInImageToolStripMenuItem.Enabled = false;
            retrieveFileFromImageToolStripMenuItem.Enabled = false;
            imageToEncodeToolStripMenuItem.Enabled = false;
            generateFractalToolStripMenuItem.Enabled = false;
        }

        private void hideFileInImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideFile();
        }

        private void retrieveFileFromImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RetrieveFile();
        }

        private void imageToDecodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenImageToDecode();
        }
    }
}

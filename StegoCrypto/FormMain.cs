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
        // The private fields.
        private byte[] encryptionKey;
        private FileInformation fi;
        private Bitmap originalImage;
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
            tabControl.Selecting += new TabControlCancelEventHandler(tabControl_Selecting);
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

        public UserPreferences UserPrefs
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

        public Bitmap OriginalImage
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

        // The methods that do the work.
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
                headerLength = (16 + 4 + 4 + (fi.FileName.Length * 8));

                estimatedBytes = (((fi.FileContents.Length) + headerLength));
                labelEstEncSize.Text = estimatedBytes.ToString();
                fp = new FormPassword(this);
                fp.Show();
                buttonGenerateFractal.Enabled = true;
                tabControl.SelectTab(0);
            }
        }

        public void SelectImageForHiding()
        {
            labelStorageCapacity.ForeColor = Color.Black;

            openFileDialogSourceFile.Filter = "Image files (*.png, *.jpg, *.jpeg, *.jpe, *.jfif, *.gif) | *.png; *.jpg; *.jpeg; *.jpe; *.jfif; *.gif;";
            openFileDialogSourceFile.FileName = "";

            if (openFileDialogSourceFile.ShowDialog() == DialogResult.OK)
            {
                originalImage = (Bitmap)Image.FromFile(openFileDialogSourceFile.FileName);
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
                    int pixelsNeeded = estimatedBytes * 2;
                    double SquareSize = Math.Sqrt(pixelsNeeded) + 1;

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

        private int RoundUp(int numToRound, int multiple)
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

            int pixelsNeeded = estimatedBytes * 2;
            int SquareSize = (int)Math.Sqrt(pixelsNeeded) + 16; // Extra 16 for the Initialization Vector

            GenerateFractal gf = new GenerateFractal(this, SquareSize);
            gf.ShowDialog();
            buttonHideFile.Enabled = true;
            hideFileInImageToolStripMenuItem.Enabled = true;
        }

        private async void HideFile()
        {
            aes = new AESencrypter(fi.InfoHeader, fi.FileContents, this);
            BitmapEncoder bmEnc = new BitmapEncoder(originalImage);
            byte[] bytes = aes.EncryptBytes();
            byte[] IV = aes.InitializationVector;
            GC.Collect();
            if (bytes.Length > (originalImage.Width * originalImage.Height) / 2)
            {
                MessageBox.Show("WARNING: It looks like the file is too large to fit in the image.");
            }
            Bitmap bmp = await bmEnc.EncodedBitmap(bytes, IV);

            openFileDialogSourceFile.Filter = "PNG files (*.png | *.png;";
            saveFileDialog.FileName = "image.png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }

            GC.Collect();
        }

        private void OpenImageToDecode()
        {
            if (openFileDialogSourceFile.ShowDialog() == DialogResult.OK)
            {
                encImg = (Bitmap)Image.FromFile(openFileDialogSourceFile.FileName);
                pictureBoxEncodedImage.Image = encImg;
                pictureBoxEncodedImage.SizeMode = PictureBoxSizeMode.Zoom;

                fp = new FormPassword(this);
                fp.ShowDialog();

                buttonRetrieveFile.Enabled = true;
                retrieveFileFromImageToolStripMenuItem.Enabled = true;
                tabControl.SelectTab(1);
            }
        }

        private async void RetrieveFile()
        {
            encryptionKey = fp.PwHandler.EncryptionKey;

            BitmapDecoder dec = new BitmapDecoder();
            decodedBytes = await dec.BytesFromImage(encImg);

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
            // NOTE: This is CRUCIAL. Failing to do so makes encryption fail on images with odd number of pixels.
            int bytesToNearest128 = RoundUp(bytes.Length, 128);
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

            // Force garbage collection here.
            GC.Collect(); 

            Console.WriteLine("Saving " + fileName);

            saveFileDialog.FileName = fileName;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog.FileName, file);

                if (checkBoxOpenAfterSave.Checked)
                    Process.Start(saveFileDialog.FileName);
            }
        }

        private void ClearEverything()
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

        // The toolstrips
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings(this);
            settings.ShowDialog();
        }

        private void fileToEncryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(0);
            this.Refresh();
            OpenFileToHide(); 
        }

        private void imageToEncodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.SelectTab(0);
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
            ClearEverything();
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

        void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage current = (sender as TabControl).SelectedTab;

            ClearEverything();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

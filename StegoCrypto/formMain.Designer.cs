namespace StegoCrypto
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToEncryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToEncodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateFractalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.imageToDecodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.hideFileInImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retrieveFileFromImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialogSourceFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageHide = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonHideFile = new System.Windows.Forms.Button();
            this.buttonGenerateFractal = new System.Windows.Forms.Button();
            this.pictureBoxOriginalImage = new System.Windows.Forms.PictureBox();
            this.buttonSelectRawImage = new System.Windows.Forms.Button();
            this.groupBoxHide = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelFileInfo = new System.Windows.Forms.Label();
            this.labelEstEncSize = new System.Windows.Forms.Label();
            this.labelStorageCapacity = new System.Windows.Forms.Label();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.tabPageShow = new System.Windows.Forms.TabPage();
            this.groupBoxShow = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelFileInfo2 = new System.Windows.Forms.Label();
            this.checkBoxOpenAfterSave = new System.Windows.Forms.CheckBox();
            this.buttonRetrieveFile = new System.Windows.Forms.Button();
            this.pictureBoxEncodedImage = new System.Windows.Forms.PictureBox();
            this.buttonOpenImage = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageHide.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalImage)).BeginInit();
            this.groupBoxHide.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageShow.SuspendLayout();
            this.groupBoxShow.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEncodedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.LightBlue;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(788, 31);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator2,
            this.hideFileInImageToolStripMenuItem,
            this.retrieveFileFromImageToolStripMenuItem,
            this.toolStripSeparator3,
            this.clearAllToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 27);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToEncryptToolStripMenuItem,
            this.imageToEncodeToolStripMenuItem,
            this.generateFractalToolStripMenuItem,
            this.toolStripSeparator1,
            this.imageToDecodeToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // fileToEncryptToolStripMenuItem
            // 
            this.fileToEncryptToolStripMenuItem.Name = "fileToEncryptToolStripMenuItem";
            this.fileToEncryptToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.fileToEncryptToolStripMenuItem.Text = "File to &Encrypt";
            this.fileToEncryptToolStripMenuItem.ToolTipText = "The source file that you want to hide.";
            this.fileToEncryptToolStripMenuItem.Click += new System.EventHandler(this.fileToEncryptToolStripMenuItem_Click);
            // 
            // imageToEncodeToolStripMenuItem
            // 
            this.imageToEncodeToolStripMenuItem.Enabled = false;
            this.imageToEncodeToolStripMenuItem.Name = "imageToEncodeToolStripMenuItem";
            this.imageToEncodeToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.imageToEncodeToolStripMenuItem.Text = "&Image to Encode";
            this.imageToEncodeToolStripMenuItem.ToolTipText = "Open an image for hiding the file inside.";
            this.imageToEncodeToolStripMenuItem.Click += new System.EventHandler(this.imageToEncodeToolStripMenuItem_Click);
            // 
            // generateFractalToolStripMenuItem
            // 
            this.generateFractalToolStripMenuItem.Enabled = false;
            this.generateFractalToolStripMenuItem.Name = "generateFractalToolStripMenuItem";
            this.generateFractalToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.generateFractalToolStripMenuItem.Text = "Generate &Fractal";
            this.generateFractalToolStripMenuItem.ToolTipText = "Generate a fractal for hiding the file inside.";
            this.generateFractalToolStripMenuItem.Click += new System.EventHandler(this.generateFractalToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(200, 6);
            // 
            // imageToDecodeToolStripMenuItem
            // 
            this.imageToDecodeToolStripMenuItem.Name = "imageToDecodeToolStripMenuItem";
            this.imageToDecodeToolStripMenuItem.Size = new System.Drawing.Size(203, 26);
            this.imageToDecodeToolStripMenuItem.Text = "Image to &Decode";
            this.imageToDecodeToolStripMenuItem.Click += new System.EventHandler(this.imageToDecodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(254, 6);
            // 
            // hideFileInImageToolStripMenuItem
            // 
            this.hideFileInImageToolStripMenuItem.Enabled = false;
            this.hideFileInImageToolStripMenuItem.Name = "hideFileInImageToolStripMenuItem";
            this.hideFileInImageToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.hideFileInImageToolStripMenuItem.Text = "&Hide File in Image";
            this.hideFileInImageToolStripMenuItem.Click += new System.EventHandler(this.hideFileInImageToolStripMenuItem_Click);
            // 
            // retrieveFileFromImageToolStripMenuItem
            // 
            this.retrieveFileFromImageToolStripMenuItem.Enabled = false;
            this.retrieveFileFromImageToolStripMenuItem.Name = "retrieveFileFromImageToolStripMenuItem";
            this.retrieveFileFromImageToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.retrieveFileFromImageToolStripMenuItem.Text = "&Retrieve File from Image";
            this.retrieveFileFromImageToolStripMenuItem.Click += new System.EventHandler(this.retrieveFileFromImageToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(254, 6);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.clearAllToolStripMenuItem.Text = "&Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(257, 26);
            this.quitToolStripMenuItem.Text = "&Quit";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(83, 27);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(58, 27);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // openFileDialogSourceFile
            // 
            this.openFileDialogSourceFile.FileName = "openFileDialog1";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageHide);
            this.tabControl.Controls.Add(this.tabPageShow);
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.Location = new System.Drawing.Point(12, 43);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(763, 449);
            this.tabControl.TabIndex = 8;
            // 
            // tabPageHide
            // 
            this.tabPageHide.BackColor = System.Drawing.Color.MintCream;
            this.tabPageHide.Controls.Add(this.groupBox1);
            this.tabPageHide.Controls.Add(this.groupBoxHide);
            this.tabPageHide.Location = new System.Drawing.Point(4, 29);
            this.tabPageHide.Name = "tabPageHide";
            this.tabPageHide.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHide.Size = new System.Drawing.Size(755, 416);
            this.tabPageHide.TabIndex = 0;
            this.tabPageHide.Text = "Hide File in Image";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.buttonHideFile);
            this.groupBox1.Controls.Add(this.buttonGenerateFractal);
            this.groupBox1.Controls.Add(this.pictureBoxOriginalImage);
            this.groupBox1.Controls.Add(this.buttonSelectRawImage);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(382, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 408);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Image";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(173, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "or";
            // 
            // buttonHideFile
            // 
            this.buttonHideFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonHideFile.Enabled = false;
            this.buttonHideFile.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonHideFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHideFile.Location = new System.Drawing.Point(13, 349);
            this.buttonHideFile.Name = "buttonHideFile";
            this.buttonHideFile.Size = new System.Drawing.Size(348, 53);
            this.buttonHideFile.TabIndex = 4;
            this.buttonHideFile.Text = "Hide File in Image";
            this.buttonHideFile.UseVisualStyleBackColor = false;
            this.buttonHideFile.Click += new System.EventHandler(this.buttonHideFile_Click);
            // 
            // buttonGenerateFractal
            // 
            this.buttonGenerateFractal.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonGenerateFractal.Enabled = false;
            this.buttonGenerateFractal.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonGenerateFractal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGenerateFractal.Location = new System.Drawing.Point(215, 22);
            this.buttonGenerateFractal.Name = "buttonGenerateFractal";
            this.buttonGenerateFractal.Size = new System.Drawing.Size(146, 49);
            this.buttonGenerateFractal.TabIndex = 2;
            this.buttonGenerateFractal.Text = "Generate Fractal";
            this.buttonGenerateFractal.UseVisualStyleBackColor = false;
            this.buttonGenerateFractal.Click += new System.EventHandler(this.buttonGenerateFractal_Click);
            // 
            // pictureBoxOriginalImage
            // 
            this.pictureBoxOriginalImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxOriginalImage.Location = new System.Drawing.Point(13, 77);
            this.pictureBoxOriginalImage.Name = "pictureBoxOriginalImage";
            this.pictureBoxOriginalImage.Size = new System.Drawing.Size(348, 266);
            this.pictureBoxOriginalImage.TabIndex = 3;
            this.pictureBoxOriginalImage.TabStop = false;
            // 
            // buttonSelectRawImage
            // 
            this.buttonSelectRawImage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonSelectRawImage.Enabled = false;
            this.buttonSelectRawImage.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonSelectRawImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectRawImage.Location = new System.Drawing.Point(13, 22);
            this.buttonSelectRawImage.Name = "buttonSelectRawImage";
            this.buttonSelectRawImage.Size = new System.Drawing.Size(141, 49);
            this.buttonSelectRawImage.TabIndex = 1;
            this.buttonSelectRawImage.Text = "Select Image";
            this.buttonSelectRawImage.UseVisualStyleBackColor = false;
            this.buttonSelectRawImage.Click += new System.EventHandler(this.buttonSelectRawImage_Click);
            // 
            // groupBoxHide
            // 
            this.groupBoxHide.Controls.Add(this.label2);
            this.groupBoxHide.Controls.Add(this.label1);
            this.groupBoxHide.Controls.Add(this.panel1);
            this.groupBoxHide.Controls.Add(this.labelEstEncSize);
            this.groupBoxHide.Controls.Add(this.labelStorageCapacity);
            this.groupBoxHide.Controls.Add(this.btnOpenFile);
            this.groupBoxHide.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxHide.Location = new System.Drawing.Point(11, 6);
            this.groupBoxHide.Name = "groupBoxHide";
            this.groupBoxHide.Size = new System.Drawing.Size(365, 408);
            this.groupBoxHide.TabIndex = 4;
            this.groupBoxHide.TabStop = false;
            this.groupBoxHide.Text = "Open File to Hide";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 378);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "Image Storage Capacity:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 349);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Estimated Encrypted Size:";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelFileInfo);
            this.panel1.Location = new System.Drawing.Point(9, 77);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 266);
            this.panel1.TabIndex = 7;
            // 
            // labelFileInfo
            // 
            this.labelFileInfo.AutoSize = true;
            this.labelFileInfo.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFileInfo.Location = new System.Drawing.Point(3, 9);
            this.labelFileInfo.Name = "labelFileInfo";
            this.labelFileInfo.Size = new System.Drawing.Size(57, 19);
            this.labelFileInfo.TabIndex = 1;
            this.labelFileInfo.Text = "File Info";
            // 
            // labelEstEncSize
            // 
            this.labelEstEncSize.AutoSize = true;
            this.labelEstEncSize.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEstEncSize.Location = new System.Drawing.Point(169, 349);
            this.labelEstEncSize.Name = "labelEstEncSize";
            this.labelEstEncSize.Size = new System.Drawing.Size(12, 19);
            this.labelEstEncSize.TabIndex = 6;
            this.labelEstEncSize.Text = "|";
            // 
            // labelStorageCapacity
            // 
            this.labelStorageCapacity.AutoSize = true;
            this.labelStorageCapacity.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStorageCapacity.Location = new System.Drawing.Point(169, 378);
            this.labelStorageCapacity.Name = "labelStorageCapacity";
            this.labelStorageCapacity.Size = new System.Drawing.Size(12, 19);
            this.labelStorageCapacity.TabIndex = 5;
            this.labelStorageCapacity.Text = "|";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOpenFile.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFile.Location = new System.Drawing.Point(9, 21);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(92, 50);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // tabPageShow
            // 
            this.tabPageShow.BackColor = System.Drawing.Color.MintCream;
            this.tabPageShow.Controls.Add(this.groupBoxShow);
            this.tabPageShow.Location = new System.Drawing.Point(4, 29);
            this.tabPageShow.Name = "tabPageShow";
            this.tabPageShow.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageShow.Size = new System.Drawing.Size(755, 416);
            this.tabPageShow.TabIndex = 1;
            this.tabPageShow.Text = "Retrieve File from Image";
            // 
            // groupBoxShow
            // 
            this.groupBoxShow.Controls.Add(this.panel2);
            this.groupBoxShow.Controls.Add(this.checkBoxOpenAfterSave);
            this.groupBoxShow.Controls.Add(this.buttonRetrieveFile);
            this.groupBoxShow.Controls.Add(this.pictureBoxEncodedImage);
            this.groupBoxShow.Controls.Add(this.buttonOpenImage);
            this.groupBoxShow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxShow.Location = new System.Drawing.Point(6, 6);
            this.groupBoxShow.Name = "groupBoxShow";
            this.groupBoxShow.Size = new System.Drawing.Size(743, 408);
            this.groupBoxShow.TabIndex = 5;
            this.groupBoxShow.TabStop = false;
            this.groupBoxShow.Text = "Retrieve File";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelFileInfo2);
            this.panel2.Location = new System.Drawing.Point(126, 304);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(604, 68);
            this.panel2.TabIndex = 5;
            // 
            // labelFileInfo2
            // 
            this.labelFileInfo2.AutoSize = true;
            this.labelFileInfo2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFileInfo2.Location = new System.Drawing.Point(3, 0);
            this.labelFileInfo2.Name = "labelFileInfo2";
            this.labelFileInfo2.Size = new System.Drawing.Size(57, 19);
            this.labelFileInfo2.TabIndex = 4;
            this.labelFileInfo2.Text = "File Info";
            // 
            // checkBoxOpenAfterSave
            // 
            this.checkBoxOpenAfterSave.AutoSize = true;
            this.checkBoxOpenAfterSave.Checked = true;
            this.checkBoxOpenAfterSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOpenAfterSave.Location = new System.Drawing.Point(13, 378);
            this.checkBoxOpenAfterSave.Name = "checkBoxOpenAfterSave";
            this.checkBoxOpenAfterSave.Size = new System.Drawing.Size(173, 24);
            this.checkBoxOpenAfterSave.TabIndex = 3;
            this.checkBoxOpenAfterSave.Text = "Open file after saving";
            this.checkBoxOpenAfterSave.UseVisualStyleBackColor = true;
            // 
            // buttonRetrieveFile
            // 
            this.buttonRetrieveFile.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonRetrieveFile.Enabled = false;
            this.buttonRetrieveFile.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonRetrieveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRetrieveFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRetrieveFile.Location = new System.Drawing.Point(6, 304);
            this.buttonRetrieveFile.Name = "buttonRetrieveFile";
            this.buttonRetrieveFile.Size = new System.Drawing.Size(114, 68);
            this.buttonRetrieveFile.TabIndex = 5;
            this.buttonRetrieveFile.Text = "Retrieve File";
            this.buttonRetrieveFile.UseVisualStyleBackColor = false;
            this.buttonRetrieveFile.Click += new System.EventHandler(this.buttonRetrieveFile_Click);
            // 
            // pictureBoxEncodedImage
            // 
            this.pictureBoxEncodedImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxEncodedImage.Location = new System.Drawing.Point(126, 26);
            this.pictureBoxEncodedImage.Name = "pictureBoxEncodedImage";
            this.pictureBoxEncodedImage.Size = new System.Drawing.Size(604, 272);
            this.pictureBoxEncodedImage.TabIndex = 1;
            this.pictureBoxEncodedImage.TabStop = false;
            // 
            // buttonOpenImage
            // 
            this.buttonOpenImage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonOpenImage.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.buttonOpenImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenImage.Location = new System.Drawing.Point(6, 26);
            this.buttonOpenImage.Name = "buttonOpenImage";
            this.buttonOpenImage.Size = new System.Drawing.Size(114, 77);
            this.buttonOpenImage.TabIndex = 4;
            this.buttonOpenImage.Text = "Open Image";
            this.buttonOpenImage.UseVisualStyleBackColor = false;
            this.buttonOpenImage.Click += new System.EventHandler(this.buttonOpenImage_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(788, 498);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StegoCrypto";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageHide.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalImage)).EndInit();
            this.groupBoxHide.ResumeLayout(false);
            this.groupBoxHide.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageShow.ResumeLayout(false);
            this.groupBoxShow.ResumeLayout(false);
            this.groupBoxShow.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEncodedImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToEncryptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToDecodeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialogSourceFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem imageToEncodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateFractalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem hideFileInImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retrieveFileFromImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageHide;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonHideFile;
        private System.Windows.Forms.Button buttonGenerateFractal;
        private System.Windows.Forms.PictureBox pictureBoxOriginalImage;
        private System.Windows.Forms.Button buttonSelectRawImage;
        private System.Windows.Forms.GroupBox groupBoxHide;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelFileInfo;
        private System.Windows.Forms.Label labelEstEncSize;
        private System.Windows.Forms.Label labelStorageCapacity;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TabPage tabPageShow;
        private System.Windows.Forms.GroupBox groupBoxShow;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelFileInfo2;
        private System.Windows.Forms.CheckBox checkBoxOpenAfterSave;
        private System.Windows.Forms.Button buttonRetrieveFile;
        private System.Windows.Forms.PictureBox pictureBoxEncodedImage;
        private System.Windows.Forms.Button buttonOpenImage;
    }
}


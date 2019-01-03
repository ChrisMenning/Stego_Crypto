namespace StegoCrypto
{
    partial class ImageTooSmall
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
            this.buttonGenerateFractal = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonOpenAnotherImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGenerateFractal
            // 
            this.buttonGenerateFractal.Location = new System.Drawing.Point(12, 117);
            this.buttonGenerateFractal.Name = "buttonGenerateFractal";
            this.buttonGenerateFractal.Size = new System.Drawing.Size(91, 80);
            this.buttonGenerateFractal.TabIndex = 0;
            this.buttonGenerateFractal.Text = "Generate Fractal Image";
            this.buttonGenerateFractal.UseVisualStyleBackColor = true;
            this.buttonGenerateFractal.Click += new System.EventHandler(this.buttonGenerateFractal_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(206, 117);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(91, 80);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(12, 19);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(286, 85);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "The selected image is too small to store the \r\nsource file.\r\n\r\nIf you\'d like, you" +
    " can generate a fractal\r\nimage at the necessary size.\r\n";
            // 
            // buttonOpenAnotherImage
            // 
            this.buttonOpenAnotherImage.Location = new System.Drawing.Point(109, 117);
            this.buttonOpenAnotherImage.Name = "buttonOpenAnotherImage";
            this.buttonOpenAnotherImage.Size = new System.Drawing.Size(91, 80);
            this.buttonOpenAnotherImage.TabIndex = 1;
            this.buttonOpenAnotherImage.Text = "Open Another Image";
            this.buttonOpenAnotherImage.UseVisualStyleBackColor = true;
            this.buttonOpenAnotherImage.Click += new System.EventHandler(this.buttonOpenAnotherImage_Click);
            // 
            // ImageTooSmall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 213);
            this.ControlBox = false;
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOpenAnotherImage);
            this.Controls.Add(this.buttonGenerateFractal);
            this.Name = "ImageTooSmall";
            this.Text = "Image Too Small";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerateFractal;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button buttonOpenAnotherImage;
    }
}
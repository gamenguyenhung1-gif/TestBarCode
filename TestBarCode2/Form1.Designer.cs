namespace TestBarCode2
{
    partial class Form1
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
            this.picImage = new System.Windows.Forms.PictureBox();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.btnReadBarcode = new System.Windows.Forms.Button();
            this.lvRoi = new System.Windows.Forms.TreeView();
            this.cbxBarcodeFormat = new System.Windows.Forms.ComboBox();
            this.btnDeleteROI = new System.Windows.Forms.Button();
            this.btnSaveJson = new System.Windows.Forms.Button();
            this.btnLoadJson = new System.Windows.Forms.Button();
            this.roiX = new System.Windows.Forms.TextBox();
            this.roiH = new System.Windows.Forms.TextBox();
            this.roiY = new System.Windows.Forms.TextBox();
            this.roiW = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(12, 12);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(397, 426);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(436, 12);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(75, 23);
            this.btnLoadImage.TabIndex = 1;
            this.btnLoadImage.Text = "LoadImg";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnReadBarcode
            // 
            this.btnReadBarcode.Location = new System.Drawing.Point(594, 12);
            this.btnReadBarcode.Name = "btnReadBarcode";
            this.btnReadBarcode.Size = new System.Drawing.Size(75, 23);
            this.btnReadBarcode.TabIndex = 3;
            this.btnReadBarcode.Text = "btnReadBarcode";
            this.btnReadBarcode.UseVisualStyleBackColor = true;
            this.btnReadBarcode.Click += new System.EventHandler(this.btnReadBarcode_Click);
            // 
            // lvRoi
            // 
            this.lvRoi.CheckBoxes = true;
            this.lvRoi.Location = new System.Drawing.Point(469, 94);
            this.lvRoi.Name = "lvRoi";
            this.lvRoi.Size = new System.Drawing.Size(151, 205);
            this.lvRoi.TabIndex = 4;
            // 
            // cbxBarcodeFormat
            // 
            this.cbxBarcodeFormat.FormattingEnabled = true;
            this.cbxBarcodeFormat.Location = new System.Drawing.Point(655, 255);
            this.cbxBarcodeFormat.Name = "cbxBarcodeFormat";
            this.cbxBarcodeFormat.Size = new System.Drawing.Size(121, 21);
            this.cbxBarcodeFormat.TabIndex = 5;
            // 
            // btnDeleteROI
            // 
            this.btnDeleteROI.Location = new System.Drawing.Point(484, 65);
            this.btnDeleteROI.Name = "btnDeleteROI";
            this.btnDeleteROI.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteROI.TabIndex = 6;
            this.btnDeleteROI.Text = "btnDeleteROI";
            this.btnDeleteROI.UseVisualStyleBackColor = true;
            // 
            // btnSaveJson
            // 
            this.btnSaveJson.Location = new System.Drawing.Point(483, 344);
            this.btnSaveJson.Name = "btnSaveJson";
            this.btnSaveJson.Size = new System.Drawing.Size(75, 23);
            this.btnSaveJson.TabIndex = 7;
            this.btnSaveJson.Text = "save";
            this.btnSaveJson.UseVisualStyleBackColor = true;
            this.btnSaveJson.Click += new System.EventHandler(this.btnSaveJson_Click);
            // 
            // btnLoadJson
            // 
            this.btnLoadJson.Location = new System.Drawing.Point(594, 344);
            this.btnLoadJson.Name = "btnLoadJson";
            this.btnLoadJson.Size = new System.Drawing.Size(75, 23);
            this.btnLoadJson.TabIndex = 8;
            this.btnLoadJson.Text = "load";
            this.btnLoadJson.UseVisualStyleBackColor = true;
            this.btnLoadJson.Click += new System.EventHandler(this.btnLoadJson_Click);
            // 
            // roiX
            // 
            this.roiX.Location = new System.Drawing.Point(676, 101);
            this.roiX.Name = "roiX";
            this.roiX.Size = new System.Drawing.Size(100, 20);
            this.roiX.TabIndex = 9;
            // 
            // roiH
            // 
            this.roiH.Location = new System.Drawing.Point(676, 179);
            this.roiH.Name = "roiH";
            this.roiH.Size = new System.Drawing.Size(100, 20);
            this.roiH.TabIndex = 10;
            // 
            // roiY
            // 
            this.roiY.Location = new System.Drawing.Point(676, 127);
            this.roiY.Name = "roiY";
            this.roiY.Size = new System.Drawing.Size(100, 20);
            this.roiY.TabIndex = 11;
            // 
            // roiW
            // 
            this.roiW.Location = new System.Drawing.Point(676, 153);
            this.roiW.Name = "roiW";
            this.roiW.Size = new System.Drawing.Size(100, 20);
            this.roiW.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.roiW);
            this.Controls.Add(this.roiY);
            this.Controls.Add(this.roiH);
            this.Controls.Add(this.roiX);
            this.Controls.Add(this.btnLoadJson);
            this.Controls.Add(this.btnSaveJson);
            this.Controls.Add(this.btnDeleteROI);
            this.Controls.Add(this.cbxBarcodeFormat);
            this.Controls.Add(this.lvRoi);
            this.Controls.Add(this.btnReadBarcode);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.picImage);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.Button btnReadBarcode;
        private System.Windows.Forms.TreeView lvRoi;
        private System.Windows.Forms.ComboBox cbxBarcodeFormat;
        private System.Windows.Forms.Button btnDeleteROI;
        private System.Windows.Forms.Button btnSaveJson;
        private System.Windows.Forms.Button btnLoadJson;
        private System.Windows.Forms.TextBox roiX;
        private System.Windows.Forms.TextBox roiH;
        private System.Windows.Forms.TextBox roiY;
        private System.Windows.Forms.TextBox roiW;
    }
}


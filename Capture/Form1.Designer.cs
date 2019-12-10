namespace Capture
{
    partial class CaptureForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.btnCaptureFF = new System.Windows.Forms.Button();
            this.btnCaptureGecko = new System.Windows.Forms.Button();
            this.btnCaptureIE = new System.Windows.Forms.Button();
            this.imgPreview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnVirtualIE = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.browser);
            this.panel1.Location = new System.Drawing.Point(3, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(667, 667);
            this.panel1.TabIndex = 0;
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(667, 667);
            this.browser.TabIndex = 5;
            // 
            // btnCaptureFF
            // 
            this.btnCaptureFF.Location = new System.Drawing.Point(447, 4);
            this.btnCaptureFF.Name = "btnCaptureFF";
            this.btnCaptureFF.Size = new System.Drawing.Size(141, 23);
            this.btnCaptureFF.TabIndex = 7;
            this.btnCaptureFF.Text = "Capture Test";
            this.btnCaptureFF.UseVisualStyleBackColor = true;
            this.btnCaptureFF.Click += new System.EventHandler(this.btnCaptureOther_Click);
            // 
            // btnCaptureGecko
            // 
            this.btnCaptureGecko.Location = new System.Drawing.Point(299, 4);
            this.btnCaptureGecko.Name = "btnCaptureGecko";
            this.btnCaptureGecko.Size = new System.Drawing.Size(141, 23);
            this.btnCaptureGecko.TabIndex = 6;
            this.btnCaptureGecko.Text = "Capture Gecko";
            this.btnCaptureGecko.UseVisualStyleBackColor = true;
            this.btnCaptureGecko.Click += new System.EventHandler(this.btnCaptureGecko_Click);
            // 
            // btnCaptureIE
            // 
            this.btnCaptureIE.Location = new System.Drawing.Point(6, 4);
            this.btnCaptureIE.Name = "btnCaptureIE";
            this.btnCaptureIE.Size = new System.Drawing.Size(141, 23);
            this.btnCaptureIE.TabIndex = 5;
            this.btnCaptureIE.Text = "Capture IE";
            this.btnCaptureIE.UseVisualStyleBackColor = true;
            this.btnCaptureIE.Click += new System.EventHandler(this.btnCaptureIE_Click);
            // 
            // imgPreview
            // 
            this.imgPreview.Location = new System.Drawing.Point(676, 36);
            this.imgPreview.Name = "imgPreview";
            this.imgPreview.Size = new System.Drawing.Size(385, 667);
            this.imgPreview.TabIndex = 8;
            this.imgPreview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(677, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Preview";
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(594, 4);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 10;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnVirtualIE
            // 
            this.btnVirtualIE.Location = new System.Drawing.Point(152, 4);
            this.btnVirtualIE.Name = "btnVirtualIE";
            this.btnVirtualIE.Size = new System.Drawing.Size(141, 23);
            this.btnVirtualIE.TabIndex = 5;
            this.btnVirtualIE.Text = "Capture Virtual IE";
            this.btnVirtualIE.UseVisualStyleBackColor = true;
            this.btnVirtualIE.Click += new System.EventHandler(this.btnCaptureVirtualIE_Click);
            // 
            // CaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 715);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imgPreview);
            this.Controls.Add(this.btnCaptureFF);
            this.Controls.Add(this.btnCaptureGecko);
            this.Controls.Add(this.btnVirtualIE);
            this.Controls.Add(this.btnCaptureIE);
            this.Controls.Add(this.panel1);
            this.Name = "CaptureForm";
            this.Text = "Capture";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.Button btnCaptureFF;
        private System.Windows.Forms.Button btnCaptureGecko;
        private System.Windows.Forms.Button btnCaptureIE;
        private System.Windows.Forms.PictureBox imgPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnVirtualIE;
    }
}


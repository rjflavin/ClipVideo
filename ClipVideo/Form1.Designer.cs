namespace ClipVideo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.clipVideoTab = new System.Windows.Forms.TabPage();
            this.combineVideoTab = new System.Windows.Forms.TabPage();
            this.axWindowsMediaPlayer2 = new AxWMPLib.AxWindowsMediaPlayer();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.loadButton = new System.Windows.Forms.Button();
            this.createClipButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.clipVideoTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.clipVideoTab);
            this.tabControl1.Controls.Add(this.combineVideoTab);
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1814, 837);
            this.tabControl1.TabIndex = 4;
            // 
            // clipVideoTab
            // 
            this.clipVideoTab.Controls.Add(this.createClipButton);
            this.clipVideoTab.Controls.Add(this.loadButton);
            this.clipVideoTab.Controls.Add(this.axWindowsMediaPlayer1);
            this.clipVideoTab.Controls.Add(this.axWindowsMediaPlayer2);
            this.clipVideoTab.Location = new System.Drawing.Point(4, 25);
            this.clipVideoTab.Name = "clipVideoTab";
            this.clipVideoTab.Padding = new System.Windows.Forms.Padding(3);
            this.clipVideoTab.Size = new System.Drawing.Size(1806, 808);
            this.clipVideoTab.TabIndex = 0;
            this.clipVideoTab.Text = "Clip Video";
            this.clipVideoTab.UseVisualStyleBackColor = true;
            // 
            // combineVideoTab
            // 
            this.combineVideoTab.Location = new System.Drawing.Point(4, 25);
            this.combineVideoTab.Name = "combineVideoTab";
            this.combineVideoTab.Padding = new System.Windows.Forms.Padding(3);
            this.combineVideoTab.Size = new System.Drawing.Size(1806, 808);
            this.combineVideoTab.TabIndex = 1;
            this.combineVideoTab.Text = "Combine Video";
            this.combineVideoTab.UseVisualStyleBackColor = true;
            // 
            // axWindowsMediaPlayer2
            // 
            this.axWindowsMediaPlayer2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.axWindowsMediaPlayer2.Enabled = true;
            this.axWindowsMediaPlayer2.Location = new System.Drawing.Point(874, 7);
            this.axWindowsMediaPlayer2.Name = "axWindowsMediaPlayer2";
            this.axWindowsMediaPlayer2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer2.OcxState")));
            this.axWindowsMediaPlayer2.Size = new System.Drawing.Size(919, 691);
            this.axWindowsMediaPlayer2.TabIndex = 2;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(7, 7);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(861, 695);
            this.axWindowsMediaPlayer1.TabIndex = 3;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(6, 704);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(862, 92);
            this.loadButton.TabIndex = 4;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // createClipButton
            // 
            this.createClipButton.Location = new System.Drawing.Point(874, 703);
            this.createClipButton.Name = "createClipButton";
            this.createClipButton.Size = new System.Drawing.Size(925, 92);
            this.createClipButton.TabIndex = 5;
            this.createClipButton.Text = "Create Clip";
            this.createClipButton.UseVisualStyleBackColor = true;
            this.createClipButton.Click += new System.EventHandler(this.createClipButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1822, 845);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "ClipVideo";
            this.ResizeBegin += new System.EventHandler(this.Form1_ResizeEnd);
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.Form1_ResizeEnd);
            this.tabControl1.ResumeLayout(false);
            this.clipVideoTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage clipVideoTab;
        private System.Windows.Forms.Button createClipButton;
        private System.Windows.Forms.Button loadButton;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer2;
        private System.Windows.Forms.TabPage combineVideoTab;
        public System.Windows.Forms.TabControl tabControl1;
    }
}


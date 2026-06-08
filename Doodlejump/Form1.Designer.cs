namespace Doodlejump
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBoxPlayer = new PictureBox();
            pictureBoxPlatform = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlatform).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxPlayer
            // 
            pictureBoxPlayer.Image = (Image)resources.GetObject("pictureBoxPlayer.Image");
            pictureBoxPlayer.Location = new Point(337, 401);
            pictureBoxPlayer.Name = "pictureBoxPlayer";
            pictureBoxPlayer.Size = new Size(118, 241);
            pictureBoxPlayer.TabIndex = 0;
            pictureBoxPlayer.TabStop = false;
            // 
            // pictureBoxPlatform
            // 
            pictureBoxPlatform.Image = (Image)resources.GetObject("pictureBoxPlatform.Image");
            pictureBoxPlatform.Location = new Point(331, 178);
            pictureBoxPlatform.Name = "pictureBoxPlatform";
            pictureBoxPlatform.Size = new Size(150, 75);
            pictureBoxPlatform.TabIndex = 1;
            pictureBoxPlatform.TabStop = false;
            pictureBoxPlatform.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(831, 640);
            Controls.Add(pictureBoxPlatform);
            Controls.Add(pictureBoxPlayer);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlatform).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxPlayer;
        private PictureBox pictureBoxPlatform;
    }
}

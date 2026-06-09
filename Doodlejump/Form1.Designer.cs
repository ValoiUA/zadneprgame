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
            buttonPlay = new Button();
            buttonSettings = new Button();
            buttonExit = new Button();
            panelMenu = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlatform).BeginInit();
            panelMenu.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxPlayer
            // 
            pictureBoxPlayer.Image = (Image)resources.GetObject("pictureBoxPlayer.Image");
            pictureBoxPlayer.Location = new Point(337, 402);
            pictureBoxPlayer.Name = "pictureBoxPlayer";
            pictureBoxPlayer.Size = new Size(119, 242);
            pictureBoxPlayer.TabIndex = 0;
            pictureBoxPlayer.TabStop = false;
            // 
            // pictureBoxPlatform
            // 
            pictureBoxPlatform.Image = (Image)resources.GetObject("pictureBoxPlatform.Image");
            pictureBoxPlatform.Location = new Point(666, 18);
            pictureBoxPlatform.Name = "pictureBoxPlatform";
            pictureBoxPlatform.Size = new Size(150, 75);
            pictureBoxPlatform.TabIndex = 1;
            pictureBoxPlatform.TabStop = false;
            pictureBoxPlatform.Visible = false;
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(64, 33);
            buttonPlay.Margin = new Padding(4, 5, 4, 5);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(151, 68);
            buttonPlay.TabIndex = 2;
            buttonPlay.Text = "Play";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += buttonPlay_Click;
            // 
            // buttonSettings
            // 
            buttonSettings.Location = new Point(64, 112);
            buttonSettings.Margin = new Padding(4, 5, 4, 5);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(151, 68);
            buttonSettings.TabIndex = 3;
            buttonSettings.Text = "Setting";
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += buttonSettings_Click;
            // 
            // buttonExit
            // 
            buttonExit.Location = new Point(64, 190);
            buttonExit.Margin = new Padding(4, 5, 4, 5);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(151, 68);
            buttonExit.TabIndex = 4;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += buttonExit_Click;
            // 
            // panelMenu
            // 
            panelMenu.Controls.Add(buttonPlay);
            panelMenu.Controls.Add(buttonExit);
            panelMenu.Controls.Add(buttonSettings);
            panelMenu.Location = new Point(251, 20);
            panelMenu.Margin = new Padding(4, 5, 4, 5);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(286, 335);
            panelMenu.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(831, 640);
            Controls.Add(panelMenu);
            Controls.Add(pictureBoxPlatform);
            Controls.Add(pictureBoxPlayer);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlatform).EndInit();
            panelMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxPlayer;
        private PictureBox pictureBoxPlatform;
        private Button buttonPlay;
        private Button buttonSettings;
        private Button buttonExit;
        private Panel panelMenu;
    }
}

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
            buttonAccept = new Button();
            textBoxNick = new TextBox();
            labelNick = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlatform).BeginInit();
            panelMenu.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxPlayer
            // 
            pictureBoxPlayer.Image = (Image)resources.GetObject("pictureBoxPlayer.Image");
            pictureBoxPlayer.Location = new Point(236, 241);
            pictureBoxPlayer.Margin = new Padding(2);
            pictureBoxPlayer.Name = "pictureBoxPlayer";
            pictureBoxPlayer.Size = new Size(83, 145);
            pictureBoxPlayer.TabIndex = 0;
            pictureBoxPlayer.TabStop = false;
            // 
            // pictureBoxPlatform
            // 
            pictureBoxPlatform.Image = (Image)resources.GetObject("pictureBoxPlatform.Image");
            pictureBoxPlatform.Location = new Point(466, 11);
            pictureBoxPlatform.Margin = new Padding(2);
            pictureBoxPlatform.Name = "pictureBoxPlatform";
            pictureBoxPlatform.Size = new Size(105, 45);
            pictureBoxPlatform.TabIndex = 1;
            pictureBoxPlatform.TabStop = false;
            pictureBoxPlatform.Visible = false;
            // 
            // buttonPlay
            // 
            buttonPlay.Location = new Point(45, 20);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(106, 41);
            buttonPlay.TabIndex = 2;
            buttonPlay.Text = "Play";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += buttonPlay_Click;
            // 
            // buttonSettings
            // 
            buttonSettings.Location = new Point(45, 67);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(106, 41);
            buttonSettings.TabIndex = 3;
            buttonSettings.Text = "Setting";
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += buttonSettings_Click;
            // 
            // buttonExit
            // 
            buttonExit.Location = new Point(45, 114);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(106, 41);
            buttonExit.TabIndex = 4;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += buttonExit_Click;
            // 
            // panelMenu
            // 
            panelMenu.Controls.Add(buttonAccept);
            panelMenu.Controls.Add(textBoxNick);
            panelMenu.Controls.Add(labelNick);
            panelMenu.Controls.Add(buttonPlay);
            panelMenu.Controls.Add(buttonExit);
            panelMenu.Controls.Add(buttonSettings);
            panelMenu.Location = new Point(176, 12);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(199, 339);
            panelMenu.TabIndex = 5;
            // 
            // buttonAccept
            // 
            buttonAccept.Location = new Point(70, 251);
            buttonAccept.Name = "buttonAccept";
            buttonAccept.Size = new Size(75, 23);
            buttonAccept.TabIndex = 7;
            buttonAccept.Text = "Accept";
            buttonAccept.UseVisualStyleBackColor = true;
            buttonAccept.Click += buttonAccept_Click;
            // 
            // textBoxNick
            // 
            textBoxNick.Location = new Point(51, 201);
            textBoxNick.Name = "textBoxNick";
            textBoxNick.Size = new Size(100, 23);
            textBoxNick.TabIndex = 6;
            // 
            // labelNick
            // 
            labelNick.AutoSize = true;
            labelNick.Location = new Point(70, 171);
            labelNick.Name = "labelNick";
            labelNick.Size = new Size(61, 15);
            labelNick.TabIndex = 5;
            labelNick.Text = "Nickname";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 384);
            Controls.Add(panelMenu);
            Controls.Add(pictureBoxPlatform);
            Controls.Add(pictureBoxPlayer);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlatform).EndInit();
            panelMenu.ResumeLayout(false);
            panelMenu.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBoxPlayer;
        private PictureBox pictureBoxPlatform;
        private Button buttonPlay;
        private Button buttonSettings;
        private Button buttonExit;
        private Panel panelMenu;
        private Button buttonAccept;
        private TextBox textBoxNick;
        private Label labelNick;
    }
}

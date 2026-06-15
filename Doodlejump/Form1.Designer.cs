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
            listBoxHistory = new ListBox();
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
            panelMenu.Controls.Add(listBoxHistory);
            panelMenu.Controls.Add(buttonAccept);
            panelMenu.Controls.Add(textBoxNick);
            panelMenu.Controls.Add(labelNick);
            panelMenu.Controls.Add(buttonPlay);
            panelMenu.Controls.Add(buttonExit);
            panelMenu.Controls.Add(buttonSettings);
            panelMenu.Location = new Point(251, 20);
            panelMenu.Margin = new Padding(4, 5, 4, 5);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(303, 624);
            panelMenu.TabIndex = 5;
            // 
            // listBoxHistory
            // 
            listBoxHistory.FormattingEnabled = true;
            listBoxHistory.ItemHeight = 25;
            listBoxHistory.Location = new Point(10, 448);
            listBoxHistory.Name = "listBoxHistory";
            listBoxHistory.Size = new Size(293, 179);
            listBoxHistory.TabIndex = 8;
            listBoxHistory.SelectedIndexChanged += listBoxHistory_SelectedIndexChanged;
            // 
            // buttonAccept
            // 
            buttonAccept.Location = new Point(98, 382);
            buttonAccept.Margin = new Padding(4, 5, 4, 5);
            buttonAccept.Name = "buttonAccept";
            buttonAccept.Size = new Size(107, 38);
            buttonAccept.TabIndex = 7;
            buttonAccept.Text = "Accept";
            buttonAccept.UseVisualStyleBackColor = true;
            buttonAccept.Click += buttonAccept_Click;
            // 
            // textBoxNick
            // 
            textBoxNick.Location = new Point(73, 335);
            textBoxNick.Margin = new Padding(4, 5, 4, 5);
            textBoxNick.Name = "textBoxNick";
            textBoxNick.Size = new Size(141, 31);
            textBoxNick.TabIndex = 6;
            // 
            // labelNick
            // 
            labelNick.AutoSize = true;
            labelNick.Location = new Point(100, 285);
            labelNick.Margin = new Padding(4, 0, 4, 0);
            labelNick.Name = "labelNick";
            labelNick.Size = new Size(90, 25);
            labelNick.TabIndex = 5;
            labelNick.Text = "Nickname";
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
        private ListBox listBoxHistory;
    }
}

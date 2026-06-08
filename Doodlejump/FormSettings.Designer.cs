namespace Doodlejump
{
    partial class FormSettings
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
            buttonToggleSound = new Button();
            trackBarVolume = new TrackBar();
            buttonBackToMenu = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).BeginInit();
            SuspendLayout();
            // 
            // buttonToggleSound
            // 
            buttonToggleSound.Location = new Point(24, 66);
            buttonToggleSound.Name = "buttonToggleSound";
            buttonToggleSound.Size = new Size(33, 28);
            buttonToggleSound.TabIndex = 0;
            buttonToggleSound.Text = "S";
            buttonToggleSound.UseVisualStyleBackColor = true;
            buttonToggleSound.Click += buttonToggleSound_Click;
            // 
            // trackBarVolume
            // 
            trackBarVolume.Location = new Point(63, 49);
            trackBarVolume.Maximum = 100;
            trackBarVolume.Name = "trackBarVolume";
            trackBarVolume.Size = new Size(104, 45);
            trackBarVolume.TabIndex = 1;
            trackBarVolume.Scroll += trackBarVolume_Scroll;
            // 
            // buttonBackToMenu
            // 
            buttonBackToMenu.Location = new Point(89, 192);
            buttonBackToMenu.Name = "buttonBackToMenu";
            buttonBackToMenu.Size = new Size(68, 31);
            buttonBackToMenu.TabIndex = 2;
            buttonBackToMenu.Text = "Save";
            buttonBackToMenu.UseVisualStyleBackColor = true;
            buttonBackToMenu.Click += buttonBackToMenu_Click;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 361);
            Controls.Add(buttonBackToMenu);
            Controls.Add(trackBarVolume);
            Controls.Add(buttonToggleSound);
            Name = "FormSettings";
            Text = "FormSettings";
            Load += FormSettings_Load;
            ((System.ComponentModel.ISupportInitialize)trackBarVolume).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonToggleSound;
        private TrackBar trackBarVolume;
        private Button buttonBackToMenu;
    }
}
namespace Doodlejump
{
    partial class FormLeaderBoard
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
            buttonLeave = new Button();
            listBoxLeaderBoard = new ListBox();
            SuspendLayout();
            // 
            // buttonLeave
            // 
            buttonLeave.Location = new Point(110, 338);
            buttonLeave.Name = "buttonLeave";
            buttonLeave.Size = new Size(95, 23);
            buttonLeave.TabIndex = 0;
            buttonLeave.Text = "Leave";
            buttonLeave.UseVisualStyleBackColor = true;
            buttonLeave.Click += buttonLeave_Click;
            // 
            // listBoxLeaderBoard
            // 
            listBoxLeaderBoard.FormattingEnabled = true;
            listBoxLeaderBoard.ItemHeight = 15;
            listBoxLeaderBoard.Location = new Point(33, 12);
            listBoxLeaderBoard.Name = "listBoxLeaderBoard";
            listBoxLeaderBoard.Size = new Size(308, 319);
            listBoxLeaderBoard.TabIndex = 1;
            // 
            // FormLeaderBoard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(411, 411);
            Controls.Add(listBoxLeaderBoard);
            Controls.Add(buttonLeave);
            Name = "FormLeaderBoard";
            Text = "FormLeaderBoard";
            Load += FormLeaderBoard_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button buttonLeave;
        private ListBox listBoxLeaderBoard;
    }
}
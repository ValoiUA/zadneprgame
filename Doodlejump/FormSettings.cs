using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doodlejump
{
    public partial class FormSettings : Form
    {
        public static bool IsSoundOn = true;
        public static int Volume = 50;


        public FormSettings()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            trackBarVolume.Value = Volume;
            trackBarVolume.Enabled = IsSoundOn;
            buttonToggleSound.Text = IsSoundOn ? "Звук: Увімкнено" : "Звук: Вимкнено";
        }

        private void buttonToggleSound_Click(object sender, EventArgs e)
        {
            IsSoundOn = !IsSoundOn;
            trackBarVolume.Enabled = IsSoundOn;
            buttonToggleSound.Text = IsSoundOn ? "Звук: Увімкнено" : "Звук: Вимкнено";
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            Volume = trackBarVolume.Value;
        }

        private void buttonBackToMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

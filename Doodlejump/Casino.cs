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
    public partial class Casino : Form
    {
        Random rnd = new Random();
        Image[] images;
        int spins;
        int currentFrame = 0;
        public Casino()
        {
            InitializeComponent();
        }
        private Image GetImageFromResource(object resource)
        {
            if (resource == null) return null;
            if (resource is Image img)
                return img;
            if (resource is byte[] bytes)
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    return Image.FromStream(ms);
            }

            return null;
        }

        private void Casino_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox9.SizeMode = PictureBoxSizeMode.Zoom;
            images = new Image[]
{
    Properties.Resources.malahova,
    Properties.Resources.mikoluk,
    Properties.Resources.fedosuk,
    Properties.Resources.gumenna
};
            pictureBox1.Image = images[0];
            pictureBox2.Image = images[0];
            pictureBox3.Image = images[0];
            pictureBox4.Image = images[0];
            pictureBox5.Image = images[0];
            pictureBox6.Image = images[0];
            pictureBox7.Image = images[0];
            pictureBox8.Image = images[0];
            pictureBox9.Image = images[0];
            spins = rnd.Next(1, 10);
            lablelCount.Text = $"Spins: {spins}";
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

        }
        private async void timerAnimation_Tick(object sender, EventArgs e)
        {
            int[] lastIdx = new int[9];

            for (int i = 0; i < 7; i++)
            {
                int[] newIdx = new int[9];

                for (int p = 0; p < 9; p++)
                {
                    int nextImg;
                    do
                    {
                        nextImg = rnd.Next(0, 4);
                    } while (nextImg == lastIdx[p]);

                    newIdx[p] = nextImg;
                    lastIdx[p] = nextImg; 
                }
                pictureBox1.Image = images[newIdx[0]];
                pictureBox2.Image = images[newIdx[1]];
                pictureBox3.Image = images[newIdx[2]];
                pictureBox4.Image = images[newIdx[3]];
                pictureBox5.Image = images[newIdx[4]];
                pictureBox6.Image = images[newIdx[5]];
                pictureBox7.Image = images[newIdx[6]];
                pictureBox8.Image = images[newIdx[7]];
                pictureBox9.Image = images[newIdx[8]];
                this.Refresh();
                await Task.Delay(150);
            
        }

            if (spins == 0)
            {
                MessageBox.Show("No more spins left!");
                this.Close();
                return;
            }
        
    
        }
        private void checkCombo()
        {
            Image first = pictureBox1.Image;
            Image second = pictureBox2.Image;
            Image third = pictureBox3.Image;
            Image fourth = pictureBox4.Image;
            Image fifth = pictureBox5.Image;
            Image sixth = pictureBox6.Image;
            Image seventh = pictureBox7.Image;
            Image eight = pictureBox8.Image;
            Image nineth = pictureBox9.Image;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (spins > 0) timerAnimation_Tick(sender, e);
            else
            {
                MessageBox.Show("No more spins left!");
                this.Close();
                return;
            }
            spins--;
            lablelCount.Text = $"Spins: {spins}";
        }

        private void Casino_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}

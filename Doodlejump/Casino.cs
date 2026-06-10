using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        Image[] toImages;
        int spins;
        int currentFrame = 0;
        public int score = 0;
        bool isAnimating = false;
        bool isMine = false;
        public int CurrentScore { get; private set; }
        public Casino(int scores)
        {
            InitializeComponent();
            score = scores;
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

            Image gum = GetImageFromResource(Properties.Resources.gum);
            Image fed = GetImageFromResource(Properties.Resources.fedos);
            Image mik = GetImageFromResource(Properties.Resources.mik);
            Image mal = GetImageFromResource(Properties.Resources.mal);
            toImages = new Image[]
            {
                mal,
                mik, 
                fed, 
                gum
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
            labelScore.Text = $"Score {score}";
        }



        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

        private async void timerAnimation_Tick(object sender, EventArgs e)
        {
            isAnimating = true; 
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

            int bet = 10;
            if (int.TryParse(textBox1.Text, out int customBet))
            {
                bet = customBet;
            }
            checkCombo(bet);
        }

        private void checkCombo(int bet)
        {
            Image i1 = pictureBox1.Image;
            Image i2 = pictureBox2.Image;
            Image i3 = pictureBox3.Image;
            Image i4 = pictureBox4.Image;
            Image i5 = pictureBox5.Image;
            Image i6 = pictureBox6.Image;
            Image i7 = pictureBox7.Image;
            Image i8 = pictureBox8.Image;
            Image i9 = pictureBox9.Image;

            bool won = false;

            // 1. ПЕРЕВІРКА НА ФУЛ ПОЛЕ (всі 9 однакові)
            if (i1 != null && i1 == i2 && i2 == i3 && i3 == i4 && i4 == i5 && i5 == i6 && i6 == i7 && i7 == i8 && i8 == i9)
            {
                score += bet * 50;
                if (i1 == images[0]){ AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[0]);
                    AnimateWinRow(pictureBox2, pictureBox6, pictureBox8, toImages[0]);
                    AnimateWinRow(pictureBox3, pictureBox4, pictureBox7, toImages[0]);
                }
                if (i1 == images[1])
                {
                    AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[1]);
                    AnimateWinRow(pictureBox2, pictureBox6, pictureBox8, toImages[1]);
                    AnimateWinRow(pictureBox3, pictureBox4, pictureBox7, toImages[1]);
                }
                if (i1 == images[2]){
                    AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[2]);
                    AnimateWinRow(pictureBox2, pictureBox6, pictureBox8, toImages[2]);
                    AnimateWinRow(pictureBox3, pictureBox4, pictureBox7, toImages[2]);
                }
                if (i1 == images[3]) {
                    AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[3]);
                    AnimateWinRow(pictureBox2, pictureBox6, pictureBox8, toImages[3]);
                    AnimateWinRow(pictureBox3, pictureBox4, pictureBox7, toImages[3]);
                }
                won = true;
            }
            // 2. ДІАГОНАЛІ
            if (i1 != null && i1 == i5 && i5 == i9)
            {
                score += bet * 5;
                if(i1 == images[0]) AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[0]);
                if(i1 == images[1]) AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[1]);
                if(i1 == images[2]) AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[2]);
                if(i1 == images[3]) AnimateWinRow(pictureBox1, pictureBox5, pictureBox9, toImages[3]);
                won = true;
            }
            if (i3 != null && i3 == i5 && i5 == i7)
            {
                score += bet * 5;
                if (i3 == images[0]) AnimateWinRow(pictureBox3, pictureBox5, pictureBox7, toImages[0]);
                if (i3 == images[1]) AnimateWinRow(pictureBox3, pictureBox5, pictureBox7, toImages[1]);
                if (i3 == images[2]) AnimateWinRow(pictureBox3, pictureBox5, pictureBox7, toImages[2]);
                if (i3 == images[3]) AnimateWinRow(pictureBox3, pictureBox5, pictureBox7, toImages[3]);
                won = true;
            }

            // 3. ГОРИЗОНТАЛІ
            if (i1 != null && i1 == i2 && i2 == i3)
            {
                score += bet * 3;
                if (i1 == images[0]) AnimateWinRow(pictureBox1, pictureBox2, pictureBox3, toImages[0]);
                if (i1 == images[1]) AnimateWinRow(pictureBox1, pictureBox2, pictureBox3, toImages[1]);
                if (i1 == images[2]) AnimateWinRow(pictureBox1, pictureBox2, pictureBox3, toImages[2]);
                if (i1 == images[3]) AnimateWinRow(pictureBox1, pictureBox2, pictureBox3, toImages[3]);
                won = true;
            }
            if (i4 != null && i4 == i5 && i5 == i6)
            {
                score += bet * 3;
                if (i4 == images[0]) AnimateWinRow(pictureBox4, pictureBox5, pictureBox6, toImages[0]);
                if (i4 == images[1]) AnimateWinRow(pictureBox4, pictureBox5, pictureBox6, toImages[1]);
                if (i4 == images[2]) AnimateWinRow(pictureBox4, pictureBox5, pictureBox6, toImages[2]);
                if (i4 == images[3]) AnimateWinRow(pictureBox4, pictureBox5, pictureBox6, toImages[3]);
                won = true;
            }
            if (i7 != null && i7 == i8 && i8 == i9)
            {
                score += bet * 3;
                if (i7 == images[0]) AnimateWinRow(pictureBox7, pictureBox8, pictureBox9, toImages[0]);
                if (i7 == images[1]) AnimateWinRow(pictureBox7, pictureBox8, pictureBox9, toImages[1]);
                if (i7 == images[2]) AnimateWinRow(pictureBox7, pictureBox8, pictureBox9, toImages[2]);
                if (i7 == images[3]) AnimateWinRow(pictureBox7, pictureBox8, pictureBox9, toImages[3]);
                won = true;
            }

            if (won)
            {
                labelScore.Text = $"Score {score}";
            }
            else
            {
                isAnimating = false;
                CheckGameOver();
            }
            if (score == 0)
            {
                isMine = true;
                MessageBox.Show("Ludic");
                this.Close();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) {
                MessageBox.Show("Input bet");
                return;
            }
            if (int.TryParse(textBox1.Text, out int bet))
            {
                if (bet <= 0)
                {
                    MessageBox.Show("Bet must be positive!");
                    return;
                }
                if (bet > score)
                {
                    MessageBox.Show("Not enough score for this bet!");
                    return;
                }

                score -= bet;
                labelScore.Text = $"Score {score}";
            }
            else
            {
                MessageBox.Show("Invalid bet amount!");
                return;
            }
            if (isAnimating) return;

            if (spins > 0)
            {
                spins--;
                lablelCount.Text = $"Spins: {spins}";
                timerAnimation_Tick(sender, e);
            }
            else
            {
                CheckGameOver();
                MessageBox.Show("No more spins left!");
            }
        }

        private void Casino_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isMine) return;
            DialogResult result = MessageBox.Show("Are you sure you want to leave the casino?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes) {
                CurrentScore = this.score; ;
            }
            else
            {
                e.Cancel = true;
            }
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void AnimateWinRow(PictureBox p1, PictureBox p2, PictureBox p3, Image newImage)
        {
            System.Windows.Forms.Timer animTimer = new System.Windows.Forms.Timer();
            animTimer.Interval = 20;

            int step = 0;
            int maxSteps = 12;
            int growth = 3;

            Size oSize1 = p1.Size; Point oLoc1 = p1.Location;
            Size oSize2 = p2.Size; Point oLoc2 = p2.Location;
            Size oSize3 = p3.Size; Point oLoc3 = p3.Location;

            animTimer.Tick += (sender, e) =>
            {
                step++;

                if (step <= maxSteps)
                {
                    ExpandPictureBox(p1, growth);
                    ExpandPictureBox(p2, growth);
                    ExpandPictureBox(p3, growth);
                }
                else if (step == maxSteps + 1)
                {
                    p1.Image = newImage;
                    p2.Image = newImage;
                    p3.Image = newImage;
                }
                else if (step <= maxSteps * 2)
                {
                    ShrinkPictureBox(p1, growth);
                    ShrinkPictureBox(p2, growth);
                    ShrinkPictureBox(p3, growth);
                }
                else
                {
                    p1.Size = oSize1; p1.Location = oLoc1;
                    p2.Size = oSize2; p2.Location = oLoc2;
                    p3.Size = oSize3; p3.Location = oLoc3;

                    animTimer.Stop();
                    animTimer.Dispose();

                    isAnimating = false;
                    CheckGameOver();
                }
            };

            animTimer.Start();
        }

        private void ExpandPictureBox(PictureBox pb, int amount)
        {
            pb.Width += amount * 2;
            pb.Height += amount * 2;
            pb.Left -= amount;
            pb.Top -= amount;
            pb.BringToFront();
        }

        private void ShrinkPictureBox(PictureBox pb, int amount)
        {
            pb.Width -= amount * 2;
            pb.Height -= amount * 2;
            pb.Left += amount;
            pb.Top += amount;
        }

        private void CheckGameOver()
        {
            if (spins <= 0 && !isAnimating)
            {
                isMine = true;
                CurrentScore = this.score;  
                MessageBox.Show("No more spins left!");
                this.Close();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NAudio.Wave;


namespace Doodlejump
{
    public partial class Form1 : Form
    {
        // Системна функція для керування аудіо-рушієм Windows
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnString, int returnLength, IntPtr callback);

        public class WorldPlatform
        {
            public PictureBox View { get; set; }
            public int WorldX { get; set; }
            public int WorldY { get; set; }
        }

        List<WorldPlatform> platforms = new List<WorldPlatform>();
        Random rnd = new Random();
        int playerWorldX;
        int playerWorldY;
        int jumpSpeed = 17;
        int gravity = 0;
        int playerSpeed = 8;
        int cameraY = 0;
        bool goLeft = false;
        private SoundPlayer player;
        bool goRight = false;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        // Шлях до файлу фонової музики
        string musicPath;

        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();

            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimerEvent;

            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.KeyPreview = true;

            // Запускаємо фонову заставку
            PrepareAndPlayBackgroundMusic();
        }

        private void PrepareAndPlayBackgroundMusic()
        {
            try
            {
                musicPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "doodle_bg_music.wav");

                using (Stream sourceStream = Properties.Resources.sound)
                using (FileStream fileStream = new FileStream(
                    musicPath,
                    FileMode.Create,
                    FileAccess.Write))
                {
                    sourceStream.CopyTo(fileStream);
                }

                audioFile = new AudioFileReader(musicPath);

                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);

                audioFile.Volume = FormSettings.Volume / 100f;

                if (FormSettings.IsSoundOn)
                    outputDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static extern int GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, int cchBuffer);

        private void UpdateVolume()
        {
            if (audioFile == null)
                return;

            if (!FormSettings.IsSoundOn)
            {
                audioFile.Volume = 0;
                return;
            }

            audioFile.Volume = FormSettings.Volume / 100f;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(450, 700);
            this.DoubleBuffered = true;
            pictureBoxPlayer.SizeMode = PictureBoxSizeMode.Zoom;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            gameTimer.Stop();

            foreach (var p in platforms)
            {
                this.Controls.Remove(p.View);
                p.View.Dispose();
            }
            platforms.Clear();

            if (pictureBoxPlayer != null) pictureBoxPlayer.Visible = false;

            this.Refresh();

            panelMenu.Visible = true;
            panelMenu.BringToFront();
        }

        private void GeneratePlatforms()
        {
            foreach (var p in platforms) this.Controls.Remove(p.View);
            platforms.Clear();

            WorldPlatform first = new WorldPlatform();
            first.View = new PictureBox { Size = new Size(80, 15), SizeMode = PictureBoxSizeMode.StretchImage };
            SetPlatformResource(first.View);

            first.WorldX = this.ClientSize.Width / 2 - 40;
            first.WorldY = 100;

            this.Controls.Add(first.View);
            platforms.Add(first);

            int nextY = 180;
            for (int i = 0; i < 12; i++)
            {
                WorldPlatform wp = new WorldPlatform();
                wp.View = new PictureBox { Size = new Size(80, 15), SizeMode = PictureBoxSizeMode.StretchImage };
                SetPlatformResource(wp.View);

                wp.WorldX = rnd.Next(10, this.ClientSize.Width - 90);
                wp.WorldY = nextY;

                nextY += rnd.Next(65, 80);

                this.Controls.Add(wp.View);
                platforms.Add(wp);
                wp.View.BringToFront();
            }
        }

        private void SetPlatformResource(PictureBox pb)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(Properties.Resources.coledge))
                {
                    pb.Image = Image.FromStream(ms);
                }
            }
            catch { pb.BackColor = Color.Green; }
        }

        private void SetPlayerResource(PictureBox pb, byte[] resourceFile)
        {
            try
            {
                int currentWidth = pb.Width;
                int currentHeight = pb.Height;

                using (MemoryStream ms = new MemoryStream(resourceFile))
                {
                    if (pb.Image != null) pb.Image.Dispose();
                    pb.Image = Image.FromStream(ms);
                }

                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Width = currentWidth;
                pb.Height = currentHeight;
            }
            catch { pb.BackColor = Color.Red; }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (panelMenu.Visible) return;

            // 1. Рух гравця у світі
            playerWorldY += gravity;
            gravity += 1;

            if (gravity < 0)
            {
                SetPlayerResource(pictureBoxPlayer, Properties.Resources.zadneprup);
            }
            else
            {
                SetPlayerResource(pictureBoxPlayer, Properties.Resources.zandepr);
            }

            if (gravity > 12) gravity = 12;

            // Керування по Х
            if (goLeft) playerWorldX -= playerSpeed;
            if (goRight) playerWorldX += playerSpeed;

            if (playerWorldX + pictureBoxPlayer.Width < 0) playerWorldX = this.ClientSize.Width;
            else if (playerWorldX > this.ClientSize.Width) playerWorldX = -pictureBoxPlayer.Width;

            // 2. Плавний рух камери
            int targetCameraY = playerWorldY - (this.ClientSize.Height / 2);
            if (targetCameraY < cameraY) cameraY += (targetCameraY - cameraY) / 4;

            // 3. Колізія
            if (gravity > 0)
            {
                foreach (var platform in platforms)
                {
                    if (playerWorldX + pictureBoxPlayer.Width >= platform.WorldX && playerWorldX <= platform.WorldX + platform.View.Width)
                    {
                        if (playerWorldY + pictureBoxPlayer.Height >= platform.WorldY &&
                            playerWorldY + pictureBoxPlayer.Height <= platform.WorldY + gravity + 2)
                        {
                            playerWorldY = platform.WorldY - pictureBoxPlayer.Height;
                            gravity = -jumpSpeed;
                            break;
                        }
                    }
                }
            }

            // 4. Екранне оновлення позицій
            pictureBoxPlayer.Left = playerWorldX;
            pictureBoxPlayer.Top = playerWorldY - cameraY;

            foreach (var platform in platforms)
            {
                platform.View.Left = platform.WorldX;
                platform.View.Top = platform.WorldY - cameraY;

                if (platform.View.Top > this.ClientSize.Height + 50)
                {
                    int highestWorldY = this.ClientSize.Height;
                    foreach (var p in platforms)
                    {
                        if (p.WorldY < highestWorldY) highestWorldY = p.WorldY;
                    }
                    platform.WorldX = rnd.Next(10, this.ClientSize.Width - platform.View.Width - 10);
                    platform.WorldY = highestWorldY - rnd.Next(65, 80);
                }
            }

            // 5. Програш
            if (pictureBoxPlayer.Top > this.ClientSize.Height)
            {
                gameTimer.Stop();
                this.Refresh();
                MessageBox.Show("Ти впав!");
                ShowMainMenu();
            }
        }

        private void ResetGame()
        {
            cameraY = 0;
            GeneratePlatforms();

            pictureBoxPlayer.Visible = true;
            pictureBoxPlayer.BringToFront();

            playerWorldX = this.ClientSize.Width / 2 - pictureBoxPlayer.Width / 2;
            playerWorldY = 30;

            gravity = 0;
            goLeft = false;
            goRight = false;

            gameTimer.Start();
        }

        // --- КНОПКИ ГОЛОВНОГО МЕНЮ ---
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            panelMenu.Visible = false;
            ResetGame();
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            FormSettings settingsForm = new FormSettings();
            settingsForm.ShowDialog();

            UpdateVolume();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Stop();
            }
            Application.Exit();
        }

        // --- КЕРУВАННЯ ---
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (panelMenu.Visible) return;

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) goLeft = true;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) goRight = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) goLeft = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) goRight = false;
        }
    }
}
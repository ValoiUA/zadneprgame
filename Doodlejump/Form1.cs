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
        [DllImport("winmm.dll")] private static extern long mciSendString(string command, StringBuilder returnString, int returnLength, IntPtr callback);

        public class WorldPlatform
        {
            public PictureBox View { get; set; }
            public int WorldX { get; set; }
            public int WorldY { get; set; }
            public bool IsBreakable { get; set; }
            public bool IsBroken { get; set; }
            public bool IsCasino { get; set; }
            public bool IsJumper { get; set; }
            public bool IsMovable { get; set; }
            public int Direction { get; set; } = 1; 
        }
        List<WorldPlatform> platforms = new List<WorldPlatform>();
        Random rnd = new Random();
        int playerWorldX;
        int playerWorldY;
        int jumpSpeed = 18;
        int gravity = 0;
        int playerSpeed = 8;
        int cameraY = 0;
        bool goLeft = false;
        private SoundPlayer player;
        bool goRight = false;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        bool isPaused = false;
        public int score = 0;
        Label labelScore;
        string musicPath;
        Label labelPause;
        int scoreOffset = 0;
        int platformSpeed = 6;

        // Змінна для відстеження поточного стану анімації (щоб не перезавантажувати картинку щокадру)
        private string currentAnimation = "";

        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        private void AudioFile_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (audioFile != null && outputDevice != null && FormSettings.IsSoundOn)
            {
                audioFile.Position = 0;
                outputDevice.Play();
            }
        }

        public Form1()
        {
            InitializeComponent();

            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimerEvent;

            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.KeyPreview = true;

            PrepareAndPlayBackgroundMusic();
        }

        private void TogglePause()
        {
            if (panelMenu.Visible) return;

            isPaused = !isPaused;

            if (isPaused)
            {
                gameTimer.Stop();

                labelPause.Location = new Point(
                    (this.ClientSize.Width - labelPause.Width) / 2,
                    (this.ClientSize.Height - labelPause.Height) / 2
                );
                labelPause.Visible = true;
                labelPause.BringToFront();
            }
            else
            {
                gameTimer.Start();
                labelPause.Visible = false;
            }
        }

        private void PrepareAndPlayBackgroundMusic()
        {
            try
            {
                musicPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "doodle_bg_music.wav");

                using (Stream sourceStream = Properties.Resources.sound)
                using (FileStream fileStream = new FileStream(musicPath, FileMode.Create, FileAccess.Write))
                {
                    sourceStream.CopyTo(fileStream);
                }

                audioFile = new AudioFileReader(musicPath);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.PlaybackStopped += AudioFile_PlaybackStopped;

                audioFile.Volume = FormSettings.Volume / 100f;

                if (FormSettings.IsSoundOn)
                    outputDevice.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateVolume()
        {
            if (audioFile == null || outputDevice == null) return;

            if (!FormSettings.IsSoundOn)
            {
                audioFile.Volume = 0;
                return;
            }

            audioFile.Volume = FormSettings.Volume / 100f;

            if (outputDevice.PlaybackState != PlaybackState.Playing)
            {
                outputDevice.Play();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(450, 700);
            this.DoubleBuffered = true;
            pictureBoxPlayer.SizeMode = PictureBoxSizeMode.Zoom;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            labelScore = new Label();
            labelScore.Text = "Очки: 0";
            labelScore.Font = new Font("Arial", 16, FontStyle.Bold);
            labelScore.ForeColor = Color.Black;
            labelScore.BackColor = Color.Transparent;
            labelScore.AutoSize = true;
            labelScore.Location = new Point(10, 10);
            this.Controls.Add(labelScore);

            labelPause = new Label();
            labelPause.Text = "ПАУЗА\n[Натисніть P для продовження]";
            labelPause.Font = new Font("Arial", 22, FontStyle.Bold);
            labelPause.ForeColor = Color.Red;
            labelPause.BackColor = Color.Transparent;
            labelPause.TextAlign = ContentAlignment.MiddleCenter;
            labelPause.AutoSize = true;
            labelPause.Visible = false;
            this.Controls.Add(labelPause);

            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            gameTimer.Stop();
            isPaused = false;
            if (labelScore != null) labelScore.Visible = false;
            if (labelPause != null) labelPause.Visible = false;
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

        private void SetPlatformResource(PictureBox pb, bool isBreakable, bool isCasino, bool isJumper, bool isMovable)
        {
            try
            {
                if (isCasino)
                {
                    if (pb.Image != null)
                    {
                        pb.Image.Dispose();
                        pb.Image = null;
                    }
                    pb.BackColor = Color.Magenta;
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(Properties.Resources.coledge))
                        {
                            if (pb.Image != null) pb.Image.Dispose();
                            pb.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        pb.Image = null;
                    }
                    return;
                }
                if (isMovable)
                {
                    if(pb.Image != null)
                    {
                        pb.Image.Dispose();
                        pb.Image = null;
                    }

                    pb.BackColor = Color.OrangeRed;
                    try
                    {
                        using (MemoryStream mns = new MemoryStream(Properties.Resources.coledge))
                        {
                            if (pb.Image != null) pb.Image.Dispose();
                            pb.Image = Image.FromStream(mns);
                        }
                    }
                    catch
                    {
                        pb.Image = null;
                    }
                }

                if (isJumper)
                {
                    if (pb.Image != null)
                    {
                        pb.Image.Dispose();
                        pb.Image = null;
                    }
                    pb.BackColor = Color.Blue;
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(Properties.Resources.coledge))
                        {
                            if (pb.Image != null) pb.Image.Dispose();
                            pb.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        pb.Image = null;
                    }
                    return;
                }

                if (isBreakable)
                {
                    pb.BackColor = Color.SaddleBrown;
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(Properties.Resources.breakcol))
                        {
                            if (pb.Image != null) pb.Image.Dispose();
                            pb.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        pb.Image = null;
                    }
                }
                else
                {
                    // Звичайні платформи - зелені
                    pb.BackColor = Color.ForestGreen;

                    try
                    {
                        using (MemoryStream ms = new MemoryStream(Properties.Resources.coledge))
                        {
                            if (pb.Image != null) pb.Image.Dispose();
                            pb.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        pb.Image = null;
                    }
                }
            }
            catch
            {
                // Запасний варіант якщо все зламалось
                pb.Image = null;
                if (isCasino)
                    pb.BackColor = Color.Magenta;
                else if (isBreakable)
                    pb.BackColor = Color.SaddleBrown;
                else if(isJumper)
                    pb.BackColor = Color.Blue;
                else
                    pb.BackColor = Color.ForestGreen;
            }
        }

        private void GeneratePlatforms()
        {
            foreach (var p in platforms) this.Controls.Remove(p.View);
            platforms.Clear();

            // Стартова платформа
            WorldPlatform first = new WorldPlatform();
            first.View = new PictureBox { Size = new Size(80, 15), SizeMode = PictureBoxSizeMode.StretchImage };
            first.IsCasino = false;
            first.IsBreakable = false;
            first.IsBroken = false;
            first.IsJumper = false;
            first.IsMovable = false;
            SetPlatformResource(first.View, false, false, false, false);
            first.WorldX = this.ClientSize.Width / 2 - 40;
            first.WorldY = 100;

            this.Controls.Add(first.View);
            platforms.Add(first);
            int nextY = 180;

            for (int i = 0; i < 12; i++)
            {
                WorldPlatform wp = new WorldPlatform();
                wp.View = new PictureBox { Size = new Size(80, 15), SizeMode = PictureBoxSizeMode.StretchImage };

                // 5% шанс на Казино
                wp.IsCasino = rnd.Next(0, 100) < 5;
                wp.IsJumper = rnd.Next(0, 100) < 10 && !wp.IsCasino;
                wp.IsMovable = rnd.Next(0, 100) < 20 && !wp.IsCasino && !wp.IsJumper;

                if (wp.IsCasino)
                {
                    wp.IsBreakable = false;
                    wp.IsBroken = false;
                }
                else if (wp.IsJumper)
                {
                    wp.IsBreakable = false;
                    wp.IsBroken = false;
                }
                else if(wp.IsMovable)
                {
                    wp.Direction = rnd.Next(0, 2) == 0 ? -1 : 1;
                }
                else
                {
                    // ВИПРАВЛЕНО: 15% шанс на ламку платформу
                    wp.IsBreakable = rnd.Next(0, 100) < 15;
                    wp.IsBroken = false;
                }

                // Встановлюємо візуал
                SetPlatformResource(wp.View, wp.IsBreakable, wp.IsCasino, wp.IsJumper, wp.IsMovable);

                wp.WorldX = rnd.Next(10, this.ClientSize.Width - 90);
                wp.WorldY = nextY;
                nextY += rnd.Next(65, 80);

                this.Controls.Add(wp.View);
                platforms.Add(wp);
                wp.View.BringToFront();
            }
        }

        private void SetPlayerResource(PictureBox pb, byte[] resourceFile, string animName)
        {
            if (currentAnimation == animName) return;
            currentAnimation = animName;

            try
            {
                using (MemoryStream ms = new MemoryStream(resourceFile))
                {
                    if (pb.Image != null) pb.Image.Dispose();
                    pb.Image = Image.FromStream(ms);
                }
            }
            catch { pb.BackColor = Color.Red; }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (panelMenu.Visible || isPaused) return;

            // 1. Рух гравця у світі
            playerWorldY += gravity;
            gravity += 1;

            if (gravity < 0)
            {
                SetPlayerResource(pictureBoxPlayer, Properties.Resources.zadneprup, "up");
            }
            else
            {
                SetPlayerResource(pictureBoxPlayer, Properties.Resources.zandepr, "down");
            }

            if (gravity > 12) gravity = 12;

            if (goLeft) playerWorldX -= playerSpeed;
            if (goRight) playerWorldX += playerSpeed;

            if (playerWorldX + pictureBoxPlayer.Width < 0) playerWorldX = this.ClientSize.Width;
            else if (playerWorldX > this.ClientSize.Width) playerWorldX = -pictureBoxPlayer.Width;

            // 2. Плавний рух камери
            int targetCameraY = playerWorldY - (this.ClientSize.Height / 2);
            if (targetCameraY < cameraY) cameraY += (targetCameraY - cameraY) / 4;

            // РОЗРАХУНОК ОЧКІВ: Висота + Бонуси з казино 🎯
            int currentProgress = (-cameraY / 10) + scoreOffset;
            if (currentProgress > score)
            {
                score = currentProgress;
                labelScore.Text = "Очки: " + score;
            }

            // 3. Колізія
            if (gravity > 0)
            {
                Rectangle playerRect = new Rectangle(
                    playerWorldX,
                    playerWorldY,
                    pictureBoxPlayer.Width,
                    pictureBoxPlayer.Height);

                foreach (var platform in platforms)
                {
                    if (platform.IsBroken)
                        continue;

                    Rectangle platformRect = new Rectangle(
                        platform.WorldX,
                        platform.WorldY,
                        platform.View.Width,
                        platform.View.Height);

                    if (playerRect.IntersectsWith(platformRect))
                    {
                        if (playerWorldY + pictureBoxPlayer.Height - gravity <= platform.WorldY)
                        {
                            if (platform.IsBreakable)
                            {
                                platform.IsBroken = true;
                                platform.View.Visible = false;
                                gravity = 5;
                            }
                            else
                            {
                                playerWorldY = platform.WorldY - pictureBoxPlayer.Height;

                                if (platform.IsCasino)
                                {
                                    goLeft = false;
                                    goRight = false;
                                    gravity = 0;

                                    gameTimer.Stop();

                                    // Передаємо поточні очки в казино
                                    Casino cs = new Casino(score);
                                    cs.ShowDialog();
                                    // ОБЧИСЛЕННЯ БОНУСУ: Дізнаємося, скільки саме гравець виграв/програв у казино
                                    int casinoWin = cs.CurrentScore - score;
                                    scoreOffset += casinoWin; // Додаємо цей результат до загального зміщення очок

                                    score = cs.CurrentScore; // Оновлюємо поточні очки
                                    labelScore.Text = "Очки: " + score; // Виводимо гарний текст

                                    gravity = -26; // Потужний стрибок
                                    gameTimer.Start();
                                }
                                else if (platform.IsJumper) gravity = -jumpSpeed * 4;
                                else
                                {
                                    gravity = -jumpSpeed;
                                }

                                break;
                            }
                        }
                    }
                }
            }

            // 4. Екранне оновлення позицій
            pictureBoxPlayer.Left = playerWorldX;
            pictureBoxPlayer.Top = playerWorldY - cameraY;

            foreach (var platform in platforms)
            {
                if (platform.IsMovable && !platform.IsBroken)
                {
                    platform.WorldX += platformSpeed * platform.Direction;

                    // Відскок від лівої та правої стінок форми
                    if (platform.WorldX <= 0)
                    {
                        platform.WorldX = 0;
                        platform.Direction = 1;
                    }
                    else if (platform.WorldX + platform.View.Width >= this.ClientSize.Width)
                    {
                        platform.WorldX = this.ClientSize.Width - platform.View.Width;
                        platform.Direction = -1;
                    }
                }
                platform.View.Left = platform.WorldX;
                platform.View.Top = platform.WorldY - cameraY;

                if (platform.View.Top > this.ClientSize.Height + 50)
                {
                    int highestWorldY = this.ClientSize.Height;
                    WorldPlatform highestPlatform = null;

                    foreach (var p in platforms)
                    {
                        if (p.WorldY < highestWorldY)
                        {
                            highestWorldY = p.WorldY;
                            highestPlatform = p;
                        }
                    }

                    platform.WorldX = rnd.Next(10, this.ClientSize.Width - platform.View.Width - 10);
                    platform.WorldY = highestWorldY - rnd.Next(65, 80);

                    // Повне скидання старих типів перед новим роллом
                    platform.IsCasino = false;
                    platform.IsJumper = false;
                    platform.IsMovable = false;
                    platform.IsBreakable = false;
                    int roll = rnd.Next(0, 100);
                    if (roll < 5)
                    {
                        platform.IsCasino = true;
                    }
                    else if (roll < 12)
                    {
                        platform.IsJumper = true;
                    }
                    else if (roll < 30) // 18% шанс, що платформа буде рухомою
                    {
                        platform.IsMovable = true;
                        platform.Direction = rnd.Next(0, 2) == 0 ? 1 : -1;
                    }
                    else
                    {
                        if (highestPlatform != null && highestPlatform.IsBreakable) platform.IsBreakable = false;
                        else platform.IsBreakable = rnd.Next(0, 100) < 15;
                    }

                    platform.IsBroken = false;
                    platform.View.Visible = true;

                    SetPlatformResource(platform.View, platform.IsBreakable, platform.IsCasino, platform.IsJumper, platform.IsMovable);
                    platform.View.BringToFront();
                }
            }

            if (labelScore != null) labelScore.BringToFront();

            // 5. Програш
            if (pictureBoxPlayer.Top > this.ClientSize.Height)
            {
                gameTimer.Stop();
                this.Refresh();
                MessageBox.Show($"Ти впав!\nТвій результат: {score} очків.");
                ShowMainMenu();
            }
        }

        private void ResetGame()
        {
            score = 0;
            scoreOffset = 0;
            isPaused = false;
            currentAnimation = "";
            if (labelScore != null)
            {
                labelScore.Text = "Очки: 0";
                labelScore.Visible = true;
            }
            if (labelPause != null) labelPause.Visible = false;

            cameraY = 0;
            GeneratePlatforms();

            pictureBoxPlayer.Visible = true;
            pictureBoxPlayer.SendToBack();
            if (labelScore != null) labelScore.BringToFront();

            playerWorldX = this.ClientSize.Width / 2 - pictureBoxPlayer.Width / 2;
            playerWorldY = 30;

            gravity = 0;
            goLeft = false;
            goRight = false;

            gameTimer.Start();
        }

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
            if (player != null) player.Stop();
            Application.Exit();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P)
            {
                TogglePause();
                return;
            }

            if (panelMenu.Visible || isPaused) return;

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) goLeft = true;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) goRight = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (isPaused) return;

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A) goLeft = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) goRight = false;
        }
    }
}
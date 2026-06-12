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
            public int WorldX { get; set; }
            public int WorldY { get; set; }
            public int Width { get; set; } = 80;
            public int Height { get; set; } = 15;
            public bool IsBreakable { get; set; }
            public bool IsBroken { get; set; }
            public bool IsCasino { get; set; }
            public bool IsJumper { get; set; }
            public bool IsMovable { get; set; }
            public int Direction { get; set; } = 1;
            public bool isMonster { get; set; }
        }

        public class Bullet
        {
            public int WorldX { get; set; }
            public int WorldY { get; set; }
            public int Width { get; set; } = 8;
            public int Height { get; set; } = 18;
        }

        public class User
        {
            public string Name { get; set; }
            public int Score { get; set; }
        }

        List<WorldPlatform> platforms = new List<WorldPlatform>();
        List<Bullet> bullets = new List<Bullet>();
        Random rnd = new Random();

        int playerWorldX;
        int playerWorldY;
        int jumpSpeed = 18;
        int gravity = 0;
        int playerSpeed = 8;
        int cameraY = 0;
        bool goLeft = false;
        bool goRight = false;
        bool isPaused = false;
        public int score = 0;

        Label labelScore;
        string musicPath;
        Label labelPause;
        int scoreOffset = 0;
        int platformSpeed = 6;
        List<Image> images = new List<Image>();
        private Image up, down;
        int MonsterSpeed = 4;
        int bulletSpeed = 15;
        private string currentAnimation = "";

        // Кеш зображень (завантажуються 1 раз)
        private Image normalPlatformImg;
        private Image breakPlatformImg;
        private Image playerUpImg;
        private Image playerDownImg;
        private Image Monster;

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private byte[] shootSoundData;



        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        // Апаратне згладжування та буферизація вікна
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Space)
            {
                if (!panelMenu.Visible && !isPaused)
                {
                    FireBullet();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public Form1()
        {
            InitializeComponent();

            // Вмикаємо double-buffering для форми
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimerEvent;

            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.Paint += Form1_Paint; // Підключаємо ручний рендеринг гри
            this.KeyPreview = true;

            PrepareAndPlayBackgroundMusic();
        }
        private void PlayGameSound(byte[] audioResource)
        {
            if (!FormSettings.IsSoundOn || audioResource == null) return;

            try
            {
                // Перетворюємо масив байтів у потік пам'яті
                using (MemoryStream ms = new MemoryStream(audioResource))
                using (SoundPlayer effectPlayer = new SoundPlayer(ms))
                {
                    // ВИПРАВЛЕНО: Використовуємо PlaySync(), щоб встигнути відтворити короткий звук до очищення об'єкта
                    effectPlayer.PlaySync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Помилка звуку: " + ex.Message);
            }
        }
        private void AudioFile_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (audioFile != null && outputDevice != null && FormSettings.IsSoundOn)
            {
                audioFile.Position = 0;
                outputDevice.Play();
            }
        }

        private void ConvertMonsterToPlatform()
        {
            foreach (var p in platforms)
            {
                if (p.isMonster)
                {
                    p.isMonster = false;
                    p.IsCasino = false;
                    p.IsJumper = false;
                    p.IsMovable = false;
                    p.IsBreakable = false;
                    p.IsBroken = false;
                }
            }
            this.Invalidate();
        }

        private void TogglePause()
        {
            if (panelMenu.Visible) return;
            isPaused = !isPaused;

            if (isPaused)
            {
                gameTimer.Stop();
                labelPause.Location = new Point((this.ClientSize.Width - labelPause.Width) / 2, (this.ClientSize.Height - labelPause.Height) / 2);
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

                if (FormSettings.IsSoundOn) outputDevice.Play();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void UpdateVolume()
        {
            if (audioFile == null || outputDevice == null) return;
            if (!FormSettings.IsSoundOn) { audioFile.Volume = 0; return; }
            audioFile.Volume = FormSettings.Volume / 100f;
            if (outputDevice.PlaybackState != PlaybackState.Playing) outputDevice.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(450, 700);
            this.DoubleBuffered = true;
            pictureBoxPlayer.SizeMode = PictureBoxSizeMode.Zoom;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Завантажуємо текстури в пам'ять ОДИН раз
            normalPlatformImg = Image.FromStream(new MemoryStream(Properties.Resources.coledge));
            breakPlatformImg = Image.FromStream(new MemoryStream(Properties.Resources.breakcol));
            playerUpImg = Image.FromStream(new MemoryStream(Properties.Resources.zadneprup));
            playerDownImg = Image.FromStream(new MemoryStream(Properties.Resources.zandepr));
            Monster = Image.FromStream(new MemoryStream(Properties.Resources.solomko));

            shootSoundData = Properties.Resources.bullet;
            pictureBoxPlayer.Visible = false;

            labelScore = new Label { Text = "Очки: 0", Font = new Font("Arial", 16, FontStyle.Bold), ForeColor = Color.Black, BackColor = Color.Transparent, AutoSize = true, Location = new Point(10, 10) };
            this.Controls.Add(labelScore);

            labelPause = new Label { Text = "ПАУЗА\n[Натисніть P для продовження]", Font = new Font("Arial", 22, FontStyle.Bold), ForeColor = Color.Red, BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter, AutoSize = true, Visible = false };
            this.Controls.Add(labelPause);

            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            gameTimer.Stop();
            isPaused = false;
            if (labelScore != null) labelScore.Visible = false;
            if (labelPause != null) labelPause.Visible = false;

            platforms.Clear();
            bullets.Clear();

            panelMenu.Visible = true;
            panelMenu.BringToFront();
            this.Invalidate(); 
        }

        private void ClearBullets()
        {
            bullets.Clear();
        }

        private void GeneratePlatforms()
        {
            platforms.Clear();

            // Стартова платформа - завжди безпечна база
            platforms.Add(new WorldPlatform { WorldX = this.ClientSize.Width / 2 - 40, WorldY = 100 });
            int nextY = 180;

            for (int i = 0; i < 12; i++)
            {
                WorldPlatform wp = new WorldPlatform();

                // Дивимось, що ми створили прямо перед цією платформою
                WorldPlatform previousPlatform = platforms[platforms.Count - 1];

                // Якщо попередня платформа була монстром або ламалася - цю робимо залізобетонно безпечною
                if (previousPlatform.isMonster || previousPlatform.IsBreakable)
                {
                    wp.isMonster = false;
                    wp.IsBreakable = false;

                    int safeRoll = rnd.Next(0, 100);
                    if (safeRoll < 10) wp.IsCasino = true;
                    else if (safeRoll < 25) wp.IsJumper = true;
                    else if (safeRoll < 55) { wp.IsMovable = true; wp.Direction = rnd.Next(0, 2) == 0 ? -1 : 1; }
                }
                else
                {
                    // Якщо попередня була безпечною, можемо спокійно ровлювати випадковий тип
                    wp.IsCasino = rnd.Next(0, 100) < 5;
                    wp.IsJumper = rnd.Next(0, 100) < 10 && !wp.IsCasino;
                    wp.IsMovable = rnd.Next(0, 100) < 20 && !wp.IsCasino && !wp.IsJumper;

                    if (wp.IsCasino || wp.IsJumper) { wp.IsBreakable = false; }
                    else if (wp.IsMovable) { wp.Direction = rnd.Next(0, 2) == 0 ? -1 : 1; }
                    else { wp.IsBreakable = rnd.Next(0, 100) < 15; }
                }

                wp.WorldX = rnd.Next(10, this.ClientSize.Width - 90);
                wp.WorldY = nextY;
                nextY += rnd.Next(65, 80);

                platforms.Add(wp);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (panelMenu.Visible || isPaused) return;

            playerWorldY += gravity;
            gravity += 1;
            if (gravity > 12) gravity = 12;

            if (goLeft) playerWorldX -= playerSpeed;
            if (goRight) playerWorldX += playerSpeed;

            // Циклічний екран для гравця
            if (playerWorldX + 60 < 0) playerWorldX = this.ClientSize.Width;
            else if (playerWorldX > this.ClientSize.Width) playerWorldX = -60;

            int targetCameraY = playerWorldY - (this.ClientSize.Height / 2);
            if (targetCameraY < cameraY) cameraY += (targetCameraY - cameraY) / 4;

            int currentProgress = (-cameraY / 10) + scoreOffset;
            if (currentProgress > score)
            {
                score = currentProgress;
                labelScore.Text = "Очки: " + score;
            }

            // --- КОРЕКТНИЙ ХІТБОКС ГРАВЦЯ ---
            Rectangle playerRect = new Rectangle(playerWorldX + 12, playerWorldY + 4, 36, 50);

            // --- КОЛІЗІЯ З МОНСТРАМИ ---
            foreach (var p in platforms)
            {
                if (p.isMonster && !p.IsBroken)
                {
                    int shrinkX = (int)(p.Width * 0.30);
                    int shrinkY = (int)(p.Height * 0.30);
                    Rectangle monsterHitbox = new Rectangle(p.WorldX + shrinkX, p.WorldY + shrinkY, p.Width - (shrinkX * 2), p.Height - (shrinkY * 2));

                    if (playerRect.IntersectsWith(monsterHitbox))
                    {
                        gameTimer.Stop();
                        this.Refresh();
                        MessageBox.Show($"Тебе з'їв монстр!\nTвій результат: {score} очків.");
                        ShowMainMenu();
                        return;
                    }
                }
            }

            // --- ЛОГІКА ПОЛЬОТУ ТА КОЛІЗІЇ КУЛЬ ---
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                var bullet = bullets[i];
                bullet.WorldY -= bulletSpeed;

                Rectangle bulletRect = new Rectangle(bullet.WorldX, bullet.WorldY, bullet.Width, bullet.Height);
                bool bulletRemoved = false;

                foreach (var p in platforms)
                {
                    if (p.isMonster && !p.IsBroken)
                    {
                        int shrinkX = (int)(p.Width * 0.30);
                        int shrinkY = (int)(p.Height * 0.30);
                        Rectangle monsterHitbox = new Rectangle(p.WorldX + shrinkX, p.WorldY + shrinkY, p.Width - (shrinkX * 2), p.Height - (shrinkY * 2));

                        if (bulletRect.IntersectsWith(monsterHitbox))
                        {
                            p.IsBroken = true;
                            bullets.RemoveAt(i);
                            bulletRemoved = true;
                            break;
                        }
                    }
                }

                if (bulletRemoved) continue;

                if (bullet.WorldY - cameraY < -50) bullets.RemoveAt(i);
            }

            // --- КОЛІЗІЯ З ПЛАТФОРМАМИ (ПАДІННЯ ВНИЗ) ---
            if (gravity > 0)
            {
                foreach (var platform in platforms)
                {
                    if (platform.IsBroken || platform.isMonster) continue;

                    Rectangle platformRect = new Rectangle(platform.WorldX, platform.WorldY, platform.Width, 6);

                    if (playerRect.IntersectsWith(platformRect))
                    {
                        if ((playerWorldY + 54) - gravity <= platform.WorldY + 6)
                        {
                            if (platform.IsBreakable)
                            {
                                platform.IsBroken = true;
                                gravity = 5;
                            }
                            else
                            {
                                playerWorldY = platform.WorldY - 54;

                                if (platform.IsCasino)
                                {
                                    goLeft = false; goRight = false; gravity = 0;
                                    gameTimer.Stop();
                                    Casino cs = new Casino(score);
                                    cs.ShowDialog();
                                    ConvertMonsterToPlatform();

                                    scoreOffset += cs.CurrentScore - score;
                                    score = cs.CurrentScore;
                                    labelScore.Text = "Очки: " + score;

                                    gravity = -26;
                                    TogglePause(); // Залишаємо гру на паузі після виходу з казино
                                }
                                else if (platform.IsJumper) gravity = -jumpSpeed * 2;
                                else gravity = -jumpSpeed;

                                break;
                            }
                        }
                    }
                }
            }

            // --- ОНОВЛЕННЯ ТА ПЕРЕГЕНЕРАЦІЯ РУХОМИХ ОБ'ЄКТІВ ---
            foreach (var platform in platforms)
            {
                if (platform.IsMovable && !platform.IsBroken)
                {
                    platform.WorldX += platformSpeed * platform.Direction;
                    if (platform.WorldX <= 0) { platform.WorldX = 0; platform.Direction = 1; }
                    else if (platform.WorldX + platform.Width >= this.ClientSize.Width) { platform.WorldX = this.ClientSize.Width - platform.Width; platform.Direction = -1; }
                }

                if (platform.isMonster && !platform.IsBroken)
                {
                    platform.WorldX += MonsterSpeed * platform.Direction;
                    if (platform.WorldX <= 0) { platform.WorldX = 0; platform.Direction = 1; }
                    else if (platform.WorldX + platform.Width >= this.ClientSize.Width) { platform.WorldX = this.ClientSize.Width - platform.Width; platform.Direction = -1; }
                }

                // Перезапуск платформи наверх
                if (platform.WorldY - cameraY > this.ClientSize.Height + 50)
                {
                    int highestWorldY = this.ClientSize.Height;
                    WorldPlatform highestPlatform = null;
                    foreach (var p in platforms)
                    {
                        if (p.WorldY < highestWorldY) { highestWorldY = p.WorldY; highestPlatform = p; }
                    }

                    platform.WorldX = rnd.Next(10, this.ClientSize.Width - platform.Width - 10);
                    platform.WorldY = highestWorldY - rnd.Next(65, 80);

                    platform.IsCasino = false; platform.IsJumper = false; platform.IsMovable = false; platform.IsBreakable = false; platform.isMonster = false;

                    // ПЕРЕВІРКА "ВИХІД Є ЗАВЖДИ"
                    if (highestPlatform != null && (highestPlatform.isMonster || highestPlatform.IsBreakable))
                    {
                        int roll = rnd.Next(0, 100);
                        if (roll < 8) platform.IsCasino = true;
                        else if (roll < 18) platform.IsJumper = true;
                        else if (roll < 45) { platform.IsMovable = true; platform.Direction = rnd.Next(0, 2) == 0 ? 1 : -1; }
                    }
                    else
                    {
                        int roll = rnd.Next(0, 100);
                        if (score > 500 && rnd.Next(0, 100) < 15)
                        {
                            platform.isMonster = true;
                            platform.Direction = rnd.Next(0, 2) == 0 ? 1 : -1;
                        }
                        else if (roll < 5) platform.IsCasino = true;
                        else if (roll < 12) platform.IsJumper = true;
                        else if (roll < 35) { platform.IsMovable = true; platform.Direction = rnd.Next(0, 2) == 0 ? 1 : -1; }
                        else
                        {
                            platform.IsBreakable = rnd.Next(0, 100) < 15;
                        }
                    }

                    platform.IsBroken = false;
                }
            }

            if (playerWorldY - cameraY > this.ClientSize.Height)
            {
                gameTimer.Stop();
                MessageBox.Show($"Ти впав!\nТвій результат: {score} очків.");
                ShowMainMenu();
                return;
            }

            this.Invalidate();
        }

        // ==========================================
        // 🎨 СУПЕР-ОПТИМІЗОВАНИЙ КЕШОВАНИЙ РЕНДЕРИНГ ГРИ
        // ==========================================
        private void Form1_Paint(object sender, PaintEventArgs e)
        { 
            if (panelMenu.Visible) return;

            Graphics g = e.Graphics;

            // 1. Малюємо кулі
            using (Brush bulletBrush = new SolidBrush(Color.Orange))
            {
                foreach (var b in bullets)
                {
                    g.FillRectangle(bulletBrush, b.WorldX, b.WorldY - cameraY, b.Width, b.Height);
                }
            }

            // 2. Малюємо платформи та монстрів
            foreach (var p in platforms)
            {
                if (p.IsBroken) continue;

                int screenX = p.WorldX;
                int screenY = p.WorldY - cameraY;

                if (p.isMonster)
                {
                    g.DrawImage(Monster, screenX, screenY, p.Width, p.Height + 25);
                }
                else if (p.IsCasino)
                {
                    g.FillRectangle(Brushes.Magenta, screenX, screenY, p.Width, p.Height);
                    g.DrawImage(normalPlatformImg, screenX, screenY, p.Width, p.Height);
                }
                else if (p.IsMovable)
                {
                    g.FillRectangle(Brushes.OrangeRed, screenX, screenY, p.Width, p.Height);
                    g.DrawImage(normalPlatformImg, screenX, screenY, p.Width, p.Height);
                }
                else if (p.IsJumper)
                {
                    g.FillRectangle(Brushes.Blue, screenX, screenY, p.Width, p.Height);
                    g.DrawImage(normalPlatformImg, screenX, screenY, p.Width, p.Height);
                }
                else if (p.IsBreakable)
                {
                    g.FillRectangle(Brushes.Brown, screenX, screenY, p.Width, p.Height);
                    g.DrawImage(breakPlatformImg, screenX, screenY, p.Width, p.Height);
                }
                else
                {
                    g.DrawImage(normalPlatformImg, screenX, screenY, p.Width, p.Height);
                }
            }

            // 3. Малюємо гравця
            Image currentPlayerImg = (gravity < 0) ? playerUpImg : playerDownImg;
            g.DrawImage(currentPlayerImg, playerWorldX, playerWorldY - cameraY, 60, 60);
        }

        private void PlayShootSound()
        {
            if (!FormSettings.IsSoundOn) return;

            var ms = new MemoryStream(shootSoundData);

            var reader = new Mp3FileReader(ms);
            var output = new WaveOutEvent();

            output.Init(reader);

            output.Play();

            output.PlaybackStopped += (s, e) =>
            {
                output.Dispose();
                reader.Dispose();
                ms.Dispose();
            };
        }
        private void FireBullet()
        {
            PlayShootSound();
            Bullet b = new Bullet
            {
                WorldX = playerWorldX + 30 - 4,
                WorldY = playerWorldY
            };
            bullets.Add(b);
        }

        private void ResetGame()
        {
            score = 0; scoreOffset = 0; isPaused = false; currentAnimation = "";
            if (labelScore != null) { labelScore.Text = "Очки: 0"; labelScore.Visible = true; }
            if (labelPause != null) labelPause.Visible = false;

            cameraY = 0;
            GeneratePlatforms();
            bullets.Clear();

            playerWorldX = this.ClientSize.Width / 2 - 30;
            playerWorldY = 30;

            gravity = 0; goLeft = false; goRight = false;
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
            if (outputDevice != null) outputDevice.Stop();
            Application.Exit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P) { TogglePause(); return; }
            if (panelMenu.Visible || isPaused) return;

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
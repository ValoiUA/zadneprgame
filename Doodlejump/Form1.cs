using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Doodlejump
{
    public partial class Form1 : Form
    {
        // Клас для збереження чистих координат платформи у світі гри
        public class WorldPlatform
        {
            public PictureBox View { get; set; }
            public int WorldX { get; set; }
            public int WorldY { get; set; }
        }

        List<WorldPlatform> platforms = new List<WorldPlatform>();
        Random rnd = new Random();

        // Фізика гравця (світові координати)
        int playerWorldX;
        int playerWorldY;
        int jumpSpeed = 17;
        int gravity = 0;
        int playerSpeed = 8;

        // Віртуальна камера (її висота у світі)
        int cameraY = 0;

        bool goLeft = false;
        bool goRight = false;

        System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();

            gameTimer.Interval = 20;
            gameTimer.Tick += GameTimerEvent;

            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(450, 700);
            this.DoubleBuffered = true;
            pictureBoxPlayer.SizeMode = PictureBoxSizeMode.Zoom;

            ResetGame();
        }

        private void GeneratePlatforms()
        {
            // Видаляємо старі візуальні елементи
            foreach (var p in platforms) this.Controls.Remove(p.View);
            platforms.Clear();

            // 1. Стартова платформа прямо під ногами (у світі гри Y росте ВГОРУ)
            WorldPlatform first = new WorldPlatform();
            first.View = new PictureBox { Size = new Size(80, 15), SizeMode = PictureBoxSizeMode.StretchImage };
            SetPlatformResource(first.View);

            first.WorldX = this.ClientSize.Width / 2 - 40;
            first.WorldY = 100; // 100 пікселів від землі

            this.Controls.Add(first.View);
            platforms.Add(first);

            // 2. Будуємо стабільні сходинки вгору у світових координатах
            int nextY = 180;
            for (int i = 0; i < 12; i++)
            {
                WorldPlatform wp = new WorldPlatform();
                wp.View = new PictureBox { Size = new Size(80, 15), SizeMode = PictureBoxSizeMode.StretchImage };
                SetPlatformResource(wp.View);

                wp.WorldX = rnd.Next(10, this.ClientSize.Width - 90);
                wp.WorldY = nextY;

                nextY += rnd.Next(65, 80); // Кожна наступна платформа чітко вища за попередню

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
            catch
            {
                pb.BackColor = Color.Green;
            }
        }
        // Допоміжний метод для безпечного завантаження картинок ГРАВЦЯ
        // Допоміжний метод для безпечного завантаження картинок ГРАВЦЯ
        private void SetPlayerResource(PictureBox pb, byte[] resourceFile)
        {
            try
            {
                // 1. Запам'ятовуємо точні поточні розміри гравця з дизайнера
                int currentWidth = pb.Width;
                int currentHeight = pb.Height;

                using (MemoryStream ms = new MemoryStream(resourceFile))
                {
                    if (pb.Image != null) pb.Image.Dispose();

                    pb.Image = Image.FromStream(ms);
                }

                // 2. Гарантуємо, що режим масштабування залишається правильним
                pb.SizeMode = PictureBoxSizeMode.Zoom;

                // 3. Жорстко повертаємо початкові розміри куба, щоб він не розширився/звузився
                pb.Width = currentWidth;
                pb.Height = currentHeight;
            }
            catch
            {
                pb.BackColor = Color.Red;
            }
        }
        private void GameTimerEvent(object sender, EventArgs e)
        {
            // 1. Рух гравця у світі (Y зменшується, коли летимо вгору, і збільшується при падінні)
            playerWorldY += gravity;
            gravity += 1; // Гравітація збільшується щокадру

            // --- МАГІЯ ЗМІНИ КАРТИНКИ ТУТ ---
            // Якщо gravity < 0, значить гравець відштовхнувся і летить ВГОРУ
            if (gravity < 0)
            {
                // Замініть `Properties.Resources.doodle_jump` на назву ВАШОГО файлу стрибка в ресурсах!
                SetPlayerResource(pictureBoxPlayer, Properties.Resources.zadneprup);
            }
            else
            {
                // Якщо gravity >= 0, гравець ПАДАЄ вниз. Ставимо стару (стандартну) картинку.
                // Переконайтеся, що `Properties.Resources.coledge` — це назва вашого стандартного дудлера!
                SetPlayerResource(pictureBoxPlayer, Properties.Resources.zandepr);
            }
            // ---------------------------------

            // Обмеження швидкості падіння (залишаємо як було)
            if (gravity > 12) gravity = 12;

            // Керування по Х
            if (goLeft && playerWorldX > 0) playerWorldX -= playerSpeed;
            if (goRight && playerWorldX < this.ClientSize.Width - pictureBoxPlayer.Width) playerWorldX += playerSpeed;

            // 2. Плавний рух камери за гравцем
            // Якщо гравець піднімається вище середини екрану відносно камери
            int targetCameraY = playerWorldY - (this.ClientSize.Height / 2);
            if (targetCameraY < cameraY)
            {
                // Поступово (плавно) підтягуємо камеру вгору за гравцем
                cameraY += (targetCameraY - cameraY) / 4;
            }
            // 2. Плавний рух вліво/вправо з телепортацією між краями екрану
            if (goLeft)
            {
                playerWorldX -= playerSpeed;
            }
            if (goRight)
            {
                playerWorldX += playerSpeed;
            }
            if (playerWorldX + pictureBoxPlayer.Width < 0)
            {
                playerWorldX = this.ClientSize.Width;
            }
            else if (playerWorldX > this.ClientSize.Width)
            {
                playerWorldX = -pictureBoxPlayer.Width;
            }

            // 3. Перевірка колізії (використовуємо світові координати)
            if (gravity > 0)
            {
                foreach (var platform in platforms)
                {
                    // Перевірка перетину по X
                    if (playerWorldX + pictureBoxPlayer.Width >= platform.WorldX && playerWorldX <= platform.WorldX + platform.View.Width)
                    {
                        // Перевірка перетину по Y (ноги наступають на платформу)
                        if (playerWorldY + pictureBoxPlayer.Height >= platform.WorldY &&
                            playerWorldY + pictureBoxPlayer.Height <= platform.WorldY + gravity + 2)
                        {
                            playerWorldY = platform.WorldY - pictureBoxPlayer.Height;
                            gravity = -jumpSpeed; // Відскок!
                            break;
                        }
                    }
                }
            }

            // 4. Оновлення позицій на екрані (Магія камери 🎥)
            // Малюємо гравця відповідно до камери
            pictureBoxPlayer.Left = playerWorldX;
            pictureBoxPlayer.Top = playerWorldY - cameraY;

            foreach (var platform in platforms)
            {
                // Малюємо платформу відповідно до камери
                platform.View.Left = platform.WorldX;
                platform.View.Top = platform.WorldY - cameraY;

                // ДИНАМІЧНА ПОЯВА: якщо платформа залишилась далеко внизу під екраном
                if (platform.View.Top > this.ClientSize.Height + 50)
                {
                    // Знаходимо найвищу платформу у світі
                    int highestWorldY = this.ClientSize.Height;
                    foreach (var p in platforms)
                    {
                        if (p.WorldY < highestWorldY) highestWorldY = p.WorldY;
                    }

                    // Переносимо стару нижню платформу на самий верх у світі
                    platform.WorldX = rnd.Next(10, this.ClientSize.Width - platform.View.Width - 10);
                    platform.WorldY = highestWorldY - rnd.Next(65, 80);
                }
            }

            // 5. Програш (якщо гравець випав за нижню межу камери)
            if (pictureBoxPlayer.Top > this.ClientSize.Height)
            {
                gameTimer.Stop();
                MessageBox.Show("Ти впав! Камеру скинуто.");
                ResetGame();
            }
        }

        private void ResetGame()
        {
            cameraY = 0; // Скидаємо камеру на землю
            GeneratePlatforms();

            // Ставимо гравця над першою платформою
            playerWorldX = this.ClientSize.Width / 2 - pictureBoxPlayer.Width / 2;
            playerWorldY = 30; // Низько біля землі

            gravity = 0;
            goLeft = false;
            goRight = false;

            gameTimer.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
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
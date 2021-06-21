﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp25
{
    /// <summary>
    /// Логика взаимодействия для Level2.xaml
    /// </summary>
    public partial class Level2 : Window
    {
        bool goLeft, goRight;
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        int ImagesOfEnemy = 0;
        int bulletTimer = 0;
        int bulletTimerLimit = 90;
        int totalEnemies = 0;
        int totalFriends = 1;
        int enemySpeed = 6;
        bool gameOver = false;
        DispatcherTimer gameTimer = new DispatcherTimer();
        ImageBrush playerSkin = new ImageBrush();
        ImageBrush myCanvasSkin = new ImageBrush();

        public Level2()
        {
            InitializeComponent();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(30); 


            gameTimer.Start();
            playerSkin.ImageSource = new BitmapImage(new Uri("Images/MyShip_-3000.png", UriKind.Relative));
            player.Fill = playerSkin;
            myCanvas.Background = myCanvasSkin;
            myCanvasSkin.ImageSource = new BitmapImage(new Uri("Images/PSM_V69_D182_Small_magellanic_cloud_and_its_variable_stars.png", UriKind.Relative));
            myCanvas.Focus();
            MakeEnemies(60);
        }

        private void GameLoop(object sender, EventArgs e) // начало игрового цикла
        {
            Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            enemiesLeft.Content = "Осталось всего пришельцев: " + totalEnemies;

            if (goLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - 10);
            }
            if (goRight == true && Canvas.GetLeft(player) + 80 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + 10);
            }
            bulletTimer -= 3;
            if (bulletTimer < 0)
            {
                EnemyBulletMaker(Canvas.GetLeft(player) + 20, 10);
                Random random = new Random();
                EnemyBulletMaker(random.Next(0, 500), random.Next(3, 10));
                EnemyBulletMaker(random.Next(0, 500), random.Next(3, 10));
                EnemyBulletMaker(random.Next(0, 500), random.Next(3, 10));
                EnemyBulletMaker(random.Next(0, 500), random.Next(3, 10));
                bulletTimer = bulletTimerLimit;
            }
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (var y in myCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bullet.IntersectsWith(enemyHit))
                            {
                                itemsToRemove.Add(x);
                                itemsToRemove.Add(y);
                                totalEnemies -= 1;
                            }
                        }
                    }
                }
                
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + enemySpeed);

                    if (Canvas.GetLeft(x) > 820)
                    {
                        Canvas.SetLeft(x, -80);
                        Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10));
                    }
                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        ShowGameOver("Пришельцы захватили мир!!");
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemyBullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10); //скорость пули врага
                    if (Canvas.GetTop(x) > 480)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect enemyBulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (playerHitBox.IntersectsWith(enemyBulletHitBox))
                    {
                        ShowGameOver("Пришельцы испепелили вас!!");
                    }
                }
            }
            foreach (Rectangle i in itemsToRemove)
            {
                myCanvas.Children.Remove(i);
            }

            if(totalEnemies < 40)
            {
                enemySpeed = 12;
                bulletTimer -= 3;
                if (bulletTimer < 0)
                {
                    Random random = new Random();
                    EnemyBulletMaker(random.Next(100, 300), random.Next(3, 10));
                    EnemyBulletMaker(random.Next(400, 790), random.Next(3, 10));
                    bulletTimer = bulletTimerLimit;
                }
            }

            if (totalEnemies < 30)
            {
                enemySpeed = 15;
                bulletTimer -= 3;
                if (bulletTimer < 0)
                {
                    EnemyBulletMaker(Canvas.GetLeft(player) + 30, 10);
                    Random random = new Random();
                    EnemyBulletMaker(random.Next(0, 280), random.Next(3, 10));
                    bulletTimer = bulletTimerLimit;
                }
            }

            if (totalEnemies < 15)
            {
                enemySpeed = 18;
                bulletTimer -= 3;
                if (bulletTimer < 0)
                {
                    Random random = new Random();
                    EnemyBulletMaker(random.Next(0, 130), random.Next(3, 10));
                    EnemyBulletMaker(random.Next(0, 280), random.Next(3, 10));
                    EnemyBulletMaker(random.Next(0, 300), random.Next(3, 10));
                    EnemyBulletMaker(random.Next(500, 740), random.Next(3, 10));
                    bulletTimer = bulletTimerLimit;
                }
            }

            if (totalEnemies < 3)
            {
                enemySpeed = 21;
            }

            if (totalEnemies < 1)
            {
                ShowGameOver("Поздравляю вы выиграли!");
                Boss bs = new Boss();
                bs.Show();
                Close();          
            }
        } //конец цикла
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }

            if (e.Key == Key.Right)
            {
                goRight = true;
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }

            if (e.Key == Key.Right)
            {
                goRight = false;
            }

            if (e.Key == Key.Space)
            {
                ImageBrush bulletSkin = new ImageBrush();
                Rectangle newBullet = new Rectangle { Tag = "bullet", Height = 50, Width = 40, Fill = bulletSkin };
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                myCanvas.Children.Add(newBullet);
                bulletSkin.ImageSource = new BitmapImage(new Uri("Images/Bullet.png", UriKind.Relative));
            }

            if (e.Key == Key.Enter && gameOver == true)
            {
                Menu menu = new Menu();
                menu.Show();
                Close();
            }
        }

        public void EnemyBulletMaker(double x, double y)
        {
            ImageBrush bulletImage = new ImageBrush();
            Rectangle enemyBullet = new Rectangle { Tag = "enemyBullet", Height = 40, Width = 15, Fill = bulletImage, StrokeThickness = 5 };
            Canvas.SetTop(enemyBullet, y);
            Canvas.SetLeft(enemyBullet, x);
            myCanvas.Children.Add(enemyBullet);
            bulletImage.ImageSource = new BitmapImage(new Uri("Images/BossBullet2.png", UriKind.Relative));
        }

        public void MakeEnemies(int limit)
        {
            int left = 0;
            totalEnemies = limit;
            for (int i = 0; i < limit; i++)
            {
                ImageBrush enemySkin = new ImageBrush();
                Rectangle newEnemy = new Rectangle { Tag = "enemy", Height = 45, Width = 45, Fill = enemySkin };
                Canvas.SetTop(newEnemy, 30);
                Canvas.SetLeft(newEnemy, left);
                myCanvas.Children.Add(newEnemy);
                left -= 60;
                ImagesOfEnemy++;

                if (ImagesOfEnemy > 4)
                {
                    ImagesOfEnemy = 1;
                }
                switch (ImagesOfEnemy)
                {
                    case 1:
                        enemySkin.ImageSource = new BitmapImage(new Uri("Images/Alien1.png", UriKind.Relative));
                        break;
                    case 2:
                        enemySkin.ImageSource = new BitmapImage(new Uri("Images/Alien2.png", UriKind.Relative));
                        break;
                    case 3:
                        enemySkin.ImageSource = new BitmapImage(new Uri("Images/Alien3.png", UriKind.Relative));
                        break;
                    case 4:
                        enemySkin.ImageSource = new BitmapImage(new Uri("Images/Alien4.png", UriKind.Relative));
                        break;
                }
            }
        }
        public void ShowGameOver(string message)
        {
            gameOver = true;
            gameTimer.Stop();
            enemiesLeft.Content = " " + message + " Нажмите Enter чтобы снова играть";
        }
    }
}


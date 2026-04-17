using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für game_window.xaml
    /// </summary>
    public partial class game_window : Window
    {
       // List<tile> tiles = new List<tile>(); TODO matrix
        private int tiles_x { get; set; } = 10;
        private int tiles_y { get; set; } = 10;// nicht größer wie 200

        private int speed { get; set; } = 10;

        List<List<tile>> tiles_list = new List<List<tile>>();

        Food food;

        DispatcherTimer gametimer;

        snake_class Snake;

        public bool verloren = true;

        private Direction nextDirection;

        private int score;

        private void create_tiles_and_lines(int tiles_x, int tiles_y)
        {
            int totalsize = 400;

            int lines_x = tiles_x + 1;
            int lines_y = tiles_y + 1;

            int px_available_x = totalsize - lines_x * 2;
            int px_available_y = totalsize - lines_y * 2;

            double size_tile_x = px_available_x / (double)tiles_x;
            double size_tile_y = px_available_y / (double)tiles_y;

            tiles_list.Clear();

            // Tiles
            for (int y = 0; y < tiles_y; y++)
            {
                List<tile> row = new List<tile>();

                for (int x = 0; x < tiles_x; x++)
                {
                    tile _tile = new tile();
                    _tile.Width = size_tile_x;
                    _tile.Height = size_tile_y;

                    _tile.x = x;
                    _tile.y = y;

                    row.Add(_tile);
                    Canvas_grid.Children.Add(_tile);
                    _tile.x_pos = 2 + x * size_tile_x + x * 2;
                    _tile.y_pos = 2 + y * size_tile_y + y * 2;
                    Canvas.SetLeft(_tile, 2 + x * size_tile_x + x * 2);
                    Canvas.SetTop(_tile, 2 + y * size_tile_y + y * 2);
                }

                tiles_list.Add(row);
            }

            // Horizontale Linien
            for (int y = 0; y < tiles_y + 1; y++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4D4864"));
                line.StrokeThickness = 4;

                line.X1 = 0;
                line.X2 = totalsize;

                double posY = y * (size_tile_y + 2);
                line.Y1 = posY;
                line.Y2 = posY;

                Canvas_grid.Children.Add(line);
            }

            // Vertikale Linien
            for (int x = 0; x < tiles_x + 1; x++)
            {
                Line vLine = new Line();
                vLine.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4D4864"));
                vLine.StrokeThickness = 4;

                vLine.Y1 = 0;
                vLine.Y2 = totalsize;

                double posX = x * (size_tile_x + 2);
                vLine.X1 = posX;
                vLine.X2 = posX;

                Canvas_grid.Children.Add(vLine);
            }
        }

        private void Apply()
        {
            tiles_x = GameSettings.FieldWidth;
            tiles_y = GameSettings.FieldHeight;
            speed = GameSettings.Speed;
            
        }

        public void add_apple_2()
        {
            bool apple_here = false;
            for (int i = 0; i < tiles_y; i++)
            {
                for (int j = 0; j < tiles_x; j++)
                {
                    if (tiles_list[i][j].Modus == "apple")
                    {
                        apple_here = true;
                        break;
                    }
                }
            }

            if (apple_here)
            {
                var (x, y) = food.GetPosition();
                
                tiles_list[y][x].Modus = "apple";
            }
        }

        private void add_apple_matrix()
        {
            bool apple_here = false;
            for (int i = 0; i < tiles_y; i++)
            {
                for (int j = 0; j < tiles_x; j++)
                {
                    if (tiles_list[i][j].Modus == "apple")
                    {
                        apple_here = true;
                        break;
                    }
                }
            }

            if (apple_here)
            {
                var (x, y) = food.GetPosition();
                tiles_list[y][x].Modus = "emthy";
                food.Respawn();
                (x, y) = food.GetPosition();
                tiles_list[y][x].Modus = "apple";
            }
            else
            { 
                food.Respawn();
                var (x, y) = food.GetPosition();
                tiles_list[y][x].Modus = "apple";
            }
        }

        public game_window()
        {
            InitializeComponent();
            GameSettings.Score = 0;
            SnakeLogger.logger.Information("Game Window Initialized");
            Apply();
            SnakeLogger.logger.Debug("Settings Applied");
            food = new Food(tiles_x, tiles_y);
            SnakeLogger.logger.Debug("Food created");
            create_tiles_and_lines(tiles_x,tiles_y);
            SnakeLogger.logger.Debug("Made Tiles & Lines");
            //add_apple_matrix();

            //MessageBox.Show(tiles_list.ToString());
            //tiles_list[4][0].Modus = "apple";

            Snake = new snake_class(Canvas_grid, tiles_list);
            SnakeLogger.logger.Debug("Snake created");
            SpawnApple();

            gametimer = new DispatcherTimer();
            SnakeLogger.logger.Debug("Create Dispatcher Timer");
            Snake.update_canvas();
            gametimer.Interval = TimeSpan.FromMilliseconds(1000 / GameSettings.Speed);
            gametimer.Tick += Gametimer_Tick;
            gametimer.Start();
            SnakeLogger.logger.Debug("Start Dispatcher Timer");
        }

        public game_window(bool laden)
        {
            InitializeComponent();
            SnakeLogger.logger.Information("Game Window (Loaded Option) Initialized");
            string ser_settings = File.ReadAllText("settings.json");
            string[] settings_splitted = ser_settings.Split("|");

            GameSettings.FieldWidth = Convert.ToInt32(settings_splitted[0]);
            GameSettings.FieldHeight = Convert.ToInt32(settings_splitted[1]);
            GameSettings.InitialLength = Convert.ToInt32(settings_splitted[2]);
            GameSettings.Speed = Convert.ToInt32(settings_splitted[3]);
            GameSettings.TileSizeX = Convert.ToDouble(settings_splitted[4]);
            GameSettings.TileSizeY = Convert.ToDouble(settings_splitted[5]);
            GameSettings.Score = Convert.ToInt32(settings_splitted[6]);
            SnakeLogger.logger.Debug("Settings Initialized");
            score = GameSettings.Score;
            Lbl_Score.Content = $"Score: {score}";

            Apply();


            string json = File.ReadAllText("tiles.json");

            var rawTiles = JsonSerializer.Deserialize<List<List<string>>>(json);

            List<List<tile>> tiles = new List<List<tile>>();

            foreach (var list in rawTiles)
            {
                List<tile> neueListe = new List<tile>();

                foreach (var tileString in list)
                {
                    neueListe.Add(tile.DeserializeTile(tileString));
                }

                tiles.Add(neueListe);
            }

            tiles_list = tiles;
            SnakeLogger.logger.Debug("Tiles overwritten from save-file");

            for (int y = 0; y < tiles.Count; y++)
            {
                for (int x = 0; x < tiles[y].Count; x++)
                {
                    if (tiles[y][x].Modus == "apple")
                    {
                        food = new Food(tiles_x, tiles_y);
                        food.SetPos(x, y);
                        add_apple_2();
                    }
                }
            }

            json = File.ReadAllText("snake.json");

            var rawSnake = JsonSerializer.Deserialize<List<string>>(json);

            List<BodySegment> segments = new List<BodySegment>();

            foreach (var segmentString in rawSnake)
            {
                segments.Add(BodySegment.Deserialize(segmentString));
            }
            
            //food = new Food(tiles_x, tiles_y);
            //create_tiles_and_lines(tiles_x, tiles_y);

            // ab hier chat
            Canvas_grid.Children.Clear();

            Canvas_grid.Children.Clear();

            double totalsize = 400;

            int lines_x = tiles_x + 1;
            int lines_y = tiles_y + 1;

            int px_available_x = (int)totalsize - lines_x * 2;
            int px_available_y = (int)totalsize - lines_y * 2;

            double size_tile_x = px_available_x / (double)tiles_x;
            double size_tile_y = px_available_y / (double)tiles_y;

            for (int y = 0; y < tiles_list.Count; y++)
            {
                for (int x = 0; x < tiles_list[y].Count; x++)
                {
                    var t = tiles_list[y][x];

                    t.x = x;
                    t.y = y;

                    t.Width = size_tile_x;
                    t.Height = size_tile_y;

                    t.x_pos = 2 + x * size_tile_x + x * 2;
                    t.y_pos = 2 + y * size_tile_y + y * 2;

                    Canvas.SetLeft(t, t.x_pos);
                    Canvas.SetTop(t, t.y_pos);

                    Canvas_grid.Children.Add(t);
                }
            }
            for (int y = 0; y < tiles_y + 1; y++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4D4864"));
                line.StrokeThickness = 4;

                line.X1 = 0;
                line.X2 = totalsize;

                double posY = y * (size_tile_y + 2);
                line.Y1 = posY;
                line.Y2 = posY;

                Canvas_grid.Children.Add(line);
            }

            // Vertikale Linien
            for (int x = 0; x < tiles_x + 1; x++)
            {
                Line vLine = new Line();
                vLine.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4D4864"));
                vLine.StrokeThickness = 4;

                vLine.Y1 = 0;
                vLine.Y2 = totalsize;

                double posX = x * (size_tile_x + 2);
                vLine.X1 = posX;
                vLine.X2 = posX;

                Canvas_grid.Children.Add(vLine);
            }
            //chat ende
            SnakeLogger.logger.Debug($"Tiles and Lines applied to Canvas {tiles_x}x{tiles_y}");


            Snake = new snake_class(Canvas_grid, tiles_list);

            Snake.bodySegments = segments;
            SnakeLogger.logger.Debug("Snake loaded from Save-file");

            var head = Snake.bodySegments.Last();
            Snake.SetDirectionFromHead(head);
            nextDirection = head.GetDirection();

            gametimer = new DispatcherTimer();
            Snake.update_canvas();
            gametimer.Interval = TimeSpan.FromMilliseconds(1000 / GameSettings.Speed);
            gametimer.Tick += Gametimer_Tick;
            
            gametimer.Start();
            
        }

        private void Gametimer_Tick(object? sender, EventArgs e)
        {
            Snake.ChangeDirection(nextDirection);
            Snake.Move();
            SnakeLogger.logger.Debug($"Snake moved {nextDirection}");

            Snake.update_canvas(); // das hier runter
            SnakeLogger.logger.Debug("Canvas Updated");
            if (Snake.CheckCollision())
            {
                SnakeLogger.logger.Debug("Snake Collided with itself");
                gametimer.Stop();
                namen_eintragen addscore = new namen_eintragen();
                addscore.ShowDialog();
                this.Close();
                return;
            }
            var head = Snake.bodySegments.Last();
            var (x, y) = head.GetPosition();

            if (x < 0 || x >= tiles_x || y < 0 || y >= tiles_y)
            {
                SnakeLogger.logger.Debug("Snake crashed into Wall");
                gametimer.Stop();

                namen_eintragen addscore = new namen_eintragen();
                addscore.ShowDialog();

                SnakeLogger.logger.Information("Game Closed");
                this.Close();
                return;
            }

            if (tiles_list[y][x].Modus == "apple")
            {
                tiles_list[y][x].Modus = "emthy"; // sofort entfernen 

                score++;
                Snake.Grow();
                SnakeLogger.logger.Debug("Snake grew in Size by: 1");
                SpawnApple();
                SnakeLogger.logger.Debug("New Apple spawned");
                Lbl_Score.Content = $"Score: {score}";
                GameSettings.Score = score;
                SnakeLogger.logger.Debug("Score went up by: 1");
            }
            
            




            if (Snake.bodySegments.Count == tiles_x * tiles_y)
            {
                SnakeLogger.logger.Debug("Won Game");
                MessageBox.Show("du hast gewonnen btw");
                namen_eintragen addscore = new namen_eintragen();
                addscore.ShowDialog();
                SnakeLogger.logger.Information("Game Closed");
                this.Close();
                
            }
        }

        private void SpawnApple()
        { // chat code:
            int x, y;

            do
            {
                food.Respawn();
                (x, y) = food.GetPosition();

            } while (Snake.IsOnSnake(x, y)); 

            // alle alten Äpfel entfernen
            foreach (var row in tiles_list)
            {
                foreach (var t in row)
                {
                    if (t.Modus == "apple")
                        t.Modus = "emthy";
                }
            }

            // neuen setzen
            tiles_list[y][x].Modus = "apple";
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (Keyboard.IsKeyDown(Key.Space))
            {
                MessageBox.Show("test");
            }
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G)
            {
                Snake.Grow();
            }
            if (e.Key == Key.P)
            {
                add_apple_matrix();
            }
            if (e.Key == Key.W)
            {
                nextDirection = Direction.Up;
            }
            if (e.Key == Key.S)
            {
                nextDirection = Direction.Down;
            }
            if (e.Key == Key.A)
            {
                nextDirection = Direction.Left;
            }
            if (e.Key == Key.D)
            {
                nextDirection = Direction.Right;
            }
            if (e.Key == Key.Escape)
            {
                gametimer.Stop();
                pause_menue pause = new pause_menue(tiles_list, Snake);
                pause.ShowDialog();
                if (!pause.weiterspielen)
                {
                    this.Close();
                    return;
                }
                gametimer.Start();
            }
        }
    }
}

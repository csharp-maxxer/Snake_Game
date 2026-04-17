using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für pause_menue.xaml
    /// </summary>
    public partial class pause_menue : Window
    {
        public bool weiterspielen;
        List<List<tile>> tiles;
        snake_class Snake;
        


        public pause_menue(List<List<tile>> tiles_list, snake_class Snake)
        {
            InitializeComponent();
            SnakeLogger.logger.Information("Pause Menu opened & initialized");
            this.tiles = tiles_list;
            this.Snake = Snake;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            SnakeLogger.logger.Debug("Continue playing button pressed");
            weiterspielen = true;
            this.Close();
        }

        private void Exit_game_Click(object sender, RoutedEventArgs e)
        {
            SnakeLogger.logger.Debug("Exit Game button pressed");
            weiterspielen = false;
            this.Close();
        }

        private void safe_game_Click(object sender, RoutedEventArgs e)
        {
            SnakeLogger.logger.Information("Safe Game button pressed");
            //MessageBox.Show("lol i ka des ned maha weil i koa objekte serialisieren ka ;-;");
            List<List<string>> new_tiles = new List<List<string>>();
            

            foreach (var list in tiles)
            {
                List<string> zwischenspeicher = new List<string>();
                foreach (var _tile in list)
                {
                    zwischenspeicher.Add(tile.SerializeTile(_tile));
                }
                new_tiles.Add(zwischenspeicher);
            }
            File.WriteAllText("tiles.json",JsonSerializer.Serialize(new_tiles));
            SnakeLogger.logger.Debug("Tiles are written into safe File");


            List<string> snake_ser = new List<string>();
            foreach (var bodypart in Snake.bodySegments)
            {
                snake_ser.Add(bodypart.Serialize());
            }
            File.WriteAllText("snake.json", JsonSerializer.Serialize(snake_ser));

            string settings_ser = $"{GameSettings.FieldWidth.ToString()}|{GameSettings.FieldHeight.ToString()}|{GameSettings.InitialLength.ToString()}|{GameSettings.Speed.ToString()}|{GameSettings.TileSizeX.ToString()}|{GameSettings.TileSizeY.ToString()}|{GameSettings.Score.ToString()}";
            File.WriteAllText("settings.json", settings_ser);
            SnakeLogger.logger.Debug("Snake is written into safe File");
        }
    }
}

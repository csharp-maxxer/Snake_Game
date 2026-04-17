using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für tile.xaml
    /// </summary>
    public partial class tile : UserControl
    {
        private string modus = "emthy";
        public string Modus {
            get 
            {
                return modus;
            }
            set 
            {
                if (value == "apple")
                {
                    ellipse_apple.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FE6566"));
                }
                else
                {
                    ellipse_apple.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }

                modus = value;
            }
        }

        private double size = 0;
        private double width = 0;
        private double height = 0;
        public double x_pos = 0;
        public double y_pos = 0;
        public int x { get; set; } = -1;
        public int y { get; set; } = -1;
        public double Size 
        { 
            get
            {
                return size;
            }
            set
            { 
                size = value;
                rect_tile.Width = value;
                rect_tile.Height = value;

                double one_seventh = value / 7;
                
                ellipse_apple.Width = one_seventh * 5;
                ellipse_apple.Height = one_seventh * 5;

                Canvas.SetLeft(ellipse_apple, one_seventh);
                Canvas.SetTop(ellipse_apple, one_seventh);
            }
        }

        public double Width
        {
            get
            {
                return rect_tile.Width;
            }
            set
            {
                rect_tile.Width = value;
                double one_seventh = value / 7;

                ellipse_apple.Width = one_seventh * 5;

                Canvas.SetLeft(ellipse_apple, one_seventh);
            }
        }

        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                rect_tile.Height = value;
                double one_seventh = value / 7;

                ellipse_apple.Height = one_seventh * 5;

                Canvas.SetTop(ellipse_apple, one_seventh);
            }
        }

        public static string SerializeTile(tile tile)
        {
            var data = new
            {
                Width = tile.Width,
                Height = tile.Height,
                X = Canvas.GetLeft(tile),
                Y = Canvas.GetTop(tile),
                FillColor = (tile.rect_tile.Fill as SolidColorBrush)?.Color.ToString(),
                Modus = tile.Modus
            };

            return JsonSerializer.Serialize(data);
        }



        public tile()
        {
            InitializeComponent();
            Modus = modus;
        }

        public static tile DeserializeTile(string json)
        {
            JsonElement data = JsonSerializer.Deserialize<JsonElement>(json);

            tile t = new tile();

            t.Width = data.GetProperty("Width").GetDouble();
            t.Height = data.GetProperty("Height").GetDouble();

            double x = data.GetProperty("X").GetDouble();
            double y = data.GetProperty("Y").GetDouble();

            Canvas.SetLeft(t, x);
            Canvas.SetTop(t, y);

            if (data.TryGetProperty("FillColor", out JsonElement colorElement))
            {
                string color = colorElement.GetString();

                if (!string.IsNullOrEmpty(color))
                {
                    t.rect_tile.Fill = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString(color)
                    );
                }
            }

            if (data.TryGetProperty("Modus", out JsonElement modusElement))
            {
                t.Modus = modusElement.GetString();
            }


            return t;
        }
    }
}

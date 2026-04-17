using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für BodySegment.xaml
    /// </summary>
    public partial class BodySegment : UserControl
    {
        public int x { get; set; }
        public int y { get; set; }

        private bool head = false;
        public bool IsHead 
        {
            get
            {
                return head;
            }
            set
            {
                head = value;
                if (value)
                {
                    Draw_Head_Segment();
                }
            }
        }

        private Direction direction;
        public void SetDirection(Direction dir)
        {
            direction = dir;

            if (IsHead)
            {
                Draw_Head_Segment();
            }
        }

        public void Draw_Head_Segment()
        {
            Canvas_Background.Children.Clear();

            double hoehe_canvas = rect_background.ActualHeight;
            double breite_canvas = rect_background.ActualWidth;

            double ein_neuntel_y = hoehe_canvas / 9;
            double ein_neuntel_x = breite_canvas / 9;

            double auge1_x = 0, auge1_y = 0;
            double auge2_x = 0, auge2_y = 0;

            switch (direction)
            {
                case Direction.Right:
                    auge1_x = ein_neuntel_x * 5;
                    auge1_y = ein_neuntel_y;

                    auge2_x = ein_neuntel_x * 5;
                    auge2_y = ein_neuntel_y * 5;
                    break;

                case Direction.Left:
                    auge1_x = ein_neuntel_x * 1;
                    auge1_y = ein_neuntel_y;

                    auge2_x = ein_neuntel_x * 1;
                    auge2_y = ein_neuntel_y * 5;
                    break;

                case Direction.Up:
                    auge1_x = ein_neuntel_x;
                    auge1_y = ein_neuntel_y * 1;

                    auge2_x = ein_neuntel_x * 5;
                    auge2_y = ein_neuntel_y * 1;
                    break;

                case Direction.Down:
                    auge1_x = ein_neuntel_x;
                    auge1_y = ein_neuntel_y * 5;

                    auge2_x = ein_neuntel_x * 5;
                    auge2_y = ein_neuntel_y * 5;
                    break;
            }

            Ellipse auge1 = new Ellipse();
            auge1.Width = ein_neuntel_x * 3;
            auge1.Height = ein_neuntel_y * 3;
            auge1.Fill = Brushes.White;
            Canvas.SetTop(auge1, auge1_y);
            Canvas.SetLeft(auge1, auge1_x);
            Canvas_Background.Children.Add(auge1);

            Ellipse auge2 = new Ellipse();
            auge2.Width = ein_neuntel_x * 3;
            auge2.Height = ein_neuntel_y * 3;
            auge2.Fill = Brushes.White;
            Canvas.SetTop(auge2, auge2_y);
            Canvas.SetLeft(auge2, auge2_x);
            Canvas_Background.Children.Add(auge2);

            double offset_x = 0;
            double offset_y = 0;

            switch (direction)
            {
                case Direction.Right: 
                    offset_x = ein_neuntel_x * 0.65; 
                    break;
                case Direction.Left: 
                    offset_x = -ein_neuntel_x * 0.65;
                    break;
                case Direction.Up: 
                    offset_y = -ein_neuntel_y * 0.65; 
                    break;
                case Direction.Down: 
                    offset_y = ein_neuntel_y * 0.65; 
                    break;
            }

            Ellipse pupille1 = new Ellipse();
            pupille1.Width = ein_neuntel_x * 2;
            pupille1.Height = ein_neuntel_y * 2;
            pupille1.Fill = Brushes.Black;
            Canvas.SetLeft(pupille1, auge1_x + ein_neuntel_x * 0.5 + offset_x);
            Canvas.SetTop(pupille1, auge1_y + ein_neuntel_y * 0.5 + offset_y);
            Canvas_Background.Children.Add(pupille1);

            Ellipse pupille2 = new Ellipse();
            pupille2.Width = ein_neuntel_x * 2;
            pupille2.Height = ein_neuntel_y * 2;
            pupille2.Fill = Brushes.Black;
            Canvas.SetLeft(pupille2, auge2_x + ein_neuntel_x * 0.5 + offset_x);
            Canvas.SetTop(pupille2, auge2_y + ein_neuntel_y * 0.5 + offset_y);
            Canvas_Background.Children.Add(pupille2);


        }

        public BodySegment(int x, int y)
        {
            InitializeComponent();
            this.x = x;
            this.y = y;
            rect_background.Width = GameSettings.TileSizeX;
            rect_background.Height = GameSettings.TileSizeY;
            //MessageBox.Show($"{GameSettings.TileSizeX},{GameSettings.TileSizeY}");
            Loaded += BodySegment_Loaded;
        }

        private void BodySegment_Loaded(object sender, RoutedEventArgs e)
        {
            if (head)
                Draw_Head_Segment();
        }

        public (int, int) GetPosition()
        {
            return (x, y);
        }

        public string Serialize ()
        {
            
        
            return JsonSerializer.Serialize(new
            {
                X = x,
                Y = y,
                IsHead = this.IsHead,
                Direction = this.direction
            });
        }

        public static BodySegment Deserialize(string json)
        {
            JsonElement data = JsonSerializer.Deserialize<JsonElement>(json);

            int x = data.GetProperty("X").GetInt32();
            int y = data.GetProperty("Y").GetInt32();

            BodySegment segment = new BodySegment(x, y);

            if (data.TryGetProperty("IsHead", out var headProp))
            {
                segment.IsHead = headProp.GetBoolean();
            }

            if (data.TryGetProperty("Direction", out var dirProp))
            {
                Direction dir = (Direction)dirProp.GetInt32();
                segment.SetDirection(dir);
            }

            return segment;
        }

        public Direction GetDirection()
        {
            return direction;
        }
    }

        
    
}

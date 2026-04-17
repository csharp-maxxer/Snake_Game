using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Snake
{
    public class snake_class
    {
        public List<BodySegment> bodySegments;
        private bool isAlive = false;
        public Direction direction;
        Canvas grid_canvas;
        List<List<tile>> tiles;

        public snake_class() { }

        public snake_class(Canvas canvas, List<List<tile>> tiles)
        {
            bodySegments = new List<BodySegment>();
            grid_canvas = canvas;
            this.tiles = tiles;
            for (int i = 0; i < GameSettings.InitialLength; i++)
            {
                BodySegment start_segments = new BodySegment(i, 0);
                if (i == GameSettings.InitialLength-1)
                {
                    start_segments.IsHead = true;
                }

                bodySegments.Add(start_segments);
            }
            direction = Direction.Down;
        }

        public void update_canvas()
        {
            //MessageBox.Show($"hi");
            //löschen
            var segments = grid_canvas.Children.OfType<BodySegment>().ToList();
            foreach (var segment in segments)
            {
                grid_canvas.Children.Remove(segment);
            }

            //adden
            for (int i = 0; i < bodySegments.Count; i++)
            {
                BodySegment current_segment = bodySegments[i];

                var (current_x, current_y) = current_segment.GetPosition();
                // x       y

                // dazugehörige absolute pos:
                double x_pos = 0;
                double y_pos = 0;
                try
                {
                    x_pos = tiles[current_y][current_x].x_pos;
                    y_pos = tiles[current_y][current_x].y_pos;
                }
                catch 
                {
                    //MessageBox.Show("out of range exception");
                    isAlive = false;
                    
                }
                

                //placen
                Canvas.SetTop(current_segment, y_pos);
                Canvas.SetLeft(current_segment, x_pos);
                //MessageBox.Show($"{x_pos}{y_pos}");
                grid_canvas.Children.Add(current_segment);
            }
        }

        public void Move()
        {
            if (direction == Direction.Left)
            {
                int altes_x = bodySegments[bodySegments.Count - 1].x;
                int altex_y = bodySegments[bodySegments.Count - 1].y;
                bodySegments[bodySegments.Count - 1].x -= 1;

                if (bodySegments.Count != 1)
                {
                    BodySegment altes_teil = bodySegments[0];
                    altes_teil.x = altes_x;
                    altes_teil.y = altex_y;
                    bodySegments.RemoveAt(0);
                    bodySegments.Insert(bodySegments.Count - 1, altes_teil);
                }
            }
            if (direction == Direction.Down)
            {
                // head tile eins runter und letztes poppen


                int altes_x = bodySegments[bodySegments.Count - 1].x;
                int altex_y = bodySegments[bodySegments.Count - 1].y;
                bodySegments[bodySegments.Count - 1].y += 1;

                if (bodySegments.Count != 1)
                {
                    BodySegment altes_teil = bodySegments[0];
                    altes_teil.x = altes_x;
                    altes_teil.y = altex_y;
                    bodySegments.RemoveAt(0);
                    bodySegments.Insert(bodySegments.Count - 1, altes_teil);
                }
            }
            if (direction == Direction.Up)
            {
                int altes_x = bodySegments[bodySegments.Count - 1].x;
                int altex_y = bodySegments[bodySegments.Count - 1].y;
                bodySegments[bodySegments.Count - 1].y -= 1;

                if (bodySegments.Count != 1)
                {
                    BodySegment altes_teil = bodySegments[0];
                    altes_teil.x = altes_x;
                    altes_teil.y = altex_y;
                    bodySegments.RemoveAt(0);
                    bodySegments.Insert(bodySegments.Count - 1, altes_teil);
                }
            }
            if (direction == Direction.Right)
            {
                int altes_x = bodySegments[bodySegments.Count - 1].x;
                int altex_y = bodySegments[bodySegments.Count - 1].y;
                bodySegments[bodySegments.Count - 1].x += 1;

                if (bodySegments.Count != 1)
                {
                    BodySegment altes_teil = bodySegments[0];
                    altes_teil.x = altes_x;
                    altes_teil.y = altex_y;
                    bodySegments.RemoveAt(0);
                    bodySegments.Insert(bodySegments.Count - 1, altes_teil);
                }
            }

            // augen ausrichten
            foreach (var s in bodySegments)
                s.IsHead = false;

            var head = bodySegments[bodySegments.Count - 1];
            head.IsHead = true;
            head.SetDirection(direction);
        }

        public void ChangeDirection(Direction newDirection)
        {
            if ((direction == Direction.Up && newDirection == Direction.Down) || (direction == Direction.Down && newDirection == Direction.Up) || (direction == Direction.Left && newDirection == Direction.Right) || (direction == Direction.Right && newDirection == Direction.Left))
                return; // :)

            this.direction = newDirection;
        }

        public void Grow()
        {
            if (bodySegments.Count == 0)
                return;

            // letztes Segment holen (Tail)
            var tail = bodySegments[0];

            // neue Position = gleiche wie Tail
            BodySegment neuesSegment = new BodySegment(tail.x, tail.y);

            // hinten einfügen
            bodySegments.Insert(0, neuesSegment);
        }

        public bool CheckCollision()
        {
            var head = bodySegments[bodySegments.Count - 1];
            int x = head.x;
            int y = head.y;

            // wande
            if (x < 0 || x >= tiles[0].Count || y < 0 || y >= tiles.Count)
            {
                return true;
            }

            for (int i = 0; i < bodySegments.Count - 1; i++)
            {
                if (bodySegments[i].x == x && bodySegments[i].y == y)
                {
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            //wofür?
        }

        public void SetDirectionFromHead(BodySegment head)
        {
            this.direction = head.GetDirection();
        }

        public bool IsOnSnake(int x, int y)
        {
            foreach (var segment in bodySegments)
            {
                if (segment.x == x && segment.y == y)
                    return true;
            }
            return false;
        }

    }
}

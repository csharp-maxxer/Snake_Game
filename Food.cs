using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Snake
{
    public class Food
    {
        private int x { get; set; }
        private int y { get; set; }
        private int fieldWith {  get; set; }
        private int fieldHeight { get; set; }
        public Food(int fieldWith, int fieldHeight)
        {
            this.fieldWith = fieldWith;
            this.fieldHeight = fieldHeight;
        }

        public void SetPos(int xp, int yp)
        {
            x = xp;
            y = yp;
        }

        public void Respawn()
        {
            Random random = new Random();
            int random_y_pos = random.Next(fieldHeight);
            int random_x_pos = random.Next(fieldWith);
            x = random_x_pos;
            y = random_y_pos;  
        }

        public (int x, int y) GetPosition()
        {
            return (x, y);
        }
    }
}

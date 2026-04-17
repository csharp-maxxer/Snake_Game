using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public static class GameSettings
    {
        public static int FieldWidth { get; set; } = 5;
        public static int FieldHeight { get; set;} = 5;
        public static int InitialLength { get; set; } = 1;
        public static int Speed { get; set; } = 1;
        public static double TileSizeX { get; set; } = 77.6;
        public static double TileSizeY { get; set; } = 77.6;

        public static int Score { get; set; } = 0;
    }
}

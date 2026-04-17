using System;
using System.Collections.Generic;
using System.IO;
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

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für leaderboard.xaml
    /// </summary>
    public partial class leaderboard : Window
    {
        public leaderboard()
        {
            InitializeComponent();
            SnakeLogger.logger.Debug($"Initialized window: leaderboard");
            string[] lines = File.ReadAllLines("scores.json");
            SnakeLogger.logger.Debug($"Loaded data from: scores.json");
            List<(string Name, int Score)> liste = new List<(string, int)>();

            foreach (string line in lines)
            {
                string[] splitted = line.Split("|");
                liste.Add((splitted[0], Convert.ToInt32(splitted[1])));
            }

            //die line kommt von chat:
            liste = liste.OrderByDescending(x => x.Score).ToList();
            SnakeLogger.logger.Debug($"Sorted Scores");
            //chat ende

            //lb_scores.Items.Clear();
            if (liste.Count < 10)
            {
                for (int i = 0; i < liste.Count; i++)
                {
                    lb_scores.Items.Add($"{i+1}. {liste[i].Name}: {liste[i].Score}");
                    
                }
                SnakeLogger.logger.Debug($"Added scores to Leaderboard");
            }

            else
            {
                for (int i = 0; i < 10; i++)
                {
                    lb_scores.Items.Add($"{i+1}. {liste[i].Name}: {liste[i].Score}");
                    
                }
                SnakeLogger.logger.Debug($"Added scores to Leaderboard");
            }
        }
    }
}

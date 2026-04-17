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
    /// Interaktionslogik für namen_eintragen.xaml
    /// </summary>
    public partial class namen_eintragen : Window
    {
        public namen_eintragen()
        {
            InitializeComponent();
            SnakeLogger.logger.Debug($"Initialized window: namen_eintragen");
            Score_label.Content = $"Dein Score: {GameSettings.Score}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Textbox_name.Text == "")
            {
                MessageBox.Show("Bitte einen Namen eingeben");
                SnakeLogger.logger.Warning($"Tried to enter empthy name");
                return;
                
            }

            string[] liste = File.ReadAllLines("scores.json");
            SnakeLogger.logger.Debug($"Loaded file: Scores");
            List<string> scores = new List<string>();

            foreach (string s in liste)
            {
                scores.Add(s);
            }

            scores.Add($"{Textbox_name.Text}|{GameSettings.Score}");
            SnakeLogger.logger.Debug($"Added Name to List");
            File.WriteAllLines("scores.json", scores);
            SnakeLogger.logger.Debug($"Safed file: Scores");
            this.Close();
            
        }
    }
}

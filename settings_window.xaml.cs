using System;
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

namespace Snake
{
    /// <summary>
    /// Interaktionslogik für settings_window.xaml
    /// </summary>
    public partial class settings_window : Window
    {
        public settings_window()
        {
            InitializeComponent();
            SnakeLogger.logger.Debug("Settings window initialized");
            Lbl_Feldweite.Content = GameSettings.FieldWidth;
            Lbl_Feldhöhe.Content = GameSettings.FieldHeight;
            Lbl_Startlenght.Content = GameSettings.InitialLength;
            Lbl_Startspeed.Content = GameSettings.Speed;
            Slider_Feldweite.Value = GameSettings.FieldWidth;
            Slider_Feldhöhe.Value = GameSettings.FieldHeight;
            Slider_Startlenght.Value = GameSettings.InitialLength;
            Slider_Startspeed.Value = GameSettings.Speed;
            SnakeLogger.logger.Debug("Loaded current Settings into GUI");
        }

        private void Slider_Feldweite_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Lbl_Feldweite.Content = $"{Slider_Feldweite.Value}";
            SnakeLogger.logger.Debug($"Slider: Feldweite changed to {Slider_Feldweite.Value}");
        }

        private void Slider_Feldhöhe_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Lbl_Feldhöhe.Content = $"{Slider_Feldhöhe.Value}";
            SnakeLogger.logger.Debug($"Slider: Feldhöhe changed to {Slider_Feldhöhe.Value}");
        }

        private void Slider_Startlenght_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Lbl_Startlenght.Content = $"{Slider_Startlenght.Value}";
            SnakeLogger.logger.Debug($"Slider: Startlength changed to {Slider_Startlenght.Value}");
        }

        private void Slider_Startspeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Lbl_Startspeed.Content = $"{Slider_Startspeed.Value}";
            SnakeLogger.logger.Debug($"Slider: Startspeed changed to {Slider_Startspeed.Value}");
        }

        private void Button_quit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SnakeLogger.logger.Debug($"Attempted to quit Settings");
            GameSettings.FieldWidth = (int)Slider_Feldweite.Value;
            GameSettings.FieldHeight = (int) Slider_Feldhöhe.Value;
            GameSettings.InitialLength = (int) Slider_Startlenght.Value;
            GameSettings.Speed = (int)Slider_Startspeed.Value;

            GameSettings.TileSizeX = (400.0 - (GameSettings.FieldWidth + 1) * 2.0) / GameSettings.FieldWidth;
            GameSettings.TileSizeY = (400.0 - (GameSettings.FieldHeight + 1) * 2.0) / GameSettings.FieldHeight;
            //MessageBox.Show($"{GameSettings.FieldWidth}{GameSettings.FieldHeight}{GameSettings.InitialLength}{GameSettings.Speed}");
            SnakeLogger.logger.Debug("Safed current settings");
            this.Close();
        }
    }
}

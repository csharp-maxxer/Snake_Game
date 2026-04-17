using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SnakeLogger.init("snake.log");
            SnakeLogger.logger.Information("Main Window Loaded");
        }

        private void Button_newGame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SnakeLogger.logger.Debug("New Game Button Pressed");
            game_window game = new game_window();
            game.ShowDialog();
            SnakeLogger.logger.Debug($"Game Closed=true");
        }

        private void Button_settings_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SnakeLogger.logger.Debug("Settings Button Pressed");
            settings_window settings = new settings_window();
            settings.ShowDialog();
            SnakeLogger.logger.Debug($"Settings Closed");
        }

        private void Button_loadGame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SnakeLogger.logger.Debug("Load Game Button Pressed");
            game_window game_Window = new game_window(true);
            game_Window.ShowDialog();
            SnakeLogger.logger.Debug($"Game Closed=true");
        }

        private void Button_Leaderboards_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SnakeLogger.logger.Debug("Leaderboards Button Pressed");
            leaderboard lb = new leaderboard();
            lb.ShowDialog();
            SnakeLogger.logger.Debug($"Leaderboards Closed");
        }
    }
}
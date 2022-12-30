using DDD_WPF.Screens._02X_Game;
using DDD_WPF.Screens._04X_Statistics;
using DDD_WPF.Screens._06X_Player;
using DDD_WPF.Screens._Global;
using System.Windows;

namespace DDD_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion
        #region navigation
        private void _001_Btn_Player_Click(object sender, RoutedEventArgs e)
        {
            var _060_Player_Main = new _060_Player_Main();
            _060_Player_Main.Show();
            this.Close();
        }
        private void _001_Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, 9999, "Exit the game", "Do you really want to exit?");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler;
            prompt.Show();
            prompt.Dispose();
        }
        private void _001_Btn_Statistic_Click(object sender, RoutedEventArgs e)
        {
            var _040_Statistic_Main = new _040_Statistic_Main();
            _040_Statistic_Main.Show();
            this.Close();
        }

        private void _001_Btn_NewGame_Click(object sender, RoutedEventArgs e)
        {
            var _020_Game_NewGame = new _020_Game_NewGame();
            _020_Game_NewGame.Show();
            this.Close();
        }

        #endregion
        #region event prompt
        private void Prompt_ReturnEventHandler(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1 && e.ReturnID == 9999) System.Environment.Exit(0);
        }
        #endregion

        private void _001_Btn_NewGame_Test_Click(object sender, RoutedEventArgs e)
        {
            var OpenDartBoard = new DDD_WPF.Screens._02X_Game._032_Game_Bot_Board(false);
            OpenDartBoard.Show();
        }
    }
}

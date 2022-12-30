#define Debug
using DDD;
using DDD.Program;
using DDD_WPF.Screens._04X_Statistics;
using DDD_WPF.Screens._Global;
using System;
using System.Windows;
using System.Windows.Documents;

namespace DDD_WPF.Screens._02X_Game
{
    /// <summary>
    /// Interaction logic for _020_Game_NewGame.xaml
    /// </summary>
    public partial class _020_Game_NewGame : Window
    {
        #region constructor
        public _020_Game_NewGame()
        {
            InitializeComponent();
            FirstExecute();
#if Debug
            _020_ListBox_Player.SelectAll();
#endif
        }
        #endregion
        #region methods
        public void FirstExecute()
        {
            _020_Sli_BotDifficult.Value = 1;
            _020_Sli_GameMode.Value = 1;
            _020_Sli_Points.Value = 1;

            _020_Sli_Sets.Value = 0;
            _020_Txt_Out_Val_Sets.Text = "1";
            _020_Sli_Legs.Value = 0;
            _020_Txt_Out_Val_Legs.Text = "1";

            new DHListBox(_020_ListBox_Player).SetPlayer();

            _020_ListBox_Player.Items.Add("Bot");
        }
        #endregion
        #region navigation header
        private void _020_Btn_Menu_Home_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
        private void _020_Btn_Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, 9999, "Exit the game", "Do you really want to exit?");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler;
            prompt.Show();
            prompt.Dispose();
        }
        private void _020_Btn_Menu_Statistic_Click(object sender, RoutedEventArgs e)
        {
            var _040_Game_Player = new _04X_Statistics._040_Statistic_Main();
            _040_Game_Player.Show();
            this.Close();
        }
        private void _020_Btn_Menu_Player_Click(object sender, RoutedEventArgs e)
        {
            var _060_Game_Player = new _06X_Player._060_Player_Main();
            _060_Game_Player.Show();
            this.Close();
        }

        #endregion
        #region event prompt
        private void Prompt_ReturnEventHandler(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1 && e.ReturnID == 9999) System.Environment.Exit(0);
        }
        #endregion
        #region user interaction
        private void _020_Btn_NewGame_Click(object sender, RoutedEventArgs e)
        {
            #region input
            var gameProp = new Game.MainProp();
            var gamePropPlayer = new Game.MainProp.Player[_020_ListBox_Player.SelectedItems.Count];

            gameProp.CountPlayer = _020_ListBox_Player.SelectedItems.Count;
            gameProp.DTGameStarted = DateTime.Now.Ticks;

            if (_020_Sli_BotDifficult.Value >= 0 && _020_Sli_BotDifficult.Value <= 33)
            {
                gameProp.BotDifficultyValue = 1;
                gameProp.BotDifficultyName = FindResource("_G_Local_020_Txt_Out_Val_BotDifficult_1").ToString();
            }
            else if (_020_Sli_BotDifficult.Value > 33 && _020_Sli_BotDifficult.Value <= 66)
            {
                gameProp.BotDifficultyValue = 2;
                gameProp.BotDifficultyName = FindResource("_G_Local_020_Txt_Out_Val_BotDifficult_2").ToString();
            }
            else if (_020_Sli_BotDifficult.Value > 66 && _020_Sli_BotDifficult.Value <= 100)
            {
                gameProp.BotDifficultyValue = 3;
                gameProp.BotDifficultyName = FindResource("_G_Local_020_Txt_Out_Val_BotDifficult_3").ToString();
            }
            for (int i = 0; i < _020_ListBox_Player.SelectedItems.Count; i++)
                if (_020_ListBox_Player.SelectedItems[i].ToString() == "Bot")
                {
                    gameProp.BotCheck = true;
                    break;
                }
                else
                {
                    gameProp.BotCheck = false;
                }
            Int32.TryParse(_020_Txt_Out_Val_Points.Text, out gameProp.Points);
            Int32.TryParse(_020_Txt_Out_Val_Sets.Text, out gameProp.Sets);
            Int32.TryParse(_020_Txt_Out_Val_Legs.Text, out gameProp.Legs);
            gameProp.GameMode = _020_Txt_Out_Val_GameMode.Text;
            for (int i = 0; i < _020_ListBox_Player.SelectedItems.Count; i++)
            {
                gamePropPlayer[i].PlayerIndex =
                    DatahandlingPlayer.GetIndexFromPlayerName(_020_ListBox_Player.SelectedItems[i].ToString().TrimEnd());
                gamePropPlayer[i].PlayerName =
                    _020_ListBox_Player.SelectedItems[i].ToString();
            }
            gameProp.PlayersStart = gamePropPlayer;
            Game.MainProperties = gameProp;
            #endregion
            #region sequence
            Sequence.Game sqPlayer = new Sequence.Game();
            sqPlayer.EventHandlerNewGame += SqPlayer_EventHandlerNewGame;
            sqPlayer.GameNew(1);
            sqPlayer.Dispose();
            #endregion
        }
        private void SqPlayer_EventHandlerNewGame(object sender, Sequence.Game.EventArgNewGame e)
        {
            var _030_Game_Overview = new _030_Game_Overview();
            _030_Game_Overview.Show();
            this.Close();
        }
        #endregion
        #region user background interaction
        private void _020_Sli_GameMode_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_020_Sli_GameMode.Value <= 33)
            {
                _020_Sli_GameMode.Value = 0;
                _020_Txt_Out_Val_GameMode.Text = FindResource("_G_Local_020_Txt_Out_Val_GameMode_1").ToString();
            }
            else if (_020_Sli_GameMode.Value > 33 && _020_Sli_GameMode.Value < 66)
            {
                _020_Sli_GameMode.Value = 50;
                _020_Txt_Out_Val_GameMode.Text = FindResource("_G_Local_020_Txt_Out_Val_GameMode_2").ToString();
            }
            else if (_020_Sli_GameMode.Value >= 66)
            {
                _020_Sli_GameMode.Value = 100;
                _020_Txt_Out_Val_GameMode.Text = FindResource("_G_Local_020_Txt_Out_Val_GameMode_3").ToString();
            }
        }
        private void _020_Sli_Points_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_020_Sli_Points.Value <= 33)
            {
                _020_Sli_Points.Value = 0;
                _020_Txt_Out_Val_Points.Text = FindResource("_G_Local_020_Txt_Out_Val_Points_1").ToString();
            }
            else if (_020_Sli_Points.Value > 33 && _020_Sli_Points.Value < 66)
            {
                _020_Sli_Points.Value = 50;
                _020_Txt_Out_Val_Points.Text = FindResource("_G_Local_020_Txt_Out_Val_Points_2").ToString();
            }
            else if (_020_Sli_Points.Value >= 66)
            {
                _020_Sli_Points.Value = 100;
                _020_Txt_Out_Val_Points.Text = FindResource("_G_Local_020_Txt_Out_Val_Points_3").ToString();
            }
        }
        private void _020_Sli_BotDifficult_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_020_Sli_BotDifficult.Value <= 33)
            {
                _020_Sli_BotDifficult.Value = 0;
                _020_Txt_Out_Val_BotDifficult.Text = FindResource("_G_Local_020_Txt_Out_Val_BotDifficult_1").ToString();
            }
            else if (_020_Sli_BotDifficult.Value > 33 && _020_Sli_BotDifficult.Value < 66)
            {
                _020_Sli_BotDifficult.Value = 50;
                _020_Txt_Out_Val_BotDifficult.Text = FindResource("_G_Local_020_Txt_Out_Val_BotDifficult_2").ToString();
            }
            else if (_020_Sli_BotDifficult.Value >= 66)
            {
                _020_Sli_BotDifficult.Value = 100;
                _020_Txt_Out_Val_BotDifficult.Text = FindResource("_G_Local_020_Txt_Out_Val_BotDifficult_3").ToString();
            }
        }
        private void _020_Sli_Sets_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_020_Sli_Sets.Value <= 10)
            {
                _020_Sli_Sets.Value = 0;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_01").ToString();
            }
            else if (_020_Sli_Sets.Value > 10 && _020_Sli_Sets.Value <= 20)
            {
                _020_Sli_Sets.Value = 11;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_02").ToString();
            }
            else if (_020_Sli_Sets.Value > 20 && _020_Sli_Sets.Value <= 30)
            {
                _020_Sli_Sets.Value = 22;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_03").ToString();
            }
            else if (_020_Sli_Sets.Value > 30 && _020_Sli_Sets.Value <= 40)
            {
                _020_Sli_Sets.Value = 33;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_04").ToString();
            }
            else if (_020_Sli_Sets.Value > 40 && _020_Sli_Sets.Value <= 50)
            {
                _020_Sli_Sets.Value = 44;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_05").ToString();
            }
            else if (_020_Sli_Sets.Value > 50 && _020_Sli_Sets.Value <= 60)
            {
                _020_Sli_Sets.Value = 55;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_06").ToString();
            }
            else if (_020_Sli_Sets.Value > 60 && _020_Sli_Sets.Value <= 70)
            {
                _020_Sli_Sets.Value = 66;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_07").ToString();
            }
            else if (_020_Sli_Sets.Value > 70 && _020_Sli_Sets.Value <= 80)
            {
                _020_Sli_Sets.Value = 77;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_08").ToString();
            }
            else if (_020_Sli_Sets.Value > 80 && _020_Sli_Sets.Value <= 90)
            {
                _020_Sli_Sets.Value = 89;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_09").ToString();
            }
            else if (_020_Sli_Sets.Value > 90 && _020_Sli_Sets.Value <= 100)
            {
                _020_Sli_Sets.Value = 100;
                _020_Txt_Out_Val_Sets.Text = FindResource("_G_Local_020_Txt_Out_Val_Sets_10").ToString();
            }
        }
        private void _020_Sli_Legs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_020_Sli_Legs.Value <= 10)
            {
                _020_Sli_Legs.Value = 0;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_01").ToString();
            }
            else if (_020_Sli_Legs.Value > 10 && _020_Sli_Legs.Value <= 20)
            {
                _020_Sli_Legs.Value = 11;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_02").ToString();
            }
            else if (_020_Sli_Legs.Value > 20 && _020_Sli_Legs.Value <= 30)
            {
                _020_Sli_Legs.Value = 22;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_03").ToString();
            }
            else if (_020_Sli_Legs.Value > 30 && _020_Sli_Legs.Value <= 40)
            {
                _020_Sli_Legs.Value = 33;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_04").ToString();
            }
            else if (_020_Sli_Legs.Value > 40 && _020_Sli_Legs.Value <= 50)
            {
                _020_Sli_Legs.Value = 44;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_05").ToString();
            }
            else if (_020_Sli_Legs.Value > 50 && _020_Sli_Legs.Value <= 60)
            {
                _020_Sli_Legs.Value = 55;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_06").ToString();
            }
            else if (_020_Sli_Legs.Value > 60 && _020_Sli_Legs.Value <= 70)
            {
                _020_Sli_Legs.Value = 66;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_07").ToString();
            }
            else if (_020_Sli_Legs.Value > 70 && _020_Sli_Legs.Value <= 80)
            {
                _020_Sli_Legs.Value = 77;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_08").ToString();
            }
            else if (_020_Sli_Legs.Value > 80 && _020_Sli_Legs.Value <= 90)
            {
                _020_Sli_Legs.Value = 89;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_09").ToString();
            }
            else if (_020_Sli_Legs.Value > 90 && _020_Sli_Legs.Value <= 100)
            {
                _020_Sli_Legs.Value = 100;
                _020_Txt_Out_Val_Legs.Text = FindResource("_G_Local_020_Txt_Out_Val_Legs_10").ToString();
            }
        }
        #endregion
    }
}

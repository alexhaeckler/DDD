using DDD;
using DDD.Program;
using System;
using DDD_WPF.Screens._Global;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace DDD_WPF.Screens._04X_Statistics
{
    /// <summary>
    /// Interaction logic for _040_Statistic_Main.xaml
    /// </summary>
    public partial class _040_Statistic_Main : Window
    {
        #region constructor
        public _040_Statistic_Main()
        {
            InitializeComponent();
            FirstExecute();
        }
        #endregion
        #region properties, fields, constants
        private bool _ActiveGameStat { get; set; }
        public bool ActiveGameStat
        {
            get { return _ActiveGameStat; }
            set
            {
                _ActiveGameStat = value;
                _ActivePlayerStat = false;
                _ActiveGraphicStat = false;

                _040_Btn_Games.IsEnabled = false;
                _040_Btn_Player.IsEnabled = true;
                _040_Btn_Graphs.IsEnabled = true;
            }
        }
        private bool _ActivePlayerStat { get; set; }
        public bool ActivePlayerStat
        {
            get { return _ActivePlayerStat; }
            set
            {
                _ActiveGameStat = false;
                _ActivePlayerStat = value;
                _ActiveGraphicStat = false;

                _040_Btn_Games.IsEnabled = true;
                _040_Btn_Player.IsEnabled = false;
                _040_Btn_Graphs.IsEnabled = true;
            }
        }
        private bool _ActiveGraphicStat { get; set; }
        public bool ActiveGraphicStat
        {
            get { return _ActiveGraphicStat; }
            set
            {
                _ActiveGameStat = false;
                _ActivePlayerStat = false;
                _ActiveGraphicStat = value;

                _040_Btn_Games.IsEnabled = true;
                _040_Btn_Player.IsEnabled = true;
                _040_Btn_Graphs.IsEnabled = false;
            }
        }

        private bool _Active_DG_Player { get; set; }
        public bool Active_DG_Player
        {
            get { return _Active_DG_Player; }
            set
            {
                _Active_DG_Player = value;
                _Active_DG_Leg = false;
                _Active_DG_GameStatistic = false;

                _040_Btn_DG_GameStatistic.IsEnabled = true;
                _040_Btn_DG_Legs.IsEnabled = true;
                _040_Btn_DG_Player.IsEnabled = false;

            }
        }
        private bool _Active_DG_Leg { get; set; }
        public bool Active_DG_Leg
        {
            get { return _Active_DG_Leg; }
            set
            {
                _Active_DG_Leg = value;
                _Active_DG_GameStatistic = false;
                _Active_DG_Player = false;

                _040_Btn_DG_GameStatistic.IsEnabled = true;
                _040_Btn_DG_Legs.IsEnabled = false;
                _040_Btn_DG_Player.IsEnabled = true;
            }
        }
        private bool _Active_DG_GameStatistic { get; set; }
        public bool Active_DG_GameStatistic
        {
            get { return _Active_DG_GameStatistic; }
            set
            {
                _Active_DG_GameStatistic = value;
                _Active_DG_Leg = false;
                _Active_DG_Player = false;

                _040_Btn_DG_GameStatistic.IsEnabled = false;
                _040_Btn_DG_Legs.IsEnabled = true;
                _040_Btn_DG_Player.IsEnabled = true;
            }
        }

        private string[] GameFileNames;
        private int CountSelectedIndex;
        private DatahandlingGame GameStatisticStruct;
        private DatahandlingPlayerStat.MainStruct MainPlayerStat;

        #endregion
        #region methods
        public void FirstExecute()
        {
            var dhGame = new DatahandlingGame(Configuration.FilePathDDDG3D);
            GameFileNames = dhGame.GetFileNames();
            dhGame.Dispose();
            SetVisiblePlayer();
        }
        private void SetVisibleGame()
        {
            #region input
            DHListBox dhListBox = new DHListBox(_040_ListBox);
            #endregion
            #region sequence
            _040_ListBox.Items.Clear();
            dhListBox.SetGameData(GameFileNames);
            dhListBox.Dispose();
            ActiveGameStat = true;
            #endregion
            #region output

            Active_DG_Player = true;

            // value out
            _040_Txt_Out_Val_AvgPoints.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Games.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_LegsWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Loss.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Player.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Points.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_SetsWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Throw.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Win.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_AvgPosition.Visibility = Visibility.Hidden;

            // display
            _040_Txt_Out_AvgPosition.Visibility = Visibility.Hidden;
            _040_Txt_Out_Games.Visibility = Visibility.Hidden;
            _040_Txt_Out_LegWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Loss.Visibility = Visibility.Hidden;
            _040_Txt_Out_Points.Visibility = Visibility.Hidden;
            _040_Txt_Out_SetsWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Throws.Visibility = Visibility.Hidden;
            _040_Txt_Out_Win.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_AvgPoints.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_DG_Game.Visibility = Visibility.Visible;

            // export
            _040_Btn_Export_Player.Visibility = Visibility.Hidden;
            _040_Btn_Export_Game.Visibility = Visibility.Visible;
            
            // datagrid
            _040_Btn_DG_GameStatistic.Visibility = Visibility.Visible;
            _040_Btn_DG_Legs.Visibility = Visibility.Visible;
            _040_Btn_DG_Player.Visibility = Visibility.Visible;
            _040_DG_GameStatistic_Player.Visibility = Visibility.Visible;
            _040_DG_GameStatistic_Legs.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic.Visibility = Visibility.Hidden;

            // listbox
            if (_040_ListBox.Items.Count > 0)
                _040_ListBox.SelectedIndex = 0;

            // buttons
            #endregion
        }
        private void SetVisiblePlayer()
        {
            #region input
            DHListBox dHListBox = new DHListBox(_040_ListBox);
            #endregion
            #region sequence
            _040_ListBox.Items.Clear();
            dHListBox.SetPlayer();
            ActivePlayerStat = true;
            dHListBox.Dispose();
            #endregion
            #region output
            // value out
            _040_Txt_Out_Val_AvgPoints.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_Games.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_LegsWin.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_Loss.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_Player.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_Points.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_SetsWin.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_Throw.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_Win.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_AvgPosition.Visibility = Visibility.Visible;

            // display
            _040_Txt_Out_AvgPosition.Visibility = Visibility.Visible;
            _040_Txt_Out_Games.Visibility = Visibility.Visible;
            _040_Txt_Out_LegWin.Visibility = Visibility.Visible;
            _040_Txt_Out_Loss.Visibility = Visibility.Visible;
            _040_Txt_Out_Points.Visibility = Visibility.Visible;
            _040_Txt_Out_SetsWin.Visibility = Visibility.Visible;
            _040_Txt_Out_Throws.Visibility = Visibility.Visible;
            _040_Txt_Out_Win.Visibility = Visibility.Visible;
            _040_Txt_Out_AvgPoints.Visibility = Visibility.Visible;
            _040_Txt_Out_Val_DG_Game.Visibility = Visibility.Hidden;

            // export
            _040_Btn_Export_Player.Visibility = Visibility.Visible;
            _040_Btn_Export_Game.Visibility = Visibility.Hidden;

            // datagrid
            _040_Btn_DG_GameStatistic.Visibility = Visibility.Hidden;
            _040_Btn_DG_Legs.Visibility = Visibility.Hidden;
            _040_Btn_DG_Player.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Player.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Legs.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic.Visibility = Visibility.Hidden;

            // listbox
            if (_040_ListBox.Items.Count > 0)
                _040_ListBox.SelectedIndex = 0;
            #endregion
        }
        private void SetVisibleGraphics()
        {
            #region input
            DHListBox dHListBox = new DHListBox(_040_ListBox);
            #endregion
            #region sequence
            _040_ListBox.Items.Clear();
            dHListBox.SetPlayer();
            ActiveGraphicStat = true;
            dHListBox.Dispose();
            #endregion
            #region output
            // value out
            _040_Txt_Out_Val_AvgPoints.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Games.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_LegsWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Loss.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Player.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Points.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_SetsWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Throw.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_Win.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_AvgPosition.Visibility = Visibility.Hidden;
            _040_Txt_Out_Val_DG_Game.Visibility = Visibility.Hidden;

            // display
            _040_Txt_Out_AvgPosition.Visibility = Visibility.Hidden;
            _040_Txt_Out_Games.Visibility = Visibility.Hidden;
            _040_Txt_Out_LegWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Loss.Visibility = Visibility.Hidden;
            _040_Txt_Out_Points.Visibility = Visibility.Hidden;
            _040_Txt_Out_SetsWin.Visibility = Visibility.Hidden;
            _040_Txt_Out_Throws.Visibility = Visibility.Hidden;
            _040_Txt_Out_Win.Visibility = Visibility.Hidden;
            _040_Txt_Out_AvgPoints.Visibility = Visibility.Hidden;

            // export
            _040_Btn_Export_Player.Visibility = Visibility.Hidden;
            _040_Btn_Export_Game.Visibility = Visibility.Hidden;

            // datagrid
            _040_Btn_DG_GameStatistic.Visibility = Visibility.Hidden;
            _040_Btn_DG_Legs.Visibility = Visibility.Hidden;
            _040_Btn_DG_Player.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Player.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Legs.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic.Visibility = Visibility.Hidden;

            // listbox
            if (_040_ListBox.Items.Count > 0)
                _040_ListBox.SelectedIndex = 0;
            #endregion
        }
        
        private void SetDataGridPlayer(DatahandlingGame dhGame)
        {
            List<Statistic.MainPlayerStruct> listMainPl =
                new List<Statistic.MainPlayerStruct>();
            for (int i = 0; i < dhGame.GameStat.Main.MainPlayer.Length; i++)
            {
                DateTime dt;
                DateTime.TryParse(dhGame.GameStat.Main.MainPlayer[i].BirthYear.ToString()
                    + dhGame.GameStat.Main.MainPlayer[i].BirthMonth.ToString()
                    + dhGame.GameStat.Main.MainPlayer[i].BirthDay, out dt);
                if (dt.Ticks == 0)
                {
                    dt.AddYears(1999);
                    dt.AddMonths(1);
                    dt.AddDays(1);
                }
                listMainPl.Add(new Statistic.MainPlayerStruct
                {
                    Index = dhGame.GameStat.Main.MainPlayer[i].Index,
                    PlayerName = dhGame.GameStat.Main.MainPlayer[i].PlayerName.TrimEnd(),
                    FirstName = dhGame.GameStat.Main.MainPlayer[i].FirstName.TrimEnd(),
                    LastName = dhGame.GameStat.Main.MainPlayer[i].LastName.TrimEnd(),
                    Country = dhGame.GameStat.Main.MainPlayer[i].Country.TrimEnd(),
                    Birthday = dt.ToLocalTime()
                });
            }
            _040_DG_GameStatistic.ItemsSource = null;
            _040_DG_GameStatistic_Legs.ItemsSource = null;
            _040_DG_GameStatistic_Player.ItemsSource = listMainPl;
        }
        private void SetDataGridLeg(DatahandlingGame dhGame)
        {
            List<Statistic.MainLegsStruct> listMainLeg =
                new List<Statistic.MainLegsStruct>();
            for (int i = 0; i < dhGame.GameStat.Main.MainLeg.Length; i++)
            {
                DateTime dt = new DateTime(dhGame.GameStat.Main.MainLeg[i].DateTime);

                listMainLeg.Add(new Statistic.MainLegsStruct
                {
                    CountPlayerIndex = dhGame.GameStat.Main.MainLeg[i].CountPlayerIndex,
                    PlayerName = dhGame.GameStat.Main.MainLeg[i].PlayerName.TrimEnd(),
                    DateTime = dt.ToLocalTime(),
                    CountFinishScore = dhGame.GameStat.Main.MainLeg[i].CountFinishScore,
                    CountLegWinnerThrows = dhGame.GameStat.Main.MainLeg[i].CountLegWinnerThrows
                });
            }

            _040_DG_GameStatistic.ItemsSource = null;
            _040_DG_GameStatistic_Player.ItemsSource = null;
            _040_DG_GameStatistic_Legs.ItemsSource = listMainLeg;
        }
        private void SetDataGridGameStat(DatahandlingGame dhGame)
        {
            List<Statistic.MainGameStatisticStruct> listMainGameStat =
                new List<Statistic.MainGameStatisticStruct>();
            for (int i = 0; i < dhGame.GameStat.Main.MainGameStatistic.Length; i++)
            {
                DateTime dt = new DateTime(dhGame.GameStat.Main.MainGameStatistic[i].CountDateTime);

                listMainGameStat.Add(new Statistic.MainGameStatisticStruct
                {
                    CountIndexThrow = dhGame.GameStat.Main.MainGameStatistic[i].CountIndexThrow,
                    CountIndexPlayer = dhGame.GameStat.Main.MainGameStatistic[i].CountIndexPlayer,
                    PlayerName = dhGame.GameStat.Main.MainGameStatistic[i].PlayerName.TrimEnd(),
                    CountRounds = dhGame.GameStat.Main.MainGameStatistic[i].CountRounds,
                    CountScore = dhGame.GameStat.Main.MainGameStatistic[i].CountScore,
                    IsScoreOverthrown = dhGame.GameStat.Main.MainGameStatistic[i].IsScoreOverthrown,
                    CountFinish = dhGame.GameStat.Main.MainGameStatistic[i].CountFinish,
                    IsFinishPossible = dhGame.GameStat.Main.MainGameStatistic[i].IsFinishPossible,
                    CountDateTime = dt.ToLocalTime(),
                });
            }

            _040_DG_GameStatistic_Player.ItemsSource = null;
            _040_DG_GameStatistic_Legs.ItemsSource = null;
            _040_DG_GameStatistic.ItemsSource = listMainGameStat;
        }
        #endregion
        #region navigation header
        private void _040_Btn_Menu_NewGame_Click(object sender, RoutedEventArgs e)
        {
            var _020_Game_NewGame = new _02X_Game._020_Game_NewGame();
            _020_Game_NewGame.Show();
            this.Close();
        }
        private void _040_Btn_Menu_Home_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
        private void _040_Btn_Menu_Player_Click(object sender, RoutedEventArgs e)
        {
            var _060_Game_NewGame = new _06X_Player._060_Player_Main();
            _060_Game_NewGame.Show();
            this.Close();
        }
        private void _040_Btn_Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, 9999, "Exit the game", "Do you really want to exit?");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler;
            prompt.Show();
            prompt.Dispose();
        }
        #endregion
        #region user interactions
        private void _040_ListBox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_040_ListBox.Items.Count > 0)
            {
                if (ActivePlayerStat)
                {
                    #region input
                    DatahandlingPlayerStat.MainStruct mainStruct = new DatahandlingPlayerStat.MainStruct();
                    int startIndex = _040_ListBox.SelectedItem.ToString().IndexOf(" ") + 1;
                    string plName = _040_ListBox.SelectedItem.ToString().Substring(startIndex).TrimEnd();
                    int plIndex = DatahandlingPlayer.GetIndexFromPlayerName(plName);
                    #endregion
                    #region sequence
                    DatahandlingPlayerStat dhPlayerStat = new DatahandlingPlayerStat(Configuration.FilePathDDDS3D);
                    mainStruct = dhPlayerStat.GetPlayerStatistic(plIndex);
                    dhPlayerStat.Dispose();
                    MainPlayerStat = mainStruct;
                    #endregion
                    #region output
                    if (mainStruct.PlayerName != null)
                    {
                        _040_Txt_Out_Val_Player.Text = mainStruct.PlayerName.TrimEnd();
                        _040_Txt_Out_Val_AvgPosition.Text = mainStruct.TotalAvgPosition.ToString();
                        if (mainStruct.TotalPoints != 0)
                            _040_Txt_Out_Val_AvgPoints.Text = 
                                (mainStruct.TotalPoints / mainStruct.TotalThrows).ToString();
                        else
                            _040_Txt_Out_Val_AvgPoints.Text = "0";
                        _040_Txt_Out_Val_Games.Text = mainStruct.TotalGames.ToString();
                        _040_Txt_Out_Val_LegsWin.Text = mainStruct.TotalLegsWin.ToString();
                        _040_Txt_Out_Val_Win.Text = mainStruct.TotalWin.ToString();
                        _040_Txt_Out_Val_Loss.Text = mainStruct.TotalLoss.ToString();
                        _040_Txt_Out_Val_Points.Text = mainStruct.TotalPoints.ToString();
                        _040_Txt_Out_Val_SetsWin.Text = mainStruct.TotalSetsWin.ToString();
                        _040_Txt_Out_Val_Throw.Text = mainStruct.TotalThrows.ToString();
                    }
                    #endregion
                }
                if (ActiveGameStat)
                {
                    #region input
                    var dhGame = new DatahandlingGame(
                        Configuration.FilePathDDDG3D
                        + (string)_040_ListBox.SelectedItem
                        + ".G3D");
                    #endregion
                    #region sequence
                    if (dhGame.Load())
                    {
                        GameStatisticStruct = dhGame;
                        if (Active_DG_Player)
                            SetDataGridPlayer(dhGame);
                        if (Active_DG_Leg)
                            SetDataGridLeg(dhGame);
                        if (Active_DG_GameStatistic)
                            SetDataGridGameStat(dhGame);

                    }
                    else
                    {
                        _G_Prompt prompt = new _G_Prompt(1, 9998, "Error", "Game data load", "");
                        prompt.Show();
                    }
                    #endregion
                    #region output
                    _040_Txt_Out_Val_DG_Game.Text = _040_ListBox.SelectedItem.ToString();
                    #endregion
                }
            }
            CountSelectedIndex = _040_ListBox.SelectedIndex;
        }

        private void _040_Btn_Player_Click(object sender, RoutedEventArgs e)
        {
            SetVisiblePlayer();
        }
        private void _040_Btn_Games_Click(object sender, RoutedEventArgs e)
        {
            SetVisibleGame();
        }
        private void _040_Btn_Graphs_Click(object sender, RoutedEventArgs e)
        {
            SetVisibleGraphics();
        }

        private void _040_Btn_DG_Player_Click(object sender, RoutedEventArgs e)
        {
            #region input

            #endregion
            #region sequence
            Active_DG_Player = true;
            _040_DG_GameStatistic.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Legs.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Player.Visibility = Visibility.Visible;
            #endregion
            #region write
            SetDataGridPlayer(GameStatisticStruct);
            #endregion
        }
        private void _040_Btn_DG_Legs_Click(object sender, RoutedEventArgs e)
        {
            #region input

            #endregion
            #region sequence
            Active_DG_Leg = true;
            _040_DG_GameStatistic.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Legs.Visibility = Visibility.Visible;
            _040_DG_GameStatistic_Player.Visibility = Visibility.Hidden;
            #endregion
            #region write
            SetDataGridLeg(GameStatisticStruct);
            #endregion
        }
        private void _040_Btn_DG_GameStatistic_Click(object sender, RoutedEventArgs e)
        {
            #region input

            #endregion
            #region sequence
            Active_DG_GameStatistic = true;
            _040_DG_GameStatistic.Visibility = Visibility.Visible;
            _040_DG_GameStatistic_Legs.Visibility = Visibility.Hidden;
            _040_DG_GameStatistic_Player.Visibility = Visibility.Hidden;
            #endregion
            #region write
            SetDataGridGameStat(GameStatisticStruct);
            #endregion
        }

        private void _040_Btn_Export_Player_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Title = "Export player data";
            saveFileDialog.Filter = "Export file as (*.csv)|*.csv|Export file as (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = "Export_Player_" + DateTime.Now.ToString("ddMMyyyy_hhMMss") + "_" + _040_ListBox.SelectedItem.ToString();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                #region input
                var sqStatisticSave = new Sequence.Statistic(saveFileDialog.FileName, MainPlayerStat, 1);
                #endregion
                #region sequence
                sqStatisticSave.StatisticSave(1);
                #endregion
                #region output
                #endregion
            }
        }
        private void _040_Btn_Export_Game_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Title = "Export game data";
            saveFileDialog.Filter = "Export file as (*.csv)|*.csv|Export file as (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = "Export_" + _040_ListBox.SelectedItem.ToString();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                #region input
                var sqStatisticSave = new Sequence.Statistic(saveFileDialog.FileName, (string)_040_ListBox.SelectedItem, GameStatisticStruct.GameStat, 2);
                #endregion
                #region sequence
                sqStatisticSave.StatisticSave(1);
                #endregion
                #region output
                #endregion
            }
        }
        #endregion
        #region event prompt
        private void Prompt_ReturnEventHandler(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1 && e.ReturnID == 9999) System.Environment.Exit(0);
        }
        #endregion
    }
}

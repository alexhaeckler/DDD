using DDD;
using DDD.Program;
using DDD_WPF.Screens._Global;
using System;
using System.Timers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Collections.Generic;
using static DDD.DatahandlingGame;
using System.Windows.Controls;
using System.Windows.Data;

namespace DDD_WPF.Screens._02X_Game
{
    /// <summary>
    /// Interaction logic for _030_Game_Overview.xaml
    /// </summary>
    public partial class _030_Game_Overview : Window
    {
        #region constructor
        public _030_Game_Overview()
        {
            InitializeComponent();
            FirstExecute();
        }
        #endregion
        #region delegates
        private delegate void delBotSeq();
        private delegate void delSeq(int stepSeq);
        #endregion
        #region methods
        private void FirstExecute()
        {

            var dhListBox = new DHListBox(_030_ListBox_Player);
            _030_ListBox_Player = dhListBox.SetPlayer(DDD.Game.MainProperties.PlayersStart);
            _030_Txt_Out_Points.Text = Game.MainProperties.Points.ToString();
            _030_Txt_Out_Val_GameMode.Text = Game.MainProperties.GameMode;
            _030_Txt_Out_Val_Points.Text = Game.MainProperties.Points.ToString();
            _030_Txt_Out_Val_Sets.Text = Game.MainProperties.Sets.ToString();
            _030_Txt_Out_Val_Legs.Text = Game.MainProperties.Legs.ToString();
            _030_Txt_Out_Val_BotDifficult.Text = Game.MainProperties.BotDifficultyName;
            Game.MainProperties.CheckGameWin = false;
            Game.MainProperties.CheckFirstThrowExecution = false;
            Sequence.Game.CountEventFirstThrowExecution = 0;
            CheckSelectionChangeProcess = false;
            CheckFirstThrowExecution = false;
            SetDatagrid();
            SetTimer();
            dhListBox.Dispose();

            void SetDatagrid()
            {
                var dgp = new DDD.Game.DataGrid[DDD.Game.MainProperties.PlayersStart.Length];

                // initialize datagrid
                DataGridTextColumn col1 = new DataGridTextColumn();
                DataGridTextColumn col2 = new DataGridTextColumn();
                DataGridTextColumn col3 = new DataGridTextColumn();
                DataGridTextColumn col4 = new DataGridTextColumn();

                _030_DataGrid_Statistic.Columns.Add(col1);
                _030_DataGrid_Statistic.Columns.Add(col2);
                _030_DataGrid_Statistic.Columns.Add(col3);
                _030_DataGrid_Statistic.Columns.Add(col4);

                col1.Binding = new Binding("dgPlayerName");
                col2.Binding = new Binding("dgFinish");
                col3.Binding = new Binding("dgAvg");
                col4.Binding = new Binding("dgLastScore");

                col1.Header = "Player name";
                col2.Header = "Finish";
                col3.Header = "Average";
                col4.Header = "Last score";

                for (int i = 0; i < DDD.Game.MainProperties.PlayersStart.Length; i++)
                {
                    dgp[i].dgAvg = 0;
                    dgp[i].dgFinish = DDD.Game.MainProperties.Points;
                    dgp[i].dgLastScore = 0;
                    dgp[i].dgPlayerName = DDD.Game.MainProperties.PlayersStart[i].PlayerName;

                    _030_DataGrid_Statistic.Items.Add(dgp[i]);
                }
            }
        }
        private void BotSequence()
        {
            #region variable decleration
            var dhListBox = new DHListBox();
            var sqGame = new Sequence.Game(_030_ListBox_Player);
            #endregion
            #region input
            Int32.TryParse(_030_Txt_Out_Points.Text, out Game.GameProperties.InputScoreBot);
            #endregion
            #region sequence
            sqGame.EventHandlerGameEnd += SqGame_EventHandlerGameEnd;
            if (!CheckFirstThrowExecution)
                sqGame.EventHandlerFirstThrowExecution += SqGame_EventHandlerFirstThrowExecution;
            
            sqGame.GameSetThrow(1);
            if (Sequence.Game.CountEventFirstThrowExecution > 0)
                sqGame.EventHandlerFirstThrowExecution -= SqGame_EventHandlerFirstThrowExecution;
            sqGame.Dispose();
            #endregion
            #region output
            if (!DDD.Game.MainProperties.CheckGameWin)
            {
                _030_Txt_In_Points.Text = Game.DisplayProperties.ActPlayer.Score.ToString();
                _030_Txt_Out_Points.Text = Game.DisplayProperties.ActPlayer.Finish.ToString();

                _030_Txt_Out_Val_Sets.Text = Game.DisplayProperties.HighScore.ActSet;
                _030_Txt_Out_Val_Legs.Text = Game.DisplayProperties.HighScore.HighestLegs;

                _030_ListBox_Player.SelectedIndex = Game.DisplayProperties.ListBoxActPlayer;
            }
            var _032_Game_Bot_Board = new _032_Game_Bot_Board();
            _032_Game_Bot_Board.Show();
            #endregion
        }
        private void SetTimer()
        {
            timer = new Timer(CountTimerMS);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = false;
            timer.Enabled = false;
        }
        /// <summary>
        /// Get int random number
        /// </summary>
        /// <param name="Min">minimum value</param>
        /// <param name="Max">maximum value</param>
        /// <returns>Integer random number</returns>
        public static int RandomInt(int Min, int Max)
        {
            if (Max < Min)
                return 0;
            return Random.Next(Min, Max + 1);
        }
        #endregion
        #region fields
        private static bool CheckSelectionChangeProcess = false;
        public static bool CheckFirstThrowExecution = false;
        private int CountResetThrowExecution = 0;
        private static readonly Random Random = new Random();
        private const int CountTimerMS = 1500;
        #endregion
        #region timer
        private static System.Timers.Timer timer;
        #endregion
        #region navigation header
        private void _030_Btn_Menu_Home_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, 9998, "End game", "Do you really want to end the game?");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler;
            prompt.Show();
            prompt.Dispose();
        }
        private void _030_Btn_Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, 9999, "Exit the game", "Do you really want to exit?");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler;
            prompt.Show();
            prompt.Dispose();
        }
        #endregion
        #region user interaction
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            CountResetThrowExecution = 0;
            timer.Enabled = false;
        }
        private void _030_ListBox_Player_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!CheckSelectionChangeProcess && !CheckFirstThrowExecution)
            {
                CheckSelectionChangeProcess = true;
                var dhListBox = new DHListBox();
                _030_ListBox_Player = dhListBox.ChangePlayer(_030_ListBox_Player);
                CheckSelectionChangeProcess = false;
            }
        }
        private void _030_Txt_In_Points_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                #region variable decleration
                var dhListBox = new DHListBox();
                var sqGame = new Sequence.Game(_030_ListBox_Player);
                #endregion
                #region input
                if (_030_Txt_In_Points.Text == "")
                    _030_Txt_In_Points.Text = "0";
                Int32.TryParse(_030_Txt_In_Points.Text, out Game.GameProperties.InputScore);
                #endregion
                #region sequence


                sqGame.EventHandlerGameEnd += SqGame_EventHandlerGameEnd;
                if (!CheckFirstThrowExecution)
                    sqGame.EventHandlerFirstThrowExecution += SqGame_EventHandlerFirstThrowExecution;
                delSeq hdlSeq = new delSeq(sqGame.GameSetThrow);
                Dispatcher.Invoke(hdlSeq,1);
                if (Sequence.Game.CountEventFirstThrowExecution > 0)
                    sqGame.EventHandlerFirstThrowExecution -= SqGame_EventHandlerFirstThrowExecution;
                sqGame.Dispose();
                #endregion
                #region output
                if (!DDD.Game.MainProperties.CheckGameWin)
                {
                    _030_Txt_In_Points.Text = Game.DisplayProperties.ActPlayer.Score.ToString();
                    _030_Txt_Out_Points.Text = Game.DisplayProperties.ActPlayer.Finish.ToString();

                    SetDataGrid();

                    _030_Txt_Out_Val_Sets.Text = Game.DisplayProperties.HighScore.ActSet;
                    _030_Txt_Out_Val_Legs.Text = Game.DisplayProperties.HighScore.HighestLegs;

                    _030_ListBox_Player.SelectedIndex = Game.DisplayProperties.ListBoxActPlayer;
                }
                if (!CheckFirstThrowExecution && DDD.Game.MainProperties.BotCheck)
                    if (DatahandlingPlayer.GetIndexFromPlayerName(_030_ListBox_Player.Items[0].ToString()) == 0)
                    {
                        delBotSeq hdlBotSeq = new delBotSeq(BotSequence);
                        Dispatcher.Invoke(hdlBotSeq);
                        return;
                    }
                if (CheckFirstThrowExecution && DDD.Game.MainProperties.BotCheck)
                    if (DatahandlingPlayer.GetIndexFromPlayerName(_030_ListBox_Player.SelectedItem.ToString()) == 0)
                    {
                        delBotSeq hdlBotSeq = new delBotSeq(BotSequence);
                        Dispatcher.Invoke(hdlBotSeq);
                        return;
                    }
                #endregion
            }
            if (e.Key == Key.Back)
            {
                if (CountResetThrowExecution == 1)
                {
                    #region variable decleration
                    var dhListBox = new DHListBox();
                    var sqGame = new Sequence.Game(_030_ListBox_Player);
                    #endregion
                    #region input

                    #endregion
                    #region sequence
                    sqGame.GameResetThrow(1);
                    sqGame.Dispose();
                    #endregion
                    #region output
                    _030_Txt_In_Points.Text = Game.DisplayProperties.ActPlayer.Score.ToString();
                    _030_Txt_Out_Points.Text = Game.DisplayProperties.ActPlayer.Finish.ToString();

                    //_030_Txt_Out_Val_NextPlayer.Text = Game.DisplayProperties.NextPlayer.Name;
                    //_030_Txt_Out_Val_Score.Text = Game.DisplayProperties.NextPlayer.Score.ToString();
                    //_030_Txt_Out_Val_ToThrow.Text = Game.DisplayProperties.NextPlayer.Finish.ToString();

                    _030_Txt_Out_Val_Sets.Text = Game.DisplayProperties.HighScore.ActSet;
                    _030_Txt_Out_Val_Legs.Text = Game.DisplayProperties.HighScore.HighestLegs;

                    _030_ListBox_Player.SelectedIndex = Game.DisplayProperties.ListBoxActPlayer;

                    CountResetThrowExecution = 0;
                    #endregion
                }
                else
                {
                    if (_030_Txt_In_Points.Text.Length == 0)
                    {
                        timer.Enabled = true;
                        CountResetThrowExecution++;
                    }
                }
            }
        }
        private void SqGame_EventHandlerFirstThrowExecution(object sender, Sequence.Game.EventArgFirstThrowExecution e)
        {
            CheckFirstThrowExecution = true;
        }

        private void _030_Txt_In_Points_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int CountInput;
            string Sender;

            Sender = sender.ToString();
            if (Sender.Length > 31)
                Sender = Sender.Remove(0, 33);

            Int32.TryParse(Sender, out CountInput);
            if (CountInput > 180)
            {
                _030_Txt_In_Points.Text = "180";
                _030_Txt_In_Points.Select(3, 0);
            }
        }
        private void _030_Txt_In_Points_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, "[^0-9]+")) e.Handled = true;
        }
        #endregion
        #region event prompt
        private void Prompt_ReturnEventHandler(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1 && e.ReturnID == 9998)
            {
                var MainWindow = new MainWindow();
                MainWindow.Show();
                this.Close();
            }

            if (e.ReturnValue == 1 && e.ReturnID == 9999) System.Environment.Exit(0);
        }
        #endregion
        #region user background interaction
        private void SqGame_EventHandlerGameEnd(object sender, Sequence.Game.EventArgGameEnd e)
        {
            #region variable decleration
            var sqGameSave = new Sequence.Game();
            #endregion
            #region sequence
            sqGameSave.GameSave(1);
            sqGameSave.Dispose();
            #endregion
            #region output
            var _035_Game_End = new _035_Game_End();
            _035_Game_End.Show();
            this.Close();
            #endregion
        }

        private void _030_ListBox_Player_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CheckFirstThrowExecution)
                _030_Txt_In_Points.Focus();
        }
        private void _030_Txt_Out_Points_GotFocus(object sender, RoutedEventArgs e)
        {
            _030_Txt_In_Points.Focus();
        }
        private void _030_Txt_Out_NextPlayer_GotFocus(object sender, RoutedEventArgs e)
        {
            _030_Txt_In_Points.Focus();
        }
        private void _030_Txt_Out_Score_GotFocus(object sender, RoutedEventArgs e)
        {
            _030_Txt_In_Points.Focus();
        }
        private void _030_Txt_Out_Finish_GotFocus(object sender, RoutedEventArgs e)
        {
            _030_Txt_In_Points.Focus();
        }
        private void _030_Txt_Out_Val_NextPlayer_GotFocus(object sender, RoutedEventArgs e)
        {
            _030_Txt_In_Points.Focus();
        }
        private void _030_Txt_Out_Val_Score_GotFocus(object sender, RoutedEventArgs e)
        {
            _030_Txt_In_Points.Focus();
        }
        private void _030_Txt_Out_Val_ToThrow_GotFocus(object sender, RoutedEventArgs e)
        {
            _030_Txt_In_Points.Focus();
        }
        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CheckFirstThrowExecution)
                _030_Txt_In_Points.Focus();
        }
        #endregion
        #region data grid statistic
        void SetDataGrid()
        {
            var dgp = new DDD.Game.DataGrid[DDD.Game.MainProperties.PlayersStart.Length];

            _030_DataGrid_Statistic.Items.Clear();

            for (int i = 0; i < DDD.Game.MainProperties.PlayersStart.Length; i++)
            {
                dgp[i].dgAvg = DDD.Game.DataGridProp[i].dgAvg;
                dgp[i].dgFinish = DDD.Game.DataGridProp[i].dgFinish;
                dgp[i].dgLastScore = DDD.Game.DataGridProp[i].dgLastScore;
                dgp[i].dgPlayerName = DDD.Game.DataGridProp[i].dgPlayerName;

                _030_DataGrid_Statistic.Items.Add(dgp[i]);
            }
            _030_DataGrid_Statistic.SelectedIndex = Game.DisplayProperties.ListBoxActPlayer;
        }
        #endregion
    }
}

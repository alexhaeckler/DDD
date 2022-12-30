using DDD;
using DDD_WPF.Screens._04X_Statistics;
using DDD_WPF.Screens._Global;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DDD_WPF.Screens._02X_Game
{
    /// <summary>
    /// Interaction logic for _035_Game_End.xaml
    /// </summary>
    public partial class _035_Game_End : Window,IDisposable
    {
        #region constructor
        public _035_Game_End()
        {
            InitializeComponent();
            FirstExecute();
        }
        #endregion
        #region properties

        #endregion
        #region methods
        private void FirstExecute()
        {
            var dhListBox = new DHListBox(_035_ListBox_Ranking);
            _035_ListBox_Ranking = dhListBox.EndGameRanking(EndRanking);
            dhListBox.Dispose();
            if (_035_ListBox_Ranking.Items.Count > 0)
                _035_ListBox_Ranking.SelectedIndex = 0;
        }
        #endregion
        #region static properties
        public static List<Game.GameProp.PlayerEndRankingStruct> EndRanking;
        #endregion
        #region navigation header
        private void _035_Btn_Menu_Home_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            this.Dispose();
            this.Close();
            MainWindow.Show();
        }
        private void _035_Btn_Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, 9999, "Exit the game", "Do you really want to exit?");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler;
            prompt.Show();
            prompt.Dispose();
        }
        private void _035_Btn_Statistic_Click(object sender, RoutedEventArgs e)
        {
            var _040_Statistic_Main = new _040_Statistic_Main();
            _040_Statistic_Main.Show();
            this.Close();
        }
        private void _035_Btn_Menu_Player_Click(object sender, RoutedEventArgs e)
        {
            var _060_Game_Player = new _06X_Player._060_Player_Main();
            _060_Game_Player.Show();
            this.Close();
        }
        #endregion
        #region user interactions
        private void _035_Btn_NewGame_Click_1(object sender, RoutedEventArgs e)
        {
            var _020_Game_NewGame = new _020_Game_NewGame();
            _020_Game_NewGame.Show();
            this.Dispose();
            this.Close();
        }
        private void _035_Btn_Continued_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void _035_Btn_RestartGame_Click(object sender, RoutedEventArgs e)
        {
            var _030_Game_Overview = new _030_Game_Overview();
            this.Dispose();
            this.Close();
            _030_Game_Overview.Show();
        }
        private void _035_ListBox_Ranking_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            #region input
            var idx = _035_ListBox_Ranking.SelectedIndex;
            DatahandlingPlayerStat.MainStruct mainStruct = new DatahandlingPlayerStat.MainStruct();
            int startIndex = _035_ListBox_Ranking.SelectedItem.ToString().IndexOf(" ") + 1;
            string plName = _035_ListBox_Ranking.SelectedItem.ToString().Substring(startIndex).TrimEnd();
            int plIndex = DatahandlingPlayer.GetIndexFromPlayerName(plName);
            #endregion
            #region sequence
            DatahandlingPlayerStat dhPlayerStat = new DatahandlingPlayerStat(Configuration.FilePathDDDS3D);
            mainStruct = dhPlayerStat.GetPlayerStatistic(plIndex);
            dhPlayerStat.Dispose();
            #endregion
            #region output
            _035_Txt_Out_Val_WinLoss.Text = EndRanking[idx].WinLoss.ToString();
            _035_Txt_Out_Val_Avg.Text = EndRanking[idx].Avg.ToString();
            _035_Txt_Out_Val_Throw.Text = EndRanking[idx].Throws.ToString();
            _035_Txt_Out_Val_HighestScore.Text = EndRanking[idx].HighestPoints.ToString();
            _035_Txt_Out_Val_RankingAvg.Text = mainStruct.TotalAvgPosition.ToString();
            #endregion
        }
        #endregion
        #region event prompt
        private void Prompt_ReturnEventHandler(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1 && e.ReturnID == 9999) System.Environment.Exit(0);
        }
        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                EndRanking = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~_035_Game_End()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}

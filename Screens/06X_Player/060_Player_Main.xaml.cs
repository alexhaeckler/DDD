using DDD_WPF.Screens._Global;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DDD;
using System;
using DDD.Program;
using System.Windows.Controls;

namespace DDD_WPF.Screens._06X_Player
{
    /// <summary>
    /// Interaction logic for _060_Player_Main.xaml
    /// </summary>
    public partial class _060_Player_Main : Window
    {
        #region constructor
        public _060_Player_Main()
        {
            InitializeComponent();
        }
        #endregion
        #region fields
        private static bool CheckTxtDisabled;
        private static bool CheckListBoxDeselect;
        private int CountListBoxSelection;
        #endregion
        #region private methods
        private void SetColorsDisabled(bool disabled)
        {
            if (disabled)
            {
                _060_Txt_Inp_BirthdayYear.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(160, 160, 160));
                _060_Txt_Inp_BirthdayMonth.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(160, 160, 160));
                _060_Txt_Inp_BirthdayDay.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(160, 160, 160));

                _060_Txt_Inp_PlayerName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(160, 160, 160));
                _060_Txt_Inp_Firstname.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(160, 160, 160));
                _060_Txt_Inp_Lastname.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(160, 160, 160));
                _060_Txt_Inp_Country.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(160, 160, 160));
            }
            else
            {
                _060_Txt_Inp_BirthdayYear.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
                _060_Txt_Inp_BirthdayMonth.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
                _060_Txt_Inp_BirthdayDay.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));

                _060_Txt_Inp_PlayerName.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
                _060_Txt_Inp_Firstname.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
                _060_Txt_Inp_Lastname.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
                _060_Txt_Inp_Country.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
            }
        }
        #endregion
        #region navigation header
        private void _060_Btn_Menu_NewGame_Click(object sender, RoutedEventArgs e)
        {
            var _020_Game_NewGame = new _02X_Game._020_Game_NewGame();
            _020_Game_NewGame.Show();
            this.Close();
        }
        private void _060_Btn_Menu_Home_Click(object sender, RoutedEventArgs e)
        {
            var MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
        private void _060_Btn_Menu_Statistic_Click(object sender, RoutedEventArgs e)
        {
            var _040_Game_NewGame = new _04X_Statistics._040_Statistic_Main();
            _040_Game_NewGame.Show();
            this.Close();
        }
        private void _060_Btn_Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, 9999, "Exit the game", "Do you really want to exit?");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler;
            prompt.Show();
            prompt.Dispose();
        }
        #endregion
        #region user interactions
        private void _060_Btn_NewPlayer_Click(object sender, RoutedEventArgs e)
        {
            #region variable decleration
            DatahandlingPlayer.Main dhMain = new DatahandlingPlayer.Main();
            #endregion
            #region input
            Int32.TryParse(_060_Txt_Inp_BirthdayYear.Text,out dhMain.BirthYear);
            Int32.TryParse(_060_Txt_Inp_BirthdayMonth.Text, out dhMain.BirthMonth);
            Int32.TryParse(_060_Txt_Inp_BirthdayDay.Text, out dhMain.BirthDay);

            dhMain.PlayerName = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_PlayerName.Text);
            dhMain.FirstName = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_Firstname.Text);
            dhMain.LastName = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_Lastname.Text);
            dhMain.Country = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_Country.Text);
            #endregion
            #region sequence
            Sequence.Player sqPlayer = new Sequence.Player(dhMain);
            sqPlayer.EventHandlerNewPlayer += SqPlayer_EventHandlerNewPlayer;
            sqPlayer.NewPlayer(1);
            sqPlayer.Dispose();
            #endregion
            #region output
            _060_Txt_Inp_PlayerName.Text = "";
            _060_Txt_Inp_Firstname.Text = "";
            _060_Txt_Inp_Lastname.Text = "";
            _060_Txt_Inp_Country.Text = "";
            _060_Txt_Inp_BirthdayYear.Text = "";
            _060_Txt_Inp_BirthdayMonth.Text = "";
            _060_Txt_Inp_BirthdayDay.Text = "";
            #endregion
        }
        private void SqPlayer_EventHandlerNewPlayer(object sender, Sequence.Player.EventArgNewPlayer e)
        {
            _060_ListBox_Player = new DHListBox(_060_ListBox_Player).SetPlayer();
        }

        private void _060_Btn_DeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, "Delete player", "Do you really want to delete?", "");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler1;
            prompt.Show();
            prompt.Dispose();
        }
        private void Prompt_ReturnEventHandler1(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1)
            {
                DatahandlingPlayer.Main dhMain = new DatahandlingPlayer.Main();
                #region input
                dhMain.Index = DatahandlingPlayer.GetIndexFromPlayerName((string)_060_ListBox_Player.SelectedItem);
                #endregion
                #region sequence
                Sequence.Player sqPlayer = new Sequence.Player(dhMain);
                sqPlayer.EventHandlerDeletePlayer += SqPlayer_EventHandlerDeletePlayer;
                sqPlayer.DeletePlayer(1);
                sqPlayer.Dispose();
                #endregion
            }
        }
        private void SqPlayer_EventHandlerDeletePlayer(object sender, Sequence.Player.EventArgDeletePlayer e)
        {
            _060_ListBox_Player = new DHListBox(_060_ListBox_Player).SetPlayer();
        }

        private void _060_ListBox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CheckListBoxDeselect)
                CountListBoxSelection++;
            if (CountListBoxSelection == 3)
                CheckListBoxDeselect = true;
            if (CountListBoxSelection == 2)
            {
                _060_ListBox_Player.UnselectAll();
                CheckListBoxDeselect = false;
                if(e.AddedItems.Count != 0)
                    _060_ListBox_Player.SelectedItem =  e.AddedItems[0];
            }
            if (_060_ListBox_Player.SelectedIndex != -1 && CheckListBoxDeselect == false)
            {
                DatahandlingPlayer.Main dhMain = new DatahandlingPlayer.Main();
                #region input
                dhMain.Index = DatahandlingPlayer.GetIndexFromPlayerName((string)_060_ListBox_Player.SelectedItem);
                #endregion
                #region sequence
                Sequence.Player sqPlayer = new Sequence.Player(dhMain);
                sqPlayer.SelectPlayer(1);
                #endregion
                #region output
                _060_Txt_Inp_BirthdayYear.Text = sqPlayer.DhMain.BirthYear.ToString();
                _060_Txt_Inp_BirthdayMonth.Text = sqPlayer.DhMain.BirthMonth.ToString();
                _060_Txt_Inp_BirthdayDay.Text = sqPlayer.DhMain.BirthDay.ToString();

                _060_Txt_Inp_PlayerName.Text = sqPlayer.DhMain.PlayerName.TrimEnd();
                _060_Txt_Inp_Firstname.Text = sqPlayer.DhMain.FirstName.TrimEnd();
                _060_Txt_Inp_Lastname.Text = sqPlayer.DhMain.LastName.TrimEnd();
                _060_Txt_Inp_Country.Text = sqPlayer.DhMain.Country.TrimEnd();

                SetColorsDisabled(true);
                CheckTxtDisabled = false;

                sqPlayer.Dispose();
                #endregion
            }
            else CountListBoxSelection = 0;
            // Reset input
            if (e.AddedItems.Count == 0)
            {
                _060_Txt_Inp_PlayerName.Text = "";
                _060_Txt_Inp_Firstname.Text = "";
                _060_Txt_Inp_Lastname.Text = "";
                _060_Txt_Inp_Country.Text = "";

                _060_Txt_Inp_BirthdayDay.Text = "";
                _060_Txt_Inp_BirthdayMonth.Text = "";
                _060_Txt_Inp_BirthdayYear.Text = "";
            }
        }

        private void _060_Btn_EditPlayer_Click(object sender, RoutedEventArgs e)
        {
            var prompt = new _G_Prompt(2, "Edit player", "Do you want to edit?", "");
            prompt.ReturnEventHandler += Prompt_ReturnEventHandler2;
            prompt.Show();
            prompt.Dispose();
        }
        private void Prompt_ReturnEventHandler2(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1)
            {
                DatahandlingPlayer.Main dhMain = new DatahandlingPlayer.Main();
                #region input
                Int32.TryParse(_060_Txt_Inp_BirthdayYear.Text, out dhMain.BirthYear);
                Int32.TryParse(_060_Txt_Inp_BirthdayMonth.Text, out dhMain.BirthMonth);
                Int32.TryParse(_060_Txt_Inp_BirthdayDay.Text, out dhMain.BirthDay);

                dhMain.PlayerName = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_PlayerName.Text);
                dhMain.FirstName = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_Firstname.Text);
                dhMain.LastName = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_Lastname.Text);
                dhMain.Country = DatahandlingPlayer.SetStringTo32Byte(_060_Txt_Inp_Country.Text);
                dhMain.Index = DatahandlingPlayer.GetIndexFromPlayerName((string)_060_ListBox_Player.SelectedValue);
                #endregion
                #region sequence
                Sequence.Player sqPlayer = new Sequence.Player(dhMain);
                sqPlayer.EventHandlerEditPlayer += SqPlayer_EventHandlerEditPlayer;
                sqPlayer.EditPlayer(1);
                sqPlayer.Dispose();
                #endregion
            }
        }
        private void SqPlayer_EventHandlerEditPlayer(object sender, Sequence.Player.EventArgEditPlayer e)
        {
            _060_ListBox_Player = new DHListBox(_060_ListBox_Player).SetPlayer();
        }
        #endregion
        #region event prompt
        private void Prompt_ReturnEventHandler(object sender, _G_Prompt.ReturnEventArgs e)
        {
            if (e.ReturnValue == 1 && e.ReturnID == 9999) System.Environment.Exit(0);
        }
        #endregion
        #region user background interactions
        private void Window_Initialized(object sender, EventArgs e)
        {
            var setPlayer = new DHListBox(_060_ListBox_Player);
            _060_ListBox_Player = setPlayer.SetPlayer();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void _060_Txt_Inp_PlayerName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SetText(1,e.Text, _060_Txt_Inp_PlayerName.Text.Length);
            e.Handled = true;
        }
        private void _060_Txt_Inp_Firstname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SetText(2, e.Text, _060_Txt_Inp_Firstname.Text.Length);
            e.Handled = true;
        }
        private void _060_Txt_Inp_Lastname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SetText(3, e.Text, _060_Txt_Inp_Lastname.Text.Length);
            e.Handled = true;
        }
        private void _060_Txt_Inp_Country_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SetText(4, e.Text, _060_Txt_Inp_Country.Text.Length);
            e.Handled = true;
        }

        private void SetText(int Action, string Text, int SelectionStart)   
        {
            if (!CheckTxtDisabled)
            {
                SetColorsDisabled(CheckTxtDisabled);
                CheckTxtDisabled = true;
            }
            // 1 = _060_Txt_Inp_PlayerName // 2 = _060_Txt_Inp_Firstname 
            // 3 = _060_Txt_Inp_Lastname   // 4 = _060_Txt_Inp_Country 
            var Umlaute = new Dictionary<char, string>() {
                  { 'ä', "ae" },
                  { 'ö', "oe" },
                  { 'ü', "ue" },
                  { 'Ä', "Ae" },
                  { 'Ö', "Oe" },
                  { 'Ü', "Ue" },
                  { 'ß', "ss" }
                };

            var Result = Text.Aggregate(
                          new StringBuilder(),
                          (sb, c) => Umlaute.TryGetValue(c, out var r) ? sb.Append(r) : sb.Append(c)
                          ).ToString();

            SelectionStart = SelectionStart + Result.Length;

            switch (Action)
            {
                case 1:
                    {
                        _060_Txt_Inp_PlayerName.Text = _060_Txt_Inp_PlayerName.Text + Result;
                        _060_Txt_Inp_PlayerName.SelectionStart = SelectionStart;
                    }
                    break;
                case 2:
                    {
                        _060_Txt_Inp_Firstname.Text = _060_Txt_Inp_Firstname.Text + Result;
                        _060_Txt_Inp_Firstname.SelectionStart = SelectionStart;
                    }
                    break;
                case 3:
                    {
                        _060_Txt_Inp_Lastname.Text = _060_Txt_Inp_Lastname.Text + Result;
                        _060_Txt_Inp_Lastname.SelectionStart = SelectionStart;
                    }
                    break;
                case 4:
                    {
                        _060_Txt_Inp_Country.Text = _060_Txt_Inp_Country.Text + Result;
                        _060_Txt_Inp_Country.SelectionStart = SelectionStart;
                    }
                    break;
            }
        }
        #endregion
    }
}

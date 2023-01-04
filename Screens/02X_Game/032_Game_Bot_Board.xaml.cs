#define debug

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

using DDD;

namespace DDD_WPF.Screens._02X_Game
{
    /// <summary>
    /// Interaction logic for _032_Game_Bot_Board.xaml
    /// </summary>
    public partial class _032_Game_Bot_Board : Window
    {
        #region constructor
        public _032_Game_Bot_Board()
        {
            this.IsBotEnabled = true;
            InitializeComponent();
            FirstExecute();
        }
        public _032_Game_Bot_Board(bool IsBotEnabled)
        {
            this.IsBotEnabled = IsBotEnabled;
            InitializeComponent();
            FirstExecute();
        }
        #endregion
        #region delegates
        private delegate void delDrawPoint(DatahandlingWPF.Win32Point win32Point);
        private delegate void delAction();
        //private delegate void delRefreshValues(int countThrow);
        #endregion
        #region static properties, fields, Bindings
        private static int CountTimer = 0, CountThrow = 0;
        private static BoardParameter.ScreenResolution MonitorRes = new BoardParameter.ScreenResolution();
        private DatahandlingWPF.Win32Point Win32Point;
        private DartBot.Scoring Scores;
        private DatahandlingWPF.Win32Point[] ScoreCoordinates = new DatahandlingWPF.Win32Point[3];
        private static int CountMaxThrow = new int();
        private static BoardParameter.ScreenCoordinates sc = new BoardParameter.ScreenCoordinates();

        private bool _IsBotEnabled;
        public bool IsBotEnabled
        {
            get { return _IsBotEnabled; }
            set { _IsBotEnabled = value;}
        }

#if debug
        private static string Debug = "X=%1;Y=%2;Quadrant=%3;Degrees=%4;Length=%5" + Environment.NewLine +
            "RadiusScore=%6" + Environment.NewLine + "DegreeScore=%7" + Environment.NewLine + "Score=%8";
#endif

        #endregion
        #region timer
        private int TimerThrowDelayInMs = 1500;
        private static System.Timers.Timer timer;
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (!IsBotEnabled)
                return;
            bool chkEnd = new bool();
            timer.Enabled = false;
            if (CountTimer >= 1)
            {
                if (CountThrow < CountMaxThrow)
                {
                    Win32Point = ScoreCoordinates[CountThrow];
                    if (CountThrow == 0)
                    {
                        delDrawPoint hndlDrawPoint = new delDrawPoint(GetCoordinates);
                        Dispatcher.Invoke(hndlDrawPoint, Win32Point);
                        Dispatcher.ExitAllFrames();
                    }
                    else if (CountThrow == 1)
                    {
                        delDrawPoint hndlDrawPoint = new delDrawPoint(GetCoordinates);
                        Dispatcher.Invoke(hndlDrawPoint, Win32Point);
                        Dispatcher.ExitAllFrames();
                    }
                    else if (CountThrow == 2)
                    {
                        delDrawPoint hndlDrawPoint = new delDrawPoint(GetCoordinates);
                        Dispatcher.Invoke(hndlDrawPoint, Win32Point);
                        Dispatcher.ExitAllFrames();
                    }
                    CountThrow++;
                }
            }
            if (CountTimer >= 4)
            {
                CountThrow = 0;
                CountMaxThrow = 0;
                chkEnd = true;
                Scores.FinishSingle = null;
                Scores.CountThrows = 0;
            }
            
            if (chkEnd == false)
            {
                CountTimer++;
                timer.Enabled = true;
            }
            else
            {
                delAction hdlAction = new delAction(this.Close);
                Dispatcher.Invoke(hdlAction);
            }
        }
        //private void TimerScale_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    timerScale.Enabled = false;

        //    Line line01 = new Line();
        //    line01.Stroke = System.Windows.Media.Brushes.Gold;
        //    line01.X1 = sc.X - 5 * scale;
        //    line01.X2 = sc.X + 5 * scale;
        //    line01.Y1 = sc.Y * scale;
        //    line01.Y2 = sc.Y * scale;
        //    line01.StrokeThickness = 3 * scale;
        //    Line line02 = new Line();
        //    line02.Stroke = System.Windows.Media.Brushes.Gold;
        //    line02.X1 = sc.X * scale;
        //    line02.X2 = sc.X * scale;
        //    line02.Y1 = sc.Y - 5 * scale;
        //    line02.Y2 = sc.Y + 5 * scale;
        //    line02.StrokeThickness = 3 * scale;

        //    Line line03 = new Line();
        //    line03.Stroke = System.Windows.Media.Brushes.DarkRed;
        //    line03.X1 = sc.X - 2 * scale;
        //    line03.X2 = sc.X + 2 * scale;
        //    line03.Y1 = sc.Y * scale;
        //    line03.Y2 = sc.Y * scale;
        //    line03.StrokeThickness = 2 * scale;
        //    Line line04 = new Line();
        //    line04.Stroke = System.Windows.Media.Brushes.DarkRed;
        //    line04.X1 = sc.X * scale;
        //    line04.X2 = sc.X * scale;
        //    line04.Y1 = sc.Y - 2 * scale;
        //    line04.Y2 = sc.Y + 2 * scale;
        //    line04.StrokeThickness = 2 * scale;

        //    DartBoard_Canvas.Children.Add(line01);
        //    DartBoard_Canvas.Children.Add(line02);
        //    DartBoard_Canvas.Children.Add(line03);
        //    DartBoard_Canvas.Children.Add(line04);

        //    scale -= 1;

        //    if(scale == 1)
        //    {
        //        timerScale.Enabled = false;
        //        scale = 50;
        //    }
        //    else
        //    {
        //        timerScale.Enabled = true;
        //    }
        //}
        #endregion
        #region methods
        private void FirstExecute()
        {
            Scores = DartBot.Scores;
            _032_Txt_Out_Val_ActScore.Text = "0";
            _032_Txt_Out_Val_ActThrow.Text = "0";
            CountMaxThrow = DartBot.Scores.CountThrows;
            CountTimer = 0;
            CountThrow = 0;
            SetTimer();
            if (this.IsBotEnabled)
                GetCoordinates(Scores);
            MonitorRes = GetScreenResolution();

            if (IsBotEnabled)
            {
                _032_Btn_Back.Visibility = Visibility.Hidden;
            }
            else
            {
                _032_Game_Debug.Visibility = Visibility.Hidden;
            }


#if debug
            _032_Game_Debug.Visibility = Visibility.Visible;
#endif
        }
        private void SetTimer()
        {
            timer = new System.Timers.Timer(TimerThrowDelayInMs);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void GetCoordinates(DatahandlingWPF.Win32Point screenCoordinates)
        {
            try
            {
                sc = BoardParameter.GetOrigin(screenCoordinates.X, screenCoordinates.Y);

                DrawPoints();

                RefreshValues(CountThrow);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                throw;
            }
        }
        private void DrawPoints()
        {
                Line line01 = new Line();
                line01.Stroke = System.Windows.Media.Brushes.Gold;
                line01.X1 = sc.X - 5;
                line01.X2 = sc.X + 5;
                line01.Y1 = sc.Y;
                line01.Y2 = sc.Y;
                line01.StrokeThickness = 3;
                Line line02 = new Line();
                line02.Stroke = System.Windows.Media.Brushes.Gold;
                line02.X1 = sc.X;
                line02.X2 = sc.X;
                line02.Y1 = sc.Y - 5;
                line02.Y2 = sc.Y + 5;
                line02.StrokeThickness = 3;

                Line line03 = new Line();
                line03.Stroke = System.Windows.Media.Brushes.DarkRed;
                line03.X1 = sc.X - 2;
                line03.X2 = sc.X + 2;
                line03.Y1 = sc.Y;
                line03.Y2 = sc.Y;
                line03.StrokeThickness = 2;
                Line line04 = new Line();
                line04.Stroke = System.Windows.Media.Brushes.DarkRed;
                line04.X1 = sc.X;
                line04.X2 = sc.X;
                line04.Y1 = sc.Y - 2;
                line04.Y2 = sc.Y + 2;
                line04.StrokeThickness = 2;

                DartBoard_Canvas.Children.Add(line01);
                DartBoard_Canvas.Children.Add(line02);
                DartBoard_Canvas.Children.Add(line03);
                DartBoard_Canvas.Children.Add(line04);
        }
        private BoardParameter.ScreenResolution GetScreenResolution()
        {
            var screenResolution = new BoardParameter.ScreenResolution();
            screenResolution.X = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            screenResolution.Y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            return screenResolution;
        }
        private void RefreshValues(int countThrow)
        {
            int score = 0;
            var scoreAll = DartBot.TransformFinishSingleToArray(DartBot.Scores.FinishSingle);
            if (scoreAll[1, CountThrow] == 4)
                score = score + 25;
            else if (scoreAll[1, CountThrow] == 5)
                score = score + 50;
            else
                for (int i = 0; i <= countThrow; i++)
                    score = scoreAll[0, i] * scoreAll[1, i] + score;

            _032_Txt_Out_Val_ActScore.Text = score.ToString();
            _032_Txt_Out_Val_ActThrow.Text = (CountThrow + 1).ToString();
        }
        private void GetCoordinates(DartBot.Scoring getScore)
        {
            BoardParameter boardParameter = new BoardParameter();
            var score = DartBot.TransformFinishSingleToArray(getScore.FinishSingle);
            for (int i = 0; i < getScore.CountThrows; i++)
                ScoreCoordinates[i] = boardParameter.GetRandomCoordinatesFromScore(score[0,i],(DartBot.ValueMultiplicator)score[1,i]);
        }
        #endregion
        #region User interaction
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var Pointer = DDD.DatahandlingWPF.GetMousePosition();
            var boardParameter = new BoardParameter();
#if debug
            DebugStart();
            _032_Txt_Out_Value.Visibility = Visibility.Hidden;
            _032_Txt_Out_ActScore.Visibility = Visibility.Hidden;
            BoardParameter.ScreenCoordinates screenCoordinates = BoardParameter.OffsetToZero((int)Pointer.X, (int)Pointer.Y, MonitorRes.X, MonitorRes.Y);
            _032_Txt_Out_Value.Text = boardParameter.GetScoreFromCoordinates(screenCoordinates.X, screenCoordinates.Y).ToString();
#endif
        }

        private void _032_Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //        {
        //            var Pointer = DDD.DatahandlingWPF.GetMousePosition();
        //            var boardParameter = new BoardParameter();
        //#if debug
        //            DebugStart();
        //            _032_Txt_Out_Value.Visibility = Visibility.Hidden;
        //            _032_Txt_Out_ActScore.Visibility = Visibility.Hidden;
        //#else
        //                        BoardParameter.ScreenCoordinates screenCoordinates = BoardParameter.OffsetToZero((int)Pointer.X, (int)Pointer.Y, MonitorRes.X, MonitorRes.Y);
        //                        _032_Txt_Out_Value.Text = boardParameter.GetScoreFromCoordinates(screenCoordinates.X, screenCoordinates.Y).ToString();
        //#endif
        //        }
        #endregion
        #region Events

        #endregion
        #region debugging
#if debug
        public void DebugStart()
        {
            #region MouseMove
            var Pointer = DatahandlingWPF.GetMousePosition();
            var boardParameter = new BoardParameter();

            string debug = Debug;
            BoardParameter.ScreenCoordinates screenCoordinates = BoardParameter.OffsetToZero((int)Pointer.X, (int)Pointer.Y, MonitorRes.X, MonitorRes.Y);
            boardParameter.GetScoreFromCoordinates(screenCoordinates.X, screenCoordinates.Y);

            debug = debug.Replace("%1", screenCoordinates.X.ToString("000"));
            debug = debug.Replace("%2", screenCoordinates.Y.ToString("000"));
            debug = debug.Replace("%3", BoardParameter.ActualQuadrant.ToString());
            debug = debug.Replace("%4", BoardParameter.ActualDegree.ToString("000.00"));
            debug = debug.Replace("%5", BoardParameter.ActualLength.ToString("000.00"));
            debug = debug.Replace("%6", BoardParameter.ActualRadiusScore.ToString());
            debug = debug.Replace("%7", BoardParameter.ActualDegreeScore.ToString());
            debug = debug.Replace("%8", BoardParameter.ActualScore.ToString());
            _032_Game_Debug.Text = debug;
            #endregion

            //#region paint dots in dartboard
            //BoardParameter boardParameter2 = new BoardParameter();
            //var coordinates = boardParameter2.GetRandomCoordinatesFromScore(BoardParameter.RandomInt(0,20), (DartBot.ValueMultiplicator)BoardParameter.RandomInt(1,3));
            //BoardParameter.ScreenCoordinates screenCoordinates2 = BoardParameter.GetOrigin(coordinates.X, coordinates.Y);


            //Line line01 = new Line();
            //line01.Stroke = System.Windows.Media.Brushes.Gold;
            //line01.X1 = screenCoordinates2.X - 5;
            //line01.X2 = screenCoordinates2.X + 5;
            //line01.Y1 = screenCoordinates2.Y;
            //line01.Y2 = screenCoordinates2.Y;
            //line01.StrokeThickness = 3;
            //Line line02 = new Line();
            //line02.Stroke = System.Windows.Media.Brushes.Gold;
            //line02.X1 = screenCoordinates2.X;
            //line02.X2 = screenCoordinates2.X;
            //line02.Y1 = screenCoordinates2.Y - 5;
            //line02.Y2 = screenCoordinates2.Y + 5;
            //line02.StrokeThickness = 3;

            //DartBoard_Canvas.Children.Add(line01);
            //DartBoard_Canvas.Children.Add(line02);
            //#endregion

        }
#endif
        #endregion
    }
}

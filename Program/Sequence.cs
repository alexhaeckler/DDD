#define debug

using DDD_WPF.Screens._Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DDD.Program
{
    class Sequence : IDisposable
    {
        #region screens
        class Scr060
        {
            public void NewPlayer()
            {
                #region variable decleration
                DatahandlingPlayer dhpl = new DatahandlingPlayer();
                #endregion
                #region variable decleration

                #endregion
                #region variable decleration

                #endregion
                #region variable decleration

                #endregion
                #region variable decleration

                #endregion





            }
            public void DeletePlayer()
            {

            }
            public void EditPlayer()
            {

            }
        }
        #endregion
        #region programm start
        public class FirstExecute
        {
            /// <summary>
            /// Sequence - first execute - StartUp
            /// </summary>
            /// <param name="stepSequence">Case 1: initialization /
            /// Case 2: sequence - registry /
            /// Case 3: sequence - filehandling /
            /// case 4: sequence - user messages
            /// case 100: configuration
            /// case 200: player data
            /// </param>
            public void StartUp(int stepSequence)
            {
                // Variable decleration
                Configuration cfg = new Configuration();
                // Initialize
                #region checking registry
                // Check registry
                cfg.CheckRegistry(1);
                cfg.CheckRegistry(2);
                cfg.CheckRegistry(3);

                // Check file path
                cfg.CheckFilePath(1);
                cfg.CheckFilePath(3);

                // Check steps
                #region stepSequence step checking
                if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2;
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDVersionExists] == false)
                    stepSequence = 2;
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2;
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegPathDDDRootExists] == false)
                    stepSequence = 2; 
                else if (cfg.Checking[(int)Configuration.CheckingBools.RegDeleteBecauseDataRefresh])
                    stepSequence = 2;
                else 
                    stepSequence = 3;
                if (stepSequence == 3)
                {
                    if (cfg.Checking[20] == false)
                        stepSequence = 3;
                    else if (cfg.Checking[21] == false)
                        stepSequence = 3;
                    else if (cfg.Checking[22] == false)
                        stepSequence = 3;
                    else if (cfg.Checking[23] == false)
                        stepSequence = 3;
                    //else if (cfg.Checking[24])
                    //    stepSequence = 3;
                    else if (cfg.Checking[25])
                        stepSequence = 3;
                    else
                        stepSequence = 4;
                    
                }
                #endregion
                #endregion
                #region sequence registry
                if (stepSequence == 2)
                {
                    cfg.RegistryWrite(cfg.Checking);
                    cfg.CheckRegistry(2);
                    cfg.CheckRegistry(3);
                    if (cfg.Checking[11])
                    {
                        cfg.RegistryDelete();
                        cfg.CheckRegistry(1);
                        cfg.RegistryWrite(cfg.Checking);
                    }
                    stepSequence++;
                }
                if (stepSequence == 3)
                {
                    cfg.FilePathCreate();
                    stepSequence++;
                }
                if (stepSequence == 4)
                {
                    cfg.UserMsgCall();
                    stepSequence++;
                }
                #endregion
                #region configuration player data
                stepSequence = 100;
                if (stepSequence == 100)
                {
                    //Configuration.FirstExecute();
                }
                #endregion
                #region sequence player data
                stepSequence = 200;
                if (stepSequence == 200)
                {
                    //DatahandlingPlayer.FirstExecute();
                }
                #endregion
            }
            public void TestFile()
            {


                //// Normal pointer to an object.
                //int[] a = new int[5] { 10, 20, 30, 40, 50 };
                //// Must be in unsafe code to use interior pointers.
                //unsafe
                //{   
                //    // Must pin object on heap so that it doesn't move while using interior pointers.
                //    fixed (int* p = &a[0])
                //    {
                //        // p is pinned as well as object, so create another pointer to show incrementing it.
                //        int* p2 = p;
                //        Console.WriteLine(*p2);
                //        // Incrementing p2 bumps the pointer by four bytes due to its type ...
                //        p2 += 1;
                //        Console.WriteLine(*p2);
                //        p2 += 1;
                //        Console.WriteLine(*p2);
                //        Console.WriteLine("--------");
                //        Console.WriteLine(*p);
                //        // Dereferencing p and incrementing changes the value of a[0] ...
                //        *p += 1;
                //        Console.WriteLine(*p);
                //        *p += 1;
                //        Console.WriteLine(*p);
                //    }
                //}

                //Console.WriteLine("--------");
                //Console.WriteLine(a[0]);

            }
        }
        #endregion
        #region player
        public class Player : IDisposable
        {
            #region constructor
            public Player() { }
            public Player(DatahandlingPlayer.Main dhMain)
            {
                DhMain = dhMain;
            }
            public Player(DatahandlingPlayer.Main dhMain, int indexPlayer)
            {
                DhMain = dhMain;
                _IndexPlayer = indexPlayer;
            }
            #endregion
            #region event definition: new player
            private EventHandler<EventArgNewPlayer> _EventHandlerNewPlayer;
            public event EventHandler<EventArgNewPlayer> EventHandlerNewPlayer
            {
                add { _EventHandlerNewPlayer += value; }
                remove { _EventHandlerNewPlayer -= value; }
            }
            public void InvokeNewPlayer()
            {
                if (_EventHandlerNewPlayer != null)
                {
                    EventArgNewPlayer args = new EventArgNewPlayer(this.DhMain);
                    _EventHandlerNewPlayer.Invoke(this, args);
                }
            }
            public class EventArgNewPlayer : EventArgs
            {
                public EventArgNewPlayer(DatahandlingPlayer.Main newPlayer)
                {
                    NewPlayer = newPlayer;
                }
                public DatahandlingPlayer.Main NewPlayer;
            }
            #endregion
            #region event definition: delete player
            private EventHandler<EventArgDeletePlayer> _EventHandlerDeletePlayer;
            public event EventHandler<EventArgDeletePlayer> EventHandlerDeletePlayer
            {
                add { _EventHandlerDeletePlayer += value; }
                remove { _EventHandlerDeletePlayer -= value; }
            }
            public void InvokeDeletePlayer()
            {
                if (_EventHandlerDeletePlayer != null)
                {
                    EventArgDeletePlayer args = new EventArgDeletePlayer(this.DhMain);
                    _EventHandlerDeletePlayer.Invoke(this, args);
                }
            }
            public class EventArgDeletePlayer : EventArgs
            {
                public EventArgDeletePlayer(DatahandlingPlayer.Main deletePlayer)
                {
                    DeletePlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D).MainLoad(deletePlayer.Index);
                }
                public DatahandlingPlayer.Main DeletePlayer;
            }
            #endregion
            #region event definition: edit player
            private EventHandler<EventArgEditPlayer> _EventHandlerEditPlayer;
            public event EventHandler<EventArgEditPlayer> EventHandlerEditPlayer
            {
                add { _EventHandlerEditPlayer += value; }
                remove { _EventHandlerEditPlayer -= value; }
            }
            public void InvokeEditPlayer()
            {
                if (_EventHandlerEditPlayer != null)
                {
                    EventArgEditPlayer args = new EventArgEditPlayer(this.DhMain);
                    _EventHandlerEditPlayer.Invoke(this, args);
                }
            }
            public class EventArgEditPlayer : EventArgs
            {
                public EventArgEditPlayer(DatahandlingPlayer.Main editPlayer)
                {
                    EditPlayer = editPlayer;
                }
                public DatahandlingPlayer.Main EditPlayer;
            }
            #endregion
            #region properties
            private DatahandlingPlayer.Main _DhMain { get; set; }
            public DatahandlingPlayer.Main DhMain
            {
                get { return _DhMain; }
                set { _DhMain = value; }
            }

            private int _IndexPlayer { get; set; }
            #endregion
            #region sequences
            public void NewPlayer(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                int playerIndex = new int();
                #endregion
                #region initialize
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    CountPrompt = DatahandlingPlayer.CheckData(1, DhMain);
                    if (CountPrompt == 1) stepSequence++;
                    else stepSequence = 0;
                }
                #endregion
                #region processing
                // Save player
                if (stepSequence == 2)
                {
                    DatahandlingPlayer dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D, DhMain);
                    if (dhPlayer.Save())
                    {
                        playerIndex = dhPlayer.SaveMain.Index;
                        stepSequence++;
                        CountPrompt = 1;
                    }
                    else CountPrompt = 5;
                }
                // Save player statistic
                if (stepSequence == 3)
                {
                    var dhPlayerStat = new DatahandlingPlayerStat(Configuration.FilePathDDDS3D, playerIndex);
                    dhPlayerStat.InitializeNewPlayerDS();
                    if (dhPlayerStat.Save())
                    {
                        stepSequence++;
                        CountPrompt = 1;
                    }
                    else CountPrompt = 20;
                    dhPlayerStat.Dispose();
                }
                // Invoke event
                if (stepSequence == 4)
                {
                    InvokeNewPlayer();
                    stepSequence++;
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Error: step sequence = " + stepSequence.ToString();
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 1)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "New player added";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 5)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Couldn't save player";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 10)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "No playername";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 11)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Player name greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 12)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "First name greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 13)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Last name greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 14)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Country greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 15)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Playername exists";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 16)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Name \"Bot\" is not possible";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 20)
                {
                    prompt.Header = "Player error";
                    prompt.Description1 = "Could not save statistic";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                #endregion
            }
            public void DeletePlayer(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                #endregion
                #region initialize
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    CountPrompt = DatahandlingPlayer.CheckData(2, DhMain);
                    if (CountPrompt == 1) stepSequence++;
                    else stepSequence = 0;
                }
                #endregion
                #region processing
                // Delete
                if (stepSequence == 2)
                {
                    DatahandlingPlayer dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D, DhMain);
                    if (dhPlayer.Delete(DhMain.Index))
                    {
                        stepSequence++;
                        CountPrompt = 1;
                    }
                    else
                    {
                        CountPrompt = 5;
                    }
                }
                // Delete player statistic
                if (stepSequence == 3)
                {
                    var dhPlayerStat = new DatahandlingPlayerStat(Configuration.FilePathDDDS3D);

                    if (dhPlayerStat.Delete(DhMain.Index))
                    {
                        stepSequence++;
                        CountPrompt = 1;
                    }
                    else CountPrompt = 30;
                    dhPlayerStat.Dispose();
                }
                // Invoke event
                if (stepSequence == 4)
                {
                    InvokeDeletePlayer();
                    stepSequence++;
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Error: step sequence = " + stepSequence.ToString();
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 1)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Deleted player";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 5)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Couldn't delete player";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 20)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "No player selected";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 30)
                {
                    prompt.Header = "Player error";
                    prompt.Description1 = "Could not save statistic";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                #endregion
            }
            public void SelectPlayer(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                int CountPlayer;
                DatahandlingPlayer.Main[] mainPlayer;
                #endregion
                #region initialize
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    CountPrompt = DatahandlingPlayer.CheckData(3, DhMain);
                    if (CountPrompt == 1) stepSequence++;
                    else stepSequence = 0;
                }
                #endregion
                #region processing
                // Load file
                if (stepSequence == 2)
                {
                    mainPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D).MainLoad();
                    stepSequence++;

                    // Select player
                    if (stepSequence == 3)
                    {
                        CountPlayer = DatahandlingPlayer.GetCountPlayer();
                        for (int i = 0; i < CountPlayer; i++)
                            if (DhMain.Index == mainPlayer[i].Index)
                            {
                                DhMain = mainPlayer[i];
                            }
                        stepSequence++;
                    }
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Error: step sequence = " + stepSequence.ToString();
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                #endregion
            }
            public void EditPlayer(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                int playerIndex = new int();
                #endregion
                #region initialize
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    CountPrompt = DatahandlingPlayer.CheckData(4, DhMain);
                    if (CountPrompt == 1) stepSequence++;
                    else stepSequence = 0;
                }
                #endregion
                #region processing
                // Edit
                if (stepSequence == 2)
                {
                    DatahandlingPlayer dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D, DhMain);
                    if (dhPlayer.Edit())
                    {
                        playerIndex = dhPlayer.StructMain.Index;
                        stepSequence++;
                        CountPrompt = 1;
                    }
                    else
                    {
                        CountPrompt = 5;
                    }
                }
                // Edit player statistic
                if (stepSequence == 3)
                {
                    var dhPlayerStat = new DatahandlingPlayerStat(Configuration.FilePathDDDS3D);
                    if (dhPlayerStat.EditPlayerName(playerIndex))
                    {
                        stepSequence++;
                        CountPrompt = 1;
                    }
                    else CountPrompt = 20;
                    dhPlayerStat.Dispose();
                }
                // Invoke event
                if (stepSequence == 4)
                {
                    InvokeEditPlayer();
                    stepSequence++;
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Error: step sequence = " + stepSequence.ToString();
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 1)
                {
                    prompt.Header = "Edit player";
                    prompt.Description1 = "Successfully changed player data";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 5)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Couldn't save player";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 20)
                {
                    prompt.Header = "Player error";
                    prompt.Description1 = "Could not save statistic";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 30)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "No playername";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 31)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Player name greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 32)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "First name greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 33)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Last name greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 34)
                {
                    prompt.Header = "Player";
                    prompt.Description1 = "Country greater 32 letters";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                #endregion
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

                    disposedValue = true;
                }
            }
            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            ~Player()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }
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
        #endregion
        #region game
        public class Game : IDisposable
        {
            #region constructor
            public Game() { }
            public Game(ListBox playerListBox)
            {
                PlayerListBox = playerListBox;
            }
            #endregion
            #region properties, fields
            private DDD.Game.MainProp _Properties { get; set; }
            public DDD.Game.MainProp Properties 
            {
                get { return _Properties; } 
                set { _Properties = value; }
            }

            private ListBox _PlayerListBox { get; set; }
            public ListBox PlayerListBox { get { return _PlayerListBox; } set { _PlayerListBox = value; } }

            // Fields
            public bool[] Checking = new bool[21]
            {
            false,false,false,false,false,false,false,false,false,false,
            false,false,false,false,false,false,false,false,false,false,false
            };
            #endregion
            #region static properties
            private static int _CountEventFirstThrowExecution { get; set; }
            public static int CountEventFirstThrowExecution 
            { 
                get { return _CountEventFirstThrowExecution; } 
                set { _CountEventFirstThrowExecution = value; }
            }
            #endregion
            #region event definition: new game
            private EventHandler<EventArgNewGame> _EventHandlerNewGame;
            public event EventHandler<EventArgNewGame> EventHandlerNewGame
            {
                add { _EventHandlerNewGame += value; }
                remove { _EventHandlerNewGame -= value; }
            }
            public void InvokeNewGame()
            {
                if (_EventHandlerNewGame != null)
                {
                    EventArgNewGame args = new EventArgNewGame(this.Properties);
                    _EventHandlerNewGame.Invoke(this, args);
                }
            }
            public class EventArgNewGame : EventArgs
            {
                public EventArgNewGame(DDD.Game.MainProp newGame)
                {
                    NewGame = newGame;
                }
                public DDD.Game.MainProp NewGame;
            }
            #endregion
            #region event definition: first throw execution
            private EventHandler<EventArgFirstThrowExecution> _EventHandlerFirstThrowExecution;
            public event EventHandler<EventArgFirstThrowExecution> EventHandlerFirstThrowExecution
            {
                add
                {
                    _EventHandlerFirstThrowExecution += value;
                    CountEventFirstThrowExecution++;
                }
                remove
                {
                    _EventHandlerFirstThrowExecution -= value;
                    CountEventFirstThrowExecution--;
                }
            }
            public void InvokeFirstThrowExecution()
            {
                if (_EventHandlerFirstThrowExecution != null)
                {
                    EventArgFirstThrowExecution args = new EventArgFirstThrowExecution(this.Properties.CheckFirstThrowExecution);
                    _EventHandlerFirstThrowExecution.Invoke(this, args);
                }
            }
            public class EventArgFirstThrowExecution : EventArgs
            {
                public EventArgFirstThrowExecution(bool firstThrowExecution)
                {
                    FirstThrowExecution = firstThrowExecution;
                }
                public bool FirstThrowExecution;
            }
            #endregion
            #region event definition: game ended
            private EventHandler<EventArgGameEnd> _EventHandlerGameEnd;
            public event EventHandler<EventArgGameEnd> EventHandlerGameEnd
            {
                add { _EventHandlerGameEnd += value; }
                remove { _EventHandlerGameEnd -= value; }
            }
            public void InvokeGameEnd()
            {
                if (_EventHandlerGameEnd != null)
                {
                    EventArgGameEnd args = new EventArgGameEnd(this.Checking[(int)CheckingBools.CheckingSetThrowGameWin]);
                    _EventHandlerGameEnd.Invoke(this, args);
                }
            }
            public class EventArgGameEnd : EventArgs
            {
                public EventArgGameEnd(bool gameEnd)
                {
                    GameEnd = gameEnd;
                }
                public bool GameEnd;
            }
            #endregion
            #region event definition: Bot
            private EventHandler<EventArgGameBot> _EventHandlerGameBot;
            public event EventHandler<EventArgGameBot> EventHandlerGameBot
            {
                add { _EventHandlerGameBot += value; }
                remove { _EventHandlerGameBot -= value; }
            }
            public void InvokeGameBot(int score)
            {
                if (_EventHandlerGameEnd != null)
                {
                    EventArgGameBot args = new EventArgGameBot(score);
                    _EventHandlerGameBot.Invoke(this, args);
                }
            }
            public class EventArgGameBot : EventArgs
            {
                public EventArgGameBot(int score)
                {
                    Score = score;
                }
                public int Score;
            }
            #endregion
            #region sequences
            public void GameNew(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                #endregion
                #region initialize
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    CountPrompt = DDD.Game.Check(1);
                    if (CountPrompt == 1) stepSequence++;
                    else stepSequence = 0;
                }
                #endregion
                #region processing
                // New game
                if (stepSequence == 2)
                {
                    stepSequence++;
                }
                // Invoke event
                if (stepSequence == 3)
                {
                    InvokeNewGame();
                    stepSequence++;
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "New game";
                    prompt.Description1 = "Error: step sequence = " + stepSequence.ToString();
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 1)
                {
                    //prompt.Header = "New game";
                    //prompt.Description1 = "Starting new game";
                    //prompt.Description2 = "";
                    //prompt.Visibility_Action01 = true;
                    //prompt.Show();
                }
                if (CountPrompt == 10)
                {
                    prompt.Header = "New game";
                    prompt.Description1 = "No player selected";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 11)
                {
                    prompt.Header = "New game";
                    prompt.Description1 = "Check player max 10";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                #endregion
            }
            public void GameSetThrow(int stepSequence)
            {
                #region variable decleration
                //_G_Prompt prompt01 = new _G_Prompt();
                int CountPrompt = new int();
                CountPrompt = 0;
                #endregion
                #region initialize / checking
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    // Initializing
                    stepSequence = 10; // Initialize
                    CountPrompt = 10;
                    if (!DDD.Game.MainProperties.CheckFirstThrowExecution) stepSequence = 100; // First throw execution
                }
                // Checking
                if (stepSequence == 10)
                {
                    var dhGame = new DDD.Game();
                    Checking[(int)CheckingBools.CheckingBot] = dhGame.GamePropertiesCheck(10);
                    if (Checking[(int)CheckingBools.CheckingBot])
                    {
                        int counter = new int();
                        var score = DartBot.TransformFinishSingleToArray(DartBot.GetThreeScores(
                            DDD.Game.GameProperties.InputScoreBot,
                            DDD.Game.MainProperties.BotDifficultyValue));
                        
                        DDD.Game.GameProperties.InputScore = DartBot.GetScore(score);

                    }
                    Checking[(int)CheckingBools.CheckingSetThrowWin] = dhGame.GamePropertiesCheck(1);
                    Checking[(int)CheckingBools.CheckingSetThrowThrowOK] = dhGame.GamePropertiesCheck(2);
                    Checking[(int)CheckingBools.CheckingSetThrowOverthrow] = dhGame.GamePropertiesCheck(3);
                    Checking[(int)CheckingBools.CheckingSetThrowFinishPossible] = dhGame.GamePropertiesCheck(4);
                    if (Checking[(int)CheckingBools.CheckingSetThrowWin])
                    {
                        Checking[(int)CheckingBools.CheckingSetThrowLegWin] = dhGame.GamePropertiesCheck(5);
                        Checking[(int)CheckingBools.CheckingSetThrowSetWin] = dhGame.GamePropertiesCheck(6);
                        Checking[(int)CheckingBools.CheckingSetThrowGameWin] = dhGame.GamePropertiesCheck(7);
                    }
                    else
                    {
                        Checking[(int)CheckingBools.CheckingSetThrowLegWin] = false;
                        Checking[(int)CheckingBools.CheckingSetThrowSetWin] = false;
                        Checking[(int)CheckingBools.CheckingSetThrowGameWin] = false;
                    }
                    Checking[(int)CheckingBools.CheckingSetThrowFirstRound] = dhGame.GamePropertiesCheck(8);
                    Checking[(int)CheckingBools.CheckingSetThrowPlayerFinished] = dhGame.GamePropertiesCheck(9);
                    dhGame.Dispose();
                    // Sequence
                    stepSequence = 500;
                    if (Checking[(int)CheckingBools.CheckingSetThrowPlayerFinished])
                        stepSequence = 2500;
                    //else if (Checking[(int)CheckingBools.CheckingBot])
                    //    stepSequence = 450;
                }
                #endregion
                #region processing
                #region first throw execution
                // 100: Set game properties first execution
                if (stepSequence == 100)
                {
                    var dhGame = new DDD.Game(PlayerListBox);
                    dhGame.FirstThrowExecute();
                    dhGame.CounterFirst(1);
                    InvokeFirstThrowExecution();
                    DDD.Game.MainProperties.CheckFirstThrowExecution = true;
                    dhGame.Dispose();

                    stepSequence = 200;
                }
                // 200: Checking
                if (stepSequence == 200)
                {
                    var dhGame = new DDD.Game();
                    Checking[(int)CheckingBools.CheckingBot] = dhGame.GamePropertiesCheck(10);
                    if (Checking[(int)CheckingBools.CheckingBot])
                    {
                        var score = DartBot.TransformFinishSingleToArray(DartBot.GetThreeScores(
                            DDD.Game.GameProperties.InputScoreBot,
                            DDD.Game.MainProperties.BotDifficultyValue));
                        
                        DDD.Game.GameProperties.InputScore = DartBot.GetScore(score);
                    }
                    Checking[(int)CheckingBools.CheckingSetThrowWin] = dhGame.GamePropertiesCheck(1);
                    Checking[(int)CheckingBools.CheckingSetThrowThrowOK] = dhGame.GamePropertiesCheck(2);
                    Checking[(int)CheckingBools.CheckingSetThrowOverthrow] = dhGame.GamePropertiesCheck(3);
                    Checking[(int)CheckingBools.CheckingSetThrowFinishPossible] = dhGame.GamePropertiesCheck(4);
                    if (Checking[(int)CheckingBools.CheckingSetThrowWin])
                    {
                        Checking[(int)CheckingBools.CheckingSetThrowLegWin] = dhGame.GamePropertiesCheck(5);
                        Checking[(int)CheckingBools.CheckingSetThrowSetWin] = dhGame.GamePropertiesCheck(6);
                        Checking[(int)CheckingBools.CheckingSetThrowGameWin] = dhGame.GamePropertiesCheck(7);
                    }
                    else
                    {
                        Checking[(int)CheckingBools.CheckingSetThrowLegWin] = false;
                        Checking[(int)CheckingBools.CheckingSetThrowSetWin] = false;
                        Checking[(int)CheckingBools.CheckingSetThrowGameWin] = false;
                    }
                    Checking[(int)CheckingBools.CheckingSetThrowFirstRound] = dhGame.GamePropertiesCheck(8);
                    Checking[(int)CheckingBools.CheckingSetThrowPlayerFinished] = dhGame.GamePropertiesCheck(9);
                    //Checking[(int)CheckingBools.CheckingBot] = dhGame.GamePropertiesCheck(10);
                    dhGame.Dispose();
                    // Sequence
                    stepSequence = 300;
                    if (Checking[(int)CheckingBools.CheckingSetThrowPlayerFinished])
                        stepSequence = 2500;
                    //else if (Checking[(int)CheckingBools.CheckingBot])
                    //    stepSequence = 250;
                }
                //// 250: Set bot throw
                //if (stepSequence == 250)
                //{
                //    int counter = new int();
                //    var score = DartBot.TransformFinishSingleToArray(DartBot.GetThreeScores(
                //        DDD.Game.GameProperties.InputScoreBot,
                //        DDD.Game.MainProperties.BotDifficultyValue));

                //    for (int i = 0; i < 3; i++)
                //        counter = counter + score[0, i];

                //    DDD.Game.GameProperties.InputScore = counter;

                //    stepSequence = 300;
                //}
                // 300: Set game properties
                if (stepSequence == 300)
                {
                    var dhGame = new DDD.Game();
                    dhGame.GamePropertiesSet(1);
                    dhGame.Dispose();

                    stepSequence = 400;
                }
                // 400: Set game properties player statistic data first execution
                if (stepSequence == 400)
                {
                    var dhGame = new DDD.Game();

                    stepSequence = 2000;
                    if (Checking[(int)CheckingBools.CheckingSetThrowWin]
                        || Checking[(int)CheckingBools.CheckingSetThrowThrowOK])
                        dhGame.GamePropertiesSetPlayerData(1);
                    if (Checking[(int)CheckingBools.CheckingSetThrowOverthrow])
                        dhGame.GamePropertiesSetPlayerData(3);
                    if (Checking[(int)CheckingBools.CheckingSetThrowWin])
                        stepSequence = 1000;
                    dhGame.Dispose();
                }
                #endregion
                #region throw execution
                //// 450: Set bot throw
                //if (stepSequence == 450)
                //{
                //    int counter = new int();
                //    var score = DartBot.TransformFinishSingleToArray(DartBot.GetThreeScores(
                //        DDD.Game.GameProperties.InputScoreBot, 
                //        DDD.Game.MainProperties.BotDifficultyValue));

                //    for (int i = 0; i < 3; i++)
                //        counter = counter + score[0,i];

                //    DDD.Game.GameProperties.InputScore = counter;

                //    stepSequence = 500;
                //}
                // 500: Set game properties 
                if (stepSequence == 500)
                {
                    var dhGame = new DDD.Game();
                    dhGame.GamePropertiesSet(2);
                    dhGame.Dispose();

                    stepSequence = 600;
                }
                // 600: Set game properties player statistic data
                if (stepSequence == 600)
                {
                    var dhGame = new DDD.Game();
                    stepSequence = 2000;
                    if (Checking[(int)CheckingBools.CheckingSetThrowFirstRound])
                    {
                        if (Checking[(int)CheckingBools.CheckingSetThrowWin]
                            || Checking[(int)CheckingBools.CheckingSetThrowThrowOK])
                            dhGame.GamePropertiesSetPlayerData(1);
                        if (Checking[(int)CheckingBools.CheckingSetThrowOverthrow])
                            dhGame.GamePropertiesSetPlayerData(3);
                        if (Checking[(int)CheckingBools.CheckingSetThrowWin])
                            stepSequence = 1000;
                    }
                    else
                    {
                        if (Checking[(int)CheckingBools.CheckingSetThrowWin]
                            || Checking[(int)CheckingBools.CheckingSetThrowThrowOK])
                            dhGame.GamePropertiesSetPlayerData(2);
                        if (Checking[(int)CheckingBools.CheckingSetThrowOverthrow] 
                            && !Checking[(int)CheckingBools.CheckingSetThrowWin])
                            dhGame.GamePropertiesSetPlayerData(4);
                        if (Checking[(int)CheckingBools.CheckingSetThrowWin])
                            stepSequence = 1000;
                    }
                    dhGame.Dispose();
                }
                // 1000: Set winner
                if (stepSequence == 1000)
                {
                    var dhGame = new DDD.Game();
                    dhGame.GamePropertiesSetPlayerData(5);
                    if (Checking[(int)CheckingBools.CheckingSetThrowSetWin])
                        dhGame.GamePropertiesSetPlayerData(6);
                    dhGame.GamePropertiesSetPlayerData(7);
                    if (Checking[(int)CheckingBools.CheckingSetThrowGameWin])
                        dhGame.GamePropertiesSetPlayerData(8);
                    dhGame.Dispose();

                    // sequence
                    stepSequence = 2000;
                    if (Checking[(int)CheckingBools.CheckingSetThrowWin])
                        stepSequence = 1900;
                }
                #endregion
                #region counter execution
                // Reset counter when finished
                if (stepSequence == 1900)
                {
                    var dhGame = new DDD.Game();
                    
                    dhGame.GamePropertiesSetPlayerData(11);
                    
                    if (Checking[(int)CheckingBools.CheckingSetThrowLegWin])
                        dhGame.GamePropertiesSetPlayerData(9);
                    if (Checking[(int)CheckingBools.CheckingSetThrowSetWin])
                        dhGame.GamePropertiesSetPlayerData(10);
                    dhGame.CounterFirst(2);
                    dhGame.Dispose();
                    stepSequence = 10000;
                }
                // Set counter
                if (stepSequence == 2000)
                {
                    var dhGame = new DDD.Game();
                    dhGame.CounterSet(1);
                    dhGame.Dispose();

                    stepSequence = 15000;
                    if (Checking[(int)CheckingBools.CheckingSetThrowGameWin])
                        stepSequence = 10000;
                }
                // Set counter when player finished
                if (stepSequence == 2500)
                {
                    var dhGame = new DDD.Game();
                    dhGame.CounterSet(2);
                    dhGame.Dispose();
                    stepSequence = 15000;
                }
                #endregion
                #region winner execution
                // Game winner
                if (stepSequence == 10000)
                {
                    stepSequence = 15000;
                    if (Checking[(int)CheckingBools.CheckingSetThrowLegWin] 
                        && !Checking[(int)CheckingBools.CheckingSetThrowSetWin])
                    {
                        CountPrompt = 3;
                    }
                    if (Checking[(int)CheckingBools.CheckingSetThrowSetWin])
                    {
                        CountPrompt = 2;
                         DDD.Game.GameCounter.CountSets++;
                    }
                    if (Checking[(int)CheckingBools.CheckingSetThrowGameWin])
                    {
                        CountPrompt = 1;
                        DDD.Game.MainProperties.CheckGameWin = true;
                        stepSequence = 999999;
                        InvokeGameEnd();
                    }
                }
                #endregion
                #region output execution
                // Set output
                if (stepSequence == 15000)
                {
                    var dhGame = new DDD.Game();
                    dhGame.GameOutputSet();
                    // Set datagrid statistic
                    dhGame.GameDataGridSet(DDD.Game.MainProperties.CountPlayer);
                    dhGame.Dispose();
                }
                #endregion
                #endregion
                #region output
                _G_Prompt prompt01 = new _G_Prompt();
                if (CountPrompt == 0)
                {
                    prompt01.Header = "Game";
                    prompt01.Description1 = "Error: step sequence = " + stepSequence.ToString();
                    prompt01.Description2 = "";
                    prompt01.Visibility_Action01 = true;
                    prompt01.Show();
                }
                //if (CountPrompt == 1)
                //{
                //    prompt01.Header = "Game win";
                //    prompt01.Description1 = "";
                //    prompt01.Description2 = "";
                //    prompt01.Visibility_Action01 = true;
                //    prompt01.Show();
                //    prompt01.Dispose();
                //}
                if (CountPrompt == 2)
                {
                    prompt01.Header = "Set win";
                    prompt01.Description1 = DDD.Game.GameProperties.PlayerWin[DDD.Game.GameProperties.PlayerWin.Count - 1].Name.ToString();
                    prompt01.Description2 = "";
                    prompt01.Visibility_Action01 = true;
                    prompt01.Show();
                    prompt01.Dispose();
                }
                if (CountPrompt == 3)
                {
                    prompt01.Header = "Leg win";
                    prompt01.Description1 = DDD.Game.GameProperties.PlayerWin[DDD.Game.GameProperties.PlayerWin.Count - 1].Name.ToString();
                    prompt01.Description2 = "";
                    prompt01.Visibility_Action01 = true;
                    prompt01.Show();
                    prompt01.Dispose();
                }
                #endregion
            }
            public void GameResetThrow(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                #endregion
                #region initialize & checking
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    stepSequence = 10;
                }
                //Checking
                if (stepSequence == 10)
                {
                    var dhGame = new DDD.Game();
                    Checking[(int)CheckingBools.CheckingResetThrowCounterOK] = dhGame.CounterResetCheck(1);
                    dhGame.Dispose();

                    // sequence
                    stepSequence = 200;
                    if (Checking[(int)CheckingBools.CheckingResetThrowCounterOK])
                        stepSequence = 100;
                }
                #endregion
                #region processing
                // Reset counter
                if (stepSequence == 100)
                {
                    var dhGame = new DDD.Game();
                    if (dhGame.CounterReset()) CountPrompt = 1;
                    else CountPrompt = 0;
                    dhGame.Dispose();

                    stepSequence = 300;
                }
                // Error
                if (stepSequence == 200)
                {
                    CountPrompt = 0;
                }
                // Output
                if (stepSequence == 300)
                {
                    var dhGame = new DDD.Game();
                    if (dhGame.GameOutputSet()) CountPrompt = 1;
                    else CountPrompt = 0;
                    // Set datagrid statistic
                    dhGame.GameDataGridSet(DDD.Game.MainProperties.CountPlayer);
                    dhGame.Dispose();
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "Reset last throw";
                    prompt.Description1 = "Error";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 1)
                {
                    prompt.Header = "Reset last throw";
                    prompt.Description1 = "Successfully";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                #endregion
            }
            public void GameSave(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                #endregion
                #region initialize
                //Initialize
                if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
                if (stepSequence == 1)
                {
                    CountPrompt = 1;
                    if (CountPrompt == 1) stepSequence++;
                    else stepSequence = 0;
                }
                #endregion
                #region processing
                // Save end ranking
                if (stepSequence == 2)
                {
                    // Save end ranking in sequence game end || Reset counter when finished if (stepSequence == 1900)
                        //var dhGame = new DDD.Game();
                        //dhGame.GamePropertiesSetPlayerData(11);
                        //dhGame.Dispose();
                        stepSequence++;
                }
                // Save game end time
                if (stepSequence == 3)
                {
                    var dhGame = new DDD.Game();
                    dhGame.GamePropertiesSetPlayerData(12);
                    dhGame.Dispose();
                    stepSequence++;
                }
                // Save player statistics
                if (stepSequence == 4)
                {
                    var dhPlayerStat = new DatahandlingPlayerStat(Configuration.FilePathDDDS3D);
                    dhPlayerStat.InitializeEndGamePlayerDS(DDD.Game.GameProperties, DDD.Game.MainProperties);
                    dhPlayerStat.SaveAll();
                    dhPlayerStat.Dispose();
                    stepSequence++;
                }
                // Save data statistics
                if (stepSequence == 5)
                {
                    string pathG3D = Configuration.FilePathDDDG3D + DatahandlingGame.SetFileNameG3D();
                    var dhGameStat = new DatahandlingGame(pathG3D, true);
                    if (dhGameStat.Save()) CountPrompt = 1;
                    else CountPrompt = 0;
                    dhGameStat.Dispose();
                    stepSequence++;
                }
                // Copy end ranking to end screen
                if (stepSequence == 6)
                {
                    DDD_WPF.Screens._02X_Game._035_Game_End.EndRanking = 
                        DDD.Game.GameProperties.PlayerEndRanking;
                    stepSequence++;
                }
                // Dispose game data
                if (stepSequence == 7)
                {
                    var dhGame = new DDD.Game();
                    dhGame.DisposeGameData();
                    dhGame.Dispose();
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "Error:";
                    prompt.Description1 = "Save game";
                    prompt.Description2 = "Step sequence = " + stepSequence.ToString();
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 1)
                {
                    //prompt.Header = "New game";
                    //prompt.Description1 = "Starting new game";
                    //prompt.Description2 = "";
                    //prompt.Visibility_Action01 = true;
                    //prompt.Show();
                }
                #endregion
            }
            //public void GameSetThrowBot(int stepSequence)
            //{
            //    #region variable decleration
            //    _G_Prompt prompt = new _G_Prompt();
            //    int CountPrompt = new int();

            //    DDD.DartBot dartBot = new DartBot(3);
            //    var getProbability = new Probability(dartBot.Difficulty);
            //    int[] scores = new int[3];
            //    Probability.Matrix probabilityMatrix;

            //    #endregion
            //    #region initialize
            //    //Initialize
            //    if (stepSequence > 1 || stepSequence < 1) CountPrompt = 0;
            //    if (stepSequence == 1)
            //    {

            //    }
            //    #endregion
            //    #region processing
            //    // Get probability
            //    if (stepSequence == 2)
            //    {

            //        stepSequence++;
            //    }
            //    // Invoke event
            //    if (stepSequence == 3)
            //    {

            //    }
            //    #endregion
            //    #region output
            //    if (CountPrompt == 0)
            //    {
            //        //prompt.Header = "New game";
            //        //prompt.Description1 = "Error: step sequence = " + stepSequence.ToString();
            //        //prompt.Description2 = "";
            //        //prompt.Visibility_Action01 = true;
            //        //prompt.Show();
            //    }
            //    if (CountPrompt == 1)
            //    {

            //    }
            //    #endregion
            //}
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
                    Checking = null;
                    _PlayerListBox = null;
                    PlayerListBox = null;

                    disposedValue = true;
                }
            }
            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            ~Game()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }
            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
            #endregion
            #region enumeration
            public enum CheckingBools 
            {
                CheckingSetThrowWin = 0,
                CheckingSetThrowThrowOK = 1,
                CheckingSetThrowOverthrow = 2,
                CheckingSetThrowFinishPossible = 3,
                CheckingSetThrowLegWin = 4,
                CheckingSetThrowSetWin = 5,
                CheckingSetThrowGameWin = 6, 
                CheckingSetThrowFirstRound = 7, 
                CheckingSetThrowPlayerFinished = 8,
                CheckingResetThrowCounterOK = 10,
                CheckingBot = 20
            }
            #endregion
        }
        #endregion
        #region statistic
        public class Statistic : IDisposable
        {
            #region constructor
            public Statistic() { }
            /// <summary>
            /// Constructor who takes 4 arguments
            /// </summary>
            /// <param name="fullPathName">Set path name and file name with extension</param>
            public Statistic(string fullPathName, string gameName, DatahandlingGame.GameStatisticStruct gameStatistic, int mode)
            {
                FullPathName = fullPathName;
                GameName = gameName;
                GameStatistic = gameStatistic;
                Mode = mode;
            }
            /// <summary>
            /// Constructor who takes 3 arguments
            /// </summary>
            /// <param name="fullPathName">Set path name and file name with extension</param>
            /// <param name="gameStatistic">Set game statistic</param>
            /// <param name="mode">Mode 1: Export player / Mode 2: Export game</param>
            public Statistic(string fullPathName, DatahandlingGame.GameStatisticStruct gameStatistic, int mode)
            {
                FullPathName = fullPathName;
                GameStatistic = gameStatistic;
                if (mode == 1 || mode == 2)
                    Mode = mode;
                else
                    return;
            }
            /// <summary>
            /// Constructor who takes 3 arguments
            /// </summary>
            /// <param name="fullPathName">Set path name and file name with extension</param>
            /// <param name="playerStatistic">Set player statistic</param>
            /// <param name="mode">Mode 1: Export player / Mode 2: Export game</param>
            public Statistic(string fullPathName, DatahandlingPlayerStat.MainStruct playerStatistic, int mode)
            {
                FullPathName = fullPathName;
                PlayerStatistic = playerStatistic;
                if (mode == 1 || mode == 2)
                    Mode = mode;
                else
                    return;
            }
            #endregion
            #region event definition: 

            #endregion
            #region properties
            private string _FullPathName { get; set; }
            public string FullPathName { 
                get { return _FullPathName; } 
                set { _FullPathName = value; } 
            }

            private string _GameName { get; set; }
            public string GameName
            {
                get { return _GameName; }
                set { _GameName = value; }
            }

            private DatahandlingGame.GameStatisticStruct _GameStatistic { get; set; }
            public DatahandlingGame.GameStatisticStruct GameStatistic 
            {
                get { return _GameStatistic; } 
                set { _GameStatistic = value; } 
            }

            private DatahandlingPlayerStat.MainStruct _PlayerStatistic { get; set; }
            public DatahandlingPlayerStat.MainStruct PlayerStatistic
            {
                get { return _PlayerStatistic; }
                set { _PlayerStatistic = value; }
            }

            private int _Mode { get; set; }
            /// <summary>
            /// Mode 1: Export player / Mode 2: Export game
            /// </summary>
            public int Mode
            {
                get { return _Mode; }
                set { _Mode = value; }
            }
            #endregion
            #region sequence
            public void StatisticSave(int stepSequence)
            {
                #region variable decleration
                _G_Prompt prompt = new _G_Prompt();
                int CountPrompt = new int();
                #endregion
                #region initialize
                //Initialize
                if (stepSequence > 1 || stepSequence < 1)
                {
                    stepSequence = 0;
                    CountPrompt = 0; 
                }
                if (stepSequence == 1)
                {
                    CountPrompt = 1;
                    stepSequence++;
                    if (Mode == 1)
                    {
                        if (FullPathName.IndexOf(".csv") > 0)
                            stepSequence = 20;
                        if (FullPathName.IndexOf(".xlsx") > 0)
                            stepSequence = 10;
                    }
                    if (Mode == 2)
                    {
                        if (FullPathName.IndexOf(".csv") > 0)
                            stepSequence = 30;
                        if (FullPathName.IndexOf(".xlsx") > 0)
                            stepSequence = 10;
                    }
                    if (Mode > 2 || Mode < 1)
                    {
                        CountPrompt = 0;
                        stepSequence = 0;
                    }
                }
                #endregion
                #region processing
                try
                {
                    // Check .xlsx
                    if (stepSequence == 10)
                    {
                        var dhStatSave = new DDD.Statistic(FullPathName);
                        if (DDD.Statistic.Check(1) == 1)
                        {
                            stepSequence = 0;
                            CountPrompt = 10;
                        }
                        else
                        {
                            CountPrompt = 1;
                            if (Mode == 1)
                                stepSequence = 25;
                            if (Mode == 2)
                                stepSequence = 35;
                        }
                        if (DDD.Statistic.Check(2) == 20)
                        {
                            stepSequence = 0;
                            CountPrompt = 20;
                        }
                        dhStatSave.Dispose();
                    }
                    // Export player
                    // Save .csv
                    if (stepSequence == 20)
                    {
                        var dhStatSave = new DDD.Statistic();
                        if (dhStatSave.SavePlayerCSV(FullPathName, PlayerStatistic))
                            CountPrompt = 1;
                        else
                            CountPrompt = 0;
                        dhStatSave.Dispose();
                    }
                    // Save .xlsx
                    if (stepSequence == 25)
                    {
                        var dhStatistic = new DDD.Statistic();
                        CountPrompt = dhStatistic.SavePlayerXLSX(FullPathName, PlayerStatistic);
                    }
                    // Export game
                    // Save .csv
                    if (stepSequence == 30)
                    {
                        var dhStatSave = new DDD.Statistic();
                        if (dhStatSave.SaveGameCSV(FullPathName, GameStatistic))
                            CountPrompt = 1;
                        else
                            CountPrompt = 0;
                        dhStatSave.Dispose();
                    }
                    // Save .xlsx
                    if (stepSequence == 35)
                    {
                        var dhStatistic = new DDD.Statistic();
                        CountPrompt = dhStatistic.SaveGameXLSX(FullPathName,
                            Configuration.FilePathDDDG3D + GameName + Configuration.FileExtensionG3D);
                    }
                }
                catch (Exception ex)
                {
#if debug
                     Console.WriteLine(ex.Message);
                     Console.WriteLine(ex.Source);
#endif
                }
                #endregion
                #region output
                if (CountPrompt == 0)
                {
                    prompt.Header = "Statistic game";
                    prompt.Description1 = "Error: Export";
                    prompt.Description2 = "step sequence = " + stepSequence.ToString();
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 1)
                {
                    prompt.Header = "Statistic game";
                    prompt.Description1 = "Saved game statistic";
                    prompt.Description2 = FullPathName;
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 10)
                {
                    prompt.Header = "Statistic game";
                    prompt.Description1 = "Error: No excel installed";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 11)
                {
                    prompt.Header = "Statistic game";
                    prompt.Description1 = "Error: Check Excel license!";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                if (CountPrompt == 20)
                {
                    prompt.Header = "Statistic game";
                    prompt.Description1 = "Error: File already exists!";
                    prompt.Description2 = "";
                    prompt.Visibility_Action01 = true;
                    prompt.Show();
                }
                #endregion
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

                    disposedValue = true;
                }
            }
            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            ~Statistic()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }
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
        #endregion
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
         ~Sequence()
         {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
         }
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

#region constructor

#endregion
#region properties, fields, constants

#endregion
#region methods

#endregion
#region class library

#endregion

#define debug

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static DDD.DartBot;
using Excel = Microsoft.Office.Interop.Excel;
using ListBox = System.Windows.Controls.ListBox;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Controls;

namespace DDD
{
    /// <summary>
    /// Datahandling
    /// </summary>
    abstract class Datahandling : IDisposable
    {
        #region constructor
        public Datahandling()
        {
            CounterInstances++;
        }
        public Datahandling(string dataPath)
        {
            DataPath = dataPath;
            CounterInstances++;
            CheckPathExist(dataPath);
        }
        #endregion
        #region static properties
        public static int CounterInstances { get; set; }

        #endregion
        #region class properties
        // properties
        private string _DataPath { get; set; }
        public string DataPath
        {
            get { return _DataPath; }
            set { _DataPath = value; }
        }

        // fields
        #endregion
        #region class methods
        public abstract bool Save();
        public abstract bool Load();
        public virtual bool Edit() { return false; }
        public virtual bool Delete(int countIndex) { return false; }
        public abstract bool HeaderSave();
        public virtual void HeaderLoad() {; }
        public abstract bool MainSave();
        public virtual void MainLoad() {; }
        public abstract void Dispose();
        #endregion
        #region static methods
        public static string SetStringTo32Byte(string ReplaceString, bool CheckLength)
        {
            char Space = ' ';

            if (ReplaceString == null)
                for (int i = 0; i < 32; i++)
                    ReplaceString = ReplaceString + Space;

            int ReplaceStringLength = ReplaceString.Length;
            if (ReplaceStringLength > 32 && CheckLength)
                ReplaceString = ReplaceString.Substring(0, 32);
            else
                for (int i = ReplaceStringLength; i < 32; i++)
                    ReplaceString = ReplaceString + Space;

            return ReplaceString;
        }
        public static string SetStringTo32Byte(string ReplaceString)
        {
            char Space = ' ';

            if (ReplaceString == null)
                for (int i = 0; i < 32; i++)
                    ReplaceString = ReplaceString + Space;

            int ReplaceStringLength = ReplaceString.Length;
            for (int i = ReplaceStringLength; i < 32; i++)
                ReplaceString = ReplaceString + Space;

            return ReplaceString;
        }

        public static void QuickSortDim2(ref int[,] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = QuickSortPartitionDim2(ref arr, left, right);

                if (pivot > 1)
                    QuickSortDim2(ref arr, left, pivot - 1);

                if (pivot + 1 < right)
                    QuickSortDim2(ref arr, pivot + 1, right);
            }
        }
        private static int QuickSortPartitionDim2(ref int[,] arr, int left, int right)
        {
            int pivot = arr[0, left];
            while (true)
            {
                while (arr[0, left] > pivot)
                    left++;

                while (arr[0, right] < pivot)
                    right--;

                if (left < right)
                {
                    if (arr[0, left] == arr[0, right]) return right;

                    int temp1 = arr[0, left];
                    int temp2 = arr[1, left];
                    arr[0, left] = arr[0, right];
                    arr[1, left] = arr[1, right];
                    arr[0, right] = temp1;
                    arr[1, right] = temp2;
                }
                else return right;
            }
        }

        public static void QuickSortDim1(ref string[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = QuickSortPartitionDim1(ref arr, left, right);

                if (pivot > 1)
                    QuickSortDim1(ref arr, left, pivot - 1);

                if (pivot + 1 < right)
                    QuickSortDim1(ref arr, pivot + 1, right);
            }
        }
        private static int QuickSortPartitionDim1(ref string[] arr, int left, int right)
        {
            int pivot = right;
            while (true)
            {
                while (left < pivot)
                    left++;

                while (right > pivot)
                    right--;

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    string temp1 = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp1;
                }
                else return right;
            }
        }

        /// <summary>
        /// Convert ticks to date time string in standard format ("dd/MM/yyyy HH:mm:ss")
        /// </summary>
        /// <param name="getTicks"></param>
        /// <returns>String DateTime ("dd/MM/yyyy HH:mm:ss")</returns>
        public static string ConvertTicksToDateTime(long getTicks)
        {
            DateTime dt = new DateTime(getTicks);
            return dt.ToString("dd/MM/yyyy HH:mm:ss");
        }
        /// <summary>
        /// Convert ticks to date time string in custom format)
        /// </summary>
        /// <param name="getTicks"></param>
        /// <param name="format"></param>
        /// <returns>String DateTime</returns>
        public static string ConvertTicksToDateTime(long getTicks, string format)
        {
            DateTime dt = new DateTime(getTicks);
            return dt.ToString(format);
        }


        #endregion
        #region help virtual methods
        public virtual bool CheckPathExist(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);

                return false;
            }
        }
        public virtual bool CheckFileInitialized()
        {

            FileInfo fileInfo = new FileInfo(DataPath);
            if (!Directory.Exists(DataPath))
                if (fileInfo.Exists)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                br.BaseStream.Seek(0, SeekOrigin.Begin);
                                if (br.ReadInt32() == -1) return false;
                                else return true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error = {0}", e.Source);
                        Console.WriteLine("Error = {0}", e.Message);
                        Console.WriteLine("Error = {0}", e.StackTrace);
                        return false;
                    }
                }
                return false;
        }
        #endregion
        #region class library
        #endregion
    }
    class DatahandlingPlayer : Datahandling
    {
        #region constructor
        public DatahandlingPlayer()
        {
        }
        public DatahandlingPlayer(string dataPath) : base(dataPath)
        {
            CheckInitialized = false;
            CaseConstructorSelection = 1;
        }
        public DatahandlingPlayer(string dataPath, Main structMain) : base(dataPath)
        {
            CheckInitialized = false;
            StructMain = structMain;
            CaseConstructorSelection = 2;
        }
        public DatahandlingPlayer(string dataPath, Header structHeader) : base(dataPath)
        {
            CheckInitialized = false;
            StructHeader = structHeader;
            CaseConstructorSelection = 3;
        }
        public DatahandlingPlayer(string dataPath, Header structHeader, Main structMain) : base(dataPath)
        {
            CheckInitialized = false;
            StructHeader = structHeader;
            StructMain = structMain;
            CaseConstructorSelection = 4;
        }
        #endregion
        #region static properties
        public static int CountPlayer { get; set; }
        #endregion
        #region class properties, fields, constants
        // Properties
        private bool _CheckInitialized { get; set; }
        public bool CheckInitialized
        {
            get { return _CheckInitialized; }
            set
            {
                if (CheckFileInitialized() == false) InitializeHeader();
                _CheckInitialized = true;
            }
        }

        private Header _StructHeader { get; set; }
        public Header StructHeader
        {
            get { return _StructHeader; }
            set { _StructHeader = value; }
        }

        private Main _StructMain { get; set; }
        public Main StructMain
        {
            get { return _StructMain; }
            set { _StructMain = value; }
        }

        private Header _LoadHeader { get; set; }
        public Header LoadHeader
        {
            get { return _LoadHeader; }
            set { _LoadHeader = value; }
        }

        public Main[] _LoadAllMain { get; set; }
        public Main[] LoadAllMain
        {
            get { return _LoadAllMain; }
            set { _LoadAllMain = value; }
        }

        private Header SaveHeader { get; set; }
        public Main SaveMain { get; set; }

        public Main LoadMain { get; set; }

        // Fields
        private const int CountAllBytesPlayer = 168;
        private const int CountAllBytesMainPlayer = 148;
        private const int CountAllBytesHeaderPlayer = 20;
        private readonly byte CaseConstructorSelection = 0;

        // Constants
        public const string NamePlayerDataFile = "PlayerData.P3D";
        #endregion
        #region override methods
        public override bool Save()
        {
            MemoryStream ms = new MemoryStream();

            bool checkTempPlayerData = true;

            string tempNamePlayerDataFile = "Temp" + NamePlayerDataFile;
            string tempDataPath = DataPath.Replace(NamePlayerDataFile, tempNamePlayerDataFile);

            switch (CaseConstructorSelection)
            {
                case 0:
                    if (SaveMain.Index > 0 && SaveMain.PlayerName != "" &&
                        SaveHeader.CountAllBytes != 0)
                        return false;
                    SaveHeader = SetHeaderData(SaveHeader);
                    SaveMain = SetMainData(SaveMain);
                    break;
                case 1:
                    if (SaveMain.Index > 0 && SaveMain.PlayerName != "")
                        return false;
                    SaveHeader = SetHeaderData(HeaderLoad());
                    SaveMain = SetMainData(SaveMain);
                    break;
                case 2:
                    if (StructMain.Index > 0 && StructMain.PlayerName != "")
                        return false;
                    SaveHeader = SetHeaderData(HeaderLoad());
                    SaveMain = SetMainData(StructMain);
                    break;
                case 3:
                    if (SaveMain.Index > 0 && SaveMain.PlayerName != "" &&
                        StructHeader.CountAllBytes != 0)
                        return false;
                    SaveHeader = SetHeaderData(StructHeader);
                    SaveMain = SetMainData(SaveMain);
                    break;
                case 4:
                    if (StructMain.Index > 0 && StructMain.PlayerName != "" &&
                        StructHeader.CountAllBytes != 0)
                        return false;
                    SaveHeader = SetHeaderData(StructHeader);
                    SaveMain = SetMainData(StructMain);
                    break;
            }

            #region copy playerfile
            try
            {
                if (CheckInitialized == true && checkTempPlayerData)
                {
                    using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        fs.CopyTo(ms);
                        ms.Dispose();
                        fs.Dispose();
                    }
                    //File.Copy(DataPath, tempDataPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
            #endregion

            if (HeaderSave(SaveHeader) == false) checkTempPlayerData = false;
            if (MainSave(SaveMain) == false) checkTempPlayerData = false;

            #region delete playerfile
            try
            {
                if (checkTempPlayerData == false)
                {
                    using (FileStream fs = new FileStream(tempDataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        ms.WriteTo(fs);
                    }
                    using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        ms.WriteTo(fs);
                        ms.Dispose();
                        fs.Dispose();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
            #endregion

            if (checkTempPlayerData) return true;
            else return false;
        }
        public override bool Load()
        {
            try
            {
                LoadHeader = HeaderLoad();

                Main[] main = new Main[LoadHeader.CountAllPlayer];
                main = MainLoad();
                LoadAllMain = main;

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool Edit()
        {
            try
            {
                var player = MainLoad();
                int CountPlayer = GetCountPlayer();

                for (int i = 0; i < CountPlayer; i++)
                    if (StructMain.Index == player[i].Index)
                    {
                        player[i].PlayerName = StructMain.PlayerName;
                        player[i].FirstName = StructMain.FirstName;
                        player[i].LastName = StructMain.LastName;
                        player[i].Country = StructMain.Country;

                        player[i].BirthDay = StructMain.BirthDay;
                        player[i].BirthMonth = StructMain.BirthMonth;
                        player[i].BirthYear = StructMain.BirthYear;
                        break;
                    }
                MainSave(player);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool Delete(int index)
        {
            try
            {
                var dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D);
                var Main = dhPlayer.MainLoad();
                int CountPlayer = GetCountPlayer();

                for (int i = 0; i < CountPlayer; i++)
                    if (Main[i].Index == index)
                    {
                        Main[i].Index = -1;
                        break;
                    }

                dhPlayer.MainSave(Main);
                var asdf = ResetHeaderDataCountPlayer(HeaderLoad());
                HeaderSave(asdf);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }

        public virtual bool HeaderSave(Header header)
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(header.CountAllBytes);
                        bw.Write(header.CountMaxPlayerIndex);
                        bw.Write(header.CountMainBytes);
                        bw.Write(header.CountPlayer);
                        bw.Write(header.CountAllPlayer);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool HeaderSave()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(SaveHeader.CountAllBytes);
                        bw.Write(SaveHeader.CountMaxPlayerIndex);
                        bw.Write(SaveHeader.CountMainBytes);
                        bw.Write(SaveHeader.CountPlayer);
                        bw.Write(SaveHeader.CountAllPlayer);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }

        public virtual bool MainSave(Main[] main)
        {
            int CountPlayer = GetCountPlayer();
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(CountAllBytesHeaderPlayer, SeekOrigin.Begin);
                        for (int i = 0; i < CountPlayer; i++)
                        {
                            bw.Write(main[i].Index);
                            bw.Write(main[i].PlayerName);
                            bw.Write(main[i].FirstName);
                            bw.Write(main[i].LastName);
                            bw.Write(main[i].Country);
                            bw.Write(main[i].BirthDay);
                            bw.Write(main[i].BirthMonth);
                            bw.Write(main[i].BirthYear);
                        }
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual bool MainSave(Main main)
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.End);
                        bw.Write(main.Index);
                        bw.Write(main.PlayerName);
                        bw.Write(main.FirstName);
                        bw.Write(main.LastName);
                        bw.Write(main.Country);
                        bw.Write(main.BirthDay);
                        bw.Write(main.BirthMonth);
                        bw.Write(main.BirthYear);
                        bw.Dispose();
                        fs.Dispose();
                    }

                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool MainSave()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.End);
                        bw.Write(SaveMain.Index);
                        bw.Write(SaveMain.PlayerName);
                        bw.Write(SaveMain.FirstName);
                        bw.Write(SaveMain.LastName);
                        bw.Write(SaveMain.Country);
                        bw.Write(SaveMain.BirthDay);
                        bw.Write(SaveMain.BirthMonth);
                        bw.Write(SaveMain.BirthYear);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }

        public new Header HeaderLoad()
        {
            Header header = new Header();
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        header.CountAllBytes = br.ReadInt32();
                        header.CountMaxPlayerIndex = br.ReadInt32();
                        header.CountMainBytes = br.ReadInt32();
                        header.CountPlayer = br.ReadInt32();
                        header.CountAllPlayer = br.ReadInt32();
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                LoadHeader = header;
                return header;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return header;
            }
        }

        public Main MainLoad(int playerIndex)
        {
            Main main = new Main();

            if (playerIndex < 1) return main;
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(Seek(playerIndex), SeekOrigin.Begin);
                        main.Index = br.ReadInt32();
                        main.PlayerName = br.ReadString();
                        main.FirstName = br.ReadString();
                        main.LastName = br.ReadString();
                        main.Country = br.ReadString();
                        main.BirthDay = br.ReadInt32();
                        main.BirthMonth = br.ReadInt32();
                        main.BirthYear = br.ReadInt32();
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                LoadMain = main;
                return main;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return main;
            }
        }
        public Main MainLoad(string playerName)
        {
            Main main = new Main();
            Header header = new Header();
            header = HeaderLoad();

            using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    br.BaseStream.Seek(CountAllBytesHeaderPlayer, SeekOrigin.Begin);
                    for (int i = 0; i < header.CountAllPlayer; i++)
                    {
                        main.Index = br.ReadInt32();
                        main.PlayerName = br.ReadString();
                        main.FirstName = br.ReadString();
                        main.LastName = br.ReadString();
                        main.Country = br.ReadString();
                        main.BirthDay = br.ReadInt32();
                        main.BirthMonth = br.ReadInt32();
                        main.BirthYear = br.ReadInt32();
                        if (main.PlayerName == playerName) return main;
                    }
                    br.Dispose();
                    fs.Dispose();
                }
            }

            return main;
        }
        public new Main[] MainLoad()
        {
            Header header = HeaderLoad();
            Main[] main = new Main[header.CountAllPlayer];

            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(CountAllBytesHeaderPlayer, SeekOrigin.Begin);
                        for (int i = 0; i < header.CountAllPlayer; i++)
                        {
                            main[i].Index = br.ReadInt32();
                            main[i].PlayerName = br.ReadString();
                            main[i].FirstName = br.ReadString();
                            main[i].LastName = br.ReadString();
                            main[i].Country = br.ReadString();
                            main[i].BirthDay = br.ReadInt32();
                            main[i].BirthMonth = br.ReadInt32();
                            main[i].BirthYear = br.ReadInt32();
                        }
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                LoadAllMain = main;
                return main;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return main;
            }
        }

        public bool Edit(Main main, int playerIndex)
        {
            if (playerIndex < 1) return false;
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(Seek(playerIndex), SeekOrigin.Begin);
                        bw.Write(main.Index);
                        bw.Write(main.PlayerName);
                        bw.Write(main.FirstName);
                        bw.Write(main.LastName);
                        bw.Write(main.Country);
                        bw.Write(main.BirthDay);
                        bw.Write(main.BirthMonth);
                        bw.Write(main.BirthYear);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        #endregion
        #region help methods
        public bool InitializeHeader()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(CountAllBytesHeaderPlayer + CountAllBytesMainPlayer);
                        bw.Write(0);
                        bw.Write(CountAllBytesMainPlayer);
                        bw.Write(1);
                        bw.Write(1);

                        bw.Write(0);
                        bw.Write(SetStringTo32Byte("BotToNowhere"));
                        bw.Write(SetStringTo32Byte("BotToNowhere"));
                        bw.Write(SetStringTo32Byte("BotToNowhere"));
                        bw.Write(SetStringTo32Byte("BotToNowhere"));
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public int Seek(int playerIndex)
        {
            return CountAllBytesHeaderPlayer + playerIndex * CountAllBytesMainPlayer;
        }

        public Header SetHeaderData(Header header)
        {
            header.CountAllBytes += CountAllBytesMainPlayer;
            header.CountMaxPlayerIndex++;
            header.CountMainBytes += CountAllBytesMainPlayer;
            header.CountPlayer++;
            header.CountAllPlayer++;

            return header;
        }
        public Header ResetHeaderDataCountPlayer(Header header)
        {
            header.CountPlayer--;

            return header;
        }

        public Main SetMainData(Main main)
        {
            Header header = new Header();
            header = HeaderLoad();

            if (header.CountMaxPlayerIndex < 0) return main;
            main.Index = header.CountMaxPlayerIndex + 1;
            return main;
        }
        public Main ResetMainData(Main main)
        {
            Header header = new Header();
            header = HeaderLoad();

            if (header.CountMaxPlayerIndex > 0) return main;
            main.Index = -1;
            return main;
        }
        #endregion
        #region static methods
        /// <summary>
        /// Check player datahandling
        /// </summary>
        /// <param name="action">Case 1: New player / Case 2: Delete player / Case 3: Select player / Case 4: Edit player</param>
        /// <param name="dhMain"></param>
        /// <returns>1: Checking OK / 10: PlayerName length 0 / 11: PlayerName greater 32 / 12: FirstName greater 32 / 13: LastName greater 32 / 14: Country greater 32/ 15: Playername exists 
        /// / 20:Check index 
        /// /  / 30: PlayerName length 0 / 31: PlayerName greater 32 / 32: FirstName greater 32 / 33: LastName greater 32 / 34: Country greater 32</returns>
        /// <returns>Get all players when index is greater then 0 </returns>
        public static int CheckData(int action, DatahandlingPlayer.Main dhMain)
        {
            switch (action)
            {
                case 1:
                    {
                        // Return 10
                        if (dhMain.PlayerName.Trim().Length == 0) return 10;
                        // Return 11
                        if (dhMain.PlayerName.Length > 32) return 11;
                        // Return 12
                        if (dhMain.FirstName.Length > 32) return 12;
                        // Return 13
                        if (dhMain.LastName.Length > 32) return 13;
                        // Return 14
                        if (dhMain.Country.Length > 32) return 14;

                        // Return 15
                        DatahandlingPlayer dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D);
                        var player = dhPlayer.MainLoad();
                        var header = dhPlayer.HeaderLoad();
                        for (int i = 0; i < header.CountAllPlayer; i++)
                        {
                            if (player[i].PlayerName == dhMain.PlayerName)
                                return 15;
                        }

                        // Return 16
                        if (dhMain.PlayerName.Trim() == "Bot") return 16;

                        return 1;
                    }
                case 2:
                    {
                        // Return 20
                        if (dhMain.Index < 0) return 20;
                        return 1;
                    }
                case 3:
                    break;
                case 4:
                    {
                        // Return 30
                        if (dhMain.PlayerName.Trim().Length == 0) return 30;
                        // Return 31
                        if (dhMain.PlayerName.Length > 32) return 31;
                        // Return 32
                        if (dhMain.FirstName.Length > 32) return 32;
                        // Return 33
                        if (dhMain.LastName.Length > 32) return 33;
                        // Return 34
                        if (dhMain.Country.Length > 32) return 34;

                        return 1;
                    }
            }

            return 1;
        }
        public static int GetCountPlayer()
        {
            var dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D);
            var header = dhPlayer.HeaderLoad();

            return header.CountAllPlayer;
        }
        public static int GetIndexFromPlayerName(string PlayerName)
        {
            if (PlayerName.TrimEnd() == "Bot")
                return 0;

            int CountPlayer = GetCountPlayer();
            Main[] main = new DatahandlingPlayer(Configuration.FilePathDDDP3D).MainLoad();

            for (int i = 0; i < CountPlayer; i++)
                if (main[i].PlayerName.TrimEnd() == PlayerName)
                    return main[i].Index;

            return -1;
        }
        public static string GetPlayerNameFromIndex(int PlayerIndex)
        {
            int CountPlayer = GetCountPlayer();
            Main[] main = new DatahandlingPlayer(Configuration.FilePathDDDP3D).MainLoad();

            for (int i = 0; i < CountPlayer; i++)
                if (main[i].Index == PlayerIndex)
                    return main[i].PlayerName.TrimEnd();

            return null;
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
                    Datahandling.CounterInstances--;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DatahandlingPlayer()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        // This code added to correctly implement the disposable pattern.
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
        #region class library
        public struct Header
        {
            public int CountAllBytes;
            public int CountMaxPlayerIndex;
            public int CountMainBytes;
            public int CountPlayer;
            public int CountAllPlayer;
        }
        public struct Main
        {
            public int Index;
            public string PlayerName;
            public string FirstName;
            public string LastName;
            public string Country;
            public int BirthYear;
            public int BirthMonth;
            public int BirthDay;
        }
        #endregion
    }
    class DatahandlingGame : Datahandling, IDisposable
    {
        #region constructor
        public DatahandlingGame()
        {
        }
        public DatahandlingGame(string dataPath) : base(dataPath)
        {
            CheckInitialized = false;
        }
        public DatahandlingGame(string dataPath, bool initializeDataStatistics) : base(dataPath)
        {
            if (initializeDataStatistics)
                InitializeDataStatistics();

            CheckInitialized = false;
        }
        public DatahandlingGame(string dataPath, MainStruct structMain) : base(dataPath)
        {
            CheckInitialized = false;
            StructMain = structMain;
        }
        public DatahandlingGame(string dataPath, HeaderStruct structHeader) : base(dataPath)
        {
            CheckInitialized = false;
            StructHeader = structHeader;
        }
        public DatahandlingGame(string dataPath, HeaderStruct structHeader, MainStruct structMain) : base(dataPath)
        {
            CheckInitialized = false;
            StructHeader = structHeader;
            StructMain = structMain;
        }
        #endregion
        #region properties, fields, constants
        // Properties
        private bool _CheckInitialized { get; set; }
        public bool CheckInitialized
        {
            get { return _CheckInitialized; }
            set
            {
                if (CheckFileInitialized() == false) InitializeHeader();
                _CheckInitialized = true;
            }
        }

        private HeaderStruct _StructHeader { get; set; }
        public HeaderStruct StructHeader
        {
            get { return _StructHeader; }
            set { _StructHeader = value; }
        }

        private MainStruct _StructMain { get; set; }
        public MainStruct StructMain
        {
            get { return _StructMain; }
            set { _StructMain = value; }
        }

        // constants
        private const int CountAllBytesMain = 264;
        private const int CountAllBytesMainGameStatistic = 63;
        private const int CountAllBytesMainLegs = 53;
        private const int CountAllBytesMainPlayer = 148;
        private const int CountAllBytesHeader = 48;

        // fields
        public string NamePlayerDataFile = "Game_$.G3D";
        public HeaderStruct SaveHeader;
        public MainStruct SaveMain;
        private static GameStatisticStruct GameStatistic;
        public GameStatisticStruct GameStat;

        #endregion
        #region static methods
        public static string SetFileNameG3D()
        {
            return "Game_" +
                DateTime.Now.Year.ToString("00") +
                DateTime.Now.Month.ToString("00") +
                DateTime.Now.Day.ToString("00") +
                "_" +
                DateTime.Now.Hour.ToString("00") +
                DateTime.Now.Minute.ToString("00") +
                DateTime.Now.Second.ToString("00") +
                ".G3D";
        }
        #endregion
        #region override methods
        public override bool Save()
        {
            bool checking = HeaderSave(GameStatistic.Header);
            if (!checking) return false;
            return MainSave(GameStatistic.Main);
        }
        public override bool Load()
        {
            try
            {
                var gameStatistic = new GameStatisticStruct();
                gameStatistic.Header = HeaderLoad();
                gameStatistic.Main.MainPlayer = MainPlayerLoad(gameStatistic.Header.CountPlayer);
                gameStatistic.Main.MainLeg = MainLegsLoad(
                    gameStatistic.Header.CountPlayer,
                    gameStatistic.Header.CountLegsPlayed);
                gameStatistic.Main.MainGameStatistic = MainGameStatisticLoad(
                    gameStatistic.Header.CountPlayer,
                    gameStatistic.Header.CountLegsPlayed,
                    gameStatistic.Header.CountThrows);
                GameStatistic = gameStatistic;
                GameStat = gameStatistic;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        private new bool Edit()
        {
            throw new NotImplementedException();
        }
        private new bool Delete(int CountIndex)
        {
            throw new NotImplementedException();
        }
        public virtual string[] GetFileNames()
        {
            var directoryInfo = new DirectoryInfo(DataPath);
            var directoryNamesUnchecked = directoryInfo.GetFiles("*.G3D");
            bool[] checkDirectory = new bool[directoryNamesUnchecked.Length];
            int countChecked = 0;
            int countFilePath = 0;

            for (int i = 0; i < directoryNamesUnchecked.Length; i++)
                if (CheckHeaderLoad(directoryNamesUnchecked[i].FullName))
                {
                    checkDirectory[i] = true;
                    countChecked++;
                }

            string[] filePath = new string[countChecked];

            for (int i = 0; i < directoryNamesUnchecked.Length; i++)
                if (checkDirectory[i])
                {
                    filePath[countFilePath] = directoryNamesUnchecked[i].Name.
                        Remove(directoryNamesUnchecked[i].Name.Length - 4);
                    countFilePath++;
                }

            Array.Reverse(filePath);

            return filePath;
        }

        public virtual bool HeaderSave(HeaderStruct header)
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(header.CountAllBytes);
                        bw.Write(header.CountHeaderBytes);
                        bw.Write(header.CountMainBytes);
                        bw.Write(header.CountPlayer);
                        bw.Write(header.CountSets);
                        bw.Write(header.CountLegs);
                        bw.Write(header.CountLegsPlayed);
                        bw.Write(header.CountThrows);
                        bw.Write(header.GameStarted);
                        bw.Write(header.GameEnded);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool HeaderSave()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(SaveHeader.CountAllBytes);
                        bw.Write(SaveHeader.CountHeaderBytes);
                        bw.Write(SaveHeader.CountMainBytes);
                        bw.Write(SaveHeader.CountPlayer);
                        bw.Write(SaveHeader.CountSets);
                        bw.Write(SaveHeader.CountLegs);
                        bw.Write(SaveHeader.CountLegsPlayed);
                        bw.Write(SaveHeader.CountThrows);
                        bw.Write(SaveHeader.GameStarted);
                        bw.Write(SaveHeader.GameEnded);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual bool MainSave(MainStruct main)
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        int CounterPlayer = main.MainPlayer.Length;
                        int CounterLegs = main.MainLeg.Length;
                        int CounterGameStatistic = main.MainGameStatistic.Length;

                        bw.BaseStream.Seek(0, SeekOrigin.End);
                        for (int i = 0; i < CounterPlayer; i++)
                        {
                            bw.Write(main.MainPlayer[i].Index);
                            bw.Write(main.MainPlayer[i].PlayerName);
                            bw.Write(main.MainPlayer[i].FirstName);
                            bw.Write(main.MainPlayer[i].LastName);
                            bw.Write(main.MainPlayer[i].Country);
                            bw.Write(main.MainPlayer[i].BirthDay);
                            bw.Write(main.MainPlayer[i].BirthMonth);
                            bw.Write(main.MainPlayer[i].BirthYear);
                        }
                        for (int i = 0; i < CounterLegs; i++)
                        {
                            bw.Write(main.MainLeg[i].CountPlayerIndex);
                            bw.Write(main.MainLeg[i].PlayerName);
                            bw.Write(main.MainLeg[i].DateTime);
                            bw.Write(main.MainLeg[i].CountFinishScore);
                            bw.Write(main.MainLeg[i].CountLegWinnerThrows);
                        }
                        for (int i = 0; i < CounterGameStatistic; i++)
                        {
                            bw.Write(main.MainGameStatistic[i].CountIndexThrow);
                            bw.Write(main.MainGameStatistic[i].CountIndexPlayer);
                            bw.Write(main.MainGameStatistic[i].PlayerName);
                            bw.Write(main.MainGameStatistic[i].CountRounds);
                            bw.Write(main.MainGameStatistic[i].CountScore);
                            bw.Write(main.MainGameStatistic[i].IsScoreOverthrown);
                            bw.Write(main.MainGameStatistic[i].CountFinish);
                            bw.Write(main.MainGameStatistic[i].IsFinishPossible);
                            bw.Write(main.MainGameStatistic[i].CountDateTime);
                        }
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool MainSave()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        int CounterPlayer = SaveMain.MainPlayer.Length;
                        int CounterLegs = SaveMain.MainLeg.Length;
                        int CounterGameStatistic = SaveMain.MainGameStatistic.Length;

                        bw.BaseStream.Seek(0, SeekOrigin.End);
                        for (int i = 0; i < CounterPlayer; i++)
                        {
                            bw.Write(SaveMain.MainPlayer[i].Index);
                            bw.Write(SaveMain.MainPlayer[i].PlayerName);
                            bw.Write(SaveMain.MainPlayer[i].FirstName);
                            bw.Write(SaveMain.MainPlayer[i].LastName);
                            bw.Write(SaveMain.MainPlayer[i].Country);
                            bw.Write(SaveMain.MainPlayer[i].BirthDay);
                            bw.Write(SaveMain.MainPlayer[i].BirthMonth);
                            bw.Write(SaveMain.MainPlayer[i].BirthYear);
                        }
                        for (int i = 0; i < CounterLegs; i++)
                        {
                            bw.Write(SaveMain.MainLeg[i].CountPlayerIndex);
                            bw.Write(SaveMain.MainLeg[i].PlayerName);
                            bw.Write(SaveMain.MainLeg[i].DateTime);
                            bw.Write(SaveMain.MainLeg[i].CountFinishScore);
                            bw.Write(SaveMain.MainLeg[i].CountLegWinnerThrows);
                        }
                        for (int i = 0; i < CounterGameStatistic; i++)
                        {
                            bw.Write(SaveMain.MainGameStatistic[i].CountIndexThrow);
                            bw.Write(SaveMain.MainGameStatistic[i].CountIndexPlayer);
                            bw.Write(SaveMain.MainGameStatistic[i].PlayerName);
                            bw.Write(SaveMain.MainGameStatistic[i].CountRounds);
                            bw.Write(SaveMain.MainGameStatistic[i].CountScore);
                            bw.Write(SaveMain.MainGameStatistic[i].IsScoreOverthrown);
                            bw.Write(SaveMain.MainGameStatistic[i].CountFinish);
                            bw.Write(SaveMain.MainGameStatistic[i].IsFinishPossible);
                            bw.Write(SaveMain.MainGameStatistic[i].CountDateTime);
                        }
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }

        public new HeaderStruct HeaderLoad()
        {
            HeaderStruct header = new HeaderStruct();
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        header.CountAllBytes = br.ReadInt32();
                        header.CountHeaderBytes = br.ReadInt32();
                        header.CountMainBytes = br.ReadInt32();
                        header.CountPlayer = br.ReadInt32();
                        header.CountSets = br.ReadInt32();
                        header.CountLegs = br.ReadInt32();
                        header.CountLegsPlayed = br.ReadInt32();
                        header.CountThrows = br.ReadInt32();
                        header.GameStarted = br.ReadInt64();
                        header.GameEnded = br.ReadInt64();
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return header;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return header;
            }
        }
        public MainPlayerStruct[] MainPlayerLoad(int countPlayer)
        {
            MainPlayerStruct[] mainPlayer = new MainPlayerStruct[countPlayer];
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(CountAllBytesHeader, SeekOrigin.Begin);
                        for (int i = 0; i < countPlayer; i++)
                        {
                            mainPlayer[i].Index = br.ReadInt32();
                            mainPlayer[i].PlayerName = br.ReadString();
                            mainPlayer[i].FirstName = br.ReadString();
                            mainPlayer[i].LastName = br.ReadString();
                            mainPlayer[i].Country = br.ReadString();
                            mainPlayer[i].BirthDay = br.ReadInt32();
                            mainPlayer[i].BirthMonth = br.ReadInt32();
                            mainPlayer[i].BirthYear = br.ReadInt32();
                        }
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return mainPlayer;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }
        public MainLegsStruct[] MainLegsLoad(int countPlayer, int countLegs)
        {
            MainLegsStruct[] mainLegs = new MainLegsStruct[countLegs];
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        int seek = CountAllBytesHeader + countPlayer * CountAllBytesMainPlayer;
                        br.BaseStream.Seek(seek, SeekOrigin.Begin);
                        for (int i = 0; i < countLegs; i++)
                        {
                            mainLegs[i].CountPlayerIndex = br.ReadInt32();
                            mainLegs[i].PlayerName = br.ReadString();
                            mainLegs[i].DateTime = br.ReadInt64();
                            mainLegs[i].CountFinishScore = br.ReadInt32();
                            mainLegs[i].CountLegWinnerThrows = br.ReadInt32();
                        }
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return mainLegs;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }
        public MainGameStatisticStruct[] MainGameStatisticLoad(int countPlayer, int countLegs, int countThrows)
        {
            MainGameStatisticStruct[] mainGameStatistic = new MainGameStatisticStruct[countThrows];
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        int seek = CountBytesHeader() +
                            CountBytesMainPlayer(countPlayer) +
                            CountBytesMainLegs(countLegs);
                        br.BaseStream.Seek(seek, SeekOrigin.Begin);
                        for (int i = 0; i < countThrows; i++)
                        {
                            mainGameStatistic[i].CountIndexThrow = br.ReadInt32();
                            mainGameStatistic[i].CountIndexPlayer = br.ReadInt32();
                            mainGameStatistic[i].PlayerName = br.ReadString();
                            mainGameStatistic[i].CountRounds = br.ReadInt32();
                            mainGameStatistic[i].CountScore = br.ReadInt32();
                            mainGameStatistic[i].IsScoreOverthrown = br.ReadBoolean();
                            mainGameStatistic[i].CountFinish = br.ReadInt32();
                            mainGameStatistic[i].IsFinishPossible = br.ReadBoolean();
                            mainGameStatistic[i].CountDateTime = br.ReadInt64();
                        }
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return mainGameStatistic;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }
        #endregion
        #region help methods
        public virtual bool CheckHeaderLoad(string pathFile)
        {
            HeaderStruct header = new HeaderStruct();
            try
            {
                using (FileStream fs = new FileStream(pathFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        header.CountAllBytes = br.ReadInt32();
                        header.CountHeaderBytes = br.ReadInt32();
                        header.CountMainBytes = br.ReadInt32();
                        header.CountPlayer = br.ReadInt32();
                        header.CountSets = br.ReadInt32();
                        header.CountLegs = br.ReadInt32();
                        header.CountLegsPlayed = br.ReadInt32();
                        header.CountThrows = br.ReadInt32();
                        header.GameStarted = br.ReadInt64();
                        header.GameEnded = br.ReadInt64();
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual bool CheckHeaderLoad()
        {
            HeaderStruct header = new HeaderStruct();
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        header.CountAllBytes = br.ReadInt32();
                        header.CountHeaderBytes = br.ReadInt32();
                        header.CountMainBytes = br.ReadInt32();
                        header.CountPlayer = br.ReadInt32();
                        header.CountSets = br.ReadInt32();
                        header.CountLegs = br.ReadInt32();
                        header.CountLegsPlayed = br.ReadInt32();
                        header.CountThrows = br.ReadInt32();
                        header.GameStarted = br.ReadInt64();
                        header.GameEnded = br.ReadInt64();
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }

        public bool InitializeHeader()
        {
            try
            {
                if (Directory.Exists(DataPath))
                    return true;
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(16);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public bool InitializeDataStatistics()
        {
            try
            {
                #region set header
                GameStatistic.Header.CountAllBytes = CountBytesAll(Game.MainProperties.CountPlayer,
                    Game.GameProperties.PlayerWin.Count, Game.GameCounter.CountAllThrowLast);
                GameStatistic.Header.CountHeaderBytes = CountBytesHeader();
                GameStatistic.Header.CountMainBytes = CountBytesMain(Game.MainProperties.CountPlayer,
                    Game.GameProperties.PlayerWin.Count, Game.GameCounter.CountAllThrowLast);
                GameStatistic.Header.CountPlayer = Game.MainProperties.CountPlayer;
                GameStatistic.Header.CountSets = Game.MainProperties.Sets;
                GameStatistic.Header.CountLegs = Game.MainProperties.Legs;
                GameStatistic.Header.CountLegsPlayed = Game.GameProperties.PlayerWin.Count;
                GameStatistic.Header.CountThrows = Game.GameCounter.CountAllThrowLast;
                GameStatistic.Header.GameStarted = Game.MainProperties.DTGameStarted;
                GameStatistic.Header.GameEnded = DateTime.Now.Ticks;
                #endregion
                #region set main player
                var dhMainPlayer = new DatahandlingGame.MainPlayerStruct[GameStatistic.Header.CountPlayer];
                var dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D);
                dhPlayer.Load();

                for (int i = 0; i < Game.MainProperties.CountPlayer; i++)
                {
                    for (int j = 0; j < dhPlayer.LoadAllMain.Length; j++)
                    {
                        if (Game.MainProperties.Players[i].PlayerIndex == dhPlayer.LoadAllMain[j].Index)
                        {
                            dhMainPlayer[i].Index = dhPlayer.LoadAllMain[j].Index;
                            dhMainPlayer[i].PlayerName = dhPlayer.LoadAllMain[j].PlayerName;
                            dhMainPlayer[i].FirstName = dhPlayer.LoadAllMain[j].FirstName;
                            dhMainPlayer[i].LastName = dhPlayer.LoadAllMain[j].LastName;
                            dhMainPlayer[i].Country = dhPlayer.LoadAllMain[j].Country;
                            dhMainPlayer[i].BirthDay = dhPlayer.LoadAllMain[j].BirthDay;
                            dhMainPlayer[i].BirthMonth = dhPlayer.LoadAllMain[j].BirthMonth;
                            dhMainPlayer[i].BirthYear = dhPlayer.LoadAllMain[j].BirthYear;
                            break;
                        }
                    }
                }
                GameStatistic.Main.MainPlayer = dhMainPlayer;
                dhPlayer.Dispose();
                #endregion
                #region set main leg
                var dhMainLeg = new DatahandlingGame.MainLegsStruct[GameStatistic.Header.CountLegsPlayed];
                for (int i = 0; i < Game.GameProperties.PlayerWin.Count; i++)
                {
                    dhMainLeg[i].CountPlayerIndex = Game.GameProperties.PlayerWin[i].Index;
                    dhMainLeg[i].PlayerName =
                        Datahandling.SetStringTo32Byte(Game.GameProperties.PlayerWin[i].Name);
                    dhMainLeg[i].DateTime = Game.GameProperties.PlayerWin[i].DateTime;
                    dhMainLeg[i].CountFinishScore = Game.GameProperties.PlayerWin[i].FinishScore;
                    dhMainLeg[i].CountLegWinnerThrows = Game.GameProperties.PlayerWin[i].WinnerThrows;
                }
                GameStatistic.Main.MainLeg = dhMainLeg;
                #endregion
                #region set main game data
                var dhMainGameStat = new DatahandlingGame.MainGameStatisticStruct[GameStatistic.Header.CountThrows];
                for (int i = 0; i < Game.GameProperties.AllScore.Count; i++)
                {
                    dhMainGameStat[i].CountIndexThrow = i + 1;
                    dhMainGameStat[i].CountIndexPlayer = Game.GameProperties.AllPlayerIndex[i];
                    dhMainGameStat[i].PlayerName = Datahandling.SetStringTo32Byte(Game.GameProperties.AllPlayerName[i]);
                    dhMainGameStat[i].CountRounds = Game.GameProperties.AllRounds[i];
                    dhMainGameStat[i].CountScore = Game.GameProperties.AllScore[i];
                    dhMainGameStat[i].IsScoreOverthrown = Game.GameProperties.AllScoreOverthrown[i];
                    dhMainGameStat[i].CountFinish = Game.GameProperties.AllFinish[i];
                    dhMainGameStat[i].IsFinishPossible = Game.GameProperties.AllFinishPossible[i];
                    dhMainGameStat[i].CountDateTime = Game.GameProperties.AllDateTime[i];
                }
                GameStatistic.Main.MainGameStatistic = dhMainGameStat;
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <param name="countPlayer"></param>
        /// <param name="countLegs"></param>
        /// <param name="countThrows"></param>
        /// <returns>All bytes</returns>
        public int CountBytesAll(int countPlayer, int countLegs, int countThrows)
        {
            return CountBytesHeader() +
                CountBytesMainPlayer(countPlayer) +
                CountBytesMainLegs(countLegs) +
                CountBytesMainGameData(countThrows);
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <returns>Header bytes</returns>
        private int CountBytesHeader()
        {
            return CountAllBytesHeader;
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <param name="countPlayer"></param>
        /// <param name="countLegs"></param>
        /// <param name="countThrows"></param>
        /// <returns>Main bytes</returns>
        public int CountBytesMain(int countPlayer, int countLegs, int countThrows)
        {
            return CountBytesMainPlayer(countPlayer) +
                CountBytesMainLegs(countLegs) +
                CountBytesMainGameData(countThrows);
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <param name="countPlayer"></param>
        /// <returns>Main player data bytes</returns>
        private int CountBytesMainPlayer(int countPlayer)
        {
            return countPlayer * CountAllBytesMainPlayer;
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <param name="countLegs"></param>
        /// <returns>Main leg bytes</returns>
        private int CountBytesMainLegs(int countLegs)
        {
            return countLegs * CountAllBytesMainLegs;
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <param name="countLegs"></param>
        /// <returns>Main game data bytes</returns>
        private int CountBytesMainGameData(int countThrows)
        {
            return countThrows * CountAllBytesMainGameStatistic;
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
                GameStatistic.Main.MainGameStatistic = null;
                GameStatistic.Main.MainLeg = null;
                GameStatistic.Main.MainPlayer = null;
                disposedValue = true;
                GC.Collect();
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DatahandlingGame()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
        #region class library
        public struct GameStatisticStruct
        {
            public HeaderStruct Header;
            public MainStruct Main;
        }

        public struct HeaderStruct
        {
            public int CountAllBytes;
            public int CountHeaderBytes;
            public int CountMainBytes;
            public int CountPlayer;
            public int CountSets;
            public int CountLegs;
            public int CountLegsPlayed;
            public int CountThrows;
            public long GameStarted;
            public long GameEnded;
        }
        public struct MainStruct
        {
            public MainPlayerStruct[] MainPlayer;
            public MainLegsStruct[] MainLeg;
            public MainGameStatisticStruct[] MainGameStatistic;
        }

        public struct MainPlayerStruct
        {
            public int Index { get; set; }
            public string PlayerName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Country { get; set; }
            public int BirthYear { get; set; }
            public int BirthMonth { get; set; }
            public int BirthDay { get; set; }
        }
        public struct MainLegsStruct
        {
            public int CountPlayerIndex { get; set; }
            public string PlayerName { get; set; }
            public long DateTime { get; set; }
            public int CountFinishScore { get; set; }
            public int CountLegWinnerThrows { get; set; }
        }
        public struct MainGameStatisticStruct
        {
            public int CountIndexThrow { get; set; }
            public int CountIndexPlayer { get; set; }
            public string PlayerName { get; set; }
            public int CountRounds { get; set; }
            public int CountScore { get; set; }
            public bool IsScoreOverthrown { get; set; }
            public int CountFinish { get; set; }
            public bool IsFinishPossible { get; set; }
            public long CountDateTime { get; set; }
        }
        #endregion
    }
    class DatahandlingPlayerStat : Datahandling, IDisposable
    {
        #region constructor
        public DatahandlingPlayerStat()
        {
        }
        public DatahandlingPlayerStat(string dataPath) : base(dataPath)
        {
            CheckInitialized = false;
        }
        public DatahandlingPlayerStat(string dataPath, int playerIndex) : base(dataPath)
        {
            PlayerIndex = playerIndex;
            CheckInitialized = false;
        }
        public DatahandlingPlayerStat(string dataPath, MainStruct structMain) : base(dataPath)
        {
            CheckInitialized = false;
            StructMain = structMain;
        }
        public DatahandlingPlayerStat(string dataPath, HeaderStruct structHeader) : base(dataPath)
        {
            CheckInitialized = false;
            StructHeader = structHeader;
        }
        public DatahandlingPlayerStat(string dataPath, HeaderStruct structHeader, MainStruct structMain) : base(dataPath)
        {
            CheckInitialized = false;
            StructHeader = structHeader;
            StructMain = structMain;
        }
        #endregion
        #region properties, fields, constants
        // Properties
        private bool _CheckInitialized { get; set; }
        public bool CheckInitialized
        {
            get { return _CheckInitialized; }
            set
            {
                if (CheckFileInitialized() == false) InitializeHeader();
                _CheckInitialized = true;
            }
        }

        private int _PlayerIndex { get; set; }
        public int PlayerIndex
        {
            get { return _PlayerIndex; }
            set { _PlayerIndex = value; }
        }

        private HeaderStruct _StructHeader { get; set; }
        public HeaderStruct StructHeader
        {
            get { return _StructHeader; }
            set { _StructHeader = value; }
        }

        private MainStruct _StructMain { get; set; }
        public MainStruct StructMain
        {
            get { return _StructMain; }
            set { _StructMain = value; }
        }

        private MainStruct[] _StructMainAll { get; set; }
        public MainStruct[] StructMainAll
        {
            get { return _StructMainAll; }
            set { _StructMainAll = value; }
        }

        // constants
        private const int CountAllBytes = 93;
        private const int CountAllBytesMain = 77;
        private const int CountAllBytesHeader = 16;
        public const string NamePlayerDataFile = "PlayerStat.S3D";

        // fields
        public HeaderStruct SaveHeader;
        public MainStruct SaveMain;
        public MainStruct[] SaveMainAll;
        public static PlayerStatisticStruct PlayerStatistic;

        #endregion
        #region static methods
        #endregion
        #region override methods
        public override bool Save()
        {
            try
            {
                bool checking = HeaderSave(SaveHeader);
                if (!checking) return false;
                return MainSave(SaveMain);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual bool SaveAll()
        {
            try
            {
                bool checking = HeaderSave(SaveHeader);
                if (!checking) return false;
                return MainSaveAll(SaveMainAll);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool Load()
        {
            try
            {
                var playerStatistic = new PlayerStatisticStruct();
                playerStatistic.Header = HeaderLoad();
                playerStatistic.Main = MainLoad(playerStatistic.Header.CountPlayer);
                PlayerStatistic = playerStatistic;
                StructHeader = PlayerStatistic.Header;
                StructMainAll = PlayerStatistic.Main;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual bool Edit(int playerIndex, MainStruct playerEdit)
        {
            try
            {
                Load();
                SaveMainAll = StructMainAll;
                for (int i = 0; i < PlayerStatistic.Header.CountPlayer; i++)
                {
                    if (PlayerStatistic.Main[i].PlayerIndex == playerIndex)
                    {
                        SaveMainAll[i].TotalGames += playerEdit.TotalGames;
                        SaveMainAll[i].TotalLegsWin += playerEdit.TotalLegsWin;
                        SaveMainAll[i].TotalSetsWin += playerEdit.TotalSetsWin;
                        SaveMainAll[i].TotalWin += playerEdit.TotalWin;
                        SaveMainAll[i].TotalLoss += playerEdit.TotalLoss;
                        SaveMainAll[i].TotalGameTime += playerEdit.TotalGameTime;
                        SaveMainAll[i].TotalThrowTime += playerEdit.TotalThrowTime;
                        SaveMainAll[i].TotalPoints += playerEdit.TotalPoints;
                        SaveMainAll[i].TotalAvgPosition += playerEdit.TotalAvgPosition;
                    }
                }
                SaveAll();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual bool EditPlayerName(int playerIndex)
        {
            try
            {
                Load();
                SaveHeader = PlayerStatistic.Header;
                SaveMainAll = PlayerStatistic.Main;
                for (int i = 0; i < PlayerStatistic.Header.CountPlayer; i++)
                {
                    if (PlayerStatistic.Main[i].PlayerIndex == playerIndex)
                    {
                        SaveMainAll[i].PlayerName =
                            SetStringTo32Byte(DatahandlingPlayer.GetPlayerNameFromIndex(playerIndex));
                        break;
                    }
                }
                SaveAll();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool Delete(int playerIndex)
        {
            try
            {
                Load();
                SaveHeader = StructHeader;
                SaveMainAll = StructMainAll;
                for (int i = 0; i < PlayerStatistic.Header.CountPlayer; i++)
                    if (SaveMainAll[i].PlayerIndex == playerIndex)
                    {
                        SaveMainAll[i].PlayerIndex = -1;
                        break;
                    }

                SaveAll();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual MainStruct GetPlayerStatistic(int playerIndex)
        {
            MainStruct mainStruct = new MainStruct();

            Load();
            for (int i = 0; i < StructMainAll.Length; i++)
                if (StructMainAll[i].PlayerIndex == playerIndex)
                {
                    mainStruct = StructMainAll[i];
                    return mainStruct;
                }
            return mainStruct;
        }

        public virtual bool HeaderSave(HeaderStruct header)
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(header.CountAllBytes);
                        bw.Write(header.CountHeaderBytes);
                        bw.Write(header.CountMainBytes);
                        bw.Write(header.CountPlayer);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool HeaderSave()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(SaveHeader.CountAllBytes);
                        bw.Write(SaveHeader.CountHeaderBytes);
                        bw.Write(SaveHeader.CountMainBytes);
                        bw.Write(SaveHeader.CountPlayer);
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public virtual bool MainSave(MainStruct main)
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.End);

                        bw.Write(main.PlayerIndex);
                        bw.Write(main.PlayerName);
                        bw.Write(main.TotalGames);
                        bw.Write(main.TotalLegsWin);
                        bw.Write(main.TotalSetsWin);
                        bw.Write(main.TotalWin);
                        bw.Write(main.TotalLoss);
                        bw.Write(main.TotalGameTime);
                        bw.Write(main.TotalThrowTime);
                        bw.Write(main.TotalThrows);
                        bw.Write(main.TotalPoints);
                        bw.Write(main.TotalAvgPosition);

                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public override bool MainSave()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(0, SeekOrigin.End);

                        bw.Write(SaveMain.PlayerIndex);
                        bw.Write(SaveMain.PlayerName);
                        bw.Write(SaveMain.TotalGames);
                        bw.Write(SaveMain.TotalLegsWin);
                        bw.Write(SaveMain.TotalSetsWin);
                        bw.Write(SaveMain.TotalWin);
                        bw.Write(SaveMain.TotalLoss);
                        bw.Write(SaveMain.TotalGameTime);
                        bw.Write(SaveMain.TotalThrowTime);
                        bw.Write(SaveMain.TotalThrows);
                        bw.Write(SaveMain.TotalPoints);
                        bw.Write(SaveMain.TotalAvgPosition);

                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public bool MainSaveAll(MainStruct[] mainAll)
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(CountAllBytesHeader, SeekOrigin.Begin);
                        for (int i = 0; i < SaveMainAll.Length; i++)
                        {
                            bw.Write(mainAll[i].PlayerIndex);
                            bw.Write(mainAll[i].PlayerName);
                            bw.Write(mainAll[i].TotalGames);
                            bw.Write(mainAll[i].TotalLegsWin);
                            bw.Write(mainAll[i].TotalSetsWin);
                            bw.Write(mainAll[i].TotalWin);
                            bw.Write(mainAll[i].TotalLoss);
                            bw.Write(mainAll[i].TotalGameTime);
                            bw.Write(mainAll[i].TotalThrowTime);
                            bw.Write(mainAll[i].TotalThrows);
                            bw.Write(mainAll[i].TotalPoints);
                            bw.Write(mainAll[i].TotalAvgPosition);
                        }
                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public bool MainSaveAll()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.BaseStream.Seek(CountAllBytesHeader, SeekOrigin.Begin);

                        for (int i = 0; i < SaveMainAll.Length; i++)
                        {
                            bw.Write(SaveMainAll[i].PlayerIndex);
                            bw.Write(SaveMainAll[i].PlayerName);
                            bw.Write(SaveMainAll[i].TotalGames);
                            bw.Write(SaveMainAll[i].TotalLegsWin);
                            bw.Write(SaveMainAll[i].TotalSetsWin);
                            bw.Write(SaveMainAll[i].TotalWin);
                            bw.Write(SaveMainAll[i].TotalLoss);
                            bw.Write(SaveMainAll[i].TotalGameTime);
                            bw.Write(SaveMainAll[i].TotalThrowTime);
                            bw.Write(SaveMainAll[i].TotalThrows);
                            bw.Write(SaveMainAll[i].TotalPoints);
                            bw.Write(SaveMainAll[i].TotalAvgPosition);
                        }

                        bw.Dispose();
                        fs.Dispose();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }

        public new HeaderStruct HeaderLoad()
        {
            HeaderStruct header = new HeaderStruct();
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        header.CountAllBytes = br.ReadInt32();
                        header.CountHeaderBytes = br.ReadInt32();
                        header.CountMainBytes = br.ReadInt32();
                        header.CountPlayer = br.ReadInt32();
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return header;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return header;
            }
        }
        public MainStruct[] MainLoad(int countPlayer)
        {
            MainStruct[] mainPlayerStat = new MainStruct[countPlayer];
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        br.BaseStream.Seek(CountAllBytesHeader, SeekOrigin.Begin);
                        for (int i = 0; i < countPlayer; i++)
                        {
                            mainPlayerStat[i].PlayerIndex = br.ReadInt32();
                            mainPlayerStat[i].PlayerName = br.ReadString();
                            mainPlayerStat[i].TotalGames = br.ReadInt32();
                            mainPlayerStat[i].TotalLegsWin = br.ReadInt32();
                            mainPlayerStat[i].TotalSetsWin = br.ReadInt32();
                            mainPlayerStat[i].TotalWin = br.ReadInt32();
                            mainPlayerStat[i].TotalLoss = br.ReadInt32();
                            mainPlayerStat[i].TotalGameTime = br.ReadInt32();
                            mainPlayerStat[i].TotalThrowTime = br.ReadInt32();
                            mainPlayerStat[i].TotalThrows = br.ReadInt32();
                            mainPlayerStat[i].TotalPoints = br.ReadInt32();
                            mainPlayerStat[i].TotalAvgPosition = br.ReadSingle();

                        }
                        br.Dispose();
                        fs.Dispose();
                    }
                }
                return mainPlayerStat;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }
        #endregion
        #region help methods
        public bool InitializeHeader()
        {
            try
            {
                using (FileStream fs = new FileStream(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        float totalAvcPosition = 0;
                        bw.BaseStream.Seek(0, SeekOrigin.Begin);
                        bw.Write(CountAllBytesHeader + CountAllBytesMain);
                        bw.Write(CountAllBytesHeader);
                        bw.Write(CountAllBytesMain);
                        bw.Write(1);
                        bw.Write(0);
                        bw.Write(SetStringTo32Byte("Bot"));
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(0);
                        bw.Write(totalAvcPosition);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public bool InitializeNewPlayerDS()
        {
            try
            {
                #region load all
                Load();
                SaveHeader = PlayerStatistic.Header;
                SaveMainAll = PlayerStatistic.Main;
                #endregion
                #region set header
                if (PlayerStatistic.Header.CountPlayer == 0)
                {
                    SaveHeader.CountAllBytes = 93;
                    SaveHeader.CountHeaderBytes = 16;
                    SaveHeader.CountMainBytes = 77;
                    SaveHeader.CountPlayer = 1;
                }
                else
                {
                    SaveHeader.CountAllBytes += CountAllBytesMain;
                    SaveHeader.CountHeaderBytes = CountAllBytesHeader;
                    SaveHeader.CountMainBytes += CountAllBytesMain;
                    SaveHeader.CountPlayer += 1;
                }
                #endregion
                #region set main
                SaveMain.PlayerIndex = PlayerIndex;
                SaveMain.PlayerName =
                    SetStringTo32Byte(DatahandlingPlayer.GetPlayerNameFromIndex(PlayerIndex));
                SaveMain.TotalGames = 0;
                SaveMain.TotalLegsWin = 0;
                SaveMain.TotalSetsWin = 0;
                SaveMain.TotalWin = 0;
                SaveMain.TotalLoss = 0;
                SaveMain.TotalGameTime = 0;
                SaveMain.TotalThrowTime = 0;
                SaveMain.TotalThrows = 0;
                SaveMain.TotalPoints = 0;
                SaveMain.TotalAvgPosition = 0;
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
            return true;
        }
        public void InitializeEndGamePlayerDS(Game.GameProp gameProp, Game.MainProp mainProp)
        {
            try
            {
                #region load all
                Load();
                SaveMainAll = PlayerStatistic.Main;
                SaveHeader = PlayerStatistic.Header;
                #endregion
                #region set main
                SetTotalGames(ref SaveMainAll, gameProp.PlayerDataStatistic);
                SetTotalLegsWin(ref SaveMainAll, gameProp.PlayerWin);
                SetTotalSetsWin(ref SaveMainAll, gameProp.PlayerDataStatistic);
                SetTotalWinLoss(ref SaveMainAll, gameProp.PlayerWin, gameProp.PlayerDataStatistic);
                SetTotalGameTime(ref SaveMainAll, gameProp.PlayerDataStatistic, mainProp);
                SetTotalThrowTime(ref SaveMainAll, gameProp, mainProp);
                SetTotalThrows(ref SaveMainAll, gameProp.AllPlayerIndex);
                SetTotalPoints(ref SaveMainAll, gameProp);
                SetTotalAvgPosition(ref SaveMainAll, gameProp);
                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return;
            }
            return;
        }
        private void SetTotalGames(ref MainStruct[] saveMainAll, List<Game.GameProp.PlayerDataStatisticStruct> plDataStat)
        {
            for (int i = 0; i < saveMainAll.Length; i++)
                for (int j = 0; j < plDataStat.Count; j++)
                    if (saveMainAll[i].PlayerIndex == plDataStat[j].CountIndexPlayer)
                    {
                        saveMainAll[i].TotalGames++;
                        break;
                    }
        }
        private void SetTotalLegsWin(ref MainStruct[] saveMainAll, List<Game.GameProp.PlayerWinStruct> rankingStruct)
        {
            for (int i = 0; i < saveMainAll.Length; i++)
                for (int j = 0; j < rankingStruct.Count; j++)
                    if (saveMainAll[i].PlayerIndex == rankingStruct[j].Index)
                    {
                        saveMainAll[i].TotalLegsWin++;
                        break;
                    }
        }
        private void SetTotalSetsWin(ref MainStruct[] saveMainAll, List<Game.GameProp.PlayerDataStatisticStruct> plDataStat)
        {
            for (int i = 0; i < plDataStat.Count; i++)
                for (int j = 0; j < saveMainAll.Length; j++)
                    if (saveMainAll[j].PlayerIndex == plDataStat[i].CountIndexPlayer)
                    {
                        saveMainAll[j].TotalSetsWin += plDataStat[i].CountSetWins;
                        break;
                    }
        }
        private void SetTotalWinLoss(ref MainStruct[] saveMainAll, List<Game.GameProp.PlayerWinStruct> rankingStruct,
            List<Game.GameProp.PlayerDataStatisticStruct> plDataStat)
        {
            for (int i = 0; i < plDataStat.Count; i++)
                for (int j = 0; j < saveMainAll.Length; j++)
                    if (saveMainAll[j].PlayerIndex == plDataStat[i].CountIndexPlayer)
                    {
                        if (rankingStruct[rankingStruct.Count - 1].Index == saveMainAll[j].PlayerIndex)
                        {
                            saveMainAll[j].TotalWin++;
                            break;
                        }
                        else
                        {
                            saveMainAll[j].TotalLoss++;
                            break;
                        }
                    }
        }
        private void SetTotalGameTime(ref MainStruct[] saveMainAll, List<Game.GameProp.PlayerDataStatisticStruct> plDataStat,
            Game.MainProp mainProp)
        {
            for (int i = 0; i < plDataStat.Count; i++)
                for (int j = 0; j < saveMainAll.Length; j++)
                    if (saveMainAll[j].PlayerIndex == plDataStat[i].CountIndexPlayer)
                    {
                        saveMainAll[j].TotalGameTime += (int)Math.Round(TimeSpan.FromTicks(mainProp.DTGameEnded -
                            mainProp.DTGameStarted).TotalSeconds, 0);
                        break;
                    }
        }
        private void SetTotalThrowTime(ref MainStruct[] saveMainAll, Game.GameProp gameProp, Game.MainProp mainProp)
        {
            for (int i = 0; i < gameProp.AllDateTime.Count; i++)
                for (int j = 0; j < saveMainAll.Length; j++)
                    if (gameProp.AllPlayerIndex[i] == saveMainAll[j].PlayerIndex)
                    {
                        if (i == 0)
                            saveMainAll[j].TotalThrowTime +=
                                (int)TimeSpan.FromTicks(gameProp.AllDateTime[i] - mainProp.DTGameStarted).TotalSeconds;
                        if (i < gameProp.AllDateTime.Count && i > 0)
                            saveMainAll[j].TotalThrowTime +=
                                (int)TimeSpan.FromTicks(gameProp.AllDateTime[i] - gameProp.AllDateTime[i - 1]).TotalSeconds;
                        break;
                    }
        }
        private void SetTotalThrows(ref MainStruct[] saveMainAll, List<int> dhGamePropPlayerIndex)
        {
            for (int i = 0; i < dhGamePropPlayerIndex.Count; i++)
                for (int j = 0; j < saveMainAll.Length; j++)
                    if (saveMainAll[j].PlayerIndex == dhGamePropPlayerIndex[i])
                    {
                        saveMainAll[j].TotalThrows++;
                        break;
                    }
        }
        private void SetTotalPoints(ref MainStruct[] saveMainAll, Game.GameProp gameProp)
        {
            for (int i = 0; i < gameProp.AllDateTime.Count; i++)
                for (int j = 0; j < saveMainAll.Length; j++)
                    if (gameProp.AllPlayerIndex[i] == saveMainAll[j].PlayerIndex)
                    {
                        saveMainAll[j].TotalPoints += gameProp.AllScore[i];
                        break;
                    }
        }
        private void SetTotalAvgPosition(ref MainStruct[] saveMainAll, Game.GameProp gameProp)
        {
            for (int i = 0; i < gameProp.PlayerDataStatistic.Count; i++)
                for (int j = 0; j < saveMainAll.Length; j++)
                    if (gameProp.AllPlayerIndex[i] == saveMainAll[j].PlayerIndex)
                    {
                        int countIndexPlayer = 0;
                        for (int k = 0; k < Game.MainProperties.CountPlayer; k++)
                            if (gameProp.PlayerEndRanking[k].Index == saveMainAll[j].PlayerIndex)
                            {
                                countIndexPlayer = k;
                                break;
                            }

                        saveMainAll[j].TotalAvgPosition =
                            ((saveMainAll[j].TotalGames - 1) * saveMainAll[j].TotalAvgPosition
                            + gameProp.PlayerEndRanking[countIndexPlayer].Position)
                            / saveMainAll[j].TotalGames;
                        break;
                    }
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <param name="countPlayer"></param>
        /// <param name="countLegs"></param>
        /// <param name="countThrows"></param>
        /// <returns>All bytes</returns>
        public int CountBytesAll(int countPlayer, int countLegs, int countThrows)
        {
            return CountBytesHeader() +
                CountBytesMain(countPlayer);
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <returns>Header bytes</returns>
        private int CountBytesHeader()
        {
            return CountAllBytesHeader;
        }
        /// <summary>
        /// Count data statistic bytes
        /// </summary>
        /// <param name="countPlayer"></param>
        /// <param name="countLegs"></param>
        /// <param name="countThrows"></param>
        /// <returns>Main bytes</returns>
        public int CountBytesMain(int countPlayer)
        {
            return CountAllBytesMain;
        }
        #endregion
        #region class library
        public struct PlayerStatisticStruct
        {
            public HeaderStruct Header;
            public MainStruct[] Main;
        }

        public struct HeaderStruct
        {
            public int CountAllBytes;
            public int CountHeaderBytes;
            public int CountMainBytes;
            public int CountPlayer;
        }
        public struct MainStruct
        {
            public int PlayerIndex;
            public string PlayerName;
            public int TotalGames;
            public int TotalLegsWin;
            public int TotalSetsWin;
            public int TotalWin;
            public int TotalLoss;
            public int TotalGameTime;
            public int TotalThrowTime;
            public int TotalThrows;
            public int TotalPoints;
            public float TotalAvgPosition;
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
                PlayerStatistic.Main = null;
                disposedValue = true;
            }
        }
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DatahandlingPlayerStat()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
        // This code added to correctly implement the disposable pattern.
        public override void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    class DHPlayerExImport : DatahandlingPlayer
    {
        #region constructor
        public DHPlayerExImport() { }
        #endregion
        #region static properties

        #endregion
        #region class properties

        #endregion
        #region virtual methods

        #endregion
        #region class methods

        #endregion

    }

    /// <summary>
    /// Datahandling WPF
    /// </summary>
    public abstract class DatahandlingWPF
    {
        #region constructor

        #endregion
        #region static properties
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        #endregion
        #region class properties
        protected virtual object _WPFObject { get; set; }
        public virtual object WPFObject { get; set; }
        #endregion
        #region virtual methods

        #endregion
        #region class methods

        #endregion
        #region static methods
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }
        #endregion
    }
    public class DHListBox : DatahandlingWPF, IDisposable
    {
        #region constructor
        public DHListBox() : this(null) { }
        public DHListBox(ListBox listBox)
        {
            ListBox = listBox;
        }
        #endregion
        #region static properties

        #endregion
        #region class properties
        private ListBox _ListBox { get; set; }
        public ListBox ListBox
        {
            get { return _ListBox; }
            set { _ListBox = value; }
        }
        #endregion
        #region methods
        public ListBox SetGameData(string[] gameFileNames)
        {
            for (int i = 0; i < gameFileNames.Length; i++)
                ListBox.Items.Add(gameFileNames[i]);

            return ListBox;
        }
        public ListBox SetGameData(ListBox lb, string[] gameFileNames)
        {
            for (int i = 0; i < gameFileNames.Length; i++)
                lb.Items.Add(gameFileNames[i]);

            return lb;
        }
        public ListBox SetPlayer()
        {
            List<string> items = new List<string>();
            DatahandlingPlayer.Main[] dhPlayer = new DatahandlingPlayer(Configuration.FilePathDDDP3D).MainLoad();
            int CountPlayer = DatahandlingPlayer.GetCountPlayer();

            if (this.ListBox.Items.Count > 0)
                this.ListBox.Items.Clear();

            for (int i = 0; i < CountPlayer; i++)
                if (dhPlayer[i].Index > 0)
                    items.Add(dhPlayer[i].PlayerName.TrimEnd());

            for (int i = 0; i < items.Count; i++)
                this.ListBox.Items.Add((string)items[i]);

            return this.ListBox;
        }
        public ListBox SetPlayer(Game.MainProp.Player[] player)
        {
            List<string> items = new List<string>();
            int CountPlayer = Game.MainProperties.CountPlayer;

            if (this.ListBox.Items.Count > 0)
                this.ListBox.Items.Clear();

            for (int i = 0; i < CountPlayer; i++)
                items.Add(player[i].PlayerName.TrimEnd());

            for (int i = 0; i < items.Count; i++)
                this.ListBox.Items.Add((string)items[i]);

            return this.ListBox;
        }
        public ListBox ChangePlayer()
        {
            try
            {
                int CountPlayer = ListBox.Items.Count;
                int CountIndex = ListBox.SelectedIndex;
                string[] PlayerNames = new string[CountPlayer];
                string PlayerNamesChange; ;
                ListBox lb = new ListBox();

                ListBox.Items.CopyTo(PlayerNames, 0);

                if (CountIndex > 0)
                {
                    PlayerNamesChange = PlayerNames[CountIndex - 1];
                    PlayerNames[CountIndex - 1] = PlayerNames[CountIndex];
                    PlayerNames[CountIndex] = PlayerNamesChange;
                }
                else
                {
                    if (CountIndex == 0)
                    {
                        ListBox.SelectedIndex = -1;
                        return ListBox;
                    }
                    else return null;
                }

                for (int i = 0; i < CountPlayer; i++)
                    lb.Items.Add(PlayerNames[i]);

                return lb;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }
        public ListBox ChangePlayer(ListBox lb)
        {
            try
            {
                int CountPlayer = lb.Items.Count;
                int CountIndex = lb.SelectedIndex;
                string[] PlayerNames = new string[CountPlayer];
                string PlayerNamesChange;

                lb.Items.CopyTo(PlayerNames, 0);

                if (CountIndex > 0)
                {
                    PlayerNamesChange = PlayerNames[CountIndex - 1];
                    PlayerNames[CountIndex - 1] = PlayerNames[CountIndex];
                    PlayerNames[CountIndex] = PlayerNamesChange;
                }
                else
                {
                    if (CountIndex == 0)
                    {
                        lb.SelectedIndex = -1;
                        return lb;
                    }
                    else return null;
                }
                lb.Items.Clear();
                for (int i = 0; i < CountPlayer; i++)
                    lb.Items.Add(PlayerNames[i]);

                return lb;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }
        public ListBox EndGameRanking(List<Game.GameProp.PlayerEndRankingStruct> endRanking)
        {
            for (int i = 0; i < endRanking.Count; i++)
                ListBox.Items.Add((i + 1).ToString() + ". " + endRanking[i].Name);

            return ListBox;
        }
        public ListBox EndGameRanking(ListBox lb, List<Game.GameProp.PlayerEndRankingStruct> endRanking)
        {
            for (int i = 0; i < endRanking.Count; i++)
                lb.Items.Add((i + 1).ToString() + ". " + endRanking[i].Name);

            return lb;
        }
        #endregion
        #region class methods

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
                    _ListBox = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DHListBox()
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
            //GC.SuppressFinalize(this);
        }
        #endregion
    }

    /// <summary>
    /// Game
    /// </summary>
    public class Game : IDisposable
    {
        #region constructor
        public Game() { }
        public Game(MainProp properties)
        {
            MainProperties = properties;
        }
        public Game(ListBox playerListBox)
        {
            PlayerListBox = playerListBox;
        }
        public Game(MainProp properties, ListBox playerListBox)
        {
            MainProperties = properties;
            PlayerListBox = playerListBox;
        }
        #endregion
        #region static properties
        public static MainProp MainProperties;
        public static DisplayProp DisplayProperties;
        public static GameProp GameProperties;
        public static GameCount GameCounter;
        public static DataGrid[] DataGridProp;

        private static bool _CheckFirstExecute { get; set; }
        public static bool CheckFirstExecute
        {
            get { return _CheckFirstExecute; }
            set { _CheckFirstExecute = value; }
        }
        #endregion
        #region class properties
        private ListBox _PlayerListBox { get; set; }
        public ListBox PlayerListBox { get { return _PlayerListBox; } set { _PlayerListBox = value; } }
        #endregion
        #region static methods
        /// <summary>
        /// Check game
        /// </summary>
        /// <param name="action">Case 1: New game / </param>
        /// <returns>1: Checking OK / 10: No player selected / 11: Check player max 10
        /// / 20</returns>
        public static int Check(int action)
        {
            switch (action)
            {
                case 1:
                    {
                        // Return 10
                        int countPlayer = MainProperties.CountPlayer;
                        if (MainProperties.BotCheck) countPlayer--;
                        if (countPlayer < 1) return 10;

                        // Return 11
                        if (MainProperties.PlayersStart.Length > 10) return 11;

                        // Return 12

                        return 1;
                    }
                case 2:
                    {

                        return 1;
                    }
            }
            return 1;
        }
        public static void FirstExecute()
        {

        }
        #endregion
        #region class methods
        // 1. Set first execute
        public bool FirstThrowExecute()
        {
            var countPlayer = PlayerListBox.Items.Count;

            #region set main properties
            MainProp.Player[] mainPropPlayer = new MainProp.Player[countPlayer];

            for (int i = 0; i < countPlayer; i++)
            {
                mainPropPlayer[i].PlayerIndex =
                    DatahandlingPlayer.GetIndexFromPlayerName(PlayerListBox.Items[i].ToString().TrimEnd());
                mainPropPlayer[i].PlayerName =
                    PlayerListBox.Items[i].ToString();
            }

            MainProperties.Players = mainPropPlayer;
            #endregion
            #region set game properties players
            GameProp.PlayerDataStatisticStruct[] playerDataStatistic = new GameProp.PlayerDataStatisticStruct[countPlayer];
            List<GameProp.PlayerDataStatisticStruct> gameProperties = new List<GameProp.PlayerDataStatisticStruct>();

            // Set actual finish
            for (int i = 0; i < countPlayer; i++)
            {
                playerDataStatistic[i].CountIndexPlayer = MainProperties.Players[i].PlayerIndex;
                playerDataStatistic[i].ActualPlayerName = MainProperties.Players[i].PlayerName;
                playerDataStatistic[i].CountActualFinish = MainProperties.Points;
                playerDataStatistic[i].CountFinish = new List<int>() { MainProperties.Points };
                playerDataStatistic[i].CountRound = new List<int>() { 1 };
                gameProperties.Add(playerDataStatistic[i]);
            }

            GameProperties.PlayerDataStatistic = gameProperties;
            #endregion

            return false;
        }

        // 2. Set Counter
        /// <summary>
        /// Set counter when throw
        /// </summary>
        /// <param name="action">Case 1: Update throw / Case 2: Update player finished / Case 3: Reset player finished</param>
        /// <returns>CheckOK = true, CheckError = false</returns>
        public bool CounterSet(int action)
        {
            switch (action)
            {
                case 1:
                    {
                        GameCounter.CountAllThrowLast++;
                        GameCounter.CountAllThrowActual++;
                        GameCounter.CountAllThrowNext++;
                        GameCounter.CountThrowOffsetLast = CountThrow(1);
                        GameCounter.CountThrowLast = CountThrow(4);
                        GameCounter.CountThrowActual = CountThrow(2);
                        GameCounter.CountThrowIndex = CountThrow(5);
                        GameCounter.CountThrowOffsetNext = CountThrow(3);
                        GameCounter.CountThrowUpdateScore = CountThrow(6);
                        GameCounter.CountRoundLast = GameCounter.CountRoundLast + CounterRound();
                        GameCounter.CountRoundActual = GameCounter.CountRoundActual + CounterRound();
                        GameCounter.CountRoundNext = GameCounter.CountRoundNext + CounterRound();
                        GameCounter.CountPlayerIndexLast = PlayerIndexSet(1, 1, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexActual = PlayerIndexSet(1, 2, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexNext = PlayerIndexSet(1, 3, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountThrowUpdateWinner = GameCounter.CountThrowUpdateWinner + CountThrow(7);
                        GameCounter.CountHighestLegs = HighestLegs();
                        GameCounter.CountMaxToReset = MaxToReset();
                        return true;
                    }
                case 2:
                    {
                        GameCounter.CountThrowOffsetLast = CountThrow(1);
                        GameCounter.CountThrowLast = CountThrow(4);
                        GameCounter.CountThrowActual = CountThrow(2);
                        GameCounter.CountThrowIndex = CountThrow(5);
                        GameCounter.CountThrowOffsetNext = CountThrow(3);
                        GameCounter.CountThrowUpdateScore = CountThrow(6);
                        GameCounter.CountRoundLast = GameCounter.CountRoundLast + CounterRound();
                        GameCounter.CountRoundActual = GameCounter.CountRoundActual + CounterRound();
                        GameCounter.CountRoundNext = GameCounter.CountRoundNext + CounterRound();
                        GameCounter.CountPlayerIndexLast = PlayerIndexSet(1, 1, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexActual = PlayerIndexSet(1, 2, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexNext = PlayerIndexSet(1, 3, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountThrowUpdateWinner = GameCounter.CountThrowUpdateWinner + CountThrow(7);
                        GameCounter.CountHighestLegs = HighestLegs();
                        GameCounter.CountMaxToReset = MaxToReset();
                        return true;
                    }
                case 3:
                    {
                        GameCounter.CountAllThrowLast++;
                        GameCounter.CountAllThrowActual++;
                        GameCounter.CountAllThrowNext++;
                        GameCounter.CountLegs++;
                        GameCounter.CountThrowOffsetLast = CountThrow(1);
                        GameCounter.CountThrowLast = CountThrow(4);
                        GameCounter.CountThrowActual = CountThrow(2);
                        GameCounter.CountThrowIndex = CountThrow(5);
                        GameCounter.CountThrowOffsetNext = CountThrow(3);
                        GameCounter.CountThrowUpdateScore = CountThrow(6);
                        GameCounter.CountRoundLast = GameCounter.CountRoundLast + CounterRound();
                        GameCounter.CountRoundActual = GameCounter.CountRoundActual + CounterRound();
                        GameCounter.CountRoundNext = GameCounter.CountRoundNext + CounterRound();
                        GameCounter.CountPlayerIndexLast = PlayerIndexSet(1, 1, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexActual = PlayerIndexSet(1, 2, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexNext = PlayerIndexSet(1, 3, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountThrowUpdateWinner = GameCounter.CountThrowUpdateWinner + CountThrow(7);
                        GameCounter.CountHighestLegs = HighestLegs();
                        GameCounter.CountMaxToReset = MaxToReset();
                        return true;
                    }
                default:
                    return false;
            }

            int CountThrow(int action02)
            {
                switch (action02)
                {
                    case 1:
                        if (GameCounter.CountThrowOffsetLast == MainProperties.CountPlayer - 1) return 0;
                        else return GameCounter.CountThrowOffsetLast + 1;
                    case 2:
                        if (GameCounter.CountThrowActual == MainProperties.CountPlayer) return 1;
                        else return GameCounter.CountThrowActual + 1;
                    case 3:
                        if (GameCounter.CountThrowOffsetNext == MainProperties.CountPlayer + 1) return 2;
                        else return GameCounter.CountThrowOffsetNext + 1;
                    case 4:
                        if (GameCounter.CountThrowLast == MainProperties.CountPlayer - 1) return 0;
                        else return GameCounter.CountThrowLast + 1;
                    case 5:
                        if (GameCounter.CountThrowIndex == MainProperties.CountPlayer - 1) return 0;
                        else return GameCounter.CountThrowIndex + 1;
                    case 6:
                        if (GameCounter.CountThrowUpdateScore == MainProperties.CountPlayer - 1) return 0;
                        else return GameCounter.CountThrowUpdateScore + 1;
                    default:
                        return 0;
                }
            }
            int CounterRound()
            {
                if (GameCounter.CountThrowActual == 1) return 1;
                else return 0;
            }
            int HighestLegs()
            {
                int CountMostLegs = 0;
                for (int i = 0; i < MainProperties.CountPlayer; i++)
                {
                    if (GameProperties.PlayerDataStatistic[i].CountLegWins > CountMostLegs)
                        CountMostLegs = GameProperties.PlayerDataStatistic[i].CountLegWins;
                }
                return CountMostLegs;
            }
            int MaxToReset()
            {
                int CountValue = 0;
                if (GameProperties.PlayerWin != null)
                    CountValue = GameProperties.PlayerWin[GameProperties.PlayerWin.Count - 1].WinnerThrows;
                return CountValue;
            }
        }
        public bool CounterReset()
        {
            GameCounter.CountRoundLast = GameCounter.CountRoundLast - CounterRound();
            GameCounter.CountRoundActual = GameCounter.CountRoundActual - CounterRound();
            GameCounter.CountRoundNext = GameCounter.CountRoundNext - CounterRound();
            GameCounter.CountAllThrowLast = GameCounter.CountAllThrowLast - 1;
            GameCounter.CountAllThrowActual = GameCounter.CountAllThrowActual - 1;
            GameCounter.CountAllThrowNext = GameCounter.CountAllThrowNext - 1;
            GameCounter.CountThrowOffsetLast = CountThrow(1);
            GameCounter.CountThrowLast = CountThrow(4);
            GameCounter.CountThrowActual = CountThrow(2);
            GameCounter.CountThrowIndex = CountThrow(5);
            GameCounter.CountThrowOffsetNext = CountThrow(3);
            GameCounter.CountThrowUpdateScore = CountThrow(6);
            GameCounter.CountPlayerIndexLast = PlayerIndexReset(1, GameCounter.CountThrowOffsetLast);
            GameCounter.CountPlayerIndexActual = PlayerIndexReset(2, GameCounter.CountThrowOffsetLast);
            GameCounter.CountPlayerIndexNext = PlayerIndexReset(3, GameCounter.CountThrowOffsetLast);
            if (GameCounter.CountAllThrowActual <= 2) GameCounter.CountFinishLast = 0;
            else GameCounter.CountFinishLast = GameProperties.AllFinish[GameCounter.CountAllThrowLast - 2];
            if (GameCounter.CountAllThrowActual == 1) GameCounter.CountFinishActual = 0;
            else GameCounter.CountFinishActual = GameProperties.AllFinish[GameCounter.CountAllThrowLast - 1];
            GameCounter.CountFinishNext = GameProperties.AllFinish[GameCounter.CountAllThrowLast];

            ResetLists();

            return true;

            int CounterRound()
            {
                if (GameCounter.CountThrowActual == 1) return 1;
                else return 0;
            }
            int CountThrow(int action02)
            {
                switch (action02)
                {
                    case 1:
                        if (GameCounter.CountThrowOffsetLast == 0) return MainProperties.CountPlayer - 1;
                        else return GameCounter.CountThrowOffsetLast - 1;
                    case 2:
                        if (GameCounter.CountThrowActual == 1) return MainProperties.CountPlayer;
                        else return GameCounter.CountThrowActual - 1;
                    case 3:
                        if (GameCounter.CountThrowOffsetNext == 2) return MainProperties.CountPlayer + 1;
                        else return GameCounter.CountThrowOffsetNext - 1;
                    case 4:
                        if (GameCounter.CountThrowLast == 0) return MainProperties.CountPlayer - 1;
                        else return GameCounter.CountThrowLast - 1;
                    case 5:
                        if (GameCounter.CountThrowIndex == 0) return MainProperties.CountPlayer - 1;
                        else return GameCounter.CountThrowIndex - 1;
                    case 6:
                        if (GameCounter.CountThrowUpdateScore == 0) return MainProperties.CountPlayer - 1;
                        else return GameCounter.CountThrowUpdateScore - 1;
                    default:
                        return 0;
                }
            }
            void ResetLists()
            {
                #region player data statistic
                var plDataStat01 = GameProperties.PlayerDataStatistic;
                var plDataStat02 = new GameProp.PlayerDataStatisticStruct();

                plDataStat02 = plDataStat01[GameCounter.CountThrowLast];
                //= GameProperties.AllScore[GameProperties.AllScore.Count - 1];
                plDataStat02.CountActualFinish += GameProperties.AllScore[GameProperties.AllScore.Count - 1];
                plDataStat02.CountActualScore -= GameProperties.AllScore[GameProperties.AllScore.Count - 1];
                if (plDataStat02.CountThrowIndex.Any()) plDataStat02.CountThrowIndex.RemoveAt(plDataStat02.CountThrowIndex.Count - 1);
                if (plDataStat02.CountScore.Any()) plDataStat02.CountScore.RemoveAt(plDataStat02.CountScore.Count - 1);
                if (plDataStat02.CountFinish.Any()) plDataStat02.CountFinish.RemoveAt(plDataStat02.CountFinish.Count - 1);
                if (plDataStat02.FinishPossible.Any()) plDataStat02.FinishPossible.RemoveAt(plDataStat02.FinishPossible.Count - 1);
                if (plDataStat02.CountRound.Any()) plDataStat02.CountRound.RemoveAt(plDataStat02.CountRound.Count - 1);
                if (plDataStat02.CountDTThrow.Any()) plDataStat02.CountDTThrow.RemoveAt(plDataStat02.CountDTThrow.Count - 1);
                plDataStat01[GameCounter.CountThrowLast] = plDataStat02;
                GameProperties.PlayerDataStatistic = plDataStat01;
                #endregion
                #region all lists
                if (GameProperties.AllScore.Any()) GameProperties.AllScore.RemoveAt(GameProperties.AllScore.Count - 1);
                if (GameProperties.AllRounds.Any()) GameProperties.AllRounds.RemoveAt(GameProperties.AllRounds.Count - 1);
                if (GameProperties.AllFinish.Any()) GameProperties.AllFinish.RemoveAt(GameProperties.AllFinish.Count - 1);
                if (GameProperties.AllFinishPossible.Any()) GameProperties.AllFinishPossible.RemoveAt(GameProperties.AllFinishPossible.Count - 1);
                if (GameProperties.AllDateTime.Any()) GameProperties.AllDateTime.RemoveAt(GameProperties.AllDateTime.Count - 1);
                if (GameProperties.AllPlayerIndex.Any()) GameProperties.AllPlayerIndex.RemoveAt(GameProperties.AllPlayerIndex.Count - 1);
                if (GameProperties.AllPlayerName.Any()) GameProperties.AllPlayerName.RemoveAt(GameProperties.AllPlayerName.Count - 1);
                if (GameProperties.AllScoreOverthrown.Any()) GameProperties.AllScoreOverthrown.RemoveAt(GameProperties.AllScoreOverthrown.Count - 1);
                #endregion
            }
        }
        /// <summary>
        /// Counter set first
        /// </summary>
        /// <param name="action">Case 1: Set first execution / Case 2: Set after leg</param>
        /// <returns></returns>
        public bool CounterFirst(int action)
        {
            switch (action)
            {
                case 1:
                    {
                        GameCounter.CountThrowUpdateWinner = 2;
                        GameCounter.CountThrowUpdateScore = MainProperties.CountPlayer - 1;
                        GameCounter.CountThrowOffsetLast = 0;
                        GameCounter.CountThrowLast = 0;
                        GameCounter.CountThrowActual = 1;
                        GameCounter.CountThrowIndex = 1;
                        GameCounter.CountThrowOffsetNext = 2;
                        GameCounter.CountAllThrowLast = 0;
                        GameCounter.CountAllThrowActual = 1;
                        GameCounter.CountAllThrowNext = 2;
                        GameCounter.CountPlayerIndexLast = PlayerIndexSet(1, 1, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexActual = PlayerIndexSet(1, 2, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexNext = PlayerIndexSet(1, 3, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerAverageLast = 0;
                        GameCounter.CountPlayerAverage = 0;
                        GameCounter.CountPlayerAverageNext = 0;
                        GameCounter.CountRoundLast = 0;
                        GameCounter.CountRoundActual = 1;
                        GameCounter.CountRoundNext = 2;
                        GameCounter.CountPlayerScoreLast = 0;
                        GameCounter.CountPlayerScoreActual = 0;
                        GameCounter.CountPlayerScoreNext = 0;
                        GameCounter.CountFinishLast = MainProperties.Points;
                        GameCounter.CountFinishActual = MainProperties.Points;
                        GameCounter.CountFinishNext = MainProperties.Points;
                        GameCounter.CountHighestThrow = 0;
                        GameCounter.CountWinner = 0;

                        return true;
                    }
                case 2:
                    {
                        GameCounter.CountThrowUpdateWinner = 2;
                        GameCounter.CountThrowUpdateScore = MainProperties.CountPlayer - 1;
                        GameCounter.CountThrowOffsetLast = 0;
                        GameCounter.CountThrowLast = 0;
                        GameCounter.CountThrowActual = 1;
                        GameCounter.CountThrowIndex = 1;
                        GameCounter.CountThrowOffsetNext = 2;
                        GameCounter.CountAllThrowLast++;
                        GameCounter.CountAllThrowActual++;
                        GameCounter.CountAllThrowNext++;
                        GameCounter.CountPlayerIndexLast = PlayerIndexSet(1, 1, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexActual = PlayerIndexSet(1, 2, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerIndexNext = PlayerIndexSet(1, 3, GameCounter.CountThrowOffsetLast);
                        GameCounter.CountPlayerAverageLast = 0;
                        GameCounter.CountPlayerAverage = 0;
                        GameCounter.CountPlayerAverageNext = 0;
                        GameCounter.CountRoundLast = 0;
                        GameCounter.CountRoundActual = 1;
                        GameCounter.CountRoundNext = 2;
                        GameCounter.CountPlayerScoreLast = 0;
                        GameCounter.CountPlayerScoreActual = 0;
                        GameCounter.CountPlayerScoreNext = 0;
                        GameCounter.CountFinishLast = MainProperties.Points;
                        GameCounter.CountFinishActual = MainProperties.Points;
                        GameCounter.CountFinishNext = MainProperties.Points;
                        GameCounter.CountHighestThrow = 0;
                        GameCounter.CountWinner = 0;

                        return true;
                    }
                default:
                    return false;
            }

        }
        /// <summary>
        /// Checks the reset
        /// </summary>
        /// <param name="action">Case 1: Check reset possible</param>
        /// <returns></returns>
        public bool CounterResetCheck(int action)
        {
            switch (action)
            {
                case 1:
                    {
                        if (GameCounter.CountAllThrowActual > GameCounter.CountMaxToReset + 1
                            && GameCounter.CountAllThrowActual > 1)
                            return true;
                        return false;
                    }
                default:
                    return false;
            }
        }

        // 3. Set Game properties
        /// <summary>
        /// Sets the game properties
        /// </summary>
        /// <param name="action01">Case 1: Set lists first execution / Case 2: Set lists</param>
        /// <returns>True if all completed OK</returns>
        public bool GamePropertiesSet(int action01)
        {
            switch (action01)
            {
                case 1:// Case 1: Set lists first execution
                    {
                        #region write 
                        GameProperties.AllScore = new List<int>() { GameProperties.InputScore };
                        GameProperties.AllScoreOverthrown = new List<bool> { GameProperties.IsOverthrown };
                        GameProperties.AllFinish = new List<int>() { GameCounter.CountFinishActual - GameProperties.InputScore };
                        GameProperties.AllFinishPossible = new List<bool>() { GamePropertiesCheck(4) };
                        GameProperties.AllDateTime = new List<long>() { DateTime.Now.Ticks };
                        GameProperties.AllPlayerIndex = new List<int>() { GameCounter.CountPlayerIndexActual };
                        GameProperties.AllPlayerName = new List<string>() {
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].ActualPlayerName };
                        GameProperties.AllRounds = new List<int>() { GameCounter.CountRoundActual };
                        #endregion

                        return true;
                    }
                case 2:// Case 2: Set lists
                    {
                        #region input
                        List<int> gpAllScore = GameProperties.AllScore;
                        List<bool> gpAllScoreOverthrown = GameProperties.AllScoreOverthrown;
                        List<int> gpAllFinish = GameProperties.AllFinish;
                        List<bool> gpAllFinishPossible = GameProperties.AllFinishPossible;
                        List<long> gpAllDateTime = GameProperties.AllDateTime;
                        List<int> gpAllPlayerIndex = GameProperties.AllPlayerIndex;
                        List<string> gpAllPlayerName = GameProperties.AllPlayerName;
                        List<int> gpAllRounds = GameProperties.AllRounds;
                        #endregion
                        #region sequence
                        gpAllScore.Add(GameProperties.InputScore);
                        gpAllScoreOverthrown.Add(GameProperties.IsOverthrown);
                        gpAllFinish.Add(GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast].CountActualFinish - GameProperties.InputScore);
                        gpAllFinishPossible.Add(GamePropertiesCheck(4));
                        gpAllDateTime.Add(DateTime.Now.Ticks);
                        gpAllPlayerIndex.Add(GameCounter.CountPlayerIndexActual);
                        GameProperties.AllPlayerName.Add(GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast].ActualPlayerName);
                        GameProperties.AllRounds.Add(GameCounter.CountRoundActual);
                        #endregion
                        #region write
                        GameProperties.AllScore = gpAllScore;
                        GameProperties.AllScoreOverthrown = gpAllScoreOverthrown;
                        GameProperties.AllFinish = gpAllFinish;
                        GameProperties.AllFinishPossible = gpAllFinishPossible;
                        GameProperties.AllDateTime = gpAllDateTime;
                        GameProperties.AllPlayerIndex = gpAllPlayerIndex;
                        GameProperties.AllPlayerName = gpAllPlayerName;
                        GameProperties.AllRounds = gpAllRounds;
                        #endregion

                        return true;
                    }
            }

            return false;
        }
        /// <summary>
        /// Set player data statistics
        /// </summary>
        /// <param name="action01">Case 1: Set actual throw first execute / Case 2: Set actual throw / Case 3: Set overthrown first execute 
        /// / Case 4: Set overthrown / Case 5: Set legs / Case 6: Set sets / Case 7: Set player ranking
        ///  / Case 8: Set player finished / Case 9: Reset finish/score on leg / Case 10: Reset finish/score on set
        ///  / Case 11: Set player end ranking / Case 12: Set game end time</param>
        /// <returns>True if set OK, false if set error</returns>
        public bool GamePropertiesSetPlayerData(int action01)
        {
            switch (action01)
            {
                case 1:// Case 1: Set actual throw first execute
                    {
                        try
                        {
                            #region input
                            var plDataStat = GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast];
                            #endregion
                            #region write
                            plDataStat.CountActualFinish = plDataStat.CountActualFinish - GameProperties.InputScore;
                            plDataStat.CountActualScore = plDataStat.CountActualScore + GameProperties.InputScore;
                            plDataStat.CountThrowIndex = new List<int>() { GameCounter.CountAllThrowActual };
                            plDataStat.CountScore = new List<int>() { GameProperties.InputScore };
                            plDataStat.CountFinish = new List<int>() { plDataStat.CountActualFinish };
                            plDataStat.FinishPossible = new List<bool>() { GamePropertiesCheck(4) };
                            plDataStat.CountRound = new List<int>() { GameCounter.CountRoundActual };
                            plDataStat.CountDTThrow = new List<long>() { DateTime.Now.Ticks };
                            plDataStat.Overthrown = new List<bool>() { GameProperties.IsOverthrown };
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast] = plDataStat;
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 2:// Case 2: Set actual throw
                    {
                        try
                        {
                            #region input
                            var plDataStat = GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast];
                            List<int> countThrowIndex = plDataStat.CountThrowIndex;
                            List<int> countScore = plDataStat.CountScore;
                            List<int> countFinish = plDataStat.CountFinish;
                            List<bool> finishPossible = plDataStat.FinishPossible;
                            List<int> countRound = plDataStat.CountRound;
                            List<long> countDTThrow = plDataStat.CountDTThrow;
                            List<bool> countOverthrown = plDataStat.Overthrown;

                            #endregion
                            #region sequence
                            plDataStat.CountActualFinish = plDataStat.CountActualFinish - GameProperties.InputScore;
                            plDataStat.CountActualScore = plDataStat.CountActualScore + GameProperties.InputScore;
                            countThrowIndex.Add(GameCounter.CountAllThrowActual);
                            countScore.Add(GameProperties.InputScore);
                            countFinish.Add(plDataStat.CountActualFinish);
                            finishPossible.Add(GamePropertiesCheck(4));
                            countRound.Add(GameCounter.CountRoundActual);
                            countDTThrow.Add(DateTime.Now.Ticks);
                            countOverthrown.Add(GameProperties.IsOverthrown);
                            #endregion
                            #region write
                            plDataStat.CountThrowIndex = countThrowIndex;
                            plDataStat.CountScore = countScore;
                            plDataStat.CountFinish = countFinish;
                            plDataStat.FinishPossible = finishPossible;
                            plDataStat.CountRound = countRound;
                            plDataStat.CountDTThrow = countDTThrow;
                            plDataStat.Overthrown = countOverthrown;
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast] = plDataStat;
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 3:// Case 3: Set overthrown first execute
                    {
                        try
                        {
                            #region input
                            var plDataStat = GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast];
                            #endregion
                            #region write
                            plDataStat.CountThrowIndex = new List<int>() { GameCounter.CountAllThrowActual };
                            plDataStat.CountScore = new List<int>() { 0 };
                            plDataStat.CountFinish = new List<int>() { plDataStat.CountActualFinish };
                            plDataStat.FinishPossible = new List<bool>() { GamePropertiesCheck(4) };
                            plDataStat.CountRound = new List<int>() { GameCounter.CountRoundActual };
                            plDataStat.CountDTThrow = new List<long>() { DateTime.Now.Ticks };
                            plDataStat.Overthrown = new List<bool>() { (GameProperties.IsOverthrown) };
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast] = plDataStat;
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 4:// Case 4: Set overthrown
                    {
                        try
                        {
                            #region input
                            var plDataStat = GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast];
                            List<int> countThrowIndex = plDataStat.CountThrowIndex;
                            List<int> countScore = plDataStat.CountScore;
                            List<int> countFinish = plDataStat.CountFinish;
                            List<bool> finishPossible = plDataStat.FinishPossible;
                            List<int> countRound = plDataStat.CountRound;
                            List<long> countDTThrow = plDataStat.CountDTThrow;
                            List<bool> countOverthrown = plDataStat.Overthrown;

                            #endregion
                            #region sequence
                            countThrowIndex.Add(GameCounter.CountAllThrowActual);
                            countScore.Add(0);
                            countFinish.Add(plDataStat.CountActualFinish);
                            finishPossible.Add(GamePropertiesCheck(4));
                            countRound.Add(GameCounter.CountRoundActual);
                            countDTThrow.Add(DateTime.Now.Ticks);
                            countOverthrown.Add(GameProperties.IsOverthrown);
                            #endregion
                            #region write
                            plDataStat.CountThrowIndex = countThrowIndex;
                            plDataStat.CountScore = countScore;
                            plDataStat.CountFinish = countFinish;
                            plDataStat.FinishPossible = finishPossible;
                            plDataStat.CountRound = countRound;
                            plDataStat.CountDTThrow = countDTThrow;
                            plDataStat.Overthrown = countOverthrown;
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast] = plDataStat;
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 5:// Case 5: Set legs
                    {
                        try
                        {
                            #region input
                            var plDataStat = GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast];
                            #endregion
                            #region sequence
                            plDataStat.CountLegWins++;
                            #endregion
                            #region write
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast] = plDataStat;
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 6:// Case 6: Set sets
                    {
                        try
                        {
                            #region input
                            var plDataStat = GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast];
                            #endregion
                            #region sequence
                            plDataStat.CountSetWins++;
                            plDataStat.CountLegWins = 0;
                            #endregion
                            #region write
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast] = plDataStat;
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 7:// Case 7: Set player ranking
                    {
                        try
                        {
                            #region input
                            var plRanking01 = new GameProp.PlayerWinStruct();
                            var plRanking02 = GameProperties.PlayerWin;
                            #endregion
                            #region sequence
                            if (GameProperties.PlayerWin == null)
                            {
                                plRanking01.DateTime = DateTime.Now.Ticks;
                                plRanking01.FinishScore = Game.GameProperties.AllScore[Game.GameProperties.AllScore.Count - 1];
                                plRanking01.Index = Game.GameProperties.AllPlayerIndex[Game.GameProperties.AllPlayerIndex.Count - 1];
                                plRanking01.Name = Game.GameProperties.AllPlayerName[Game.GameProperties.AllPlayerIndex.Count - 1];
                                plRanking01.WinnerThrows = Game.GameCounter.CountAllThrowActual;
                            }
                            else
                            {
                                plRanking01.DateTime = DateTime.Now.Ticks;
                                plRanking01.FinishScore = Game.GameProperties.AllScore[Game.GameProperties.AllScore.Count - 1];
                                plRanking01.Index = Game.GameProperties.AllPlayerIndex[Game.GameProperties.AllPlayerIndex.Count - 1];
                                plRanking01.Name = Game.GameProperties.AllPlayerName[Game.GameProperties.AllPlayerIndex.Count - 1];
                                plRanking01.WinnerThrows = Game.GameCounter.CountAllThrowActual;
                            }
                            #endregion
                            #region write
                            if (GameProperties.PlayerWin == null)
                            {
                                GameProperties.PlayerWin = new List<GameProp.PlayerWinStruct>() { plRanking01 };
                            }
                            else
                            {
                                plRanking02.Add(plRanking01);
                                GameProperties.PlayerWin = plRanking02;
                            }
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 8:// Case 8: Set player finished
                    {
                        try
                        {
                            #region input
                            var plDataStat = GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast];
                            #endregion
                            #region sequence
                            plDataStat.GameFinished = true;
                            #endregion
                            #region write
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast] = plDataStat;
                            #endregion
                            return true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error = {0}", e.Source);
                            Console.WriteLine("Error = {0}", e.Message);
                            Console.WriteLine("Error = {0}", e.StackTrace);
                            return false;
                        }
                    }
                case 9:// Case 9: Reset finish/score on leg
                    {
                        #region input
                        var plDataStat01 = GameProperties.PlayerDataStatistic;
                        var plDataStat02 = new GameProp.PlayerDataStatisticStruct();
                        #endregion
                        #region sequence
                        for (int i = 0; i < plDataStat01.Count; i++)
                        {
                            if (plDataStat01[i].CountScore != null)
                            {
                                plDataStat02 = plDataStat01[i];
                                plDataStat02.CountActualFinish = MainProperties.Points;
                                plDataStat02.CountActualScore = 0;
                                plDataStat02.CountThrowIndex.Clear();
                                plDataStat02.CountScore.Clear();
                                plDataStat02.CountFinish.Clear();
                                plDataStat02.FinishPossible.Clear();
                                plDataStat02.CountRound.Clear();
                                plDataStat02.CountDTThrow.Clear();
                                plDataStat01[i] = plDataStat02;
                            }
                            else
                            {
                                plDataStat02 = plDataStat01[i];
                                plDataStat02.CountActualFinish = MainProperties.Points;
                                plDataStat02.CountActualScore = 0;
                                plDataStat01[i] = plDataStat02;
                            }
                        }
                        GameProperties.PlayerDataStatistic = plDataStat01;
                        #endregion
                        #region output
                        return true;
                        #endregion
                    }
                case 10:// Case 10: Reset finish/score on set
                    {
                        #region input
                        var plDataStat01 = GameProperties.PlayerDataStatistic;
                        var plDataStat02 = new GameProp.PlayerDataStatisticStruct();
                        #endregion
                        for (int i = 0; i < plDataStat01.Count; i++)
                        {
                            plDataStat02 = plDataStat01[i];
                            plDataStat02.CountLegWins = 0;
                            plDataStat01[i] = plDataStat02;
                        }
                        GameProperties.PlayerDataStatistic = plDataStat01;
                        return true;
                    }
                case 11:// Case 11: Set player end ranking
                    {
                        #region input
                        var pds = DDD.Game.GameProperties.PlayerDataStatistic;
                        var listEndRanking = new List<GameProp.PlayerEndRankingStruct>(MainProperties.CountPlayer);
                        var plEndRanking = new GameProp.PlayerEndRankingStruct[MainProperties.CountPlayer];
                        int[,] countSets = new int[2, MainProperties.CountPlayer];
                        int[,] countLegs = new int[2, MainProperties.CountPlayer];
                        int[,] countLastPoints = new int[2, MainProperties.CountPlayer];
                        int[] countIndexPlayer = new int[MainProperties.CountPlayer];
                        //var plRanking01 = new GameProp.PlayerWinStruct();
                        //var plRanking02 = GameProperties.PlayerWin;
                        #endregion
                        #region sequence
                        #region define end ranking
                        for (int i = 0; i < MainProperties.CountPlayer; i++)
                        {
                            countSets[1, i] = MainProperties.Players[i].PlayerIndex;
                            countLegs[1, i] = MainProperties.Players[i].PlayerIndex;
                            countLastPoints[1, i] = MainProperties.Players[i].PlayerIndex;
                            countSets[0, i] = GameProperties.PlayerDataStatistic[i].CountSetWins;
                        }

                        for (int i = 0; i < GameProperties.PlayerWin.Count; i++)
                            for (int j = 0; j < MainProperties.CountPlayer; j++)
                                if (GameProperties.PlayerWin[i].Index == countLegs[1, j])
                                {
                                    countLegs[0, j]++;
                                    break;
                                }

                        for (int i = 0; i < GameProperties.AllPlayerIndex.Count; i++)
                            for (int j = 0; j < MainProperties.CountPlayer; j++)
                                if (GameProperties.AllPlayerIndex[i] == countLastPoints[1, j])
                                {
                                    countLastPoints[0, j] += GameProperties.AllScore[i];
                                    break;
                                }

                        #endregion
                        #region sort arrays
                        Datahandling.QuickSortDim2(ref countSets, 0, MainProperties.CountPlayer - 1);
                        Datahandling.QuickSortDim2(ref countLegs, 0, MainProperties.CountPlayer - 1);
                        Datahandling.QuickSortDim2(ref countLastPoints, 0, MainProperties.CountPlayer - 1);
                        #endregion
                        #region set player ranking
                        int countIndex = 0;
                        countIndexPlayer[countIndex] = countSets[1, 0];
                        countIndex++;

                        for (int i = 0; i < MainProperties.CountPlayer; i++)
                        {
                            if (countLastPoints[1, i] != countIndexPlayer[0])
                            {
                                countIndexPlayer[countIndex] = countLastPoints[1, i];
                                countIndex++;
                            }
                        }
                        for (int i = 0; i < MainProperties.CountPlayer; i++)
                        {
                            plEndRanking[i] = SetRanking(plEndRanking[i], countIndexPlayer[i]);
                            plEndRanking[i].Position = i + 1;
                        }
                        Game.GameProp.PlayerEndRankingStruct SetRanking(GameProp.PlayerEndRankingStruct per, int setIndex)
                        {
                            var endRanking = per;

                            for (int i = 0; i < MainProperties.CountPlayer; i++)
                            {
                                if (countSets[1, i] == setIndex)
                                {
                                    endRanking.Index = countSets[1, i];
                                    endRanking.Name = DatahandlingPlayer.GetPlayerNameFromIndex(countSets[1, i]);
                                    endRanking.SetsWin = countSets[0, i];
                                }
                                if (countLegs[1, i] == setIndex)
                                    endRanking.LegsWin = countLegs[0, i];

                                if (countLastPoints[1, i] == setIndex)
                                    endRanking.Points = countLastPoints[0, i];
                            }

                            return endRanking;
                        }
                        #endregion
                        #endregion
                        #region Set Avg/Throws/Highest score/ WinLoss
                        for (int i = 0; i < plEndRanking.Length; i++)
                        {
                            // declare 
                            int idx = 0;

                            for (int j = 0; j < pds.Count; j++)
                            {
                                if (pds[i].CountIndexPlayer == countIndexPlayer[j])
                                {
                                    idx = j;
                                    break;
                                }
                            }

                            // Avg
                            plEndRanking[idx].Avg = GetPlayerAvg(pds[i].CountScore, pds[i].Overthrown);
                            // Player ranking
                            if (pds[i].CountScore != null)
                                plEndRanking[idx].Throws = pds[i].CountScore.Count;
                            else
                                plEndRanking[idx].Throws = 0;
                            // Highest score
                            if (pds[i].CountScore != null)
                            {
                                for (int j = 0; j < pds[i].CountScore.Count; j++)
                                    if (pds[i].CountScore[j] > plEndRanking[idx].HighestPoints)
                                        plEndRanking[idx].HighestPoints = pds[i].CountScore[j];
                            }
                            else
                                plEndRanking[i].HighestPoints = 0;
                            // WinLoss
                            for (int j = 0; j < pds.Count; j++)
                            {
                                var plDhps = new DatahandlingPlayerStat(DDD.Configuration.FilePathDDDS3D);
                                var plStat = plDhps.GetPlayerStatistic(plEndRanking[idx].Index);
                                if (plStat.TotalLoss == 0)
                                    plEndRanking[idx].WinLoss = plStat.TotalWin / 1;
                                else
                                    plEndRanking[idx].WinLoss = plStat.TotalWin / plStat.TotalLoss;
                            }
                        }

                        #endregion
                        #region write
                        for (int i = 0; i < MainProperties.CountPlayer; i++)
                            listEndRanking.Add(plEndRanking[i]);

                        GameProperties.PlayerEndRanking = listEndRanking;
                        #endregion

                        return true;
                    }
                case 12:// Case 12: Set game end time
                    {
                        #region write
                        MainProperties.DTGameEnded = DateTime.Now.Ticks;
                        #endregion

                        return true;
                    }

                default:
                    return false;
            }
        }
        /// <summary>
        /// Checks the actual throw
        /// </summary>
        /// <param name="action01">Case 1: Check win / Case 2: Check throw ok / Case 3: Check overthrown and set InputScore to 0
        ///  / Case 4: Check finish possible / Case 5: Check legs win / Case 6: Check sets win
        ///  / Case 7: Check game win / Case 8: Check first round / Case 9: Check player finished / Case 10: Check bot throw</param>
        /// <returns>Check false, Check true</returns>
        public bool GamePropertiesCheck(int action01)
        {
            switch (action01)
            {
                case 1:// Case 1: Check win
                    {
                        if (GameProperties.InputScore -
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountActualFinish == 0) return true;
                        else return false;
                    }
                case 2:// Case 2: Check throw ok
                    {
                        if (GameProperties.InputScore + GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountActualScore <=
                            MainProperties.Points) return true;
                        return false;
                    }
                case 3:// Case 3: Check overthrown and set InputScore to 0
                    {
                        if (GameProperties.InputScore + GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountActualScore >
                            MainProperties.Points)
                        {
                            GameProperties.IsOverthrown = true;
                            GameProperties.InputScore = 0;
                            return true;
                        }
                        GameProperties.IsOverthrown = false;
                        return false;
                    }
                case 4:// Case 4: Check finish possible
                    {
                        bool checkFinish = ScoringTable.CheckFinishExists(
                            GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountActualFinish);
                        return checkFinish;
                    }
                case 5:// Case 5: Check legs win
                    {
                        if (GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountLegWins + 1 <= MainProperties.Legs)
                            return true;
                        return false;
                    }
                case 6:// Case 6: Check sets win
                    {
                        if (GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountLegWins + 1 >= MainProperties.Legs)
                            return true;
                        return false;
                    }
                case 7:// Case 7: Check game win
                    {
                        if (MainProperties.Legs == GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountLegWins + 1)
                            if (GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountSetWins + 1 == MainProperties.Sets)
                                return true;
                        return false;
                    }
                case 8:// Case 8: Check first round
                    {
                        if (GameCounter.CountRoundActual < 2)
                            return true;
                        return false;
                    }
                case 9:// Case 9: Check player finished
                    {
                        if (GameProperties.PlayerDataStatistic[GameCounter.CountThrowOffsetLast].GameFinished
                           )
                            return true;
                        return false;
                    }
                case 10:// Case 10: Check bot throw
                    {
                        if (GameCounter.CountPlayerIndexActual == 0)
                            return true;
                        return false;
                    }

                default:
                    return false;
            }
        }

        // 4. Output
        /// <summary>
        /// Sets the datagrid data for the game
        /// </summary>
        public void GameDataGridSet(int cntPlayer)
        {
            if (cntPlayer <= 0)
                return;

            int cntRound = 0;
            int cntScore = 0;
            var dgp = new DataGrid[cntPlayer];

            for (int i = 0; i < cntPlayer; i++)
            {
                if (DDD.Game.GameProperties.PlayerDataStatistic[i].CountScore == null)
                {
                    dgp[i].dgAvg = 0;
                    dgp[i].dgLastScore = 0;
                }
                else
                {
                    cntRound = DDD.Game.GameProperties.PlayerDataStatistic[i].CountScore.Count;
                    for (int j = 0; j < cntRound; j++)
                        if (!DDD.Game.GameProperties.PlayerDataStatistic[i].Overthrown[j])
                            cntScore += DDD.Game.GameProperties.PlayerDataStatistic[i].CountScore[j];
                    
                    if (cntScore > 0)
                    {
                        dgp[i].dgAvg = cntScore /
                            DDD.Game.GameProperties.PlayerDataStatistic[i].CountScore.Count;
                        dgp[i].dgLastScore = DDD.Game.GameProperties.PlayerDataStatistic[i].
                            CountScore[DDD.Game.GameProperties.PlayerDataStatistic[i].CountScore.Count - 1];
                    }
                    cntRound = 0;
                    cntScore = 0;
                }

                dgp[i].dgFinish = DDD.Game.GameProperties.PlayerDataStatistic[i].CountActualFinish;
                dgp[i].dgPlayerName = DDD.Game.GameProperties.PlayerDataStatistic[i].ActualPlayerName;
            }

            DataGridProp = dgp;
        }
        /// <summary>
        /// Set output variables for gui
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool GameOutputSet()
        {
            // Set actual player
            DisplayProperties.ActPlayer.Finish =
                GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].CountActualFinish;
            DisplayProperties.ActPlayer.Score = "";
            DisplayProperties.ActPlayer.Name =
                GameProperties.PlayerDataStatistic[GameCounter.CountThrowLast].ActualPlayerName;

            // Set next player
            if (MainProperties.CountPlayer > 1)
            {
                DisplayProperties.NextPlayer.Finish =
                    GameProperties.PlayerDataStatistic[GameCounter.CountThrowIndex].CountActualFinish;
                DisplayProperties.NextPlayer.Score =
                    GameProperties.PlayerDataStatistic[GameCounter.CountThrowIndex].CountActualScore;
                DisplayProperties.NextPlayer.Name =
                    GameProperties.PlayerDataStatistic[GameCounter.CountThrowIndex].ActualPlayerName;
            }

            // Set HighScore
            DisplayProperties.HighScore.HighestLegs = HighestLegs().ToString()
                + " / " + MainProperties.Legs.ToString();
            DisplayProperties.HighScore.PlayerName = HighScoreLegPlayerName() + " / " + MainProperties.Legs.ToString();
            DisplayProperties.HighScore.ActSet = GameCounter.CountSets.ToString() + " / " + MainProperties.Sets.ToString();

            // Set ListBox
            DisplayProperties.ListBoxActPlayer = GameCounter.CountThrowLast;

            return true;

            int HighestLegs()
            {
                int CountMostLegs = 0;
                for (int i = 0; i < MainProperties.CountPlayer; i++)
                {
                    if (GameProperties.PlayerDataStatistic[i].CountLegWins > CountMostLegs)
                        CountMostLegs = GameProperties.PlayerDataStatistic[i].CountLegWins;
                }
                return CountMostLegs;
            }
            string HighScoreLegPlayerName()
            {
                string PlayerNames = "";
                if (GameCounter.CountLegs > 0)
                    for (int i = 0; i < MainProperties.CountPlayer; i++)
                    {
                        if (GameProperties.PlayerDataStatistic[i].CountLegWins == GameCounter.CountHighestLegs)
                        {
                            PlayerNames += GameProperties.PlayerDataStatistic[i].ActualPlayerName.Substring(0, 4) + "  ";
                        }
                    }
                return PlayerNames;
            }
        }

        /// <summary>
        /// Counter set or reset index from player
        /// </summary>
        /// <param name="action01">Case 1: Set / Case 2: Reset</param>
        /// <param name="action02">Case 1: Last / Case 2: Actual / Case 3: Next</param>
        /// <param name="throwActualLast"></param>
        /// <returns>The new index value</returns>
        private int PlayerIndexSet(int action01, int action02, int throwActualLast)
        {
            // Variable decleration
            int[] CountPlayerIndex = new int[MainProperties.CountPlayer];

            // Sequence programm
            for (int i = 0; i < MainProperties.CountPlayer; i++)
                CountPlayerIndex[i] = MainProperties.Players[i].PlayerIndex;

            switch (action01)
            {
                case 1: // set
                    {
                        switch (action02) // 1 = Last // 2 = Actual // 3 = Next
                        {
                            case 1:
                                {
                                    if (throwActualLast == 0) return CountPlayerIndex[MainProperties.CountPlayer - 1];
                                    else return CountPlayerIndex[throwActualLast - 1];
                                }
                            case 2:
                                {
                                    return CountPlayerIndex[throwActualLast];
                                }
                            case 3:
                                {
                                    if (throwActualLast == MainProperties.CountPlayer - 1) return CountPlayerIndex[0];
                                    else return CountPlayerIndex[throwActualLast + 1];
                                }
                            default:
                                return -1;
                        }
                    }
                case 2: // reset
                    {
                        switch (action02) // 1 = Last // 2 = Actual // 3 = Next
                        {
                            case 1:
                                {
                                    if (throwActualLast == 0) return CountPlayerIndex[MainProperties.CountPlayer - 1];
                                    else return CountPlayerIndex[throwActualLast - 1];
                                }
                            case 2:
                                {
                                    return CountPlayerIndex[throwActualLast];
                                }
                            case 3:
                                {
                                    if (throwActualLast == MainProperties.CountPlayer - 1) return CountPlayerIndex[0];
                                    else return CountPlayerIndex[throwActualLast + 1];
                                }
                            default:
                                return -1;
                        }
                    }
                default:
                    return -1;
            }
        }
        private int PlayerIndexReset(int action, int throwActualLast)
        {
            // Variable decleration
            Game.MainProp.Player[] mpPlayer = MainProperties.Players;
            int[] plIndex = new int[mpPlayer.Length];

            // Sequence programm
            for (int i = 0; i < mpPlayer.Length; i++)
                plIndex[i] = mpPlayer[i].PlayerIndex;

            switch (action)
            {
                case 1:
                    {
                        if (throwActualLast == 0) return plIndex[MainProperties.CountPlayer - 1];
                        else return plIndex[throwActualLast - 1];
                    }
                case 2:
                    {
                        return plIndex[throwActualLast];
                    }
                case 3:
                    {
                        if (throwActualLast == MainProperties.CountPlayer - 1) return plIndex[0];
                        else return plIndex[throwActualLast + 1];
                    }
            }

            return -1;
        }

        // 5. Helper methods
        /// <summary>
        /// Get the average score
        /// </summary>
        /// <param name="points">Get the score</param>
        /// <param name="overthrown">Get overthrown</param>
        /// <returns>If points or overthrown is null, -1 is returned.</returns>
        public double GetPlayerAvg(IList<int> points, IList<bool> overthrown)
        {
            if (points == null || overthrown == null)
                return 0;

            double retPoints = new int();
            double cntScore = 0;

            for (int i = 0; i < points.Count; i++)
                if (!overthrown[i])
                    cntScore += points[i];

            if (cntScore > 0)
                retPoints = cntScore / points.Count;
                
            return retPoints;
        }

        //// Reset score
        //public bool Reset()
        //{
        //    PR.Game._Counter.CountRoundLast = PR.Game._Counter.CountRoundLast - CounterRound();
        //    PR.Game._Counter.CountRoundActual = PR.Game._Counter.CountRoundActual - CounterRound();
        //    PR.Game._Counter.CountRoundNext = PR.Game._Counter.CountRoundNext - CounterRound();
        //    PR.Game._Counter.CountAllThrowLast = PR.Game._Counter.CountAllThrowLast - 1;
        //    PR.Game._Counter.CountAllThrowActual = PR.Game._Counter.CountAllThrowActual - 1;
        //    PR.Game._Counter.CountAllThrowNext = PR.Game._Counter.CountAllThrowNext - 1;
        //    PR.Game._Counter.CountThrowOffsetLast = CountThrow(1);
        //    PR.Game._Counter.CountThrowLast = CountThrow(4);
        //    PR.Game._Counter.CountThrowActual = CountThrow(2);
        //    PR.Game._Counter.CountThrowIndex = CountThrow(5);
        //    PR.Game._Counter.CountThrowOffsetNext = CountThrow(3);
        //    PR.Game._Counter.CountThrowUpdateScore = CountThrow(6);
        //    PR.Game._Counter.CountPlayerIndexLast = ResetPlayerIndex(1, PR.Game._Counter.CountThrowOffsetLast);
        //    PR.Game._Counter.CountPlayerIndexActual = ResetPlayerIndex(2, PR.Game._Counter.CountThrowOffsetLast);
        //    PR.Game._Counter.CountPlayerIndexNext = ResetPlayerIndex(3, PR.Game._Counter.CountThrowOffsetLast);
        //    if (PR.Game._Counter.CountAllThrowActual <= 2) PR.Game._Counter.CountFinishLast = 0;
        //    else PR.Game._Counter.CountFinishLast = PR.Game.PlayersAllFinish[PR.Game._Counter.CountAllThrowLast - 2];
        //    if (PR.Game._Counter.CountAllThrowActual == 1) PR.Game._Counter.CountFinishActual = 0;
        //    else PR.Game._Counter.CountFinishActual = PR.Game.PlayersAllFinish[PR.Game._Counter.CountAllThrowLast - 1];
        //    PR.Game._Counter.CountFinishNext = PR.Game.PlayersAllFinish[PR.Game._Counter.CountAllThrowLast];

        //    ResetLists();

        //    return true;

        //    int CountThrow(int Action02)
        //    {
        //        switch (Action02)
        //        {
        //            case 1:
        //                if (PR.Game._Counter.CountThrowOffsetLast == 0) return PR.Game.CountPlayer - 1;
        //                else return PR.Game._Counter.CountThrowOffsetLast - 1;
        //            case 2:
        //                if (PR.Game._Counter.CountThrowActual == 1) return PR.Game.CountPlayer;
        //                else return PR.Game._Counter.CountThrowActual - 1;
        //            case 3:
        //                if (PR.Game._Counter.CountThrowOffsetNext == 2) return PR.Game.CountPlayer + 1;
        //                else return PR.Game._Counter.CountThrowOffsetNext - 1;
        //            case 4:
        //                if (PR.Game._Counter.CountThrowLast == 0) return PR.Game.CountPlayer - 1;
        //                else return PR.Game._Counter.CountThrowLast - 1;
        //            case 5:
        //                if (PR.Game._Counter.CountThrowIndex == 0) return PR.Game.CountPlayer - 1;
        //                else return PR.Game._Counter.CountThrowIndex - 1;
        //            case 6:
        //                if (PR.Game._Counter.CountThrowUpdateScore == 0) return PR.Game.CountPlayer - 1;
        //                else return PR.Game._Counter.CountThrowUpdateScore - 1;
        //            default:
        //                return 0;
        //        }
        //    }
        //    int CounterRound()
        //    {
        //        if (PR.Game._Counter.CountThrowActual == 1) return 1;
        //        else return 0;
        //    }
        //    void ResetLists()
        //    {
        //        if (PR.Game.PlayersAllScore[PR.Game._Counter.CountAllThrowLast] <= PR.Game.PlayersFinish[PR.Game._Counter.CountThrowOffsetLast] ||
        //            PR.Game.PlayersAllScoreNoFinish[PR.Game._Counter.CountAllThrowLast] == false)
        //        {
        //            PR.Game.PlayersScore[PR.Game._Counter.CountThrowOffsetLast] =
        //                 PR.Game.PlayersScore[PR.Game._Counter.CountThrowOffsetLast] - PR.Game.PlayersAllScore[PR.Game._Counter.CountAllThrowLast];

        //            PR.Game.PlayersFinish[PR.Game._Counter.CountThrowOffsetLast] =
        //                PR.Game.PlayersFinish[PR.Game._Counter.CountThrowOffsetLast] + PR.Game.PlayersAllScore[PR.Game._Counter.CountAllThrowLast];
        //        }
        //        if (PR.Game.PlayersAllScore.Any()) PR.Game.PlayersAllScore.RemoveAt(PR.Game.PlayersAllScore.Count - 1);
        //        if (PR.Game.PlayersAllDateTime.Any()) PR.Game.PlayersAllDateTime.RemoveAt(PR.Game.PlayersAllDateTime.Count - 1);
        //        if (PR.Game.PlayersAllIndex.Any()) PR.Game.PlayersAllIndex.RemoveAt(PR.Game.PlayersAllIndex.Count - 1);
        //        if (PR.Game.PlayersAllRound.Any()) PR.Game.PlayersAllRound.RemoveAt(PR.Game.PlayersAllRound.Count - 1);
        //        if (PR.Game.PlayersAllFinish.Any()) PR.Game.PlayersAllFinish.RemoveAt(PR.Game.PlayersAllFinish.Count - 1);
        //        if (PR.Game.PlayersAllScoreNoFinish.Any()) PR.Game.PlayersAllScoreNoFinish.RemoveAt(PR.Game.PlayersAllScoreNoFinish.Count - 1);
        //    }
        //}
        #endregion
        #region struct library
        public struct MainProp
        {
            #region properties
            public string GameMode;
            public int Points;
            public int Sets;
            public int Legs;
            public int BotDifficultyValue;
            public string BotDifficultyName;
            public bool BotCheck;
            public long DTGameStarted;
            public long DTGameEnded;
            public int CountPlayer;
            public int CountWinner;
            public bool CheckFirstThrowExecution;
            public bool CheckGameWin;
            public Player[] PlayersStart;
            public Player[] Players;
            #endregion
            #region struct library
            public struct Player
            {
                public string PlayerName;
                public int PlayerIndex;
            }
            #endregion
        }
        public struct DisplayProp
        {
            #region properties
            public ActPlayerStruct ActPlayer;
            public NextPlayerStruct NextPlayer;
            public HighScoreStruct HighScore;
            public int ListBoxActPlayer;

            #endregion
            #region class library
            public struct ActPlayerStruct
            {
                public string Name;
                public string Score;
                public int Finish;
            }
            public struct NextPlayerStruct
            {
                public string Name;
                public int Score;
                public int Finish;
            }
            public struct HighScoreStruct
            {
                public string ActSet;
                public string HighestLegs;
                public string PlayerName;
            }
            #endregion
        }
        public struct GameProp
        {
            #region fields
            public int InputScore;
            public int InputScoreBot;
            public bool IsOverthrown;
            public List<int> AllScore;
            public List<bool> AllScoreOverthrown;
            public List<int> AllFinish;
            public List<bool> AllFinishPossible;
            public List<long> AllDateTime;
            public List<int> AllPlayerIndex;
            public List<string> AllPlayerName;
            public List<int> AllRounds;
            public List<PlayerWinStruct> PlayerWin;
            public List<PlayerDataStatisticStruct> PlayerDataStatistic;
            public List<PlayerEndRankingStruct> PlayerEndRanking;
            #endregion
            #region class library
            public struct PlayerWinStruct
            {
                public int Index;
                public string Name;
                public long DateTime;
                public int FinishScore;
                public int WinnerThrows;
            }
            public struct PlayerDataStatisticStruct
            {
                public int CountIndexPlayer;
                public int CountLegWins;
                public int CountSetWins;
                public int CountActualScore;
                public int CountActualFinish;
                public string ActualPlayerName;
                public bool GameFinished;
                public List<int> CountThrowIndex;
                public List<int> CountScore;
                public List<int> CountFinish;
                public List<bool> FinishPossible;
                public List<bool> Overthrown;
                public List<int> CountRound;
                public List<long> CountDTThrow;
            }
            public struct PlayerEndRankingStruct
            {
                public int Position;
                public int Index;
                public string Name;
                public double Avg;
                public int Throws;
                public double WinLoss;
                public int HighestPoints;
                public int SetsWin;
                public int LegsWin;
                public int Points;
            }
            #endregion
        }
        public struct GameCount
        {
            public int CountPlayerIndexLast;
            public int CountPlayerIndexActual;
            public int CountPlayerIndexNext;
            public int CountPlayerAverageLast;
            public int CountPlayerAverage;
            public int CountPlayerAverageNext;
            public int CountThrowUpdateWinner;
            public int CountThrowUpdateScore;
            public int CountThrowOffsetLast;
            public int CountThrowLast;
            public int CountThrowActual; // 1,2,3...1,2,3
            public int CountThrowIndex;
            public int CountThrowOffsetNext;
            public int CountAllThrowLast;
            public int CountAllThrowActual; // 1,2,3,4,5,6,7,8,9,10...
            public int CountAllThrowNext;
            public int CountRoundLast;
            public int CountRoundActual;// 1,1,1,2,2,2,3,3,3,4,4,4,5,5,5,6,6,6,7,7,7...
            public int CountRoundNext;
            public int CountPlayerScoreLast;
            public int CountPlayerScoreActual;
            public int CountPlayerScoreNext;
            public int CountFinishLast;
            public int CountFinishActual;
            public int CountFinishNext;
            public int CountHighestThrow;
            public int CountWinner;
            public int CountSets;
            public int CountLegs;
            public int CountHighestLegs;
            public int CountMaxToReset;
        }
        public struct DataGrid
        {
            public string dgPlayerName { get; set; }
            public int dgFinish { get; set; }
            public int dgAvg { get; set; }
            public int dgLastScore { get; set; }
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
            //// TODO: uncomment the following line if the finalizer is overridden above.
            //GC.SuppressFinalize(this);
        }
        public void DisposeGameData()
        {
            #region reset main properties
            MainProperties.DTGameEnded = 0;
            MainProperties.DTGameStarted = 0;
            MainProperties.Players = null;
            MainProperties.CountWinner = 0;
            MainProperties.CheckFirstThrowExecution = false;
            #endregion
            #region reset display properties
            DisplayProperties.ActPlayer.Finish = 0;
            DisplayProperties.ActPlayer.Name = null;
            DisplayProperties.ActPlayer.Score = null;
            DisplayProperties.HighScore.ActSet = null;
            DisplayProperties.HighScore.HighestLegs = null;
            DisplayProperties.HighScore.PlayerName = null;
            DisplayProperties.NextPlayer.Finish = 0;
            DisplayProperties.NextPlayer.Name = null;
            DisplayProperties.NextPlayer.Score = 0;
            DisplayProperties.ListBoxActPlayer = 0;
            #endregion
            #region reset game properties
            GameProperties.AllDateTime = null;
            GameProperties.AllFinish = null;
            GameProperties.AllFinishPossible = null;
            GameProperties.AllPlayerIndex = null;
            GameProperties.AllPlayerName = null;
            GameProperties.AllRounds = null;
            GameProperties.AllScore = null;
            GameProperties.AllScoreOverthrown = null;
            GameProperties.InputScore = 0;
            GameProperties.PlayerDataStatistic = null;
            GameProperties.PlayerWin = null;
            GameProperties.PlayerEndRanking = null;
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// Statistic
    /// </summary>
    public class Statistic : IDisposable
    {
        #region constructor
        public Statistic() {; }
        public Statistic(string fullPath)
        {
            FullPath = fullPath;
        }
        #endregion
        #region properties, fields, constants
        //private DatahandlingGame.GameStatisticStruct GameStatistic;
        private const string HeaderPlayerStatistic = "PlayerIndex,PlayerName,TotalGames,TotalLegsWin," +
            "TotalSetsWin,TotalWin,TotalLoss,TotalGameTime,TotalThrowTime," +
            "TotalThrows,TotalPoints,TotalAvgPosition";

        private const string HeaderPlayerGameStatistic00 = "Index,PlayerName,FirstName,LastName,Country,BirthYear,BirthMonth,BirthDay," +
            "PlayerIndex,PlayerName,DateTime,FinishScore,WinnerThrows," +
            "ThrowIndex,PlayerIndex,PlayerName,CountRounds,CountScore,IsScoreOverthrown,CountFinish,IsFinishPossible,DateTime";// Complete
        private const string HeaderPlayerGameStatistic01 = "Index,PlayerName,FirstName,LastName,Country,BirthYear,BirthMonth,BirthDay"; // Player
        private const string HeaderPlayerGameStatistic02 = "PlayerIndex,PlayerName,DateTime,FinishScore,WinnerThrows"; // Leg
        private const string HeaderPlayerGameStatistic03 = "ThrowIndex,PlayerIndex,PlayerName,CountRounds,CountScore,IsScoreOverthrown,CountFinish,IsFinishPossible,DateTime"; // Game";

        private static string _FullPath { get; set; }
        private static string FullPath
        {
            get { return _FullPath; }
            set { _FullPath = value; }
        }
        #endregion
        #region static methods
        /// <summary>
        /// Check statistic
        /// </summary>
        /// <param name="action">Case 1: Check if excel exists</param>
        /// <returns>Return 1 if error / Return 10 Check excel / Return 20 File exists</returns>
        public static int Check(int action)
        {
            switch (action)
            {
                case 1:
                    {
                        Func<string, string, int> func = ((a, b) => 
                        {
                            if (a != null)
                                return(a.IndexOf(b,0));
                            else
                                return 0;
                        });
                        string sSearchProgram = "Microsoft Excel ";

                        string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                        List<string> listPrograms = new List<string>();
                        using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstallKey))
                        {
                            foreach (string skName in rk.GetSubKeyNames())
                            { 
                                using (Microsoft.Win32.RegistryKey sk = rk.OpenSubKey(skName))
                                {
                                    try
                                    {
                                        if (func((string)sk.GetValue("DisplayName"), sSearchProgram) >= sSearchProgram.Length)
                                        {
#if debug
                                            Console.WriteLine("*****************EXCEL*****************");
                                            Console.WriteLine(sk.GetValue("DisplayName"));
#endif
                                            return 10;
                                        }
                                    }
                                    catch (Exception ex)
                                    { }
                                }
                            }
                        }
                        return 1;
                    }
                case 2:
                    {
                        // Return 20 File exists
                        if (FullPath != null)
                        {
                            FileInfo fileInfo = new FileInfo(FullPath);
                            if (fileInfo.Exists)
                                return 20;
                        }

                        return 1;
                    }
            }
            return 1;
        }
        #endregion
        #region methods
        public bool SavePlayerCSV(string FilePath, object playerStatistic)
        {
            try
            {
                string write = CreatePlayerCSV((DatahandlingPlayerStat.MainStruct)playerStatistic);
                if (write == null)
                    return false;

                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(write);
                        sw.Dispose();
                        fs.Dispose();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public bool SaveGameCSV(string FilePath, object gameStatistic)
        {
            try
            {
                string write = CreateGameCSV((DatahandlingGame.GameStatisticStruct)gameStatistic);
                if (write == null)
                    return false;

                using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(write);
                        sw.Dispose();
                        fs.Dispose();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }

        public int SavePlayerXLSX(string FilePath, object playerStatistic)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();

                if (excelApp == null)
                    return 10;

                excelApp.Visible = false;

                object misVal = System.Reflection.Missing.Value;
                Excel.Workbook excelWB = excelApp.Workbooks.Add(misVal);
                System.Threading.Thread.Sleep(3000);


                // Write sheets
                Excel._Worksheet excelWSPlayerStat = (Excel._Worksheet)excelWB.Sheets[1];
                excelWSPlayerStat = CreatePlayerDataXLSX(excelWSPlayerStat, (DatahandlingPlayerStat.MainStruct)playerStatistic);

                Excel._Worksheet excelWSGameData = (Excel._Worksheet)excelWB.Worksheets.Add(After: excelWB.Sheets[excelWB.Sheets.Count]);
                excelWSGameData = CreatePlayerGameDataXLSX(excelWSGameData, (DatahandlingPlayerStat.MainStruct)playerStatistic);

                // Save
                excelWB.SaveAs(FilePath, Excel.XlFileFormat.xlOpenXMLWorkbook, misVal, misVal, misVal, misVal,
                    Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, misVal, misVal, misVal);

                excelWB.Close(true, misVal, misVal);
                excelApp.Quit();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Marshal.ReleaseComObject(excelWSGameData);
                Marshal.ReleaseComObject(excelWSPlayerStat);
                Marshal.ReleaseComObject(excelWB);
                Marshal.ReleaseComObject(excelApp);

                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return 11;
            }
        }
        public int SaveGameXLSX(string FilePath, string gameName)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();

                if (excelApp == null)
                    return 10;

                excelApp.Visible = false;

                object misVal = System.Reflection.Missing.Value;
                Excel.Workbook excelWB = excelApp.Workbooks.Add(misVal);
                System.Threading.Thread.Sleep(3000);


                // Write sheets
                Excel._Worksheet excelWSGameData = (Excel._Worksheet)excelWB.Worksheets.Add(After: excelWB.Sheets[excelWB.Sheets.Count]);
                excelWSGameData = CreateGameDataXLSX(excelWSGameData, gameName);

                // Save
                excelWB.SaveAs(FilePath, Excel.XlFileFormat.xlOpenXMLWorkbook, misVal, misVal, misVal, misVal,
                    Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, true, misVal, misVal, misVal);

                excelWB.Close(true, misVal, misVal);
                excelApp.Quit();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Marshal.ReleaseComObject(excelWSGameData);
                Marshal.ReleaseComObject(excelWB);
                Marshal.ReleaseComObject(excelApp);

                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return 11;
            }
        }
        #endregion
        #region help methods
        private string CreatePlayerCSV(DatahandlingPlayerStat.MainStruct playerStatistic)
        {
            try
            {
                var sb = new StringBuilder();

                // header
                sb.Append("PlayerIndex,PlayerName,TotalGames,TotalLegsWin,TotalSetsWin,TotalWin,TotalLoss,TotalGameTime,TotalThrowTime,TotalThrows,TotalPoints,TotalAvgPosition");
                sb.Append(Environment.NewLine);

                // data
                sb.Append(playerStatistic.PlayerIndex.ToString());
                sb.Append(",");
                sb.Append(playerStatistic.PlayerName.TrimEnd());
                sb.Append(",");
                sb.Append(playerStatistic.TotalGames.ToString());
                sb.Append(",");
                sb.Append(playerStatistic.TotalLegsWin.ToString());
                sb.Append(",");
                sb.Append(playerStatistic.TotalSetsWin.ToString());
                sb.Append(",");
                sb.Append(playerStatistic.TotalWin.ToString());
                sb.Append(",");
                sb.Append(playerStatistic.TotalLoss.ToString());
                sb.Append(",");
                if (playerStatistic.TotalGameTime > 0)
                {
                    DateTime dt = new DateTime(playerStatistic.TotalGameTime);
                    sb.Append(dt.ToString("dd/MM/yyyy HH:mm:ss"));
                    sb.Append(",");
                }
                else
                {
                    DateTime dt = new DateTime(0);
                    sb.Append(dt.ToString("dd/MM/yyyy HH:mm:ss"));
                    sb.Append(",");
                }
                if (playerStatistic.TotalThrowTime > 0)
                {
                    DateTime dt = new DateTime(playerStatistic.TotalThrowTime);
                    sb.Append(dt.ToString("dd/MM/yyyy HH:mm:ss"));
                    sb.Append(",");
                }
                else
                {
                    DateTime dt = new DateTime(0);
                    sb.Append(dt.ToString("dd/MM/yyyy HH:mm:ss"));
                    sb.Append(",");
                }
                sb.Append(playerStatistic.TotalThrows.ToString());
                sb.Append(",");
                sb.Append(playerStatistic.TotalPoints.ToString());
                sb.Append(",");
                sb.Append(playerStatistic.TotalAvgPosition.ToString().Replace(',', '.'));

                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }
        private string CreateGameCSV(DatahandlingGame.GameStatisticStruct gameStatistic)
        {
            try
            {
                var sb = new StringBuilder();
                int[] countRowMain = new int[3];
                countRowMain[0] = gameStatistic.Main.MainPlayer.Length;
                countRowMain[1] = gameStatistic.Main.MainLeg.Length;
                countRowMain[2] = gameStatistic.Main.MainGameStatistic.Length;
                int countRowMax = countRowMain[0];

                for (int i = 1; i < 3; i++)
                    if (countRowMain[i] > countRowMax)
                        countRowMax = countRowMain[i];

                // header
                sb.Append("Index,PlayerName,FirstName,LastName,Country,BirthYear,BirthMonth,BirthDay,"); // Player
                sb.Append(",PlayerIndex,PlayerName,DateTime,FinishScore,WinnerThrows,"); // Leg
                sb.Append(",ThrowIndex,PlayerIndex,PlayerName,CountRounds,CountScore,IsScoreOverthrown,CountFinish,IsFinishPossible,DateTime"); // Game
                sb.Append(Environment.NewLine);
                // data
                for (int i = 0; i < countRowMax; i++)
                {
                    if (i < countRowMain[0])
                    {
                        sb.Append(gameStatistic.Main.MainPlayer[i].Index.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainPlayer[i].PlayerName.TrimEnd());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainPlayer[i].FirstName.TrimEnd());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainPlayer[i].LastName.TrimEnd());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainPlayer[i].Country.TrimEnd());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainPlayer[i].BirthYear.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainPlayer[i].BirthMonth.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainPlayer[i].BirthDay.ToString());
                        sb.Append(",");
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                    }
                    if (i < countRowMain[1])
                    {
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainLeg[i].CountPlayerIndex.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainLeg[i].PlayerName.TrimEnd());
                        sb.Append(",");
                        DateTime dt = new DateTime(gameStatistic.Main.MainLeg[i].DateTime);
                        sb.Append(dt.ToString("dd/MM/yyyy HH:mm:ss"));
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainLeg[i].CountFinishScore.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainLeg[i].CountLegWinnerThrows.ToString());
                        sb.Append(",");
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                    }
                    if (i < countRowMain[2])
                    {
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].CountIndexThrow.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].CountIndexPlayer.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].PlayerName.TrimEnd());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].CountRounds.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].CountScore.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].IsScoreOverthrown.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].CountFinish.ToString());
                        sb.Append(",");
                        sb.Append(gameStatistic.Main.MainGameStatistic[i].IsFinishPossible.ToString());
                        sb.Append(",");
                        DateTime dt = new DateTime(gameStatistic.Main.MainGameStatistic[i].CountDateTime);
                        sb.Append(dt.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                        sb.Append(",");
                    }
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return null;
            }
        }

        private Excel._Worksheet CreatePlayerDataXLSX(Excel._Worksheet excelWorksheet, DatahandlingPlayerStat.MainStruct plStatistic)
        {
            try
            {
                string[] headerPlStat = HeaderPlayerStatistic.Split(',');

                excelWorksheet.Name = "PlayerStat";

                // write header and Data
                for (int i = 0; i < headerPlStat.Length; i++)
                    excelWorksheet.Cells[2, 2 + i] = headerPlStat[i];

                excelWorksheet.Cells[3, 2] = plStatistic.PlayerIndex;
                excelWorksheet.Cells[3, 3] = plStatistic.PlayerName.TrimEnd();
                excelWorksheet.Cells[3, 4] = plStatistic.TotalGames;
                excelWorksheet.Cells[3, 5] = plStatistic.TotalLegsWin;
                excelWorksheet.Cells[3, 6] = plStatistic.TotalSetsWin;
                excelWorksheet.Cells[3, 7] = plStatistic.TotalWin;
                excelWorksheet.Cells[3, 8] = plStatistic.TotalLoss;
                excelWorksheet.Cells[3, 9] = plStatistic.TotalGameTime;
                excelWorksheet.Cells[3, 10] = plStatistic.TotalThrowTime;
                excelWorksheet.Cells[3, 11] = plStatistic.TotalThrows;
                excelWorksheet.Cells[3, 12] = plStatistic.TotalPoints;
                excelWorksheet.Cells[3, 13] = plStatistic.TotalAvgPosition;

                return excelWorksheet;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return excelWorksheet;
            }
        }
        private Excel._Worksheet CreatePlayerGameDataXLSX(Excel._Worksheet excelWorksheet, DatahandlingPlayerStat.MainStruct plStatistic)
        {
            try
            {
                excelWorksheet.Name = "GameStatistic";
                // Get all GameDatasFiles
                DirectoryInfo directoryInfo = new DirectoryInfo(Configuration.FilePathDDDG3D);
                var files = directoryInfo.GetFiles("*" + Configuration.FileExtensionG3D, SearchOption.AllDirectories);
                int countFiles = files.Length;
                int countRow = 1;
                int countGame = 1;

                for (int i = 0; i < countFiles; i++)
                {
                    FileInfo fileInfo = new FileInfo(files[i].FullName);
                    if (fileInfo.Exists)
                        using (DatahandlingGame dhGame = new DatahandlingGame(fileInfo.FullName))
                        {
                            dhGame.Load();
                            int countPlayer = dhGame.GameStat.Header.CountPlayer;

                            for (int j = 0; j < countPlayer; j++)
                                if (plStatistic.PlayerIndex == dhGame.GameStat.Main.MainPlayer[j].Index)
                                {
                                    var header01 = HeaderPlayerGameStatistic01.Split(',');
                                    var header02 = HeaderPlayerGameStatistic02.Split(',');
                                    var header03 = HeaderPlayerGameStatistic03.Split(',');

                                    // Header game write
                                    excelWorksheet.Cells[countRow, 1] = files[i].Name;
                                    countRow++;

                                    #region player
                                    // Pre header game write
                                    for (int k = 0; k < header01.Length; k++)
                                        excelWorksheet.Cells[countRow, 2 + k] = header01[k];
                                    countRow++;

                                    //sb.Append("Index,PlayerName,FirstName,LastName,Country,BirthYear,BirthMonth,BirthDay,"); // Player
                                    for (int k = 0; k < dhGame.GameStat.Main.MainPlayer.Length; k++)
                                    {
                                        excelWorksheet.Cells[countRow, 2] = dhGame.GameStat.Main.MainPlayer[k].Index;
                                        excelWorksheet.Cells[countRow, 3] = dhGame.GameStat.Main.MainPlayer[k].PlayerName;
                                        excelWorksheet.Cells[countRow, 4] = dhGame.GameStat.Main.MainPlayer[k].FirstName;
                                        excelWorksheet.Cells[countRow, 5] = dhGame.GameStat.Main.MainPlayer[k].LastName;
                                        excelWorksheet.Cells[countRow, 6] = dhGame.GameStat.Main.MainPlayer[k].Country;
                                        excelWorksheet.Cells[countRow, 7] = dhGame.GameStat.Main.MainPlayer[k].BirthYear;
                                        excelWorksheet.Cells[countRow, 8] = dhGame.GameStat.Main.MainPlayer[k].BirthMonth;
                                        excelWorksheet.Cells[countRow, 9] = dhGame.GameStat.Main.MainPlayer[k].BirthDay;
                                        countRow++;
                                    }
                                    countRow++;
                                    #endregion
                                    #region leg
                                    // Pre header game write
                                    for (int k = 0; k < header02.Length; k++)
                                        excelWorksheet.Cells[countRow, 2 + k] = header02[k];
                                    countRow++;

                                    //sb.Append(",PlayerIndex,PlayerName,DateTime,FinishScore,WinnerThrows,"); // Leg
                                    for (int k = 0; k < dhGame.GameStat.Main.MainLeg.Length; k++)
                                    {
                                        excelWorksheet.Cells[countRow, 2] = dhGame.GameStat.Main.MainLeg[k].CountPlayerIndex;
                                        excelWorksheet.Cells[countRow, 3] = dhGame.GameStat.Main.MainLeg[k].PlayerName;
                                        excelWorksheet.Cells[countRow, 4] = Datahandling.ConvertTicksToDateTime(dhGame.GameStat.Main.MainLeg[k].DateTime);
                                        excelWorksheet.Cells[countRow, 5] = dhGame.GameStat.Main.MainLeg[k].CountFinishScore;
                                        excelWorksheet.Cells[countRow, 6] = dhGame.GameStat.Main.MainLeg[k].CountLegWinnerThrows;
                                        countRow++;
                                    }
                                    countRow++;
                                    #endregion
                                    #region game
                                    for (int k = 0; k < header03.Length; k++)
                                        excelWorksheet.Cells[countRow, 2 + k] = header03[k];
                                    countRow++;

                                    for (int k = 0; k < dhGame.GameStat.Main.MainGameStatistic.Length; k++)
                                    {
                                        excelWorksheet.Cells[countRow, 2] = dhGame.GameStat.Main.MainGameStatistic[k].CountIndexThrow;
                                        excelWorksheet.Cells[countRow, 3] = dhGame.GameStat.Main.MainGameStatistic[k].CountIndexPlayer;
                                        excelWorksheet.Cells[countRow, 4] = dhGame.GameStat.Main.MainGameStatistic[k].PlayerName;
                                        excelWorksheet.Cells[countRow, 5] = dhGame.GameStat.Main.MainGameStatistic[k].CountRounds;
                                        excelWorksheet.Cells[countRow, 6] = dhGame.GameStat.Main.MainGameStatistic[k].CountScore;
                                        excelWorksheet.Cells[countRow, 7] = dhGame.GameStat.Main.MainGameStatistic[k].IsScoreOverthrown;
                                        excelWorksheet.Cells[countRow, 8] = dhGame.GameStat.Main.MainGameStatistic[k].CountFinish;
                                        excelWorksheet.Cells[countRow, 9] = dhGame.GameStat.Main.MainGameStatistic[k].IsFinishPossible;
                                        excelWorksheet.Cells[countRow, 10] = Datahandling.ConvertTicksToDateTime(dhGame.GameStat.Main.MainGameStatistic[k].CountDateTime);
                                        countRow++;
                                    }
                                    countRow = countRow + 2;
                                    #endregion

                                    countGame++;
                                    break;
                                }

                        }
                }
                return excelWorksheet;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return excelWorksheet;
            }
        }
        private Excel._Worksheet CreateGameDataXLSX(Excel._Worksheet excelWorksheet, string gameName)
        {
            try
            {
                excelWorksheet.Name = "GameStatistic";
                // Get all GameDatasFiles
                int countRow = 1;
                int countGame = 1;

                FileInfo fileInfo = new FileInfo(gameName);
                if (fileInfo.Exists)
                {
                    using (DatahandlingGame dhGame = new DatahandlingGame(fileInfo.FullName))
                    {
                        dhGame.Load();
                        int countPlayer = dhGame.GameStat.Header.CountPlayer;

                        var header01 = HeaderPlayerGameStatistic01.Split(',');
                        var header02 = HeaderPlayerGameStatistic02.Split(',');
                        var header03 = HeaderPlayerGameStatistic03.Split(',');

                        // Header game write
                        excelWorksheet.Cells[countRow, 1] = fileInfo.Name;
                        countRow++;

                        #region player
                        // Pre header game write
                        for (int k = 0; k < header01.Length; k++)
                            excelWorksheet.Cells[countRow, 2 + k] = header01[k];
                        countRow++;

                        //sb.Append("Index,PlayerName,FirstName,LastName,Country,BirthYear,BirthMonth,BirthDay,"); // Player
                        for (int k = 0; k < dhGame.GameStat.Main.MainPlayer.Length; k++)
                        {
                            excelWorksheet.Cells[countRow, 2] = dhGame.GameStat.Main.MainPlayer[k].Index;
                            excelWorksheet.Cells[countRow, 3] = dhGame.GameStat.Main.MainPlayer[k].PlayerName;
                            excelWorksheet.Cells[countRow, 4] = dhGame.GameStat.Main.MainPlayer[k].FirstName;
                            excelWorksheet.Cells[countRow, 5] = dhGame.GameStat.Main.MainPlayer[k].LastName;
                            excelWorksheet.Cells[countRow, 6] = dhGame.GameStat.Main.MainPlayer[k].Country;
                            excelWorksheet.Cells[countRow, 7] = dhGame.GameStat.Main.MainPlayer[k].BirthYear;
                            excelWorksheet.Cells[countRow, 8] = dhGame.GameStat.Main.MainPlayer[k].BirthMonth;
                            excelWorksheet.Cells[countRow, 9] = dhGame.GameStat.Main.MainPlayer[k].BirthDay;
                            countRow++;
                        }
                        countRow++;
                        #endregion
                        #region leg
                        // Pre header game write
                        for (int k = 0; k < header02.Length; k++)
                            excelWorksheet.Cells[countRow, 2 + k] = header02[k];
                        countRow++;

                        //sb.Append(",PlayerIndex,PlayerName,DateTime,FinishScore,WinnerThrows,"); // Leg
                        for (int k = 0; k < dhGame.GameStat.Main.MainLeg.Length; k++)
                        {
                            excelWorksheet.Cells[countRow, 2] = dhGame.GameStat.Main.MainLeg[k].CountPlayerIndex;
                            excelWorksheet.Cells[countRow, 3] = dhGame.GameStat.Main.MainLeg[k].PlayerName;
                            excelWorksheet.Cells[countRow, 4] = Datahandling.ConvertTicksToDateTime(dhGame.GameStat.Main.MainLeg[k].DateTime);
                            excelWorksheet.Cells[countRow, 5] = dhGame.GameStat.Main.MainLeg[k].CountFinishScore;
                            excelWorksheet.Cells[countRow, 6] = dhGame.GameStat.Main.MainLeg[k].CountLegWinnerThrows;
                            countRow++;
                        }
                        countRow++;
                        #endregion
                        #region game
                        for (int k = 0; k < header03.Length; k++)
                            excelWorksheet.Cells[countRow, 2 + k] = header03[k];
                        countRow++;

                        for (int k = 0; k < dhGame.GameStat.Main.MainGameStatistic.Length; k++)
                        {
                            excelWorksheet.Cells[countRow, 2] = dhGame.GameStat.Main.MainGameStatistic[k].CountIndexThrow;
                            excelWorksheet.Cells[countRow, 3] = dhGame.GameStat.Main.MainGameStatistic[k].CountIndexPlayer;
                            excelWorksheet.Cells[countRow, 4] = dhGame.GameStat.Main.MainGameStatistic[k].PlayerName;
                            excelWorksheet.Cells[countRow, 5] = dhGame.GameStat.Main.MainGameStatistic[k].CountRounds;
                            excelWorksheet.Cells[countRow, 6] = dhGame.GameStat.Main.MainGameStatistic[k].CountScore;
                            excelWorksheet.Cells[countRow, 7] = dhGame.GameStat.Main.MainGameStatistic[k].IsScoreOverthrown;
                            excelWorksheet.Cells[countRow, 8] = dhGame.GameStat.Main.MainGameStatistic[k].CountFinish;
                            excelWorksheet.Cells[countRow, 9] = dhGame.GameStat.Main.MainGameStatistic[k].IsFinishPossible;
                            excelWorksheet.Cells[countRow, 10] = Datahandling.ConvertTicksToDateTime(dhGame.GameStat.Main.MainGameStatistic[k].CountDateTime);
                            countRow++;
                        }
                        countRow = countRow + 2;
                        #endregion

                        countGame++;
                    }
                }
                return excelWorksheet;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return excelWorksheet;
            }
        }
        #endregion
        #region class library
        public struct MainPlayerStruct
        {
            public int Index { get; set; }
            public string PlayerName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Country { get; set; }
            public DateTime Birthday { get; set; }
        }
        public struct MainLegsStruct
        {
            public int CountPlayerIndex { get; set; }
            public string PlayerName { get; set; }
            public DateTime DateTime { get; set; }
            public int CountFinishScore { get; set; }
            public int CountLegWinnerThrows { get; set; }
        }
        public struct MainGameStatisticStruct
        {
            public int CountIndexThrow { get; set; }
            public int CountIndexPlayer { get; set; }
            public string PlayerName { get; set; }
            public int CountRounds { get; set; }
            public int CountScore { get; set; }
            public bool IsScoreOverthrown { get; set; }
            public int CountFinish { get; set; }
            public bool IsFinishPossible { get; set; }
            public DateTime CountDateTime { get; set; }
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
        // ~Statistic()
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

    /// <summary>
    /// Configuration data for DDD
    /// </summary>
    class Configuration : IDisposable
    {
        #region constructor
        public Configuration() : this(false) {; }
        public Configuration(bool setRegistry)
        {
            SetRegistry = setRegistry;
            FirstExecute();
        }
        #endregion
        #region static fields, properties, constants
        // static fields
        // file
        public static string FileNameG3D = "Game_ddMMyyyy_hhmmss" + FileExtensionG3D;
        public static string FileNameEditG3D = "Game_X" + FileExtensionG3D;
        public static string FileNameP3D = "PlayerData" + FileExtensionP3D;
        public static string FileNameS3D = "PlayerStat" + FileExtensionS3D;
        public static string FilePathDocuments =
            System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string FilePathDDDRoot = "";
        public static string FilePathDDDG3D = "";
        public static string FilePathDDDP3D = "";
        public static string FilePathDDDS3D = "";
        // DDD
        public static string DDDVersion =
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", "_");
        public static string DDD = "DDD";
        public static string DDDPathRoot = "";
        public static string DDDPathG3D = "";
        public static string DDDPathS3D = "";
        public static string DDDPathP3D = "";
        // Registry
        public static string RegFileDDD = @"DrunkenDudeDarts";
        public static string RegFileVersion = @"Version";
        public static string RegFileExtensions = @"FileExtensionsExImp";
        public static string RegFileG3D = @"PathG3D";
        public static string RegFileP3D = @"PathP3D";
        public static string RegFileS3D = @"PathS3D";
        public static string RegFileCurrentUser = @"HKEY_CURRENT_USER";
        public static string RegFileSoftware = @"Software";
        public static string RegFilePath = RegFileCurrentUser + @"\" + RegFileSoftware + @"\";
        public static string RegFilePathDDDRoot = RegFilePath + RegFileDDD;
        public static string RegFilePathDDDVersion = RegFilePathDDDRoot + @"\" + RegFileVersion;
        public static string RegFilePathDDDExtensions = RegFilePathDDDRoot + @"\" + RegFileExtensions;
        public static string RegFilePathDDDG3D = RegFilePathDDDRoot + @"\" + RegFileG3D;
        public static string RegFilePathDDDP3D = RegFilePathDDDRoot + @"\" + RegFileP3D;
        public static string RegFilePathDDDS3D = RegFilePathDDDRoot + @"\" + RegFileS3D;
        // ML model
        public static string MLModelPath;
        public static string[] MLModels = new string[] { "DartBoard_L.onnx", "DartBoard_L_old.onnx", "DartBoard_M.onnx", "DartBoard_S.onnx" };
        // Ini file 
        public static string FileNameIni = "DDD" + FileExtensionINI;
        public static string FilePathIni = new string("");


        // Properties
        private bool _SetRegistry { get; set; }
        public bool SetRegistry
        {
            get { return _SetRegistry; }
            set
            {
                Checking[(int)CheckingBools.RegDeleteBecauseDataRefresh] = true;
                _SetRegistry = value;
            }
        }

        // Fields
        public bool[] Checking = new bool[40]
        {
            false,false,false,false,false,false,false,false,false,false,
            false,false,false,false,false,false,false,false,false,false,
            false,false,false,false,false,false,false,false,false,false,
            false,false,false,false,false,false,false,false,false,false
        };
        // Constants
        public const string FileExtensionG3D = ".G3D";
        public const string FileExtensionP3D = ".P3D";
        public const string FileExtensionS3D = ".S3D";
        public const string FileExtensionINI = ".INI";
        public const string FileExtensionsExImp = ".G3D/.P3D/.S3D";
        public const string FileExtensionsAll = ".G3D/.P3D/.S3D/.INI";

        #endregion
        #region static methods
        public static void FirstExecute()
        {
            FilePathDDDRoot = FilePathDocuments + @"\DDD_" + DDDVersion + @"\";
            FilePathDDDG3D = FilePathDDDRoot + @"Game_Data\";
            FilePathDDDP3D = FilePathDDDRoot + FileNameP3D;
            FilePathDDDS3D = FilePathDDDRoot + FileNameS3D;
            MLModelPath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("DDD.dll", @"Assets\Models\");
        }
        
        #endregion
        #region methods
        #region registry
        /// <summary>
        /// Checking registry, description for items to check in enum CheckingBools
        /// </summary>
        /// <param name="action">Case 1: Check path / 
        /// Case 2: Check if path is correct / 
        /// Case 3: Check if refresh data</param>
        /// <returns>Checking bool array</returns>
        public bool[] CheckRegistry(int action)
        {
            switch (action)
            {
                case 1:
                    {
                        Checking[0] = true;
                        Checking[1] = true;
                        Checking[2] = true;
                        Checking[3] = true;
                        Checking[4] = true;
                        Checking[5] = true;

                        var Checking00 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, null, null);
                        var Checking01 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileVersion, null);
                        var Checking02 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileExtensions, null);
                        var Checking03 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileG3D, null);
                        var Checking04 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileP3D, null);
                        var Checking05 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileS3D, null);

                        if (Checking00 == null) Checking[(int)CheckingBools.RegPathDDDRootExists] = false;
                        if (Checking01 == null) Checking[(int)CheckingBools.RegPathDDDVersionExists] = false;
                        if (Checking02 == null) Checking[(int)CheckingBools.RegPathDDDExtensionsExists] = false;
                        if (Checking03 == null) Checking[(int)CheckingBools.RegPathDDDG3DExists] = false;
                        if (Checking04 == null) Checking[(int)CheckingBools.RegPathDDDP3DExists] = false;
                        if (Checking05 == null) Checking[(int)CheckingBools.RegPathDDDS3DExists] = false;
                    }
                    break;
                case 2:
                    {
                        Checking[6] = false;
                        Checking[7] = false;
                        Checking[8] = false;
                        Checking[9] = false;
                        Checking[10] = false;

                        var Checking00 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, null, null);
                        var Checking01 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileVersion, null);
                        var Checking02 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileExtensions, null);
                        var Checking03 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileG3D, null);
                        var Checking04 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileP3D, null);
                        var Checking05 = Microsoft.Win32.Registry.GetValue(RegFilePathDDDRoot, RegFileS3D, null);

                        if (Checking01 != null)
                            if (Checking01.ToString() == DDDVersion) Checking[(int)CheckingBools.RegDDDCompareVersion] = true;
                        if (Checking02 != null)
                            if (Checking02.ToString() == FileExtensionsExImp) Checking[(int)CheckingBools.RegDDDCompareExtension] = true;
                        if (Checking03 != null)
                            if (Checking03.ToString() == FilePathDDDG3D) Checking[(int)CheckingBools.RegDDDCompareFileG3D] = true;
                        if (Checking04 != null)
                            if (Checking04.ToString() == FilePathDDDP3D) Checking[(int)CheckingBools.RegDDDCompareFileP3D] = true;
                        if (Checking05 != null)
                            if (Checking05.ToString() == FilePathDDDS3D) Checking[(int)CheckingBools.RegDDDCompareFileS3D] = true;
                    }
                    break;
                case 3:
                    {
                        Checking[11] = false;
                        for (int i = 6; i <= 10; i++)
                        {
                            if (Checking[i] == false)
                            {
                                Checking[(int)CheckingBools.RegDeleteBecauseDataRefresh] = true;
                                Checking[(int)CheckingBools.UserMsgActive] = true;
                                Checking[(int)CheckingBools.UserMsgRegistryChanged] = true;
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return Checking;
        }
        public bool RegistryWrite(bool[] checking)
        {
            try
            {
                if (checking[(int)CheckingBools.RegPathDDDRootExists] == false)
                    Microsoft.Win32.Registry.SetValue(RegFilePathDDDRoot, null, 0);

                if (checking[(int)CheckingBools.RegPathDDDVersionExists] == false)
                    Microsoft.Win32.Registry.SetValue(RegFilePathDDDRoot, RegFileVersion, DDDVersion);

                if (checking[(int)CheckingBools.RegPathDDDExtensionsExists] == false)
                    Microsoft.Win32.Registry.SetValue(RegFilePathDDDRoot, RegFileExtensions, FileExtensionsExImp);

                if (checking[(int)CheckingBools.RegPathDDDG3DExists] == false)
                    Microsoft.Win32.Registry.SetValue(RegFilePathDDDRoot, RegFileG3D, FilePathDDDG3D);

                if (checking[(int)CheckingBools.RegPathDDDP3DExists] == false)
                    Microsoft.Win32.Registry.SetValue(RegFilePathDDDRoot, RegFileP3D, FilePathDDDP3D);

                if (checking[(int)CheckingBools.RegPathDDDS3DExists] == false)
                    Microsoft.Win32.Registry.SetValue(RegFilePathDDDRoot, RegFileS3D, FilePathDDDS3D);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return false;
            }
        }
        public bool RegistryDelete()
        {
            if (Checking[(int)CheckingBools.RegDeleteBecauseDataRefresh])
                try
                {
                    string keyName = RegFileSoftware;
                    using (Microsoft.Win32.RegistryKey key =
                        Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyName, true))
                    {
                        if (key != null)
                        {
                            key.DeleteSubKey(RegFileDDD);
                            Checking[(int)CheckingBools.RegDeleteBecauseDataRefresh] = false;
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
                    return false;
                }
            return true;
        }
        #endregion
        #region file path
        /// <summary>
        /// Checking file path, description for items to check in enum CheckingBools
        /// </summary>
        /// <param name="action">Case 1: Check file path / 
        /// Case 2: Check if delete old data / 
        /// Case 3: Check if write new data</param>
        /// <returns>Checking bool array</returns>
        public bool[] CheckFilePath(int action)
        {
            DirectoryInfo diG3D = new DirectoryInfo(FilePathDDDG3D);
            FileInfo diS3D = new FileInfo(FilePathDDDS3D);
            FileInfo diP3D = new FileInfo(FilePathDDDP3D);
            DirectoryInfo diRoot = new DirectoryInfo(FilePathDDDRoot);

            switch (action)
            {
                case 1:
                    {
                        Checking[20] = true;
                        Checking[21] = true;
                        Checking[22] = true;
                        Checking[23] = true;
                        Checking[26] = true;

                        if (diRoot.Exists == false)
                            Checking[(int)CheckingBools.FilePathRootExists] = false;
                        if (diP3D.Exists == false)
                            Checking[(int)CheckingBools.FilePathP3DExists] = false;
                        if (diS3D.Exists == false)
                            Checking[(int)CheckingBools.FilePathS3DExists] = false;
                        if (diG3D.Exists == false)
                            Checking[(int)CheckingBools.FilePathG3DExists] = false;
                    }
                    break;
                case 2:
                    {
                        var di = Directory.GetDirectories(
                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                        string[] pathsDDD = new string[di.Length];
                        int CountPaths = 0;
                        Checking[(int)CheckingBools.FilePathOldExist] = false;

                        for (int i = 0; i < di.Length; i++)
                        {
                            int diDDDLength = di[i].LastIndexOf(DDD);
                            if (diDDDLength > 0)
                                pathsDDD[i] = di[i];
                        }

                        for (int i = 0; i < di.Length; i++)
                        {
                            if (pathsDDD[i] != "")
                                CountPaths++;
                        }
                        if (CountPaths > 1)
                            Checking[24] = true;
                    }
                    break;
                case 3:
                    {
                        Checking[(int)CheckingBools.FilePathWriteNew] = false;
                        for (int i = 20; i <= 23; i++)
                            if (Checking[i] == false)
                            {
                                Checking[(int)CheckingBools.FilePathWriteNew] = true;
                                Checking[(int)CheckingBools.UserMsgActive] = true;
                                Checking[(int)CheckingBools.UserMsgDataCreated] = true;
                                break;
                            }

                    }
                    break;
                default:
                    break;
            }
            return Checking;
        }
        public bool FilePathCreate()
        {
            if (Checking[20] == false)
            {
                if (_CreateRoot())
                    Checking[20] = true;
            }
            if (Checking[21] == false)
            {
                if (_CreatePathG3D())
                    Checking[21] = true;
            }
            if (Checking[22] == false)
            {
                if (_CreateP3DData())
                    Checking[22] = true;
            }
            if (Checking[23] == false)
            {
                if (_CreateS3DData())
                    Checking[23] = true;
            }
            if (Checking[26] == false)
            {
                if (_CreateMLPath())
                    Checking[26] = true;
            }
            if (Checking[27] == false)
            {
                if (_CreateIniFile())
                    Checking[27] = true;
            }

            return true;

            bool _CreateRoot()
            {
                try
                {
                    var asdf = Directory.CreateDirectory(FilePathDDDRoot);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
                    return false;
                }
            }
            bool _CreatePathG3D()
            {
                try
                {
                    Directory.CreateDirectory(FilePathDDDG3D);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
                    return false;
                }
            }
            bool _CreateP3DData()
            {
                try
                {
                    Datahandling dh = new DatahandlingPlayer(FilePathDDDP3D);
                    dh.Dispose();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
                    return false;
                }
            }
            bool _CreateS3DData()
            {
                try
                {
                    Datahandling dhps = new DatahandlingPlayerStat(FilePathDDDS3D);
                    dhps.Dispose();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
                    return false;
                }
            }
            bool _CreateMLPath()
            {
                try
                {
                    MLModelPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
                    return false;
                }
            }
            bool _CreateIniFile()
            {
                try
                {
                    
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
                    return false;
                }
            }
        }
        #endregion
        #region call messagebox
        public void UserMsgCall()
        {
            if (Checking[(int)CheckingBools.UserMsgRegistryChanged])
                MessageBox.Show("Updated game data");
            if (Checking[(int)CheckingBools.UserMsgDataCreated])
                MessageBox.Show("Created new files in: \n" + FilePathDDDRoot);
        }
        #endregion
        #endregion
        #region enumeration
        public enum CheckingBools
        {
            RegPathDDDRootExists = 0,
            RegPathDDDVersionExists,
            RegPathDDDExtensionsExists,
            RegPathDDDG3DExists,
            RegPathDDDP3DExists,
            RegPathDDDS3DExists,
            RegDDDCompareVersion,
            RegDDDCompareExtension,
            RegDDDCompareFileG3D,
            RegDDDCompareFileP3D,
            RegDDDCompareFileS3D,
            RegDeleteBecauseDataRefresh,
            FilePathRootExists = 20,
            FilePathG3DExists,
            FilePathP3DExists,
            FilePathS3DExists,
            FilePathOldExist,
            FilePathWriteNew,
            FilePathMLModel,
            UserMsgActive = 30,
            UserMsgRegistryChanged,
            UserMsgDataCreated
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
        ~Configuration()
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
        #region IniFile
        /// <summary>
        /// Create a New INI file to store or load data
        /// </summary>
        public class IniFile
        {
            public string path;

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section,
                string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section,
                     string key, string def, StringBuilder retVal,
                int size, string filePath);

            /// <summary>
            /// INIFile Constructor.
            /// </summary>
            /// <PARAM name="INIPath"></PARAM>
            public IniFile(string INIPath)
            {
                path = INIPath;
            }
            /// <summary>
            /// Write Data to the INI File
            /// </summary>
            /// <PARAM name="Section"></PARAM>
            /// Section name
            /// <PARAM name="Key"></PARAM>
            /// Key Name
            /// <PARAM name="Value"></PARAM>
            /// Value Name
            public void IniWriteValue(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, this.path);
            }

            /// <summary>
            /// Read Data Value From the Ini File
            /// </summary>
            /// <PARAM name="Section"></PARAM>
            /// <PARAM name="Key"></PARAM>
            /// <PARAM name="Path"></PARAM>
            /// <returns></returns>
            public string IniReadValue(string Section, string Key)
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(Section, Key, "", temp,
                                                255, this.path);
                return temp.ToString();

            }

            public enum IniSections
            {
                Configuration = 1,
                Game = 10,
                Statistic = 20
            }
            public enum IniKey
            {
                ConfigVideo = 1,
                ConfigVideo_BOOL_PredAuto,
                ConfigVideo_STRING_WebcamName,
                GameDifficulty = 10
            }
        }
        #endregion
    }

    /// <summary>
    /// Bot
    /// </summary>
    public abstract class Bot
    {
        #region constructor
        public Bot() : this(0) { }
        public Bot(int difficulty)
        {
            Difficulty = difficulty;
            CounterBots++;
        }
        public Bot(int difficulty, Person person)
        {
            Difficulty = difficulty;
            GetPerson = person;

            CounterBots++;
        }
        public Bot(int difficulty, string name, long birthdayAsTicks)
        {
            Difficulty = difficulty;
            Name = name;
            BirthdayAsTicks = birthdayAsTicks;

            CounterBots++;
        }
        #endregion
        #region properties
        private int _Difficulty { get; set; }
        public virtual int Difficulty { get { return _Difficulty; } set { _Difficulty = value; } }

        private Person _GetPerson { get; set; }
        public virtual Person GetPerson
        {
            get { return _GetPerson; }
            set
            {
                _GetPerson = value;
            }
        }

        private string _Name { get; set; }
        public virtual string Name
        {
            get { return _Name; }
            set
            {
                var person = new Person();
                if (value.Length == 0) _Name = person.GetName();
                else _Name = value;
            }
        }

        private long _BirthdayAsTicks { get; set; }
        public virtual long BirthdayAsTicks
        {
            get { return _BirthdayAsTicks; }
            set
            {
                var person = new Person();
                if (value == 0) _BirthdayAsTicks = person.GetBirthdayAsTicks();
                else _BirthdayAsTicks = value;
            }
        }

        public virtual double RandomNumberDbl
        {
            get { return GetRandomNumber.NextDouble() * 10; }
        }
        public virtual int RandomNumberInt
        {
            get { return (int)(Math.Round(GetRandomNumber.NextDouble() * int.MaxValue)); }
        }
        #endregion
        #region instance methods
        //public abstract void Processing(int Value01, int Value02);
        //public abstract int GetValue();
        //public abstract int[] GetCoordinates();
        //public abstract int GetProbability(int Value01);
        //public abstract int GetNeededScore(int Value01);
        #endregion
        #region class properties
        public static int CounterBots { get; private set; }
        private static readonly Random GetRandomNumber = new Random();
        #endregion
        #region class methods
        public virtual int RandomNumber(int min, int max)
        {
            return GetRandomNumber.Next(min, max);
        }

        #endregion
        #region class library
        public class Person
        {
            #region properties
            private string _Name { get; set; }
            public string Name
            {
                get { return _Name; }
                set
                {
                    if (value == "") _Name = GetName();
                    else _Name = value;
                }
            }

            private long _BirthdayAsTicks { get; set; }
            public long BirthdayAsTicks
            {
                get { return _BirthdayAsTicks; }
                set
                {
                    if (value == 0) _BirthdayAsTicks = GetBirthdayAsTicks();
                    else _BirthdayAsTicks = value;
                }
            }
            #endregion
            #region class properties
            private static string[] CreateNames
            {
                get
                {
                    string[] _names = new string[50];
                    _names[0] = "Lavonda";
                    _names[1] = "Maragret";
                    _names[2] = "Clemencia";
                    _names[3] = "Cathryn";
                    _names[4] = "Jackeline";
                    _names[5] = "Maryellen";
                    _names[6] = "Ardella";
                    _names[7] = "Retha";
                    _names[8] = "Lianne";
                    _names[9] = "Breanna";
                    _names[10] = "Carly";
                    _names[11] = "Kayce";
                    _names[12] = "Shirlee";
                    _names[13] = "Huong";
                    _names[14] = "Nakita";
                    _names[15] = "Aundrea";
                    _names[16] = "Adelia";
                    _names[17] = "Rufina";
                    _names[18] = "Lakia";
                    _names[19] = "Delbert";
                    _names[20] = "Britany";
                    _names[21] = "Rhonda";
                    _names[22] = "Emmitt";
                    _names[23] = "Tisha";
                    _names[24] = "Dionne";
                    _names[25] = "Tammie";
                    _names[26] = "Lizette";
                    _names[27] = "Scottie";
                    _names[28] = "Catalina";
                    _names[29] = "Brinda";
                    _names[30] = "Zenobia";
                    _names[31] = "Emilia";
                    _names[32] = "Marquita";
                    _names[33] = "Kathrin";
                    _names[34] = "Winter";
                    _names[35] = "Eddie";
                    _names[36] = "Mariela";
                    _names[37] = "Belinda";
                    _names[38] = "Sharie";
                    _names[39] = "Francesca";
                    _names[40] = "Kirsten";
                    _names[41] = "Trina";
                    _names[42] = "Gertrude";
                    _names[43] = "Victor";
                    _names[44] = "Kayla";
                    _names[45] = "Belva";
                    _names[46] = "Estelle";
                    _names[47] = "Marcie";
                    _names[48] = "Zachariah";
                    _names[49] = "Josette";
                    return _names;
                }
            }
            private static long[] CreateBirthdayAsTicks
            {
                get
                {
                    long[] _birthday = new long[50];
                    for (int i = 0; i < 50; i++)
                    {
                        int Day = Bot.GetRandomNumber.Next(1, 28);
                        int Month = Bot.GetRandomNumber.Next(1, 12);
                        int Year = Bot.GetRandomNumber.Next(1900, 2010);
                        DateTime dateTime = new DateTime(Year, Month, Day);

                        _birthday[i] = dateTime.Ticks;
                    }

                    return _birthday;
                }

            }
            #endregion
            #region instance methods
            public string GetName()
            {
                return CreateNames[GetRandomNumber.Next(0, 49)];
            }
            public long GetBirthdayAsTicks()
            {
                return CreateBirthdayAsTicks[GetRandomNumber.Next(0, 49)];
            }
            #endregion
        }
        #endregion
    }
    public class DartBot : Bot, IDisposable
    {
        #region constructor
        public DartBot() : base()
        {
            CounterDartBots++;
            FirstExecute();
        }
        public DartBot(int difficulty) : base(difficulty)
        {
            this.Difficulty = difficulty;
            CounterDartBots++;
            FirstExecute();
        }
        public DartBot(int difficulty, Bot.Person person) : base(difficulty, person)
        {
            this.Difficulty = difficulty;
            CounterDartBots++;
            FirstExecute();
        }
        public DartBot(int difficulty, string name, long birthdayAsTicks) : base(difficulty, name, birthdayAsTicks)
        {
            this.Difficulty = difficulty;
            CounterDartBots++;
            FirstExecute();
        }
        #endregion
        #region class properties
        public static int CounterDartBots { get; private protected set; }

        private static int _CountThrows { get; set; }
        public static int CountThrows { get => _CountThrows; private set => _CountThrows = value; }

        private static ScoringTable.FinishSingle _ActualThrows { get; set; }
        public static ScoringTable.FinishSingle ActualThrows { get => _ActualThrows; private set => _ActualThrows = value; }

        private static Scoring _Scores;
        public static Scoring Scores { get => _Scores; private set => _Scores = value; }

        #endregion
        #region Methods
        private void FirstExecute()
        {

        }
        private ScoringTable.Scoring GetThrows(int score)
        {
            ScoringTable scoringTable = new ScoringTable();
            ScoringTable.Scoring scoringPoints = new ScoringTable.Scoring();
            if (ScoringTable.CheckFinishExists(score))
                scoringTable.GetFinishTable().TryGetValue(score, out scoringPoints);
            else
                scoringTable.keyScoringTable.TryGetValue(1, out scoringPoints);                
                
            return scoringPoints;
        }
        private ScoringTable.FinishSingle GetFinishSingle(ScoringTable.Scoring finish)
        {
            Func<ScoringTable.Scoring, int> funcGetComnination = ((x) => 
            {
                int countThrow = 0;
                if (x.No_01_Score_01_Mode > 0)
                    countThrow++;
                if (x.No_02_Score_01_Mode > 0)
                    countThrow++;
                if (x.No_03_Score_01_Mode > 0)
                    countThrow++;

                return countThrow;
            });

            ScoringTable.FinishSingle finishSingle = new ScoringTable.FinishSingle();
            int countRandom = BoardParameter.RandomInt(1, funcGetComnination(finish));

            if (countRandom == 1)
            {
                finishSingle.Score_01 = finish.No_01_Score_01;
                finishSingle.Score_02 = finish.No_01_Score_02;
                finishSingle.Score_03 = finish.No_01_Score_03;
                finishSingle.Score_01_Mode = finish.No_01_Score_01_Mode;
                finishSingle.Score_02_Mode = finish.No_01_Score_02_Mode;
                finishSingle.Score_03_Mode = finish.No_01_Score_03_Mode;
            }
            else if (countRandom == 2)
            {
                finishSingle.Score_01 = finish.No_02_Score_01;
                finishSingle.Score_02 = finish.No_02_Score_02;
                finishSingle.Score_03 = finish.No_02_Score_03;
                finishSingle.Score_01_Mode = finish.No_02_Score_01_Mode;
                finishSingle.Score_02_Mode = finish.No_02_Score_02_Mode;
                finishSingle.Score_03_Mode = finish.No_02_Score_03_Mode;
            }
            else if (countRandom == 3)
            {
                finishSingle.Score_01 = finish.No_03_Score_01;
                finishSingle.Score_02 = finish.No_03_Score_02;
                finishSingle.Score_03 = finish.No_03_Score_03;
                finishSingle.Score_01_Mode = finish.No_03_Score_01_Mode;
                finishSingle.Score_02_Mode = finish.No_03_Score_02_Mode;
                finishSingle.Score_03_Mode = finish.No_03_Score_03_Mode;
            }

            ActualThrows = finishSingle;

            return finishSingle;
        }
        private int GetThrowCounter(ScoringTable.FinishSingle finish)
        {
            int countThrow = 0;
            if (finish.Score_01_Mode > 0)
                countThrow++;
            if (finish.Score_02_Mode > 0)
                countThrow++;
            if (finish.Score_03_Mode > 0)
                countThrow++;

            CountThrows = countThrow;

            return countThrow;
        }
        public static int[,] TransformFinishSingleToArray(ScoringTable.FinishSingle finishSingle)
        {
            int[,] finishSingleInt = new int[2, 3];

            finishSingleInt[0, 0] = finishSingle.Score_01;
            finishSingleInt[0, 1] = finishSingle.Score_02;
            finishSingleInt[0, 2] = finishSingle.Score_03;

            finishSingleInt[1, 0] = finishSingle.Score_01_Mode;
            finishSingleInt[1, 1] = finishSingle.Score_02_Mode;
            finishSingleInt[1, 2] = finishSingle.Score_03_Mode;

            return finishSingleInt;
        }
        private static ScoringTable.FinishSingle TransformArrayScoreToFinishSingle(int[] score)
        {
            ScoringTable.FinishSingle finishSingle = new ScoringTable.FinishSingle();
            var scoringToMultiplier = new ScoringTable().GetScoringToMultiplier();
            var scoringToMultiplierSingle = new ScoringTable.ScoringToMultiplier[3];


            scoringToMultiplier.TryGetValue(score[0], out scoringToMultiplierSingle[0]);
            scoringToMultiplier.TryGetValue(score[1], out scoringToMultiplierSingle[1]);
            scoringToMultiplier.TryGetValue(score[2], out scoringToMultiplierSingle[2]);

            finishSingle.Score_01 = (byte)scoringToMultiplierSingle[0].Score;
            finishSingle.Score_02 = (byte)scoringToMultiplierSingle[1].Score;
            finishSingle.Score_03 = (byte)scoringToMultiplierSingle[2].Score;

            finishSingle.Score_01_Mode = (byte)scoringToMultiplierSingle[0].Multiplicator;
            finishSingle.Score_02_Mode = (byte)scoringToMultiplierSingle[1].Multiplicator;
            finishSingle.Score_03_Mode = (byte)scoringToMultiplierSingle[2].Multiplicator;

            return finishSingle;
        }
        public static int GetThrows(ScoringTable.FinishSingle finishSingle, int countFinish)
        {
            var finishSingleArray = TransformFinishSingleToArray(finishSingle);
            int countPoints = 0, countGetThrows = 0;
            bool chkOverthrown = new bool();

            for (int i = 0; i < 3; i++)
            {
                countPoints = finishSingleArray[0, i] + countPoints;
                if (countPoints <= countFinish)
                    countGetThrows++;
                else chkOverthrown = true;
            }

            if (chkOverthrown && countGetThrows < 3)
                countGetThrows++;

            return countGetThrows;
        }
        public static int GetScore(int[,] getScore)
        {
            int score = new int();
            for (int i = 0; i < 3; i++)
            {
                if (getScore[1, i] == 4)
                    score = score + 25;
                else if (getScore[1, i] == 5)
                    score = score + 50;
                else score = score + getScore[0, i] * getScore[1, i];
            }
            return score;
        }
        public static ScoringTable.FinishSingle GetThreeScores(int score, int difficulty)
        {
            DDD.DartBot dartBot = new DartBot(difficulty);
            var getProbability = new Probability(dartBot.Difficulty);
            int[] scores = new int[3];
            ScoringTable.FinishSingle scoresFS = new ScoringTable.FinishSingle();
            Probability.Matrix probabilityMatrix;

            var getThrows = dartBot.GetThrows(score);
            var getFinishSingle = dartBot.GetFinishSingle(getThrows);
            var CounterThrows = dartBot.GetThrowCounter(getFinishSingle);
            var getFinishSingleTransform = TransformFinishSingleToArray(getFinishSingle);

            for (int i = 0; i < CounterThrows; i++)
            {
                var countScore = getFinishSingleTransform[0, i];
                var countMultiplicator = (ValueMultiplicator)getFinishSingleTransform[1, i];
                probabilityMatrix = getProbability.GetProbabilityMatrix(countScore, countMultiplicator);
                scores[i] = getProbability.GetScore(probabilityMatrix, (int)countMultiplicator);
            }
            scoresFS = TransformArrayScoreToFinishSingle(scores);
            _Scores.FinishSingle = scoresFS;
            _Scores.CountThrows = GetThrows(scoresFS, Game.GameCounter.CountFinishActual);

            return scoresFS;
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
        ~DartBot()
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
            GC.SuppressFinalize(this);
        }
        #endregion
        #region override method
        public override string ToString()
        {
            return "Counter bots: " + DartBot.CounterDartBots;
        }
        #endregion
        #region class library
        public struct Scoring
        {
            public int CountThrows;
            public ScoringTable.FinishSingle FinishSingle;
        }
        #endregion
        #region enumeration
        public enum ValueMultiplicator
        {
            Nothing = 0,
            Single = 1,
            Double = 2,
            Triple = 3,
            HalfBull = 4,
            Bull = 5
        }
        public enum ValueSingle
        {
            _00 = 0, // Only for error
            _01 = 1,
            _02,
            _03,
            _04,
            _05,
            _06,
            _07,
            _08,
            _09,
            _10,
            _11,
            _12,
            _13,
            _14,
            _15,
            _16,
            _17,
            _18,
            _19,
            _20
        }
        public enum ValueDouble
        {
            _01 = 2,
            _02 = 4,
            _03 = 6,
            _04 = 8,
            _05 = 10,
            _06 = 12,
            _07 = 14,
            _08 = 16,
            _09 = 18,
            _10 = 20,
            _11 = 22,
            _12 = 24,
            _13 = 26,
            _14 = 28,
            _15 = 30,
            _16 = 32,
            _17 = 34,
            _18 = 36,
            _19 = 38,
            _20 = 40
        }
        public enum ValueTriple
        {
            _01 = 3,
            _02 = 6,
            _03 = 9,
            _04 = 12,
            _05 = 15,
            _06 = 18,
            _07 = 21,
            _08 = 24,
            _09 = 27,
            _10 = 30,
            _11 = 33,
            _12 = 36,
            _13 = 39,
            _14 = 42,
            _15 = 45,
            _16 = 48,
            _17 = 51,
            _18 = 54,
            _19 = 57,
            _20 = 60
        }
        public enum ValueBull
        {
            _25 = 25,
            _50 = 50
        }
        public enum ValueCheckOut
        {
            Single = 1,
            Double,
            Triple
        }
        public enum CheckValuesDescription
        {
            NeededScore_FinishSingle = 0,
            NeededScore_FinishDouble,
            NeededScore_FinishTriple,
            NeededScore_FinishPossible,
            NeededScore_FinishTryTripleTwenty,
            NeededScore_FinishScoreNull01,
            NeededScore_FinishScoreNull02,
            NeededScore_FinishScoreNull03,
            Probability_FinishCase01 = 10,
            Probability_FinishCase02,
            Probability_FinishCase03
        }
        #endregion
    }
    public class BoardParameter : IDisposable
    {
        #region constructor
        public BoardParameter() : this(0) { }
        public BoardParameter(int boardFactor)
        {
            BoardFactor = boardFactor;
            scoreDegrees = GetScoreDegrees();
            scoreRadius = GetScoreRadius();
        }
        #endregion
        #region properties and fields
        private static readonly Random Random = new Random();
        // properties
        private int _BoardFactor { get; set; }
        public int BoardFactor
        {
            get
            {
                return _BoardFactor;
            }
            private set
            {
                _BoardFactor = value;
            }
        }

        private const int BoardResolutionWidth = 666; // In px
        private const int BoardResolutionHeight = 666; // In px

        private static Quadrant _ActualQuadrant { get; set; }
        public static Quadrant ActualQuadrant { get => _ActualQuadrant; private set => _ActualQuadrant = value; }

        private static double _ActualDegree { get; set; }
        public static double ActualDegree { get => _ActualDegree; private set => _ActualDegree = value; }

        private static double _ActualLength { get; set; }
        public static double ActualLength { get => _ActualLength; private set => _ActualLength = value; }

        private static DartBot.ValueMultiplicator _ActualRadiusScore { get; set; }
        public static DartBot.ValueMultiplicator ActualRadiusScore { get => _ActualRadiusScore; private set => _ActualRadiusScore = value; }

        private static DartBot.ValueSingle _ActualDegreeScore { get; set; }
        public static DartBot.ValueSingle ActualDegreeScore { get => _ActualDegreeScore; private set => _ActualDegreeScore = value; }

        private static int _ActualScore { get; set; }
        public static int ActualScore { get => _ActualScore; private set => _ActualScore = value; }

        private Dictionary<int, ScoreDegrees> _scoreDegrees { get; set; }
        public Dictionary<int, ScoreDegrees> scoreDegrees
        {
            get
            {
                return _scoreDegrees;
            }
            private set
            {
                _scoreDegrees = value;
            }
        }

        private Dictionary<int, ScoreRadius> _scoreRadius { get; set; }
        public Dictionary<int, ScoreRadius> scoreRadius
        {
            get
            {
                return _scoreRadius;
            }
            private set
            {
                _scoreRadius = value;
            }
        }

        // fields
        public byte[] BoardScore = new byte[20]
        {
            20,1,18,4,13,6,10,15,2,17,3,19,7,16,8,11,14,9,12,5
        };
        #endregion
        #region instance methods
        private byte[] GetScoreBoard()
        {
            var boardValue = new byte[20];

            boardValue[0] = 20;
            boardValue[1] = 1;
            boardValue[2] = 18;
            boardValue[3] = 4;
            boardValue[4] = 13;
            boardValue[5] = 6;
            boardValue[6] = 10;
            boardValue[7] = 15;
            boardValue[8] = 2;
            boardValue[9] = 17;
            boardValue[10] = 3;
            boardValue[11] = 19;
            boardValue[12] = 7;
            boardValue[13] = 16;
            boardValue[14] = 8;
            boardValue[15] = 11;
            boardValue[16] = 14;
            boardValue[17] = 9;
            boardValue[18] = 12;
            boardValue[19] = 5;

            return boardValue;
        }
        private Dictionary<int, ScoreDegrees> GetScoreDegrees()
        {
            var keyScoreDegrees = new Dictionary<int, ScoreDegrees>(22);
            keyScoreDegrees.Add(20, new ScoreDegrees
            {
                CounterParameter = 2,
                ParaDegree = new double[2, 2] { { -1, 9.5 },{ 351.5, 360} }
            });
            keyScoreDegrees.Add(1, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 9.5,27.5} }
            });
            keyScoreDegrees.Add(18, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 27.5, 45.5 } }
            });
            keyScoreDegrees.Add(4, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 45.5, 63.5 } }
            });
            keyScoreDegrees.Add(13, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 63.5, 81.5 } }
            });
            keyScoreDegrees.Add(6, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 81.5, 99.5 } }
            });
            keyScoreDegrees.Add(10, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 99.5, 117.5 } }
            });
            keyScoreDegrees.Add(15, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 117.5, 135.5 } }
            });
            keyScoreDegrees.Add(2, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 135.5, 153.5 } }
            });
            keyScoreDegrees.Add(17, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 153.5, 171.5 } }
            });
            keyScoreDegrees.Add(3, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 171.5, 189.5 } }
            });
            keyScoreDegrees.Add(19, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 189.5, 207.5 } }
            });
            keyScoreDegrees.Add(7, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 207.5, 225.5 } }
            });
            keyScoreDegrees.Add(16, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 225.5, 243.5 } }
            });
            keyScoreDegrees.Add(8, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 243.5, 261.5 } }
            });
            keyScoreDegrees.Add(11, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 261.5, 279.5 } }
            });
            keyScoreDegrees.Add(14, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 279.5, 297.5 } }
            });
            keyScoreDegrees.Add(9, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 297.5, 315.5 } }
            });
            keyScoreDegrees.Add(12, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 315.5, 333.5 } }
            });
            keyScoreDegrees.Add(5, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 333.5, 351.5 } }
            });
            keyScoreDegrees.Add(25, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 0, 359 } }
            });
            keyScoreDegrees.Add(50, new ScoreDegrees
            {
                CounterParameter = 1,
                ParaDegree = new double[1, 2] { { 0, 359 } }
            });
            return keyScoreDegrees;
        }
        private Dictionary<int, ScoreRadius> GetScoreRadius()
        {
            var keyScoreRadius = new Dictionary<int, ScoreRadius>(5);
            keyScoreRadius.Add((int)ValueMultiplicator.Single, new ScoreRadius
            {
                CounterParameter = 2,
                ParaRadius = new double[2,2] { { 20, 128 },{ 140, 209 } },
            });
            keyScoreRadius.Add((int)ValueMultiplicator.Double, new ScoreRadius
            {
                CounterParameter = 1,
                ParaRadius = new double[1,2] { { 209, 223 }},
            });
            keyScoreRadius.Add((int)ValueMultiplicator.Triple, new ScoreRadius
            {
                CounterParameter = 1,
                ParaRadius = new double[1, 2] { { 128, 140 } },
            });
            keyScoreRadius.Add((int)ValueMultiplicator.HalfBull, new ScoreRadius
            {
                CounterParameter = 1,
                ParaRadius = new double[1, 2] { { 8, 20 } },
            });
            keyScoreRadius.Add((int)ValueMultiplicator.Bull, new ScoreRadius
            {
                CounterParameter = 1,
                ParaRadius = new double[1, 2] { { 0, 8 } },
            });
            return keyScoreRadius;
        }
        #endregion
        #region Static methods
        /// <summary>
        /// Get double random number
        /// </summary>
        /// <param name="Min">minimum value</param>
        /// <param name="Max">maximum value</param>
        /// <returns>Double random number</returns>
        public static double RandomDouble(double Min, double Max)
        {
            if (Max < Min)
                return 0;
            return Min + Random.NextDouble() * (Max - Min);
        }
        /// <summary>
        /// Get int random number
        /// </summary>
        /// <param name="Min">minimum value</param>
        /// <param name="Max">maximum value</param>
        /// <returns>Integer random number</returns>
        public static int RandomInt(int Min, int Max)
        {
            if (Max <= Min)
                return 0;
            return Random.Next(Min, Max + 1);
        }
        #endregion
        #region methods
        /// <summary>
        /// Function to get offset for dartboardscreen.
        /// </summary>
        /// <param name="CoordinateX">Set the actual coordinate from x</param>
        /// <param name="CoordinateY">Set the actual coordinate from y</param>
        /// <param name="ScreenWidth">Set the actual screen width</param>
        /// <param name="ScreenHeight">Set the actual screen height</param>
        /// <returns>Get the coordinates from the middle</returns>
        public static ScreenCoordinates OffsetToZero(int CoordinateX, int CoordinateY, int ScreenWidth, int ScreenHeight)
        {
            ScreenCoordinates screenCoordinates = new ScreenCoordinates();
            screenCoordinates.X = CoordinateX - (int)ScreenWidth / 2;
            screenCoordinates.Y = (CoordinateY - (int)ScreenHeight / 2) * -1;
            return screenCoordinates;
        }
        /// <summary>
        /// Function to get origin coordinates for dartboardscreen.
        /// </summary>
        /// <param name="CoordinateX">Set the actual coordinate from x</param>
        /// <param name="CoordinateY">Set the actual coordinate from y</param>
        /// <param name="ScreenWidth">Set the actual screen width</param>
        /// <param name="ScreenHeight">Set the actual screen height</param>
        /// <returns>Get the origin coordinates</returns>
        public static ScreenCoordinates GetOrigin(int CoordinateX, int CoordinateY)
        {
            try
            {
                ScreenCoordinates screenCoordinates = new ScreenCoordinates();
                //if (CoordinateX < 0)
                //    screenCoordinates.X = (CoordinateX * -1) + BoardResolutionWidth / 2;
                //else
                    screenCoordinates.X = CoordinateX + BoardResolutionWidth / 2;
                //if (CoordinateY < 0)
                //    screenCoordinates.Y = (CoordinateY * -1) + BoardResolutionHeight / 2;
                //else
                    screenCoordinates.Y = CoordinateY + BoardResolutionHeight / 2;
                return screenCoordinates;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
                return new ScreenCoordinates() { X = -1, Y = -1 };
            }

        }
        /// <summary>
        /// Get the Score from coordinates 
        /// </summary>
        /// <param name="GetX">Set X coordinate</param>
        /// <param name="GetY">Set Y coordinate</param>
        /// <returns>Get Score</returns>
        public int GetScoreFromCoordinates(int GetX, int GetY)
        {
            Func<int, int, Quadrant> funcQuadrant = ((x,y) => 
            {
                if (x > 0 && y > 0)
                    return Quadrant.Q1;
                else if (x < 0 && y > 0)
                    return Quadrant.Q2;
                else if (x < 0 && y < 0)
                    return Quadrant.Q3;
                else if (x > 0 && y < 0)
                    return Quadrant.Q4;
                else if (x == 0 && y > 0)
                    return Quadrant.Q12;
                else if (x < 0 && y == 0)
                    return Quadrant.Q23;
                else if (x == 0 && y < 0)
                    return Quadrant.Q34;
                else if (x > 0 && y == 0)
                    return Quadrant.Q41;
                return Quadrant.Q0;
            });
            Func<double, double, Quadrant, double> funcDegree = ((x,y,z) => 
            {
                if (z == Quadrant.Q1)
                    return Math.Round((Math.Atan(x / y) * (180.0 / Math.PI)), 2);
                else if (z == Quadrant.Q2)
                    return Math.Round((Math.Atan(y / x) * -1 * (180.0 / Math.PI)), 2) + 270;
                else if (z == Quadrant.Q3)
                    return Math.Round((Math.Atan(x / y) * (180.0 / Math.PI)), 2) + 180;
                else if (z == Quadrant.Q4)
                    return Math.Round((Math.Atan(y / x) * -1 * (180.0 / Math.PI)), 2) + 90;
                else if (z == Quadrant.Q12)
                    return 0;
                else if (z == Quadrant.Q23)
                    return 270;
                else if (z == Quadrant.Q34)
                    return 180;
                else if (z == Quadrant.Q41)
                    return 90;
                else if (z == Quadrant.Q0)
                    return 0;

                return 999; 
            });
            Func<int, int, double> funcLength = ((x, y) => {return Math.Round(Math.Sqrt(Math.Pow(x,2)+Math.Pow(y,2)),2);});
            Func<double, ValueMultiplicator> funcRadiusScore = (x => 
            {
                int i;
                foreach (var item in scoreRadius)
                {
                    for (i = 0; i < item.Value.CounterParameter; i++)
                        if (x >= item.Value.ParaRadius[i, 0] && x < item.Value.ParaRadius[i, 1])//&& x < item.Value.ParaRadius[i,1])
                            return (DartBot.ValueMultiplicator)item.Key;
                }
                return 0;
            });
            Func<double, ValueSingle> funcDegreeScore = ((x) => 
            {
                int i;
                foreach (var item in scoreDegrees)
                {
                    for (i = 0; i < item.Value.CounterParameter; i++)
                        if (x > item.Value.ParaDegree[i, 0] && x <= item.Value.ParaDegree[i, 1])
                            return (DartBot.ValueSingle)item.Key;
                }
                return 0; 
            });
            Func<ValueMultiplicator, ValueSingle, int> funcActScore = ((x, y) => 
            {
                if ((int)x >= 1 && (int)x <= 3)
                    return (int)x * (int)y;
                else if ((int)x == 0)
                    return 0;
                else if ((int)x == 4)
                    return 25;
                else if ((int)x == 5)
                    return 50;
                return 0; 
            });
            
            ActualQuadrant = funcQuadrant(GetX, GetY);
            ActualDegree = funcDegree(GetX, GetY, ActualQuadrant);
            ActualLength = funcLength(GetX, GetY);
            ActualRadiusScore = funcRadiusScore(ActualLength);
            ActualDegreeScore = funcDegreeScore(ActualDegree);
            ActualScore = funcActScore(ActualRadiusScore, ActualDegreeScore);

            return ActualScore;
        }
        /// <summary>
        /// Get random coordinates from input value
        /// </summary>
        /// <param name="score">Input the score</param>
        /// <param name="valueMultiplicator">Input the value Multiplicator</param>
        /// <param name="valueSingle">Input the single, double, triple value</param>
        /// <returns>Random coordinates</returns>
        public DatahandlingWPF.Win32Point GetRandomCoordinatesFromScore(int score, ValueMultiplicator valueMultiplicator)
        {
#if DEBUG
            //Score 39
            double debug_scoreDegreeRandom = 0, debug_scoreRadiusRandom = 0;
#endif
            var win32Point = new DatahandlingWPF.Win32Point();
            try
            {
                Func<int, ScoreDegrees> funcScoreDegree = ((x) =>
                {
                    ScoreDegrees scoreDegree;
                    GetScoreDegrees().TryGetValue(x, out scoreDegree);

                    return scoreDegree;
                });
                Func<int, ScoreRadius> funcScoreRadius = ((x) =>
                {
                    ScoreRadius scoreRadius;
                    GetScoreRadius().TryGetValue(x, out scoreRadius);

                    return scoreRadius;
                });

                int scoreDegreePara = 0, scoreRadiusPara = 0;
                double scoreDegreeRandom = 0, scoreRadiusRandom = 0;

                if (score == 0)
                {
                    int degrees = RandomInt(0, 360);
                    int radius = RandomInt(224,300);
                    win32Point.X = (int)(Math.Sin((degrees * Math.PI) / 180) * radius);
                    win32Point.Y = (int)(Math.Cos((degrees * Math.PI) / 180) * radius);
                    return win32Point;
                }

                var scoreDegree = funcScoreDegree(score);
                var scoreRadius = funcScoreRadius((int)valueMultiplicator);

                if (scoreDegree.CounterParameter > 1)
                    scoreDegreePara = RandomInt(1, scoreDegree.CounterParameter) - 1;
                if (scoreRadius.CounterParameter > 1)
                    scoreRadiusPara = RandomInt(1, scoreRadius.CounterParameter) - 1;

                scoreDegreeRandom = RandomDouble(scoreDegree.ParaDegree[scoreDegreePara, 0],
                    scoreDegree.ParaDegree[scoreDegreePara, 1]);
                if (scoreDegreeRandom < 0)
                    scoreDegreeRandom = scoreDegreeRandom * -1;
            
                scoreRadiusRandom = RandomDouble(scoreRadius.ParaRadius[scoreRadiusPara, 0],
                    scoreRadius.ParaRadius[scoreRadiusPara, 1]);
#if DEBUG
                debug_scoreDegreeRandom = scoreDegreeRandom;
                debug_scoreRadiusRandom = scoreRadiusRandom;
#endif
                if (scoreDegreeRandom == 0 || scoreRadiusRandom == 0)
                    win32Point.X = 0;
                else
                    win32Point.X = (int)(Math.Sin((scoreDegreeRandom * Math.PI) / 180) * scoreRadiusRandom);

                if (scoreRadiusRandom == 0)
                    win32Point.Y = 0;
                else
                    win32Point.Y = (int)-(Math.Cos((scoreDegreeRandom * Math.PI) / 180) * scoreRadiusRandom);

                return win32Point;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error = {0}", e.Source);
                Console.WriteLine("Error = {0}", e.Message);
                Console.WriteLine("Error = {0}", e.StackTrace);
#if DEBUG
                Console.WriteLine("ToDebug debug_scoreDegreeRandom: {0}", debug_scoreDegreeRandom);
                Console.WriteLine("ToDebug debug_scoreRadiusRandom: {0}", debug_scoreRadiusRandom);
#endif
                Console.WriteLine("*********** Select confirm dartboard numbers ***********");

                win32Point.X = 0;
                win32Point.Y = 0;

                return win32Point;
            }
        }
        #endregion
        #region class library
        public struct ScoreDegrees
        {
            public byte CounterParameter;
            public double[,] ParaDegree;
        }
        public struct ScoreRadius
        {
            public byte CounterParameter;
            public double[,] ParaRadius;
        }
        public struct ScreenResolution
        {
            internal int X;
            internal int Y;
        }
        public struct ScreenCoordinates
        {
            internal int X;
            internal int Y;
        }
        #endregion
        #region enum
        public enum Quadrant
        {
            Q1=1,
            Q2,
            Q3,
            Q4,
            Q12,
            Q23,
            Q34,
            Q41,
            Q0
        }
        #endregion
        #region dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null

                _scoreDegrees = null;
                _scoreRadius = null;
                scoreDegrees = null;
                scoreRadius = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BoardParameter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    public class ScoringTable : IDisposable
    {
        #region constructor
        public ScoringTable() : this(0) {; }
        public ScoringTable(int score)
        {
            if (CheckFinishExistsIntern(score)) keyFinishTable = GetFinishTable();
            else keyScoringTable = GetScoringTable();
        }
        #endregion
        #region properties
        private Dictionary<int, Scoring> _keyFinishTable { get; set; }
        public Dictionary<int, Scoring> keyFinishTable
        {
            get => _keyFinishTable;
            set
            {
                if (value != null) _keyFinishTable = value;
            }
        }
        private Dictionary<int, Scoring> _keyScoringTable { get; set; }
        public Dictionary<int,Scoring> keyScoringTable { get => _keyScoringTable; set => _keyScoringTable = value; }
        #endregion
        #region instance methods
        public Dictionary<int, Scoring> GetFinishTable()
        {
            var keyFinishTable = new Dictionary<int, Scoring>(182);
            #region finish table
            keyFinishTable.Add(182, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(181, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 3,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 19,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(180, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(179, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 15,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(178, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 14,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(177, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 11,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(176, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 13,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(175, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 4,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 25,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(174, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 12,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(173, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 10,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(172, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 11,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(171, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 3,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 7,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(170, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 5,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 50,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(169, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 17,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(168, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 5,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 50,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(167, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 16,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(166, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 6,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(165, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 13,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(164, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 5,
                No_01_Score_01 = 20,
                No_01_Score_02 = 18,
                No_01_Score_03 = 50,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 4,
                No_02_Score_01 = 19,
                No_02_Score_02 = 19,
                No_02_Score_03 = 50,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(163, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 3,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(162, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 2,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(161, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 5,
                No_01_Score_01 = 20,
                No_01_Score_02 = 17,
                No_01_Score_03 = 50,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(160, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(159, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 3,
                No_01_Score_01 = 20,
                No_01_Score_02 = 15,
                No_01_Score_03 = 3,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(158, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 19,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(157, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(156, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 18,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(155, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 19,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(154, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 18,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(153, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 18,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(152, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(151, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 17,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 18,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(150, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 18,
                No_01_Score_03 = 18,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 19,
                No_02_Score_03 = 18,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(149, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(148, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 14,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 17,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(147, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 17,
                No_01_Score_03 = 18,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 18,
                No_02_Score_03 = 18,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(146, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 18,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 19,
                No_02_Score_03 = 16,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(145, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 14,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(144, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 12,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(143, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 17,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 18,
                No_02_Score_03 = 16,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(142, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 14,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 19,
                No_02_Score_03 = 14,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(141, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 12,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(140, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 10,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(139, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 13,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 19,
                No_02_Score_03 = 11,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(138, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 18,
                No_01_Score_03 = 12,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 19,
                No_02_Score_03 = 12,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(137, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 10,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(136, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 8,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(135, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 17,
                No_01_Score_03 = 12,
                No_02_Score_01_Mode = 4,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 5,
                No_02_Score_01 = 25,
                No_02_Score_02 = 20,
                No_02_Score_03 = 50,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(134, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 16,
                No_01_Score_03 = 13,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(133, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 8,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(132, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 16,
                No_01_Score_03 = 12,
                No_02_Score_01_Mode = 4,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 5,
                No_02_Score_01 = 25,
                No_02_Score_02 = 19,
                No_02_Score_03 = 50,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(131, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 14,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 13,
                No_02_Score_03 = 16,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(130, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 5,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(129, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 16,
                No_01_Score_03 = 12,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 19,
                No_02_Score_03 = 6,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(128, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 18,
                No_01_Score_02 = 14,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 18,
                No_02_Score_03 = 7,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(127, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 17,
                No_01_Score_03 = 8,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(126, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 19,
                No_01_Score_03 = 6,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(125, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 18,
                No_01_Score_02 = 19,
                No_01_Score_03 = 7,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 15,
                No_02_Score_03 = 10,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(124, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 14,
                No_01_Score_03 = 11,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(123, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 16,
                No_01_Score_03 = 9,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(122, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 18,
                No_01_Score_02 = 18,
                No_01_Score_03 = 7,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(121, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 11,
                No_01_Score_03 = 14,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 17,
                No_02_Score_02 = 20,
                No_02_Score_03 = 5,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(120, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(119, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 12,
                No_01_Score_03 = 13,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(118, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 18,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(117, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 20,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 17,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 0,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(116, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 19,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 16,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 0,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(115, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 15,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 18,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 19,
                No_02_Score_02 = 0,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(114, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 17,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 14,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 0,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(113, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 16,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(112, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 12,
                No_01_Score_03 = 8,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(111, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 14,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 11,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 20,
                No_02_Score_02 = 0,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(110, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 10,
                No_01_Score_03 = 10,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 5,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 20,
                No_03_Score_02 = 50,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(109, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 9,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(108, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 16,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 20,
                No_02_Score_03_Mode = 8,
                No_02_Score_01 = 3,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 0,
                No_03_Score_02 = 20,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(107, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 10,
                No_01_Score_03 = 10,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 5,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 19,
                No_03_Score_02 = 50,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(106, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 10,
                No_01_Score_03 = 8,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(105, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 13,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(104, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 15,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 5,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 18,
                No_03_Score_02 = 50,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(103, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 6,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 19,
                No_02_Score_03_Mode = 10,
                No_02_Score_01 = 3,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 0,
                No_03_Score_02 = 18,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(102, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 10,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 20,
                No_02_Score_03_Mode = 6,
                No_02_Score_01 = 3,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 0,
                No_03_Score_02 = 18,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(101, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 20,
                No_01_Score_02 = 9,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 2,
                No_02_Score_02_Mode = 1,
                No_02_Score_03_Mode = 5,
                No_02_Score_01 = 20,
                No_02_Score_02 = 1,
                No_02_Score_03 = 50,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(100, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(99, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 2,
                No_01_Score_01 = 19,
                No_01_Score_02 = 10,
                No_01_Score_03 = 16,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 19,
                No_02_Score_03_Mode = 6,
                No_02_Score_01 = 3,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 0,
                No_03_Score_02 = 18,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(98, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 19,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(97, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(96, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 18,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(95, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 19,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 4,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 25,
                No_02_Score_02 = 20,
                No_02_Score_03 = 5,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(94, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 18,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 4,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 25,
                No_02_Score_02 = 19,
                No_02_Score_03 = 6,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(93, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 18,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 4,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 25,
                No_02_Score_02 = 18,
                No_02_Score_03 = 7,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(92, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 4,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 25,
                No_02_Score_02 = 17,
                No_02_Score_03 = 8,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(91, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 17,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 4,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 25,
                No_02_Score_02 = 16,
                No_02_Score_03 = 9,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(90, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 15,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 18,
                No_03_Score_02 = 18,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(89, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(88, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 14,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(87, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 17,
                No_01_Score_02 = 18,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(86, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 18,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(85, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 15,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 19,
                No_02_Score_02 = 14,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(84, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 12,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(83, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 17,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(82, new Scoring
            {
                No_01_Score_01_Mode = 5,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 50,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 17,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 4,
                No_02_Score_02 = 0,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 14,
                No_03_Score_02 = 20,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(81, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 12,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 15,
                No_02_Score_02 = 18,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(80, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 10,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 2,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 20,
                No_02_Score_02 = 20,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(79, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 11,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 13,
                No_02_Score_02 = 20,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(78, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 18,
                No_01_Score_02 = 12,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(77, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 10,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(76, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 8,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 16,
                No_02_Score_02 = 14,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(75, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 17,
                No_01_Score_02 = 12,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(74, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 14,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(73, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 8,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(72, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 16,
                No_01_Score_02 = 12,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 20,
                No_02_Score_02 = 6,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(71, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 13,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(70, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 18,
                No_01_Score_02 = 8,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 20,
                No_03_Score_02 = 5,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(69, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 6,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 19,
                No_03_Score_02 = 6,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(68, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 16,
                No_01_Score_02 = 10,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 20,
                No_02_Score_02 = 4,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 18,
                No_03_Score_02 = 7,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(67, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 9,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 17,
                No_03_Score_02 = 8,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(66, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 10,
                No_01_Score_02 = 18,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 18,
                No_02_Score_02 = 6,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 16,
                No_03_Score_02 = 9,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(65, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 11,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 19,
                No_02_Score_02 = 4,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 15,
                No_03_Score_02 = 10,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(64, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 16,
                No_01_Score_02 = 8,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 14,
                No_03_Score_02 = 11,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(63, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 13,
                No_01_Score_02 = 12,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 17,
                No_02_Score_02 = 6,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 13,
                No_03_Score_02 = 12,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(62, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 10,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 12,
                No_03_Score_02 = 13,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(61, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 15,
                No_01_Score_02 = 8,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 7,
                No_02_Score_02 = 20,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 3,
                No_03_Score_02_Mode = 2,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 11,
                No_03_Score_02 = 14,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(60, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(59, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(58, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 18,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(57, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 17,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(56, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 16,
                No_01_Score_02 = 4,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(55, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 15,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(54, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 14,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(53, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 13,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(52, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 12,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 20,
                No_02_Score_02 = 16,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(51, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 11,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 19,
                No_02_Score_02 = 16,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(50, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 10,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 18,
                No_02_Score_02 = 16,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(49, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 9,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 17,
                No_02_Score_02 = 16,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(48, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 16,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 8,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 1,
                No_02_Score_02 = 0,
                No_02_Score_03 = 20,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(47, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 7,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 15,
                No_02_Score_02 = 16,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(46, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 6,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 10,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 1,
                No_02_Score_02 = 0,
                No_02_Score_03 = 18,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(45, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 13,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 19,
                No_02_Score_02 = 13,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(44, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 12,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 4,
                No_02_Score_02 = 20,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(43, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 3,
                No_01_Score_02 = 20,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 11,
                No_02_Score_02 = 16,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(42, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 10,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 6,
                No_02_Score_03_Mode = 2,
                No_02_Score_01 = 1,
                No_02_Score_02 = 0,
                No_02_Score_03 = 18,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 1,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(41, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 9,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(40, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 20,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 1,
                No_02_Score_02_Mode = 2,
                No_02_Score_03_Mode = 1,
                No_02_Score_01 = 20,
                No_02_Score_02 = 10,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(39, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 10,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(38, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 19,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(37, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 18,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(36, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 18,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(35, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 17,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(34, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 17,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(33, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 16,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(32, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 16,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(31, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 15,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(30, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 15,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(29, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 14,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(28, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 14,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(27, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 13,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(26, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 13,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(25, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 12,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(24, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 12,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(23, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 11,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(22, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 11,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(21, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 10,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(20, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 10,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(19, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 9,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(18, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 9,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(17, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 8,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(16, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 8,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(15, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 7,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(14, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 7,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(13, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 6,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(12, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 6,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(11, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 5,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(10, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 5,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(9, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 4,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(8, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 4,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(7, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 3,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(6, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 1,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 3,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(5, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 2,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(4, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 2,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(3, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 2,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 1,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(2, new Scoring
            {
                No_01_Score_01_Mode = 2,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            keyFinishTable.Add(1, new Scoring
            {
                No_01_Score_01_Mode = 1,
                No_01_Score_02_Mode = 0,
                No_01_Score_03_Mode = 0,
                No_01_Score_01 = 1,
                No_01_Score_02 = 0,
                No_01_Score_03 = 0,
                No_02_Score_01_Mode = 0,
                No_02_Score_02_Mode = 0,
                No_02_Score_03_Mode = 0,
                No_02_Score_01 = 0,
                No_02_Score_02 = 0,
                No_02_Score_03 = 0,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            #endregion
            return keyFinishTable;
        }
        public Dictionary<int, Scoring> GetScoringTable()
        {
            var keyScoringTable = new Dictionary<int, Scoring>(1);
            keyScoringTable.Add(1, new Scoring
            {
                No_01_Score_01_Mode = 3,
                No_01_Score_02_Mode = 3,
                No_01_Score_03_Mode = 3,
                No_01_Score_01 = 20,
                No_01_Score_02 = 20,
                No_01_Score_03 = 20,
                No_02_Score_01_Mode = 3,
                No_02_Score_02_Mode = 3,
                No_02_Score_03_Mode = 3,
                No_02_Score_01 = 19,
                No_02_Score_02 = 19,
                No_02_Score_03 = 19,
                No_03_Score_01_Mode = 0,
                No_03_Score_02_Mode = 0,
                No_03_Score_03_Mode = 0,
                No_03_Score_01 = 0,
                No_03_Score_02 = 0,
                No_03_Score_03 = 0
            });
            return keyScoringTable;
        }
        public Dictionary<int, ScoringToMultiplier> GetScoringToMultiplier()
        {
            var keyScoringToMultiplierTable = new Dictionary<int, ScoringToMultiplier>(61);
            keyScoringToMultiplierTable.Add(0, new ScoringToMultiplier
            {
                Score = 0,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(1, new ScoringToMultiplier
            {
                Score = 1,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(2, new ScoringToMultiplier
            {
                Score = 2,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(3, new ScoringToMultiplier
            {
                Score = 3,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(4, new ScoringToMultiplier
            {
                Score = 4,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(5, new ScoringToMultiplier
            {
                Score = 5,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(6, new ScoringToMultiplier
            {
                Score = 6,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(7, new ScoringToMultiplier
            {
                Score = 7,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(8, new ScoringToMultiplier
            {
                Score = 8,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(9, new ScoringToMultiplier
            {
                Score = 9,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(10, new ScoringToMultiplier
            {
                Score = 10,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(11, new ScoringToMultiplier
            {
                Score = 11,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(12, new ScoringToMultiplier
            {
                Score = 12,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(13, new ScoringToMultiplier
            {
                Score = 13,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(14, new ScoringToMultiplier
            {
                Score = 14,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(15, new ScoringToMultiplier
            {
                Score = 15,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(16, new ScoringToMultiplier
            {
                Score = 16,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(17, new ScoringToMultiplier
            {
                Score = 17,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(18, new ScoringToMultiplier
            {
                Score = 18,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(19, new ScoringToMultiplier
            {
                Score = 19,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(20, new ScoringToMultiplier
            {
                Score = 20,
                Multiplicator = ValueMultiplicator.Single
            });
            keyScoringToMultiplierTable.Add(21, new ScoringToMultiplier
            {
                Score = 7,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(22, new ScoringToMultiplier
            {
                Score = 11,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(23, new ScoringToMultiplier
            {
                Score = 23,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(24, new ScoringToMultiplier
            {
                Score = 12,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(25, new ScoringToMultiplier
            {
                Score = 25,
                Multiplicator = ValueMultiplicator.HalfBull
            });
            keyScoringToMultiplierTable.Add(26, new ScoringToMultiplier
            {
                Score = 13,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(27, new ScoringToMultiplier
            {
                Score = 9,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(28, new ScoringToMultiplier
            {
                Score = 14,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(29, new ScoringToMultiplier
            {
                Score = 29,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(30, new ScoringToMultiplier
            {
                Score = 10,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(31, new ScoringToMultiplier
            {
                Score = 31,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(32, new ScoringToMultiplier
            {
                Score = 16,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(33, new ScoringToMultiplier
            {
                Score = 11,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(34, new ScoringToMultiplier
            {
                Score = 17,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(35, new ScoringToMultiplier
            {
                Score = 35,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(36, new ScoringToMultiplier
            {
                Score = 12,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(37, new ScoringToMultiplier
            {
                Score = 37,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(38, new ScoringToMultiplier
            {
                Score = 19,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(39, new ScoringToMultiplier
            {
                Score = 39,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(40, new ScoringToMultiplier
            {
                Score = 20,
                Multiplicator = ValueMultiplicator.Double
            });
            keyScoringToMultiplierTable.Add(41, new ScoringToMultiplier
            {
                Score = 41,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(42, new ScoringToMultiplier
            {
                Score = 14,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(43, new ScoringToMultiplier
            {
                Score = 43,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(44, new ScoringToMultiplier
            {
                Score = 44,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(45, new ScoringToMultiplier
            {
                Score = 15,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(46, new ScoringToMultiplier
            {
                Score = 46,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(47, new ScoringToMultiplier
            {
                Score = 47,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(48, new ScoringToMultiplier
            {
                Score = 16,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(49, new ScoringToMultiplier
            {
                Score = 49,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(50, new ScoringToMultiplier
            {
                Score = 50,
                Multiplicator = ValueMultiplicator.Bull
            });
            keyScoringToMultiplierTable.Add(51, new ScoringToMultiplier
            {
                Score = 17,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(52, new ScoringToMultiplier
            {
                Score = 52,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(53, new ScoringToMultiplier
            {
                Score = 53,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(54, new ScoringToMultiplier
            {
                Score = 18,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(55, new ScoringToMultiplier
            {
                Score = 55,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(56, new ScoringToMultiplier
            {
                Score = 56,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(57, new ScoringToMultiplier
            {
                Score = 19,
                Multiplicator = ValueMultiplicator.Triple
            });
            keyScoringToMultiplierTable.Add(58, new ScoringToMultiplier
            {
                Score = 58,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(59, new ScoringToMultiplier
            {
                Score = 59,
                Multiplicator = ValueMultiplicator.Nothing
            });
            keyScoringToMultiplierTable.Add(60, new ScoringToMultiplier
            {
                Score = 20,
                Multiplicator = ValueMultiplicator.Triple
            });

            return keyScoringToMultiplierTable;
        }
        private protected bool CheckFinishExistsIntern(int score) 
        {
            if (GetFinishTable().ContainsKey(score)) return true;
            else return false;
        }
        public static bool CheckFinishExists(int value)
        {
            bool check = new bool();
            ScoringTable scoringTable = new ScoringTable(value);
            
            if (scoringTable.GetFinishTable().ContainsKey(value))
                check = true;
            else
                check = false;
            scoringTable.Dispose(true);
            return check;
        }
        #endregion
        #region class library
        public struct Scoring
        {
            public byte No_01_Score_01_Mode;
            public byte No_01_Score_02_Mode;
            public byte No_01_Score_03_Mode;
            public byte No_01_Score_01;
            public byte No_01_Score_02;
            public byte No_01_Score_03;
            public byte No_02_Score_01_Mode;
            public byte No_02_Score_02_Mode;
            public byte No_02_Score_03_Mode;
            public byte No_02_Score_01;
            public byte No_02_Score_02;
            public byte No_02_Score_03;
            public byte No_03_Score_01_Mode;
            public byte No_03_Score_02_Mode;
            public byte No_03_Score_03_Mode;
            public byte No_03_Score_01;
            public byte No_03_Score_02;
            public byte No_03_Score_03;
        }
        public struct ScoringToMultiplier
        {
            public byte Score;
            public DartBot.ValueMultiplicator Multiplicator;
        }
        public class FinishSingle
        {
            public byte Score_01_Mode;
            public byte Score_02_Mode;
            public byte Score_03_Mode;
            public byte Score_01;
            public byte Score_02;
            public byte Score_03;
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
                _keyFinishTable = null;
                keyFinishTable = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~ScoringTable()
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
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    public class Probability
    {
        #region constructor
        public Probability() { }
        public Probability(int Difficulty)
        {
            this.Difficulty = Difficulty;
        }
        #endregion
        #region properties, fields
        private double[,] _ShotProbability { get; set; }
        public double[,] ShotProbability { get { return _ShotProbability; } set { _ShotProbability = value; } }

        private int _Difficulty{ get; set; }
        public int Difficulty { get => _Difficulty; private set => _Difficulty = value; }

        #endregion
        #region methods
        /// <summary>
        /// Calculates the score from the input score
        /// </summary>
        /// <param name="score">Set the score</param>
        /// <param name="mode">Set the mode</param>
        /// <returns>[0] = score / [1] = score probability / [2] = mode probability</returns>
        public double[,] Sequence(int score, int mode)
        {
            //int stepSequence = 1;
            //Probability.Matrix probability = new Probability.Matrix();
            //// Initialize
            //if (stepSequence == 1)
            //{
            //    stepSequence = 10;
            //}

            //// Get proberbility board value 
            //if (stepSequence == 10)
            //{
            //    probability = GetProbabilityMatrix(score);
            //    stepSequence += 10;
            //}

            //// Get proberbility 
            //if (stepSequence == 20)
            //{
            //    ShotProbability = probability;
            //}

            return ShotProbability;
        }

        /// <summary>
        /// Get the probability matrix for throws, easy to hard, best probability in position 10. Max i 21
        /// </summary>
        /// <param name="difficulty">Set the difficulty</param>
        /// <returns>[0,X] sets the points / [1,X] sets the probabilitys</returns>
        public Matrix GetProbabilityMatrix(int score, ValueMultiplicator valMult)
        {
            Table table = new Table();
            double[,] probabilityMatrix = new double[3, 21];
            Matrix probabilityMatrixTransform = new Matrix();

            // 20,1,18,4,13,6,10,15,2,17,03,19,7,16,8,11,14,09,12,5
            if (score == 20)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 19;
                probabilityMatrix[0, 2] = 7;
                probabilityMatrix[0, 3] = 16;
                probabilityMatrix[0, 4] = 8;
                probabilityMatrix[0, 5] = 11;
                probabilityMatrix[0, 6] = 14;
                probabilityMatrix[0, 7] = 9;
                probabilityMatrix[0, 8] = 12;
                probabilityMatrix[0, 9] = 5;
                probabilityMatrix[0, 10] = 20;
                probabilityMatrix[0, 11] = 1;
                probabilityMatrix[0, 12] = 18;
                probabilityMatrix[0, 13] = 4;
                probabilityMatrix[0, 14] = 13;
                probabilityMatrix[0, 15] = 6;
                probabilityMatrix[0, 16] = 10;
                probabilityMatrix[0, 17] = 15;
                probabilityMatrix[0, 18] = 2;
                probabilityMatrix[0, 19] = 17;
                probabilityMatrix[0, 20] = 3;
            }
            else if (score == 1)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 7;
                probabilityMatrix[0, 2] = 16;
                probabilityMatrix[0, 3] = 8;
                probabilityMatrix[0, 4] = 11;
                probabilityMatrix[0, 5] = 14;
                probabilityMatrix[0, 6] = 9;
                probabilityMatrix[0, 7] = 12;
                probabilityMatrix[0, 8] = 5;
                probabilityMatrix[0, 9] = 20;
                probabilityMatrix[0, 10] = 1;
                probabilityMatrix[0, 11] = 18;
                probabilityMatrix[0, 12] = 4;
                probabilityMatrix[0, 13] = 13;
                probabilityMatrix[0, 14] = 6;
                probabilityMatrix[0, 15] = 10;
                probabilityMatrix[0, 16] = 15;
                probabilityMatrix[0, 17] = 2;
                probabilityMatrix[0, 18] = 17;
                probabilityMatrix[0, 19] = 3;
                probabilityMatrix[0, 20] = 19;
            }
            else if (score == 18)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 16;
                probabilityMatrix[0, 2] = 8;
                probabilityMatrix[0, 3] = 11;
                probabilityMatrix[0, 4] = 14;
                probabilityMatrix[0, 5] = 9;
                probabilityMatrix[0, 6] = 12;
                probabilityMatrix[0, 7] = 5;
                probabilityMatrix[0, 8] = 20;
                probabilityMatrix[0, 9] = 1;
                probabilityMatrix[0, 10] = 18;
                probabilityMatrix[0, 11] = 4;
                probabilityMatrix[0, 12] = 13;
                probabilityMatrix[0, 13] = 6;
                probabilityMatrix[0, 14] = 10;
                probabilityMatrix[0, 15] = 15;
                probabilityMatrix[0, 16] = 2;
                probabilityMatrix[0, 17] = 17;
                probabilityMatrix[0, 18] = 3;
                probabilityMatrix[0, 19] = 19;
                probabilityMatrix[0, 20] = 7;
            }
            else if (score == 4)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 8;
                probabilityMatrix[0, 2] = 11;
                probabilityMatrix[0, 3] = 14;
                probabilityMatrix[0, 4] = 9;
                probabilityMatrix[0, 5] = 12;
                probabilityMatrix[0, 6] = 5;
                probabilityMatrix[0, 7] = 20;
                probabilityMatrix[0, 8] = 1;
                probabilityMatrix[0, 9] = 18;
                probabilityMatrix[0, 10] = 4;
                probabilityMatrix[0, 11] = 13;
                probabilityMatrix[0, 12] = 6;
                probabilityMatrix[0, 13] = 10;
                probabilityMatrix[0, 14] = 15;
                probabilityMatrix[0, 15] = 2;
                probabilityMatrix[0, 16] = 17;
                probabilityMatrix[0, 17] = 3;
                probabilityMatrix[0, 18] = 19;
                probabilityMatrix[0, 19] = 7;
                probabilityMatrix[0, 20] = 16;
            }
            else if (score == 13)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 11;
                probabilityMatrix[0, 2] = 14;
                probabilityMatrix[0, 3] = 9;
                probabilityMatrix[0, 4] = 12;
                probabilityMatrix[0, 5] = 5;
                probabilityMatrix[0, 6] = 20;
                probabilityMatrix[0, 7] = 1;
                probabilityMatrix[0, 8] = 18;
                probabilityMatrix[0, 9] = 4;
                probabilityMatrix[0, 10] = 13;
                probabilityMatrix[0, 11] = 6;
                probabilityMatrix[0, 12] = 10;
                probabilityMatrix[0, 13] = 15;
                probabilityMatrix[0, 14] = 2;
                probabilityMatrix[0, 15] = 17;
                probabilityMatrix[0, 16] = 3;
                probabilityMatrix[0, 17] = 19;
                probabilityMatrix[0, 18] = 7;
                probabilityMatrix[0, 19] = 16;
                probabilityMatrix[0, 20] = 8;
            }
            else if (score == 6)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 14;
                probabilityMatrix[0, 2] = 9;
                probabilityMatrix[0, 3] = 12;
                probabilityMatrix[0, 4] = 5;
                probabilityMatrix[0, 5] = 20;
                probabilityMatrix[0, 6] = 1;
                probabilityMatrix[0, 7] = 18;
                probabilityMatrix[0, 8] = 4;
                probabilityMatrix[0, 9] = 13;
                probabilityMatrix[0, 10] = 6;
                probabilityMatrix[0, 11] = 10;
                probabilityMatrix[0, 12] = 15;
                probabilityMatrix[0, 13] = 2;
                probabilityMatrix[0, 14] = 17;
                probabilityMatrix[0, 15] = 3;
                probabilityMatrix[0, 16] = 19;
                probabilityMatrix[0, 17] = 7;
                probabilityMatrix[0, 18] = 16;
                probabilityMatrix[0, 19] = 8;
                probabilityMatrix[0, 20] = 11;
            }
            else if (score == 10)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 9;
                probabilityMatrix[0, 2] = 12;
                probabilityMatrix[0, 3] = 5;
                probabilityMatrix[0, 4] = 20;
                probabilityMatrix[0, 5] = 1;
                probabilityMatrix[0, 6] = 18;
                probabilityMatrix[0, 7] = 4;
                probabilityMatrix[0, 8] = 13;
                probabilityMatrix[0, 9] = 6;
                probabilityMatrix[0, 10] = 10;
                probabilityMatrix[0, 11] = 15;
                probabilityMatrix[0, 12] = 2;
                probabilityMatrix[0, 13] = 17;
                probabilityMatrix[0, 14] = 3;
                probabilityMatrix[0, 15] = 19;
                probabilityMatrix[0, 16] = 7;
                probabilityMatrix[0, 17] = 16;
                probabilityMatrix[0, 18] = 8;
                probabilityMatrix[0, 19] = 11;
                probabilityMatrix[0, 20] = 14;
            }
            else if (score == 15)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 12;
                probabilityMatrix[0, 2] = 5;
                probabilityMatrix[0, 3] = 20;
                probabilityMatrix[0, 4] = 1;
                probabilityMatrix[0, 5] = 18;
                probabilityMatrix[0, 6] = 4;
                probabilityMatrix[0, 7] = 13;
                probabilityMatrix[0, 8] = 6;
                probabilityMatrix[0, 9] = 10;
                probabilityMatrix[0, 10] = 15;
                probabilityMatrix[0, 11] = 2;
                probabilityMatrix[0, 12] = 17;
                probabilityMatrix[0, 13] = 3;
                probabilityMatrix[0, 14] = 19;
                probabilityMatrix[0, 15] = 7;
                probabilityMatrix[0, 16] = 16;
                probabilityMatrix[0, 17] = 8;
                probabilityMatrix[0, 18] = 11;
                probabilityMatrix[0, 19] = 14;
                probabilityMatrix[0, 20] = 9;
            }
            else if (score == 2)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 5;
                probabilityMatrix[0, 2] = 20;
                probabilityMatrix[0, 3] = 1;
                probabilityMatrix[0, 4] = 18;
                probabilityMatrix[0, 5] = 4;
                probabilityMatrix[0, 6] = 13;
                probabilityMatrix[0, 7] = 6;
                probabilityMatrix[0, 8] = 10;
                probabilityMatrix[0, 9] = 15;
                probabilityMatrix[0, 10] = 2;
                probabilityMatrix[0, 11] = 17;
                probabilityMatrix[0, 12] = 3;
                probabilityMatrix[0, 13] = 19;
                probabilityMatrix[0, 14] = 7;
                probabilityMatrix[0, 15] = 16;
                probabilityMatrix[0, 16] = 8;
                probabilityMatrix[0, 17] = 11;
                probabilityMatrix[0, 18] = 14;
                probabilityMatrix[0, 19] = 9;
                probabilityMatrix[0, 20] = 12;
            }
            else if (score == 17)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 20;
                probabilityMatrix[0, 2] = 1;
                probabilityMatrix[0, 3] = 18;
                probabilityMatrix[0, 4] = 4;
                probabilityMatrix[0, 5] = 13;
                probabilityMatrix[0, 6] = 6;
                probabilityMatrix[0, 7] = 10;
                probabilityMatrix[0, 8] = 15;
                probabilityMatrix[0, 9] = 2;
                probabilityMatrix[0, 10] = 17;
                probabilityMatrix[0, 11] = 3;
                probabilityMatrix[0, 12] = 19;
                probabilityMatrix[0, 13] = 7;
                probabilityMatrix[0, 14] = 16;
                probabilityMatrix[0, 15] = 8;
                probabilityMatrix[0, 16] = 11;
                probabilityMatrix[0, 17] = 14;
                probabilityMatrix[0, 18] = 9;
                probabilityMatrix[0, 19] = 12;
                probabilityMatrix[0, 20] = 5;
            }
            else if (score == 3)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 1;
                probabilityMatrix[0, 2] = 18;
                probabilityMatrix[0, 3] = 4;
                probabilityMatrix[0, 4] = 13;
                probabilityMatrix[0, 5] = 6;
                probabilityMatrix[0, 6] = 10;
                probabilityMatrix[0, 7] = 15;
                probabilityMatrix[0, 8] = 2;
                probabilityMatrix[0, 9] = 17;
                probabilityMatrix[0, 10] = 3;
                probabilityMatrix[0, 11] = 19;
                probabilityMatrix[0, 12] = 7;
                probabilityMatrix[0, 13] = 16;
                probabilityMatrix[0, 14] = 8;
                probabilityMatrix[0, 15] = 11;
                probabilityMatrix[0, 16] = 14;
                probabilityMatrix[0, 17] = 9;
                probabilityMatrix[0, 18] = 12;
                probabilityMatrix[0, 19] = 5;
                probabilityMatrix[0, 20] = 20;
            }
            else if (score == 19)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 18;
                probabilityMatrix[0, 2] = 4;
                probabilityMatrix[0, 3] = 13;
                probabilityMatrix[0, 4] = 6;
                probabilityMatrix[0, 5] = 10;
                probabilityMatrix[0, 6] = 15;
                probabilityMatrix[0, 7] = 2;
                probabilityMatrix[0, 8] = 17;
                probabilityMatrix[0, 9] = 3;
                probabilityMatrix[0, 10] = 19;
                probabilityMatrix[0, 11] = 7;
                probabilityMatrix[0, 12] = 16;
                probabilityMatrix[0, 13] = 8;
                probabilityMatrix[0, 14] = 11;
                probabilityMatrix[0, 15] = 14;
                probabilityMatrix[0, 16] = 9;
                probabilityMatrix[0, 17] = 12;
                probabilityMatrix[0, 18] = 5;
                probabilityMatrix[0, 19] = 20;
                probabilityMatrix[0, 20] = 1;
            }
            else if (score == 7)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 4;
                probabilityMatrix[0, 2] = 13;
                probabilityMatrix[0, 3] = 6;
                probabilityMatrix[0, 4] = 10;
                probabilityMatrix[0, 5] = 15;
                probabilityMatrix[0, 6] = 2;
                probabilityMatrix[0, 7] = 17;
                probabilityMatrix[0, 8] = 3;
                probabilityMatrix[0, 9] = 19;
                probabilityMatrix[0, 10] = 7;
                probabilityMatrix[0, 11] = 16;
                probabilityMatrix[0, 12] = 8;
                probabilityMatrix[0, 13] = 11;
                probabilityMatrix[0, 14] = 14;
                probabilityMatrix[0, 15] = 9;
                probabilityMatrix[0, 16] = 12;
                probabilityMatrix[0, 17] = 5;
                probabilityMatrix[0, 18] = 20;
                probabilityMatrix[0, 19] = 1;
                probabilityMatrix[0, 20] = 18;
            }
            else if (score == 16)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 13;
                probabilityMatrix[0, 2] = 6;
                probabilityMatrix[0, 3] = 10;
                probabilityMatrix[0, 4] = 15;
                probabilityMatrix[0, 5] = 2;
                probabilityMatrix[0, 6] = 17;
                probabilityMatrix[0, 7] = 3;
                probabilityMatrix[0, 8] = 19;
                probabilityMatrix[0, 9] = 7;
                probabilityMatrix[0, 10] = 16;
                probabilityMatrix[0, 11] = 8;
                probabilityMatrix[0, 12] = 11;
                probabilityMatrix[0, 13] = 14;
                probabilityMatrix[0, 14] = 9;
                probabilityMatrix[0, 15] = 12;
                probabilityMatrix[0, 16] = 5;
                probabilityMatrix[0, 17] = 20;
                probabilityMatrix[0, 18] = 1;
                probabilityMatrix[0, 19] = 18;
                probabilityMatrix[0, 20] = 4;
            }
            else if (score == 8)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 6;
                probabilityMatrix[0, 2] = 10;
                probabilityMatrix[0, 3] = 15;
                probabilityMatrix[0, 4] = 2;
                probabilityMatrix[0, 5] = 17;
                probabilityMatrix[0, 6] = 3;
                probabilityMatrix[0, 7] = 19;
                probabilityMatrix[0, 8] = 7;
                probabilityMatrix[0, 9] = 16;
                probabilityMatrix[0, 10] = 8;
                probabilityMatrix[0, 11] = 11;
                probabilityMatrix[0, 12] = 14;
                probabilityMatrix[0, 13] = 9;
                probabilityMatrix[0, 14] = 12;
                probabilityMatrix[0, 15] = 5;
                probabilityMatrix[0, 16] = 20;
                probabilityMatrix[0, 17] = 1;
                probabilityMatrix[0, 18] = 18;
                probabilityMatrix[0, 19] = 4;
                probabilityMatrix[0, 20] = 13;
            }
            else if (score == 11)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 10;
                probabilityMatrix[0, 2] = 15;
                probabilityMatrix[0, 3] = 2;
                probabilityMatrix[0, 4] = 17;
                probabilityMatrix[0, 5] = 3;
                probabilityMatrix[0, 6] = 19;
                probabilityMatrix[0, 7] = 7;
                probabilityMatrix[0, 8] = 16;
                probabilityMatrix[0, 9] = 8;
                probabilityMatrix[0, 10] = 11;
                probabilityMatrix[0, 11] = 14;
                probabilityMatrix[0, 12] = 9;
                probabilityMatrix[0, 13] = 12;
                probabilityMatrix[0, 14] = 5;
                probabilityMatrix[0, 15] = 20;
                probabilityMatrix[0, 16] = 1;
                probabilityMatrix[0, 17] = 18;
                probabilityMatrix[0, 18] = 4;
                probabilityMatrix[0, 19] = 13;
                probabilityMatrix[0, 20] = 6;
            }
            else if (score == 14)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 15;
                probabilityMatrix[0, 2] = 2;
                probabilityMatrix[0, 3] = 17;
                probabilityMatrix[0, 4] = 3;
                probabilityMatrix[0, 5] = 19;
                probabilityMatrix[0, 6] = 7;
                probabilityMatrix[0, 7] = 16;
                probabilityMatrix[0, 8] = 8;
                probabilityMatrix[0, 9] = 11;
                probabilityMatrix[0, 10] = 14;
                probabilityMatrix[0, 11] = 9;
                probabilityMatrix[0, 12] = 12;
                probabilityMatrix[0, 13] = 5;
                probabilityMatrix[0, 14] = 20;
                probabilityMatrix[0, 15] = 1;
                probabilityMatrix[0, 16] = 18;
                probabilityMatrix[0, 17] = 4;
                probabilityMatrix[0, 18] = 13;
                probabilityMatrix[0, 19] = 6;
                probabilityMatrix[0, 20] = 10;
            }
            else if (score == 9)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 2;
                probabilityMatrix[0, 2] = 17;
                probabilityMatrix[0, 3] = 3;
                probabilityMatrix[0, 4] = 19;
                probabilityMatrix[0, 5] = 7;
                probabilityMatrix[0, 6] = 16;
                probabilityMatrix[0, 7] = 8;
                probabilityMatrix[0, 8] = 11;
                probabilityMatrix[0, 9] = 14;
                probabilityMatrix[0, 10] = 9;
                probabilityMatrix[0, 11] = 12;
                probabilityMatrix[0, 12] = 5;
                probabilityMatrix[0, 13] = 20;
                probabilityMatrix[0, 14] = 1;
                probabilityMatrix[0, 15] = 18;
                probabilityMatrix[0, 16] = 4;
                probabilityMatrix[0, 17] = 13;
                probabilityMatrix[0, 18] = 6;
                probabilityMatrix[0, 19] = 10;
                probabilityMatrix[0, 20] = 15;
            }
            else if (score == 12)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 17;
                probabilityMatrix[0, 2] = 3;
                probabilityMatrix[0, 3] = 19;
                probabilityMatrix[0, 4] = 7;
                probabilityMatrix[0, 5] = 16;
                probabilityMatrix[0, 6] = 8;
                probabilityMatrix[0, 7] = 11;
                probabilityMatrix[0, 8] = 14;
                probabilityMatrix[0, 9] = 9;
                probabilityMatrix[0, 10] = 12;
                probabilityMatrix[0, 11] = 5;
                probabilityMatrix[0, 12] = 20;
                probabilityMatrix[0, 13] = 1;
                probabilityMatrix[0, 14] = 18;
                probabilityMatrix[0, 15] = 4;
                probabilityMatrix[0, 16] = 13;
                probabilityMatrix[0, 17] = 6;
                probabilityMatrix[0, 18] = 10;
                probabilityMatrix[0, 19] = 15;
                probabilityMatrix[0, 20] = 2;
            }
            else if (score == 5)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 3;
                probabilityMatrix[0, 2] = 19;
                probabilityMatrix[0, 3] = 7;
                probabilityMatrix[0, 4] = 16;
                probabilityMatrix[0, 5] = 8;
                probabilityMatrix[0, 6] = 11;
                probabilityMatrix[0, 7] = 14;
                probabilityMatrix[0, 8] = 9;
                probabilityMatrix[0, 9] = 12;
                probabilityMatrix[0, 10] = 5;
                probabilityMatrix[0, 11] = 20;
                probabilityMatrix[0, 12] = 1;
                probabilityMatrix[0, 13] = 18;
                probabilityMatrix[0, 14] = 4;
                probabilityMatrix[0, 15] = 13;
                probabilityMatrix[0, 16] = 6;
                probabilityMatrix[0, 17] = 10;
                probabilityMatrix[0, 18] = 15;
                probabilityMatrix[0, 19] = 2;
                probabilityMatrix[0, 20] = 17;
            }
            else if (score == 0)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 0;
                probabilityMatrix[0, 2] = 0;
                probabilityMatrix[0, 3] = 0;
                probabilityMatrix[0, 4] = 0;
                probabilityMatrix[0, 5] = 0;
                probabilityMatrix[0, 6] = 0;
                probabilityMatrix[0, 7] = 0;
                probabilityMatrix[0, 8] = 0;
                probabilityMatrix[0, 9] = 0;
                probabilityMatrix[0, 10] = 0;
                probabilityMatrix[0, 11] = 0;
                probabilityMatrix[0, 12] = 0;
                probabilityMatrix[0, 13] = 0;
                probabilityMatrix[0, 14] = 0;
                probabilityMatrix[0, 15] = 0;
                probabilityMatrix[0, 16] = 0;
                probabilityMatrix[0, 17] = 0;
                probabilityMatrix[0, 18] = 0;
                probabilityMatrix[0, 19] = 0;
                probabilityMatrix[0, 20] = 0;
            }
            else if (score == 50)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 0;
                probabilityMatrix[0, 2] = 0;
                probabilityMatrix[0, 3] = 0;
                probabilityMatrix[0, 4] = 0;
                probabilityMatrix[0, 5] = 0;
                probabilityMatrix[0, 6] = 0;
                probabilityMatrix[0, 7] = 0;
                probabilityMatrix[0, 8] = 0;
                probabilityMatrix[0, 9] = 0;
                probabilityMatrix[0, 10] = 50;
                probabilityMatrix[0, 11] = 0;
                probabilityMatrix[0, 12] = 0;
                probabilityMatrix[0, 13] = 0;
                probabilityMatrix[0, 14] = 0;
                probabilityMatrix[0, 15] = 0;
                probabilityMatrix[0, 16] = 0;
                probabilityMatrix[0, 17] = 0;
                probabilityMatrix[0, 18] = 0;
                probabilityMatrix[0, 19] = 0;
                probabilityMatrix[0, 20] = 0;
            }
            else if (score == 25)
            {
                probabilityMatrix[0, 0] = 0;
                probabilityMatrix[0, 1] = 0;
                probabilityMatrix[0, 2] = 0;
                probabilityMatrix[0, 3] = 0;
                probabilityMatrix[0, 4] = 0;
                probabilityMatrix[0, 5] = 0;
                probabilityMatrix[0, 6] = 0;
                probabilityMatrix[0, 7] = 0;
                probabilityMatrix[0, 8] = 0;
                probabilityMatrix[0, 9] = 0;
                probabilityMatrix[0, 10] = 25;
                probabilityMatrix[0, 11] = 0;
                probabilityMatrix[0, 12] = 0;
                probabilityMatrix[0, 13] = 0;
                probabilityMatrix[0, 14] = 0;
                probabilityMatrix[0, 15] = 0;
                probabilityMatrix[0, 16] = 0;
                probabilityMatrix[0, 17] = 0;
                probabilityMatrix[0, 18] = 0;
                probabilityMatrix[0, 19] = 0;
                probabilityMatrix[0, 20] = 0;
            }
            // If Multiplier == 4 || Multiplier == 5 and Score == 0
            else if (score == -1)
            {
                probabilityMatrix[0, 0] = 1;
                probabilityMatrix[0, 1] = 2;
                probabilityMatrix[0, 2] = 3;
                probabilityMatrix[0, 3] = 4;
                probabilityMatrix[0, 4] = 5;
                probabilityMatrix[0, 5] = 6;
                probabilityMatrix[0, 6] = 7;
                probabilityMatrix[0, 7] = 8;
                probabilityMatrix[0, 8] = 9;
                probabilityMatrix[0, 9] = 10;
                probabilityMatrix[0, 10] = 11;
                probabilityMatrix[0, 11] = 12;
                probabilityMatrix[0, 12] = 13;
                probabilityMatrix[0, 13] = 14;
                probabilityMatrix[0, 14] = 15;
                probabilityMatrix[0, 15] = 16;
                probabilityMatrix[0, 16] = 17;
                probabilityMatrix[0, 17] = 18;
                probabilityMatrix[0, 18] = 19;
                probabilityMatrix[0, 19] = 20;
                probabilityMatrix[0, 20] = 0;

                for (int i = 0; i < 21; i++)
                    probabilityMatrix[1, i] = 4.7619047619047619047619047619048;

                return TransformProbMatrix(probabilityMatrix);
            }

            switch (Difficulty)
            {
                case 1: //  3 + 2,4195 * e ^ (-(pi / (100 * pi)) * (x - 10) ^ 2)  // Easy
                    {
                        for (int i = 0; i < 21; i++)
                            probabilityMatrix[1, i] = Math.Round(3 + 2.4195 * Math.Exp(-Math.PI / (100 * Math.PI) * Math.Pow((i - 10), 2)), 2);
                    }
                    break;
                case 2: //  2 + 11,0199 * e ^ (-(pi / (8, 8176 * pi)) * (x - 10) ^ 2)      // Middle
                    {
                        for (int i = 0; i < 21; i++)
                            probabilityMatrix[1, i] = Math.Round(2 + 11.0199 * Math.Exp(-Math.PI / (8.8176 * Math.PI) * Math.Pow((i - 10), 2)), 2);
                    }
                    break;
                case 3: //  0.6229 + 80 * e ^ (-(Pi) * (x - 10) ^ 2)                   // Hard
                    {
                        for (int i = 0; i < 21; i++)
                            probabilityMatrix[1, i] = Math.Round(0.6229 + 80 * Math.Exp(-Math.PI * Math.Pow((i - 10), 2)), 2);
                    }
                    break;
                default:
                    break;
            }
            switch ((int)valMult)
            {
                // nothing
                case 0:
                    {
                        switch (Difficulty)
                        {
                            case 1:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 100;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 0;
                                }
                                break;
                            case 2:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 100;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 0;
                                }
                                break;
                            case 3:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 100;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 0;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 0;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // single
                case 1:
                    {
                        switch (Difficulty)
                        {
                            case 1:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 22;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 30;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 12;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 12;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 12;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 12;
                                }
                                break;
                            case 2:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 60;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 15;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 10;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 5;
                                }
                                break;
                            case 3:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 2;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 86;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 4;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 4;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 2;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 2;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // double
                case 2:
                    {
                        switch (Difficulty)
                        {
                            case 1:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 22;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 12;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 30;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 12;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 12;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 12;
                                }
                                break;
                            case 2:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 15;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 40;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 15;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 5;
                                }
                                break;
                            case 3:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 50;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 6;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 2;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 2;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // triple
                case 3:
                    {
                        switch (Difficulty)
                        {
                            case 1:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 30;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 15;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 15;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 15;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 15;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 10;
                                }
                                break;
                            case 2:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 10;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 30;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 10;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 40;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 5;
                                }
                                break;
                            case 3:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 4;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 4;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 4;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 80;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 4;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 4;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // half bull
                case 4:
                    {
                        switch (Difficulty)
                        {
                            case 1:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 30;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 5;
                                }
                                break;
                            case 2:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 25;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 10;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 10;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 25;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 20;
                                }
                                break;
                            case 3:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 2;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 25;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 2;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 6;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 35;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 30;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                // bull
                case 5:
                    {
                        switch (Difficulty)
                        {
                            case 1:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 30;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 5;
                                }
                                break;
                            case 2:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 5;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 25;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 10;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 10;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 20;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 25;
                                }
                                break;
                            case 3:
                                {
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Nothing] = 2;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Single] = 25;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Double] = 2;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Triple] = 6;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.HalfBull] = 30;
                                    probabilityMatrix[2, (int)DartBot.ValueMultiplicator.Bull] = 35;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }

            return TransformProbMatrix(probabilityMatrix);
        }
        private Matrix TransformProbMatrix(double[,] probabilityMatrixDouble)
        {
            Matrix probabilityMatrix = new Matrix();
            int CountUBound = probabilityMatrixDouble.GetLength(1);

            for (int i = 0; i < CountUBound; i++)
            {
                probabilityMatrix.Value[i] = (int)probabilityMatrixDouble[0, i];
                probabilityMatrix.ScoreValue[i] = probabilityMatrixDouble[1, i];
                if (i <= 5)
                    probabilityMatrix.MultiplicationValue[i] = probabilityMatrixDouble[2, i];
            }

            //ProbabilityMatrix = probabilityMatrix;
            return probabilityMatrix;
        }
        public int GetScore(Matrix probabilityMatrix, int finishSingleMode)
        {
            Func<double[],int[], int, int> funcGetValue = ((x,y,z) =>
            {
                double CountMin = 0, CountMax = x[0];
                double RandomDbl = Math.Round(BoardParameter.RandomDouble(0, 100), 2);
                for (int i = 0; i <= 20; i++)
                {
                    if (RandomDbl >= CountMin && RandomDbl < CountMax)
                        return (int)y[i] * z;
                    if (i < 20)
                    {
                        CountMin = CountMin + x[i];
                        CountMax = CountMax + x[i + 1];
                    }
                }
                return -1;
            });
            Func<double[], int, int> funcGetValueMultiplikator = ((x,y) =>
            {
#if DEBUG
                double[] debug_x;
                if (x == null)
                {
                    Console.WriteLine("ToDebug double[] = null");
                    debug_x = null;
                }
                else
                    debug_x = x;

                int debug_y = y;
#endif
                try
                {
                    double CountMin = 0, CountMax = x[0], RandomDbl;
                    int Count = 0, CountUBound = x.GetLength(0);

                    RandomDbl = Math.Round(BoardParameter.RandomDouble(0, 100), 2);
                    for (int i = 0; i <= CountUBound; i++)
                    {
                        if (RandomDbl >= CountMin && RandomDbl < CountMax)
                        {
                            Count = i;
                            break;
                        }
                        if (i < CountUBound)
                        {
                            CountMin = CountMin + x[i];
                            CountMax = CountMax + x[i + 1];
                        }
                    }

                    return Count;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error = {0}", e.Source);
                    Console.WriteLine("Error = {0}", e.Message);
                    Console.WriteLine("Error = {0}", e.StackTrace);
#if DEBUG
                    Console.WriteLine("ToDebug double[] x: {0}", debug_x);
                    Console.WriteLine("ToDebug int x: {0}", debug_y);
#endif
                    return 0;
                }

            });

            int Score = new int(), Multiplikator = new int();

            if (finishSingleMode == 0 || finishSingleMode == 1 || 
                finishSingleMode == 2 || finishSingleMode == 3)
                Score = funcGetValue(probabilityMatrix.ScoreValue, probabilityMatrix.Value, finishSingleMode);
            
            Multiplikator = funcGetValueMultiplikator(probabilityMatrix.MultiplicationValue, finishSingleMode);

            if (Multiplikator == (int)ValueMultiplicator.Single || Multiplikator == (int)ValueMultiplicator.Double ||
                Multiplikator == (int)ValueMultiplicator.Triple)
                return Score;

            if (Multiplikator == 4 && Score == 0)
            {
                Matrix matrix = GetProbabilityMatrix(-1, (ValueMultiplicator)4);
                return funcGetValue(matrix.ScoreValue, matrix.Value, 1);
            }
            else if (Multiplikator == 5 && Score == 0)
            {
                Matrix matrix = GetProbabilityMatrix(-1,(ValueMultiplicator)5);
                return funcGetValue(matrix.ScoreValue, matrix.Value, 1);
            }

            if (Multiplikator == (int)ValueMultiplicator.HalfBull)
                return 25;
            if (Multiplikator == (int)ValueMultiplicator.Bull)
                return 50;

            return 0;
        }
        #endregion
        #region class library
        public class Matrix
        {
            public int[] Value = new int[21];
            public double[] ScoreValue = new double[21];
            public double[] MultiplicationValue = new double[6];
        }
        #endregion
    }
    public class Calculate : DartBot, IDisposable
    {
        #region constructor
        public Calculate(double[,] probability)
        {
            Probability = probability;
        }
        #endregion
        #region properties, fields, constants
        private static int stepSequence;

        private double[,] _Probability { get; set; }
        public double[,] Probability { get { return _Probability; } set { _Probability = value; } }

        private int _Score { get; set; }
        public int Score { get { return _Score; } private set { _Score = value; } }
        #endregion
        #region methods
        public int Sequence()
        {
            int returnScore = -1;
            stepSequence = 1;
            // Initialize
            if (stepSequence == 1)
            {
                stepSequence = 10;
            }

            // 
            if (stepSequence == 10)
            {
                returnScore = GetScore();
            }
            return returnScore;
        }
        private int GetScore()
        {
            int score = 0;
            double randomNumberDbl = (RandomNumberDbl * 100);
            int randomNumber = (int)randomNumberDbl;

            #region get throw field
            if (Probability[0, 1] == 1)
            {

            }
            #endregion
            return Score;
        }
        #endregion
        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Calculate()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    public class Table
    {
        public byte[] BoardValue = new byte[20]
        {
            20,1,18,4,13,6,10,15,2,17,03,19,7,16,8,11,14,09,12,5
        };

        public byte[] BoardValueMode = new byte[5]
        {
            1,2,3,25,50
        };
    }

    /// <summary>
    /// Math Parser class
    /// </summary>
    ///<param name="decimalSeperator" 
    public class MathParser
    {
        #region constructor
        /// <summary>
        /// Initialize new instance of MathParser
        /// (symbol of decimal separator is read
        /// from regional settings in system)
        /// </summary>
        public MathParser()
        {
            try
            {
                this.DecimalSeparator = Char.Parse(System.Globalization.CultureInfo.
                    CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            catch (Exception e)
            {
                throw new FormatException("Error: can't read char decimal " +
                    "separator from system, check your regional settings.", e);
            }
        }
        /// <summary>
        /// Initialize new instance of MathParser
        /// </summary>
        /// <param name="DecimalSeperator">Set decimal separator</param>
        public MathParser(char decimalSeperator)
        {
            this.DecimalSeparator = decimalSeperator;
        }
        #endregion
        #region properties
        private char _DecimalSeperator { get; set; }
        public char DecimalSeparator{ 
            get => _DecimalSeperator; set => _DecimalSeperator = value;
        }

        private bool _IsRadians { get; set; }
        public bool IsRadians { get => _IsRadians; set => _IsRadians = value; }
        #endregion
        #region constants
        private const string Plus = OperatorMarker + "+";
        private const string UnPlus = OperatorMarker + "un+";
        private const string Minus = OperatorMarker + "-";
        private const string UnMinus = OperatorMarker + "un-";
        private const string Multiply = OperatorMarker + "*";
        private const string Divide = OperatorMarker + "/";
        private const string Degree = OperatorMarker + "^";
        private const string LeftParent = OperatorMarker + "(";
        private const string RightParent = OperatorMarker + ")";
        private const string Sqrt = FunctionMarker + "sqrt";
        private const string Sin = FunctionMarker + "sin";
        private const string Cos = FunctionMarker + "cos";
        private const string Tg = FunctionMarker + "tg";
        private const string Ctg = FunctionMarker + "ctg";
        private const string Sh = FunctionMarker + "sh";
        private const string Ch = FunctionMarker + "ch";
        private const string Th = FunctionMarker + "th";
        private const string Log = FunctionMarker + "log";
        private const string Ln = FunctionMarker + "ln";
        private const string Exp = FunctionMarker + "exp";
        private const string Abs = FunctionMarker + "abs";

        private const string NumberMaker = "#";
        private const string OperatorMarker = "$";
        private const string FunctionMarker = "@";
        #endregion
        #region dictionarys
        private readonly Dictionary<string, string> supportedOperators =
            new Dictionary<string, string>
            {
                { "+", Plus },
                { "-", Minus },
                { "*", Multiply },
                { "/", Divide },
                { "^", Degree },
                { "(", LeftParent },
                { ")", RightParent }
            };

        private readonly Dictionary<string, string> supportedFunctions =
            new Dictionary<string, string>
            {
                { "sqrt", Sqrt },
                { "√", Sqrt },
                { "sin", Sin },
                { "cos", Cos },
                { "tg", Tg },
                { "ctg", Ctg },
                { "sh", Sh },
                { "ch", Ch },
                { "th", Th },
                { "log", Log },
                { "exp", Exp },
                { "abs", Abs }
            };

        private readonly Dictionary<string, string> supportedConstants =
            new Dictionary<string, string>
            {
                {"pi", NumberMaker +  Math.PI.ToString() },
                {"e", NumberMaker + Math.E.ToString() }
            };
        #endregion
        #region methods
        /// <summary>
        /// Produce result of the given math expression
        /// </summary>
        /// <param name="expression">Math expression (infix/standard notation)</param>
        /// <returns>Result</returns>
        public double Parse(string expression, bool isRadians = true)
        {
            this.IsRadians = isRadians;

            try
            {
                return Calculate(ConvertToRPN(FormatString(expression)));
            }
            catch (DivideByZeroException e)
            {
                throw e;
            }
            catch (FormatException e)
            {
                throw e;
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Format string in one iteration and check number of parenthesis
        /// </summary>
        private string FormatString(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException("Expression is null or empty");

            StringBuilder formattedString = new StringBuilder();
            int balanceOfParenth = 0;
            formattedString.Append(expression.Trim().ToLower());

            // Check if parenth equals 0
            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];

                if (ch == '(')
                    balanceOfParenth++;
                else if (ch == ')')
                    balanceOfParenth--;
            }

            if (balanceOfParenth != 0)
                throw new FormatException("Number of left and right parenthesis is not equal");

            return formattedString.ToString();
        }

        /// <summary>
        /// Produce math expression in reverse polish notation
        /// by the given string
        /// </summary>
        /// <param name="expression">Math expression in infix notation</param>
        /// <returns>Math expression in postfix notation (RPN)</returns>
        private string ConvertToRPN(string expression)
        {
            int i = 0;
            StringBuilder outputString = new StringBuilder();
            Stack<string> stack = new Stack<string>();

            // While there is unhandled char in expression
            while (i < expression.Length)
            {
                string token = LexicalAnalysisInfixNotation(expression, ref i);
                outputString = SyntaxAnalysisInfixNotation(token, outputString, stack);
            }

            // Pop all elements from stack to output string            
            while (stack.Count > 0)
            {
                // There should be only operators
                if (stack.Peek()[0] == OperatorMarker[0])
                    outputString.Append(stack.Pop());
                else
                    throw new FormatException("Format exception,"
                    + " there is function without parenthesis");
            }

            return outputString.ToString();
        }

        /// <summary>
        /// Produce token by the given math expression
        /// </summary>
        /// <param name="expression">Math expression in infix notation</param>
        /// <param name="pos">Current position in string for lexical analysis</param>
        /// <returns>Token</returns>
        private string LexicalAnalysisInfixNotation(string expression, ref int i)
        {
            // Receive first char
            StringBuilder token = new StringBuilder();
            token.Append(expression[i]);

            // If it is a operator
            if (supportedOperators.ContainsKey(token.ToString()))
            {
                // Determine it is unary or binary operator
                bool isUnary = i == 0 || expression[i - 1] == '(';
                i++;

                switch (token.ToString())
                {
                    case "+":
                        return isUnary ? UnPlus : Plus;
                    case "-":
                        return isUnary ? UnMinus : Minus;
                    default:
                        return supportedOperators[token.ToString()];
                }
            }
            else if (Char.IsLetter(token[0])
                || supportedFunctions.ContainsKey(token.ToString())
                || supportedConstants.ContainsKey(token.ToString()))
            {
                // Read function or constant name
                while (++i < expression.Length
                    && Char.IsLetter(expression[i]))
                    token.Append(expression[i]);

                if (supportedFunctions.ContainsKey(token.ToString()))
                    return supportedFunctions[token.ToString()];
                else if (supportedConstants.ContainsKey(token.ToString()))
                    return supportedConstants[token.ToString()];
                else
                    throw new ArgumentException("Unknown token");

            }
            else if (Char.IsDigit(token[0]) || token[0] == DecimalSeparator)
            {
                // Read number
                // Read the whole part of number
                if (Char.IsDigit(token[0]))
                {
                    while (++i < expression.Length
                    && Char.IsDigit(expression[i]))
                        token.Append(expression[i]);
                }
                else
                    // Because system decimal separator
                    // will be added below
                    token.Clear();

                // Read the fractional part of number
                if (i < expression.Length
                    && expression[i] == DecimalSeparator)
                {
                    // Add current system specific decimal separator
                    token.Append(System.Globalization.CultureInfo.CurrentCulture.
                        NumberFormat.NumberDecimalSeparator);

                    while (++i < expression.Length
                    && Char.IsDigit(expression[i]))
                        token.Append(expression[i]);
                }

                // Read scientific notation (suffix)
                if (i + 1 < expression.Length && expression[i] == 'e'
                    && (Char.IsDigit(expression[i + 1])
                        || (i + 2 < expression.Length
                            && (expression[i + 1] == '+'
                                || expression[i + 1] == '-')
                            && Char.IsDigit(expression[i + 2]))))
                {
                    token.Append(expression[i++]); // e

                    if (expression[i] == '+' || expression[i] == '-')
                        token.Append(expression[i++]); // sign

                    while (i < expression.Length
                        && Char.IsDigit(expression[i]))
                        token.Append(expression[i++]); // power

                    // Convert number from scientific notation to decimal notation
                    return NumberMaker + Convert.ToDouble(token.ToString());
                }

                return NumberMaker + token.ToString();
            }
            else
            {
                throw new ArgumentException("Unknown token in expression");
            }
        }

        /// <summary>
        /// Syntax analysis of infix notation
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="outputString">Output string (math expression in RPN)</param>
        /// <param name="stack">Stack which contains operators (or functions)</param>
        /// <returns>Output string (math expression in RPN)</returns>
        private StringBuilder SyntaxAnalysisInfixNotation(string token, StringBuilder outputString, Stack<string> stack)
        {
            // If it's a number just put to string            
            if (token[0] == NumberMaker[0])
                outputString.Append(token);
            else if (token[0] == FunctionMarker[0])
                // if it's a function push to stack
                stack.Push(token);
            else if (token == LeftParent)
                // If its '(' push to stack
                stack.Push(token);
            else if (token == RightParent)
            {
                // If its ')' pop elements from stack to output string
                // until find the ')'

                string elem;
                while ((elem = stack.Pop()) != LeftParent)
                    outputString.Append(elem);

                // if after this a function is in the peek of stack then put it to string
                if (stack.Count > 0 &&
                    stack.Peek()[0] == FunctionMarker[0])
                    outputString.Append(stack.Pop());
            }
            else
            {
                // While priority of elements at peek of stack >= (>) token's priority
                // put these elements to output string
                while (stack.Count > 0 &&
                    Priority(token, stack.Peek()))
                    outputString.Append(stack.Pop());

                stack.Push(token);
            }

            return outputString;
        }

        /// <summary>
        /// Is priority of token less (or equal) to priority of p
        /// </summary>
        private bool Priority(string token, string p)
        {
            return IsRightAssociated(token) ?
                GetPriority(token) < GetPriority(p) :
                GetPriority(token) <= GetPriority(p);
        }

        /// <summary>
        /// Is right associated operator
        /// </summary>
        private bool IsRightAssociated(string token)
        {
            return token == Degree;
        }

        /// <summary>
        /// Get priority of operator
        /// </summary>
        private int GetPriority(string token)
        {
            switch (token)
            {
                case LeftParent:
                    return 0;
                case Plus:
                case Minus:
                    return 2;
                case UnPlus:
                case UnMinus:
                    return 6;
                case Multiply:
                case Divide:
                    return 4;
                case Degree:
                case Sqrt:
                    return 8;
                case Sin:
                case Cos:
                case Tg:
                case Ctg:
                case Sh:
                case Ch:
                case Th:
                case Log:
                case Ln:
                case Exp:
                case Abs:
                    return 10;
                default:
                    throw new ArgumentException("Unknown operator");
            }
        }

        /// <summary>
        /// Calculate expression in reverse-polish notation
        /// </summary>
        /// <param name="expression">Math expression in reverse-polish notation</param>
        /// <returns>Result</returns>
        private double Calculate(string expression)
        {
            int pos = 0; // Current position of lexical analysis
            var stack = new Stack<double>(); // Contains operands

            // Analyse entire expression
            while (pos < expression.Length)
            {
                string token = LexicalAnalysisRPN(expression, ref pos);
                stack = SyntaxAnalysisRPN(stack, token);
            }

            // At end of analysis in stack should be only one operand (result)
            if (stack.Count > 1)
                throw new ArgumentException("Excess operand");

            return stack.Pop();
        }

        /// <summary>
        /// Produce token by the given math expression
        /// </summary>
        /// <param name="expression">Math expression in reverse-polish notation</param>
        /// <param name="pos">Current position of lexical analysis</param>
        /// <returns>Token</returns>
        private string LexicalAnalysisRPN(string expression, ref int pos)
        {
            StringBuilder token = new StringBuilder();

            // Read token from marker to next marker

            token.Append(expression[pos++]);

            while (pos < expression.Length && expression[pos] != NumberMaker[0]
                && expression[pos] != OperatorMarker[0]
                && expression[pos] != FunctionMarker[0])
            {
                token.Append(expression[pos++]);
            }

            return token.ToString();
        }

        /// <summary>
        /// Syntax analysis of reverse-polish notation
        /// </summary>
        /// <param name="stack">Stack which contains operands</param>
        /// <param name="token">Token</param>
        /// <returns>Stack which contains operands</returns>
        private Stack<double> SyntaxAnalysisRPN(Stack<double> stack, string token)
        {
            // if it's operand then just push it to stack
            if (token[0] == NumberMaker[0])
            {
                stack.Push(double.Parse(token.Remove(0, 1)));
            }
            // Otherwise apply operator or function to elements in stack
            else if (NumberOfArguments(token) == 1)
            {
                double arg = stack.Pop();
                double rst;

                switch (token)
                {
                    case UnPlus:
                        rst = arg;
                        break;
                    case UnMinus:
                        rst = -arg;
                        break;
                    case Sqrt:
                        rst = Math.Sqrt(arg);
                        break;
                    case Sin:
                        rst = ApplyTrigFunction(Math.Sin, arg);
                        break;
                    case Cos:
                        rst = ApplyTrigFunction(Math.Cos, arg);
                        break;
                    case Tg:
                        rst = ApplyTrigFunction(Math.Tan, arg);
                        break;
                    case Ctg:
                        rst = 1 / ApplyTrigFunction(Math.Tan, arg);
                        break;
                    case Sh:
                        rst = Math.Sinh(arg);
                        break;
                    case Ch:
                        rst =
                    rst = Math.Cosh(arg);
                        break;
                    case Th:
                        rst = Math.Tanh(arg);
                        break;
                    case Ln:
                        rst = Math.Log(arg);
                        break;
                    case Exp:
                        rst = Math.Exp(arg);
                        break;
                    case Abs:
                        rst = Math.Abs(arg);
                        break;
                    default:
                        throw new ArgumentException("Unknown operator");
                }

                stack.Push(rst);
            }
            else
            {
                // otherwise operator's number of arguments equals to 2

                double arg2 = stack.Pop();
                double arg1 = stack.Pop();

                double rst;

                switch (token)
                {
                    case Plus:
                        rst = arg1 + arg2;
                        break;
                    case Minus:
                        rst = arg1 - arg2;
                        break;
                    case Multiply:
                        rst = arg1 * arg2;
                        break;
                    case Divide:
                        if (arg2 == 0)
                        {
                            throw new DivideByZeroException("Second argument is zero");
                        }
                        rst = arg1 / arg2;
                        break;
                    case Degree:
                        rst = Math.Pow(arg1, arg2);
                        break;
                    case Log:
                        rst = Math.Log(arg2, arg1);
                        break;
                    default:
                        throw new ArgumentException("Unknown operator");
                }

                stack.Push(rst);
            }

            return stack;
        }

        /// <summary>
        /// Apply trigonometric function
        /// </summary>
        /// <param name="func">Trigonometric function</param>
        /// <param name="arg">Argument</param>
        /// <returns>Result of function</returns>
        private double ApplyTrigFunction(Func<double, double> func, double arg)
        {
            if (!IsRadians)
                arg = arg * Math.PI / 180; // Convert value to degree

            return func(arg);
        }

        /// <summary>
        /// Produce number of arguments for the given operator
        /// </summary>
        private int NumberOfArguments(string token)
        {
            switch (token)
            {
                case UnPlus:
                case UnMinus:
                case Sqrt:
                case Tg:
                case Sh:
                case Ch:
                case Th:
                case Ln:
                case Ctg:
                case Sin:
                case Cos:
                case Exp:
                case Abs:
                    return 1;
                case Plus:
                case Minus:
                case Multiply:
                case Divide:
                case Degree:
                case Log:
                    return 2;
                default:
                    throw new ArgumentException("Unknown operator");
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Windows;

namespace DDD_WPF.Screens._Global
{
    /// <summary>
    /// Interaction logic for _G_Prompt.xaml
    /// </summary>
    public partial class _G_Prompt : Window,IDisposable
    {
        #region constructor
        public _G_Prompt() : this(0,0) { }
        /// <summary>
        /// Set action for the prompt
        /// </summary>
        /// <param name="action">
        /// Case 1: OK(1) / 
        /// Case 2: Yes(1),No(2) / 
        /// Case 3: Yes(1),Cancle(2),No(3)
        /// </param>
        /// <param name="returnID">Type in the actual step sequence</param>
        public _G_Prompt(int action, int returnID)
        {
            InitializeComponent();
            VisibilityChanged += new EventHandler<CustomEventArgs>(this.Visibility_Action);
            if (action == 1) Visibility_Action01 = true;
            if (action == 2) Visibility_Action02 = true;
            if (action == 3) Visibility_Action03 = true;
            ReturnID = returnID;
        }
        /// <summary>
        /// Set descriptions for the prompt
        /// </summary>
        /// <param name="header">Type in header</param>
        /// <param name="description1">Type in description 1</param>
        /// <param name="description2">Type in description 2</param>
        public _G_Prompt(string header, string description1, string description2)
        {
            InitializeComponent();
            VisibilityChanged += new EventHandler<CustomEventArgs>(this.Visibility_Action);
            Header = header;
            Description1 = description1;
            Description2 = description2;
        }
        /// <summary>
        /// Set properties for the prompt
        /// </summary>
        /// <param name="action">        
        /// Case 1: OK(1) / 
        /// Case 2: Yes(1),No(2) / 
        /// Case 3: Yes(1),Cancle(2),No(3)
        /// </param>
        /// <param name="header">Type in header</param>
        /// <param name="description1">Type in description 1</param>
        /// <param name="returnID">Type in the actual step sequence</param>
        public _G_Prompt(int action, int returnID, string header, string description1)
        {
            InitializeComponent();
            VisibilityChanged += new EventHandler<CustomEventArgs>(this.Visibility_Action);
            if (action == 1) Visibility_Action01 = true;
            if (action == 2) Visibility_Action02 = true;
            if (action == 3) Visibility_Action03 = true;
            Header = header;
            Description1 = description1;
            ReturnID = returnID;
        }
        /// <summary>
        /// Set properties for the prompt
        /// </summary>
        /// <param name="action">        
        /// Case 1: OK(1) / 
        /// Case 2: Yes(1),No(2) / 
        /// Case 3: Yes(1),Cancle(2),No(3)
        /// </param>
        /// <param name="header">Type in header</param>
        /// <param name="description1">Type in description 1</param>
        /// <param name="description2">Type in description 2</param>
        public _G_Prompt(int action, string header, string description1, string description2)
        {
            InitializeComponent();
            VisibilityChanged += new EventHandler<CustomEventArgs>(this.Visibility_Action);
            if (action == 1) Visibility_Action01 = true;
            if (action == 2) Visibility_Action02 = true;
            if (action == 3) Visibility_Action03 = true;
            Header = header;
            Description1 = description1;
            Description2 = description2;
        }
        /// <summary>
        /// Set properties for the prompt
        /// </summary>
        /// <param name="action">        
        /// Case 1: OK(1) / 
        /// Case 2: Yes(1),No(2) / 
        /// Case 3: Yes(1),Cancle(2),No(3)
        /// </param>
        /// <param name="header">Type in header</param>
        /// <param name="description1">Type in description 1</param>
        /// <param name="description2">Type in description 2</param>
        /// <param name="returnID">Type in the actual step sequence</param>
        public _G_Prompt(int action, int returnID, string header, string description1, string description2)
        {
            InitializeComponent();
            VisibilityChanged += new EventHandler<CustomEventArgs>(this.Visibility_Action);
            if (action == 1) Visibility_Action01 = true;
            if (action == 2) Visibility_Action02 = true;
            if (action == 3) Visibility_Action03 = true;
            Header = header;
            Description1 = description1;
            Description2 = description2;
            ReturnID = returnID;
        }
        #endregion
        #region event definition: change visibility
        private EventHandler<CustomEventArgs> _VisibilityChanged;
        public event EventHandler<CustomEventArgs> VisibilityChanged
        {
            add { _VisibilityChanged += value; }
            remove { _VisibilityChanged -= value; }
        }
        public void ChangedVisibility(int arg)
        {
            CustomEventArgs customEventArgs = new CustomEventArgs(arg, _Visibility_Action01, _Visibility_Action02, _Visibility_Action03);

            _VisibilityChanged.Invoke(this, customEventArgs);
        }
        public class CustomEventArgs : EventArgs
        {
            public CustomEventArgs() : this(0, false, false, false) { }
            public CustomEventArgs(int arg, bool visibility01, bool visibility02, bool visibility03)
            {
                visibilityAction.Add(arg);
                visibilityAction.Add(visibility01);
                visibilityAction.Add(visibility02);
                visibilityAction.Add(visibility03);
            }
            public List<object> visibilityAction = new List<object>(4);
        }
        #endregion
        #region event definition: return trigger
        private EventHandler<ReturnEventArgs> _ReturnEventHandler;
        public event EventHandler<ReturnEventArgs> ReturnEventHandler
        {
            add { _ReturnEventHandler += value; }
            remove { _ReturnEventHandler -= value; }
        }
        public void ReturnHandler()
        {
            if (_ReturnEventHandler != null)
            {
                ReturnEventArgs args = new ReturnEventArgs(ReturnValue, ReturnID);
                _ReturnEventHandler.Invoke(this, args);
            }
        }
        public class ReturnEventArgs : EventArgs
        {
            public ReturnEventArgs() : this(0) { }
            public ReturnEventArgs(int args)
            {
                ReturnValue = args;
            }
            public ReturnEventArgs(int args, int returnID)
            {
                ReturnValue = args;
                ReturnID = returnID;
            }

            public int ReturnValue;
            public int ReturnID;
        }
        #endregion
        #region properties, fields
        private bool _Visibility_Action01 { get; set; }
        public bool Visibility_Action01
        {
            get { return _Visibility_Action01; }
            set 
            {
                ChangedVisibility(1);
            }
        }

        private bool _Visibility_Action02 { get; set; }
        public bool Visibility_Action02
        {
            get { return _Visibility_Action02; }
            set 
            {
                ChangedVisibility(2);
            }
        }

        private bool _Visibility_Action03 { get; set; }
        public bool Visibility_Action03
        {
            get { return _Visibility_Action03; }
            set 
            {
                ChangedVisibility(3);
            }
        }

        private string _Header { get; set; }
        public string Header { 
            get { return _Header; }
            set 
            {
                _G_Txt_Out_Header_Line_01.Text = value;
                _Header = value; 
            } }

        private string _Description1 { get; set; }
        public string Description1 { 
            get { return _Description1; } 
            set 
            {
                _G_Txt_Out_Val_Line_02.Text = value;
                _Description1 = value; 
            } }

        private string _Description2 { get; set; }
        public string Description2 {
            get { return _Description2; }
            set
            {
                _G_Txt_Out_Val_Line_03.Text = value;
                _Description2 = value;
            }
        }

        private int _ReturnID { get; set; }
        public int ReturnID 
        { 
            get { return _ReturnID; }
            set { _ReturnID = value; } 
        }

        private bool _AnimateShowClose { get; set; }
        public bool AnimateShowClose
        {
            get { return _AnimateShowClose; }
            set { _AnimateShowClose = value; }
        }

        private int ReturnValue;
        #endregion
        #region events
        private void Visibility_Action(object sender, CustomEventArgs e)
        {
            SetActionVisible((int)e.visibilityAction[0]);
            if ((int)e.visibilityAction[0] == 1)
            {
                _Visibility_Action01 = true;
                _Visibility_Action02 = false;
                _Visibility_Action03 = false;
            }
            if ((int)e.visibilityAction[0] == 2)
            {
                _Visibility_Action01 = false;
                _Visibility_Action02 = true;
                _Visibility_Action03 = false;
            }
            if ((int)e.visibilityAction[0] == 3)
            {
                _Visibility_Action01 = false;
                _Visibility_Action02 = false;
                _Visibility_Action03 = true;
            }
        }
        private void _G_Btn_01_OK_Click(object sender, RoutedEventArgs e)
        {
            ReturnValue = 1;
            ReturnHandler();
            ReturnValue = 0;
            this.Close();
        }
        private void _G_Btn_02_Yes_Click(object sender, RoutedEventArgs e)
        {
            ReturnValue = 1;
            ReturnHandler();
            ReturnValue = 0;
            this.Close();
        }
        private void _G_Btn_02_No_Click(object sender, RoutedEventArgs e)
        {
            ReturnValue = 2;
            ReturnHandler();
            ReturnValue = 0;
            this.Close();
        }
        private void _G_Btn_03_Yes_Click(object sender, RoutedEventArgs e)
        {
            ReturnValue = 1;
            ReturnHandler();
            ReturnValue = 0;
            this.Close();
        }
        private void _G_Btn_03_Cancle_Click(object sender, RoutedEventArgs e)
        {
            ReturnValue = 2;
            ReturnHandler();
            ReturnValue = 0;
            this.Close();
        }
        private void _G_Btn_03_No_Click(object sender, RoutedEventArgs e)
        {
            ReturnValue = 3;
            ReturnHandler();
            ReturnValue = 0;
            this.Close();
        }
        #endregion
        #region methods
        private void SetActionVisible(int action)
        {
            Visibility btnVisTrue = Visibility.Visible;
            Visibility btnVisFalse = Visibility.Hidden;
            
            switch (action)
            {
                case 1:
                    _G_Btn_01_OK.Visibility = btnVisTrue;
                    _G_Btn_02_Yes.Visibility = btnVisFalse;
                    _G_Btn_02_No.Visibility = btnVisFalse;
                    _G_Btn_03_Yes.Visibility = btnVisFalse;
                    _G_Btn_03_Cancle.Visibility = btnVisFalse;
                    _G_Btn_03_No.Visibility = btnVisFalse;
                    break;
                case 2:
                    _G_Btn_01_OK.Visibility = btnVisFalse;
                    _G_Btn_02_Yes.Visibility = btnVisTrue;
                    _G_Btn_02_No.Visibility = btnVisTrue;
                    _G_Btn_03_Yes.Visibility = btnVisFalse;
                    _G_Btn_03_Cancle.Visibility = btnVisFalse;
                    _G_Btn_03_No.Visibility = btnVisFalse;
                    break;
                case 3:
                    _G_Btn_01_OK.Visibility = btnVisFalse;
                    _G_Btn_02_Yes.Visibility = btnVisFalse;
                    _G_Btn_02_No.Visibility = btnVisFalse;
                    _G_Btn_03_Yes.Visibility = btnVisTrue;
                    _G_Btn_03_Cancle.Visibility = btnVisTrue;
                    _G_Btn_03_No.Visibility = btnVisTrue;
                    break;
                default:
                    break;
            }
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
        ~_G_Prompt()
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
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Threading;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using WinForms = System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

using BondTech.HotKeyManagement.WPF._4;


namespace DogDrip
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        HotKeyManager MyHotKeyManager;
        private WshShell WSH = new WshShell();
        public System.Windows.Forms.NotifyIcon notify;
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        #region **HotKeys
        GlobalHotKey ghkDefault = new GlobalHotKey("ghkDefault", ModifierKeys.Control, Keys.D, true);
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            
            System.Drawing.Icon icon = DogDrip.Properties.Resources.favicon;
            MemoryStream iconStream = new MemoryStream();
            icon.Save(iconStream);
            iconStream.Seek(0, SeekOrigin.Begin);
            BitmapFrame newIcon = BitmapFrame.Create(iconStream);
            this.Icon = newIcon;

            textBox1.HotKeyIsSet += (s, e) =>
            {
                ModifierKeys modifier = ModifierKeys.None;
                Keys keys = Keys.None;

                modifier = e.UserModifier;
                keys = e.UserKey;

                ghkDefault.Key = keys;
                ghkDefault.Modifier = modifier;
                textBox1.textRemoverButton.Click += TextRemoverButton_Click;
            };
        }

        private void TextRemoverButton_Click(object sender, RoutedEventArgs e)
        {
            ghkDefault.Key = Keys.None;
            ghkDefault.Modifier = ModifierKeys.None;
        }

        void MyHotKeyManager_GlobalHotKeyPressed(object sender, GlobalHotKeyEventArgs e)
        {
            switch (e.HotKey.Name.ToLower())
            {
                case "ghkdefault":
                    ClipboardCalc();
                    break;
            }
        }

        void RegisterHotKeys()
        {
            MyHotKeyManager = new HotKeyManager(this);
            MyHotKeyManager.AddGlobalHotKey(ghkDefault);
            MyHotKeyManager.GlobalHotKeyPressed += new GlobalHotKeyEventHandler(MyHotKeyManager_GlobalHotKeyPressed);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();

                System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
                System.Windows.Forms.MenuItem item2 = new System.Windows.Forms.MenuItem();
                System.Windows.Forms.MenuItem item3 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item1);
                menu.MenuItems.Add(item2);
                menu.MenuItems.Add(item3);
                item1.Index = 0;
                item1.Text = "종료";
                item1.Click += delegate (object click, EventArgs eClick)
                {
                    notify.Visible = false;
                    notify.Dispose();
                    System.Windows.Application.Current.Shutdown();
                };
                item2.Index = 0;
                item2.Text = "도움말";
                item2.Click += delegate (object click, EventArgs eClick)
                {
                    Window read_me = new ReadMe();
                    read_me.Show();
                };
                item3.Index = 0;
                item3.Text = "열기";
                item3.Click += delegate (object click, EventArgs eClick)
                {
                    this.Show();
                };

                notify = new System.Windows.Forms.NotifyIcon();
                notify.Icon = DogDrip.Properties.Resources.favicon;
                notify.Visible = true;
                notify.DoubleClick +=
                    delegate (object senders, EventArgs args)
                    {
                        this.Show();
                        this.WindowState = WindowState.Normal;
                    };
                notify.ContextMenu = menu;
                notify.Text = "평↔m² 변환기 v1.0";
            }
            catch (Exception ee)
            {

            }
        }

        public static void DoIt()
        {
            MessageBox.Show("HotKey pressed!");
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterHotKeys();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            if (show_notify != false)
            {
                notify.BalloonTipTitle = "평↔m² 변환기 v1.0";
                notify.BalloonTipText = "시스템 트레이로 옮겨졌습니다.";
                notify.ShowBalloonTip(10000);
            }
        }

        bool print_measure = false;
        bool reverse_mode = false;
        bool show_notify = true;

        #region **CheckBoxOptions
        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            print_measure = true;
        }
        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            print_measure = false;
        }
        private void CheckBox2_Checked(object sender, RoutedEventArgs e)
        {
            reverse_mode = true;
        }
        private void CheckBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            reverse_mode = false;
        }
        private void CheckBox3_Checked(object sender, RoutedEventArgs e)
        {
            show_notify = false;
        }
        private void CheckBox3_Unchecked(object sender, RoutedEventArgs e)
        {
            show_notify = true;
        }
        #endregion

        void ClipboardCalc()
        {
            string resultStr = string.Empty;
            string temp = string.Empty;
            string digit = string.Empty;
            double result = 0;



            // Need to copy "selected text from other application" to clipboard
            //_timer.Start();
            //resultStr = Clipboard.GetText();
            try
            {
            resultStr = GetTextFromFocusedControl();
            resultStr = string.Join(string.Empty, Regex.Match(resultStr, @"\d+(\.\d+)?"));
                result = Convert.ToDouble(resultStr);
            if (reverse_mode == false)
                result *= 3.30579;
            else
                result /= 3.30579;
            digit = "F" + textBox2.Text;

            if (result == (int)result)
                resultStr = result.ToString();
            else
                resultStr = result.ToString(digit);
            resultStr = string.Format("{0:0.##########}", Convert.ToDouble(resultStr));

            if (print_measure != false)
            {
                if (reverse_mode != false)
                    resultStr += "평";
                else
                    resultStr += "m²";
            }

            // Then paste clipboard text here.

            SetTextFromFocusedControl(resultStr);
            }
            catch (Exception exp)
            {
            }
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(combined_text(sender as TextBox, e.Text));
        }
        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 0 && i <= 10;
        }
        private string combined_text(TextBox tb, string new_text)
        {
            return tb.Text.Substring(0, tb.SelectionStart) + new_text + tb.Text.Substring(tb.SelectionStart + tb.SelectionLength);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(GetTextFromFocusedControl());
            }
            catch (Exception exp)
            {
                Clipboard.SetText(exp.Message);
            }
        }

        public const uint WM_SYSKEYDOWN = 260;
        public const uint WM_COPY = 0x0301;
        public const uint WM_PASTE = 0x0302;
        public const uint WM_KEYDOWN = 0x0100;
        public const uint WM_KEYUP = 0x0101;
        public const uint EM_GETSEL = 0xB0;
        public const uint EM_SETSEL = 0xB1;
        public const uint WM_DRAWCLIPBOARD = 0x0308;
        public const uint WM_GETTEXT = 13;
        private object boolFalse = false;
        public const uint CF_TEXT = 1;

        //IntPtr key = (IntPtr)Keys.F9;

        private string GetTextFromControlAtMousePosition()
        {
            IntPtr selectionStart = IntPtr.Zero;
            IntPtr selectionEnd = IntPtr.Zero;
            string w = string.Empty;

            try
            {
                Point p;
                if (GetCursorPos(out p))
                {
                    IntPtr ptr = WindowFromPoint(p);
                    if (ptr != IntPtr.Zero)
                    {
                        SendMessage(ptr, WM_COPY, out selectionStart, out selectionEnd);
                        w = Clipboard.GetText();
                        SendMessage(ptr, WM_PASTE, out selectionStart, out selectionEnd);
                    }
                }
                return w;
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }
        //Get the text of the focused control
        private string GetTextFromFocusedControl()
        {
            try
            {
                
                int activeWinPtr = GetForegroundWindow().ToInt32();
                int activeThreadId = 0, processId;
                activeThreadId = GetWindowThreadProcessId(activeWinPtr, out processId);
                int currentThreadId = GetCurrentThreadId();
                if (activeThreadId != currentThreadId)
                    AttachThreadInput(activeThreadId, currentThreadId, true);
                IntPtr activeCtrlId = GetFocus();
                
                IntPtr selectionStart = IntPtr.Zero;
                IntPtr selectionEnd = IntPtr.Zero;

                IntPtr test;
                string w = string.Empty;
                //uint key = (uint)System.Windows.Forms.Keys.C;
                //uint modifier = (uint)System.Windows.Forms.Keys.ControlKey;
                //SendMessage(activeCtrlId, WM_SYSKEYDOWN, out key, out lptr);
                //SendMessage(activeCtrlId, EM_GETSEL, out selectionStart, out selectionEnd);
                //SendMessage(activeCtrlId, EM_GETSEL, out selectionStart, out selectionEnd);
                test = GetOpenClipboardWindow();
                if (test != IntPtr.Zero)
                    System.Console.WriteLine(test.ToString());
                //WSH.SendKeys("^{c}", boolFalse);
                System.Windows.Forms.SendKeys.SendWait("^{c}");
                Thread.Sleep(50);
                test = GetOpenClipboardWindow();
                if (test != IntPtr.Zero)
                    System.Console.WriteLine(test.ToString());
                //SendMessage(activeCtrlId, WM_COPY, out selectionStart, out selectionEnd);
                //SendMessage(activeCtrlId, WM_DRAWCLIPBOARD, 0, 0);
                //PostMessage(activeCtrlId, WM_KEYDOWN, modifier, 0x01);
                //PostMessage(activeCtrlId, WM_KEYDOWN, key, 0x01);
                //PostMessage(activeCtrlId, WM_KEYUP, key, 0xC0000001);
                //PostMessage(activeCtrlId, WM_KEYUP, modifier, 0xC0000001);
                //IntPtr selectionStart = IntPtr.Zero;
                //IntPtr selectionEnd = IntPtr.Zero;
                //uint selectionStart = 0;
                //uint selectionEnd = 0;
                //bool _opened = false;
                //_opened = OpenClipboard(activeCtrlId);

                //test = GetOpenClipboardWindow();
                if (test != IntPtr.Zero)
                    System.Console.WriteLine(test.ToString());
                IDataObject iData = Clipboard.GetDataObject();
                //test = GetClipboardData(CF_TEXT);
                //int length = System.Runtime.InteropServices.Marshal.SizeOf(test);
                //byte[] buffer = new byte[length];
                //IntPtr glock = GlobalLock(test);
                //Marshal.Copy(glock, buffer, 0, length);
                test = GetOpenClipboardWindow();
                if (test != IntPtr.Zero)
                    System.Console.WriteLine(test.ToString());
                //w = Encoding.Default.GetString(buffer);
                w = (string)iData.GetData(DataFormats.Text);
                //SendMessage(activeCtrlId, EM_GETSEL, out selectionStart, out selectionEnd);
                //Thread.Sleep(5000);
                //SendMessage(activeCtrlId, EM_SETSEL, out selectionStart, out selectionEnd);

                return w;
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }

        //Get the text of the focused control
        void SetTextFromFocusedControl(string str)
        {
            try
            {
                int activeWinPtr = GetForegroundWindow().ToInt32();
                int activeThreadId = 0, processId;
                activeThreadId = GetWindowThreadProcessId(activeWinPtr, out processId);
                int currentThreadId = GetCurrentThreadId();
                if (activeThreadId != currentThreadId)
                    AttachThreadInput(activeThreadId, currentThreadId, true);
                IntPtr activeCtrlId = GetFocus();
                IntPtr selectionStart = IntPtr.Zero;
                IntPtr selectionEnd = IntPtr.Zero;
                string w = string.Empty;
                uint key = (uint)System.Windows.Forms.Keys.V;
                uint modifier = (uint)System.Windows.Forms.Keys.ControlKey;

                Clipboard.SetDataObject(str);
                Thread.Sleep(50);
                //WSH.SendKeys("^{v}", boolFalse);
                System.Windows.Forms.SendKeys.SendWait("^{v}");

                //SendMessage(activeCtrlId, WM_PASTE, out selectionStart, out selectionEnd);
                //PostMessage(activeCtrlId, WM_KEYDOWN, modifier, 0x01);
                //PostMessage(activeCtrlId, WM_KEYDOWN, key, 0x01);
                //PostMessage(activeCtrlId, WM_KEYUP, key, 0xC0000001);
                //PostMessage(activeCtrlId, WM_KEYUP, modifier, 0xC0000001);
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }
        }

        ////Get the text of the focused control
        //private string GetTextFromFocusedControl()
        //{
        //    try
        //    {
        //        int activeWinPtr = GetForegroundWindow().ToInt32();
        //        int activeThreadId = 0, processId;
        //        activeThreadId = GetWindowThreadProcessId(activeWinPtr, out processId);
        //        int currentThreadId = GetCurrentThreadId();
        //        if (activeThreadId != currentThreadId)
        //            AttachThreadInput(activeThreadId, currentThreadId, true);
        //        IntPtr activeCtrlId = GetFocus();

        //        return GetText(activeCtrlId);
        //    }
        //    catch (Exception exp)
        //    {
        //        return exp.Message;
        //    }
        //}

        ////Get the text of the control at the mouse position
        //private string GetTextFromControlAtMousePosition()
        //{
        //    try
        //    {
        //        Point p;
        //        if (GetCursorPos(out p))
        //        {
        //            IntPtr ptr = WindowFromPoint(p);
        //            if (ptr != IntPtr.Zero)
        //            {
        //                return GetText(ptr);
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception exp)
        //    {
        //        return exp.Message;
        //    }
        //}

        ////Get the text of a control with its handle
        //private string GetText(IntPtr handle)
        //{
        //    int maxLength = 100;
        //    IntPtr buffer = Marshal.AllocHGlobal((maxLength + 1) * 2);
        //    IntPtr selectionStart = IntPtr.Zero;
        //    IntPtr selectionEnd = IntPtr.Zero;
        //    string w = string.Empty;
        //    SendMessage(handle, EM_GETSEL, out selectionStart, out selectionEnd);
        //    SendMessageW(handle, WM_GETTEXT, maxLength, buffer);
        //    w = Marshal.PtrToStringUni(buffer);
        //    Marshal.FreeHGlobal(buffer);
        //    if (selectionStart != IntPtr.Zero && selectionEnd != IntPtr.Zero)
        //    {
        //        w = w.Substring((int)selectionStart);
        //        // Fix it
        //    }

        //    return w;
        //}

        [DllImport("kernel.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("user32.dll")]
        static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        public static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, out IntPtr wParam, out IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, out uint wParam, out uint lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, uint Msg, out IntPtr wParam, out IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int WaitForInputIdle(int hProcess, uint dwMilliseconds);


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pt);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        public static extern int SendMessageW([InAttribute] System.IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
        

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetFocus();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowThreadProcessId(int handle, out int processId);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern int AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);
        [DllImport("kernel32.dll")]
        internal static extern int GetCurrentThreadId();

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowText(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, int nMaxCount);
    }
}
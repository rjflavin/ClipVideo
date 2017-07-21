using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace ClipVideo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            IntPtr window = this.Handle;

            //Attach Hook
            attachWindowHook();

            //Initial Video Setup
            axWindowsMediaPlayer1.URL = @"C:\Cases\SF v Downey\Gonzalez\DVD Video Recording_Title_01_01.mpg";
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.settings.autoStart = false;

            axWindowsMediaPlayer2.URL = @"C:\Cases\SF v Downey\Gonzalez\DVD Video Recording_Title_01_01.mpg";
            axWindowsMediaPlayer2.Ctlcontrols.stop();
            axWindowsMediaPlayer2.settings.autoStart = false;

            //Resize
            Form1_ResizeEnd(null, null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

            //axWindowsMediaPlayer1.openPlayer(axWindowsMediaPlayer1.URL);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            tabControl1.Size = new Size(this.ClientRectangle.Width, this.ClientRectangle.Height);
            axWindowsMediaPlayer1.Width = (int)(this.tabControl1.SelectedTab.Width / 2);
            axWindowsMediaPlayer1.Height = (int)(7*this.tabControl1.SelectedTab.Height / 8);
            axWindowsMediaPlayer1.Location = new Point(0, 0);

            axWindowsMediaPlayer2.Location = new Point(this.tabControl1.SelectedTab.Width / 2, 0);
            axWindowsMediaPlayer2.Height = (int)(7*this.tabControl1.SelectedTab.Height / 8);
            axWindowsMediaPlayer2.Width = (int)(this.tabControl1.SelectedTab.Width / 2);

            loadButton.Location = new Point(0, this.tabControl1.SelectedTab.Height - this.tabControl1.SelectedTab.Height / 8);
            loadButton.Width = this.tabControl1.SelectedTab.Width / 2;
            loadButton.Height = this.tabControl1.SelectedTab.Height /8;

            createClipButton.Location = new Point(this.tabControl1.SelectedTab.Width / 2, this.tabControl1.SelectedTab.Height - this.tabControl1.SelectedTab.Height / 8);
            createClipButton.Width = this.tabControl1.SelectedTab.Width / 2;
            createClipButton.Height = this.tabControl1.SelectedTab.Height / 8;
        }


        private void attachWindowHook()
        {
            if (hHook == 0)
            {
                // Create an instance of HookProc.
                MouseHookProcedure = new HookProc(Form1.MouseHookProc);

                hHook = SetWindowsHookEx(WH_MOUSE,
                MouseHookProcedure,
                (IntPtr)0,
                AppDomain.GetCurrentThreadId());
                //If the SetWindowsHookEx function fails.
                if (hHook == 0)
                {
                    MessageBox.Show("SetWindowsHookEx Failed");
                    return;
                }
            }
            else
            {
                bool ret = UnhookWindowsHookEx(hHook);
                //If the UnhookWindowsHookEx function fails.
                if (ret == false)
                {
                    MessageBox.Show("UnhookWindowsHookEx Failed");
                    return;
                }
                hHook = 0;
                this.Text = "Mouse Hook";
            }
        }

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Declare the hook handle as an int.
        static int hHook = 0;

        //Declare the mouse hook constant.
        //For other hook types, you can obtain these values from Winuser.h in the Microsoft SDK.
        public const int WH_MOUSE = 7;

        //Declare MouseHookProcedure as a HookProc type.
        HookProc MouseHookProcedure;

        //Declare the wrapper managed POINT class.
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        //Declare the wrapper managed MouseHookStruct class.
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //This is the Import for the SetWindowsHookEx function.
        //Use this function to install a thread-specific hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
        IntPtr hInstance, int threadId);

        //This is the Import for the UnhookWindowsHookEx function.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //This is the Import for the CallNextHookEx function.
        //Use this function to pass the hook information to the next hook procedure in chain.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode,
        IntPtr wParam, IntPtr lParam);

        public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //Marshall the data from the callback.
            MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

            if (nCode < 0)
            {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                //Create a string variable that shows the current mouse coordinates.
                String strCaption = "x = " +
                MyMouseHookStruct.pt.x.ToString("d") +
                "  y = " +
                MyMouseHookStruct.pt.y.ToString("d");
                //You must get the active form because it is a static function.
                //Form tempForm = Form.ActiveForm;

                //Set the caption of the form.
                //tempForm.Text = strCaption;
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();

            if (ofd.CheckFileExists)
            {
                axWindowsMediaPlayer1.URL = ofd.FileName;
                axWindowsMediaPlayer2.URL = ofd.FileName;

            }
        }

        private void createClipButton_Click(object sender, EventArgs e)
        {
            String startTime = axWindowsMediaPlayer1.Ctlcontrols.currentPosition.ToString();
            String endTime = axWindowsMediaPlayer2.Ctlcontrols.currentPosition.ToString();
            

            String runTime = (axWindowsMediaPlayer2.Ctlcontrols.currentPosition - axWindowsMediaPlayer1.Ctlcontrols.currentPosition).ToString();
            String inputFile = axWindowsMediaPlayer1.URL;
            String folder = "output";

            TimeSpan startTimespan = TimeSpan.FromSeconds(Convert.ToDouble(startTime));
            TimeSpan runTimespan = TimeSpan.FromSeconds(Convert.ToDouble(runTime));

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!(startTimespan.TotalSeconds > Convert.ToDouble(endTime)))
            {
                //MPEG 4 ARGUMENTS: -f lavfi -i aevalsrc=0 -shortest -c:v libxvid -qscale:v 2 -c:a libmp3lame
                //H264 BASELINE: -f lavfi -i aevalsrc=0 -shortest -c:v libx264 -profile:v baseline -crf 23 -c:a aac -strict experimental
                ProcessStartInfo startInfo = new ProcessStartInfo("ffmpeg", "-y -i \"" + inputFile + "\" -f lavfi -i aevalsrc=0 -shortest -c:v libx264 -profile:v baseline -crf 23 -c:a aac -strict experimental -ss " + startTimespan.ToString().Substring(0, startTimespan.ToString().Length - 4) + " -to " + TimeSpan.FromSeconds(Convert.ToDouble(endTime)) + " " + "\"" + axWindowsMediaPlayer1.URL.ToString().Split('\\')[axWindowsMediaPlayer1.URL.Split('\\').Length - 1].Split('.')[0] + "_" + startTime.ToString().Replace('.', '-') + "_" + endTime.ToString().Replace('.', '-') + ".mp4" + "\"");
                //startInfo.Arguments = "/K ffmpeg -i \"" + inputFile + "\" -c:v libx264 -preset medium -ss " + startTimespan.ToString().Substring(0, startTimespan.ToString().Length - 4) + " -to " + TimeSpan.FromSeconds(Convert.ToDouble(endTime)) + " " + "\"" + axWindowsMediaPlayer1.URL.ToString().Split('\\')[axWindowsMediaPlayer1.URL.Split('\\').Length - 1].Split('.')[0] + "_" + startTime.ToString().Replace('.', '-') + "_" + endTime.ToString().Replace('.', '-') + ".mp4" + "\"";
                //startInfo.FileName = "cmd.exe";
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;

                using (Process proc = Process.Start(startInfo))
                {
                    
                    while (!proc.StandardOutput.EndOfStream)
                    {
                        string line = proc.StandardOutput.ReadLine();
                        Console.WriteLine(line);
                    }
                }
                //string command = "ffmpeg -i \"" + inputFile + "\" -c:v libx264 -preset medium -ss " + startTimespan.ToString().Substring(0, startTimespan.ToString().Length - 4) + " -to " + TimeSpan.FromSeconds(Convert.ToDouble(endTime)) + " " + "\"" + axWindowsMediaPlayer1.URL.ToString().Split('\\')[axWindowsMediaPlayer1.URL.Split('\\').Length - 1].Split('.')[0] + "_" + startTime.ToString().Replace('.', '-') + "_" + endTime.ToString().Replace('.', '-') + ".mp4" + "\"";
                //Process.Start("CMD.exe", "/K echo " + command + "&" + command);
            }
            else
            {
                MessageBox.Show("Clip start time comes before clip end time");
            }
        }
    }
}

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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using NAudio;
using NAudio.Wave;
//using System.Windows;
//using System.Windows.Controls;
using System.Windows.Media.Imaging;
//using System.Windows.Media;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace ClipVideo
{


    public partial class Form1 : Form
    {
        #if DEBUG
        Form debugForm = new Form();
        System.Windows.Forms.TextBox debugTextBox = new System.Windows.Forms.TextBox();
        #endif

        public Form1()
        {
            InitializeComponent();

            //INITIAL ITEMS
            panel1.BackColor = System.Drawing.Color.Black;

            //DEBUG BOX
            #if DEBUG
                debugTextBox.Multiline = true;
                debugTextBox.Width = debugForm.Width;
                debugTextBox.Height = debugForm.Height;
                debugForm.Controls.Add(debugTextBox);
                debugForm.Show();
            #endif

            IntPtr window = this.Handle;

            //Initial Video Setup
            axWindowsMediaPlayer1.URL = @"C:\Cases\SF v Downey\Gonzalez\DVD Video Recording_Title_01_01.mpg";
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.settings.autoStart = false;


            axWindowsMediaPlayer2.URL = @"C:\Cases\SF v Downey\Gonzalez\DVD Video Recording_Title_01_01.mpg";
            axWindowsMediaPlayer2.Ctlcontrols.stop();
            axWindowsMediaPlayer2.settings.autoStart = false;

            //Resize
            Form1_ResizeEnd(null,null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            int rows = 16;

            //RESIZE TAB CONTROL
            tabControl1.Size = new System.Drawing.Size(this.ClientRectangle.Width-5, this.ClientRectangle.Height);

            //VIDEO SIZES
            axWindowsMediaPlayer1.Width = (int)(tabControl1.SelectedTab.Width / 2);
            axWindowsMediaPlayer1.Height = (int)(11*tabControl1.SelectedTab.Height / rows);
            axWindowsMediaPlayer1.Location = tabControl1.SelectedTab.ClientRectangle.Location;
            axWindowsMediaPlayer2.Location = new System.Drawing.Point(tabControl1.SelectedTab.Width / 2, 0);
            axWindowsMediaPlayer2.Height = (int)(11*tabControl1.SelectedTab.Height / rows);
            axWindowsMediaPlayer2.Width = (int)(tabControl1.SelectedTab.Width / 2);

            //PANEL RESIZE
            panel1.Location = new System.Drawing.Point(tabControl1.SelectedTab.ClientRectangle.Location.X, tabControl1.SelectedTab.Height - 5 * tabControl1.SelectedTab.Height / rows);
            panel1.Width = tabControl1.SelectedTab.Width / 2;
            panel1.Height = 2 * tabControl1.SelectedTab.Height / rows;
            panel2.Location = new System.Drawing.Point(tabControl1.SelectedTab.Width / 2, tabControl1.SelectedTab.Height - 5 * tabControl1.SelectedTab.Height / rows);
            panel2.Width = tabControl1.SelectedTab.Width / 2;
            panel2.Height = 2 * tabControl1.SelectedTab.Height / rows;

            //PANEL HANDLERS
            //panel1.Paint += panel_Paint;

            //AUDIO FORM BOXES (TEMP PLACEHOLDERS)
            pictureBox1.Width = tabControl1.SelectedTab.Width / 2;
            pictureBox1.Height = 2*tabControl1.SelectedTab.Height / rows;
            pictureBox1.Location = new System.Drawing.Point(pictureBox1.Width/2 + (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition, 0);
            pictureBox2.Location = new System.Drawing.Point(0 + (int)axWindowsMediaPlayer2.Ctlcontrols.currentPosition, 0);
            pictureBox2.Width = tabControl1.SelectedTab.Width / 2;
            pictureBox2.Height = 2 * tabControl1.SelectedTab.Height / rows;
            pictureBox2.Location = new System.Drawing.Point(pictureBox2.Width/2 + (int)axWindowsMediaPlayer2.Ctlcontrols.currentPosition, 0);

            //BUTTON RESIZE
            loadButton.Location = new System.Drawing.Point(0, this.tabControl1.SelectedTab.Height - 3*this.tabControl1.SelectedTab.ClientRectangle.Height / rows);
            loadButton.Width = this.tabControl1.SelectedTab.Width / 2;
            loadButton.Height = 2*this.tabControl1.SelectedTab.Height /rows;
            createClipButton.Location = new System.Drawing.Point(this.tabControl1.SelectedTab.Width / 2, this.tabControl1.SelectedTab.Height - 3*this.tabControl1.SelectedTab.Height / rows);
            createClipButton.Width = this.tabControl1.SelectedTab.Width / 2;
            createClipButton.Height = 2*this.tabControl1.SelectedTab.Height / rows;

            //Progress Bar
            progressBar1.Location = new System.Drawing.Point(tabControl1.SelectedTab.ClientRectangle.Location.X, this.tabControl1.SelectedTab.Height - this.tabControl1.SelectedTab.Height / rows);
            progressBar1.Width = tabControl1.SelectedTab.Width;
            progressBar1.Height = tabControl1.SelectedTab.Height;

            //tabControl1.Refresh();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void graphFile(string file)
        {
            var canvas = new System.Windows.Controls.Canvas();

            //WaveFormat target = new WaveFormat(8000, 8, 1);
            //WaveStream stream = new WaveFileReader("c:\\test.wav");
            //WaveFormatConversionStream str = new WaveFormatConversionStream(target, stream);
            //WaveFileWriter.CreateWaveFile("c:\\converted.wav", str);

            using (var reader = new AudioFileReader(@file))
            {
                var samples = (reader.Length) / (reader.WaveFormat.Channels * reader.WaveFormat.BitsPerSample / 8);
                var f = 0.0f;
                var max = 0.0f;
                // waveform will be a maximum of 4000 pixels wide:
                var waveFormWidth = pictureBox1.Width;
                var batch = (int)Math.Max(40, samples / waveFormWidth) / 4;
                var mid = pictureBox1.Height/2;
                var yScale = pictureBox1.Height;
                //var yScale = pictureBox1.Height/2;
                float[] buffer = new float[batch];
                int read;
                var xPos = 0;

                Boolean insideFile = false;
                int[] startEndPositions = { 0, 0 };
                while ((read = reader.Read(buffer, 0, batch)) == batch)
                {
                    for (int n = 0; n < read; n++)
                    {
                        max = Math.Max(Math.Abs(buffer[n]), max);
                    }

                    var line = new System.Windows.Shapes.Line();
                    line.X1 = xPos;
                    line.X2 = xPos;
                    line.Y1 = mid + (max * yScale);
                    line.Y2 = mid - (max * yScale);
                    line.StrokeThickness = 1;

                    if (line.Y1 < 100)
                    {
                        line.Stroke = System.Windows.Media.Brushes.Green;

                        if (insideFile == true)
                        {
                            insideFile = false;
                            startEndPositions[1] = (int)reader.Position;
                        }
                    }
                    else
                    {
                        line.Stroke = System.Windows.Media.Brushes.Green;

                        if (insideFile == false)
                        {
                            startEndPositions[0] = (int)reader.Position;
                            insideFile = true;
                        }
                    }
                    canvas.Children.Add(line);
                    max = 0;
                    xPos++;
                }
            }
            /*
            var scrollViewer = new ScrollViewer();
            scrollViewer.Content = canvas;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;*/

            CreateSaveBitmap(canvas, "test");
        }

        private void CreateSaveBitmap(System.Windows.Controls.Canvas canvas, string filename)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
             (int)pictureBox1.Width, (int)pictureBox1.Height,
             96d, 96d, System.Windows.Media.PixelFormats.Pbgra32);
            // needed otherwise the image output is black
            canvas.Measure(new System.Windows.Size((int)pictureBox1.Width, (int)pictureBox1.Height));
            canvas.Arrange(new System.Windows.Rect(new System.Windows.Size((int)pictureBox1.Width, (int)pictureBox1.Height)));

            renderBitmap.Render(canvas);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            Bitmap bitmap = BitmapFromSource(renderBitmap);
            pictureBox1.Refresh();
            pictureBox1.Image = bitmap;
            pictureBox2.Image = bitmap;

            using (FileStream file = File.Create(filename))
            {
                encoder.Save(file);
            }
        }

        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();

                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.ShowDialog();

            if (ofd.CheckFileExists)
            {

                //MEDIA PLAYER 1 LOAD FILE SET HANDLERS
                axWindowsMediaPlayer1.URL = ofd.FileName;
                axWindowsMediaPlayer1.PlayStateChange += (sender2,e2)=>AxWindowsMediaPlayer_PlayStateChange(sender2,e2,axWindowsMediaPlayer1,pictureBox1);

                //MEDIA PLAYER 2 LOAD FILE SET HANDLERS
                axWindowsMediaPlayer2.URL = ofd.FileName;
                axWindowsMediaPlayer2.PlayStateChange += (sender3, e3) => AxWindowsMediaPlayer_PlayStateChange(sender3, e3, axWindowsMediaPlayer2,pictureBox2);

                graphFile(ofd.FileName);

            }
        }

        private void AxWindowsMediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e, AxWMPLib.AxWindowsMediaPlayer player, PictureBox pictureBox)
        {
            //TIMER SETUP
            System.Windows.Forms.Timer playTimer = new System.Windows.Forms.Timer();
            playTimer.Tick += (sender2, e2) => PlayTimer_Tick(sender2, e2, player, pictureBox);
            playTimer.Interval = 1;

            //PLAYING
            if (e.newState == 3)
            {
                playTimer.Start();
            }
            //PAUSED
            if(e.newState ==2)
            {
                playTimer.Stop();
                playTimer.Dispose();
            }
            //VIDEO INITIALIZED
            //STOPPED
            if (e.newState == 1)
            {
                playTimer.Stop();
                playTimer.Dispose();
            }
            //VIDEO INITIALIZED
            if (e.newState==9)
            {

            }

            throw new NotImplementedException();
        }

        private void PlayTimer_Tick(object sender, EventArgs e, AxWMPLib.AxWindowsMediaPlayer player,PictureBox pictureBox)
        {
            double xPosition = -1*(double)pictureBox.Image.Width*(player.Ctlcontrols.currentPosition / player.currentMedia.duration);

            pictureBox.Location = new Point(pictureBox.Width/2+(int)xPosition,0);
            pictureBox.Refresh();

            #if DEBUG
            debugTextBox.Text = player.Ctlcontrols.currentPosition +System.Environment.NewLine + debugTextBox.Text;
            #endif
        }

        private void createClipButton_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                CreateClips();
            }).Start();
        }

        private void CreateClips()
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

                //* Set your output and error (asynchronous) handlers
                Process process = new Process();
                process.StartInfo =  new ProcessStartInfo("ffmpeg", "-y -i \"" + inputFile + "\" -f lavfi -i aevalsrc=0 -shortest -c:v libx264 -profile:v baseline -crf 23 -c:a aac -strict experimental -ss " + startTimespan.ToString().Substring(0, startTimespan.ToString().Length - 4) + " -to " + TimeSpan.FromSeconds(Convert.ToDouble(endTime)) + " " + "\"" + axWindowsMediaPlayer1.URL.ToString().Split('\\')[axWindowsMediaPlayer1.URL.Split('\\').Length - 1].Split('.')[0] + "_" + startTime.ToString().Replace('.', '-') + "_" + endTime.ToString().Replace('.', '-') + ".mp4" + "\"");
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.OutputDataReceived += new DataReceivedEventHandler((sender, e) => OutputHandler(sender, e, runTimespan));
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => OutputHandler(sender, e, runTimespan));
                //* Start process and handlers
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                //string command = "ffmpeg -i \"" + inputFile + "\" -c:v libx264 -preset medium -ss " + startTimespan.ToString().Substring(0, startTimespan.ToString().Length - 4) + " -to " + TimeSpan.FromSeconds(Convert.ToDouble(endTime)) + " " + "\"" + axWindowsMediaPlayer1.URL.ToString().Split('\\')[axWindowsMediaPlayer1.URL.Split('\\').Length - 1].Split('.')[0] + "_" + startTime.ToString().Replace('.', '-') + "_" + endTime.ToString().Replace('.', '-') + ".mp4" + "\"";
                //Process.Start("CMD.exe", "/K echo " + command + "&" + command);
            }
            else
            {
                System.Windows.MessageBox.Show("Clip start time comes before clip end time");
            }
        }

        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine, TimeSpan runTime)
        {
            string outputLine = outLine.Data;
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Console.WriteLine(outputLine);
            if(outputLine!=null)
            if(outputLine.Contains("time=")==true)
            {
                TimeSpan currentTimeSpan = TimeSpan.Parse(outputLine.Split(new string[] { "time=" }, StringSplitOptions.None)[1].Split(' ')[0]);
                int progressStep = (int)(100*currentTimeSpan.TotalSeconds / runTime.TotalSeconds);

                MethodInvoker mi = new MethodInvoker(() => UpdateProgress(progressStep));
                if (progressBar1.InvokeRequired)
                {
                    progressBar1.Invoke(mi);
                }   
                else
                {
                    mi.Invoke();
                }
            }
        }

        public void UpdateProgress(int progressStep)
        {
            
            progressBar1.Step = progressStep- progressBar1.Value;
            progressBar1.PerformStep();
        }

        public static void DrawNormalizedAudio(ref float[] data, PictureBox pb,
    System.Drawing.Color color)
        {
            Bitmap bmp;
            if (pb.Image == null)
            {
                bmp = new Bitmap(pb.Width, pb.Height);
            }
            else
            {
                bmp = (Bitmap)pb.Image;
            }

            int BORDER_WIDTH = 5;
            int width = bmp.Width - (2 * BORDER_WIDTH);
            int height = bmp.Height - (2 * BORDER_WIDTH);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(System.Drawing.Color.Black);
                System.Drawing.Pen pen = new System.Drawing.Pen(color);
                int size = data.Length;
                for (int iPixel = 0; iPixel < width; iPixel++)
                {
                    // determine start and end points within WAV
                    int start = (int)((float)iPixel * ((float)size / (float)width));
                    int end = (int)((float)(iPixel + 1) * ((float)size / (float)width));
                    float min = float.MaxValue;
                    float max = float.MinValue;
                    for (int i = start; i < end; i++)
                    {
                        float val = data[i];
                        min = val < min ? val : min;
                        max = val > max ? val : max;
                    }
                    int yMax = BORDER_WIDTH + height - (int)((max + 1) * .5 * height);
                    int yMin = BORDER_WIDTH + height - (int)((min + 1) * .5 * height);
                    g.DrawLine(pen, iPixel + BORDER_WIDTH, yMax,
                        iPixel + BORDER_WIDTH, yMin);
                }
            }
            pb.Image = bmp;
        }

        private static TimeSpan GetVideoDuration(string filePath)
        {
            using (var shell = ShellObject.FromParsingName(filePath))
            {
                IShellProperty prop = shell.Properties.System.Media.Duration;
                var t = (ulong)prop.ValueAsObject;
                return TimeSpan.FromTicks((long)t);
            }
        }

        private System.Drawing.Image getSubImage(System.Drawing.Image image)
        {
            Rectangle subImageRect = new Rectangle((int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition, 0, (int)(float)pictureBox1.Width / 2, pictureBox1.Height);
            try
            {
                Bitmap croppedImage = (new Bitmap((System.Drawing.Image)image.Clone()).Clone(subImageRect, image.PixelFormat));
                return croppedImage;
            }
            catch
            {

            }

            return image;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            var g = e.Graphics;

            g.DrawLine((new Pen(Color.Red)), p.Width / 2, 0, p.Width / 2, p.Height);
        }
    }
}
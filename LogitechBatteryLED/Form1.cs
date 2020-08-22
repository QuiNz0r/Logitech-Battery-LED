using System;
using System.ComponentModel;
using System.Windows.Forms;
using Memory;
using LedCSharp;
using System.Threading;

namespace LogitechBatteryLED
{
    public partial class Form1 : Form
    {
        private Mem _memory = new Mem();
        private BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private int _checkIntervalms = 20000;
        private bool _alphaMode = true;
        private bool _logiSDKInit = false;
        private float _batteryCharge = 0;

        public Form1()
        {
            InitializeComponent();

            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            _backgroundWorker.RunWorkerAsync();
        }

        void InitializeLogiSDK()
        {
            _logiSDKInit = LogitechGSDK.LogiLedInitWithName("Battery LED");

            if (_logiSDKInit) Console.WriteLine("LED SDK Initialized");
            LogitechGSDK.LogiLedSetTargetDevice(LogitechGSDK.LOGI_DEVICETYPE_RGB);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (true)
            {
                if (!_logiSDKInit)
                {
                    LogitechGSDK.LogiLedShutdown();
                    int pid = _memory.GetProcIdFromName("lghub_agent");
                    if (pid <= 0)
                    {
                        Console.WriteLine("looking for lghub...");
                        Thread.Sleep(5000);
                        continue;
                    }
                    else
                    {
                        InitializeLogiSDK();
                    }
                }



                Console.WriteLine("reading memory...");
                ReadBatteryStatus();
                SetColor(_batteryCharge);
                Thread.Sleep(_checkIntervalms);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogitechGSDK.LogiLedShutdown();
        }

        void SetColor(float batteryPercentage)
        {
            float Alpha = (batteryPercentage + 5) / 100; // 5 = offset alpha

            float Rlower = 100;
            float Glower = batteryPercentage * 2;

            float Rupper = (100 - batteryPercentage) * 2;
            float Gupper = 100;

            if (_alphaMode)
            {
                Rlower *= Alpha;
                Glower *= Alpha;
                Rupper *= Alpha;
                Gupper *= Alpha;
            }

            if (batteryPercentage > 0)
            {
                //if (batteryPercentage <= 10)
                //{
                //    LogitechGSDK.LogiLedPulseLighting(100, (int)batteryPercentage, 0, _checkIntervalms, 500);
                //}
                if (batteryPercentage <= 50)
                {
                    LogitechGSDK.LogiLedSetLightingForTargetZone(DeviceType.Mouse, 1, (int)Rlower, (int)Glower, 0);
                    Console.WriteLine("RGB: " + (int)batteryPercentage + "% " + (int)Rlower + " " + (int)Glower + " 0");
                }
                else if (batteryPercentage > 50)
                {
                    LogitechGSDK.LogiLedSetLightingForTargetZone(DeviceType.Mouse, 1, (int)Rupper, (int)Gupper, 0);
                    Console.WriteLine("RGB: " + (int)batteryPercentage + "% " + (int)Rupper + " " + (int)Gupper + " 0");
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SetColor(trackBar1.Value);
        }

        private void ReadBatteryStatus()
        {
            int pid = _memory.GetProcIdFromName("lghub_agent");
            bool openProc = false;
            if (pid > 0) openProc = _memory.OpenProcess(pid);

            if (openProc)
            {
                _batteryCharge = _memory.ReadFloat("lghub_agent.exe+0x015C5048,0x10,0x0,0x30,0x58,0x2E8,0x0,0x1B8,0x28,0x170,0x0,0x450,0x30,0x30,0x90,0x348");
                notifyIcon1.Text = "Logitech Battery LED " + (int)_batteryCharge + "%";
            }
            else
            {
                _logiSDKInit = false;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            notifyIcon1.Visible = true;
            Hide();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void AlphaMode_Click(object sender, EventArgs e)
        {
            _alphaMode = !_alphaMode;
            AlphaMode.Checked = _alphaMode;
            AlphaMode.Text = "AlphaMode: " + (_alphaMode ? "ON" : "OFF");
        }

    }
}

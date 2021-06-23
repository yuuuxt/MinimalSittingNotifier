using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;

namespace MinimalSittingNotifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly int maxSitMinute = 30;
        private int timeCountNumber;

        private readonly Font defaultIconFont = new Font("Calibri", 8);
        private readonly Color defaultIconFontColor = Color.White;
        private readonly Color defaultIconStopColor = Color.Yellow;
        private readonly int IconSize = 16;

        private System.Timers.Timer _timer;
        private int h, m, s;
        private readonly Forms.NotifyIcon _notifyIcon;

        public App()
        {
            _notifyIcon = new Forms.NotifyIcon();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ShowNumberIcon(maxSitMinute.ToString());
            _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, ExitProgram);
            _notifyIcon.MouseClick += ReStartTimer;
            _notifyIcon.Visible = true;
            StartTimer();
            
            base.OnStartup(e);
        }

        private void StartTimer()
        {   
            timeCountNumber = maxSitMinute;

            _timer = new System.Timers.Timer(1000 * 60); //1 sec * 60 = 1 min interval;
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
            ShowNumberIcon(timeCountNumber.ToString());

        }

        private void ReStartTimer(object? sender, Forms.MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
            {
                _timer.Dispose();
                StartTimer();
            }

        }

        private void OnTimedEvent(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (timeCountNumber <= 1)
            {
                _timer.Enabled = false;
                ShowStopIcon();
            }
            else
            {
                timeCountNumber--;
                ShowNumberIcon(timeCountNumber.ToString());
            }
        }

        private void ExitProgram(object? sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            Environment.Exit(0);
            
        }


        private void ShowNumberIcon(String numberText)
        {
            Brush brush = new SolidBrush(defaultIconFontColor);
            Bitmap bitmap = new Bitmap(IconSize, IconSize);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawString(numberText, defaultIconFont, brush, 0, 0);

            Icon icon = Icon.FromHandle(bitmap.GetHicon());
            _notifyIcon.Icon = icon;
        }


        private void ShowStopIcon()
        {
            Brush brush = new SolidBrush(defaultIconStopColor);
            Bitmap bitmap = new Bitmap(IconSize, IconSize);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(brush, 0, 0, IconSize, IconSize);

            Icon icon = Icon.FromHandle(bitmap.GetHicon());
            _notifyIcon.Icon = icon;
        }


        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
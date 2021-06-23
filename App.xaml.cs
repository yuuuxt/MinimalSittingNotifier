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
        private readonly int maxSitTime = 5;
        private int timeCountNumber;

        private readonly Font defaultIconFont = new Font("Calibri", 8);
        private readonly Color defaultIconFontColor = Color.White;
        private readonly Color defaultIconStopColor = Color.Yellow;
        private readonly int IconSize = 16;

        private System.Timers.Timer _timer = new System.Timers.Timer(1000); //1 sec * 60 = 1 min interval;
        private int h, m, s;
        private readonly Forms.NotifyIcon _notifyIcon;

        public App()
        {
            _notifyIcon = new Forms.NotifyIcon();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ShowNumberIcon(maxSitTime.ToString());
            _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", Image.FromFile("Resources/icon.ico"), ExitProgram);
            _notifyIcon.Click += GoToStart;
            _notifyIcon.Visible = true;
            
            base.OnStartup(e);
        }

        private void GoToStart(object? sender, EventArgs e)
        {
            timeCountNumber = maxSitTime;
            _timer.Elapsed += OnTimedEvent;
            
            _timer.Enabled = true;

            ShowNumberIcon(timeCountNumber.ToString());
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
            
        }


        private void ShowNumberIcon(String numberText)
        {
            Brush brush = new SolidBrush(defaultIconFontColor);
            Bitmap bitmap = new Bitmap(IconSize, IconSize);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawString(numberText, defaultIconFont, brush, 0, 0);

            Icon icon = Icon.FromHandle(bitmap.GetHicon());
            _notifyIcon.Icon = icon;

            // for debugging
            _notifyIcon.Text = numberText;
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
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
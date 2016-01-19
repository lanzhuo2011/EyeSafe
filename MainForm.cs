using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EyeSafe
{
    public enum RunningState {
        Normal = 0,
        Sleep = 1
    }

    public partial class MainForm : Form
    {
        private int curCount = 0;
        private int preCount = 0;

        private int sleepValue;
        private int spanTimeValue;
        private RunningState state = RunningState.Normal;

        public MainForm()
        {
            InitializeComponent();
            Reset();
            WindowState = FormWindowState.Minimized;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            preCount = curCount = 0;
            state = RunningState.Normal;
            ExitSleep();
        }

        private void EnterSleep()
        {
            this.Show();
            WindowState = FormWindowState.Maximized;
        }
        private void ExitSleep()
        {
            this.Hide();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 按 任意键 退出
            ExitSleep();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (RunningState.Normal == state)
            {
                if (curCount >= preCount + spanTimeValue)
                {
                    // 进入 sleep 模式
                    state = RunningState.Sleep;
                    curCount = preCount = 0;
                    EnterSleep();
                }
                if ((curCount - preCount) == (spanTimeValue - 10))   // 提前 10s 提示进入sleep模式
                {
                    notifyIcon.BalloonTipText = "10s 后进入sleep";
                    notifyIcon.ShowBalloonTip(2);
                }
            }
            else
            {
                if (curCount >= preCount + sleepValue)
                {
                    // 退出 sleep 模式
                    state = RunningState.Normal;
                    curCount = preCount = 0;
                    ExitSleep();
                }
                // 在 sleep 模式
                Countdown(preCount + sleepValue - curCount);

            }
            curCount++;
        }

        private void Countdown(int count)
        {
            int mins = count / 60;
            int secs = count % 60;
            lblTime.Text = "" + mins + " : " + secs;
        }

        private void SettingMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            int spanH = spanTimeValue / 60 / 60;
            int spanM = spanTimeValue / 60;
            int sleepM = sleepValue / 60;
            SettingDlg dlg = new SettingDlg(spanH, spanM, sleepM);
            dlg.ShowDialog();

            if (dlg.IsModified)
            {
                spanH = dlg.SpanHours;
                spanM = dlg.SpanMins;
                sleepM = dlg.SleepMins;
                spanTimeValue = spanH * 60 * 60 + spanM * 60;
                sleepValue = sleepM * 60;  
                SaveConfig();
                Reset();
            }
            timer.Start();
        }

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigWrite(String key, String value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");     //重新加载新的配置文件 
        }
        private String ConfigRead(String key)
        {
            String res = ConfigurationManager.AppSettings[key];
            return res;
        }
        private void LoadConfig()
        {
            sleepValue = Convert.ToInt32(ConfigRead("Sleep"));
            spanTimeValue = Convert.ToInt32(ConfigRead("SpanTime"));
        }
        private void SaveConfig()
        {
            ConfigWrite("Sleep", "" + sleepValue);
            ConfigWrite("SpanTime", "" + spanTimeValue);
        }

        private void Reset()
        {
            LoadConfig();
            state = RunningState.Normal;
            preCount = curCount = 0;
            timer.Stop();
            timer.Start();
        }

        private void AboutDlgMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }

        private void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            int spanH = (spanTimeValue - (curCount - preCount)) / 60 / 60;
            int spanM = (spanTimeValue - (curCount - preCount)) / 60;
            int spanS = (spanTimeValue - (curCount - preCount)) % 60;
            notifyIcon.Text = "还有" + spanH + "小时 " + spanM + "分钟 " + spanS + "秒 休息 ^_^ ";
        }

    }
}

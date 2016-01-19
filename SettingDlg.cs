using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EyeSafe
{
    public partial class SettingDlg : Form
    {
        private int spanHours;
        private int spanMins;
        private int sleepMins;

        private bool isModified = false;

        public SettingDlg(int spanH, int spanM, int sleepM)
        {
            InitializeComponent();
            isModified = false;
            spanHours = spanH;
            spanMins = spanM;
            sleepMins = sleepM;
            LaodValue();
        }

        public int SpanHours
        {
            get { return spanHours; }
            set { spanHours = value; }
        }
        public int SpanMins
        {
            get { return spanMins; }
            set { spanMins = value; }
        }
        public int SleepMins
        {
            get { return sleepMins; }
            set { sleepMins = value; }
        }
        public bool IsModified
        {
            get { return isModified; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (sleepMins != Convert.ToInt32(tbSleepTime.Text.Trim()))
            {
                sleepMins = Convert.ToInt32(tbSleepTime.Text.Trim());
                isModified = true;
            }
            if (spanHours != Convert.ToInt32(tbSpanHours.Text.Trim()))
            {
                spanHours = Convert.ToInt32(tbSpanHours.Text.Trim());
                isModified = true;
            }
            if (spanMins != Convert.ToInt32(tbSpanMins.Text.Trim()))
            {
                spanMins = Convert.ToInt32(tbSpanMins.Text.Trim());
                isModified = true;
            }
            this.Close();
        }

        private void LaodValue()
        {
            tbSleepTime.Text = "" + SleepMins;
            tbSpanHours.Text = "" + SpanHours;
            tbSpanMins.Text = "" + SpanMins;
        }
    }
}

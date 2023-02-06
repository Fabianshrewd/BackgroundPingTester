using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;

namespace BackgroundPingTester
{
    public partial class Form1 : Form
    {
        public long[] arr;
        public int i;

        public Form1()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arr = new long[10];
            for(int u = 0; u < arr.Length; u++)
            {
                arr[u] = 1;
            }
            i = 0;
            Thread t1 = new Thread(whilePingTest);
            t1.Start();
        }

        private void whilePingTest()
        {
            while(true)
            {
                PingTest();
                Thread.Sleep(1000);
            }
        }

        private void PingTest()
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send("google.at", 10000);

            try
            {
                textBox_overview.BeginInvoke((Action)delegate
                {
                    textBox_overview.Text += "Status :  " + reply.Status + " Time : " + reply.RoundtripTime.ToString() + " Address : " + reply.Address + "\n";
                });

                arr[i] = reply.RoundtripTime;
                i++;

                if(i > 9)
                {
                    Thread.Sleep(5000);

                    i = 0;
                    textBox_overview.BeginInvoke((Action)delegate
                    {
                        textBox_overview.Text = "";
                    });
                }

                long sum = 0;
                for(int u = 0; u < arr.Length; u++)
                {
                    sum += arr[u];
                }

                textBox_average.BeginInvoke((Action)delegate
                {
                    textBox_average.Text = "Avg: " + Convert.ToString(sum / arr.Length);
                });
                
            }
            catch(Exception e)
            {

            }
        }
    }
}

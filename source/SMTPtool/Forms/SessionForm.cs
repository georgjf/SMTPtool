using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Timers;

namespace SMTPtool
{
    public partial class Telnet : Form
    {

        private String serverIP;
        private int serverPort;
        private List<string> hisItems = new List<string>();
        private String commandToSend = "";
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        System.Timers.Timer connectionDelayTimer;
        System.Timers.Timer reconnectionDelayTimer;

        Thread ctThread;

        public Telnet()
        {
            InitializeComponent();
        }

        public void connect()
        {
            //dirty workaround to stop the main window from freezing during establishing the connection
            connectionDelayTimer = new System.Timers.Timer();
            connectionDelayTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            connectionDelayTimer.Interval = 100;
            connectionDelayTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            connectionDelayTimer.Stop();
            start();
        }

        public void start()
        {

            try
            {
                txtOutput.AppendText("Connecting to " + serverIP + " on port" + serverPort + "\r\n", Color.Red);
                clientSocket.Connect(serverIP, serverPort);
                ctThread = new System.Threading.Thread(new ThreadStart(Run));
                ctThread.IsBackground = true;
                ctThread.Name = "asdf";
                ctThread.Start();
                txtOutput.AppendText("Connected\r\n", Color.Red);

                this.Invoke((MethodInvoker)delegate()
                {
                    scrollDownOutput();
                });
            }
            catch (Exception)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    txtOutput.AppendText("Connection Error\r\n", Color.Red);
                    scrollDownOutput();
                });
            }
        }

        private void reconnect()
        {
            try
            {
                txtOutput.AppendText("Connecting to " + serverIP + " on port" + serverPort + "\r\n\r\n", Color.Red);
                clientSocket = new System.Net.Sockets.TcpClient();
                clientSocket.Connect(serverIP, serverPort);
                ctThread = new System.Threading.Thread(new ThreadStart(Run));
                ctThread.IsBackground = true;
                ctThread.Start();
                //txtOutput.AppendText("Connected to " + serverIP + " on port" + serverPort + "\r\n", Color.Red);

            }
            catch (Exception)
            {
                txtOutput.AppendText("Connection Error\r\n", Color.Red);
            }
        }

        private void OnReconnectTimedEvent(object source, ElapsedEventArgs e)
        {
            reconnectionDelayTimer.Stop();
            reconnect();
        }

        private void btnRecon_Click(object sender, EventArgs e)
        {

            reconnectionDelayTimer = new System.Timers.Timer();
            reconnectionDelayTimer.Elapsed += new ElapsedEventHandler(OnReconnectTimedEvent);
            reconnectionDelayTimer.Interval = 100;
            reconnectionDelayTimer.Enabled = true;
            reconnectionDelayTimer.Start();
        }

        public void Run()
        {
            String strMessage;

            while (true)
            {
                try
                {
                    strMessage = Read();
                    if (strMessage.Equals(""))
                    {
                        this.Invoke((MethodInvoker)delegate()
                        {
                            txtOutput.AppendText("Connection closed\r\n", Color.Red);
                            scrollDownOutput();
                            btnRecon.Enabled = true;
                            clientSocket.Close();
                            ctThread.Abort();
                        });
                        break;
                    }
                    this.Invoke((MethodInvoker)delegate()
                    {
                        txtOutput.AppendText("<< " + strMessage, Color.Blue);
                        scrollDownOutput();
                    });
                }
                catch (Exception exception)
                {
                    ctThread.Abort();
                    this.Invoke((MethodInvoker)delegate()
                    {
                        txtOutput.AppendText("\r\n Error - Connection closed\r\n", Color.Red);
                        scrollDownOutput();
                    });

                    clientSocket.Close();
                    btnSendLine.Enabled = false;
                    txtCommand.ReadOnly = true;
                    Debug.WriteLine("EEEEEEEEEEEEEEERRROR: RUN " + exception.Message);
                    break;
                }
            }
        }

        private void Write()
        {

            if (clientSocket.Connected)
            {
                scrollDownOutput();

                NetworkStream clientStream = clientSocket.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(commandToSend + "\r\n");

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

                txtOutput.AppendText(">> " + commandToSend + "\r\n", Color.Green);
                scrollDownOutput();
                commandToSend = "";
            }
        }

        private String Read()
        {
            try
            {
                byte[] messageBytes = new byte[8192];
                int bytesRead = 0;
                NetworkStream clientStream = clientSocket.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                bytesRead = clientStream.Read(messageBytes, 0, 8192);
                string strMessage = encoder.GetString(messageBytes, 0, bytesRead);
                return strMessage;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("EEEEEEEEEEEEEEERRROR: READ " + exception.Message);
                txtOutput.AppendText("\r\nError - Connection closed\r\n", Color.Red);
                scrollDownOutput();
                clientSocket.Close();
                return null;
            }
        }

        private void input_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Debug.WriteLine("RETURN PRESS");

                hisItems.Add(txtCommand.Text);
                lbxHistory.DataSource = null;
                lbxHistory.DataSource = hisItems;
                lbxHistory.SetSelected(hisItems.Count - 1, true);

                commandToSend = txtCommand.Text;
                txtCommand.Text = "";
                Write();
                scrollDownHist();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnSendLine_Click(object sender, EventArgs e)
        {
            hisItems.Add(txtCommand.Text);
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);
            commandToSend = txtCommand.Text;
            txtCommand.Text = "";
            Write();
            scrollDownHist();
        }

        private void formClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // ctThread.Abort();
                //  clientSocket.Close();
                this.BeginInvoke(new MethodInvoker(Close));

            }
            catch (Exception exception)
            {
                Debug.WriteLine("EEEEEEEEEEEEEEERRROR: " + exception.Message);
            }
        }

        internal void setIP(string p)
        {
            this.serverIP = p;
        }

        internal void setPort(int p)
        {
            this.serverPort = p;
        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            if (lbxHistory.SelectedIndex != -1)
            {
                commandToSend = lbxHistory.GetItemText(lbxHistory.SelectedItem);
                Write();

                if (lbxHistory.SelectedIndex != hisItems.Count - 1)
                {
                    lbxHistory.SetSelected(lbxHistory.SelectedIndex + 1, true);
                }
            }
        }

        private void btnClearCommand_Click(object sender, EventArgs e)
        {
            int selectedIndex = lbxHistory.SelectedIndex;
            if (lbxHistory.SelectedIndex != -1)
            {
                hisItems.RemoveAt(lbxHistory.SelectedIndex);
                lbxHistory.DataSource = null;
                lbxHistory.DataSource = hisItems;
                //Debug.WriteLine("Selected Index: " + selectedIndex);
                if (selectedIndex == 0)
                {
                    if (hisItems.Count > 0)
                    {
                        lbxHistory.SetSelected(0, true);
                    }
                    else
                    {
                        lbxHistory.ClearSelected();
                    }
                }
                else
                {
                    lbxHistory.SetSelected(selectedIndex - 1, true);
                }
            }
        }

        private void btnFrom_Click(object sender, EventArgs e)
        {
            hisItems.Add("mail from: " + cbxFrom.Text);
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "mail from:" + cbxFrom.Text;
            Write();
            scrollDownHist();
        }

        private void btnRcpt_Click(object sender, EventArgs e)
        {
            hisItems.Add("rcpt to:" + cbxTo.Text);
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "rcpt to:" + cbxTo.Text;
            Write();
            scrollDownHist();
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            hisItems.Add("data");
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "data";
            Write();
            scrollDownHist();
        }

        private void btnHelo_Click(object sender, EventArgs e)
        {
            hisItems.Add("helo");
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);
            commandToSend = "helo";
            Write();
            scrollDownHist();
        }

        private void scrollDownHist()
        {
            //scroll history list box to bottom
            int visibleItems = lbxHistory.ClientSize.Height / lbxHistory.ItemHeight;
            lbxHistory.TopIndex = Math.Max(lbxHistory.Items.Count - visibleItems + 1, 0);
        }

        private void scrollDownOutput()
        {
            // scroll output box to bottom
            txtOutput.SelectionStart = txtOutput.Text.Length;
            txtOutput.ScrollToCaret();
        }

        internal void setFrom(List<String> p)
        {
            this.cbxFrom.DataSource = p;
        }

        internal void setTo(List<String> p)
        {
            this.cbxTo.DataSource = p;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            hisItems.Add("quit");
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "quit";
            Write();
            scrollDownHist();
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            hisItems.Add(".");
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = ".";
            Write();
            scrollDownHist();
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            hisItems.Add("rset");
            lbxHistory.DataSource = null;
            lbxHistory.DataSource = hisItems;
            lbxHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "rset";
            Write();
            scrollDownHist();
        }



    }
}

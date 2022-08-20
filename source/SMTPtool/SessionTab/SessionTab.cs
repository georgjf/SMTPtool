using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMTPtool;
using System.Windows.Forms;
using System.Timers;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;

namespace SMTPtestTool
{
    public class SessionTab
    {
        Main _linkToMain;
        Telnet mySessionForm;
        Thread ctThread;

        public Boolean isConnected = false;

        private String serverIP = "";
        private int serverPort;

        private String commandToSend = "";

        private List<string> hisItems = new List<string>();

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        System.Timers.Timer connectionDelayTimer;
        System.Timers.Timer reconnectionDelayTimer;

        //constructor
        public SessionTab(Main _linkToMain)
        {
            this._linkToMain = _linkToMain;
            this._linkToMain.txtSessionCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_KeyDown);
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
           
            _linkToMain.Invoke((MethodInvoker)delegate()
            {
                if (_linkToMain.cbxSessionServer.Text.Equals(""))
                {
                    serverIP = "192.168.0.1";
                }
                else
                {
                    serverIP = _linkToMain.cbxSessionServer.Text;
                }
            });

            try
            {
                serverPort = int.Parse(_linkToMain.cbxSessionPort.Text);
            }
            catch (Exception)
            {
                serverPort = 25;
            }

            try
            {
                _linkToMain.txtSessionOutput.AppendText("Connecting to " + serverIP + " on port" + serverPort + "\r\n", Color.Red);
                clientSocket.Connect(serverIP, serverPort);
                ctThread = new System.Threading.Thread(new ThreadStart(Run));
                ctThread.IsBackground = true;
                ctThread.Name = "asdf";
                ctThread.Start();
                _linkToMain.Invoke((MethodInvoker)delegate()
                {
                    _linkToMain.txtSessionOutput.AppendText("Connected\r\n", Color.Red);
                    scrollDownOutput();
                });
            }
            catch (Exception)
            {
                _linkToMain.Invoke((MethodInvoker)delegate()
                {
                    _linkToMain.txtSessionOutput.AppendText("Connection Error\r\n", Color.Red);
                    scrollDownOutput();
                });
            }
        }

        public void reconnect()
        {
            reconnectionDelayTimer = new System.Timers.Timer();
            reconnectionDelayTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
            reconnectionDelayTimer.Interval = 100;
            reconnectionDelayTimer.Enabled = true;
            reconnectionDelayTimer.Start();
        }

        private void OnTimedEvent2(object source, ElapsedEventArgs e) {
            Debug.WriteLine("TIMER2 FIRED");
            reconnectionDelayTimer.Enabled = false;
            reconnectionDelayTimer.Stop();
            startReconnect();
        }

        private void startReconnect()
        {
            _linkToMain.Invoke((MethodInvoker)delegate()
            {
                if (_linkToMain.cbxSessionServer.Text.Equals(""))
                {
                    serverIP = "192.168.0.1";
                }
                else
                {
                    serverIP = _linkToMain.cbxSessionServer.Text;
                }
            });

            try
            {
                serverPort = int.Parse(_linkToMain.cbxSessionPort.Text);
            }
            catch (Exception)
            {
                serverPort = 25;
            }
            try
            {
                _linkToMain.Invoke((MethodInvoker)delegate()
                {
                    _linkToMain.txtSessionOutput.AppendText("Connecting to " + serverIP + " on port" + serverPort + "\r\n\r\n", Color.Red);
                });
                
                clientSocket = new System.Net.Sockets.TcpClient();
                clientSocket.Connect(serverIP, serverPort);
                ctThread = new System.Threading.Thread(new ThreadStart(Run));
                ctThread.IsBackground = true;
                ctThread.Start();
            }
            catch (Exception)
            {
                _linkToMain.txtSessionOutput.AppendText("Connection Error\r\n", Color.Red);
            }
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
                        _linkToMain.Invoke((MethodInvoker)delegate()
                        {
                            _linkToMain.txtSessionOutput.AppendText("Connection closed\r\n", Color.Red);
                            scrollDownOutput();
                            clientSocket.Close();
                            ctThread.Abort();
                        });
                        break;
                    }
                    _linkToMain.Invoke((MethodInvoker)delegate()
                    {
                        _linkToMain.txtSessionOutput.AppendText("<< " + strMessage, Color.Blue);
                        scrollDownOutput();
                    });
                }
                catch (Exception exception)
                {
                    ctThread.Abort();
                    _linkToMain.Invoke((MethodInvoker)delegate()
                    {
                        _linkToMain.txtSessionOutput.AppendText("\r\n Error - Connection closed\r\n", Color.Red);
                        scrollDownOutput();
                    });

                    clientSocket.Close();
                    _linkToMain.btnSessionSendLine.Enabled = false;
                    _linkToMain.txtSessionCommand.ReadOnly = true;
                    Debug.WriteLine("EEEEEEEEEEEEEEERRROR: RUN " + exception.Message);
                    break;
                }
            }
        }

        private void scrollDownHist()
        {
            //scroll history list box to bottom
            int visibleItems = _linkToMain.lbxSessionHistory.ClientSize.Height / _linkToMain.lbxSessionHistory.ItemHeight;
            _linkToMain.lbxSessionHistory.TopIndex = Math.Max(_linkToMain.lbxSessionHistory.Items.Count - visibleItems + 1, 0);
        }

        private void scrollDownOutput()
        {
            // scroll output box to bottom
            _linkToMain.txtSessionOutput.SelectionStart = _linkToMain.txtSessionOutput.Text.Length;
            _linkToMain.txtSessionOutput.ScrollToCaret();
        }

        private void input_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Debug.WriteLine("RETURN PRESS");

                hisItems.Add(_linkToMain.txtSessionCommand.Text);
                _linkToMain.lbxSessionHistory.DataSource = null;
                _linkToMain.lbxSessionHistory.DataSource = hisItems;
                _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);

                commandToSend = _linkToMain.txtSessionCommand.Text;
                _linkToMain.txtSessionCommand.Text = "";
                Write();
                scrollDownHist();

                e.Handled = true;
                e.SuppressKeyPress = true;
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
                _linkToMain.txtSessionOutput.AppendText("\r\nError - Connection closed\r\n", Color.Red);
                scrollDownOutput();
                clientSocket.Close();
                return null;
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

                _linkToMain.txtSessionOutput.AppendText(">> " + commandToSend + "\r\n", Color.Green);
                scrollDownOutput();
                commandToSend = "";
            }
        }

        public void btnSendLine_Click()
        {
            hisItems.Add(_linkToMain.txtSessionCommand.Text);
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);
            commandToSend = _linkToMain.txtSessionCommand.Text;
            _linkToMain.txtSessionCommand.Text = "";
            Write();
            scrollDownHist();
        }

        internal void startSessionInNewWindow()
        {

            mySessionForm = new Telnet();

            String serverIP = "";
            String serverPort = "";
            String from = "";
            String to = "";

            _linkToMain.Invoke((MethodInvoker)delegate()
            {
                if (_linkToMain.cbxServer.Text.Equals(""))
                {
                    _linkToMain.cbxSessionServer.Text = "192.168.0.1";
                }

                serverIP = _linkToMain.cbxSessionServer.Text;
                mySessionForm.setIP(serverIP);
                _linkToMain.addServerToList(_linkToMain.cbxSessionServer.Text);

                try
                {
                    int.Parse(_linkToMain.cbxSessionPort.Text);
                }
                catch (Exception)
                {
                    _linkToMain.cbxSessionPort.Text = "25";
                }
                serverPort = _linkToMain.cbxSessionPort.Text;

                from = _linkToMain.cbxSessionFrom.Text;
                to = _linkToMain.cbxSessionTo.Text;
            });

            mySessionForm.Text = "SMTP session to " + serverIP + " on port " + serverPort;
            mySessionForm.Show();

            mySessionForm.setPort(int.Parse(serverPort));
            mySessionForm.setFrom(_linkToMain.mailFromList);
            mySessionForm.setTo(_linkToMain.mailToList);

            mySessionForm.connect();

            _linkToMain.myParser.writeXML();

        }

        #region historyCommands
        public void btnResend_Click()
        {
            if (_linkToMain.lbxSessionHistory.SelectedIndex != -1)
            {
                commandToSend = _linkToMain.lbxSessionHistory.GetItemText(_linkToMain.lbxSessionHistory.SelectedItem);
                Write();

                if (_linkToMain.lbxSessionHistory.SelectedIndex != hisItems.Count - 1)
                {
                    _linkToMain.lbxSessionHistory.SetSelected(_linkToMain.lbxSessionHistory.SelectedIndex + 1, true);
                }
            }
        }

        public void btnClearCommand_Click()
        {
            int selectedIndex = _linkToMain.lbxSessionHistory.SelectedIndex;
            if (_linkToMain.lbxSessionHistory.SelectedIndex != -1)
            {
                hisItems.RemoveAt(_linkToMain.lbxSessionHistory.SelectedIndex);
                _linkToMain.lbxSessionHistory.DataSource = null;
                _linkToMain.lbxSessionHistory.DataSource = hisItems;
                //Debug.WriteLine("Selected Index: " + selectedIndex);
                if (selectedIndex == 0)
                {
                    if (hisItems.Count > 0)
                    {
                        _linkToMain.lbxSessionHistory.SetSelected(0, true);
                    }
                    else
                    {
                        _linkToMain.lbxSessionHistory.ClearSelected();
                    }
                }
                else
                {
                    _linkToMain.lbxSessionHistory.SetSelected(selectedIndex - 1, true);
                }
            }
        }
        #endregion

        #region quickCommands

        public void btnFrom_Click()
        {
            hisItems.Add("mail from: " + _linkToMain.cbxSessionFrom.Text);
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "mail from:" + _linkToMain.cbxSessionFrom.Text;
            Write();
            scrollDownHist();
        }

        public void btnRcpt_Click()
        {
            hisItems.Add("rcpt to:" + _linkToMain.cbxSessionTo.Text);
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "rcpt to:" + _linkToMain.cbxSessionTo.Text;
            Write();
            scrollDownHist();
        }

        public void btnData_Click()
        {
            hisItems.Add("data");
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "data";
            Write();
            scrollDownHist();
        }

        public void btnHelo_Click()
        {
            hisItems.Add("ehlo " + _linkToMain.txtSessionEhlo.Text);
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);
            commandToSend = "ehlo " + _linkToMain.txtSessionEhlo.Text;
            Write();
            scrollDownHist();
        }

        public void btnQuit_Click()
        {
            hisItems.Add("quit");
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "quit";
            Write();
            scrollDownHist();
        }

        public void btnDot_Click()
        {
            hisItems.Add(".");
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = ".";
            Write();
            scrollDownHist();
        }

        public void btnReset_Click()
        {
            hisItems.Add("rset");
            _linkToMain.lbxSessionHistory.DataSource = null;
            _linkToMain.lbxSessionHistory.DataSource = hisItems;
            _linkToMain.lbxSessionHistory.SetSelected(hisItems.Count - 1, true);

            commandToSend = "rset";
            Write();
            scrollDownHist();
        }
        #endregion

    }
}

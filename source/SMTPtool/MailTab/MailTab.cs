using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMTPtool;
using System.Diagnostics;
using System.Timers;
using System.Threading;
using System.Net.Mail;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SMTPtestTool
{
    public class MailTab
    {
        Main _linkToMain;

        MailMessage mail;
        List<Attachment> attachmentNameList = new List<Attachment>();
        List<string> attachmentPaths = new List<string>();
        public System.Timers.Timer mySendTimer;
        public int numberOfMessagesSent = 0;
        public int numberOfMessagesToSend = 0;

        //holds the time when send was clicked
        public DateTime sendStart;

        //constructor
        public MailTab(Main _linkToMain)
        {
            this._linkToMain = _linkToMain;
        }

        public void btnSendClicked()
        {
            _linkToMain.btnSend.Enabled = false;
            _linkToMain.btnPing.Enabled = false;

            _linkToMain.btnSend.Text = "Sending";
            mySendTimer = new System.Timers.Timer();
            mySendTimer.Elapsed += new ElapsedEventHandler(OnSendTimedEvent);
            mySendTimer.Interval = 300;
            mySendTimer.Enabled = true;

            //check server address and do list logic
            if (_linkToMain.cbxServer.Text.Equals(""))
            {
                _linkToMain.cbxServer.Text = "192.168.0.1";
            }

            //Debug.WriteLine("selected server: " + _linkToMain.cbxServer.Text);
            _linkToMain.addServerToList(_linkToMain.cbxServer.Text);

            addLogMessage("SMTP connection attempt to " + _linkToMain.cbxServer.Text + " on port " + _linkToMain.txtPort.Text);

            //check from address and do list logic
            try
            {
                var addr = new System.Net.Mail.MailAddress(_linkToMain.cbxFrom.Text);
            }
            catch
            {
                _linkToMain.cbxFrom.Text = "default@test.test";
            }

            //Debug.WriteLine("selected from: " + _linkToMain.cbxFrom.Text);
            _linkToMain.addMailFromtoList(_linkToMain.cbxFrom.Text);

            //check to address and do list logic
            try
            {
                var addr = new System.Net.Mail.MailAddress(_linkToMain.cbxTo.Text);
            }
            catch
            {
                _linkToMain.cbxTo.Text = "default@test.test";
            }

            //Debug.WriteLine("selected from: " + _linkToMain.cbxTo.Text);
            _linkToMain.addRcptToToList(_linkToMain.cbxTo.Text);

            try
            {
                int.Parse(_linkToMain.txtPort.Text);
            }
            catch (Exception)
            {
                _linkToMain.txtPort.Text = "25";
            }

            //it could be checked with a 2 sec timeout if the server is actually listening on the given port
            //would avoid the long default TCP handshake timeout
            /*
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = socket.BeginConnect(txtServer.Text, Convert.ToInt32(txtPort.Text), null, null);
            // Two second timeout
            bool success = result.AsyncWaitHandle.WaitOne(2000, true);
            if (!success)
            {
                socket.Close();
                throw new ApplicationException("Failed to connect server.");
            }
            */

            numberOfMessagesToSend = Convert.ToInt32(_linkToMain.nrcCount.Value) * Convert.ToInt32(_linkToMain.nrcThreadCount.Value);
            if (numberOfMessagesToSend == 1)
            {
                numberOfMessagesToSend = 0;
            }
            numberOfMessagesSent = 0;

            sendStart = DateTime.Now;
            SMTPsender mySender = new SMTPsender();
            mySender.Init(int.Parse(_linkToMain.txtPort.Text), _linkToMain.cbxServer.Text, _linkToMain);
            List<MailMessage> mailList = new List<MailMessage>();


            int counter = 1;
            Boolean addSubjectCounter = true;
            for (int b = 1; b <= _linkToMain.nrcThreadCount.Value; b++)
            {
                mySender = new SMTPsender();
                mySender.Init(int.Parse(_linkToMain.txtPort.Text), _linkToMain.cbxServer.Text, _linkToMain);
                mailList = new List<MailMessage>();

                for (int a = 1; a <= _linkToMain.nrcCount.Value; a++)
                {
                    mail = new MailMessage(_linkToMain.cbxFrom.Text, _linkToMain.cbxTo.Text);

                    if (addSubjectCounter) {
                        mail.Subject = _linkToMain.txtSubject.Text + " " + counter;
                    }
                    {
                        mail.Subject = _linkToMain.txtSubject.Text + " " + counter;
                    }
                    

                    mail.Body = _linkToMain.txtBody.Text;

                    //add attachments
                    if (attachmentNameList.Count > 0)
                    {
                        for (int i = 1; i <= attachmentNameList.Count; i++)
                        {
                            mail.Attachments.Add(attachmentNameList[i - 1]);
                        }
                    }

                    mailList.Add(mail);
                    counter++;
                }

                mySender.addMailList(mailList);
                Thread thread = new System.Threading.Thread(new ThreadStart(mySender.Run));
                thread.IsBackground = true;
                thread.Start();

            }
            _linkToMain.myParser.writeXML();
        }

        private void OnSendTimedEvent(object source, ElapsedEventArgs e)
        {
            _linkToMain.Invoke((MethodInvoker)delegate ()
            {
                switch (_linkToMain.btnSend.Text)
                {
                    case "Sending":
                        _linkToMain.btnSend.Text = "Sending.";
                        break;
                    case "Sending.":
                        _linkToMain.btnSend.Text = "Sending..";
                        break;
                    case "Sending..":
                        _linkToMain.btnSend.Text = "Sending...";
                        break;
                    case "Sending...":
                        _linkToMain.btnSend.Text = "Sending";
                        break;
                }
            });
        }

        #region attachment handling

        public void btnAttachClicked()
        {
            OpenFileDialog fileDialogAttachment = new OpenFileDialog();
            fileDialogAttachment.RestoreDirectory = true;
            fileDialogAttachment.Multiselect = true;

            try
            {
                if (fileDialogAttachment.ShowDialog() == DialogResult.OK)
                {
                    foreach (String fileName in fileDialogAttachment.FileNames)
                    {
                        if (File.Exists(fileName))
                        {
                            addAttachment(fileName);
                        }
                    }
                }
            }
            catch (Exception excpection)
            {
                MessageBox.Show(excpection.Message, "Attachment I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void addAttachment(String fileName)
        {
            try
            {
                attachmentNameList.Add(new Attachment(fileName));
                //Debug.WriteLine("FILENAME: " + Path.GetFileName(fileName)); 
                attachmentPaths.Add(Path.GetFileName(fileName));
                _linkToMain.lbAttachments.DataSource = null;
                _linkToMain.lbAttachments.DataSource = attachmentPaths;
                _linkToMain.btnDelAttachment.Enabled = true;
                _linkToMain.btnDelAttachmentAll.Enabled = true;
            }
            catch (Exception excpection)
            {
                MessageBox.Show(excpection.Message, "Attachment I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void btnDelAttachmentClicked()
        {
            try
            {
                foreach (var item in _linkToMain.lbAttachments.SelectedItems)
                {
                    attachmentNameList.RemoveAt(attachmentPaths.IndexOf(item.ToString()));
                    attachmentPaths.Remove(item.ToString());
                }
            }
            catch { }

            _linkToMain.lbAttachments.DataSource = null;
            _linkToMain.lbAttachments.DataSource = attachmentPaths;
            if (attachmentPaths.Count == 0)
            {
                _linkToMain.btnDelAttachment.Enabled = false;
                _linkToMain.btnDelAttachmentAll.Enabled = false;
            }
        }

        public void btnDelAttachmentAllClicked()
        {
            attachmentNameList = new List<Attachment>();
            attachmentPaths = new List<string>();

            _linkToMain.lbAttachments.DataSource = null;
            _linkToMain.lbAttachments.DataSource = attachmentPaths;

            _linkToMain.btnDelAttachment.Enabled = false;
            _linkToMain.btnDelAttachmentAll.Enabled = false;
        }
        #endregion



        //add log message
        public void addLogMessage(String logMessage)
        {
            String timeStamp = DateTime.Now.ToString("MMM dd HH:mm:ss");
            _linkToMain.txtLog.AppendText(timeStamp + "  " + logMessage + "\r\n");
            _linkToMain.txtLog.SelectionStart = _linkToMain.txtLog.Text.Length;
            _linkToMain.txtLog.ScrollToCaret();
        }

        public void btnPingClicked()
        {
            _linkToMain.btnPing.Enabled = false;
            _linkToMain.btnSend.Enabled = false;
            _linkToMain.btnPing.Text = "Ping Server";
            System.Timers.Timer myPingTimer = new System.Timers.Timer();
            myPingTimer.Elapsed += new ElapsedEventHandler(OnPingTimedEvent);
            myPingTimer.Interval = 300;
            myPingTimer.Enabled = true;

            if (_linkToMain.cbxServer.Text.Equals(""))
            {
                _linkToMain.cbxServer.Text = "192.168.0.1";
            }
            string myPingServer = _linkToMain.cbxServer.Text;
            BackgroundWorker bw = new BackgroundWorker();
            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;

                Ping ping = new Ping();
                try
                {
                    PingReply myPingReply = ping.Send(myPingServer);

                    if (myPingReply.Status == IPStatus.Success)
                    {
                        _linkToMain.Invoke((MethodInvoker)delegate ()
                        {
                            addLogMessage("Ping to " + myPingServer.ToString() + " [" + myPingReply.Address.ToString() + "]" + " Successful"
                           + " Response delay = " + myPingReply.RoundtripTime.ToString() + " ms");
                        });
                    }
                    else
                    {
                        _linkToMain.Invoke((MethodInvoker)delegate ()
                        {
                            addLogMessage("Ping to " + myPingServer.ToString() + " " + myPingReply.Status);
                        });
                    }
                }
                catch
                {
                    _linkToMain.Invoke((MethodInvoker)delegate ()
                    {
                        addLogMessage("Ping to " + myPingServer.ToString() + " DNS error");
                    });
                }

                _linkToMain.Invoke((MethodInvoker)delegate ()
                {
                    _linkToMain.btnPing.Enabled = true;
                    _linkToMain.btnSend.Enabled = true;
                    _linkToMain.btnPing.Text = "Ping Server";
                    myPingTimer.Enabled = false;
                });

            });

            bw.RunWorkerAsync();
        }

        private void OnPingTimedEvent(object source, ElapsedEventArgs e)
        {
            _linkToMain.Invoke((MethodInvoker)delegate ()
            {
                switch (_linkToMain.btnPing.Text)
                {
                    case "Ping Server":
                        _linkToMain.btnPing.Text = "Ping Server.";
                        break;
                    case "Ping Server.":
                        _linkToMain.btnPing.Text = "Ping Server..";
                        break;
                    case "Ping Server..":
                        _linkToMain.btnPing.Text = "Ping Server...";
                        break;
                    case "Ping Server...":
                        _linkToMain.btnPing.Text = "Ping Server";
                        break;
                }
            });
        }
    }
}

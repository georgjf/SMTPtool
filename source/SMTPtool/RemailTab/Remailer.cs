using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using SMTPtool;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.IO;
using System.Collections;

namespace SMTPtestTool
{
    public class Remailer
    {

        private String serverIP = "192.168.0.25";
        private int serverPort = 25;
        private String mailFrom;
        private String rcptTo;

        public List<string> serverList = new List<string>();
        public List<string> mailFromList = new List<string>();
        public List<string> mailToList = new List<string>();

        private List<string> hisItems = new List<string>();
        public String commandToSend = "";
        TcpClient clientSocket;

        private DateTime sendStart;
        private DateTime sendEnd;

        private Boolean messageSent = false;

        //prevents that mailFrom & rcptTo are send twice
        //MIMEsweeper has an older SMTP RFC implemented
        private Boolean mailFromSent = false;
        private Boolean rcptToSent = false;

        public Main _linkToMain;

        //public String mailBody;
        public Boolean sendSingle;

        //used to hold the path of the current file to remail when a folder has been selected
        public String fullMailBody;

        //number of data lines per Command
        private int chunkSize = 0;

        Thread ctThread;

        //constructor
        public Remailer(Main _linkToMain)
        {
            this._linkToMain = _linkToMain;
        }

        public void connect()
        {
            _linkToMain.btnRemail.Enabled = false;
            if (_linkToMain.cbxRemailIP.Text.Equals(""))
            {
                _linkToMain.cbxRemailIP.Text = "192.168.0.1";
            }
            serverIP = _linkToMain.cbxRemailIP.Text;
            _linkToMain.addServerToList(serverIP);


            try { int.Parse(_linkToMain.txtRemailPort.Text); }
            catch { _linkToMain.txtRemailPort.Text = "25"; }
            serverPort = int.Parse(_linkToMain.txtRemailPort.Text);

            try
            {
                var addr = new System.Net.Mail.MailAddress(_linkToMain.cbxRemailFrom.Text);
                mailFrom = _linkToMain.cbxRemailFrom.Text;
            }
            catch
            {
                _linkToMain.cbxRemailFrom.Text = "";
                mailFrom = _linkToMain.cbxRemailFrom.Text;
            }
            _linkToMain.addMailFromtoList(mailFrom);

            try
            {
                var addr = new System.Net.Mail.MailAddress(_linkToMain.cbxRemailTo.Text);
                rcptTo = _linkToMain.cbxRemailTo.Text;
            }
            catch
            {
                _linkToMain.cbxRemailTo.Text = "default@test.test";
                rcptTo = _linkToMain.cbxRemailTo.Text;
            }
            _linkToMain.addRcptToToList(rcptTo);

            try
            {
                clientSocket = new TcpClient();
                clientSocket.Connect(serverIP, serverPort);
                ctThread = new System.Threading.Thread(new ThreadStart(Run));
                ctThread.IsBackground = true;
                ctThread.Start();

                sendStart = DateTime.Now;
                if (sendSingle)
                {
                    _linkToMain.txtRemailOutput.AppendText(getTimeStamp() + " - Connecting to " + serverIP + " on port" + serverPort + "\r\n", Color.Red);
                }

                scrollDownOutput();
            }

            catch (Exception)
            {
                _linkToMain.txtRemailOutput.AppendText("Connection Error\r\n\r\n", Color.Red);
                scrollDownOutput();
                _linkToMain.btnRemail.Enabled = true;
            }

            _linkToMain.myParser.writeXML();
        }

        public void Run()
        {
            String strMessage;
            String singleCommand;

            String lastCommand = "none";

            while (true)
            {

                strMessage = Read();
                Debug.WriteLine(strMessage);

                _linkToMain.Invoke((MethodInvoker)delegate ()
                {
                    if (!strMessage.Equals(""))
                    {
                        if (sendSingle)
                        {
                            _linkToMain.txtRemailOutput.AppendText("<< " + strMessage, Color.Blue);
                        }
                    }
                    scrollDownOutput();
                });

                singleCommand = strMessage.TrimEnd('\r', '\n');
                singleCommand = singleCommand.ToLower();

                if (strMessage.Equals(""))
                {
                    _linkToMain.Invoke((MethodInvoker)delegate ()
                    {
                        if (sendSingle)
                        {
                            _linkToMain.txtRemailOutput.AppendText("Connection closed\r\n\r\n", Color.Red);
                            scrollDownOutput();
                        }
                        clientSocket.Close();
                        ctThread.Abort();
                        _linkToMain.btnRemail.Enabled = true;

                    });
                    break;
                }
                // EHLO
                else if (strMessage.StartsWith("220") && lastCommand.Equals("none"))
                {

                    //String hostname =
                    String hostname = strMessage.Split(' ', ' ')[1];
                    Debug.WriteLine("HOSTNAME: " + hostname);
                    commandToSend = "HELO " + hostname;
                    lastCommand = "helo";
                    write();
                }
                //MAIL FROM
                else if ((strMessage.StartsWith("250") && mailFromSent == false) && lastCommand.Equals("helo"))
                {
                    commandToSend = "mail from: <" + mailFrom + ">";
                    mailFromSent = true;
                    lastCommand = "mailFrom";
                    write();
                }
                //RCPT TO
                else if ((strMessage.StartsWith("250 2.1.0") || strMessage.StartsWith("250 " + mailFrom) || strMessage.StartsWith("250 Go ahead")) && lastCommand.Equals("mailFrom"))
                { 
                    commandToSend = "rcpt to: <" + rcptTo + ">";
                    lastCommand = "rcpt";
                    write();
                }
                //DATA
                else if ((strMessage.StartsWith("250 2.1.5") || strMessage.StartsWith("250 " + rcptTo) || strMessage.StartsWith("250 Go ahead")) && lastCommand.Equals("rcpt"))
                {
                    commandToSend = "data";
                    lastCommand = "data";
                    write();
                }
                else if (strMessage.StartsWith("354") && lastCommand.Equals("data"))
                {
                    //sending folder
                    if (fullMailBody != null)
                    {

                    }
                    //sending single file
                    else
                    {
                        fullMailBody = _linkToMain.txtMailView.Text;
                    }

                    using (StringReader sr = new StringReader(fullMailBody))
                    {
                        String line;
                        String singleChunk = "";
                        List<String> messageChunks = new List<string>();
                        int currentLine = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            //dont split it up in junks if chunk size set to 0
                            if (chunkSize == 0)
                            {
                                singleChunk = singleChunk + line + "\r\n";
                            }
                            else
                            {
                                if (currentLine < chunkSize)
                                {
                                    if (currentLine == chunkSize + 1)
                                    {
                                        singleChunk = singleChunk + line;
                                    }
                                    else
                                    {
                                        singleChunk = singleChunk + line + "\r\n";
                                    }
                                    currentLine++;
                                }
                                else
                                {
                                    messageChunks.Add(singleChunk);
                                    singleChunk = "";
                                    singleChunk = singleChunk + line + "\r\n";
                                    currentLine = 0;
                                }
                            }
                        }
                        if (!singleChunk.Equals(""))
                        {
                            messageChunks.Add(singleChunk);
                        }

                        //working code for progress bar
                        //not useful if chunksize = 0
                        //because the whole data part of the message is sent in one command

                        int currentChunkNumber = 0;
                        int numberOfChunks = messageChunks.Count;

                        foreach (String chunk in messageChunks)
                        {
                            currentChunkNumber++;
                            //Debug.WriteLine("singleChunk: " + chunk);
                            commandToSend = chunk;
                            writeChunk();
                            /*
                            _linkToMain.Invoke((MethodInvoker)delegate()
                            {
                                float percentage = (currentChunkNumber * 100) / numberOfChunks;
                                _linkToMain.lblRemailProgressPercent.Text = percentage + "%";
                                _linkToMain.remailProgress.Value = (int)Math.Round(percentage);
                            });
                             */
                        }
                        /*
                        _linkToMain.Invoke((MethodInvoker)delegate()
                        {
                            _linkToMain.lblRemailProgressPercent.Text = "100%";
                            _linkToMain.remailProgress.Value = 100;
                            //_linkToMain.toolStripStatusLabel1.Text = currentChunkNumber.ToString();
                        });
                         * */

                    }
                    commandToSend = ".";
                    lastCommand = "content";
                    write();

                }
                //QUIT
                else if (strMessage.StartsWith("250")  && lastCommand.Equals("content"))
                {

                    messageSent = true;
                    sendEnd = DateTime.Now;
                    commandToSend = "quit";
                    //Debug.WriteLine("linetosend " + lineNumber);
                    write();

                }
                else
                {
                    _linkToMain.Invoke((MethodInvoker)delegate ()
                    {
                        Debug.WriteLine("send start: " + sendStart);
                        Debug.WriteLine("send end: " + sendEnd);

                        if (messageSent)
                        {
                            Debug.WriteLine("MESSAGE SENT SUCESSFULLY");
                            TimeSpan duration = sendEnd - sendStart;
                            _linkToMain.txtRemailOutput.AppendText(getTimeStamp() + " - Message sent successfully - duration " + Math.Round(duration.TotalSeconds, 2) + " seconds \r\n", Color.Red);
                        }
                        else
                        {
                            Debug.WriteLine("MESSAGE NOT SENT");
                            _linkToMain.txtRemailOutput.AppendText(getTimeStamp() + " - Message not sent \r\n", Color.Red);
                        }
                        if (sendSingle)
                        {
                            _linkToMain.txtRemailOutput.AppendText("Connection closed\r\n\r\n", Color.Red);
                        }
                        scrollDownOutput();
                        clientSocket.Close();
                        ctThread.Abort();
                        _linkToMain.btnRemail.Enabled = true;

                    });
                }
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
                Debug.WriteLine("EEEEEEEEEEEEEEERRROR: " + exception.Message);
                scrollDownOutput();
                clientSocket.Close();
                return null;
            }
        }

        //write method to send single line SMTP commands
        public void write()
        {
            if (clientSocket.Connected)
            {
                if (sendSingle)
                {
                    scrollDownOutput();
                }
                NetworkStream clientStream = clientSocket.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(commandToSend + "\r\n");

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

                if (sendSingle)
                {
                    _linkToMain.txtRemailOutput.AppendText(">> " + commandToSend + "\r\n", Color.Green);
                    scrollDownOutput();
                }

                commandToSend = "";
            }
        }

        public String getTimeStamp()
        {
            return DateTime.Now.ToString("MMM dd HH:mm:ss");
        }

        //second send method used to write large data parts
        public void writeChunk()
        {
            if (clientSocket.Connected)
            {
                scrollDownOutput();
                NetworkStream clientStream = clientSocket.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(commandToSend);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
                /*
                if (commandToSend.Length > 1000)
                {
                    _linkToMain.txtRemailOutput.AppendText(">> \r\n", Color.Green);
                    IEnumerable<String> myChunks = Split(commandToSend, 100);
                    foreach (String chunk in myChunks)
                    {
                        _linkToMain.txtRemailOutput.AppendText(chunk, Color.Green);
                        scrollDownOutput();
                    }
                }
                else {
                    _linkToMain.txtRemailOutput.AppendText(">> \r\n" + commandToSend, Color.Green);
                    scrollDownOutput();
                }
                 * */
                if (sendSingle)
                {
                    _linkToMain.txtRemailOutput.AppendText(">> \r\n" + commandToSend, Color.Green);
                    scrollDownOutput();
                }

                commandToSend = "";
            }
        }

        static IEnumerable<String> Split(String str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        private void scrollDownOutput()
        {
            // scroll output box to bottom
            try
            {
                _linkToMain.Invoke((MethodInvoker)delegate ()
                {
                    _linkToMain.txtRemailOutput.SelectionStart = _linkToMain.txtRemailOutput.Text.Length;
                    _linkToMain.txtRemailOutput.ScrollToCaret();
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Data);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Timers;
using System.Xml;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using SMTPtool.helper;
using SMTPtestTool;
using System.Drawing;


namespace SMTPtool
{
    public partial class Main : Form
    {
        public XMLparser myParser;

        public static String CURRENT_VERSION = "4.0";
        public static String RELEASE_DATE = "08/2016";
        public static String UPDATE_URL = "https://raw.githubusercontent.com/georgjf/SMTPtool/master/update/updateInfo.xml";
        
        public String updateInfoURL = "https://github.com/georgjf/SMTPtool";
        public String authorEmail = "mailto:georg.felgitsch@gmail.com";

        //max number of elementes stored in dropdown menus
        public int histSize = 10;

        public List<string> serverList = new List<string>();
        public List<string> mailFromList = new List<string>();
        public List<string> mailToList = new List<string>();

        //subclasses
        public MailTab mailTab;
        public RemailTab remailTab;
        public SessionTab sessionTab;

        public Main()
        {
            InitializeComponent();

            //initToolTips();
            this.Text = "SMTP Test Tool";
            this.Text = "SMTP Test Tool - v5.0 PRELEASE RC2 - Alyn Version - PLEASE REPORT BUGS ASAP";
            

            mailTab = new MailTab(this);
            remailTab = new RemailTab(this);
            sessionTab = new SessionTab(this);

            smtpTabPage.TabPages[1].BackColor = Main.DefaultBackColor;
            smtpTabPage.TabPages[0].BackColor = Main.DefaultBackColor;
           //smtpTabPage.SelectedIndex = 1;

            myParser = new XMLparser(this);
            myParser.loadMainXML();

            updateCheck();

            Debug.WriteLine("");
            Debug.WriteLine("SMTPtool started");
            Debug.WriteLine("SMTPtool initiation complete");
            
            //experimental SMTP receiver service
            // startSMTPServer();
            
        }


        private void updateCheck()
        {
            statusLabel.Text = "";
            try
            {
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(UPDATE_URL);
                //check if a proxy is in use
                //if necessary use NTLM authentication
                bool useProxy = !string.Equals(System.Net.WebRequest.DefaultWebProxy.GetProxy(new Uri(UPDATE_URL)), UPDATE_URL);
                //Console.WriteLine(useProxy ? "Yes" : "No");
                if (useProxy)
                {
                    IWebProxy proxy = myWebRequest.Proxy;
                    string proxyuri = proxy.GetProxy(myWebRequest.RequestUri).ToString();
                    myWebRequest.UseDefaultCredentials = true;
                    myWebRequest.Proxy = new WebProxy(proxyuri, false);
                    myWebRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }

                String xmlResponse = "";
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                xmlResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();

                XDocument xmlDoc = XDocument.Parse(xmlResponse);
                var newestVersionNumber = xmlDoc.Element("upgrades").Element("currentVersion").Attribute("version").Value;
                var newestVersionDate = xmlDoc.Element("upgrades").Element("currentVersion").Attribute("releaseDate").Value;
                var newestVersionComment = xmlDoc.Element("upgrades").Element("currentVersion").Attribute("comment").Value;
                updateInfoURL = xmlDoc.Element("upgrades").Element("currentVersion").Attribute("updateURL").Value;
                var newestVersionAuthor = xmlDoc.Element("upgrades").Element("currentVersion").Attribute("author").Value;
                authorEmail = xmlDoc.Element("upgrades").Element("currentVersion").Attribute("authorMail").Value;
            
                if (newestVersionNumber.Equals(CURRENT_VERSION))
                {
                    statusLabel.Text = "v" + newestVersionNumber + "  -  " + newestVersionDate + "  -  up to date";
                    statusLabelAuthorInfo.Text = " |  Author:";
                    statusLabelAuthorInfo.Visible = true;
                    statusLabelMail.Text = newestVersionAuthor;
                    statusLabelMail.Visible = true;
                    statusLabelUpdateInfo.Text = "|  " + newestVersionComment;
                    statusLabelUpdateInfo.Visible = true;
                    statusLabelLink.Text = updateInfoURL;
                    statusLabelLink.Visible = true;
                }
                else {
                    statusLabel.Text = "v" + CURRENT_VERSION + "  |  release " + RELEASE_DATE + "  |  Latest version: " + newestVersionNumber;
                    statusLabel.ForeColor = Color.Red;
                    statusLabelAuthorInfo.Text = " |  Author:";
                    statusLabelAuthorInfo.Visible = true;
                    statusLabelMail.Text = newestVersionAuthor;
                    statusLabelMail.Visible = true;
                    statusLabelUpdateInfo.Text = "|  " + newestVersionComment;
                    statusLabelUpdateInfo.Visible = true;
                    statusLabelLink.Text = updateInfoURL;
                    statusLabelLink.Visible = true;
                }
                
            }
            catch (Exception e) {
                Debug.WriteLine("EXCEPTION: " + e.Message);
                statusLabel.Text = "v" + CURRENT_VERSION + "  |  release " + RELEASE_DATE + "  |  Could not connect to update server";
                statusLabelLink.Text = updateInfoURL;
                statusLabelLink.Visible = true;
                statusLabelAuthorInfo.Text = "|  Author:";
                statusLabelAuthorInfo.Visible = true;
                statusLabelMail.Text = "Georg Felgitsch";
                statusLabelMail.Visible = true;
                statusLabelUpdateInfo.Text = "|  " + "Visit";
                statusLabelUpdateInfo.Visible = true;
                statusLabelLink.Text = updateInfoURL;
                statusLabelLink.Visible = true;
            }
            
        }

        public void addServerToList(String server)
        {
            serverList.Reverse();
            serverList.Add(server);
            serverList.Reverse();
            serverList = serverList.Distinct().ToList();

            if (serverList.Count > histSize)
            {
                serverList.RemoveAt(histSize);
            }

            cbxServer.DataSource = null;
            cbxServer.DataSource = serverList;
            int index = cbxServer.FindString(server);
            cbxServer.SelectedIndex = index;

            cbxRemailIP.DataSource = null;
            cbxRemailIP.DataSource = serverList;
            index = cbxRemailIP.FindString(server);
            cbxRemailIP.SelectedIndex = index;

            cbxSessionServer.DataSource = null;
            cbxSessionServer.DataSource = serverList;
            index = cbxSessionServer.FindString(server);
            cbxSessionServer.SelectedIndex = index;
        }

        public void addMailFromtoList(String mailFrom)
        {
            mailFromList.Reverse();
            mailFromList.Add(mailFrom);
            mailFromList.Reverse();
            mailFromList = mailFromList.Distinct().ToList();

            if (mailFromList.Count > histSize)
            {
                mailFromList.RemoveAt(histSize);
            }
            cbxFrom.DataSource = null;
            cbxFrom.DataSource = mailFromList;
            int index = cbxFrom.FindString(mailFrom);
            cbxFrom.SelectedIndex = index;

            cbxRemailFrom.DataSource = null;
            cbxRemailFrom.DataSource = mailFromList;
            index = cbxRemailFrom.FindString(mailFrom);
            cbxRemailFrom.SelectedIndex = index;
        }

        public void addRcptToToList(String rcptTo)
        {
            mailToList.Reverse();
            mailToList.Add(rcptTo);
            mailToList.Reverse();
            mailToList = mailToList.Distinct().ToList();

            if (mailToList.Count > histSize)
            {
                mailToList.RemoveAt(histSize);
                //mailToList.RemoveRange(histSize, mailToList.Count - histSize);
            }

            cbxTo.DataSource = null;
            cbxTo.DataSource = mailToList;
            int index = cbxTo.FindString(rcptTo);
            cbxTo.SelectedIndex = index;

            cbxRemailTo.DataSource = null;
            cbxRemailTo.DataSource = mailToList;
            index = cbxRemailTo.FindString(rcptTo);
            cbxRemailTo.SelectedIndex = index;

        }

        //init experimental SMTP receiver service
        private void startSMTPServer()
        {
            SMTPServer myServer = new SMTPServer();
            Thread thread = new System.Threading.Thread(new ThreadStart(myServer.Run));
            thread.IsBackground = true;
            thread.Start();
        }

        private void initToolTips()
        {
            ToolTip myToolTip = toolTipFactory();
            //myToolTip.SetToolTip(this.btnSend, "Send the message");
            //myToolTip = toolTipFactory();
            myToolTip.SetToolTip(this.cbxServer, "Add IP or FQDN");
        }

        private ToolTip toolTipFactory()
        {
            ToolTip myToolTip = new ToolTip();
            myToolTip.InitialDelay = 1000;
            myToolTip.ReshowDelay = 500;
            myToolTip.ShowAlways = true;
            return myToolTip;
        }

        private void form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void notifyAboutSentMessage(String result, TimeSpan duration)
        {

            if (result.Equals("success"))
            {
                //if there was only one message to send
                if (mailTab.numberOfMessagesToSend == 0)
                {
                    mailTab.addLogMessage("Message sent successfully - duration " + Math.Round(duration.TotalSeconds, 2) + " seconds.");
                }
                else
                {
                    mailTab.addLogMessage("Message " + (mailTab.numberOfMessagesSent + 1) + " sent successfully - duration " + Math.Round(duration.TotalSeconds, 2) + " seconds.");
                    mailTab.numberOfMessagesSent++;
                }
            }
            //if there was an error sending a message
            else
            {
                if (mailTab.numberOfMessagesToSend == 0)
                {
                    mailTab.addLogMessage(result);
                }
                else
                {
                    mailTab.addLogMessage(result + " " + (mailTab.numberOfMessagesSent + 1));
                    mailTab.numberOfMessagesSent++;
                }
            }

            //if all messages are sent enable the buttons 
            if (mailTab.numberOfMessagesSent == mailTab.numberOfMessagesToSend)
            {
                btnPing.Enabled = true;
                btnSend.Enabled = true;
                btnSend.Text = "Send";
                mailTab.mySendTimer.Enabled = false;
            }
        }

        private void mailTabDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                foreach (string fileLoc in filePaths)
                {
                    if (File.Exists(fileLoc))
                    {
                        mailTab.addAttachment(fileLoc);
                    }
                }
            }
        }

        private void remailTabDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                foreach (string fileLoc in filePaths)
                {
                    if (File.Exists(fileLoc))
                    {
                        String extension = Path.GetExtension(fileLoc);
                        if (extension.Equals(".eml") || extension.Equals(".qa"))
                        {
                            remailTab.createDirectories();
                            try {
                                File.Delete(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Import\\" + Path.GetFileName(fileLoc));
                            }
                            catch (Exception){}
                            File.Copy(fileLoc, Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Import\\" + Path.GetFileName(fileLoc));
                        }
                        else {
                            MessageBox.Show("A " + extension + " file cannot be imported", "Import error", MessageBoxButtons.OK, MessageBoxIcon.Information,MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        }
                    }
                }
            }
        }

        #region buttons

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtLog.Text = "";
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            mailTab.btnPingClicked();
        }

        private void btnDelAttachmentAll_Click(object sender, EventArgs e)
        {
            mailTab.btnDelAttachmentAllClicked();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            mailTab.btnSendClicked();
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            mailTab.btnAttachClicked();
        }

        private void btnDelAttachment_Click(object sender, EventArgs e)
        {
            mailTab.btnDelAttachmentClicked();
        }

        private void btnRemail_Click(object sender, EventArgs e)
        {
            remailTab.btnRemailClicked();
        }

        private void btnDeleteMail_Click(object sender, EventArgs e)
        {
            remailTab.btnDeleteMailClicked();
        }

        private void btnOpenMail_Click(object sender, EventArgs e)
        {
            remailTab.btnOpenMailClicked();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            remailTab.btnOpenFolderClicked();
        }

        private void btnRemailSaveMail_Click(object sender, EventArgs e)
        {
            remailTab.btnSaveClicked();
        }

        private void btnRemailRenameMail_Click(object sender, EventArgs e)
        {
            remailTab.btnRenameClicked();
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            remailTab.btnNewFolderClicked();
        }

        private void btnDeleteFolder_Click(object sender, EventArgs e)
        {
            remailTab.btnDeleteFolderClicked();
        }

        private void btnRemailCopyMail_Click(object sender, EventArgs e)
        {
            remailTab.btnCopyMailClicked();
        }

        private void statusLabelLink_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(updateInfoURL);
        }

        private void statusLabelMail_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + authorEmail);
        }


        private void btnSessionOpenNewWindow_Click(object sender, EventArgs e)
        {
            sessionTab.startSessionInNewWindow();
        }

        private void btnSessionConnect_Click(object sender, EventArgs e)
        {
            if (sessionTab.isConnected)
            {
                sessionTab.reconnect();

            }
            else
            {
                sessionTab.connect();
                sessionTab.isConnected = true;
            }
        }

        private void btnSessionSendLine_Click(object sender, EventArgs e)
        {
            sessionTab.btnSendLine_Click();
        }

        private void btnSessionHelo_Click(object sender, EventArgs e)
        {
            sessionTab.btnHelo_Click();
        }

        private void btnSessionFrom_Click(object sender, EventArgs e)
        {
            sessionTab.btnFrom_Click();
        }

        private void btnSessionRcpt_Click(object sender, EventArgs e)
        {
            sessionTab.btnRcpt_Click();
        }

        private void btnSessionData_Click(object sender, EventArgs e)
        {
            sessionTab.btnData_Click();
        }

        private void btnSessionDot_Click(object sender, EventArgs e)
        {
            sessionTab.btnDot_Click();
        }

        private void btnSessionReset_Click(object sender, EventArgs e)
        {
            sessionTab.btnReset_Click();
        }

        private void btnSessionQuit_Click(object sender, EventArgs e)
        {
            sessionTab.btnQuit_Click();
        }

        private void btnSessionResend_Click(object sender, EventArgs e)
        {
            sessionTab.btnResend_Click();
        }

        private void btnSessionClearCommand_Click(object sender, EventArgs e)
        {
            sessionTab.btnClearCommand_Click();
        }

        #endregion

        private void chbSaveInOutbox_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSaveInOutbox.Checked == true)
            {
                nrcCount.Enabled = false;
                nrcCount.Value = 1;
                nrcThreadCount.Enabled = false;
                nrcThreadCount.Value = 1;
            }
            else {
                nrcThreadCount.Enabled = true;
                nrcCount.Enabled = true;
            }
        }


           
    }
}

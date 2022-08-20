using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using SMTPtestTool;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SMTPtool
{
    class SMTPsender
    {
        SmtpClient client;       
        Main linkToMain;
        int mailNumber = 0;
        int maxMailCount = 1;

        List<MailMessage> mailList = new List<MailMessage>();

        internal void Init(int port, String server, Main linkToMain)
        {
            //this.mailTab = mailTab;
            this.linkToMain = linkToMain;
            client = new SmtpClient();
            client.Port = port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = server;
            client.EnableSsl = false;
            
            /*
            ServicePointManager.ServerCertificateValidationCallback =
            delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
            */
            //ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
             
        }

        bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;
            // If there are errors in the certificate chain, look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        bool chainIsValid = chain.Build((X509Certificate2)certificate);
                        if (!chainIsValid)
                        {
                            isOk = false;
                        }
                    }
                }
            }
            return isOk;

        }

        public void Run()
        {
            for (int i = 1; i <= mailList.Count; i++)
            {
                try
                {
                    DateTime sendStart = DateTime.Now;
                    client.Send(mailList[i - 1]);
                    linkToMain.Invoke((MethodInvoker)delegate()
                    {
                        TimeSpan duration = DateTime.Now - linkToMain.mailTab.sendStart;
                        linkToMain.notifyAboutSentMessage("success", duration);
                    });
                }
                catch (Exception exception)
                {
                    linkToMain.Invoke((MethodInvoker)delegate()
                    {
                        TimeSpan duration = DateTime.Now - linkToMain.mailTab.sendStart;
                        linkToMain.notifyAboutSentMessage(exception.Message, duration);

                    });                    
                }

                if (linkToMain.chbSaveInOutbox.Checked == true)
                {
                    linkToMain.remailTab.createDirectories();
                    Debug.Write("MESSAGE SAVE TO OUTBOX TRIGGERED");
                    client = new SmtpClient();
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Outbox\\"; ;
                    client.Send(mailList[0]);
                    linkToMain.Invoke((MethodInvoker)delegate()
                    {
                        linkToMain.mailTab.addLogMessage("Message saved in outbox!");
                        linkToMain.remailTab.triggerTreeViewRebuild();
                    }); 

                }
            }

            
        }


        internal void setMailNumber(int a)
        {
            this.mailNumber = a;
        }

        internal void setMailCount(int maxMailCount)
        {
            this.maxMailCount = maxMailCount;
        }

        internal void addMailList(List<MailMessage> mailList)
        {
            this.mailList = mailList;
        }

    }
}

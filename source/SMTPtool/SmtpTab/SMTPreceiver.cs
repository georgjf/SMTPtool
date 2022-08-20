using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net.Mail;
using System.IO;
using System.Reflection;

namespace SMTPtool
{
    class SMTPconnection
    {
        TcpClient client;
        string myRecipient = "";
        string mySender = "";
        string body = "";
        bool waitingForData = false;

        SmtpClient smtpClient;
        MailMessage myMail;

        internal void Init(System.Net.Sockets.TcpClient client)
        {
            this.client = client;
            Debug.WriteLine("CURRENT TIMEOUT: " + client.SendTimeout);
        }

        public void Run()
        {
            Write("220 SMTP Relay -- Welcome");
            string strMessage = "";
            string singleCommand = "";
            

            while (true)
            {
                try
                {
                    strMessage = Read();
                }
                catch (Exception)
                {
                    //a socket error has occured
                    break;
                }

                //Debug.WriteLine("character: " + strMessage);

                if (strMessage.Length > 0)
                {
                    singleCommand = singleCommand + strMessage;
                    if (strMessage.Contains((char)13))
                    {
                        //singleCommand = singleCommand.Remove(singleCommand.Length - 1);

                        

                        if (waitingForData)
                        {
                            Debug.WriteLine("commandLenght:" + singleCommand.Length);
                            Debug.WriteLine("singleCommand:" + singleCommand);
                            
                            if (singleCommand.StartsWith(".") && singleCommand.Length == 3)
                            {
                                Debug.WriteLine("commandLenght:" + singleCommand.Length);
                                Debug.WriteLine("commasingleCommandndLenght:" + singleCommand);
                                //set mail object parameter
                                myMail = new MailMessage(mySender, myRecipient);
                                myMail.Body = body;

                                Write("250 2.0.0 Message accepted for delivery");
                                //accept other commands again
                                waitingForData = false;
                                //Debug.WriteLine("Body: " + body);

                                //save message to inbox
                                smtpClient = new SmtpClient();
                                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory; 
                                //if directory outbox doesn't exist > create it > save eml to outbox
                                System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\inbox");
                                smtpClient.PickupDirectoryLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\inbox";
                                smtpClient.Send(myMail);
                                Debug.WriteLine("MESSAGE SAVED");
                            }
                            else
                            {
                                body = body + singleCommand;
                               // Debug.WriteLine(singleCommand);
                            }

                        }
                        else
                        {
                            singleCommand = singleCommand.TrimEnd('\r', '\n');
                            singleCommand = singleCommand.ToLower();

                            if (singleCommand.StartsWith("quit"))
                            {
                                Write("221 2.0.0 smtp.tool.test closing connection");
                                client.Close();
                                break;
                            }
                            else if (singleCommand.StartsWith("helo"))
                            {
                                Write("250 smtp.tool.test Hello [192.168.0.3], pleased to meet you");     
                            }
                            else if (singleCommand.StartsWith("rcpt to:"))
                            {
                                myRecipient = singleCommand.Substring(8);
                                Write("250 2.1.5" + myRecipient + "... Recipient ok");
                            }
                            else if (singleCommand.StartsWith("mail from:"))
                            {
                                mySender = singleCommand.Substring(10);
                                Write("250 2.1.5" + mySender + "... Sender ok");
                            }
                            else if (singleCommand.StartsWith("data"))
                            {
                                if (mySender.Equals(""))
                                {
                                    Write("503 5.5.0 Need MAIL command");
                                }
                                else if (myRecipient.Equals(""))
                                {
                                    Write("503 5.5.0 Need RCPT (recipient)");
                                }
                                else
                                {
                                    Write("354 Enter mail, end with \".\" on a line by itself");
                                    //Write("250 OK");
                                    

                                    waitingForData = true;
                                }

                            }

                            else
                            {
                                Write("500 5.5.1 Command unrecognized");
                            }
                        }


                        //Debug.WriteLine(singleCommand);
                        singleCommand = "";

                    }


                }
            }
        }

        private void Write(String strMessage)
        {
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(strMessage + "\r\n");

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        private String Read()
        {
            byte[] messageBytes = new byte[8192];
            int bytesRead = 0;
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            bytesRead = clientStream.Read(messageBytes, 0, 8192);
            string strMessage = encoder.GetString(messageBytes, 0, bytesRead);
            return strMessage;
        }
    }
}

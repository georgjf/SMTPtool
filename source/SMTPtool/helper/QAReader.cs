using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ionic.Zlib;
using System.Text.RegularExpressions;

namespace Core
{
    public class QAReader
    {
        #region Fields
        /// <summary>
        /// Unique GUID that identifies a MSW quarantine file.
        /// </summary> 
        Guid m_mswGuid = new Guid("{4D2045C6-D3B3-48a3-BDD4-1B911E711B67}");

        /// <summary>
        /// Unique GUID that identifies a Appliance quarantine file.
        /// </summary>
        Guid m_v3ApplianceGuid = new Guid("{4089d378-278f-4626-8215-b06b4bba4e5a}");

        /// <summary>
        /// Unique GUID that identifies a v4 Appliance quarantine file.
        /// </summary>
        Guid m_v4ApplianceGuid = new Guid("{4089d478-278f-4626-8215-b06b4bba4e5a}");

        /// <summary>
        /// Unique GUID identifying this Appliance quarantine file.
        Guid m_guid;
        /// </summary>


        /// <summary>
        /// BaseFile object.
        /// </summary>
        BaseFile m_file;

        /// <summary>
        /// The xml context property list.
        /// </summary>
        XmlNode m_contextPropertyList;

        /// <summary>
        /// The raw message as a string.
        /// </summary>
        string m_rawMessage;

        /// <summary>
        /// Set to true if the constructor fails
        /// </summary>
        public bool failed;

        /// <summary>
        /// The Id of message extracted from the raw message.
        /// </summary>
        Guid m_spamToolsId;

        string m_spamToolsReceived;
        string m_spamToolsIP;
        string m_spamToolsScore;
        string m_spamToolsDetectedByTM;
        string m_spamToolsFrom;
        //string m_refId;
        #endregion

        #region Properties
        public List<string> DetectedBy
        {
            get
            {
                try
                {
                    return GetDetectionReason(m_contextPropertyList);
                }
                catch (Exception e)
                {
                    Console.WriteLine("QA: Error: Error thrown in QAReader: Get DetectedBy", e);
                    return null;
                }

            }
        }

        public string RefId
        {
            get
            {
                try
                {
                    return this.GetRefId(m_contextPropertyList);
                }
                catch (Exception e)
                {
                    Console.WriteLine("QA: Error thrown in QAReader.GetRefId", e);
                    return null;
                }
            }
        }

        public string LastMta
        {
            get
            {
                try
                {
                    return this.GetLastMta(m_contextPropertyList);
                }
                catch (Exception e)
                {
                    Console.WriteLine("QA: Error thrown in QAReader.GetLastMta", e);
                    return null;
                }
            }
        }


        public Guid SpamToolsId
        {
            get { return m_spamToolsId; }
        }

        public string SpamToolsReceived
        {
            get { return this.m_spamToolsReceived; }
        }

        public string SpamToolsIP
        {
            get { return this.m_spamToolsIP; }
        }

        public string SpamToolsScore
        {
            get { return this.m_spamToolsScore; }
        }

        public string SpamToolsDetectedByTM
        {
            get { return this.m_spamToolsDetectedByTM; }
        }

        public string SpamToolsFrom
        {
            get { return this.m_spamToolsFrom; }
        }

        public string RawMessage
        {
            get { return this.m_rawMessage; }
        }
        #endregion

        #region Constructors
        public QAReader(BaseFile file)
        {
            failed = false;
            m_file = file;

            try
            {
                Parse();
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Possible missing QA file", e);
                failed = true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Parse the QA file.
        /// </summary>
        private void Parse()
        {
            try
            {
                m_contextPropertyList = GenerateContextPropertyList();

                m_rawMessage = ExtractRawMessage(m_file.Stream);

            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.Parse()", e);
                throw;
            }
        }

        #region Parsing context helper methods



        private XmlNode GenerateContextPropertyList()
        {
            // Extract the context as a string.

            try
            {
                string contextString = ExtractContext(m_file.Stream);



                // Load into xml doc.
                XmlDocument contextDoc = new XmlDocument();
                contextDoc.LoadXml(contextString);

                return contextDoc.SelectSingleNode("Context//PropertyList");



            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.GenerateContextPropertyList()", e);
                throw;
            }

        }


        private string GetRefId(XmlNode propertyList)
        {
            try
            {
                XmlNodeList nodeList = propertyList.SelectNodes("Property[@name='OutbreakRefId']");

                if (nodeList.Count == 1)
                {
                    return nodeList[0].Attributes["value"].Value;
                }
                else if (nodeList.Count > 1)
                {
                    List<KeyValuePair<int, string>> RefIDKVPList = new List<KeyValuePair<int, string>>();

                    foreach (XmlNode node in nodeList)
                    {
                        // Match V4 multiline strings
                        string nodeString = node.Attributes["value"].Value;
                        string nodeKeyString = "";
                        int nodeKey = 0;

                        if (nodeString.StartsWith("["))
                        {
                            nodeString = nodeString.Substring(1, nodeString.Length - 1);

                            while (Regex.IsMatch(nodeString, @"^\d"))
                            {
                                nodeKeyString += nodeString[0];
                                nodeString = nodeString.Substring(1, nodeString.Length - 1);
                            }

                            nodeString = nodeString.Substring(1, nodeString.Length - 1);

                            nodeKey = int.Parse(nodeKeyString);

                            RefIDKVPList.Add(new KeyValuePair<int, string>(nodeKey, nodeString));
                        }
                    }

                    RefIDKVPList.Sort((x, y) => x.Key.CompareTo(y.Key));

                    string returnString = "";

                    foreach (KeyValuePair<int, string> KVP in RefIDKVPList)
                    {
                        returnString += KVP.Value;
                    }

                    return returnString;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.GetRefId()", e);
                throw;
            }
        }

        private string GetLastMta(XmlNode propertyList)
        {
            try
            {
                XmlNode node = propertyList.SelectSingleNode("Property[@name='LastMTA']");
                if (node != null)
                {
                    return node.Attributes["value"].Value;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.GetLastMta()", e);
                throw;
            }
        }


        /// <summary>
        /// Grab the reason the message was blocked from the context file
        ///  and convert to nicer format.
        /// </summary>
        /// <param name="propertyList"></param>
        private List<string> GetDetectionReason(XmlNode propertyList)
        {
            try
            {

                List<string> reasons = new List<string>();
                //extract from <Property name="SpamReason" type="string" value="Junk Email Detection, URL Blocklist"/> the different detection reasons
                //If the if there is no reasons, store as "Message Processing Failure".

                if (propertyList.SelectSingleNode("Property[@name='OriginalActionName']") != null)
                {
                    string originalAction = propertyList.SelectSingleNode("Property[@name='OriginalActionName']").Attributes["value"].Value;
                    if (originalAction.Contains("Failure"))
                    {
                        reasons.Add("MPF");
                    }
                    else if (propertyList.SelectSingleNode("Property[@name='PrimaryThreats']") == null)
                    {
                        reasons.Add("Undetected");
                    }
                    else if (propertyList.SelectSingleNode("Property[@name='SpamReason']") != null)
                    {
                        string primaryReason = propertyList.SelectSingleNode("Property[@name='SpamReason']").Attributes["value"].Value;

                        if (primaryReason.Contains("URL Blocklist"))
                        {
                            reasons.Add("SURBL");
                        }
                        if (primaryReason.Contains("Junk Email Detection") || primaryReason.Contains("Bulk Email Detection"))
                        {
                            reasons.Add("Commtouch");
                        }
                        if (primaryReason.Contains("SpamLogic Statistical"))
                        {
                            reasons.Add("ASE");
                        }
                        if (primaryReason.Contains("Bayes"))
                        {
                            reasons.Add("Bayes");
                        }
                    }
                }
                else
                {
                    reasons.Add("MPF");
                }

                return reasons;

            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.GetDetectionReason()", e);
                throw;
            }
        }

        /// <summary>
        /// Returns needle as substring, ignoring all chars in haystack after needle.
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="needle"></param>
        /// <returns></returns>
        private string ExtractReasonFromProperty(string haystack, string needle)
        {
            if (haystack.StartsWith(needle))
            {
                return needle;
            }
            else
            {
                return haystack;
            }
        }

        /// <summary>
        /// Replaces needle with another string and then returns needle as substring.
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="needleToBeReplaced"></param>
        /// <param name="needleToReplace"></param>
        /// <returns></returns>
        private string ExtractReasonFromProperty(string haystack, string needleToBeReplaced, string needleToReplace)
        {
            haystack = haystack.Replace(needleToBeReplaced, needleToReplace);
            return ExtractReasonFromProperty(haystack, needleToReplace);
        }

        #endregion

        #region Context extraction methods
        /// <summary>
        /// Extract context from .qa file and save raw message as .eml file.
        /// </summary>
        private string ExtractContext(MemoryStream messageFile)
        {

            try
            {
                //Check GUID of .qa file
                CheckGUID(messageFile);

                //Eat up all bytes before the starting "<?xml"
                long startPosition = EatJunk(messageFile);


                //Get length of the context from the 4 bytes before "<?xml"
                int length = ReadContextLength(messageFile, startPosition);

                //Get context of .qa file as a string.
                return ReadContext(messageFile, length);



            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ExtractContext()", e);
                throw;
            }
        }

        /// <summary>
        /// Return context of .qa file as a string.
        /// </summary>
        /// <param name="messageFile"></param>
        /// <param name="length">length in bytes of context section</param>
        /// <returns></returns>
        private string ReadContext(MemoryStream messageFile, int length)
        {
            try
            {
                byte[] buffer = new byte[length];
                messageFile.Read(buffer, 0, length);
                return System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ReadContext()", e);
                throw;
            }
        }

        /// <summary>
        /// Returns the length of the context section. 
        /// The 4 bytes prior to the xml opening tag contain the length of the context.
        /// This method accepts a long position variable which is the offset between
        /// beginning of the file and the opening xml tag
        /// </summary>
        /// <param name="messageFile"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private int ReadContextLength(MemoryStream messageFile, long position)
        {
            try
            {
                //Seek depending on the application the qa file is from
                if (this.m_guid == this.m_v3ApplianceGuid || this.m_guid == this.m_mswGuid)
                {
                    byte[] buffer = new byte[4];

                    //Seek backwards 4 bytes
                    messageFile.Seek(position - 4, SeekOrigin.Begin);
                    //Read 4 bytes containing length of context into buffer
                    messageFile.Read(buffer, 0, 4);

                    int length = ByteToInt(buffer);
                    return length;
                }
                else if (this.m_guid == this.m_v4ApplianceGuid)
                {
                    byte[] buffer = new byte[8];

                    //Seek backwards 8 bytes
                    messageFile.Seek(position - 8, SeekOrigin.Begin);
                    //Read 8 bytes containing length of context into buffer
                    messageFile.Read(buffer, 0, 8);

                    int length = ByteToInt(buffer);
                    return length;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ReadContextLength()", e);
                throw;
            }
        }

        /// <summary>
        /// .qa files for the old software and the appliance are different.
        /// This function eats up the bytes prior to the xml opening tag regardless
        /// of what's there.
        /// 
        /// Note: This is probably a very inelegant solution.
        /// </summary>
        /// <param name="messageFile"></param>
        /// <returns></returns>
        private long EatJunk(MemoryStream messageFile)
        {
            try
            {
                //Find beginning of the context file, ignoring all the junk before "<?xml"
                byte[] buffer;
                long startPosition = 0;
                for (int i = 0; i < messageFile.Length; i++)
                {
                    buffer = new byte[1];
                    messageFile.Read(buffer, 0, 1);
                    string s = System.Text.Encoding.UTF8.GetString(buffer);
                    if (buffer[0] == 0x3C)
                    {
                        buffer = new byte[2];
                        messageFile.Read(buffer, 0, 2);
                        if (buffer[0] == 0x3F && buffer[1] == 0x78)
                        {
                            startPosition = messageFile.Position - 3;
                            break;
                        }
                    }
                }

                //Throw exception if "<?xml" was not found
                if (startPosition == 0)
                {
                    Console.WriteLine("QA: Error: Invalid QA file- no <xml tag found");

                }

                return startPosition;
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.EatJunk()", e);
                throw;
            }
        }

        /// <summary>
        /// Check the GUID is valid.
        /// </summary>
        /// <param name="messageFile"></param>
        private void CheckGUID(MemoryStream messageFile)
        {
            try
            {
                //Get GUID from context
                byte[] buffer = new byte[16];
                messageFile.Read(buffer, 0, 16);
                this.m_guid = new Guid(buffer);

                //Check GUID is valid
                if (this.m_guid != this.m_v3ApplianceGuid && this.m_guid != this.m_mswGuid && this.m_guid != this.m_v4ApplianceGuid)
                {
                    Console.WriteLine("QA: Error: Invalid QA file - no valid GUI");

                }


            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.CheckGUID()", e);
                throw;
            }
        }
        #endregion

        #region Raw message extraction methods

        /// <summary>
        /// Save raw message as .eml file.
        /// </summary>
        /// <param name="messageFile"></param>
        private string ExtractRawMessage(MemoryStream messageFile)
        {
            try
            {

                UInt64 offset = ReadNodeNameAndOffset(messageFile);

                messageFile.Seek((Int64)offset, System.IO.SeekOrigin.Begin);

                byte[] buffer = new byte[4];
                messageFile.Read(buffer, 0, 4);	// get the length of the compressed data for this node.

                int length = ByteToInt(buffer);

                buffer = new byte[length];
                messageFile.Read(buffer, 0, length);

                MemoryStream compressedStream = new MemoryStream(buffer);
                MemoryStream decompressedStream = new MemoryStream();
                ZlibStream zOut = new ZlibStream(decompressedStream, Ionic.Zlib.CompressionMode.Decompress);
                CopyStream(compressedStream, zOut);

                // Remove the first few bytes from the stream.
                // These bytes constitute "Received Headers" that were automatically added
                // but we don't want them as they "change" the message.
                // Hashes of the original message would be different before and after the
                // new headers were added.

                byte[] streamByteArray = decompressedStream.ToArray();

                string text = System.Text.Encoding.UTF8.GetString(streamByteArray);

                //string msg = ProcessHeaders(text);
                string msg = ProcessSpamToolsHeaders(text);

                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ExtractRawMessage()", e);
                throw;
            }
        }

        public void SaveRawMessage(string path)
        {
            try
            {
                File.WriteAllText(path, m_rawMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.SaveRawMessage()", e);
                throw;
            }
        }

        public void SaveRawMessageWithJemdHeader(string path)
        {
            try
            {
                if (this.m_guid == this.m_v4ApplianceGuid)
                {
                    var rawMessage = this.m_rawMessage;
                    var splitRawMessage = rawMessage.Split('\n').ToList();

                    var foundFirstWhitespace = false;
                    for (int i = 0; i < splitRawMessage.Count; i++)
                    {
                        if (!foundFirstWhitespace && string.IsNullOrWhiteSpace(splitRawMessage[i]))
                        {
                            foundFirstWhitespace = true;

                            splitRawMessage.Insert(i, "x-msw-jemd-refid: " + this.GetHeaderRefId() + '\r' + '\n' + "x-msw-jemd-lastmta: " + this.LastMta + '\r' + '\n' + '\r' + '\n');
                        }
                    }

                    File.WriteAllText(path, string.Join("\n", splitRawMessage.ToArray()));
                }
                else
                {
                    File.WriteAllText(path, m_rawMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.SaveRawMessageWithJemdHeader()", e);
                throw;
            }
        }

        public string GetHeaderRefId()
        {
            if (this.RefId == string.Empty)
            {
                return "NotChecked";
            }
            else
            {
                return this.RefId;
            }
        }

        /// <summary>
        /// When the message is sent to the gateway received headers are 
        /// automatically added so we need to remove these. We also need to 
        /// extract the X-SpamTools: <guid> property from the message that
        /// was added in the SpamFeedProcessor.
        /// </summary>
        /// <param name="msg">string of the message</param>
        /// <returns>string of the message minus the headers</returns>
        public string ProcessHeaders(string text)
        {
            // Check for X-SpamTools property
            // If this property doesn't exist it means the message didn't 
            // come from SpamFeedProcessor
            try
            {
                int indexOfSpamToolsId = text.IndexOf("X-SpamTools");

                if (indexOfSpamToolsId != -1)
                {
                    // Remove headers up to X-SpamTools property.
                    text = text.Substring(text.IndexOf("X-SpamTools"));

                    string temp = text.Substring(98, 120);

                    // Extract Guid from X-SpamTools: <id> property
                    m_spamToolsId = new Guid(text.Substring(13, 36));

                    // Remove X-SpamTools property
                    text = text.Substring(50);
                }
                else
                {
                    Console.WriteLine("Error: {0} didn't have an X-SpamTools header.", m_file.FileName);

                    // When this exception is caught it will save the failed .qa file out to failed folder.
                    throw new ApplicationException();
                }

                // Return text with headers removed.
                return text;
            }
            catch (Exception e)
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ProcessHeaders()", e);
                throw;
            }
        }

        public string ProcessSpamToolsHeaders(string text)
        {
            List<string> headers = new List<string>()
            {
                "X-SpamTools",
                "X-SpamTools-ReceivedTime",
                "X-SpamTools-SourceIP",
                "X-SpamTools-TrustManagerScore",
                "X-SpamTools-DetectedByTrustManager",
                "X-SpamTools-From"
            };

            List<string> lines = new List<string>();
            using (StringReader reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            List<int> linesToRemove = new List<int>();

            int i = 0;
            bool done = false;
            foreach (string line in lines)
            {
                foreach (string header in headers)
                {
                    Match match = Regex.Match(line, @"^" + header + ":[ ]*(.*)$");

                    if (match.Success)
                    {
                        switch (header)
                        {
                            case "X-SpamTools":
                                this.m_spamToolsId = new Guid(match.Groups[1].Value);
                                break;
                            case "X-SpamTools-ReceivedTime":
                                this.m_spamToolsReceived = match.Groups[1].Value;
                                break;
                            case "X-SpamTools-SourceIP":
                                this.m_spamToolsIP = match.Groups[1].Value;
                                break;
                            case "X-SpamTools-TrustManagerScore":
                                this.m_spamToolsScore = match.Groups[1].Value;
                                break;
                            case "X-SpamTools-DetectedByTrustManager":
                                this.m_spamToolsDetectedByTM = match.Groups[1].Value;
                                break;
                            case "X-SpamTools-From":
                                this.m_spamToolsFrom = match.Groups[1].Value;
                                break;
                        }

                        linesToRemove.Add(i);

                        if (linesToRemove.Count == headers.Count)
                        {
                            break;
                            //done = true;
                        }
                    }

                    if (done)
                        break;
                }
            }

            return String.Join(Environment.NewLine, lines);
        }

        #endregion

        #region Mental stream/byte manipulation methods
        /// <summary>
        /// Converts a MemoryStream to a string. Makes some assumptions about the content of the stream. 
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private String MemoryStreamToString(System.IO.MemoryStream ms)
        {
            try
            {
                byte[] ByteArray = ms.ToArray();
                return System.Text.Encoding.ASCII.GetString(ByteArray);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error thrown in MemoryStreamToString()", e);
                throw;
            }
        }

        private void CopyStream(System.IO.Stream src, System.IO.Stream dest)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int len = src.Read(buffer, 0, buffer.Length);
                while (len > 0)
                {
                    dest.Write(buffer, 0, len);
                    len = src.Read(buffer, 0, buffer.Length);
                }
                dest.Flush();
            }
            catch
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.CopyStream()");
                throw;
            }
        }

        private UInt64 ReadNodeNameAndOffset(MemoryStream message_file)
        {
            try
            {
                UInt64 offset;
                System.Text.StringBuilder name = new System.Text.StringBuilder();
                Byte[] wc = new Byte[2];
                char c = '\0';
                do
                {
                    c = '\0';
                    message_file.Read(wc, 0, 2);
                    for (int i = 0; i < 2; i++)
                    {
                        UInt16 b = wc[i];
                        b &= 0x00ff;	// make sure we don't have any garbage in the character.
                        b <<= i * 8;		// shift it along to the correct position (swap the bits)
                        c |= (char)b;	// cast and assign to a char.
                    }
                    if (c != '\0')
                    {
                        name.Append(c);
                    }
                }
                while (c != '\0');

                byte[] buffer = new byte[8];
                message_file.Read(buffer, 0, 8);

                offset = ByteToInt64(buffer);
                return offset;
            }
            catch
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ReadNodeNameAndOffSet()");
                throw;
            }
        }

        /// <summary>
        /// private helper function to reverse a byte[] into a little endian offset/length value.
        /// </summary>
        /// <param name="array">the buffer</param>
        /// <returns>the converted integer representation.</returns>
        private int ByteToInt(byte[] array)
        {
            try
            {
                int val = 0;
                for (int i = 0; i <= 3; i++)
                {
                    int tmp = array[i];
                    tmp <<= i * 8;
                    val |= tmp;
                }
                return val;
            }
            catch
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ByteToInt()");
                throw;
            }
        }

        /// <summary>
        /// private helper function to reverse a byte[] into a little endian offset/length value.
        /// </summary>
        /// <param name="array">the buffer</param>
        /// <returns>the converted UInt64 representation.</returns>
        private UInt64 ByteToInt64(Byte[] array)
        {
            try
            {
                UInt64 val = 0;
                for (int i = 0; i <= 7; i++)
                {
                    UInt32 tmp = array[i];
                    tmp <<= i * 8;
                    val |= tmp;
                }
                return val;
            }
            catch
            {
                Console.WriteLine("QA: Error: Error thrown in QAReader.ByteToInt64()");
                throw;
            }
        }

        public void Dispose()
        {

            if (m_file != null)
            {
                m_file.Dispose();
            }

            if (m_contextPropertyList != null)
            {
                m_contextPropertyList = null;
            }

            if (m_file.Stream != null)
            {
                m_file.Stream.Dispose();
            }
        }
        #endregion
        #endregion
    }
}

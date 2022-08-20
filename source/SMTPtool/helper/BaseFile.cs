using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Core
{
    public class BaseFile
    {
        #region Fields
        string m_fileName;
        string m_filePath;
        string m_text;
        byte[] m_bytes;
        MemoryStream m_stream;
        #endregion

        #region Properties
        public string FileName
        {
            get { return this.m_fileName; }
        }

        /// <summary>
        /// The name of the file excluding extension.
        /// </summary>
        public string FileNameWithoutExtension
        {
            get { return Path.GetFileNameWithoutExtension(this.m_fileName); }
        }

        /// <summary>
        /// The absolute path to the file.
        /// </summary>
        public string FilePath
        {
            get { return this.m_filePath; }
        }

        /// <summary>
        /// Reads and returns the text of the file.
        /// </summary>
        public string Text
        {
            get
            {
                int attempt = 0;

                while (this.m_text == null && attempt < 5)
                {

                    try
                    {
                        this.m_text = File.ReadAllText(this.m_filePath);
                        attempt++;
                    }
                    catch
                    {
                        attempt++;
                    }
                }

                if (this.m_text == null)
                {
                    this.m_text = " ";
                }

                return this.m_text;
            }
        }

        /// <summary>
        /// Reads and returns the bytes of the file.
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                if (this.m_bytes == null)
                {
                    int error = 0;

                    do
                    {
                        try
                        {
                            this.m_bytes = File.ReadAllBytes(this.m_filePath);
                        }
                        catch
                        {
                            if (error < 4)
                            {
                                error++;
                            }
                            else
                            {
                                throw;
                            }
                        }

                    }
                    while (this.m_bytes == null);
                }

                return this.m_bytes;
            }
        }

        /// <summary>
        /// Returns the file represented as a MemoryStream.
        /// </summary>
        public MemoryStream Stream
        {
            get
            {
                if (this.m_stream == null)
                {
                    try
                    {
                        this.m_stream = new MemoryStream(this.Bytes);
                    }
                    catch
                    {
                        if (this.m_stream != null)
                        {
                            this.m_stream.Dispose();
                        }

                        throw;
                    }
                }

                return this.m_stream;
            }
        }
        #endregion

        #region Constructors
        public BaseFile(string path)
        {
            try
            {
                this.m_filePath = path;
                this.m_fileName = Path.GetFileName(this.m_filePath);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            this.m_fileName = null;
            this.m_text = null;
            this.m_bytes = null;

            if (this.m_stream != null)
            {
                this.m_stream.Dispose();
            }
        }
        #endregion
    }
}

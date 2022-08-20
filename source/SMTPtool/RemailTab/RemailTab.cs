using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMTPtool;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;
using HLIB.MailFormats;
using System.Threading;
using SMTPtestTool;
using System.ComponentModel;
using Microsoft.Win32;

namespace SMTPtestTool
{
    public class RemailTab
    {
        Main _linkToMain;

        public List<string> serverList = new List<string>();
        public List<string> mailFromList = new List<string>();
        public List<string> mailToList = new List<string>();

        public String mailPath;

        public Remailer myRemailer;

        ContextMenuStrip mnRemailFile;
        ContextMenuStrip mnRemailFolder;

        private String fileToCopy;
        private ToolStripMenuItem mnRemailFilePaste;
        private ToolStripMenuItem mnRemailFolderPaste;

        String mailboxPath;
        static ImageList _imageList; //holds treeView icons
        public TreeNode previousSelectedNode = null;

        public Boolean txtMailViewIsDirty = false;
        public Boolean txtMailViewCanBeDirty = false;

        public Boolean sendNext;

        //constructor
        public RemailTab(Main _linkToMain)
        {
            this._linkToMain = _linkToMain;

            createDirectories();
            mailboxPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox";

            //init tree view
            _linkToMain.treeViewMails.DrawMode = TreeViewDrawMode.OwnerDrawText;
            _linkToMain.treeViewMails.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_DrawNode);
            _linkToMain.treeViewMails.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.nodeClicked);
            _linkToMain.treeViewMails.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewItemExpanded);
            _linkToMain.treeViewMails.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewItemCollapsed);
            _linkToMain.treeViewMails.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewItemDoubleClicked);
            _linkToMain.treeViewMails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeViewKeyDown);

            mnRemailFile = new ContextMenuStrip();
            mnRemailFile.Items.Add(new ToolStripMenuItem("Open", null, new System.EventHandler(this.mnuSessionFileOpen_Click), "openMail"));
            mnRemailFile.Items.Add(new ToolStripMenuItem("Copy", null, new System.EventHandler(this.mnuSessionFileCopy_Click), "copyMail"));
            mnRemailFilePaste = new ToolStripMenuItem("Paste", null, new System.EventHandler(this.mnuSessionFilePaste_Click), "pasteMail");
            mnRemailFilePaste.Enabled = false;
            mnRemailFile.Items.Add(mnRemailFilePaste);
            mnRemailFile.Items.Add(new ToolStripMenuItem("Delete", null, new System.EventHandler(this.mnuSessionFileDelete_Click), "deleteMail"));
            mnRemailFile.Items.Add(new ToolStripMenuItem("Rename", null, new System.EventHandler(this.mnuSessionFileRename_Click), "renameMail"));

            mnRemailFolder = new ContextMenuStrip();
            mnRemailFolder.Items.Add(new ToolStripMenuItem("Open directory", null, new System.EventHandler(this.mnuSessionFolderOpen_Click), "openDirectory"));
            mnRemailFolder.Items.Add(new ToolStripMenuItem("Delete", null, new System.EventHandler(this.mnuSessionFolderDelete_Click), "deleteFolder"));
            mnRemailFolderPaste = new ToolStripMenuItem("Paste", null, new System.EventHandler(this.mnuSessionFilePaste_Click), "pasteMail");
            mnRemailFolderPaste.Enabled = false;
            mnRemailFolder.Items.Add(mnRemailFolderPaste);

            buildMailTreeView(_linkToMain.treeViewMails, mailboxPath);
            _linkToMain.treeViewMails.ExpandAll();

            convertMessages();

            //fileWatcher to check for filesystem changes in the mailbox folder
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = mailboxPath;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnChanged);

            _linkToMain.txtMailView.TextChanged += new System.EventHandler(TextChanged);

        }


        public void triggerTreeViewRebuild()
        {
            buildMailTreeView(_linkToMain.treeViewMails, mailboxPath);
        }


        private void buildMailTreeView(TreeView treeView, string path)
        {
            createDirectories();
            if (!File.Exists(fileToCopy))
            {
                mnRemailFilePaste.Enabled = false;
                mnRemailFolderPaste.Enabled = false;
            }

            //check which directory nodes are expanded before the rebuild
            Dictionary<String, Boolean> directoryNodes = new Dictionary<String, Boolean>();
            foreach (TreeNode currentNode in treeView.Nodes)
            {
                directoryNodes.Add(currentNode.Tag.ToString(), currentNode.IsExpanded);
            }

            //check if something was selected
            //so that it can be selected after the rebuild
            String selectedNode = "";
            if (!(treeView.SelectedNode == null)) selectedNode = treeView.SelectedNode.Tag.ToString();

            //clear nodes and rebuild the treeview with new items
            treeView.Nodes.Clear();
            treeView.ImageList = RemailTab.ImageList;
            var rootDirectory = new DirectoryInfo(path);
            foreach (var directory in rootDirectory.GetDirectories())
            {
                //var childDirectoryNode = new TreeNode(directory.Name) { Tag = "directory" };
                var childDirectoryNode = new TreeNode(directory.Name) { Tag = directory.FullName };
                childDirectoryNode.ImageKey = "folder";
                childDirectoryNode.SelectedImageKey = "folder";
                String currentPath = directory.FullName;
                //Debug.WriteLine("Directory: " + currentPath);

                foreach (var file in Directory.GetFiles(currentPath, "*.eml", SearchOption.AllDirectories))
                {
                    try
                    {
                        TreeNode mailNode = new TreeNode(Path.GetFileName(file)) { Tag = file };
                        mailNode.ImageKey = "mail";
                        mailNode.SelectedImageKey = "mail";
                        childDirectoryNode.Nodes.Add(mailNode);

                    }
                    catch (Exception excpection)
                    {
                        MessageBox.Show(excpection.Message, "Attachment I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                   
                }
                treeView.Nodes.Add(childDirectoryNode);
            }

            //iterate over all nodes to check if the previously selected node still exists
            //if yes, select it
            //and expand a directory node in case it was selected before the rebuild
            foreach (TreeNode currentNode in treeView.Nodes)
            {
                if (currentNode.Tag.ToString().Equals(selectedNode))
                {
                    treeView.SelectedNode = currentNode;
                    // break;
                }
                if (directoryNodes.ContainsKey(currentNode.Tag.ToString()))
                {
                    //Debug.WriteLine("Key: " + currentNode.Tag.ToString());
                    //Debug.WriteLine("Value: " + directoryNodes[currentNode.Tag.ToString()]);

                    if (directoryNodes[currentNode.Tag.ToString()])
                    {
                        currentNode.Expand();
                    }
                }

                foreach (TreeNode childNode in currentNode.Nodes)
                {
                    if (childNode.Tag.ToString().Equals(selectedNode))
                    {
                        treeView.SelectedNode = childNode;
                        break;
                    }
                }
            }
        }

        private void readFileAsync()
        {
            try
            {
                StreamReader streamReader = new StreamReader(mailPath, Encoding.UTF8);

                String text = streamReader.ReadToEnd();
                _linkToMain.Invoke((MethodInvoker)delegate ()
                {
                    //EMLReader myReader = new EMLReader(text);
                    //Debug.WriteLine(myReader.Body);
                    
                    _linkToMain.txtMailView.Text = text;
                    txtMailViewCanBeDirty = true;
                    _linkToMain.txtMailView.ReadOnly = false;

                });

                streamReader.Close();
            }
            catch (Exception excpection)
            {
                MessageBox.Show(excpection.Message, "Attachment I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Debug.WriteLine("Filewatcher triggered");
            createDirectories();
            _linkToMain.Invoke((MethodInvoker)delegate ()
            {
                _linkToMain.txtMailView.Text = "";
                buildMailTreeView(_linkToMain.treeViewMails, mailboxPath);
            });
            convertMessages();
        }

        private void convertMessages()
        {
            createDirectories();
            var rootDirectory = new DirectoryInfo(mailboxPath);
            foreach (var directory in rootDirectory.GetDirectories())
            {
                String currentPath = directory.FullName;

                foreach (var file in Directory.GetFiles(currentPath, "*.qa", SearchOption.AllDirectories))
                {
                    // Debug.WriteLine("QA PATH: " + file);

                    // Save the qa file as an eml file
                    Core.QAReader reader = new Core.QAReader(new Core.BaseFile(file));

                    try {
                        reader.SaveRawMessageWithJemdHeader(Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".eml");
                        Debug.WriteLine("OUTPUT FILE: " + Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".eml");
                    }
                    catch (Exception excpection)
                    {
                        MessageBox.Show(excpection.Message, "Attachment I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                                       

                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception)
                    {

                    }

                }
            }
        }

        #region actions
        /// 
        /// ACTIONS
        /// 
        ///

        private void renameFile(String pathToFolder)
        {
            String input = Microsoft.VisualBasic.Interaction.InputBox("Type in new name for " + Path.GetFileName(mailPath), "Rename message", Path.GetFileName(mailPath), -1, -1);
            Debug.WriteLine(Path.GetExtension(input));

            if (!input.Equals(""))
            {
                if (!Path.GetExtension(input).Equals(".eml"))
                {
                    Debug.WriteLine("OIOIOI NO extension");
                    input = input + ".eml";
                }

                if (File.Exists(Path.GetDirectoryName(mailPath) + "\\" + input))
                {
                    MessageBox.Show("The name already exists \r\n\r\n " + input, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    Debug.WriteLine("FILE MOVED TO: " + Path.GetDirectoryName(mailPath) + "\\" + input);
                    System.IO.File.Move(mailPath, Path.GetDirectoryName(mailPath) + "\\" + input);
                    _linkToMain.lblRemailSize.Text = "";
                    _linkToMain.btnRemail.Enabled = false;
                }
            }
        }

        private void TextChanged(object Sender, EventArgs e)
        {

            //  Debug.WriteLine("TEXT CHANGED");
            if (txtMailViewCanBeDirty)
            {
                if (txtMailViewIsDirty)
                {
                    _linkToMain.btnRemailSaveMail.Enabled = true;
                }
                else
                {
                    txtMailViewIsDirty = true;
                    _linkToMain.btnRemailSaveMail.Enabled = true;
                }
            }
        }

        private void deleteFolder(String pathToFolder)
        {
            if (pathToFolder.Equals(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Import") ||
                pathToFolder.Equals(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Outbox") ||
                pathToFolder.Equals(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Inbox"))
            {
                MessageBox.Show("The folders Import, Inbox and Outbox are system mailboxes and cannot be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(pathToFolder);

                if (MessageBox.Show("Are you sure you want to delete the folder \r\n\r\n" + System.IO.Path.GetFileName(pathToFolder), "Delete folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    di.Delete(true);
                    triggerTreeViewRebuild();
                    _linkToMain.btnRemail.Enabled = false;
                }
            }
        }

        private void deleteFile(String pathToFile)
        {
            if (MessageBox.Show("Are you sure you want to delete \r\n\r\n" + System.IO.Path.GetFileName(pathToFile), "Delete mail", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                _linkToMain.txtMailView.Text = "";
                System.IO.File.Delete(pathToFile);
                buildMailTreeView(_linkToMain.treeViewMails, mailboxPath);
                _linkToMain.btnRemail.Enabled = false;
                _linkToMain.btnRemailSaveMail.Enabled = false;
                _linkToMain.lblRemailSize.Text = "";

            }
        }

        private void openFile(String pathToFile)
        {
            Process.Start(pathToFile);
        }

        private void openFolder(String pathToFolder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo { Arguments = pathToFolder, FileName = "explorer.exe" };
            Process.Start(startInfo);
        }

        private void copyFile(String sourcePath, String target)
        {
            if (File.Exists(sourcePath))
            {
                String sourceFileNameWithoutExt = Path.GetFileNameWithoutExtension(sourcePath);
                String sourceFileName = Path.GetFileName(sourcePath);

                String targetDirectory;
                String finalTargetPath;

                FileAttributes attr = File.GetAttributes(@"" + target);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    targetDirectory = target;
                }
                else
                {
                    targetDirectory = Path.GetDirectoryName(target);
                }

                if (File.Exists(targetDirectory + "\\" + sourceFileName))
                {
                    if (File.Exists(targetDirectory + "\\" + sourceFileNameWithoutExt + " - Copy.eml"))
                    {
                        String fullTargetPath = targetDirectory + "\\" + sourceFileNameWithoutExt + " - Copy.eml";
                        int counter = 2;
                        while (File.Exists(fullTargetPath))
                        {
                            fullTargetPath = targetDirectory + "\\" + sourceFileNameWithoutExt + " - Copy" + counter + ".eml";
                            counter++;
                        }
                        finalTargetPath = fullTargetPath;
                    }
                    else
                    {
                        finalTargetPath = targetDirectory + "\\" + sourceFileNameWithoutExt + " - Copy.eml";
                    }
                }
                else
                {
                    finalTargetPath = targetDirectory + "\\" + sourceFileName;
                }
                File.Copy(sourcePath, finalTargetPath);

                triggerTreeViewRebuild();

                //select the pasted mail
                foreach (TreeNode currentNode in _linkToMain.treeViewMails.Nodes)
                {
                    if (currentNode.Tag.ToString().Equals(finalTargetPath))
                    {
                        _linkToMain.treeViewMails.SelectedNode = currentNode;
                        break;
                    }
                    foreach (TreeNode childNode in currentNode.Nodes)
                    {
                        if (childNode.Tag.ToString().Equals(finalTargetPath))
                        {
                            _linkToMain.treeViewMails.SelectedNode = childNode;
                            break;
                        }
                    }
                }
                _linkToMain.btnRemail.Enabled = true;
                txtMailViewCanBeDirty = false;
                txtMailViewIsDirty = false;
                _linkToMain.btnRemailSaveMail.Enabled = false;
                _linkToMain.lblRemailSize.Text = "";
            }
        }

        #endregion

        #region mouseClicks
        /// 
        /// MOUSE CLICKS
        /// 
        ///
        private void nodeClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {

                txtMailViewCanBeDirty = false;
                _linkToMain.txtMailView.Text = "";
                txtMailViewIsDirty = false;
                _linkToMain.txtMailView.ReadOnly = true;
                _linkToMain.btnRemailSaveMail.Enabled = false;
                _linkToMain.lblRemailSize.Text = "";

                //Debug.WriteLine("Item clicked: " + e.Node.Tag);
                // get the file attributes for file or directory
                FileAttributes attr = File.GetAttributes(@"" + e.Node.Tag);

                if (e.Button == MouseButtons.Right)
                {
                    // Point where the mouse is clicked.
                    Point p = new Point(e.X, e.Y);

                    // Get the node that the user has clicked.
                    TreeNode node = _linkToMain.treeViewMails.GetNodeAt(p);
                    //Debug.WriteLine("selected NODE: " + node.Tag);
                    if (node != null)
                    {
                        // Select the node the user has clicked.
                        _linkToMain.treeViewMails.SelectedNode = node;
                        mnRemailFile.Show(_linkToMain.treeViewMails, p);

                        // Find the appropriate ContextMenu depending on the selected node.
                        if (attr.HasFlag(FileAttributes.Directory))
                        {
                            mnRemailFolder.Show(_linkToMain.treeViewMails, p);
                        }
                        else
                        {
                            mnRemailFile.Show(_linkToMain.treeViewMails, p);
                        }
                    }
                }

                //detect whether its a directory or file
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    //directory
                    _linkToMain.txtMailView.Text = "Click Remail to send all messages in the selected folder.";

                    _linkToMain.btnRemail.Enabled = true;
                }
                else
                {
                    //file  
                    this.mailPath = e.Node.Tag.ToString();
                    long lengthInKB = new System.IO.FileInfo(this.mailPath).Length / 100 / 8;

                    //check file size
                    //Debug.WriteLine("File Size: " + lengthInKB + " KB");
                    if (lengthInKB < 20000)
                    {
                        Thread LoadThread = new Thread(new ThreadStart(readFileAsync));
                        LoadThread.Start();

                        _linkToMain.btnRemail.Enabled = true;
                        if (lengthInKB == 0)
                        {
                            lengthInKB = 1;
                        }
                        _linkToMain.lblRemailSize.Text = "Size: " + lengthInKB + " KB";
                    }
                    else
                    {
                        _linkToMain.Invoke((MethodInvoker)delegate ()
                        {
                            _linkToMain.txtMailView.Text = lengthInKB + " KB - Maximum file size exceeded for displaying the content. Maximum supported file size 20000 KB";
                        });
                    }
                }
            }
            catch (Exception)
            {
                triggerTreeViewRebuild();

            }
        }

        private void mnuSessionFolderDelete_Click(object sender, EventArgs e)
        {
            deleteFolder(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }
        private void mnuSessionFolderOpen_Click(object sender, EventArgs e)
        {
            openFolder(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }
        private void mnuSessionFileDelete_Click(object sender, EventArgs e)
        {
            deleteFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }
        private void mnuSessionFileOpen_Click(object sender, EventArgs e)
        {
            openFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }
        private void mnuSessionFileCopy_Click(object sender, EventArgs e)
        {
            fileToCopy = _linkToMain.treeViewMails.SelectedNode.Tag.ToString();
            mnRemailFolderPaste.Enabled = true;
            mnRemailFilePaste.Enabled = true;
        }
        private void mnuSessionFilePaste_Click(object sender, EventArgs e)
        {
            copyFile(fileToCopy, _linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }
        private void mnuSessionFileRename_Click(object sender, EventArgs e)
        {
            renameFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }

        public void treeViewItemDoubleClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            FileAttributes attr = File.GetAttributes(@"" + e.Node.Tag);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                openFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
            }
        }

        public void treeViewItemExpanded(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageKey = "Openfolder";
            e.Node.SelectedImageKey = "Openfolder";
        }
        public void treeViewItemCollapsed(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageKey = "folder";
            e.Node.SelectedImageKey = "folder";
        }

        #endregion

        #region KeyPress

        private void treeViewKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            String path = "";
            FileAttributes attr;
            if (!(_linkToMain.treeViewMails.SelectedNode == null))
            {
                path = _linkToMain.treeViewMails.SelectedNode.Tag.ToString();
                attr = File.GetAttributes(path);
                if (e.KeyCode == Keys.Enter)
                {
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        openFolder(path);
                    }
                }
                if (e.KeyData == (Keys.V | Keys.Control))
                {
                    Debug.WriteLine("CONTROL + V pressed");
                    copyFile(fileToCopy, _linkToMain.treeViewMails.SelectedNode.Tag.ToString());
                }
                if (e.KeyData == (Keys.C | Keys.Control))
                {
                    Debug.WriteLine("CONTROL + C pressed");
                    fileToCopy = _linkToMain.treeViewMails.SelectedNode.Tag.ToString();
                    mnRemailFolderPaste.Enabled = true;
                    mnRemailFilePaste.Enabled = true;
                }
                if (e.KeyCode == Keys.Delete)
                {
                    Debug.WriteLine("DELETE pressed");

                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        deleteFolder(path);
                    }
                    else
                    {
                        deleteFile(path);
                    }
                }
                if (e.KeyCode == Keys.F2)
                {
                    Debug.WriteLine("F2 pressed");
                    renameFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
                }

            }

            //e.Handled = true;
            e.SuppressKeyPress = true;

            //Debug.WriteLine("KEYDATA: " + e.KeyData);
            //Debug.WriteLine("KeyCode: " + e.KeyCode);
            //Debug.WriteLine("Keyvalue: " + e.KeyValue);            
        }
        #endregion

        #region Buttons
        /// 
        /// BUTTONS
        /// 
        ///
        public void btnOpenMailClicked()
        {
            openFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }

        public void btnDeleteMailClicked()
        {
            deleteFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }

        public void btnRemailClicked()
        {
            FileAttributes attr = File.GetAttributes(@"" + _linkToMain.treeViewMails.SelectedNode.Tag);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                //directory
                _linkToMain.txtMailView.Text = "Click Remail to send all messages in the selected folder.";

                //check if there any subnodes/mails in the selected folder
                if (_linkToMain.treeViewMails.SelectedNode.Nodes.Count > 0)
                {
                    _linkToMain.btnRemail.Enabled = true;

                    if (_linkToMain.cbxRemailIP.Text.Equals(""))
                    {
                        _linkToMain.cbxRemailIP.Text = "192.168.0.1";
                    }

                    try { int.Parse(_linkToMain.txtRemailPort.Text); }
                    catch { _linkToMain.txtRemailPort.Text = "25"; }

                    _linkToMain.txtRemailOutput.AppendText(DateTime.Now.ToString("MMM dd HH:mm:ss") + " - Connecting to " + _linkToMain.cbxRemailIP.Text + " on port" + int.Parse(_linkToMain.txtRemailPort.Text) + "\r\n", Color.Red);

                    foreach (TreeNode myNode in _linkToMain.treeViewMails.SelectedNode.Nodes)
                    {
                        String mailPath = myNode.Tag.ToString();
                        StreamReader streamReader = new StreamReader(mailPath, Encoding.UTF8);
                        String text = streamReader.ReadToEnd();
                        streamReader.Close();
                        myRemailer = new Remailer(_linkToMain);
                        myRemailer.sendSingle = false;
                        myRemailer.fullMailBody = text;
                        //myRemailer.mailPath = _linkToMain.treeViewMails.SelectedNode.Tag.ToString();
                        myRemailer.connect();
                    }
                }
                else
                {
                    _linkToMain.btnRemail.Enabled = false;
                }

            }
            else
            //file
            {
                String mailPath = _linkToMain.treeViewMails.SelectedNode.Tag.ToString();
                myRemailer = new Remailer(_linkToMain);
                myRemailer.sendSingle = true;
                myRemailer.connect();
            }

        }

        internal void btnRenameClicked()
        {
            renameFile(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }

        internal void btnNewFolderClicked()
        {
            String folderName = Microsoft.VisualBasic.Interaction.InputBox("Type in folder name " + Path.GetFileName(mailPath), "New folder", "new folder", -1, -1);
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\" + folderName);
            triggerTreeViewRebuild();
        }

        public void btnSaveClicked()
        {
            File.WriteAllText(_linkToMain.treeViewMails.SelectedNode.Tag.ToString(), _linkToMain.txtMailView.Text);
            _linkToMain.btnRemailSaveMail.Enabled = false;
            txtMailViewIsDirty = false;
        }

        internal void btnOpenFolderClicked()
        {
            openFolder(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }

        internal void btnDeleteFolderClicked()
        {
            deleteFolder(_linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }

        internal void btnCopyMailClicked()
        {
            copyFile(fileToCopy, _linkToMain.treeViewMails.SelectedNode.Tag.ToString());
        }

        #endregion

        #region "helper"

        public void createDirectories()
        {
            //create all important mail directories in case they are not here
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox");
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Import");
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Inbox");
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\mailbox\\Outbox");
        }

        //Determines a text file's encoding by analyzing its byte order mark (BOM).
        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }
            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        //init image list for treeView Icons
        public static ImageList ImageList
        {
            get
            {
                if (_imageList == null)
                {
                    _imageList = new ImageList();
                    _imageList.Images.Add("mail", Properties.Resources.mailIcon);
                    _imageList.Images.Add("winFolder", Properties.Resources.windowsFolderIcon);
                    _imageList.Images.Add("openFolder", Properties.Resources.openFolder);
                    _imageList.Images.Add("folder", Properties.Resources.folder);
                    _imageList.Images.Add("mainSymbol", Properties.Resources.mail);
                }
                return _imageList;
            }
        }

        //keep the selected treeView item nicely selected even if treeView looses focus
        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //return;
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var unfocused = !e.Node.TreeView.Focused;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                Debug.WriteLine("TREE VIEW DRAW NOTE TRIGGERED");
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }


        }
        #endregion


    }
}

using System.Windows.Forms;
namespace SMTPtool
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnSend = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lvlTo = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.lblSubject = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.lblBody = new System.Windows.Forms.Label();
            this.btnAttach = new System.Windows.Forms.Button();
            this.nrcCount = new System.Windows.Forms.NumericUpDown();
            this.lblCount = new System.Windows.Forms.Label();
            this.lbAttachments = new System.Windows.Forms.ListBox();
            this.lblAttachments = new System.Windows.Forms.Label();
            this.btnDelAttachment = new System.Windows.Forms.Button();
            this.btnPing = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxServer = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbSaveInOutbox = new System.Windows.Forms.CheckBox();
            this.btnDelAttachmentAll = new System.Windows.Forms.Button();
            this.cbxTo = new System.Windows.Forms.ComboBox();
            this.cbxFrom = new System.Windows.Forms.ComboBox();
            this.nrcThreadCount = new System.Windows.Forms.NumericUpDown();
            this.lblThreadCount = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.smtpTabPage = new System.Windows.Forms.TabControl();
            this.mailTabPage = new System.Windows.Forms.TabPage();
            this.RemailTabPage = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblRemailSize = new System.Windows.Forms.Label();
            this.txtRemailOutput = new System.Windows.Forms.RichTextBox();
            this.btnRemailSaveMail = new System.Windows.Forms.Button();
            this.txtMailView = new System.Windows.Forms.TextBox();
            this.treeViewMails = new System.Windows.Forms.TreeView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbxRemailFrom = new System.Windows.Forms.ComboBox();
            this.cbxRemailTo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRemail = new System.Windows.Forms.Button();
            this.txtRemailPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxRemailIP = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnSessionSendLine = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lbxSessionHistory = new System.Windows.Forms.ListBox();
            this.btnSessionResend = new System.Windows.Forms.Button();
            this.btnSessionClearCommand = new System.Windows.Forms.Button();
            this.txtSessionOutput = new System.Windows.Forms.RichTextBox();
            this.txtSessionCommand = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cbxSessionTo = new System.Windows.Forms.ComboBox();
            this.cbxSessionFrom = new System.Windows.Forms.ComboBox();
            this.btnSessionReset = new System.Windows.Forms.Button();
            this.btnSessionHelo = new System.Windows.Forms.Button();
            this.btnSessionFrom = new System.Windows.Forms.Button();
            this.btnSessionDot = new System.Windows.Forms.Button();
            this.btnSessionRcpt = new System.Windows.Forms.Button();
            this.btnSessionData = new System.Windows.Forms.Button();
            this.btnSessionQuit = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnSessionOpenNewWindow = new System.Windows.Forms.Button();
            this.cbxSessionServer = new System.Windows.Forms.ComboBox();
            this.btnSessionConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxSessionPort = new System.Windows.Forms.TextBox();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelAuthorInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelMail = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelUpdateInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelLink = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTipUpdateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtSessionEhlo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nrcCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nrcThreadCount)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.smtpTabPage.SuspendLayout();
            this.mailTabPage.SuspendLayout();
            this.RemailTabPage.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(466, 24);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(93, 23);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(15, 19);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(808, 151);
            this.txtLog.TabIndex = 10;
            this.txtLog.Text = "";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(13, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(33, 13);
            this.lblFrom.TabIndex = 3;
            this.lblFrom.Text = "From:";
            // 
            // lvlTo
            // 
            this.lvlTo.AutoSize = true;
            this.lvlTo.Location = new System.Drawing.Point(13, 53);
            this.lvlTo.Name = "lvlTo";
            this.lvlTo.Size = new System.Drawing.Size(23, 13);
            this.lvlTo.TabIndex = 4;
            this.lvlTo.Text = "To:";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(12, 27);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 7;
            this.lblServer.Text = "Server:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(237, 27);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 9;
            this.lblPort.Text = "Port:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(268, 27);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(37, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "25";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtBody
            // 
            this.txtBody.Location = new System.Drawing.Point(16, 150);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBody.Size = new System.Drawing.Size(543, 186);
            this.txtBody.TabIndex = 6;
            this.txtBody.Text = "This is my test message body.";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Location = new System.Drawing.Point(12, 81);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(46, 13);
            this.lblSubject.TabIndex = 13;
            this.lblSubject.Text = "Subject:";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(16, 100);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(543, 20);
            this.txtSubject.TabIndex = 5;
            this.txtSubject.Text = "This is my test message subject";
            // 
            // lblBody
            // 
            this.lblBody.AutoSize = true;
            this.lblBody.Location = new System.Drawing.Point(16, 127);
            this.lblBody.Name = "lblBody";
            this.lblBody.Size = new System.Drawing.Size(34, 13);
            this.lblBody.TabIndex = 15;
            this.lblBody.Text = "Body:";
            // 
            // btnAttach
            // 
            this.btnAttach.Location = new System.Drawing.Point(578, 117);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(58, 23);
            this.btnAttach.TabIndex = 16;
            this.btnAttach.Text = "Add";
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // nrcCount
            // 
            this.nrcCount.Location = new System.Drawing.Point(390, 21);
            this.nrcCount.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nrcCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrcCount.Name = "nrcCount";
            this.nrcCount.Size = new System.Drawing.Size(48, 20);
            this.nrcCount.TabIndex = 18;
            this.nrcCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(273, 24);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(106, 13);
            this.lblCount.TabIndex = 19;
            this.lblCount.Text = "Number of messages";
            // 
            // lbAttachments
            // 
            this.lbAttachments.FormattingEnabled = true;
            this.lbAttachments.Location = new System.Drawing.Point(578, 150);
            this.lbAttachments.Name = "lbAttachments";
            this.lbAttachments.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAttachments.Size = new System.Drawing.Size(245, 186);
            this.lbAttachments.TabIndex = 20;
            // 
            // lblAttachments
            // 
            this.lblAttachments.AutoSize = true;
            this.lblAttachments.Location = new System.Drawing.Point(575, 98);
            this.lblAttachments.Name = "lblAttachments";
            this.lblAttachments.Size = new System.Drawing.Size(69, 13);
            this.lblAttachments.TabIndex = 21;
            this.lblAttachments.Text = "Attachments:";
            // 
            // btnDelAttachment
            // 
            this.btnDelAttachment.Enabled = false;
            this.btnDelAttachment.Location = new System.Drawing.Point(642, 117);
            this.btnDelAttachment.Name = "btnDelAttachment";
            this.btnDelAttachment.Size = new System.Drawing.Size(58, 23);
            this.btnDelAttachment.TabIndex = 22;
            this.btnDelAttachment.Text = "Remove";
            this.btnDelAttachment.UseVisualStyleBackColor = true;
            this.btnDelAttachment.Click += new System.EventHandler(this.btnDelAttachment_Click);
            // 
            // btnPing
            // 
            this.btnPing.Location = new System.Drawing.Point(329, 27);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(109, 23);
            this.btnPing.TabIndex = 27;
            this.btnPing.Text = "Ping server";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxServer);
            this.groupBox1.Controls.Add(this.btnPing);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Location = new System.Drawing.Point(20, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(841, 67);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // cbxServer
            // 
            this.cbxServer.FormattingEnabled = true;
            this.cbxServer.Location = new System.Drawing.Point(59, 27);
            this.cbxServer.Name = "cbxServer";
            this.cbxServer.Size = new System.Drawing.Size(162, 21);
            this.cbxServer.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbSaveInOutbox);
            this.groupBox2.Controls.Add(this.btnDelAttachmentAll);
            this.groupBox2.Controls.Add(this.cbxTo);
            this.groupBox2.Controls.Add(this.cbxFrom);
            this.groupBox2.Controls.Add(this.nrcThreadCount);
            this.groupBox2.Controls.Add(this.lblThreadCount);
            this.groupBox2.Controls.Add(this.lbAttachments);
            this.groupBox2.Controls.Add(this.lvlTo);
            this.groupBox2.Controls.Add(this.lblCount);
            this.groupBox2.Controls.Add(this.nrcCount);
            this.groupBox2.Controls.Add(this.btnAttach);
            this.groupBox2.Controls.Add(this.lblFrom);
            this.groupBox2.Controls.Add(this.lblBody);
            this.groupBox2.Controls.Add(this.btnDelAttachment);
            this.groupBox2.Controls.Add(this.txtSubject);
            this.groupBox2.Controls.Add(this.lblAttachments);
            this.groupBox2.Controls.Add(this.lblSubject);
            this.groupBox2.Controls.Add(this.txtBody);
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Location = new System.Drawing.Point(20, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(841, 356);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Message";
            // 
            // chbSaveInOutbox
            // 
            this.chbSaveInOutbox.AutoSize = true;
            this.chbSaveInOutbox.Location = new System.Drawing.Point(466, 58);
            this.chbSaveInOutbox.Name = "chbSaveInOutbox";
            this.chbSaveInOutbox.Size = new System.Drawing.Size(99, 17);
            this.chbSaveInOutbox.TabIndex = 29;
            this.chbSaveInOutbox.Text = "Save in Outbox";
            this.chbSaveInOutbox.UseVisualStyleBackColor = true;
            this.chbSaveInOutbox.CheckedChanged += new System.EventHandler(this.chbSaveInOutbox_CheckedChanged);
            // 
            // btnDelAttachmentAll
            // 
            this.btnDelAttachmentAll.Enabled = false;
            this.btnDelAttachmentAll.Location = new System.Drawing.Point(706, 117);
            this.btnDelAttachmentAll.Name = "btnDelAttachmentAll";
            this.btnDelAttachmentAll.Size = new System.Drawing.Size(73, 23);
            this.btnDelAttachmentAll.TabIndex = 28;
            this.btnDelAttachmentAll.Text = "Remove all";
            this.btnDelAttachmentAll.UseVisualStyleBackColor = true;
            this.btnDelAttachmentAll.Click += new System.EventHandler(this.btnDelAttachmentAll_Click);
            // 
            // cbxTo
            // 
            this.cbxTo.FormattingEnabled = true;
            this.cbxTo.Location = new System.Drawing.Point(59, 55);
            this.cbxTo.Name = "cbxTo";
            this.cbxTo.Size = new System.Drawing.Size(170, 21);
            this.cbxTo.TabIndex = 4;
            // 
            // cbxFrom
            // 
            this.cbxFrom.FormattingEnabled = true;
            this.cbxFrom.Location = new System.Drawing.Point(59, 19);
            this.cbxFrom.Name = "cbxFrom";
            this.cbxFrom.Size = new System.Drawing.Size(170, 21);
            this.cbxFrom.TabIndex = 3;
            // 
            // nrcThreadCount
            // 
            this.nrcThreadCount.Location = new System.Drawing.Point(390, 51);
            this.nrcThreadCount.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nrcThreadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrcThreadCount.Name = "nrcThreadCount";
            this.nrcThreadCount.Size = new System.Drawing.Size(48, 20);
            this.nrcThreadCount.TabIndex = 25;
            this.nrcThreadCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblThreadCount
            // 
            this.lblThreadCount.AutoSize = true;
            this.lblThreadCount.Location = new System.Drawing.Point(273, 53);
            this.lblThreadCount.Name = "lblThreadCount";
            this.lblThreadCount.Size = new System.Drawing.Size(94, 13);
            this.lblThreadCount.TabIndex = 24;
            this.lblThreadCount.Text = "Number of threads";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtLog);
            this.groupBox3.Location = new System.Drawing.Point(20, 460);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(841, 186);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log";
            // 
            // smtpTabPage
            // 
            this.smtpTabPage.AllowDrop = true;
            this.smtpTabPage.Controls.Add(this.mailTabPage);
            this.smtpTabPage.Controls.Add(this.RemailTabPage);
            this.smtpTabPage.Controls.Add(this.tabPage1);
            this.smtpTabPage.Location = new System.Drawing.Point(8, 12);
            this.smtpTabPage.Name = "smtpTabPage";
            this.smtpTabPage.SelectedIndex = 0;
            this.smtpTabPage.Size = new System.Drawing.Size(889, 699);
            this.smtpTabPage.TabIndex = 31;
            // 
            // mailTabPage
            // 
            this.mailTabPage.AllowDrop = true;
            this.mailTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.mailTabPage.Controls.Add(this.groupBox1);
            this.mailTabPage.Controls.Add(this.groupBox3);
            this.mailTabPage.Controls.Add(this.groupBox2);
            this.mailTabPage.Location = new System.Drawing.Point(4, 22);
            this.mailTabPage.Name = "mailTabPage";
            this.mailTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mailTabPage.Size = new System.Drawing.Size(881, 673);
            this.mailTabPage.TabIndex = 0;
            this.mailTabPage.Text = "Simple Mail";
            this.mailTabPage.DragDrop += new System.Windows.Forms.DragEventHandler(this.mailTabDragDrop);
            this.mailTabPage.DragEnter += new System.Windows.Forms.DragEventHandler(this.form_DragEnter);
            // 
            // RemailTabPage
            // 
            this.RemailTabPage.AllowDrop = true;
            this.RemailTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.RemailTabPage.Controls.Add(this.groupBox5);
            this.RemailTabPage.Controls.Add(this.groupBox4);
            this.RemailTabPage.Location = new System.Drawing.Point(4, 22);
            this.RemailTabPage.Name = "RemailTabPage";
            this.RemailTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RemailTabPage.Size = new System.Drawing.Size(881, 673);
            this.RemailTabPage.TabIndex = 1;
            this.RemailTabPage.Text = "Remail";
            this.RemailTabPage.DragDrop += new System.Windows.Forms.DragEventHandler(this.remailTabDragDrop);
            this.RemailTabPage.DragEnter += new System.Windows.Forms.DragEventHandler(this.form_DragEnter);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblRemailSize);
            this.groupBox5.Controls.Add(this.txtRemailOutput);
            this.groupBox5.Controls.Add(this.btnRemailSaveMail);
            this.groupBox5.Controls.Add(this.txtMailView);
            this.groupBox5.Controls.Add(this.treeViewMails);
            this.groupBox5.Location = new System.Drawing.Point(20, 98);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(841, 562);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Messages";
            // 
            // lblRemailSize
            // 
            this.lblRemailSize.AutoSize = true;
            this.lblRemailSize.Location = new System.Drawing.Point(360, 24);
            this.lblRemailSize.Name = "lblRemailSize";
            this.lblRemailSize.Size = new System.Drawing.Size(10, 13);
            this.lblRemailSize.TabIndex = 17;
            this.lblRemailSize.Text = " ";
            // 
            // txtRemailOutput
            // 
            this.txtRemailOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemailOutput.Location = new System.Drawing.Point(289, 398);
            this.txtRemailOutput.Name = "txtRemailOutput";
            this.txtRemailOutput.ReadOnly = true;
            this.txtRemailOutput.Size = new System.Drawing.Size(534, 143);
            this.txtRemailOutput.TabIndex = 16;
            this.txtRemailOutput.Text = "";
            // 
            // btnRemailSaveMail
            // 
            this.btnRemailSaveMail.Enabled = false;
            this.btnRemailSaveMail.Location = new System.Drawing.Point(289, 19);
            this.btnRemailSaveMail.Name = "btnRemailSaveMail";
            this.btnRemailSaveMail.Size = new System.Drawing.Size(57, 23);
            this.btnRemailSaveMail.TabIndex = 14;
            this.btnRemailSaveMail.Text = "Save";
            this.btnRemailSaveMail.UseVisualStyleBackColor = true;
            this.btnRemailSaveMail.Click += new System.EventHandler(this.btnRemailSaveMail_Click);
            // 
            // txtMailView
            // 
            this.txtMailView.Location = new System.Drawing.Point(289, 48);
            this.txtMailView.MaxLength = 0;
            this.txtMailView.Multiline = true;
            this.txtMailView.Name = "txtMailView";
            this.txtMailView.ReadOnly = true;
            this.txtMailView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMailView.Size = new System.Drawing.Size(534, 334);
            this.txtMailView.TabIndex = 1;
            // 
            // treeViewMails
            // 
            this.treeViewMails.HideSelection = false;
            this.treeViewMails.Location = new System.Drawing.Point(15, 19);
            this.treeViewMails.Name = "treeViewMails";
            this.treeViewMails.Size = new System.Drawing.Size(251, 522);
            this.treeViewMails.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbxRemailFrom);
            this.groupBox4.Controls.Add(this.cbxRemailTo);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.btnRemail);
            this.groupBox4.Controls.Add(this.txtRemailPort);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cbxRemailIP);
            this.groupBox4.Location = new System.Drawing.Point(20, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(841, 67);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Remail Parameter";
            // 
            // cbxRemailFrom
            // 
            this.cbxRemailFrom.FormattingEnabled = true;
            this.cbxRemailFrom.Location = new System.Drawing.Point(363, 27);
            this.cbxRemailFrom.Name = "cbxRemailFrom";
            this.cbxRemailFrom.Size = new System.Drawing.Size(162, 21);
            this.cbxRemailFrom.TabIndex = 3;
            this.cbxRemailFrom.Text = "test@test.com";
            // 
            // cbxRemailTo
            // 
            this.cbxRemailTo.FormattingEnabled = true;
            this.cbxRemailTo.Location = new System.Drawing.Point(565, 27);
            this.cbxRemailTo.Name = "cbxRemailTo";
            this.cbxRemailTo.Size = new System.Drawing.Size(162, 21);
            this.cbxRemailTo.TabIndex = 4;
            this.cbxRemailTo.Text = "georg@insider.com";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(324, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "From:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(536, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "To:";
            // 
            // btnRemail
            // 
            this.btnRemail.Enabled = false;
            this.btnRemail.Location = new System.Drawing.Point(748, 27);
            this.btnRemail.Name = "btnRemail";
            this.btnRemail.Size = new System.Drawing.Size(75, 23);
            this.btnRemail.TabIndex = 5;
            this.btnRemail.Text = "Remail";
            this.btnRemail.UseVisualStyleBackColor = true;
            this.btnRemail.Click += new System.EventHandler(this.btnRemail_Click);
            // 
            // txtRemailPort
            // 
            this.txtRemailPort.Location = new System.Drawing.Point(268, 27);
            this.txtRemailPort.Name = "txtRemailPort";
            this.txtRemailPort.Size = new System.Drawing.Size(37, 20);
            this.txtRemailPort.TabIndex = 2;
            this.txtRemailPort.Text = "25";
            this.txtRemailPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Server:";
            // 
            // cbxRemailIP
            // 
            this.cbxRemailIP.FormattingEnabled = true;
            this.cbxRemailIP.Location = new System.Drawing.Point(59, 27);
            this.cbxRemailIP.Name = "cbxRemailIP";
            this.cbxRemailIP.Size = new System.Drawing.Size(162, 21);
            this.cbxRemailIP.TabIndex = 1;
            this.cbxRemailIP.Text = "192.168.0.25";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBox9);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(881, 673);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "SMTP Session";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.btnSessionSendLine);
            this.groupBox9.Controls.Add(this.groupBox8);
            this.groupBox9.Controls.Add(this.txtSessionOutput);
            this.groupBox9.Controls.Add(this.txtSessionCommand);
            this.groupBox9.Controls.Add(this.groupBox7);
            this.groupBox9.Location = new System.Drawing.Point(20, 98);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(841, 556);
            this.groupBox9.TabIndex = 35;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Session";
            // 
            // btnSessionSendLine
            // 
            this.btnSessionSendLine.Location = new System.Drawing.Point(549, 19);
            this.btnSessionSendLine.Name = "btnSessionSendLine";
            this.btnSessionSendLine.Size = new System.Drawing.Size(75, 23);
            this.btnSessionSendLine.TabIndex = 39;
            this.btnSessionSendLine.Text = "Send";
            this.btnSessionSendLine.UseVisualStyleBackColor = true;
            this.btnSessionSendLine.Click += new System.EventHandler(this.btnSessionSendLine_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lbxSessionHistory);
            this.groupBox8.Controls.Add(this.btnSessionResend);
            this.groupBox8.Controls.Add(this.btnSessionClearCommand);
            this.groupBox8.Location = new System.Drawing.Point(543, 285);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(281, 252);
            this.groupBox8.TabIndex = 38;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Command History";
            // 
            // lbxSessionHistory
            // 
            this.lbxSessionHistory.FormattingEnabled = true;
            this.lbxSessionHistory.Location = new System.Drawing.Point(15, 19);
            this.lbxSessionHistory.Name = "lbxSessionHistory";
            this.lbxSessionHistory.Size = new System.Drawing.Size(244, 186);
            this.lbxSessionHistory.TabIndex = 8;
            // 
            // btnSessionResend
            // 
            this.btnSessionResend.Location = new System.Drawing.Point(15, 214);
            this.btnSessionResend.Name = "btnSessionResend";
            this.btnSessionResend.Size = new System.Drawing.Size(110, 23);
            this.btnSessionResend.TabIndex = 11;
            this.btnSessionResend.Text = "Resend Command";
            this.btnSessionResend.UseVisualStyleBackColor = true;
            this.btnSessionResend.Click += new System.EventHandler(this.btnSessionResend_Click);
            // 
            // btnSessionClearCommand
            // 
            this.btnSessionClearCommand.Location = new System.Drawing.Point(141, 214);
            this.btnSessionClearCommand.Name = "btnSessionClearCommand";
            this.btnSessionClearCommand.Size = new System.Drawing.Size(118, 23);
            this.btnSessionClearCommand.TabIndex = 12;
            this.btnSessionClearCommand.Text = "Clear Command";
            this.btnSessionClearCommand.UseVisualStyleBackColor = true;
            this.btnSessionClearCommand.Click += new System.EventHandler(this.btnSessionClearCommand_Click);
            // 
            // txtSessionOutput
            // 
            this.txtSessionOutput.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtSessionOutput.Location = new System.Drawing.Point(24, 48);
            this.txtSessionOutput.Name = "txtSessionOutput";
            this.txtSessionOutput.ReadOnly = true;
            this.txtSessionOutput.Size = new System.Drawing.Size(500, 489);
            this.txtSessionOutput.TabIndex = 37;
            this.txtSessionOutput.Text = "";
            // 
            // txtSessionCommand
            // 
            this.txtSessionCommand.Location = new System.Drawing.Point(24, 19);
            this.txtSessionCommand.Name = "txtSessionCommand";
            this.txtSessionCommand.Size = new System.Drawing.Size(501, 20);
            this.txtSessionCommand.TabIndex = 36;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtSessionEhlo);
            this.groupBox7.Controls.Add(this.cbxSessionTo);
            this.groupBox7.Controls.Add(this.cbxSessionFrom);
            this.groupBox7.Controls.Add(this.btnSessionReset);
            this.groupBox7.Controls.Add(this.btnSessionHelo);
            this.groupBox7.Controls.Add(this.btnSessionFrom);
            this.groupBox7.Controls.Add(this.btnSessionDot);
            this.groupBox7.Controls.Add(this.btnSessionRcpt);
            this.groupBox7.Controls.Add(this.btnSessionData);
            this.groupBox7.Controls.Add(this.btnSessionQuit);
            this.groupBox7.Location = new System.Drawing.Point(543, 48);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(281, 220);
            this.groupBox7.TabIndex = 35;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Quick Commands";
            // 
            // cbxSessionTo
            // 
            this.cbxSessionTo.FormattingEnabled = true;
            this.cbxSessionTo.Location = new System.Drawing.Point(87, 79);
            this.cbxSessionTo.Name = "cbxSessionTo";
            this.cbxSessionTo.Size = new System.Drawing.Size(172, 21);
            this.cbxSessionTo.TabIndex = 36;
            // 
            // cbxSessionFrom
            // 
            this.cbxSessionFrom.FormattingEnabled = true;
            this.cbxSessionFrom.Location = new System.Drawing.Point(87, 50);
            this.cbxSessionFrom.Name = "cbxSessionFrom";
            this.cbxSessionFrom.Size = new System.Drawing.Size(172, 21);
            this.cbxSessionFrom.TabIndex = 35;
            // 
            // btnSessionReset
            // 
            this.btnSessionReset.Location = new System.Drawing.Point(6, 162);
            this.btnSessionReset.Name = "btnSessionReset";
            this.btnSessionReset.Size = new System.Drawing.Size(75, 23);
            this.btnSessionReset.TabIndex = 24;
            this.btnSessionReset.Text = "rset";
            this.btnSessionReset.UseVisualStyleBackColor = true;
            this.btnSessionReset.Click += new System.EventHandler(this.btnSessionReset_Click);
            // 
            // btnSessionHelo
            // 
            this.btnSessionHelo.Location = new System.Drawing.Point(6, 19);
            this.btnSessionHelo.Name = "btnSessionHelo";
            this.btnSessionHelo.Size = new System.Drawing.Size(75, 23);
            this.btnSessionHelo.TabIndex = 19;
            this.btnSessionHelo.Text = "ehlo";
            this.btnSessionHelo.UseVisualStyleBackColor = true;
            this.btnSessionHelo.Click += new System.EventHandler(this.btnSessionHelo_Click);
            // 
            // btnSessionFrom
            // 
            this.btnSessionFrom.Location = new System.Drawing.Point(6, 48);
            this.btnSessionFrom.Name = "btnSessionFrom";
            this.btnSessionFrom.Size = new System.Drawing.Size(75, 23);
            this.btnSessionFrom.TabIndex = 13;
            this.btnSessionFrom.Text = "mail from:";
            this.btnSessionFrom.UseVisualStyleBackColor = true;
            this.btnSessionFrom.Click += new System.EventHandler(this.btnSessionFrom_Click);
            // 
            // btnSessionDot
            // 
            this.btnSessionDot.Location = new System.Drawing.Point(6, 135);
            this.btnSessionDot.Name = "btnSessionDot";
            this.btnSessionDot.Size = new System.Drawing.Size(75, 23);
            this.btnSessionDot.TabIndex = 23;
            this.btnSessionDot.Text = "\".\"";
            this.btnSessionDot.UseVisualStyleBackColor = true;
            this.btnSessionDot.Click += new System.EventHandler(this.btnSessionDot_Click);
            // 
            // btnSessionRcpt
            // 
            this.btnSessionRcpt.Location = new System.Drawing.Point(6, 77);
            this.btnSessionRcpt.Name = "btnSessionRcpt";
            this.btnSessionRcpt.Size = new System.Drawing.Size(75, 23);
            this.btnSessionRcpt.TabIndex = 14;
            this.btnSessionRcpt.Text = "rcpt to:";
            this.btnSessionRcpt.UseVisualStyleBackColor = true;
            this.btnSessionRcpt.Click += new System.EventHandler(this.btnSessionRcpt_Click);
            // 
            // btnSessionData
            // 
            this.btnSessionData.Location = new System.Drawing.Point(6, 106);
            this.btnSessionData.Name = "btnSessionData";
            this.btnSessionData.Size = new System.Drawing.Size(75, 23);
            this.btnSessionData.TabIndex = 15;
            this.btnSessionData.Text = "data";
            this.btnSessionData.UseVisualStyleBackColor = true;
            this.btnSessionData.Click += new System.EventHandler(this.btnSessionData_Click);
            // 
            // btnSessionQuit
            // 
            this.btnSessionQuit.Location = new System.Drawing.Point(6, 191);
            this.btnSessionQuit.Name = "btnSessionQuit";
            this.btnSessionQuit.Size = new System.Drawing.Size(75, 23);
            this.btnSessionQuit.TabIndex = 21;
            this.btnSessionQuit.Text = "quit";
            this.btnSessionQuit.UseVisualStyleBackColor = true;
            this.btnSessionQuit.Click += new System.EventHandler(this.btnSessionQuit_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnSessionOpenNewWindow);
            this.groupBox6.Controls.Add(this.cbxSessionServer);
            this.groupBox6.Controls.Add(this.btnSessionConnect);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.cbxSessionPort);
            this.groupBox6.Location = new System.Drawing.Point(20, 22);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(841, 67);
            this.groupBox6.TabIndex = 29;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Server";
            // 
            // btnSessionOpenNewWindow
            // 
            this.btnSessionOpenNewWindow.Location = new System.Drawing.Point(434, 27);
            this.btnSessionOpenNewWindow.Name = "btnSessionOpenNewWindow";
            this.btnSessionOpenNewWindow.Size = new System.Drawing.Size(85, 23);
            this.btnSessionOpenNewWindow.TabIndex = 37;
            this.btnSessionOpenNewWindow.Text = "New window";
            this.btnSessionOpenNewWindow.UseVisualStyleBackColor = true;
            this.btnSessionOpenNewWindow.Click += new System.EventHandler(this.btnSessionOpenNewWindow_Click);
            // 
            // cbxSessionServer
            // 
            this.cbxSessionServer.FormattingEnabled = true;
            this.cbxSessionServer.Location = new System.Drawing.Point(59, 27);
            this.cbxSessionServer.Name = "cbxSessionServer";
            this.cbxSessionServer.Size = new System.Drawing.Size(162, 21);
            this.cbxSessionServer.TabIndex = 26;
            // 
            // btnSessionConnect
            // 
            this.btnSessionConnect.Location = new System.Drawing.Point(327, 27);
            this.btnSessionConnect.Name = "btnSessionConnect";
            this.btnSessionConnect.Size = new System.Drawing.Size(85, 23);
            this.btnSessionConnect.TabIndex = 28;
            this.btnSessionConnect.Text = "Connect";
            this.btnSessionConnect.UseVisualStyleBackColor = true;
            this.btnSessionConnect.Click += new System.EventHandler(this.btnSessionConnect_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Server:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(237, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Port:";
            // 
            // cbxSessionPort
            // 
            this.cbxSessionPort.Location = new System.Drawing.Point(268, 27);
            this.cbxSessionPort.Name = "cbxSessionPort";
            this.cbxSessionPort.Size = new System.Drawing.Size(37, 20);
            this.cbxSessionPort.TabIndex = 3;
            this.cbxSessionPort.Text = "25";
            this.cbxSessionPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusLabelAuthorInfo,
            this.statusLabelMail,
            this.statusLabelUpdateInfo,
            this.statusLabelLink});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 716);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(910, 22);
            this.mainStatusStrip.SizingGrip = false;
            this.mainStatusStrip.TabIndex = 32;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 17);
            this.statusLabel.Text = "status";
            // 
            // statusLabelAuthorInfo
            // 
            this.statusLabelAuthorInfo.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.statusLabelAuthorInfo.Name = "statusLabelAuthorInfo";
            this.statusLabelAuthorInfo.Size = new System.Drawing.Size(44, 17);
            this.statusLabelAuthorInfo.Text = "Author";
            this.statusLabelAuthorInfo.Visible = false;
            // 
            // statusLabelMail
            // 
            this.statusLabelMail.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.statusLabelMail.IsLink = true;
            this.statusLabelMail.Name = "statusLabelMail";
            this.statusLabelMail.Size = new System.Drawing.Size(30, 17);
            this.statusLabelMail.Text = "Mail";
            this.statusLabelMail.Visible = false;
            this.statusLabelMail.Click += new System.EventHandler(this.statusLabelMail_Click);
            // 
            // statusLabelUpdateInfo
            // 
            this.statusLabelUpdateInfo.Name = "statusLabelUpdateInfo";
            this.statusLabelUpdateInfo.Size = new System.Drawing.Size(65, 17);
            this.statusLabelUpdateInfo.Text = "updateInfo";
            this.statusLabelUpdateInfo.Visible = false;
            // 
            // statusLabelLink
            // 
            this.statusLabelLink.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.statusLabelLink.IsLink = true;
            this.statusLabelLink.Name = "statusLabelLink";
            this.statusLabelLink.Size = new System.Drawing.Size(65, 17);
            this.statusLabelLink.Text = "updateURL";
            this.statusLabelLink.Visible = false;
            this.statusLabelLink.Click += new System.EventHandler(this.statusLabelLink_Click);
            // 
            // toolTipUpdateStatus
            // 
            this.toolTipUpdateStatus.Name = "toolTipUpdateStatus";
            this.toolTipUpdateStatus.Size = new System.Drawing.Size(23, 23);
            // 
            // txtSessionEhlo
            // 
            this.txtSessionEhlo.Location = new System.Drawing.Point(87, 21);
            this.txtSessionEhlo.Name = "txtSessionEhlo";
            this.txtSessionEhlo.Size = new System.Drawing.Size(172, 20);
            this.txtSessionEhlo.TabIndex = 37;
            this.txtSessionEhlo.Text = "servername";
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 738);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.smtpTabPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "SMTP Test Tool";
            ((System.ComponentModel.ISupportInitialize)(this.nrcCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nrcThreadCount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.smtpTabPage.ResumeLayout(false);
            this.mailTabPage.ResumeLayout(false);
            this.RemailTabPage.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.RichTextBox txtLog;
        public System.Windows.Forms.Label lblFrom;
        public System.Windows.Forms.Label lvlTo;
        public System.Windows.Forms.Label lblServer;
        public System.Windows.Forms.Label lblPort;
        public System.Windows.Forms.TextBox txtPort;
        public System.Windows.Forms.TextBox txtBody;
        public System.Windows.Forms.Label lblSubject;
        public System.Windows.Forms.TextBox txtSubject;
        public System.Windows.Forms.Label lblBody;
        public System.Windows.Forms.Button btnAttach;
        public System.Windows.Forms.NumericUpDown nrcCount;
        public System.Windows.Forms.Label lblCount;
        public System.Windows.Forms.ListBox lbAttachments;
        public System.Windows.Forms.Label lblAttachments;
        public System.Windows.Forms.Button btnDelAttachment;
        public System.Windows.Forms.Button btnPing;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.Label lblThreadCount;
        public System.Windows.Forms.NumericUpDown nrcThreadCount;
        public System.Windows.Forms.ComboBox cbxServer;
        public System.Windows.Forms.ComboBox cbxFrom;
        public System.Windows.Forms.ComboBox cbxTo;
        public System.Windows.Forms.Button btnDelAttachmentAll;
        public System.Windows.Forms.TabControl smtpTabPage;
        public System.Windows.Forms.TabPage mailTabPage;
        public System.Windows.Forms.TabPage RemailTabPage;
        public System.Windows.Forms.CheckBox chbSaveInOutbox;
        public TreeView treeViewMails;
        public GroupBox groupBox4;
        public Label label1;
        public ComboBox cbxRemailIP;
        public TextBox txtMailView;
        public GroupBox groupBox5;
        public ComboBox cbxRemailFrom;
        public ComboBox cbxRemailTo;
        public Label label3;
        public Label label4;
        public Button btnRemail;
        public TextBox txtRemailPort;
        public Label label2;
        public Button btnRemailSaveMail;
        public StatusStrip mainStatusStrip;
        public ToolStripStatusLabel toolStripStatusLabel1;
        public RichTextBox txtRemailOutput;
        public TabPage tabPage1;
        public ToolStripStatusLabel toolTipUpdateStatus;
        public ToolStripStatusLabel statusLabel;
        public ToolStripStatusLabel statusLabelLink;
        public ToolStripStatusLabel statusLabelMail;
        public ToolStripStatusLabel statusLabelUpdateInfo;
        public ToolStripStatusLabel statusLabelAuthorInfo;
        public GroupBox groupBox6;
        public ComboBox cbxSessionServer;
        public Button btnSessionConnect;
        public Label label5;
        public Label label6;
        public TextBox cbxSessionPort;
        public Button btnSessionOpenNewWindow;
        private GroupBox groupBox9;
        public Button btnSessionSendLine;
        public GroupBox groupBox8;
        public ListBox lbxSessionHistory;
        public Button btnSessionResend;
        public Button btnSessionClearCommand;
        public RichTextBox txtSessionOutput;
        public TextBox txtSessionCommand;
        public GroupBox groupBox7;
        public ComboBox cbxSessionTo;
        public ComboBox cbxSessionFrom;
        public Button btnSessionReset;
        public Button btnSessionHelo;
        public Button btnSessionFrom;
        public Button btnSessionDot;
        public Button btnSessionRcpt;
        public Button btnSessionData;
        public Button btnSessionQuit;
        public Label lblRemailSize;
        public TextBox txtSessionEhlo;

        public System.EventHandler treeViewExpanded { get; set; }
    }
}


using System.Windows.Forms;
using System.Drawing;
namespace SMTPtool
{
    partial class Telnet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Telnet));
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.btnSendLine = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.lbxHistory = new System.Windows.Forms.ListBox();
            this.btnResend = new System.Windows.Forms.Button();
            this.btnClearCommand = new System.Windows.Forms.Button();
            this.btnFrom = new System.Windows.Forms.Button();
            this.btnRcpt = new System.Windows.Forms.Button();
            this.btnData = new System.Windows.Forms.Button();
            this.btnHelo = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnRecon = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.cbxFrom = new System.Windows.Forms.ComboBox();
            this.cbxTo = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(9, 10);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(501, 20);
            this.txtCommand.TabIndex = 1;
            this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_KeyDown);
            // 
            // btnSendLine
            // 
            this.btnSendLine.Location = new System.Drawing.Point(522, 7);
            this.btnSendLine.Name = "btnSendLine";
            this.btnSendLine.Size = new System.Drawing.Size(75, 23);
            this.btnSendLine.TabIndex = 2;
            this.btnSendLine.Text = "Send";
            this.btnSendLine.UseVisualStyleBackColor = true;
            this.btnSendLine.Click += new System.EventHandler(this.btnSendLine_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Font = new System.Drawing.Font("Courier New", 10F);
            this.txtOutput.Location = new System.Drawing.Point(9, 36);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(500, 434);
            this.txtOutput.TabIndex = 3;
            this.txtOutput.Text = "";
            // 
            // lbxHistory
            // 
            this.lbxHistory.FormattingEnabled = true;
            this.lbxHistory.Location = new System.Drawing.Point(15, 19);
            this.lbxHistory.Name = "lbxHistory";
            this.lbxHistory.Size = new System.Drawing.Size(234, 147);
            this.lbxHistory.TabIndex = 8;
            // 
            // btnResend
            // 
            this.btnResend.Location = new System.Drawing.Point(15, 172);
            this.btnResend.Name = "btnResend";
            this.btnResend.Size = new System.Drawing.Size(110, 23);
            this.btnResend.TabIndex = 11;
            this.btnResend.Text = "Resend Command";
            this.btnResend.UseVisualStyleBackColor = true;
            this.btnResend.Click += new System.EventHandler(this.btnResend_Click);
            // 
            // btnClearCommand
            // 
            this.btnClearCommand.Location = new System.Drawing.Point(141, 172);
            this.btnClearCommand.Name = "btnClearCommand";
            this.btnClearCommand.Size = new System.Drawing.Size(108, 23);
            this.btnClearCommand.TabIndex = 12;
            this.btnClearCommand.Text = "Clear Command";
            this.btnClearCommand.UseVisualStyleBackColor = true;
            this.btnClearCommand.Click += new System.EventHandler(this.btnClearCommand_Click);
            // 
            // btnFrom
            // 
            this.btnFrom.Location = new System.Drawing.Point(6, 48);
            this.btnFrom.Name = "btnFrom";
            this.btnFrom.Size = new System.Drawing.Size(75, 23);
            this.btnFrom.TabIndex = 4;
            this.btnFrom.Text = "mail from:";
            this.btnFrom.UseVisualStyleBackColor = true;
            this.btnFrom.Click += new System.EventHandler(this.btnFrom_Click);
            // 
            // btnRcpt
            // 
            this.btnRcpt.Location = new System.Drawing.Point(6, 77);
            this.btnRcpt.Name = "btnRcpt";
            this.btnRcpt.Size = new System.Drawing.Size(75, 23);
            this.btnRcpt.TabIndex = 6;
            this.btnRcpt.Text = "rcpt to:";
            this.btnRcpt.UseVisualStyleBackColor = true;
            this.btnRcpt.Click += new System.EventHandler(this.btnRcpt_Click);
            // 
            // btnData
            // 
            this.btnData.Location = new System.Drawing.Point(6, 106);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(75, 23);
            this.btnData.TabIndex = 8;
            this.btnData.Text = "data";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // btnHelo
            // 
            this.btnHelo.Location = new System.Drawing.Point(6, 19);
            this.btnHelo.Name = "btnHelo";
            this.btnHelo.Size = new System.Drawing.Size(75, 23);
            this.btnHelo.TabIndex = 3;
            this.btnHelo.Text = "helo";
            this.btnHelo.UseVisualStyleBackColor = true;
            this.btnHelo.Click += new System.EventHandler(this.btnHelo_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(7, 191);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 11;
            this.btnQuit.Text = "quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnRecon
            // 
            this.btnRecon.Location = new System.Drawing.Point(603, 7);
            this.btnRecon.Name = "btnRecon";
            this.btnRecon.Size = new System.Drawing.Size(75, 23);
            this.btnRecon.TabIndex = 22;
            this.btnRecon.Text = "Reconnect";
            this.btnRecon.UseVisualStyleBackColor = true;
            this.btnRecon.Click += new System.EventHandler(this.btnRecon_Click);
            // 
            // btnDot
            // 
            this.btnDot.Location = new System.Drawing.Point(6, 135);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(75, 23);
            this.btnDot.TabIndex = 9;
            this.btnDot.Text = "\".\"";
            this.btnDot.UseVisualStyleBackColor = true;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbxHistory);
            this.groupBox1.Controls.Add(this.btnResend);
            this.groupBox1.Controls.Add(this.btnClearCommand);
            this.groupBox1.Location = new System.Drawing.Point(515, 263);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 207);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Command History";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxTo);
            this.groupBox2.Controls.Add(this.cbxFrom);
            this.groupBox2.Controls.Add(this.btnReset);
            this.groupBox2.Controls.Add(this.btnHelo);
            this.groupBox2.Controls.Add(this.btnFrom);
            this.groupBox2.Controls.Add(this.btnDot);
            this.groupBox2.Controls.Add(this.btnRcpt);
            this.groupBox2.Controls.Add(this.btnData);
            this.groupBox2.Controls.Add(this.btnQuit);
            this.groupBox2.Location = new System.Drawing.Point(515, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 220);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Quick Commands";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(7, 162);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "rset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cbxFrom
            // 
            this.cbxFrom.FormattingEnabled = true;
            this.cbxFrom.Location = new System.Drawing.Point(87, 51);
            this.cbxFrom.Name = "cbxFrom";
            this.cbxFrom.Size = new System.Drawing.Size(162, 21);
            this.cbxFrom.TabIndex = 12;
            // 
            // cbxTo
            // 
            this.cbxTo.FormattingEnabled = true;
            this.cbxTo.Location = new System.Drawing.Point(87, 79);
            this.cbxTo.Name = "cbxTo";
            this.cbxTo.Size = new System.Drawing.Size(162, 21);
            this.cbxTo.TabIndex = 13;
            // 
            // Telnet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 482);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRecon);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnSendLine);
            this.Controls.Add(this.txtCommand);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Telnet";
            this.Text = "SMTP Session to ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Button btnSendLine;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.ListBox lbxHistory;
        private System.Windows.Forms.Button btnResend;
        private System.Windows.Forms.Button btnClearCommand;
        private System.Windows.Forms.Button btnFrom;
        private System.Windows.Forms.Button btnRcpt;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Button btnHelo;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnRecon;
        private System.Windows.Forms.Button btnDot;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button btnReset;
        private ComboBox cbxTo;
        private ComboBox cbxFrom;
    }
}
namespace SrP_ClassroomInq
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.broadcastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sb_send_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnSend = new System.Windows.Forms.Button();
            this.trayICON = new System.Windows.Forms.NotifyIcon(this.components);
            this.grpbxFeed = new System.Windows.Forms.GroupBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.grpbx_Reply = new System.Windows.Forms.GroupBox();
            this.btnRPL = new System.Windows.Forms.Button();
            this.btnCLS = new System.Windows.Forms.Button();
            this.btnCLR = new System.Windows.Forms.Button();
            this.txtbx_Reply = new System.Windows.Forms.TextBox();
            this.lbl_question = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.grpbxFeed.SuspendLayout();
            this.grpbx_Reply.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToolStripMenuItem,
            this.replyToolStripMenuItem,
            this.controlsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(364, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sendToolStripMenuItem
            // 
            this.sendToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.broadcastToolStripMenuItem,
            this.directMessageToolStripMenuItem});
            this.sendToolStripMenuItem.Name = "sendToolStripMenuItem";
            this.sendToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.sendToolStripMenuItem.Text = "Compose";
            // 
            // broadcastToolStripMenuItem
            // 
            this.broadcastToolStripMenuItem.Name = "broadcastToolStripMenuItem";
            this.broadcastToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.broadcastToolStripMenuItem.Text = "Broadcast";
            // 
            // directMessageToolStripMenuItem
            // 
            this.directMessageToolStripMenuItem.Name = "directMessageToolStripMenuItem";
            this.directMessageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.directMessageToolStripMenuItem.Text = "Direct Message";
            // 
            // replyToolStripMenuItem
            // 
            this.replyToolStripMenuItem.Name = "replyToolStripMenuItem";
            this.replyToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.replyToolStripMenuItem.Text = "Reply";
            this.replyToolStripMenuItem.Click += new System.EventHandler(this.replyToolStripMenuItem_Click);
            // 
            // controlsToolStripMenuItem
            // 
            this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            this.controlsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.controlsToolStripMenuItem.Text = "Controls";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 27);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(340, 45);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Enter Message here..";
            this.textBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sb_send_status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 528);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(364, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sb_send_status
            // 
            this.sb_send_status.Name = "sb_send_status";
            this.sb_send_status.Size = new System.Drawing.Size(167, 17);
            this.sb_send_status.Text = "Welcome to Classroom Inquisition";
            // 
            // timer
            // 
            this.timer.Interval = 75;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(277, 80);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 29);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send Msg";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Visible = false;
            // 
            // trayICON
            // 
            this.trayICON.BalloonTipText = "Raising Hands is a thing of the past.";
            this.trayICON.BalloonTipTitle = "Classroom Inq.";
            this.trayICON.Icon = ((System.Drawing.Icon)(resources.GetObject("trayICON.Icon")));
            this.trayICON.Text = "Classroom Inq.";
            this.trayICON.Visible = true;
            this.trayICON.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayICON_MouseDoubleClick);
            // 
            // grpbxFeed
            // 
            this.grpbxFeed.BackColor = System.Drawing.SystemColors.Control;
            this.grpbxFeed.Controls.Add(this.vScrollBar1);
            this.grpbxFeed.Controls.Add(this.grpbx_Reply);
            this.grpbxFeed.Location = new System.Drawing.Point(12, 80);
            this.grpbxFeed.Name = "grpbxFeed";
            this.grpbxFeed.Size = new System.Drawing.Size(340, 432);
            this.grpbxFeed.TabIndex = 5;
            this.grpbxFeed.TabStop = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(321, 16);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 413);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.Visible = false;
            // 
            // grpbx_Reply
            // 
            this.grpbx_Reply.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.grpbx_Reply.Controls.Add(this.btnRPL);
            this.grpbx_Reply.Controls.Add(this.btnCLS);
            this.grpbx_Reply.Controls.Add(this.btnCLR);
            this.grpbx_Reply.Controls.Add(this.txtbx_Reply);
            this.grpbx_Reply.Controls.Add(this.lbl_question);
            this.grpbx_Reply.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grpbx_Reply.Location = new System.Drawing.Point(6, 19);
            this.grpbx_Reply.Name = "grpbx_Reply";
            this.grpbx_Reply.Size = new System.Drawing.Size(328, 32);
            this.grpbx_Reply.TabIndex = 2;
            this.grpbx_Reply.TabStop = false;
            // 
            // btnRPL
            // 
            this.btnRPL.Location = new System.Drawing.Point(230, 98);
            this.btnRPL.Name = "btnRPL";
            this.btnRPL.Size = new System.Drawing.Size(67, 25);
            this.btnRPL.TabIndex = 4;
            this.btnRPL.Text = "Repl&y";
            this.btnRPL.UseVisualStyleBackColor = true;
            // 
            // btnCLS
            // 
            this.btnCLS.Location = new System.Drawing.Point(129, 98);
            this.btnCLS.Name = "btnCLS";
            this.btnCLS.Size = new System.Drawing.Size(67, 25);
            this.btnCLS.TabIndex = 3;
            this.btnCLS.Text = "Clos&e";
            this.btnCLS.UseVisualStyleBackColor = true;
            this.btnCLS.Click += new System.EventHandler(this.btnCLS_Click);
            // 
            // btnCLR
            // 
            this.btnCLR.Location = new System.Drawing.Point(23, 98);
            this.btnCLR.Name = "btnCLR";
            this.btnCLR.Size = new System.Drawing.Size(67, 25);
            this.btnCLR.TabIndex = 2;
            this.btnCLR.Text = "Clea&r";
            this.btnCLR.UseVisualStyleBackColor = true;
            this.btnCLR.Click += new System.EventHandler(this.btnCLR_Click);
            // 
            // txtbx_Reply
            // 
            this.txtbx_Reply.AcceptsReturn = true;
            this.txtbx_Reply.Location = new System.Drawing.Point(23, 32);
            this.txtbx_Reply.Multiline = true;
            this.txtbx_Reply.Name = "txtbx_Reply";
            this.txtbx_Reply.Size = new System.Drawing.Size(274, 51);
            this.txtbx_Reply.TabIndex = 1;
            // 
            // lbl_question
            // 
            this.lbl_question.AutoSize = true;
            this.lbl_question.Location = new System.Drawing.Point(62, 13);
            this.lbl_question.Name = "lbl_question";
            this.lbl_question.Size = new System.Drawing.Size(191, 13);
            this.lbl_question.TabIndex = 0;
            this.lbl_question.Text = "Why is there evil in the world??\t-Joseph";
            this.lbl_question.Click += new System.EventHandler(this.lbl_question_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(364, 550);
            this.ControlBox = false;
            this.Controls.Add(this.grpbxFeed);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = " Classroom Inquisition";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpbxFeed.ResumeLayout(false);
            this.grpbx_Reply.ResumeLayout(false);
            this.grpbx_Reply.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem broadcastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sb_send_status;
        private System.Windows.Forms.ToolStripMenuItem controlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.NotifyIcon trayICON;
        private System.Windows.Forms.GroupBox grpbxFeed;
        private System.Windows.Forms.GroupBox grpbx_Reply;
        private System.Windows.Forms.Button btnCLR;
        private System.Windows.Forms.TextBox txtbx_Reply;
        private System.Windows.Forms.Label lbl_question;
        private System.Windows.Forms.Button btnRPL;
        private System.Windows.Forms.Button btnCLS;
        private System.Windows.Forms.VScrollBar vScrollBar1;
    }
}


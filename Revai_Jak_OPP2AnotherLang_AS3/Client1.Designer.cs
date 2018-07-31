namespace Revai_Jak_OPP2AnotherLang_AS3
{
    partial class Client1
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
            this.btnStartChat = new System.Windows.Forms.Button();
            this.lblxChat = new System.Windows.Forms.ListBox();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menu = new System.Windows.Forms.ToolStripMenuItem();
            this.previousConversationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewErrorlogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartChat
            // 
            this.btnStartChat.Location = new System.Drawing.Point(12, 27);
            this.btnStartChat.Name = "btnStartChat";
            this.btnStartChat.Size = new System.Drawing.Size(107, 37);
            this.btnStartChat.TabIndex = 0;
            this.btnStartChat.Text = "Start Chat";
            this.btnStartChat.UseVisualStyleBackColor = true;
            this.btnStartChat.Click += new System.EventHandler(this.btnStartChat_Click);
            // 
            // lblxChat
            // 
            this.lblxChat.FormattingEnabled = true;
            this.lblxChat.Location = new System.Drawing.Point(12, 79);
            this.lblxChat.Name = "lblxChat";
            this.lblxChat.Size = new System.Drawing.Size(393, 238);
            this.lblxChat.TabIndex = 1;
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(12, 334);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(312, 20);
            this.txtSend.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(330, 332);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(422, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menu
            // 
            this.menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previousConversationToolStripMenuItem,
            this.viewErrorlogToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(50, 20);
            this.menu.Text = "Menu";
            // 
            // previousConversationToolStripMenuItem
            // 
            this.previousConversationToolStripMenuItem.Name = "previousConversationToolStripMenuItem";
            this.previousConversationToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.previousConversationToolStripMenuItem.Text = "Previous Conversation";
            this.previousConversationToolStripMenuItem.Click += new System.EventHandler(this.previousConversationToolStripMenuItem_Click);
            // 
            // viewErrorlogToolStripMenuItem
            // 
            this.viewErrorlogToolStripMenuItem.Name = "viewErrorlogToolStripMenuItem";
            this.viewErrorlogToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.viewErrorlogToolStripMenuItem.Text = "View error-log";
            this.viewErrorlogToolStripMenuItem.Click += new System.EventHandler(this.viewErrorlogToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Client1
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 378);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.lblxChat);
            this.Controls.Add(this.btnStartChat);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Client1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client1_FormClosed);
            this.Load += new System.EventHandler(this.Client1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartChat;
        private System.Windows.Forms.ListBox lblxChat;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menu;
        private System.Windows.Forms.ToolStripMenuItem previousConversationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewErrorlogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}


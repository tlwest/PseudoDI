namespace PseudoDI
{
    partial class PseudoDI
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
            this.commandTextBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.commandListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.feedbackArea = new System.Windows.Forms.RichTextBox();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openButton = new System.Windows.Forms.Button();
            this.c_loadIniButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // commandTextBox
            // 
            this.commandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandTextBox.Location = new System.Drawing.Point(12, 16);
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.Size = new System.Drawing.Size(494, 20);
            this.commandTextBox.TabIndex = 0;
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.Location = new System.Drawing.Point(512, 15);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(53, 23);
            this.sendButton.TabIndex = 2;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Commands:";
            // 
            // commandListBox
            // 
            this.commandListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandListBox.FormattingEnabled = true;
            this.commandListBox.Location = new System.Drawing.Point(12, 59);
            this.commandListBox.Name = "commandListBox";
            this.commandListBox.Size = new System.Drawing.Size(782, 212);
            this.commandListBox.TabIndex = 4;
            this.commandListBox.SelectedIndexChanged += new System.EventHandler(this.commandListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 278);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Connection (DI in Green, TickerManager in Blue):";
            // 
            // feedbackArea
            // 
            this.feedbackArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.feedbackArea.Location = new System.Drawing.Point(12, 295);
            this.feedbackArea.Name = "feedbackArea";
            this.feedbackArea.Size = new System.Drawing.Size(782, 166);
            this.feedbackArea.TabIndex = 6;
            this.feedbackArea.Text = "";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timeTextBox.Location = new System.Drawing.Point(750, 16);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.ReadOnly = true;
            this.timeTextBox.Size = new System.Drawing.Size(44, 20);
            this.timeTextBox.TabIndex = 7;
            this.timeTextBox.Text = "00:00";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(716, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Time:";
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openButton.Location = new System.Drawing.Point(640, 15);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(66, 23);
            this.openButton.TabIndex = 9;
            this.openButton.Text = "Open INI";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // c_loadIniButton
            // 
            this.c_loadIniButton.Location = new System.Drawing.Point(572, 15);
            this.c_loadIniButton.Name = "c_loadIniButton";
            this.c_loadIniButton.Size = new System.Drawing.Size(62, 23);
            this.c_loadIniButton.TabIndex = 10;
            this.c_loadIniButton.Text = "Load INI";
            this.c_loadIniButton.UseVisualStyleBackColor = true;
            // 
            // PseudoDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 473);
            this.Controls.Add(this.c_loadIniButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.timeTextBox);
            this.Controls.Add(this.feedbackArea);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.commandListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.commandTextBox);
            this.Name = "PseudoDI";
            this.Text = "PseudoDI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PseudoDI_FormClosing);
            this.Load += new System.EventHandler(this.PseudoDI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox commandTextBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox commandListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox feedbackArea;
        private System.Windows.Forms.TextBox timeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button c_loadIniButton;
    }
}


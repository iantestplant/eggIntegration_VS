namespace TestPlant.EggPlantVSPackage
{
    partial class ePF_TestCreation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ePF_TestCreation));
            this.button_OK = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.hostValue = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button_addScript = new System.Windows.Forms.Button();
            this.SuitePath_text = new System.Windows.Forms.TextBox();
            this.button_suitePath = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.openFileDialog_suite = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog_suite = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.spin_portValue = new System.Windows.Forms.NumericUpDown();
            this.label_port = new System.Windows.Forms.Label();
            this.button_removeScript = new System.Windows.Forms.Button();
            this.label_host = new System.Windows.Forms.Label();
            this.linklabel_drive = new System.Windows.Forms.LinkLabel();
            this.radioButton_local = new System.Windows.Forms.RadioButton();
            this.radioButton_drive = new System.Windows.Forms.RadioButton();
            this.linkLabel_commandline = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Script = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin_portValue)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(214, 360);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 7;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 300;
            this.toolTip1.ShowAlways = true;
            // 
            // hostValue
            // 
            this.hostValue.Location = new System.Drawing.Point(74, 41);
            this.hostValue.Name = "hostValue";
            this.hostValue.Size = new System.Drawing.Size(106, 20);
            this.hostValue.TabIndex = 3;
            this.hostValue.Text = "localhost";
            this.toolTip1.SetToolTip(this.hostValue, "host name or IP address\r\nEnter localhost to run scripts on the local machine.\r\n");
            this.hostValue.TextChanged += new System.EventHandler(this.host_TextChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Script});
            this.dataGridView1.Location = new System.Drawing.Point(12, 209);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(277, 145);
            this.dataGridView1.TabIndex = 5;
            this.toolTip1.SetToolTip(this.dataGridView1, "For scripts on a remote system you must manually add the script\r\nnames in the sel" +
        "ected suite\r\n\r\n  Click in a empty row to add a new entry.\r\n  Double click a scri" +
        "pt name to edit it.\r\n");
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // button_addScript
            // 
            this.button_addScript.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_addScript.Enabled = false;
            this.button_addScript.Location = new System.Drawing.Point(295, 230);
            this.button_addScript.Name = "button_addScript";
            this.button_addScript.Size = new System.Drawing.Size(75, 37);
            this.button_addScript.TabIndex = 4;
            this.button_addScript.Text = "&Add scripts...";
            this.toolTip1.SetToolTip(this.button_addScript, "add eggPlant Functional scripts from the selected suite on the local filesystem");
            this.button_addScript.UseVisualStyleBackColor = true;
            this.button_addScript.Click += new System.EventHandler(this.addScript_button_Click);
            // 
            // SuitePath_text
            // 
            this.SuitePath_text.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SuitePath_text.Location = new System.Drawing.Point(12, 176);
            this.SuitePath_text.Name = "SuitePath_text";
            this.SuitePath_text.Size = new System.Drawing.Size(322, 20);
            this.SuitePath_text.TabIndex = 2;
            this.toolTip1.SetToolTip(this.SuitePath_text, "The  path to the suite directory.\r\nIf the suite is on a remote system you must en" +
        "ter\r\nthe path on that system.");
            this.SuitePath_text.TextChanged += new System.EventHandler(this.SuitePath_text_TextChanged);
            // 
            // button_suitePath
            // 
            this.button_suitePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_suitePath.Location = new System.Drawing.Point(340, 176);
            this.button_suitePath.Name = "button_suitePath";
            this.button_suitePath.Size = new System.Drawing.Size(30, 21);
            this.button_suitePath.TabIndex = 3;
            this.button_suitePath.Text = "...";
            this.button_suitePath.UseVisualStyleBackColor = true;
            this.button_suitePath.Click += new System.EventHandler(this.suitePath_btn_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(295, 360);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 8;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog_suite
            // 
            this.openFileDialog_suite.FileName = "*.suite";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Path to the eggPlant Functional suite:";
            // 
            // spin_portValue
            // 
            this.spin_portValue.Location = new System.Drawing.Point(221, 41);
            this.spin_portValue.Maximum = new decimal(new int[] {
            49151,
            0,
            0,
            0});
            this.spin_portValue.Name = "spin_portValue";
            this.spin_portValue.Size = new System.Drawing.Size(64, 20);
            this.spin_portValue.TabIndex = 4;
            this.spin_portValue.Value = new decimal(new int[] {
            5400,
            0,
            0,
            0});
            // 
            // label_port
            // 
            this.label_port.AutoSize = true;
            this.label_port.Location = new System.Drawing.Point(186, 44);
            this.label_port.Name = "label_port";
            this.label_port.Size = new System.Drawing.Size(29, 13);
            this.label_port.TabIndex = 6;
            this.label_port.Text = "&Port:";
            // 
            // button_removeScript
            // 
            this.button_removeScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_removeScript.Enabled = false;
            this.button_removeScript.Location = new System.Drawing.Point(295, 273);
            this.button_removeScript.Name = "button_removeScript";
            this.button_removeScript.Size = new System.Drawing.Size(75, 37);
            this.button_removeScript.TabIndex = 6;
            this.button_removeScript.Text = "&Remove scripts";
            this.button_removeScript.UseVisualStyleBackColor = true;
            this.button_removeScript.Click += new System.EventHandler(this.button_removeScript_Click);
            // 
            // label_host
            // 
            this.label_host.AutoSize = true;
            this.label_host.Location = new System.Drawing.Point(36, 44);
            this.label_host.Name = "label_host";
            this.label_host.Size = new System.Drawing.Size(32, 13);
            this.label_host.TabIndex = 13;
            this.label_host.Text = "&Host:";
            // 
            // linklabel_drive
            // 
            this.linklabel_drive.AutoSize = true;
            this.linklabel_drive.Location = new System.Drawing.Point(232, 22);
            this.linklabel_drive.Name = "linklabel_drive";
            this.linklabel_drive.Size = new System.Drawing.Size(94, 13);
            this.linklabel_drive.TabIndex = 2;
            this.linklabel_drive.TabStop = true;
            this.linklabel_drive.Text = "More information...";
            this.linklabel_drive.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.doclink_LinkClicked);
            // 
            // radioButton_local
            // 
            this.radioButton_local.AutoSize = true;
            this.radioButton_local.Location = new System.Drawing.Point(7, 78);
            this.radioButton_local.Name = "radioButton_local";
            this.radioButton_local.Size = new System.Drawing.Size(179, 17);
            this.radioButton_local.TabIndex = 5;
            this.radioButton_local.Text = "Run scripts on the local machine";
            this.radioButton_local.UseVisualStyleBackColor = true;
            this.radioButton_local.CheckedChanged += new System.EventHandler(this.radioButton_local_CheckedChanged);
            // 
            // radioButton_drive
            // 
            this.radioButton_drive.AutoSize = true;
            this.radioButton_drive.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.radioButton_drive.Checked = true;
            this.radioButton_drive.Location = new System.Drawing.Point(7, 20);
            this.radioButton_drive.Name = "radioButton_drive";
            this.radioButton_drive.Size = new System.Drawing.Size(219, 17);
            this.radioButton_drive.TabIndex = 1;
            this.radioButton_drive.TabStop = true;
            this.radioButton_drive.Text = "Run scripts on any machine via eggDrive";
            this.radioButton_drive.UseVisualStyleBackColor = true;
            // 
            // linkLabel_commandline
            // 
            this.linkLabel_commandline.AutoSize = true;
            this.linkLabel_commandline.Location = new System.Drawing.Point(186, 80);
            this.linkLabel_commandline.Name = "linkLabel_commandline";
            this.linkLabel_commandline.Size = new System.Drawing.Size(94, 13);
            this.linkLabel_commandline.TabIndex = 6;
            this.linkLabel_commandline.TabStop = true;
            this.linkLabel_commandline.Text = "More information...";
            this.linkLabel_commandline.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_commandline_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabel_commandline);
            this.groupBox1.Controls.Add(this.radioButton_drive);
            this.groupBox1.Controls.Add(this.radioButton_local);
            this.groupBox1.Controls.Add(this.linklabel_drive);
            this.groupBox1.Controls.Add(this.label_host);
            this.groupBox1.Controls.Add(this.label_port);
            this.groupBox1.Controls.Add(this.spin_portValue);
            this.groupBox1.Controls.Add(this.hostValue);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 107);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Local or remote execution";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(353, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Use this tool to create Generic Tests that run eggPlant Functional Scripts.";
            // 
            // Script
            // 
            this.Script.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Script.HeaderText = "Script";
            this.Script.Name = "Script";
            this.Script.ToolTipText = " ";
            // 
            // ePF_TestCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 395);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_removeScript);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button_addScript);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.SuitePath_text);
            this.Controls.Add(this.button_suitePath);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 434);
            this.Name = "ePF_TestCreation";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "eggPlant Functional Generic Test Creation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ePF_TestCreation_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin_portValue)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button_suitePath;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_addScript;
        private System.Windows.Forms.OpenFileDialog openFileDialog_suite;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_suite;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox SuitePath_text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown spin_portValue;
        private System.Windows.Forms.Label label_port;
        private System.Windows.Forms.TextBox hostValue;
        private System.Windows.Forms.Button button_removeScript;
        private System.Windows.Forms.Label label_host;
        private System.Windows.Forms.LinkLabel linklabel_drive;
        private System.Windows.Forms.RadioButton radioButton_local;
        private System.Windows.Forms.RadioButton radioButton_drive;
        private System.Windows.Forms.LinkLabel linkLabel_commandline;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.DataGridViewTextBoxColumn Script;
    }
}
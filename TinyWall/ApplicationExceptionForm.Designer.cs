namespace pylorak.TinyWall
{
    partial class ApplicationExceptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationExceptionForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnChooseService = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbTimer = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.transparentLabel1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpAllowedHosts = new System.Windows.Forms.GroupBox();
            this.radOnlySpecifiedHosts = new System.Windows.Forms.RadioButton();
            this.radAllowAnyHost = new System.Windows.Forms.RadioButton();
            this.panelHostButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddHost = new System.Windows.Forms.Button();
            this.btnModifyHost = new System.Windows.Forms.Button();
            this.btnRemoveHost = new System.Windows.Forms.Button();
            this.btnRemoveAllHosts = new System.Windows.Forms.Button();
            this.listAllowedHosts = new System.Windows.Forms.ListView();
            this.columnHeaderHost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoveSoftware = new System.Windows.Forms.Button();
            this.listViewAppPath = new System.Windows.Forms.ListView();
            this.columnHeaderApplication = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSelectUwpApp = new System.Windows.Forms.Button();
            this.chkRestrictToLocalNetwork = new System.Windows.Forms.CheckBox();
            this.radBlock = new System.Windows.Forms.RadioButton();
            this.radTcpUdpOut = new System.Windows.Forms.RadioButton();
            this.radTcpUdpUnrestricted = new System.Windows.Forms.RadioButton();
            this.radUnrestricted = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.txtListenPortUDP = new System.Windows.Forms.TextBox();
            this.txtListenPortTCP = new System.Windows.Forms.TextBox();
            this.InTCPLabel = new System.Windows.Forms.Label();
            this.InUDPLabel = new System.Windows.Forms.Label();
            this.txtOutboundPortUDP = new System.Windows.Forms.TextBox();
            this.txtOutboundPortTCP = new System.Windows.Forms.TextBox();
            this.OutTCPLabel = new System.Windows.Forms.Label();
            this.OutUDPLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.radOnlySpecifiedPorts = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkInheritToChildren = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.grpAllowedHosts.SuspendLayout();
            this.panelHostButtons.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::pylorak.TinyWall.Resources.Icons.cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Image = global::pylorak.TinyWall.Resources.Icons.accept;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.BackColor = System.Drawing.Color.AliceBlue;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnProcess
            // 
            resources.ApplyResources(this.btnProcess, "btnProcess");
            this.btnProcess.BackColor = System.Drawing.Color.AliceBlue;
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnChooseService
            // 
            resources.ApplyResources(this.btnChooseService, "btnChooseService");
            this.btnChooseService.BackColor = System.Drawing.Color.AliceBlue;
            this.btnChooseService.Name = "btnChooseService";
            this.btnChooseService.UseVisualStyleBackColor = false;
            this.btnChooseService.Click += new System.EventHandler(this.btnChooseService_Click);
            // 
            // ofd
            // 
            resources.ApplyResources(this.ofd, "ofd");
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.SystemColors.Window;
            this.label6.Name = "label6";
            // 
            // cmbTimer
            // 
            resources.ApplyResources(this.cmbTimer, "cmbTimer");
            this.cmbTimer.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTimer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimer.FormattingEnabled = true;
            this.cmbTimer.Name = "cmbTimer";
            this.cmbTimer.SelectedIndexChanged += new System.EventHandler(this.cmbTimer_SelectedIndexChanged);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = global::pylorak.TinyWall.Resources.Icons.green_banner;
            this.panel1.Controls.Add(this.transparentLabel1);
            this.panel1.Name = "panel1";
            // 
            // transparentLabel1
            // 
            resources.ApplyResources(this.transparentLabel1, "transparentLabel1");
            this.transparentLabel1.BackColor = System.Drawing.Color.Transparent;
            this.transparentLabel1.ForeColor = System.Drawing.Color.White;
            this.transparentLabel1.Name = "transparentLabel1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.grpAllowedHosts);
            this.panel2.Controls.Add(this.btnRemoveSoftware);
            this.panel2.Controls.Add(this.listViewAppPath);
            this.panel2.Controls.Add(this.btnSelectUwpApp);
            this.panel2.Controls.Add(this.cmbTimer);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.btnBrowse);
            this.panel2.Controls.Add(this.btnProcess);
            this.panel2.Controls.Add(this.btnChooseService);
            this.panel2.Name = "panel2";
            //
            // grpAllowedHosts
            //
            this.grpAllowedHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAllowedHosts.Controls.Add(this.radOnlySpecifiedHosts);
            this.grpAllowedHosts.Controls.Add(this.radAllowAnyHost);
            this.grpAllowedHosts.Controls.Add(this.panelHostButtons);
            this.grpAllowedHosts.Controls.Add(this.listAllowedHosts);
            this.grpAllowedHosts.Enabled = false;
            this.grpAllowedHosts.Location = new System.Drawing.Point(552, 262);
            this.grpAllowedHosts.Name = "grpAllowedHosts";
            this.grpAllowedHosts.Size = new System.Drawing.Size(256, 282);
            this.grpAllowedHosts.TabIndex = 8;
            this.grpAllowedHosts.TabStop = false;
            this.grpAllowedHosts.Text = "Allowed IPs/Hosts";
            //
            // radOnlySpecifiedHosts
            //
            this.radOnlySpecifiedHosts.AutoSize = true;
            this.radOnlySpecifiedHosts.Location = new System.Drawing.Point(13, 45);
            this.radOnlySpecifiedHosts.Name = "radOnlySpecifiedHosts";
            this.radOnlySpecifiedHosts.Size = new System.Drawing.Size(158, 19);
            this.radOnlySpecifiedHosts.TabIndex = 2;
            this.radOnlySpecifiedHosts.Text = "Only allow specified hosts";
            this.radOnlySpecifiedHosts.UseVisualStyleBackColor = true;
            this.radOnlySpecifiedHosts.CheckedChanged += new System.EventHandler(this.AllowedHostOption_CheckedChanged);
            //
            // radAllowAnyHost
            //
            this.radAllowAnyHost.AutoSize = true;
            this.radAllowAnyHost.Checked = true;
            this.radAllowAnyHost.Location = new System.Drawing.Point(13, 22);
            this.radAllowAnyHost.Name = "radAllowAnyHost";
            this.radAllowAnyHost.Size = new System.Drawing.Size(109, 19);
            this.radAllowAnyHost.TabIndex = 1;
            this.radAllowAnyHost.TabStop = true;
            this.radAllowAnyHost.Text = "Allow any hosts";
            this.radAllowAnyHost.UseVisualStyleBackColor = true;
            this.radAllowAnyHost.CheckedChanged += new System.EventHandler(this.AllowedHostOption_CheckedChanged);
            //
            // panelHostButtons
            //
            this.panelHostButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHostButtons.AutoSize = true;
            this.panelHostButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelHostButtons.Controls.Add(this.btnAddHost);
            this.panelHostButtons.Controls.Add(this.btnModifyHost);
            this.panelHostButtons.Controls.Add(this.btnRemoveHost);
            this.panelHostButtons.Controls.Add(this.btnRemoveAllHosts);
            this.panelHostButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelHostButtons.Location = new System.Drawing.Point(173, 72);
            this.panelHostButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelHostButtons.Name = "panelHostButtons";
            this.panelHostButtons.Size = new System.Drawing.Size(0, 0);
            this.panelHostButtons.TabIndex = 4;
            this.panelHostButtons.WrapContents = false;
            //
            // btnAddHost
            //
            this.btnAddHost.AutoSize = true;
            this.btnAddHost.BackColor = System.Drawing.Color.AliceBlue;
            this.btnAddHost.FlatAppearance.BorderSize = 0;
            this.btnAddHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddHost.Image = global::pylorak.TinyWall.Resources.Icons.add;
            this.btnAddHost.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddHost.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnAddHost.Name = "btnAddHost";
            this.btnAddHost.Size = new System.Drawing.Size(111, 27);
            this.btnAddHost.TabIndex = 0;
            this.btnAddHost.Text = "Add IP/Host";
            this.btnAddHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddHost.UseVisualStyleBackColor = false;
            this.btnAddHost.Click += new System.EventHandler(this.btnAddHost_Click);
            //
            // btnModifyHost
            //
            this.btnModifyHost.AutoSize = true;
            this.btnModifyHost.BackColor = System.Drawing.Color.AliceBlue;
            this.btnModifyHost.Enabled = false;
            this.btnModifyHost.FlatAppearance.BorderSize = 0;
            this.btnModifyHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModifyHost.Image = global::pylorak.TinyWall.Resources.Icons.modify;
            this.btnModifyHost.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModifyHost.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnModifyHost.Name = "btnModifyHost";
            this.btnModifyHost.Size = new System.Drawing.Size(82, 27);
            this.btnModifyHost.TabIndex = 1;
            this.btnModifyHost.Text = "Modify";
            this.btnModifyHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnModifyHost.UseVisualStyleBackColor = false;
            this.btnModifyHost.Click += new System.EventHandler(this.btnModifyHost_Click);
            //
            // btnRemoveHost
            //
            this.btnRemoveHost.AutoSize = true;
            this.btnRemoveHost.BackColor = System.Drawing.Color.AliceBlue;
            this.btnRemoveHost.Enabled = false;
            this.btnRemoveHost.FlatAppearance.BorderSize = 0;
            this.btnRemoveHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveHost.Image = global::pylorak.TinyWall.Resources.Icons.remove;
            this.btnRemoveHost.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveHost.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.btnRemoveHost.Name = "btnRemoveHost";
            this.btnRemoveHost.Size = new System.Drawing.Size(91, 27);
            this.btnRemoveHost.TabIndex = 2;
            this.btnRemoveHost.Text = "Remove";
            this.btnRemoveHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRemoveHost.UseVisualStyleBackColor = false;
            this.btnRemoveHost.Click += new System.EventHandler(this.btnRemoveHost_Click);
            //
            // btnRemoveAllHosts
            //
            this.btnRemoveAllHosts.AutoSize = true;
            this.btnRemoveAllHosts.BackColor = System.Drawing.Color.AliceBlue;
            this.btnRemoveAllHosts.Enabled = false;
            this.btnRemoveAllHosts.FlatAppearance.BorderSize = 0;
            this.btnRemoveAllHosts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveAllHosts.Image = global::pylorak.TinyWall.Resources.Icons.delete;
            this.btnRemoveAllHosts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveAllHosts.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemoveAllHosts.Name = "btnRemoveAllHosts";
            this.btnRemoveAllHosts.Size = new System.Drawing.Size(110, 27);
            this.btnRemoveAllHosts.TabIndex = 3;
            this.btnRemoveAllHosts.Text = "Remove all";
            this.btnRemoveAllHosts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRemoveAllHosts.UseVisualStyleBackColor = false;
            this.btnRemoveAllHosts.Click += new System.EventHandler(this.btnRemoveAllHosts_Click);
            //
            // listAllowedHosts
            //
            this.listAllowedHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listAllowedHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderHost});
            this.listAllowedHosts.FullRowSelect = true;
            this.listAllowedHosts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listAllowedHosts.HideSelection = false;
            this.listAllowedHosts.Location = new System.Drawing.Point(13, 72);
            this.listAllowedHosts.MultiSelect = false;
            this.listAllowedHosts.Name = "listAllowedHosts";
            this.listAllowedHosts.ShowGroups = false;
            this.listAllowedHosts.Size = new System.Drawing.Size(151, 197);
            this.listAllowedHosts.TabIndex = 3;
            this.listAllowedHosts.UseCompatibleStateImageBehavior = false;
            this.listAllowedHosts.View = System.Windows.Forms.View.Details;
            this.listAllowedHosts.SelectedIndexChanged += new System.EventHandler(this.listAllowedHosts_SelectedIndexChanged);
            this.listAllowedHosts.DoubleClick += new System.EventHandler(this.listAllowedHosts_DoubleClick);
            //
            // columnHeaderHost
            //
            this.columnHeaderHost.Text = "IP/Host";
            this.columnHeaderHost.Width = 140;
            //
            // btnRemoveSoftware
            //
            resources.ApplyResources(this.btnRemoveSoftware, "btnRemoveSoftware");
            this.btnRemoveSoftware.BackColor = System.Drawing.Color.AliceBlue;
            this.btnRemoveSoftware.Name = "btnRemoveSoftware";
            this.btnRemoveSoftware.UseVisualStyleBackColor = false;
            this.btnRemoveSoftware.Click += new System.EventHandler(this.btnRemoveSoftware_Click);
            // 
            // listViewAppPath
            // 
            resources.ApplyResources(this.listViewAppPath, "listViewAppPath");
            this.listViewAppPath.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderApplication,
            this.columnHeaderType});
            this.listViewAppPath.FullRowSelect = true;
            this.listViewAppPath.GridLines = true;
            this.listViewAppPath.HideSelection = false;
            this.listViewAppPath.Name = "listViewAppPath";
            this.listViewAppPath.UseCompatibleStateImageBehavior = false;
            this.listViewAppPath.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderApplication
            // 
            resources.ApplyResources(this.columnHeaderApplication, "columnHeaderApplication");
            // 
            // columnHeaderType
            // 
            resources.ApplyResources(this.columnHeaderType, "columnHeaderType");
            // 
            // btnSelectUwpApp
            // 
            resources.ApplyResources(this.btnSelectUwpApp, "btnSelectUwpApp");
            this.btnSelectUwpApp.BackColor = System.Drawing.Color.AliceBlue;
            this.btnSelectUwpApp.Name = "btnSelectUwpApp";
            this.btnSelectUwpApp.UseVisualStyleBackColor = false;
            this.btnSelectUwpApp.Click += new System.EventHandler(this.btnSelectUwpApp_Click);
            // 
            // chkRestrictToLocalNetwork
            // 
            resources.ApplyResources(this.chkRestrictToLocalNetwork, "chkRestrictToLocalNetwork");
            this.chkRestrictToLocalNetwork.Name = "chkRestrictToLocalNetwork";
            this.chkRestrictToLocalNetwork.UseVisualStyleBackColor = true;
            // 
            // radBlock
            // 
            resources.ApplyResources(this.radBlock, "radBlock");
            this.radBlock.Name = "radBlock";
            this.radBlock.TabStop = true;
            this.radBlock.UseVisualStyleBackColor = true;
            this.radBlock.CheckedChanged += new System.EventHandler(this.radRestriction_CheckedChanged);
            // 
            // radTcpUdpOut
            // 
            resources.ApplyResources(this.radTcpUdpOut, "radTcpUdpOut");
            this.radTcpUdpOut.Name = "radTcpUdpOut";
            this.radTcpUdpOut.TabStop = true;
            this.radTcpUdpOut.UseVisualStyleBackColor = true;
            this.radTcpUdpOut.CheckedChanged += new System.EventHandler(this.radRestriction_CheckedChanged);
            // 
            // radTcpUdpUnrestricted
            // 
            resources.ApplyResources(this.radTcpUdpUnrestricted, "radTcpUdpUnrestricted");
            this.radTcpUdpUnrestricted.Checked = true;
            this.radTcpUdpUnrestricted.Name = "radTcpUdpUnrestricted";
            this.radTcpUdpUnrestricted.TabStop = true;
            this.radTcpUdpUnrestricted.UseVisualStyleBackColor = true;
            this.radTcpUdpUnrestricted.CheckedChanged += new System.EventHandler(this.radRestriction_CheckedChanged);
            // 
            // radUnrestricted
            // 
            resources.ApplyResources(this.radUnrestricted, "radUnrestricted");
            this.radUnrestricted.Name = "radUnrestricted";
            this.radUnrestricted.TabStop = true;
            this.radUnrestricted.UseVisualStyleBackColor = true;
            this.radUnrestricted.CheckedChanged += new System.EventHandler(this.radRestriction_CheckedChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txtListenPortUDP
            // 
            resources.ApplyResources(this.txtListenPortUDP, "txtListenPortUDP");
            this.txtListenPortUDP.Name = "txtListenPortUDP";
            // 
            // txtListenPortTCP
            // 
            resources.ApplyResources(this.txtListenPortTCP, "txtListenPortTCP");
            this.txtListenPortTCP.Name = "txtListenPortTCP";
            // 
            // InTCPLabel
            // 
            resources.ApplyResources(this.InTCPLabel, "InTCPLabel");
            this.InTCPLabel.Name = "InTCPLabel";
            // 
            // InUDPLabel
            // 
            resources.ApplyResources(this.InUDPLabel, "InUDPLabel");
            this.InUDPLabel.Name = "InUDPLabel";
            // 
            // txtOutboundPortUDP
            // 
            resources.ApplyResources(this.txtOutboundPortUDP, "txtOutboundPortUDP");
            this.txtOutboundPortUDP.Name = "txtOutboundPortUDP";
            // 
            // txtOutboundPortTCP
            // 
            resources.ApplyResources(this.txtOutboundPortTCP, "txtOutboundPortTCP");
            this.txtOutboundPortTCP.Name = "txtOutboundPortTCP";
            // 
            // OutTCPLabel
            // 
            resources.ApplyResources(this.OutTCPLabel, "OutTCPLabel");
            this.OutTCPLabel.Name = "OutTCPLabel";
            // 
            // OutUDPLabel
            // 
            resources.ApplyResources(this.OutUDPLabel, "OutUDPLabel");
            this.OutUDPLabel.Name = "OutUDPLabel";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Name = "label10";
            // 
            // radOnlySpecifiedPorts
            // 
            resources.ApplyResources(this.radOnlySpecifiedPorts, "radOnlySpecifiedPorts");
            this.radOnlySpecifiedPorts.Name = "radOnlySpecifiedPorts";
            this.radOnlySpecifiedPorts.TabStop = true;
            this.radOnlySpecifiedPorts.UseVisualStyleBackColor = true;
            this.radOnlySpecifiedPorts.CheckedChanged += new System.EventHandler(this.radRestriction_CheckedChanged);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.OutTCPLabel);
            this.panel3.Controls.Add(this.txtListenPortUDP);
            this.panel3.Controls.Add(this.txtOutboundPortTCP);
            this.panel3.Controls.Add(this.txtOutboundPortUDP);
            this.panel3.Controls.Add(this.OutUDPLabel);
            this.panel3.Controls.Add(this.InUDPLabel);
            this.panel3.Controls.Add(this.InTCPLabel);
            this.panel3.Controls.Add(this.txtListenPortTCP);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Name = "panel3";
            // 
            // chkInheritToChildren
            // 
            resources.ApplyResources(this.chkInheritToChildren, "chkInheritToChildren");
            this.chkInheritToChildren.Name = "chkInheritToChildren";
            this.chkInheritToChildren.UseVisualStyleBackColor = true;
            // 
            // ApplicationExceptionForm
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ControlBox = false;
            this.Controls.Add(this.chkInheritToChildren);
            this.Controls.Add(this.radOnlySpecifiedPorts);
            this.Controls.Add(this.radUnrestricted);
            this.Controls.Add(this.radTcpUdpUnrestricted);
            this.Controls.Add(this.radTcpUdpOut);
            this.Controls.Add(this.radBlock);
            this.Controls.Add(this.chkRestrictToLocalNetwork);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ApplicationExceptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ApplicationExceptionForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.grpAllowedHosts.ResumeLayout(false);
            this.grpAllowedHosts.PerformLayout();
            this.panelHostButtons.ResumeLayout(false);
            this.panelHostButtons.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnChooseService;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbTimer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label transparentLabel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkRestrictToLocalNetwork;
        private System.Windows.Forms.RadioButton radBlock;
        private System.Windows.Forms.RadioButton radTcpUdpOut;
        private System.Windows.Forms.RadioButton radTcpUdpUnrestricted;
        private System.Windows.Forms.RadioButton radUnrestricted;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtListenPortUDP;
        private System.Windows.Forms.TextBox txtListenPortTCP;
        private System.Windows.Forms.Label InTCPLabel;
        private System.Windows.Forms.Label InUDPLabel;
        private System.Windows.Forms.TextBox txtOutboundPortUDP;
        private System.Windows.Forms.TextBox txtOutboundPortTCP;
        private System.Windows.Forms.Label OutTCPLabel;
        private System.Windows.Forms.Label OutUDPLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton radOnlySpecifiedPorts;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkInheritToChildren;
        private System.Windows.Forms.Button btnSelectUwpApp;
        private System.Windows.Forms.ListView listViewAppPath;
        private System.Windows.Forms.Button btnRemoveSoftware;
        private System.Windows.Forms.ColumnHeader columnHeaderApplication;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.GroupBox grpAllowedHosts;
        private System.Windows.Forms.FlowLayoutPanel panelHostButtons;
        private System.Windows.Forms.Button btnAddHost;
        private System.Windows.Forms.Button btnModifyHost;
        private System.Windows.Forms.Button btnRemoveHost;
        private System.Windows.Forms.Button btnRemoveAllHosts;
        private System.Windows.Forms.ListView listAllowedHosts;
        private System.Windows.Forms.ColumnHeader columnHeaderHost;
        private System.Windows.Forms.RadioButton radAllowAnyHost;
        private System.Windows.Forms.RadioButton radOnlySpecifiedHosts;
    }
}
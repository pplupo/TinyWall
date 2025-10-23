﻿namespace pylorak.TinyWall
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtExceptionListFilter = new System.Windows.Forms.TextBox();
            this.btnAppRemoveAll = new System.Windows.Forms.Button();
            this.btnAppAutoDetect = new System.Windows.Forms.Button();
            this.btnSubmitAssoc = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAppRemove = new System.Windows.Forms.Button();
            this.btnAppModify = new System.Windows.Forms.Button();
            this.btnAppAdd = new System.Windows.Forms.Button();
            this.listApplications = new System.Windows.Forms.ListView();
            this.columnApp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDetails = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLastModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IconList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listOptionalGlobalProfiles = new System.Windows.Forms.CheckedListBox();
            this.listRecommendedGlobalProfiles = new System.Windows.Forms.CheckedListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.chkHostsBlocklist = new System.Windows.Forms.CheckBox();
            this.chkEnableBlocklists = new System.Windows.Forms.CheckBox();
            this.chkBlockMalwarePorts = new System.Windows.Forms.CheckBox();
            this.chkDisplayOffBlock = new System.Windows.Forms.CheckBox();
            this.chkLockHostsFile = new System.Windows.Forms.CheckBox();
            this.comboLanguages = new System.Windows.Forms.ComboBox();
            this.chkEnableHotkeys = new System.Windows.Forms.CheckBox();
            this.chkAutoUpdateCheck = new System.Windows.Forms.CheckBox();
            this.chkAskForExceptionDetails = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkChangePassword = new System.Windows.Forms.CheckBox();
            this.txtPasswordAgain = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnGithub = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblLinkAttributions = new System.Windows.Forms.LinkLabel();
            this.btnDonate = new System.Windows.Forms.PictureBox();
            this.lblLinkLicense = new System.Windows.Forms.LinkLabel();
            this.label10 = new System.Windows.Forms.Label();
            this.lblAboutHomepageLink = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnWeb = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDonate)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtExceptionListFilter);
            this.tabPage3.Controls.Add(this.btnAppRemoveAll);
            this.tabPage3.Controls.Add(this.btnAppAutoDetect);
            this.tabPage3.Controls.Add(this.btnSubmitAssoc);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.btnAppRemove);
            this.tabPage3.Controls.Add(this.btnAppModify);
            this.tabPage3.Controls.Add(this.btnAppAdd);
            this.tabPage3.Controls.Add(this.listApplications);
            this.tabPage3.Controls.Add(this.label4);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtExceptionListFilter
            // 
            resources.ApplyResources(this.txtExceptionListFilter, "txtExceptionListFilter");
            this.txtExceptionListFilter.Name = "txtExceptionListFilter";
            this.txtExceptionListFilter.TextChanged += new System.EventHandler(this.txtExceptionListFilter_TextChanged);
            // 
            // btnAppRemoveAll
            // 
            resources.ApplyResources(this.btnAppRemoveAll, "btnAppRemoveAll");
            this.btnAppRemoveAll.Name = "btnAppRemoveAll";
            this.btnAppRemoveAll.UseVisualStyleBackColor = true;
            this.btnAppRemoveAll.Click += new System.EventHandler(this.btnAppRemoveAll_Click);
            // 
            // btnAppAutoDetect
            // 
            resources.ApplyResources(this.btnAppAutoDetect, "btnAppAutoDetect");
            this.btnAppAutoDetect.Name = "btnAppAutoDetect";
            this.btnAppAutoDetect.UseVisualStyleBackColor = true;
            this.btnAppAutoDetect.Click += new System.EventHandler(this.btnAppAutoDetect_Click);
            // 
            // btnSubmitAssoc
            // 
            resources.ApplyResources(this.btnSubmitAssoc, "btnSubmitAssoc");
            this.btnSubmitAssoc.Name = "btnSubmitAssoc";
            this.btnSubmitAssoc.UseVisualStyleBackColor = true;
            this.btnSubmitAssoc.Click += new System.EventHandler(this.btnSubmitAssoc_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnAppRemove
            // 
            resources.ApplyResources(this.btnAppRemove, "btnAppRemove");
            this.btnAppRemove.Name = "btnAppRemove";
            this.btnAppRemove.UseVisualStyleBackColor = true;
            this.btnAppRemove.Click += new System.EventHandler(this.btnAppRemove_Click);
            // 
            // btnAppModify
            // 
            resources.ApplyResources(this.btnAppModify, "btnAppModify");
            this.btnAppModify.Name = "btnAppModify";
            this.btnAppModify.UseVisualStyleBackColor = true;
            this.btnAppModify.Click += new System.EventHandler(this.btnAppModify_Click);
            // 
            // btnAppAdd
            // 
            resources.ApplyResources(this.btnAppAdd, "btnAppAdd");
            this.btnAppAdd.Name = "btnAppAdd";
            this.btnAppAdd.UseVisualStyleBackColor = true;
            this.btnAppAdd.Click += new System.EventHandler(this.btnAppAdd_Click);
            // 
            // listApplications
            // 
            resources.ApplyResources(this.listApplications, "listApplications");
            this.listApplications.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnApp,
            this.columnType,
            this.columnDetails,
            this.columnLastModified});
            this.listApplications.FullRowSelect = true;
            this.listApplications.GridLines = true;
            this.listApplications.HideSelection = false;
            this.listApplications.Name = "listApplications";
            this.listApplications.SmallImageList = this.IconList;
            this.listApplications.UseCompatibleStateImageBehavior = false;
            this.listApplications.View = System.Windows.Forms.View.Details;
            this.listApplications.VirtualMode = true;
            this.listApplications.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listApplications_ColumnClick);
            this.listApplications.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listApplications_RetrieveVirtualItem);
            this.listApplications.SelectedIndexChanged += new System.EventHandler(this.listApplications_SelectedIndexChanged);
            this.listApplications.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.listApplications_VirtualItemsSelectionRangeChanged);
            this.listApplications.DoubleClick += new System.EventHandler(this.listApplications_DoubleClick);
            // 
            // columnApp
            // 
            this.columnApp.Tag = "colApplication";
            resources.ApplyResources(this.columnApp, "columnApp");
            // 
            // columnType
            // 
            this.columnType.Tag = "colType";
            resources.ApplyResources(this.columnType, "columnType");
            // 
            // columnDetails
            // 
            this.columnDetails.Tag = "colDetails";
            resources.ApplyResources(this.columnDetails, "columnDetails");
            // 
            // columnLastModified
            // 
            this.columnLastModified.Tag = "colLastModified";
            // 
            // IconList
            // 
            this.IconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.IconList, "IconList");
            this.IconList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.listOptionalGlobalProfiles);
            this.tabPage2.Controls.Add(this.listRecommendedGlobalProfiles);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // listOptionalGlobalProfiles
            // 
            resources.ApplyResources(this.listOptionalGlobalProfiles, "listOptionalGlobalProfiles");
            this.listOptionalGlobalProfiles.CheckOnClick = true;
            this.listOptionalGlobalProfiles.FormattingEnabled = true;
            this.listOptionalGlobalProfiles.Name = "listOptionalGlobalProfiles";
            this.listOptionalGlobalProfiles.Sorted = true;
            this.listOptionalGlobalProfiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listOptionalGlobalProfiles_ItemCheck);
            // 
            // listRecommendedGlobalProfiles
            // 
            resources.ApplyResources(this.listRecommendedGlobalProfiles, "listRecommendedGlobalProfiles");
            this.listRecommendedGlobalProfiles.CheckOnClick = true;
            this.listRecommendedGlobalProfiles.FormattingEnabled = true;
            this.listRecommendedGlobalProfiles.Name = "listRecommendedGlobalProfiles";
            this.listRecommendedGlobalProfiles.Sorted = true;
            this.listRecommendedGlobalProfiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listRecommendedGlobalProfiles_ItemCheck);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkHostsBlocklist, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkEnableBlocklists, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkBlockMalwarePorts, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkDisplayOffBlock, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkLockHostsFile, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboLanguages, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkEnableHotkeys, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkAutoUpdateCheck, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkAskForExceptionDetails, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // chkHostsBlocklist
            // 
            resources.ApplyResources(this.chkHostsBlocklist, "chkHostsBlocklist");
            this.chkHostsBlocklist.Name = "chkHostsBlocklist";
            this.chkHostsBlocklist.UseVisualStyleBackColor = true;
            // 
            // chkEnableBlocklists
            // 
            resources.ApplyResources(this.chkEnableBlocklists, "chkEnableBlocklists");
            this.tableLayoutPanel1.SetColumnSpan(this.chkEnableBlocklists, 2);
            this.chkEnableBlocklists.Name = "chkEnableBlocklists";
            this.chkEnableBlocklists.UseVisualStyleBackColor = true;
            this.chkEnableBlocklists.CheckedChanged += new System.EventHandler(this.chkEnableBlocklists_CheckedChanged);
            // 
            // chkBlockMalwarePorts
            // 
            resources.ApplyResources(this.chkBlockMalwarePorts, "chkBlockMalwarePorts");
            this.chkBlockMalwarePorts.Name = "chkBlockMalwarePorts";
            this.chkBlockMalwarePorts.UseVisualStyleBackColor = true;
            // 
            // chkDisplayOffBlock
            // 
            resources.ApplyResources(this.chkDisplayOffBlock, "chkDisplayOffBlock");
            this.tableLayoutPanel1.SetColumnSpan(this.chkDisplayOffBlock, 2);
            this.chkDisplayOffBlock.Name = "chkDisplayOffBlock";
            this.chkDisplayOffBlock.UseVisualStyleBackColor = true;
            // 
            // chkLockHostsFile
            // 
            resources.ApplyResources(this.chkLockHostsFile, "chkLockHostsFile");
            this.tableLayoutPanel1.SetColumnSpan(this.chkLockHostsFile, 2);
            this.chkLockHostsFile.Name = "chkLockHostsFile";
            this.chkLockHostsFile.UseVisualStyleBackColor = true;
            // 
            // comboLanguages
            // 
            resources.ApplyResources(this.comboLanguages, "comboLanguages");
            this.comboLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLanguages.FormattingEnabled = true;
            this.comboLanguages.Name = "comboLanguages";
            // 
            // chkEnableHotkeys
            // 
            resources.ApplyResources(this.chkEnableHotkeys, "chkEnableHotkeys");
            this.tableLayoutPanel1.SetColumnSpan(this.chkEnableHotkeys, 2);
            this.chkEnableHotkeys.Name = "chkEnableHotkeys";
            this.chkEnableHotkeys.UseVisualStyleBackColor = true;
            // 
            // chkAutoUpdateCheck
            // 
            resources.ApplyResources(this.chkAutoUpdateCheck, "chkAutoUpdateCheck");
            this.tableLayoutPanel1.SetColumnSpan(this.chkAutoUpdateCheck, 2);
            this.chkAutoUpdateCheck.Name = "chkAutoUpdateCheck";
            this.chkAutoUpdateCheck.UseVisualStyleBackColor = true;
            // 
            // chkAskForExceptionDetails
            // 
            resources.ApplyResources(this.chkAskForExceptionDetails, "chkAskForExceptionDetails");
            this.tableLayoutPanel1.SetColumnSpan(this.chkAskForExceptionDetails, 2);
            this.chkAskForExceptionDetails.Name = "chkAskForExceptionDetails";
            this.chkAskForExceptionDetails.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.chkChangePassword);
            this.groupBox1.Controls.Add(this.txtPasswordAgain);
            this.groupBox1.Controls.Add(this.txtPassword);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkChangePassword
            // 
            resources.ApplyResources(this.chkChangePassword, "chkChangePassword");
            this.chkChangePassword.Name = "chkChangePassword";
            this.chkChangePassword.UseVisualStyleBackColor = true;
            this.chkChangePassword.CheckedChanged += new System.EventHandler(this.chkEnablePassword_CheckedChanged);
            // 
            // txtPasswordAgain
            // 
            resources.ApplyResources(this.txtPasswordAgain, "txtPasswordAgain");
            this.txtPasswordAgain.Name = "txtPasswordAgain";
            this.txtPasswordAgain.UseSystemPasswordChar = true;
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnGithub);
            this.tabPage4.Controls.Add(this.btnImport);
            this.tabPage4.Controls.Add(this.btnExport);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.btnUpdate);
            this.tabPage4.Controls.Add(this.btnWeb);
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnGithub
            // 
            resources.ApplyResources(this.btnGithub, "btnGithub");
            this.btnGithub.Name = "btnGithub";
            this.btnGithub.UseVisualStyleBackColor = true;
            this.btnGithub.Click += new System.EventHandler(this.btnGithub_Click);
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblLinkAttributions);
            this.groupBox2.Controls.Add(this.btnDonate);
            this.groupBox2.Controls.Add(this.lblLinkLicense);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.lblAboutHomepageLink);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lblVersion);
            this.groupBox2.Controls.Add(this.label12);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // lblLinkAttributions
            // 
            resources.ApplyResources(this.lblLinkAttributions, "lblLinkAttributions");
            this.lblLinkAttributions.Name = "lblLinkAttributions";
            this.lblLinkAttributions.TabStop = true;
            this.lblLinkAttributions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLinkAttributions_LinkClicked);
            // 
            // btnDonate
            // 
            resources.ApplyResources(this.btnDonate, "btnDonate");
            this.btnDonate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDonate.BackColor = System.Drawing.Color.Transparent;
            this.btnDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.TabStop = false;
            this.btnDonate.Click += new System.EventHandler(this.btnDonate_Click);
            this.btnDonate.MouseEnter += new System.EventHandler(this.btnDonate_MouseEnter);
            this.btnDonate.MouseLeave += new System.EventHandler(this.btnDonate_MouseLeave);
            // 
            // lblLinkLicense
            // 
            resources.ApplyResources(this.lblLinkLicense, "lblLinkLicense");
            this.lblLinkLicense.Name = "lblLinkLicense";
            this.lblLinkLicense.TabStop = true;
            this.lblLinkLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLinkLicense_LinkClicked);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // lblAboutHomepageLink
            // 
            resources.ApplyResources(this.lblAboutHomepageLink, "lblAboutHomepageLink");
            this.lblAboutHomepageLink.Name = "lblAboutHomepageLink";
            this.lblAboutHomepageLink.TabStop = true;
            this.lblAboutHomepageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAboutHomepageLink_LinkClicked);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // btnUpdate
            // 
            resources.ApplyResources(this.btnUpdate, "btnUpdate");
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnWeb
            // 
            resources.ApplyResources(this.btnWeb, "btnWeb");
            this.btnWeb.Name = "btnWeb";
            this.btnWeb.UseVisualStyleBackColor = true;
            this.btnWeb.Click += new System.EventHandler(this.btnWeb_Click);
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "xml";
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SettingsForm_KeyDown);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDonate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAppRemove;
        private System.Windows.Forms.Button btnAppModify;
        private System.Windows.Forms.Button btnAppAdd;
        private System.Windows.Forms.ListView listApplications;
        private System.Windows.Forms.ColumnHeader columnApp;
        private System.Windows.Forms.ColumnHeader columnDetails;
        private System.Windows.Forms.ColumnHeader columnLastModified;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox listOptionalGlobalProfiles;
        private System.Windows.Forms.CheckedListBox listRecommendedGlobalProfiles;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox chkAskForExceptionDetails;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkChangePassword;
        private System.Windows.Forms.TextBox txtPasswordAgain;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSubmitAssoc;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnWeb;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox chkBlockMalwarePorts;
        private System.Windows.Forms.Button btnAppAutoDetect;
        private System.Windows.Forms.ImageList IconList;
        private System.Windows.Forms.CheckBox chkAutoUpdateCheck;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel lblAboutHomepageLink;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.LinkLabel lblLinkLicense;
        private System.Windows.Forms.PictureBox btnDonate;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.CheckBox chkLockHostsFile;
        private System.Windows.Forms.CheckBox chkHostsBlocklist;
        private System.Windows.Forms.Button btnAppRemoveAll;
        private System.Windows.Forms.TextBox txtExceptionListFilter;
        private System.Windows.Forms.CheckBox chkEnableBlocklists;
        private System.Windows.Forms.ComboBox comboLanguages;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkEnableHotkeys;
        private System.Windows.Forms.LinkLabel lblLinkAttributions;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.CheckBox chkDisplayOffBlock;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnGithub;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using pylorak.Windows;
using pylorak.TinyWall.DatabaseClasses;

namespace pylorak.TinyWall
{
    internal partial class ApplicationExceptionForm : Form
    {
        private static readonly char[] PORT_LIST_SEPARATORS = new char[] { ',' };

        private List<FirewallExceptionV3> TmpExceptionSettings = new();

        internal List<FirewallExceptionV3> ExceptionSettings
        {
            get { return TmpExceptionSettings; }
        }

        internal ApplicationExceptionForm(FirewallExceptionV3 fwex)
        {
            InitializeComponent();
            Utils.SetRightToLeft(this);

            try
            {
                Type type = transparentLabel1.GetType();
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo method = type.GetMethod("SetStyle", flags);

                if (method != null)
                {
                    object[] param = { ControlStyles.SupportsTransparentBackColor, true };
                    method.Invoke(transparentLabel1, param);
                }
            }
            catch
            {
                // Don't do anything, we are running in a trusted context.
            }

            this.Icon = Resources.Icons.firewall;
            this.btnOK.Image = GlobalInstances.ApplyBtnIcon;
            this.btnCancel.Image = GlobalInstances.CancelBtnIcon;

            this.TmpExceptionSettings.Add(fwex);

            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Width = this.Width;
            panel2.Location = new System.Drawing.Point(0, panel1.Height);
            panel2.Width = this.Width;

            cmbTimer.SuspendLayout();
            var timerTexts = new Dictionary<AppExceptionTimer, KeyValuePair<string, AppExceptionTimer>>
            {
                {
                    AppExceptionTimer.Permanent,
                    new KeyValuePair<string, AppExceptionTimer>(Resources.Messages.Permanent, AppExceptionTimer.Permanent)
                },
                {
                    AppExceptionTimer.Until_Reboot,
                    new KeyValuePair<string, AppExceptionTimer>(Resources.Messages.UntilReboot, AppExceptionTimer.Until_Reboot)
                },
                {
                    AppExceptionTimer.For_5_Minutes,
                    new KeyValuePair<string, AppExceptionTimer>(string.Format(CultureInfo.CurrentCulture, Resources.Messages.XMinutes, 5), AppExceptionTimer.For_5_Minutes)
                },
                {
                    AppExceptionTimer.For_30_Minutes,
                    new KeyValuePair<string, AppExceptionTimer>(string.Format(CultureInfo.CurrentCulture, Resources.Messages.XMinutes, 30), AppExceptionTimer.For_30_Minutes)
                },
                {
                    AppExceptionTimer.For_1_Hour,
                    new KeyValuePair<string, AppExceptionTimer>(string.Format(CultureInfo.CurrentCulture, Resources.Messages.XHour, 1), AppExceptionTimer.For_1_Hour)
                },
                {
                    AppExceptionTimer.For_4_Hours,
                    new KeyValuePair<string, AppExceptionTimer>(string.Format(CultureInfo.CurrentCulture, Resources.Messages.XHours, 4), AppExceptionTimer.For_4_Hours)
                },
                {
                    AppExceptionTimer.For_9_Hours,
                    new KeyValuePair<string, AppExceptionTimer>(string.Format(CultureInfo.CurrentCulture, Resources.Messages.XHours, 9), AppExceptionTimer.For_9_Hours)
                },
                {
                    AppExceptionTimer.For_24_Hours,
                    new KeyValuePair<string, AppExceptionTimer>(string.Format(CultureInfo.CurrentCulture, Resources.Messages.XHours, 24), AppExceptionTimer.For_24_Hours)
                }
            };

            foreach (AppExceptionTimer timerVal in Enum.GetValues(typeof(AppExceptionTimer)))
            {
                if (timerVal != AppExceptionTimer.Invalid)
                    cmbTimer.Items.Add(timerTexts[timerVal]);
            }
            cmbTimer.DisplayMember = "Key";
            cmbTimer.ValueMember = "Value";
            cmbTimer.ResumeLayout(true);
        }

        private void ApplicationExceptionForm_Load(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Display timer
            for (int i = 0; i < cmbTimer.Items.Count; ++i)
            {
                if (((KeyValuePair<string, AppExceptionTimer>)cmbTimer.Items[i]).Value == TmpExceptionSettings[0].Timer)
                {
                    cmbTimer.SelectedIndex = i;
                    break;
                }
            }

            var exeSubj = TmpExceptionSettings[0].Subject as ExecutableSubject;
            var srvSubj = TmpExceptionSettings[0].Subject as ServiceSubject;
            var uwpSubj = TmpExceptionSettings[0].Subject as AppContainerSubject;

            // Update top colored banner
            bool hasSignature = false;
            bool validSignature = false;
            if (exeSubj != null)
            {
                hasSignature = exeSubj.IsSigned;
                validSignature = exeSubj.CertValid;
            }
            else if (uwpSubj != null)
            {
                var packageList = new UwpPackageList();
                var package = packageList.FindPackage(uwpSubj.Sid);
                if (package.HasValue && (package.Value.Tampered != UwpPackageList.TamperedState.Unknown))
                {
                    hasSignature = true;
                    validSignature = (package.Value.Tampered == UwpPackageList.TamperedState.No);
                }
            }

            if (hasSignature && validSignature)
            {
                // Recognized app
                panel1.BackgroundImage = Resources.Icons.green_banner;
                transparentLabel1.Text = string.Format(CultureInfo.InvariantCulture, Resources.Messages.RecognizedApplication, TmpExceptionSettings[0].Subject.ToString());
            }
            else if (hasSignature && !validSignature)
            {
                // Recognized, but compromised app
                panel1.BackgroundImage = Resources.Icons.red_banner;
                transparentLabel1.Text = string.Format(CultureInfo.InvariantCulture, Resources.Messages.CompromisedApplication, TmpExceptionSettings[0].Subject.ToString());
            }
            else
            {
                // Unknown app
                panel1.BackgroundImage = Resources.Icons.blue_banner;
                transparentLabel1.Text = Resources.Messages.UnknownApplication;
            }

            Utils.CenterControlInParent(transparentLabel1);

            // Update subject fields
            switch (TmpExceptionSettings[0].Subject.SubjectType)
            {
                case SubjectType.Global:
                    txtAppPath.Text = Resources.Messages.AllApplications;
                    txtSrvName.Text = Resources.Messages.SubjectTypeGlobal;
                    break;
                case SubjectType.Executable:
                    txtAppPath.Text = exeSubj!.ExecutablePath;
                    txtSrvName.Text = Resources.Messages.SubjectTypeExecutable;
                    break;
                case SubjectType.Service:
                    txtAppPath.Text = srvSubj!.ServiceName + " (" + srvSubj.ExecutablePath + ")";
                    txtSrvName.Text = Resources.Messages.SubjectTypeService;
                    break;
                case SubjectType.AppContainer:
                    txtAppPath.Text = uwpSubj!.DisplayName;
                    txtSrvName.Text = Resources.Messages.SubjectTypeUwpApp;
                    break;
                default:
                    throw new NotImplementedException();
            }

            // Update rule/policy fields

            chkInheritToChildren.Checked = TmpExceptionSettings[0].ChildProcessesInherit;

            switch (TmpExceptionSettings[0].Policy.PolicyType)
            {
                case PolicyType.HardBlock:
                    ClearHostList();
                    radBlock.Checked = true;
                    radRestriction_CheckedChanged(this, EventArgs.Empty);
                    break;
                case PolicyType.RuleList:
                    ClearHostList();
                    radBlock.Enabled = false;
                    radUnrestricted.Enabled = false;
                    radTcpUdpUnrestricted.Enabled = false;
                    radTcpUdpOut.Enabled = false;
                    radOnlySpecifiedPorts.Enabled = false;
                    radOnlySpecifiedHosts.Enabled = false;
                    chkRestrictToLocalNetwork.Enabled = false;
                    chkRestrictToLocalNetwork.Checked = false;
                    break;
                case PolicyType.TcpUdpOnly:
                    TcpUdpPolicy pol = (TcpUdpPolicy)TmpExceptionSettings[0].Policy;
                    PopulateAllowedHosts(pol);
                    if (!string.IsNullOrEmpty(pol.AllowedRemoteHosts))
                    {
                        radOnlySpecifiedHosts.Checked = true;
                    }
                    else if (
                        string.Equals(pol.AllowedLocalTcpListenerPorts, "*")
                        && string.Equals(pol.AllowedLocalUdpListenerPorts, "*")
                        && string.Equals(pol.AllowedRemoteTcpConnectPorts, "*")
                        && string.Equals(pol.AllowedRemoteUdpConnectPorts, "*")
                    )
                    {
                        radTcpUdpUnrestricted.Checked = true;
                    }
                    else if (
                        string.Equals(pol.AllowedRemoteTcpConnectPorts, "*")
                        && string.Equals(pol.AllowedRemoteUdpConnectPorts, "*")
                        )
                    {
                        radTcpUdpOut.Checked = true;
                    }
                    else
                    {
                        radOnlySpecifiedPorts.Checked = true;
                    }

                    radRestriction_CheckedChanged(this, EventArgs.Empty);
                    chkRestrictToLocalNetwork.Checked = pol.LocalNetworkOnly;
                    txtOutboundPortTCP.Text = (pol.AllowedRemoteTcpConnectPorts is null) ? string.Empty : pol.AllowedRemoteTcpConnectPorts.Replace(",", ", ");
                    txtOutboundPortUDP.Text = (pol.AllowedRemoteUdpConnectPorts is null) ? string.Empty : pol.AllowedRemoteUdpConnectPorts.Replace(",", ", ");
                    txtListenPortTCP.Text = (pol.AllowedLocalTcpListenerPorts is null) ? string.Empty : pol.AllowedLocalTcpListenerPorts.Replace(",", ", ");
                    txtListenPortUDP.Text = (pol.AllowedLocalUdpListenerPorts is null) ? string.Empty : pol.AllowedLocalUdpListenerPorts.Replace(",", ", ");
                    break;
                case PolicyType.Unrestricted:
                    UnrestrictedPolicy upol = (UnrestrictedPolicy)TmpExceptionSettings[0].Policy;
                    ClearHostList();
                    radUnrestricted.Checked = true;
                    radRestriction_CheckedChanged(this, EventArgs.Empty);
                    chkRestrictToLocalNetwork.Checked = upol.LocalNetworkOnly;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void PopulateAllowedHosts(TcpUdpPolicy pol)
        {
            listAllowedHosts.BeginUpdate();
            listAllowedHosts.Items.Clear();

            if (!string.IsNullOrEmpty(pol.AllowedRemoteHosts))
            {
                foreach (string entry in pol.AllowedRemoteHosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = entry.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        listAllowedHosts.Items.Add(new ListViewItem(trimmed));
                }
            }

            listAllowedHosts.EndUpdate();
            UpdateHostButtons();
        }

        private void ClearHostList()
        {
            listAllowedHosts.BeginUpdate();
            listAllowedHosts.Items.Clear();
            listAllowedHosts.EndUpdate();
            UpdateHostButtons();
        }

        private static string CleanupPortsList(string str)
        {
            string res = str;
            res = res.Replace(" ", string.Empty);
            res = res.Replace(';', ',');

            // Remove empty elements
            while (res.Contains(",,"))
                res = res.Replace(",,", ",");

            // Terminate early if nothing left
            if (string.IsNullOrEmpty(res))
                return string.Empty;

            // Check validity
            string[] elems = res.Split(PORT_LIST_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
            res = string.Empty;
            foreach (var e in elems)
            {
                bool isRange = (-1 != e.IndexOf('-'));
                if (isRange)
                {
                    string[] minmax = e.Split('-');
                    ushort x = ushort.Parse(minmax[0], System.Globalization.CultureInfo.InvariantCulture);
                    ushort y = ushort.Parse(minmax[1], System.Globalization.CultureInfo.InvariantCulture);
                    ushort min = Math.Min(x, y);
                    ushort max = Math.Max(x, y);
                    res = $"{res},{min:D}-{max:D}";
                }
                else
                {
                    if (e.Equals("*"))
                        // If we have a wildcard, all other list elements are redundant
                        return "*";

                    ushort x = ushort.Parse(e, System.Globalization.CultureInfo.InvariantCulture);
                    res = $"{res},{x:D}";
                }
            }

            // Now we have a ',' at the very start. Remove it.
            res = res.Remove(0, 1);

            return res;
        }

        private string? BuildAllowedHostList()
        {
            if (listAllowedHosts.Items.Count == 0)
                return null;

            var hosts = new List<string>(listAllowedHosts.Items.Count);
            foreach (ListViewItem item in listAllowedHosts.Items)
            {
                string value = item.Text.Trim();
                if (!string.IsNullOrEmpty(value))
                    hosts.Add(value);
            }

            if (hosts.Count == 0)
                return null;

            return string.Join(",", hosts);
        }

        private bool ContainsHostEntry(string entry, ListViewItem? exclude = null)
        {
            foreach (ListViewItem item in listAllowedHosts.Items)
            {
                if (item == exclude)
                    continue;

                if (string.Equals(item.Text, entry, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void UpdateHostButtons()
        {
            bool enabled = grpAllowedHosts.Enabled;
            bool hasSelection = listAllowedHosts.SelectedItems.Count > 0;
            bool hasItems = listAllowedHosts.Items.Count > 0;

            btnModifyHost.Enabled = enabled && hasSelection;
            btnRemoveHost.Enabled = enabled && hasSelection;
            btnRemoveAllHosts.Enabled = enabled && hasItems;
        }

        private void btnAddHost_Click(object sender, EventArgs e)
        {
            using var dialog = new HostEntryForm(Resources.Messages.AddHostTitle, null);
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            if (ContainsHostEntry(dialog.EntryValue))
            {
                Utils.ShowMessageBox(
                    Resources.Messages.HostEntryDuplicate,
                    Resources.Messages.TinyWall,
                    Microsoft.Samples.TaskDialogCommonButtons.Ok,
                    Microsoft.Samples.TaskDialogIcon.Warning,
                    this);
                return;
            }

            listAllowedHosts.Items.Add(new ListViewItem(dialog.EntryValue));
            UpdateHostButtons();
        }

        private void btnModifyHost_Click(object sender, EventArgs e)
        {
            if (listAllowedHosts.SelectedItems.Count == 0)
                return;

            ListViewItem item = listAllowedHosts.SelectedItems[0];
            using var dialog = new HostEntryForm(Resources.Messages.EditHostTitle, item.Text);
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            if (ContainsHostEntry(dialog.EntryValue, item))
            {
                Utils.ShowMessageBox(
                    Resources.Messages.HostEntryDuplicate,
                    Resources.Messages.TinyWall,
                    Microsoft.Samples.TaskDialogCommonButtons.Ok,
                    Microsoft.Samples.TaskDialogIcon.Warning,
                    this);
                return;
            }

            item.Text = dialog.EntryValue;
            UpdateHostButtons();
        }

        private void btnRemoveHost_Click(object sender, EventArgs e)
        {
            if (listAllowedHosts.SelectedItems.Count == 0)
                return;

            listAllowedHosts.Items.Remove(listAllowedHosts.SelectedItems[0]);
            UpdateHostButtons();
        }

        private void btnRemoveAllHosts_Click(object sender, EventArgs e)
        {
            listAllowedHosts.Items.Clear();
            UpdateHostButtons();
        }

        private void listAllowedHosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHostButtons();
        }

        private void listAllowedHosts_DoubleClick(object sender, EventArgs e)
        {
            if (listAllowedHosts.SelectedItems.Count > 0)
                btnModifyHost_Click(sender, e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TmpExceptionSettings[0].ChildProcessesInherit = chkInheritToChildren.Checked;

            if (radBlock.Checked)
            {
                TmpExceptionSettings[0].Policy = HardBlockPolicy.Instance;
            }
            else if (radOnlySpecifiedPorts.Checked || radOnlySpecifiedHosts.Checked || radTcpUdpOut.Checked || radTcpUdpUnrestricted.Checked)
            {
                var pol = new TcpUdpPolicy();

                try
                {
                    pol.LocalNetworkOnly = chkRestrictToLocalNetwork.Checked;
                    pol.AllowedRemoteTcpConnectPorts = CleanupPortsList(txtOutboundPortTCP.Text);
                    pol.AllowedRemoteUdpConnectPorts = CleanupPortsList(txtOutboundPortUDP.Text);
                    pol.AllowedLocalTcpListenerPorts = CleanupPortsList(txtListenPortTCP.Text);
                    pol.AllowedLocalUdpListenerPorts = CleanupPortsList(txtListenPortUDP.Text);
                    string? allowedHosts = null;
                    if (radOnlySpecifiedHosts.Checked)
                    {
                        allowedHosts = BuildAllowedHostList();
                        if (string.IsNullOrEmpty(allowedHosts))
                        {
                            Utils.ShowMessageBox(
                                Resources.Messages.HostListRequired,
                                Resources.Messages.TinyWall,
                                Microsoft.Samples.TaskDialogCommonButtons.Ok,
                                Microsoft.Samples.TaskDialogIcon.Warning,
                                this);
                            return;
                        }
                    }

                    pol.AllowedRemoteHosts = allowedHosts;
                    TmpExceptionSettings[0].Policy = pol;
                }
                catch
                {
                    Utils.ShowMessageBox(
                        Resources.Messages.PortListInvalid,
                        Resources.Messages.TinyWall,
                        Microsoft.Samples.TaskDialogCommonButtons.Ok,
                        Microsoft.Samples.TaskDialogIcon.Warning,
                        this);

                    return;
                }
            }
            else if (radUnrestricted.Checked)
            {
                var pol = new UnrestrictedPolicy();
                pol.LocalNetworkOnly = chkRestrictToLocalNetwork.Checked;
                TmpExceptionSettings[0].Policy = pol;
            }

            this.TmpExceptionSettings[0].CreationDate = DateTime.Now;
            
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            var procList = new List<ProcessInfo>();
            using (var pf = new ProcessesForm(false))
            {
                    if (pf.ShowDialog(this) == DialogResult.Cancel)
                        return;

                procList.AddRange(pf.Selection);
            }
            if (procList.Count == 0) return;

            ExceptionSubject subject;
            if (procList[0].Package.HasValue)
                subject = new AppContainerSubject(procList[0].Package!.Value);
            else
                subject = new ExecutableSubject(procList[0].Path!);

            ReinitFormFromSubject(subject);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;

            ReinitFormFromSubject(new ExecutableSubject(PathMapper.Instance.ConvertPathIgnoreErrors(ofd.FileName, PathFormat.Win32)));
        }

        private void ofd_FileOk(object sender, CancelEventArgs e)
        {
            if (sender is not OpenFileDialog dialog)
                return;

            string selectedPath = dialog.FileName;

            if (PathRuleRegex.ContainsRegex(selectedPath))
                return;

            if (SubjectIdentity.IsValidExecutablePath(PathMapper.Instance.ConvertPathIgnoreErrors(selectedPath, PathFormat.Win32)))
                return;

            e.Cancel = true;
            Utils.ShowMessageBox(
                Resources.Messages.SelectedFileDoesNotExist,
                Resources.Messages.TinyWall,
                Microsoft.Samples.TaskDialogCommonButtons.Ok,
                Microsoft.Samples.TaskDialogIcon.Warning,
                this);
        }

        private void btnChooseService_Click(object sender, EventArgs e)
        {
            ServiceSubject? subject = ServicesForm.ChooseService(this);
            if (subject == null) return;

            ReinitFormFromSubject(subject);
        }

        private void btnSelectUwpApp_Click(object sender, EventArgs e)
        {
            var packageList = UwpPackagesForm.ChoosePackage(this, false);
            if (packageList.Count == 0) return;

            ReinitFormFromSubject(new AppContainerSubject(packageList[0]));
        }

        private void ReinitFormFromSubject(ExceptionSubject subject)
        {
            List<FirewallExceptionV3> exceptions = GlobalInstances.AppDatabase.GetExceptionsForApp(subject, true, out _);
            if (exceptions.Count == 0)
                return;

            TmpExceptionSettings = exceptions;

            UpdateUI();

            if (TmpExceptionSettings.Count > 1)
                // Multiple known files, just accept them as is
                this.DialogResult = DialogResult.OK;
        }

        private void txtAppPath_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtSrvName_TextChanged(object sender, EventArgs e)
        {
        }

        private void cmbTimer_SelectedIndexChanged(object sender, EventArgs e)
        {
            TmpExceptionSettings[0].Timer = ((KeyValuePair<string, AppExceptionTimer>)cmbTimer.SelectedItem).Value;
        }

        private void radRestriction_CheckedChanged(object sender, EventArgs e)
        {
            grpAllowedHosts.Enabled = radOnlySpecifiedHosts.Checked;
            if (radBlock.Checked)
            {
                panel3.Enabled = false;
                txtListenPortTCP.Text = string.Empty;
                txtListenPortUDP.Text = string.Empty;
                txtOutboundPortTCP.Text = string.Empty;
                txtOutboundPortUDP.Text = string.Empty;
                chkRestrictToLocalNetwork.Enabled = false;
                chkRestrictToLocalNetwork.Checked = false;
            }
            else if (radOnlySpecifiedPorts.Checked || radOnlySpecifiedHosts.Checked)
            {
                panel3.Enabled = true;
                txtListenPortTCP.Text = string.Empty;
                txtListenPortUDP.Text = string.Empty;
                txtOutboundPortTCP.Text = string.Empty;
                txtOutboundPortUDP.Text = string.Empty;
                txtOutboundPortTCP.Enabled = true;
                txtOutboundPortUDP.Enabled = true;
                label7.Enabled = true;
                label8.Enabled = true;
                chkRestrictToLocalNetwork.Enabled = true;
            }
            else if (radTcpUdpOut.Checked)
            {
                panel3.Enabled = true;
                txtListenPortTCP.Text = string.Empty;
                txtListenPortUDP.Text = string.Empty;
                txtOutboundPortTCP.Text = "*";
                txtOutboundPortUDP.Text = "*";
                txtOutboundPortTCP.Enabled = false;
                txtOutboundPortUDP.Enabled = false;
                label7.Enabled = false;
                label8.Enabled = false;
                chkRestrictToLocalNetwork.Enabled = true;
            }
            else if (radTcpUdpUnrestricted.Checked)
            {
                panel3.Enabled = false;
                txtListenPortTCP.Text = "*";
                txtListenPortUDP.Text = "*";
                txtOutboundPortTCP.Text = "*";
                txtOutboundPortUDP.Text = "*";
                chkRestrictToLocalNetwork.Enabled = true;
            }
            else if (radUnrestricted.Checked)
            {
                panel3.Enabled = false;
                txtListenPortTCP.Text = "*";
                txtListenPortUDP.Text = "*";
                txtOutboundPortTCP.Text = "*";
                txtOutboundPortUDP.Text = "*";
                chkRestrictToLocalNetwork.Enabled = true;
            }
            else
            {
                throw new InvalidOperationException();
            }

            UpdateHostButtons();
        }
    }
}

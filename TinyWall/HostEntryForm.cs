using System;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using Microsoft.Samples;
using pylorak.TinyWall.Resources;

namespace pylorak.TinyWall
{
    internal partial class HostEntryForm : Form
    {
        internal string EntryValue { get; private set; } = string.Empty;

        internal HostEntryForm(string title, string? initialValue)
        {
            InitializeComponent();
            Utils.SetRightToLeft(this);

            this.Icon = Resources.Icons.firewall;
            this.Text = title;

            lblPrompt.Text = Messages.HostEntryPrompt;

            btnOK.Text = Messages.ButtonOK;
            btnOK.Image = Resources.Icons.accept;
            btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            btnCancel.Text = Messages.ButtonCancel;
            btnCancel.Image = Resources.Icons.cancel;
            btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            if (!string.IsNullOrWhiteSpace(initialValue))
            {
                txtHost.Text = initialValue;
                txtHost.SelectAll();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!TryNormalizeHostEntry(txtHost.Text, out string normalized, out string? errorMessage))
            {
                Utils.ShowMessageBox(
                    errorMessage ?? Messages.HostEntryInvalid,
                    Messages.TinyWall,
                    TaskDialogCommonButtons.Ok,
                    TaskDialogIcon.Warning,
                    this);

                txtHost.Focus();
                txtHost.SelectAll();
                return;
            }

            EntryValue = normalized;
            DialogResult = DialogResult.OK;
        }

        private static bool TryNormalizeHostEntry(string? input, out string normalized, out string? errorMessage)
        {
            normalized = string.Empty;
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage = Messages.HostEntryRequired;
                return false;
            }

            string trimmed = input.Trim();
            int slashIndex = trimmed.IndexOf('/');
            if (slashIndex >= 0)
            {
                string ipPart = trimmed.Substring(0, slashIndex).Trim();
                string suffix = trimmed.Substring(slashIndex + 1).Trim();

                if (!IPAddress.TryParse(ipPart, out IPAddress? ip) || ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    errorMessage = Messages.HostEntryInvalid;
                    return false;
                }

                if (suffix.Length == 0)
                {
                    errorMessage = Messages.HostEntryInvalid;
                    return false;
                }

                if (suffix.IndexOf('.') >= 0)
                {
                    try
                    {
                        normalized = NetworkConverter.ConvertToCidr(string.Concat(ipPart, "/", suffix));
                        return true;
                    }
                    catch (ArgumentException)
                    {
                        errorMessage = Messages.HostEntryInvalidSubnetMask;
                        return false;
                    }
                }
                else
                {
                    if (!int.TryParse(suffix, NumberStyles.Integer, CultureInfo.InvariantCulture, out int prefixLength) || prefixLength < 0 || prefixLength > 32)
                    {
                        errorMessage = Messages.HostEntryInvalidPrefix;
                        return false;
                    }

                    normalized = string.Concat(ipPart, "/", prefixLength.ToString(CultureInfo.InvariantCulture));
                    return true;
                }
            }

            if (IPAddress.TryParse(trimmed, out IPAddress? address) && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                normalized = address.ToString();
                return true;
            }

            errorMessage = Messages.HostEntryInvalid;
            return false;
        }
    }
}

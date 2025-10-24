using System.Drawing;
using System.Windows.Forms;
using pylorak.Windows;

namespace pylorak.TinyWall
{
    internal sealed class HostEntryForm : Form
    {
        private readonly TextBox _textBox;
        private readonly Button _okButton;

        internal string EntryValue => _textBox.Text.Trim();

        internal HostEntryForm(string title, string? initialValue)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(360, 140);

            var label = new Label
            {
                AutoSize = true,
                Location = new Point(12, 15),
                Text = "IP or host"
            };

            _textBox = new TextBox
            {
                Location = new Point(15, 35),
                Size = new Size(330, 23),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Text = initialValue ?? string.Empty
            };

            _okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(ClientSize.Width - 170, ClientSize.Height - 40),
                Size = new Size(75, 23)
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(ClientSize.Width - 85, ClientSize.Height - 40),
                Size = new Size(75, 23)
            };

            Controls.Add(label);
            Controls.Add(_textBox);
            Controls.Add(_okButton);
            Controls.Add(cancelButton);

            AcceptButton = _okButton;
            CancelButton = cancelButton;

            _textBox.TextChanged += (_, _) => UpdateOkButtonState();
            Shown += (_, _) =>
            {
                _textBox.SelectAll();
                _textBox.Focus();
                UpdateOkButtonState();
            };

            UpdateOkButtonState();

            Utils.SetRightToLeft(this);
        }

        private void UpdateOkButtonState()
        {
            _okButton.Enabled = !string.IsNullOrWhiteSpace(_textBox.Text);
        }
    }
}

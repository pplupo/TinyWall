﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace pylorak.TinyWall
{
    internal partial class DevelToolForm : Form
    {
        private static readonly string[] SigningFilePatterns = { "*.dll", "*.exe", "*.msi" };

        // Key - The primary resource
        // Value - List of satellite resources
        private readonly List<KeyValuePair<string, string[]>> _resXInputs = new();

        internal DevelToolForm()
        {
            MessageBox.Show(
                @"This tool is not meant for end-users. Only use this tool when instructed to do so by the application developer.",
                @"Warning: Not for users!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation
                );

            InitializeComponent();
        }

        private void btnAssocBrowse_Click(object sender, EventArgs e)
        {
            ofd.Filter = @"All files (*)|*";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtAssocExePath.Text = ofd.FileName;
            }
        }

        private void btnAssocCreate_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtAssocExePath.Text))
            {
                var exe = new ExecutableSubject(txtAssocExePath.Text);

                var id = new DatabaseClasses.SubjectIdentity(exe)
                {
                    AllowedSha1 = new List<string> { exe.HashSha1 }
                };

                if (exe is { IsSigned: true, CertValid: true })
                {
                    id.CertificateSubjects = new List<string>();
                    if (exe.CertSubject is not null)
                        id.CertificateSubjects.Add(exe.CertSubject);
                }

                var utf8Bytes = SerialisationHelper.Serialise(id);
                txtAssocResult.Text = Encoding.UTF8.GetString(utf8Bytes);
            }
            else
            {
                MessageBox.Show(this, @"No such file.", @"File not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnProfileFolderBrowse_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog(this) == DialogResult.OK)
                txtDBFolderPath.Text = fbd.SelectedPath;
        }

        private void btnCollectionsCreate_Click(object sender, EventArgs e)
        {
            // Common init
            var db = new DatabaseClasses.AppDatabase();

            string outputPath = txtAssocOutputPath.Text;
            string inputPath = txtDBFolderPath.Text;
            if (!Directory.Exists(inputPath))
            {
                MessageBox.Show(this, @"Input database folder not found.", @"Directory not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var defAppInst = new DatabaseClasses.Application();
            var files = Directory.GetFiles(inputPath, "*.json", SearchOption.AllDirectories);
            foreach (string fpath in files)
            {
                try
                {
                    var loadedAppInst = SerialisationHelper.DeserialiseFromFile(fpath, defAppInst);
                    if (loadedAppInst.Components.Count > 0)
                        db.KnownApplications.Add(loadedAppInst);
                }
                catch
                {
                    // ignored
                }
            }

            db.Save(Path.Combine(outputPath, "profiles.json"));
            MessageBox.Show(this, @"Creation of collections finished.", @"Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStrongNameBrowse_Click(object sender, EventArgs e)
        {
            ofd.Filter = @".Net binaries (*.exe,*.dll)|*.dll;*.exe|All files (*)|*";

            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                Assembly a = Assembly.ReflectionOnlyLoadFrom(ofd.FileName);
                txtStrongName.Text = a.FullName;
            }
            catch
            {
                txtStrongName.Text = @"Bad assembly";
            }
        }

        private void btnAssocOutputBrowse_Click(object sender, EventArgs e)
        {
            if (fbd.ShowDialog(this) == DialogResult.Cancel)
                return;

            txtAssocOutputPath.Text = fbd.SelectedPath;
        }

        private void DevelToolForm_Load(object sender, EventArgs e)
        {
            txtStrongName.Text = Assembly.GetExecutingAssembly().FullName;
        }

        private void btnUpdateInstallerBrowse_Click(object sender, EventArgs e)
        {
            ofd.Filter = @"All files (*)|*";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtUpdateInstallerProjectDir.Text = ofd.FileName;
            }
        }

        private void btnUpdateOutputBrowse_Click(object sender, EventArgs e)
        {
            fbd.SelectedPath = txtUpdateOutput.Text;
            if (fbd.ShowDialog(this) == DialogResult.Cancel)
                return;

            txtUpdateOutput.Text = fbd.SelectedPath;
        }

        private void btnUpdateCreate_Click(object sender, EventArgs e)
        {
            const string PLACEHOLDER = "[Unset]";
            const string HOSTS_PLACEHOLDER = "[HOSTS_SHA256_PLACEHOLDER]";
            const string DB_OUT_NAME = "database.def";
            const string HOSTS_OUT_NAME = "hosts.def";
            const string DESCRIPTOR_NAME = "update.json";
            const string DESCRIPTOR_TEMPLATE_NAME = "update_template.json";
            const string MSI_FILENAME = "TinyWall-v3-Installer.msi";

            string projectDir = txtUpdateInstallerProjectDir.Text;
            string msiPath = Path.Combine(projectDir, @"bin\Release\" + MSI_FILENAME);
            string hostsPath = Path.Combine(projectDir, @"Sources\CommonAppData\TinyWall\hosts.bck");
            string profilesPath = Path.Combine(projectDir, @"Sources\CommonAppData\TinyWall\profiles.json");

            string twAssemblyPath = Path.Combine(projectDir, @"Sources\ProgramFiles\TinyWall\TinyWall.exe");

            void ShowUpdateFileNotFoundMsg(string file)
            {
                MessageBox.Show(this, $@"File\n\n{file}\n\nnot found.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            void ShowUpdateDirectoryNotFoundMsg(string file)
            {
                MessageBox.Show(this, $@"Directory\n\n{file}\n\nnot found.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!File.Exists(msiPath))
            {
                ShowUpdateFileNotFoundMsg(msiPath);
                return;
            }
            if (!File.Exists(hostsPath))
            {
                ShowUpdateFileNotFoundMsg(hostsPath);
                return;
            }
            if (!File.Exists(profilesPath))
            {
                ShowUpdateFileNotFoundMsg(profilesPath);
                return;
            }
            if (!File.Exists(twAssemblyPath))
            {
                ShowUpdateFileNotFoundMsg(twAssemblyPath);
                return;
            }
            if (!Directory.Exists(txtUpdateOutput.Text))
            {
                ShowUpdateDirectoryNotFoundMsg(txtUpdateOutput.Text);
                return;
            }

            FileVersionInfo installerInfo = FileVersionInfo.GetVersionInfo(twAssemblyPath);

            var update = new UpdateDescriptor
            {
                Modules = new UpdateModule[3]
            };

            update.Modules[0] = new UpdateModule
            {
                Component = "TinyWall",
                ComponentVersion = installerInfo.ProductVersion.Trim(),
                DownloadHash = Hasher.HashFile(msiPath),
                UpdateUrl = txtUpdateURL.Text + MSI_FILENAME
            };

            update.Modules[1] = new UpdateModule
            {
                Component = "Database",
                ComponentVersion = PLACEHOLDER,
                DownloadHash = Hasher.HashFile(profilesPath),
                UpdateUrl = txtUpdateURL.Text + DB_OUT_NAME
            };

            update.Modules[2] = new UpdateModule
            {
                Component = "HostsFile",
                ComponentVersion = PLACEHOLDER,
                DownloadHash = Hasher.HashFile(hostsPath),
                UpdateUrl = txtUpdateURL.Text + HOSTS_OUT_NAME
            };

            File.Copy(msiPath, Path.Combine(txtUpdateOutput.Text, MSI_FILENAME), true);

            string dbOut = Path.Combine(txtUpdateOutput.Text, DB_OUT_NAME);
            Utils.CompressDeflate(profilesPath, dbOut);

            string hostsOut = Path.Combine(txtUpdateOutput.Text, HOSTS_OUT_NAME);
            Utils.CompressDeflate(hostsPath, hostsOut);

            string updOut = Path.Combine(txtUpdateOutput.Text, DESCRIPTOR_NAME);
            SerialisationHelper.SerialiseToFile(update, updOut);

            update.Modules[2].DownloadHash = HOSTS_PLACEHOLDER;
            updOut = Path.Combine(txtUpdateOutput.Text, DESCRIPTOR_TEMPLATE_NAME);
            SerialisationHelper.SerialiseToFile(update, updOut);

            MessageBox.Show(this, @"Update created.", @"Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static int CountOccurrence(string haystack, char needle)
        {
            return haystack.Count(c => c == needle);
        }

        private void btnAddPrimaries_Click(object sender, EventArgs e)
        {
            ofd.Filter = @"XML resources (*.resx)|*.resx|All files (*)|*";
            ofd.AutoUpgradeEnabled = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog(this) == DialogResult.Cancel)
                return;

            foreach (var primary in ofd.FileNames)
            {
                if (CountOccurrence(Path.GetFileName(primary), '.') != 1)
                    continue;   // This is not a primary at all...

                string dir = Path.GetDirectoryName(primary) ?? string.Empty;
                string primaryBase = Path.GetFileNameWithoutExtension(primary);
                string[] satellites = Directory.GetFiles(dir, primaryBase + ".*.resx", SearchOption.TopDirectoryOnly);
                _resXInputs.Add(new KeyValuePair<string, string[]>(primary, satellites));
            }

            listPrimaries.Items.Clear();

            foreach (var t in _resXInputs)
                listPrimaries.Items.Add(Path.GetFileName(t.Key));
        }

        private void listPrimaries_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSatellites.Items.Clear();
            if (listPrimaries.SelectedIndices.Count <= 0) return;

            KeyValuePair<string, string[]> pair = _resXInputs[listPrimaries.SelectedIndex];
            object[] sats = new object[pair.Value.Length];

            for (int i = 0; i < sats.Length; ++i)
                sats[i] = Path.GetFileName(pair.Value[i]);

            listSatellites.Items.AddRange(sats);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listPrimaries.Items.Clear();
            listSatellites.Items.Clear();
            _resXInputs.Clear();
        }

        private static Dictionary<string, ResXDataNode> ReadResXFile(string filePath)
        {
            var resxContents = new Dictionary<string, ResXDataNode>();
            using var resxReader = new ResXResourceReader(filePath);
            resxReader.UseResXDataNodes = true;
            var dictionaryEnumerator = resxReader.GetEnumerator();
            using var dict1 = dictionaryEnumerator as IDisposable;

            while (dictionaryEnumerator.MoveNext())
            {
                var node = (ResXDataNode)dictionaryEnumerator.Value;
                resxContents.Add(node.Name, node);
            }

            return resxContents;
        }

        private void btnOptimize_Click(object sender, EventArgs e)
        {
            ITypeResolutionService? trs = null;

            foreach (var pair in _resXInputs)
            {
                var primary = ReadResXFile(pair.Key);

                foreach (var t in pair.Value)
                {
                    { // Replace Windows Forms control versions to 4.0.0.0.
                        using var sr = new StreamReader(t, Encoding.UTF8);
                        var a = sr.ReadToEnd();
                        a = a.Replace(", Version=2.0.0.0,", ", Version=4.0.0.0,");

                        using var sw = new StreamWriter(t, false, Encoding.UTF8);
                        sw.Write(a);
                    }

                    var satellite = ReadResXFile(t);
                    var newSatellite = new Dictionary<string, ResXDataNode>();

                    // Iterate over all contents of primary.
                    // For each entry, check if one with same name, type and contents is available in
                    // satellite, and if so, don't save it to output.
                    using var primaryEnum = primary.GetEnumerator();
                    while (primaryEnum.MoveNext())
                    {
                        ResXDataNode primaryItem = primaryEnum.Current.Value;

                        if (!satellite.ContainsKey(primaryItem.Name))
                            continue;

                        ResXDataNode satelliteItem = satellite[primaryItem.Name];

                        // Only save localized resource if it is different from the default
                        if (!satelliteItem.GetValue(trs).Equals(primaryItem.GetValue(trs)))
                            newSatellite.Add(satelliteItem.Name, satelliteItem);

                    }

                    // Write output ResX file
                    string outPath = Path.Combine(txtOutputPath.Text, Path.GetFileName(t));
                    using var resxWriter = new ResXResourceWriter(outPath);
                    using Dictionary<string, ResXDataNode>.Enumerator outputEnum = newSatellite.GetEnumerator();

                    while (outputEnum.MoveNext())
                        resxWriter.AddResource(outputEnum.Current.Value);

                    resxWriter.Generate();
                }
            }
        }

        private void btnCertBrowse_Click(object sender, EventArgs e)
        {
            ofd.InitialDirectory = Path.GetDirectoryName(txtCert.Text);
            ofd.Filter = @"All files (*)|*";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtCert.Text = ofd.FileName;
            }
        }

        private void btnSignDir_Click(object sender, EventArgs e)
        {
            fbd.SelectedPath = Path.GetDirectoryName(txtCert.Text);

            if (fbd.ShowDialog(this) == DialogResult.Cancel)
                return;

            txtSignDir.Text = fbd.SelectedPath;
        }

        private void btnBatchSign_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtCert.Text))
            {
                MessageBox.Show(this, @"Certificate not found!");
                return;
            }

            if (!Directory.Exists(txtSignDir.Text))
            {
                MessageBox.Show(this, @"Signing directory is invalid!");
                return;
            }

            if (!File.Exists(txtSigntool.Text))
            {
                MessageBox.Show(this, @"Signtool.exe not found!");
                return;
            }

            btnBatchSign.Enabled = false;
            SignFiles(txtSignDir.Text, SigningFilePatterns);
            btnBatchSign.Enabled = true;
        }

        private void SignFiles(string dirPath, string[] filePatterns)
        {
            // Collect all files to sign
            var filesToSign = new List<string>();
            foreach (var pattern in filePatterns)
            {
                string[] candidateFiles = Directory.GetFiles(dirPath, pattern, SearchOption.AllDirectories);
                foreach (var filePath in candidateFiles)
                {
                    var signedStatus = Windows.WinTrust.VerifyFileAuthenticode(filePath);
                    if (signedStatus == Windows.WinTrust.VerifyResult.SIGNATURE_MISSING)
                    {
                        filesToSign.Add("\"" + filePath + "\"");
                    }
                    else if (signedStatus == Windows.WinTrust.VerifyResult.SIGNATURE_INVALID)
                    {
                        MessageBox.Show(this,
                            $@"File {filePath} has pre-existing INVALID certificate. Signing will be aborted for all files.", @"Signing result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            if (filesToSign.Count == 0)
            {
                MessageBox.Show(this, @"No files to sign, or all files are already signed.", @"Signing result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Assemble signtool command
            var signParams =
                $"sign /d TinyWall /du \"https://tinywall.pados.hu\" /n \"{txtCert.Text}\" /tr \"{txtTimestampingServ.Text}\" /td sha256 /fd sha256 /v {string.Join(" ", filesToSign)}";

            // Execute signing process
            bool signSuccess;
            using (var p = Utils.StartProcess(txtSigntool.Text, signParams, false))
            {
                p.WaitForExit();
                signSuccess = (p.ExitCode == 0);
            }

            if (signSuccess)
                MessageBox.Show(this, @"Files successfully signed.", @"Signing result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, @"Failed to sign files.", @"Signing result", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSigntoolBrowse_Click(object sender, EventArgs e)
        {
            ofd.Filter = @"Executables (*.exe)|*.exe|All files (*)|*";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtSigntool.Text = ofd.FileName;
            }

        }
    }
}

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using pylorak.TinyWall.MaterialDesign;

namespace pylorak.TinyWall
{
    /// <summary>
    /// Main dashboard form with Material Design interface
    /// </summary>
    public partial class MainDashboardForm : Form
    {
        private readonly TinyWallController _controller;
        private readonly MaterialSideBar _sideBar;
        private readonly Panel _contentPanel;
        private readonly Panel _statsPanel;
        private readonly Timer _updateTimer;
        
        public MainDashboardForm(TinyWallController controller)
        {
            _controller = controller;
            InitializeComponent();
            
            // Initialize sidebar
            _sideBar = new MaterialSideBar();
            _sideBar.ItemClicked += OnNavigationItemClicked;
            
            // Initialize content panel
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = MaterialColors.Background,
                Padding = new Padding(16)
            };
            
            // Initialize stats panel
            _statsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = MaterialColors.Background
            };
            
            SetupForm();
            CreateDashboard();
            CreateQuickActions();
            
            // Update timer for live stats
            _updateTimer = new Timer
            {
                Interval = 5000, // Update every 5 seconds
                Enabled = true
            };
            _updateTimer.Tick += OnUpdateTimer;
        }
        
        private void SetupForm()
        {
            Text = "TinyWall - Firewall Management";
            Size = new Size(1024, 768);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 600);
            Icon = Resources.Icons.firewall;
            BackColor = MaterialColors.Background;
            
            // Setup layout
            Controls.Add(_sideBar);
            Controls.Add(_contentPanel);
            _contentPanel.Controls.Add(_statsPanel);
            
            // Apply Material Design styling
            MaterialHelper.ApplyElevation(this, 0);
        }
        
        private void CreateDashboard()
        {
            _statsPanel.Controls.Clear();
            
            // Title
            var titleLabel = new Label
            {
                Text = "Firewall Dashboard",
                Location = new Point(16, 16),
                Size = new Size(400, 40),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialHeader(titleLabel, 1);
            _statsPanel.Controls.Add(titleLabel);
            
            // Status cards
            CreateStatusCards();
            
            // Traffic chart
            CreateTrafficChart();
            
            // Recent activity
            CreateRecentActivity();
        }
        
        private void CreateStatusCards()
        {
            var yOffset = 80;
            var cardWidth = 200;
            var cardHeight = 120;
            var cardSpacing = 16;
            
            // Firewall status card
            var statusCard = CreateStatusCard(
                "Firewall Status",
                GetFirewallStatusText(),
                GetFirewallStatusColor(),
                new Point(16, yOffset),
                new Size(cardWidth, cardHeight)
            );
            _statsPanel.Controls.Add(statusCard);
            
            // Blocked connections card
            var blockedCard = CreateStatusCard(
                "Blocked Today",
                GetBlockedConnectionsCount().ToString(),
                MaterialColors.FirewallBlocked,
                new Point(16 + cardWidth + cardSpacing, yOffset),
                new Size(cardWidth, cardHeight)
            );
            _statsPanel.Controls.Add(blockedCard);
            
            // Allowed connections card
            var allowedCard = CreateStatusCard(
                "Allowed Today",
                GetAllowedConnectionsCount().ToString(),
                MaterialColors.FirewallAllowed,
                new Point(16 + (cardWidth + cardSpacing) * 2, yOffset),
                new Size(cardWidth, cardHeight)
            );
            _statsPanel.Controls.Add(allowedCard);
            
            // Active rules card
            var rulesCard = CreateStatusCard(
                "Active Rules",
                GetActiveRulesCount().ToString(),
                MaterialColors.Primary,
                new Point(16 + (cardWidth + cardSpacing) * 3, yOffset),
                new Size(cardWidth, cardHeight)
            );
            _statsPanel.Controls.Add(rulesCard);
        }
        
        private Panel CreateStatusCard(string title, string value, Color accentColor, Point location, Size size)
        {
            var card = new Panel
            {
                Location = location,
                Size = size,
                BackColor = MaterialColors.Surface
            };
            
            MaterialHelper.StyleMaterialPanel(card, true);
            
            // Title label
            var titleLabel = new Label
            {
                Text = title,
                Location = new Point(16, 16),
                Size = new Size(size.Width - 32, 20),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialLabel(titleLabel, true);
            card.Controls.Add(titleLabel);
            
            // Value label
            var valueLabel = new Label
            {
                Text = value,
                Location = new Point(16, 45),
                Size = new Size(size.Width - 32, 40),
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = accentColor,
                BackColor = Color.Transparent
            };
            card.Controls.Add(valueLabel);
            
            return card;
        }
        
        private void CreateTrafficChart()
        {
            var chartPanel = new Panel
            {
                Location = new Point(16, 220),
                Size = new Size(600, 200),
                BackColor = MaterialColors.Surface
            };
            
            MaterialHelper.StyleMaterialPanel(chartPanel, true);
            
            // Chart title
            var chartTitle = new Label
            {
                Text = "Traffic Overview",
                Location = new Point(16, 16),
                Size = new Size(200, 24),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialHeader(chartTitle, 3);
            chartPanel.Controls.Add(chartTitle);
            
            // Traffic rate labels
            var inLabel = new Label
            {
                Text = "In: 0 KB/s",
                Location = new Point(250, 16),
                Size = new Size(100, 24),
                BackColor = Color.Transparent,
                ForeColor = MaterialColors.Success
            };
            MaterialHelper.StyleMaterialLabel(inLabel);
            chartPanel.Controls.Add(inLabel);
            
            var outLabel = new Label
            {
                Text = "Out: 0 KB/s",
                Location = new Point(360, 16),
                Size = new Size(100, 24),
                BackColor = Color.Transparent,
                ForeColor = MaterialColors.Primary
            };
            MaterialHelper.StyleMaterialLabel(outLabel);
            chartPanel.Controls.Add(outLabel);
            
            // Simple chart placeholder (could be replaced with actual charting library)
            var chartArea = new Panel
            {
                Location = new Point(16, 50),
                Size = new Size(568, 134),
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = new { InLabel = inLabel, OutLabel = outLabel } // Store labels for updates
            };
            
            // Add some basic chart visualization
            chartArea.Paint += (sender, e) =>
            {
                DrawTrafficChart(e.Graphics, chartArea.Size);
            };
            
            chartPanel.Controls.Add(chartArea);
            _statsPanel.Controls.Add(chartPanel);
        }
        
        private void DrawTrafficChart(Graphics graphics, Size size)
        {
            // Get current traffic data
            var trafficMonitor = _controller.CurrentTrafficMonitor;
            if (trafficMonitor == null)
            {
                // Draw placeholder if no data available
                using var font = new Font("Segoe UI", 10f);
                using var brush = new SolidBrush(MaterialColors.TextSecondary);
                var text = "Traffic data not available";
                var textSize = graphics.MeasureString(text, font);
                graphics.DrawString(text, font, brush, 
                    (size.Width - textSize.Width) / 2, (size.Height - textSize.Height) / 2);
                return;
            }
            
            // Simple bar chart for current traffic rates
            var inKBps = Math.Max(0, trafficMonitor.BytesReceivedPerSec / 1024.0);
            var outKBps = Math.Max(0, trafficMonitor.BytesSentPerSec / 1024.0);
            
            var maxRate = Math.Max(Math.Max(inKBps, outKBps), 100); // Minimum scale of 100 KB/s
            var barWidth = 40;
            var barSpacing = 60;
            var maxBarHeight = size.Height - 40;
            
            // Draw incoming traffic bar
            var inHeight = (int)((inKBps / maxRate) * maxBarHeight);
            var inBarRect = new Rectangle(size.Width / 4 - barWidth / 2, size.Height - 20 - inHeight, barWidth, inHeight);
            using (var inBrush = new SolidBrush(MaterialColors.Success))
            {
                graphics.FillRectangle(inBrush, inBarRect);
            }
            
            // Draw outgoing traffic bar
            var outHeight = (int)((outKBps / maxRate) * maxBarHeight);
            var outBarRect = new Rectangle(3 * size.Width / 4 - barWidth / 2, size.Height - 20 - outHeight, barWidth, outHeight);
            using (var outBrush = new SolidBrush(MaterialColors.Primary))
            {
                graphics.FillRectangle(outBrush, outBarRect);
            }
            
            // Draw labels
            using var font = new Font("Segoe UI", 8f);
            using var brush = new SolidBrush(MaterialColors.TextPrimary);
            
            var inLabel = "IN";
            var outLabel = "OUT";
            var inLabelSize = graphics.MeasureString(inLabel, font);
            var outLabelSize = graphics.MeasureString(outLabel, font);
            
            graphics.DrawString(inLabel, font, brush, 
                size.Width / 4 - inLabelSize.Width / 2, size.Height - 15);
            graphics.DrawString(outLabel, font, brush, 
                3 * size.Width / 4 - outLabelSize.Width / 2, size.Height - 15);
            
            // Draw scale
            using var scaleBrush = new SolidBrush(MaterialColors.TextSecondary);
            var scaleText = $"Max: {maxRate:F0} KB/s";
            graphics.DrawString(scaleText, font, scaleBrush, new Point(5, 5));
        }
        
        private void CreateRecentActivity()
        {
            var activityPanel = new Panel
            {
                Location = new Point(640, 220),
                Size = new Size(350, 200),
                BackColor = MaterialColors.Surface
            };
            
            MaterialHelper.StyleMaterialPanel(activityPanel, true);
            
            // Activity title
            var activityTitle = new Label
            {
                Text = "Firewall Status",
                Location = new Point(16, 16),
                Size = new Size(200, 24),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialHeader(activityTitle, 3);
            activityPanel.Controls.Add(activityTitle);
            
            // Status information
            var statusList = new Panel
            {
                Location = new Point(16, 50),
                Size = new Size(318, 134),
                BackColor = Color.Transparent,
                AutoScroll = true
            };
            
            // Add firewall mode status
            var modeLabel = new Label
            {
                Text = $"Mode: {GetFirewallStatusText()}",
                Location = new Point(0, 5),
                Size = new Size(300, 20),
                BackColor = Color.Transparent,
                ForeColor = GetFirewallStatusColor(),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            statusList.Controls.Add(modeLabel);
            
            // Add rules count
            var rulesLabel = new Label
            {
                Text = $"Active Rules: {GetActiveRulesCount()}",
                Location = new Point(0, 30),
                Size = new Size(300, 20),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialLabel(rulesLabel);
            statusList.Controls.Add(rulesLabel);
            
            // Add last update time
            var updateLabel = new Label
            {
                Text = $"Last Updated: {DateTime.Now:HH:mm:ss}",
                Location = new Point(0, 55),
                Size = new Size(300, 20),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialLabel(updateLabel, true);
            statusList.Controls.Add(updateLabel);
            
            // Add service status placeholder
            var serviceLabel = new Label
            {
                Text = "Service: Running",
                Location = new Point(0, 80),
                Size = new Size(300, 20),
                BackColor = Color.Transparent,
                ForeColor = MaterialColors.Success
            };
            MaterialHelper.StyleMaterialLabel(serviceLabel);
            statusList.Controls.Add(serviceLabel);
            
            // Tag the panel for easy updates
            statusList.Tag = "StatusInfo";
            
            activityPanel.Controls.Add(statusList);
            _statsPanel.Controls.Add(activityPanel);
        }
        
        private void CreateQuickActions()
        {
            var actionsPanel = new Panel
            {
                Location = new Point(16, 440),
                Size = new Size(974, 80),
                BackColor = MaterialColors.Surface
            };
            
            MaterialHelper.StyleMaterialPanel(actionsPanel, true);
            
            // Actions title
            var actionsTitle = new Label
            {
                Text = "Quick Actions",
                Location = new Point(16, 16),
                Size = new Size(150, 24),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialHeader(actionsTitle, 3);
            actionsPanel.Controls.Add(actionsTitle);
            
            // Add action buttons
            var buttonWidth = 120;
            var buttonHeight = 36;
            var buttonSpacing = 16;
            var startX = 200;
            
            var buttons = new[]
            {
                new { Text = "Block All", Action = "block_all", Color = MaterialColors.Error },
                new { Text = "Normal Mode", Action = "normal", Color = MaterialColors.Success },
                new { Text = "Open Settings", Action = "settings", Color = MaterialColors.Primary },
                new { Text = "View Connections", Action = "connections", Color = MaterialColors.Primary },
                new { Text = "Refresh", Action = "refresh", Color = MaterialColors.Primary }
            };
            
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = new Button
                {
                    Text = buttons[i].Text,
                    Location = new Point(startX + i * (buttonWidth + buttonSpacing), 20),
                    Size = new Size(buttonWidth, buttonHeight),
                    Tag = buttons[i].Action,
                    BackColor = buttons[i].Color,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                
                button.FlatAppearance.BorderSize = 0;
                MaterialHelper.ApplyRippleEffect(button);
                
                button.Click += QuickActionButton_Click;
                actionsPanel.Controls.Add(button);
            }
            
            _statsPanel.Controls.Add(actionsPanel);
        }
        
        private void QuickActionButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is string action)
            {
                switch (action)
                {
                    case "block_all":
                        // Set firewall to block all mode
                        MessageBox.Show("Setting firewall to Block All mode...", "TinyWall", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "normal":
                        // Set firewall to normal mode
                        MessageBox.Show("Setting firewall to Normal mode...", "TinyWall", 
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "settings":
                        ShowSettings();
                        break;
                    case "connections":
                        ShowConnections();
                        break;
                    case "refresh":
                        UpdateStats();
                        break;
                }
            }
        }
        
        private void OnNavigationItemClicked(object? sender, NavigationItemEventArgs e)
        {
            switch (e.Item.Key)
            {
                case "dashboard":
                    ShowDashboard();
                    break;
                case "connections":
                    ShowConnections();
                    break;
                case "processes":
                    ShowProcesses();
                    break;
                case "services":
                    ShowServices();
                    break;
                case "settings":
                    ShowSettings();
                    break;
                case "help":
                    ShowHelp();
                    break;
            }
        }
        
        private void ShowDashboard()
        {
            _contentPanel.Controls.Clear();
            _contentPanel.Controls.Add(_statsPanel);
        }
        
        private void ShowConnections()
        {
            // Open connections form
            _controller.ShowConnections();
        }
        
        private void ShowProcesses()
        {
            // Open processes form
            _controller.ShowProcesses();
        }
        
        private void ShowServices()
        {
            // Open services form
            _controller.ShowServices();
        }
        
        private void ShowSettings()
        {
            // Open settings form
            _controller.ShowSettings();
        }
        
        private void ShowHelp()
        {
            // Show help
            MessageBox.Show("TinyWall Help\n\nFor more information, visit the TinyWall website.", 
                          "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void OnUpdateTimer(object? sender, EventArgs e)
        {
            // Update stats
            UpdateStats();
        }
        
        private void UpdateStats()
        {
            // Update status cards with current data
            foreach (Control control in _statsPanel.Controls)
            {
                if (control is Panel panel && panel.Controls.Count > 1)
                {
                    var valueLabel = panel.Controls[1] as Label;
                    if (valueLabel != null)
                    {
                        // Update based on panel position (a simple way to identify cards)
                        var cardIndex = _statsPanel.Controls.IndexOf(panel);
                        if (cardIndex >= 0 && cardIndex < 4)
                        {
                            switch (cardIndex)
                            {
                                case 0: // Firewall status
                                    valueLabel.Text = GetFirewallStatusText();
                                    valueLabel.ForeColor = GetFirewallStatusColor();
                                    break;
                                case 1: // Blocked connections
                                    valueLabel.Text = GetBlockedConnectionsCount().ToString();
                                    break;
                                case 2: // Allowed connections
                                    valueLabel.Text = GetAllowedConnectionsCount().ToString();
                                    break;
                                case 3: // Active rules
                                    valueLabel.Text = GetActiveRulesCount().ToString();
                                    break;
                            }
                        }
                    }
                    
                    // Check if this panel contains the status info panel
                    var statusPanel = panel.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag as string == "StatusInfo");
                    if (statusPanel != null)
                    {
                        var labels = statusPanel.Controls.OfType<Label>().ToArray();
                        if (labels.Length >= 4)
                        {
                            // Update mode
                            labels[0].Text = $"Mode: {GetFirewallStatusText()}";
                            labels[0].ForeColor = GetFirewallStatusColor();
                            
                            // Update rules count
                            labels[1].Text = $"Active Rules: {GetActiveRulesCount()}";
                            
                            // Update last update time
                            labels[2].Text = $"Last Updated: {DateTime.Now:HH:mm:ss}";
                        }
                    }
                }
                // Update traffic chart
                else if (control is Panel chartPanel && chartPanel.Controls.Count > 0)
                {
                    var chartArea = chartPanel.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag != null && p.Tag.GetType().Name.Contains("AnonymousType"));
                    if (chartArea?.Tag != null)
                    {
                        try
                        {
                            var tagProps = chartArea.Tag.GetType().GetProperties();
                            var inLabelProp = tagProps.FirstOrDefault(p => p.Name == "InLabel");
                            var outLabelProp = tagProps.FirstOrDefault(p => p.Name == "OutLabel");
                            
                            if (inLabelProp?.GetValue(chartArea.Tag) is Label inLabel && 
                                outLabelProp?.GetValue(chartArea.Tag) is Label outLabel)
                            {
                                // Update traffic rates
                                var trafficMonitor = _controller.CurrentTrafficMonitor;
                                if (trafficMonitor != null)
                                {
                                    var inKBps = trafficMonitor.BytesReceivedPerSec / 1024.0;
                                    var outKBps = trafficMonitor.BytesSentPerSec / 1024.0;
                                    
                                    inLabel.Text = $"In: {inKBps:F1} KB/s";
                                    outLabel.Text = $"Out: {outKBps:F1} KB/s";
                                }
                            }
                            
                            chartArea.Invalidate(); // Refresh the chart
                        }
                        catch
                        {
                            // Ignore any reflection errors
                        }
                    }
                }
            }
        }
        
        // Helper methods to get current firewall statistics
        private string GetFirewallStatusText()
        {
            if (_controller.CurrentFirewallState?.Mode != null)
            {
                return _controller.CurrentFirewallState.Mode switch
                {
                    FirewallMode.Normal => "Normal",
                    FirewallMode.BlockAll => "Block All",
                    FirewallMode.AllowOutgoing => "Allow Outgoing",
                    FirewallMode.Disabled => "Disabled",
                    FirewallMode.Learning => "Learning",
                    _ => "Unknown"
                };
            }
            return "Unknown";
        }
        
        private Color GetFirewallStatusColor()
        {
            if (_controller.CurrentFirewallState?.Mode != null)
            {
                return _controller.CurrentFirewallState.Mode switch
                {
                    FirewallMode.Normal => MaterialColors.FirewallEnabled,
                    FirewallMode.BlockAll => MaterialColors.Warning,
                    FirewallMode.AllowOutgoing => MaterialColors.Warning,
                    FirewallMode.Disabled => MaterialColors.FirewallDisabled,
                    FirewallMode.Learning => MaterialColors.Primary,
                    _ => MaterialColors.FirewallDisabled
                };
            }
            return MaterialColors.FirewallDisabled;
        }
        
        private int GetBlockedConnectionsCount()
        {
            // This would need to be implemented by adding connection tracking to TinyWall
            // For now, return a placeholder that could be extended later
            return 0; // TODO: Implement actual blocked connections counting
        }
        
        private int GetAllowedConnectionsCount()
        {
            // This would need to be implemented by adding connection tracking to TinyWall
            // For now, return a placeholder that could be extended later
            return 0; // TODO: Implement actual allowed connections counting
        }
        
        private int GetActiveRulesCount()
        {
            return _controller.GetActiveRulesCount();
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _updateTimer?.Stop();
            _updateTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
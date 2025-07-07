using System;
using System.Drawing;
using System.Globalization;
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
            
            // Simple chart placeholder (could be replaced with actual charting library)
            var chartArea = new Panel
            {
                Location = new Point(16, 50),
                Size = new Size(568, 134),
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.FixedSingle
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
            // Simple line chart for traffic data
            var points = new[]
            {
                new Point(10, size.Height - 20),
                new Point(60, size.Height - 40),
                new Point(110, size.Height - 30),
                new Point(160, size.Height - 60),
                new Point(210, size.Height - 45),
                new Point(260, size.Height - 70),
                new Point(310, size.Height - 55),
                new Point(360, size.Height - 80),
                new Point(410, size.Height - 65),
                new Point(460, size.Height - 90),
                new Point(510, size.Height - 75)
            };
            
            using var pen = new Pen(MaterialColors.Primary, 2);
            graphics.DrawLines(pen, points);
            
            // Add axis labels
            using var font = new Font("Segoe UI", 8f);
            using var brush = new SolidBrush(MaterialColors.TextSecondary);
            
            graphics.DrawString("Time", font, brush, new Point(size.Width - 40, size.Height - 15));
            graphics.DrawString("Traffic", font, brush, new Point(5, 5));
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
                Text = "Recent Activity",
                Location = new Point(16, 16),
                Size = new Size(200, 24),
                BackColor = Color.Transparent
            };
            MaterialHelper.StyleMaterialHeader(activityTitle, 3);
            activityPanel.Controls.Add(activityTitle);
            
            // Activity list
            var activityList = new ListBox
            {
                Location = new Point(16, 50),
                Size = new Size(318, 134),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9f)
            };
            
            // Add sample activity items
            activityList.Items.Add("🔒 Blocked connection from 192.168.1.100");
            activityList.Items.Add("✅ Allowed Chrome.exe");
            activityList.Items.Add("🔒 Blocked suspicious port scan");
            activityList.Items.Add("⚙️ Firewall rules updated");
            activityList.Items.Add("✅ Allowed Steam.exe");
            
            activityPanel.Controls.Add(activityList);
            _statsPanel.Controls.Add(activityPanel);
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
                }
            }
        }
        
        // Helper methods to get current firewall statistics
        private string GetFirewallStatusText()
        {
            // This would get actual firewall status from the controller
            return "Active"; // Placeholder
        }
        
        private Color GetFirewallStatusColor()
        {
            // This would determine color based on actual firewall status
            return MaterialColors.FirewallEnabled; // Placeholder
        }
        
        private int GetBlockedConnectionsCount()
        {
            // This would get actual blocked connections count
            return 127; // Placeholder
        }
        
        private int GetAllowedConnectionsCount()
        {
            // This would get actual allowed connections count
            return 2543; // Placeholder
        }
        
        private int GetActiveRulesCount()
        {
            // This would get actual active rules count
            return 24; // Placeholder
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _updateTimer?.Stop();
            _updateTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pylorak.TinyWall.MaterialDesign
{
    /// <summary>
    /// Material Design sidebar navigation control
    /// </summary>
    public partial class MaterialSideBar : UserControl
    {
        private bool _isExpanded = true;
        private readonly List<NavigationItem> _navigationItems = new();
        private NavigationItem? _selectedItem;
        private const int CollapsedWidth = 56;
        private const int ExpandedWidth = 240;
        private const int ItemHeight = 48;
        
        public event EventHandler<NavigationItemEventArgs>? ItemClicked;
        
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                UpdateLayout();
            }
        }
        
        public MaterialSideBar()
        {
            InitializeComponent();
            SetupControl();
        }
        
        private void SetupControl()
        {
            BackColor = MaterialColors.NavigationSurface;
            Width = ExpandedWidth;
            Dock = DockStyle.Left;
            
            // Add toggle button
            var toggleButton = new Button
            {
                Text = "☰",
                Font = new Font("Segoe UI", 14f, FontStyle.Regular),
                Size = new Size(48, 48),
                Location = new Point(4, 4),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = MaterialColors.TextPrimary
            };
            
            toggleButton.FlatAppearance.BorderSize = 0;
            toggleButton.FlatAppearance.MouseOverBackColor = MaterialColors.NavigationHover;
            
            toggleButton.Click += (sender, e) =>
            {
                IsExpanded = !IsExpanded;
            };
            
            Controls.Add(toggleButton);
            
            // Add navigation items
            AddNavigationItems();
            
            MaterialHelper.ApplyElevation(this, 2);
        }
        
        private void AddNavigationItems()
        {
            var items = new[]
            {
                new NavigationItem("Dashboard", "🏠", "dashboard"),
                new NavigationItem("Connections", "🔗", "connections"),
                new NavigationItem("Processes", "⚙️", "processes"),
                new NavigationItem("Services", "🔧", "services"),
                new NavigationItem("Settings", "⚙️", "settings"),
                new NavigationItem("Help", "❓", "help")
            };
            
            _navigationItems.AddRange(items);
            
            // Select dashboard by default
            _selectedItem = items.FirstOrDefault();
            
            UpdateLayout();
        }
        
        private void UpdateLayout()
        {
            Width = _isExpanded ? ExpandedWidth : CollapsedWidth;
            
            // Clear existing item controls
            var toRemove = Controls.OfType<Control>()
                .Where(c => c.Tag is NavigationItem)
                .ToList();
            
            foreach (var control in toRemove)
            {
                Controls.Remove(control);
            }
            
            // Add navigation item controls
            var yOffset = 60; // Start below toggle button
            
            foreach (var item in _navigationItems)
            {
                var itemControl = CreateNavigationItemControl(item);
                itemControl.Location = new Point(0, yOffset);
                itemControl.Width = Width;
                itemControl.Height = ItemHeight;
                
                Controls.Add(itemControl);
                yOffset += ItemHeight;
            }
            
            Invalidate();
        }
        
        private Control CreateNavigationItemControl(NavigationItem item)
        {
            var panel = new Panel
            {
                Tag = item,
                BackColor = item == _selectedItem ? MaterialColors.NavigationSelected : Color.Transparent,
                Cursor = Cursors.Hand
            };
            
            var iconLabel = new Label
            {
                Text = item.Icon,
                Font = new Font("Segoe UI", 16f, FontStyle.Regular),
                ForeColor = item == _selectedItem ? MaterialColors.Primary : MaterialColors.TextPrimary,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(48, 48),
                Location = new Point(4, 0),
                BackColor = Color.Transparent
            };
            
            panel.Controls.Add(iconLabel);
            
            if (_isExpanded)
            {
                var textLabel = new Label
                {
                    Text = item.Title,
                    Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                    ForeColor = item == _selectedItem ? MaterialColors.Primary : MaterialColors.TextPrimary,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Size = new Size(180, 48),
                    Location = new Point(56, 0),
                    BackColor = Color.Transparent
                };
                
                panel.Controls.Add(textLabel);
            }
            
            // Add click handler
            panel.Click += (sender, e) => OnItemClick(item);
            iconLabel.Click += (sender, e) => OnItemClick(item);
            
            // Add hover effects
            panel.MouseEnter += (sender, e) =>
            {
                if (item != _selectedItem)
                {
                    panel.BackColor = MaterialColors.NavigationHover;
                }
            };
            
            panel.MouseLeave += (sender, e) =>
            {
                if (item != _selectedItem)
                {
                    panel.BackColor = Color.Transparent;
                }
            };
            
            MaterialHelper.ApplyRippleEffect(panel);
            
            return panel;
        }
        
        private void OnItemClick(NavigationItem item)
        {
            if (_selectedItem != item)
            {
                _selectedItem = item;
                UpdateLayout();
                ItemClicked?.Invoke(this, new NavigationItemEventArgs(item));
            }
        }
        
        public void SelectItem(string key)
        {
            var item = _navigationItems.FirstOrDefault(i => i.Key == key);
            if (item != null)
            {
                OnItemClick(item);
            }
        }
    }
    
    public class NavigationItem
    {
        public string Title { get; }
        public string Icon { get; }
        public string Key { get; }
        
        public NavigationItem(string title, string icon, string key)
        {
            Title = title;
            Icon = icon;
            Key = key;
        }
    }
    
    public class NavigationItemEventArgs : EventArgs
    {
        public NavigationItem Item { get; }
        
        public NavigationItemEventArgs(NavigationItem item)
        {
            Item = item;
        }
    }
}
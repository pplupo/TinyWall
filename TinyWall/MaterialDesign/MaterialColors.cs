using System.Drawing;

namespace pylorak.TinyWall.MaterialDesign
{
    /// <summary>
    /// Material Design color constants following Google's Material Design guidelines
    /// </summary>
    public static class MaterialColors
    {
        // Primary colors
        public static readonly Color Primary = Color.FromArgb(33, 150, 243);  // Blue 500
        public static readonly Color PrimaryDark = Color.FromArgb(25, 118, 210);  // Blue 600
        public static readonly Color PrimaryLight = Color.FromArgb(100, 181, 246);  // Blue 300
        
        // Accent colors
        public static readonly Color Accent = Color.FromArgb(255, 64, 129);  // Pink A200
        public static readonly Color AccentLight = Color.FromArgb(255, 138, 180);  // Pink A100
        
        // Surface colors
        public static readonly Color Surface = Color.FromArgb(255, 255, 255);
        public static readonly Color SurfaceDark = Color.FromArgb(18, 18, 18);
        public static readonly Color Background = Color.FromArgb(250, 250, 250);
        public static readonly Color BackgroundDark = Color.FromArgb(48, 48, 48);
        
        // Text colors
        public static readonly Color TextPrimary = Color.FromArgb(33, 33, 33);
        public static readonly Color TextSecondary = Color.FromArgb(117, 117, 117);
        public static readonly Color TextPrimaryDark = Color.FromArgb(255, 255, 255);
        public static readonly Color TextSecondaryDark = Color.FromArgb(180, 180, 180);
        
        // Status colors
        public static readonly Color Success = Color.FromArgb(76, 175, 80);  // Green 500
        public static readonly Color Warning = Color.FromArgb(255, 193, 7);  // Amber 500
        public static readonly Color Error = Color.FromArgb(244, 67, 54);  // Red 500
        
        // Elevation shadows
        public static readonly Color ShadowLight = Color.FromArgb(30, 0, 0, 0);
        public static readonly Color ShadowMedium = Color.FromArgb(60, 0, 0, 0);
        public static readonly Color ShadowDark = Color.FromArgb(90, 0, 0, 0);
        
        // Navigation colors
        public static readonly Color NavigationSurface = Color.FromArgb(255, 255, 255);
        public static readonly Color NavigationSelected = Color.FromArgb(225, 245, 254);  // Blue 50
        public static readonly Color NavigationHover = Color.FromArgb(245, 245, 245);  // Grey 50
        
        // Firewall specific colors
        public static readonly Color FirewallEnabled = Color.FromArgb(76, 175, 80);  // Green
        public static readonly Color FirewallDisabled = Color.FromArgb(158, 158, 158);  // Grey
        public static readonly Color FirewallBlocked = Color.FromArgb(244, 67, 54);  // Red
        public static readonly Color FirewallAllowed = Color.FromArgb(139, 195, 74);  // Light Green
    }
}
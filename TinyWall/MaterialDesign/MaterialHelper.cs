using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace pylorak.TinyWall.MaterialDesign
{
    /// <summary>
    /// Helper class for Material Design effects and styling
    /// </summary>
    public static class MaterialHelper
    {
        /// <summary>
        /// Applies Material Design elevation shadow to a control
        /// </summary>
        public static void ApplyElevation(Control control, int elevation = 2)
        {
            control.Paint += (sender, e) =>
            {
                var rect = new Rectangle(0, 0, control.Width, control.Height);
                DrawElevationShadow(e.Graphics, rect, elevation);
            };
        }
        
        /// <summary>
        /// Draws elevation shadow for Material Design
        /// </summary>
        public static void DrawElevationShadow(Graphics graphics, Rectangle bounds, int elevation)
        {
            var shadowOffset = Math.Max(1, elevation);
            var shadowColor = elevation switch
            {
                1 => MaterialColors.ShadowLight,
                2 => MaterialColors.ShadowMedium,
                _ => MaterialColors.ShadowDark
            };
            
            using var shadowBrush = new SolidBrush(shadowColor);
            var shadowRect = new Rectangle(bounds.X + shadowOffset, bounds.Y + shadowOffset, 
                                         bounds.Width - shadowOffset, bounds.Height - shadowOffset);
            
            graphics.FillRectangle(shadowBrush, shadowRect);
        }
        
        /// <summary>
        /// Creates a Material Design style button
        /// </summary>
        public static void StyleMaterialButton(Button button, bool isPrimary = false)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Height = 36;
            button.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            
            if (isPrimary)
            {
                button.BackColor = MaterialColors.Primary;
                button.ForeColor = Color.White;
                button.FlatAppearance.MouseOverBackColor = MaterialColors.PrimaryDark;
            }
            else
            {
                button.BackColor = Color.Transparent;
                button.ForeColor = MaterialColors.Primary;
                button.FlatAppearance.MouseOverBackColor = MaterialColors.NavigationHover;
            }
            
            // Add elevation effect
            button.Paint += (sender, e) =>
            {
                if (isPrimary)
                {
                    var rect = new Rectangle(0, 0, button.Width, button.Height);
                    DrawElevationShadow(e.Graphics, rect, 2);
                }
            };
        }
        
        /// <summary>
        /// Creates a Material Design style panel
        /// </summary>
        public static void StyleMaterialPanel(Panel panel, bool elevated = true)
        {
            panel.BackColor = MaterialColors.Surface;
            panel.Padding = new Padding(16);
            
            if (elevated)
            {
                ApplyElevation(panel, 1);
            }
        }
        
        /// <summary>
        /// Creates a Material Design style text box
        /// </summary>
        public static void StyleMaterialTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = MaterialColors.Surface;
            textBox.ForeColor = MaterialColors.TextPrimary;
            textBox.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            textBox.Height = 32;
            
            // Add bottom border
            textBox.Paint += (sender, e) =>
            {
                using var pen = new Pen(MaterialColors.TextSecondary, 1);
                e.Graphics.DrawLine(pen, 0, textBox.Height - 1, textBox.Width, textBox.Height - 1);
            };
        }
        
        /// <summary>
        /// Creates a Material Design style label
        /// </summary>
        public static void StyleMaterialLabel(Label label, bool isSecondary = false)
        {
            label.ForeColor = isSecondary ? MaterialColors.TextSecondary : MaterialColors.TextPrimary;
            label.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            label.BackColor = Color.Transparent;
        }
        
        /// <summary>
        /// Creates a Material Design style header label
        /// </summary>
        public static void StyleMaterialHeader(Label label, int level = 1)
        {
            label.ForeColor = MaterialColors.TextPrimary;
            label.BackColor = Color.Transparent;
            
            label.Font = level switch
            {
                1 => new Font("Segoe UI", 24f, FontStyle.Light),
                2 => new Font("Segoe UI", 18f, FontStyle.Regular),
                3 => new Font("Segoe UI", 14f, FontStyle.Medium),
                _ => new Font("Segoe UI", 12f, FontStyle.Regular)
            };
        }
        
        /// <summary>
        /// Applies Material Design ripple effect to a control
        /// </summary>
        public static void ApplyRippleEffect(Control control)
        {
            var rippleTimer = new Timer { Interval = 50 };
            var rippleProgress = 0f;
            Point rippleCenter = Point.Empty;
            
            control.MouseDown += (sender, e) =>
            {
                rippleCenter = e.Location;
                rippleProgress = 0f;
                rippleTimer.Start();
            };
            
            rippleTimer.Tick += (sender, e) =>
            {
                rippleProgress += 0.1f;
                control.Invalidate();
                
                if (rippleProgress >= 1f)
                {
                    rippleTimer.Stop();
                }
            };
            
            control.Paint += (sender, e) =>
            {
                if (rippleProgress > 0f && rippleProgress < 1f)
                {
                    var maxRadius = Math.Max(control.Width, control.Height);
                    var currentRadius = (int)(maxRadius * rippleProgress);
                    var alpha = (int)(100 * (1f - rippleProgress));
                    
                    using var brush = new SolidBrush(Color.FromArgb(alpha, MaterialColors.Primary));
                    e.Graphics.FillEllipse(brush, 
                        rippleCenter.X - currentRadius, 
                        rippleCenter.Y - currentRadius, 
                        currentRadius * 2, 
                        currentRadius * 2);
                }
            };
        }
    }
}
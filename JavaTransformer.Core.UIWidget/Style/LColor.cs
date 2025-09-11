using JavaTransformer.Core.UIWidget.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Xps.Serialization;

namespace JavaTransformer.Core.UIWidget.Style
{
    public interface DefaultColorGetters
    {
        byte GetRed();
        byte GetGreen();
        byte GetBlue();
        byte GetAlpha();
    }

    public interface DefaultColorSetters
    {
        void SetRed(byte red);
        void SetGreen(byte green);
        void SetBlue(byte blue);
        void SetAlpha(byte alpha);
    }

    public interface DefaultColor : DefaultColorGetters, DefaultColorSetters
    {

    }

    public class LColor : DefaultColor, 
        IEquatable<LColor>, IEquatable<DefaultColorGetters>
    {
        private byte r;
        private byte g;
        private byte b;
        private byte a;

        public LColor(Brush brush)
            : this(brush.ToLColor())
        {
        
        }

        public LColor(string hex) : this(ConvertString(hex))
        {

        }

        public LColor(LColor copy)
        {
            r = copy.r;
            g = copy.g;
            b = copy.b;
            a = copy.a;
        }

        public LColor(Color color)
            : this(color.R, color.G, color.B, color.A)
        {

        }

        public LColor(byte r, byte g, byte b) 
            : this(r, g, b, MAX_COLOR)
        {
            
        }

        public LColor(byte r, byte g, byte b, byte a)
        {
            setColor(r, g, b, a);   
        }

        protected void setColor(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public bool Equals(LColor? other)
        {
            if (other is null) return false;

            return r == other.r 
                && g == other.g 
                && b == other.b 
                && a == other.a;
        }

        public bool Equals(DefaultColorGetters? other)
        {
            if (other is null) return false;

            return r == other.GetRed()
                && g == other.GetGreen()
                && b == other.GetBlue()
                && a == other.GetAlpha();
        }

        public byte GetRed()
        {
            return r;
        }

        public byte GetGreen()
        {
            return g;
        }

        public byte GetBlue()
        {
            return b;
        }

        public byte GetAlpha()
        {
            return a;
        }

        public void SetRed(byte red)
        {
            r = red;
        }

        public void SetGreen(byte green)
        {
            g = green;
        }

        public void SetBlue(byte blue)
        {
            b = blue;
        }

        public void SetAlpha(byte alpha)
        {
            a = alpha;
        }

        public Color FromRgb()
        {
            return Color.FromArgb(a, r, g, b);
        }

        public SolidColorBrush FromSolidColorBrush()
        {
            return GetSolidColorBrush(this);
        }

        public override string ToString()
        {
            return $"R={r},G={g},B={b},A={a}";
        }

        public static LColor ConvertString(string text)
        {
            string lower = text.ToLower();
            if (Colors.ContainsKey(lower))
            {
                return Colors[lower];
            }

            return ConvertUseHexString(text);
        }

        public static SolidColorBrush GetSolidColorBrush(LColor instance)
        {
            return new SolidColorBrush(instance.FromRgb());
        }

        public static LColor ConvertUseHexString(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Hex string cannot be null or empty", nameof(hex));

            string cleanHex = hex.Trim().Replace("#", "").Replace("0x", "").Replace("0X", "");


            if (cleanHex.Length != 3 && cleanHex.Length != 6 && cleanHex.Length != 8)
                throw new ArgumentException("Hex string must be 3, 6 or 8 characters long", nameof(hex));

            if (!System.Text.RegularExpressions.Regex.IsMatch(cleanHex, @"^[0-9a-fA-F]+$"))
                throw new ArgumentException("Hex string contains invalid characters", nameof(hex));


            byte r, g, b, a = 255;

            try
            {
                switch (cleanHex.Length)
                {
                    case 3:
                        r = Convert.ToByte($"{cleanHex[0]}{cleanHex[0]}", 16);
                        g = Convert.ToByte($"{cleanHex[1]}{cleanHex[1]}", 16);
                        b = Convert.ToByte($"{cleanHex[2]}{cleanHex[2]}", 16);
                        break;

                    case 6:
                        r = Convert.ToByte(cleanHex.Substring(0, 2), 16);
                        g = Convert.ToByte(cleanHex.Substring(2, 2), 16);
                        b = Convert.ToByte(cleanHex.Substring(4, 2), 16);
                        break;

                    case 8:
                        r = Convert.ToByte(cleanHex.Substring(0, 2), 16);
                        g = Convert.ToByte(cleanHex.Substring(2, 2), 16);
                        b = Convert.ToByte(cleanHex.Substring(4, 2), 16);
                        a = Convert.ToByte(cleanHex.Substring(6, 2), 16);
                        break;

                    default:
                        throw new ArgumentException("Invalid hex length", nameof(hex));
                }
            }
            catch (Exception ex) when (ex is FormatException or OverflowException)
            {
                throw new ArgumentException("Invalid hex format", nameof(hex), ex);
            }

            return new LColor(r, g, b,a);
        }




        public static LColor of(Color color)
        {
            return new LColor(color);
        }

        public static LColor of(string text)
        {
            return new LColor(text);
        } 

        public static LColor of(Brush brush)
        {
            return new LColor(brush);
        }

        public const byte MAX_COLOR = byte.MaxValue;

        public static Dictionary<string, LColor> Colors = new Dictionary<string, LColor>(StringComparer.OrdinalIgnoreCase)
        {
            {"red", new LColor(255, 0, 0, 255) },
            {"green", new LColor(0, 255, 0, 255) },
            {"blue", new LColor(0, 0, 255, 255) },
            {"yellow", new LColor(255, 255, 0, 255) },
            {"magenta", new LColor(255, 0, 255, 255) },
            {"cyan", new LColor(0, 255, 255, 255) },
            {"orange", new LColor(255, 165, 0, 255) },
            {"purple", new LColor(128, 0, 128, 255) },
            {"pink", new LColor(255, 192, 203, 255) },
            {"white", new LColor(255, 255, 255, 255) },
            {"black", new LColor(0, 0, 0, 255) },
            {"gray", new LColor(128, 128, 128, 255) },
            {"grey", new LColor(128, 128, 128, 255) },
            {"lightgray", new LColor(211, 211, 211, 255) },
            {"darkgray", new LColor(169, 169, 169, 255) },
            {"silver", new LColor(192, 192, 192, 255) },
            {"maroon", new LColor(128, 0, 0, 255) },
            {"olive", new LColor(128, 128, 0, 255) },
            {"lime", new LColor(0, 255, 0, 255) },
            {"teal", new LColor(0, 128, 128, 255) },
            {"navy", new LColor(0, 0, 128, 255) },
            {"fuchsia", new LColor(255, 0, 255, 255) },
            {"aqua", new LColor(0, 255, 255, 255) },
            {"brown", new LColor(165, 42, 42, 255) },
            {"sienna", new LColor(160, 82, 45, 255) },
            {"chocolate", new LColor(210, 105, 30, 255) },
            {"tan", new LColor(210, 180, 140, 255) },
            {"beige", new LColor(245, 245, 220, 255) },
            {"wheat", new LColor(245, 222, 179, 255) },
            {"lightpink", new LColor(255, 182, 193, 255) },
            {"lightblue", new LColor(173, 216, 230, 255) },
            {"lightgreen", new LColor(144, 238, 144, 255) },
            {"lightyellow", new LColor(255, 255, 224, 255) },
            {"lightcyan", new LColor(224, 255, 255, 255) },
            {"darkred", new LColor(139, 0, 0, 255) },
            {"darkgreen", new LColor(0, 100, 0, 255) },
            {"darkblue", new LColor(0, 0, 139, 255) },
            {"darkorange", new LColor(255, 140, 0, 255) },
            {"darkviolet", new LColor(148, 0, 211, 255) },
            {"gold", new LColor(255, 215, 0, 255) },
            {"violet", new LColor(238, 130, 238, 255) },
            {"indigo", new LColor(75, 0, 130, 255) },
            {"turquoise", new LColor(64, 224, 208, 255) },
            {"coral", new LColor(255, 127, 80, 255) },
            {"salmon", new LColor(250, 128, 114, 255) },
            {"tomato", new LColor(255, 99, 71, 255) },
            {"transparent", new LColor(0, 0, 0, 0) },
            {"semitransparent", new LColor(255, 255, 255, 128) },
            {"goldenrod", new LColor(218, 165, 32, 255) },
            {"bronze", new LColor(205, 127, 50, 255) },
            {"skyblue", new LColor(135, 206, 235, 255) },
            {"forestgreen", new LColor(34, 139, 34, 255) },
            {"seagreen", new LColor(46, 139, 87, 255) },
            {"royalblue", new LColor(65, 105, 225, 255) },
            {"midnightblue", new LColor(25, 25, 112, 255) },
            {"orchid", new LColor(218, 112, 214, 255) },
            {"plum", new LColor(221, 160, 221, 255) },
            {"thistle", new LColor(216, 191, 216, 255) },
            {"lavender", new LColor(230, 230, 250, 255) },
            {"azure", new LColor(240, 255, 255, 255) },
            {"ivory", new LColor(255, 255, 240, 255) },
            {"snow", new LColor(255, 250, 250, 255) },
            {"ghostwhite", new LColor(248, 248, 255, 255) }
        };
    }
}

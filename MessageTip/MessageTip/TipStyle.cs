using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using AhDung.Drawing;

namespace AhDung
{
    /// <summary>
    /// 消息窗样式
    /// </summary>
    public sealed class TipStyle : IDisposable
    {
        bool _isPresets;
        bool _keepFont;
        bool _keepIcon;

        readonly Border _border;
        /// <summary>
        /// 获取边框信息。内部用
        /// </summary>
        internal Border Border => _border;

        /// <summary>
        /// 获取或设置图标。默认null
        /// </summary>
        public Bitmap Icon { get; set; }

        /// <summary>
        /// 获取或设置图标与文本的间距。默认为3像素
        /// </summary>
        public int IconSpacing { get; set; }

        /// <summary>
        /// 获取或设置文本字体。默认12号的消息框文本
        /// </summary>
        public Font TextFont { get; set; }

        /// <summary>
        /// 获取或设置文本偏移，用于微调
        /// </summary>
        public Point TextOffset { get; set; }

        /// <summary>
        /// 获取或设置文本颜色（默认黑色）
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// 获取或设置背景颜色（默认浅白）
        /// <para>- 若想呈现多色及复杂背景，请使用BackBrush属性，当后者不为null时，本属性被忽略</para>
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// 获取或设置背景画刷生成方法
        /// <para>- 是个委托，入参矩形由绘制函数传入，表示内容区，便于构造画刷</para>
        /// <para>- 默认null，为null时使用BackColor绘制单色背景</para>
        /// <para>- 方法返回的画刷需释放</para>
        /// </summary>
        public BrushSelector<Rectangle> BackBrush { get; set; }

        /// <summary>
        /// 获取或设置边框颜色（默认深灰）
        /// </summary>
        public Color BorderColor
        {
            get => _border.Color;
            set => _border.Color = value;
        }

        /// <summary>
        /// 获取或设置边框粗细（默认1）
        /// </summary>
        public int BorderWidth
        {
            get => _border.Width / 2;
            set => _border.Width = value * 2;
        }

        /// <summary>
        /// 获取或设置圆角半径（默认3）
        /// </summary>
        public int CornerRadius { get; set; }

        /// <summary>
        /// 获取或设置阴影颜色（默认深灰）
        /// </summary>
        public Color ShadowColor { get; set; }

        /// <summary>
        /// 获取或设置阴影羽化半径（默认4）
        /// </summary>
        public int ShadowRadius { get; set; }

        /// <summary>
        /// 获取或设置阴影偏移（默认x=0,y=3）
        /// </summary>
        public Point ShadowOffset { get; set; }

        /// <summary>
        /// 获取或设置四周空白（默认left,right=10; top,bottom=5）
        /// </summary>
        public Padding Padding { get; set; }

        /// <summary>
        /// 初始化样式
        /// </summary>
        public TipStyle()
        {
            _border = new Border(PresetsResources.Colors[0, 0])
            {
                Behind = true,
                Width = 2
            };
            IconSpacing = 5;
            TextFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 12);
            var fontName = TextFont.Name;
            if (fontName == "宋体") { TextOffset = new Point(1, 1); }
            TextColor = Color.Black;
            BackColor = Color.FromArgb(252, 252, 252);
            CornerRadius = 3;
            ShadowColor = PresetsResources.Colors[0, 2];
            ShadowRadius = 4;
            ShadowOffset = new Point(0, 3);
            Padding = new Padding(10, 5, 10, 5);
        }

        /// <summary>
        /// 清理本类使用的资源
        /// </summary>
        /// <param name="keepFont">是否保留字体</param>
        /// <param name="keepIcon">是否保留图标</param>
        public void Clear(bool keepFont = false, bool keepIcon = false)
        {
            _keepFont = keepFont;
            _keepIcon = keepIcon;
            ((IDisposable)this).Dispose();
        }

        static TipStyle _gray;
        /// <summary>
        /// 预置的灰色样式
        /// </summary>
        public static TipStyle Gray => _gray ?? (_gray = CreatePresetsStyle(0));

        static TipStyle _green;
        /// <summary>
        /// 预置的绿色样式
        /// </summary>
        public static TipStyle Green => _green ?? (_green = CreatePresetsStyle(1));

        static TipStyle _orange;
        /// <summary>
        /// 预置的橙色样式
        /// </summary>
        public static TipStyle Orange => _orange ?? (_orange = CreatePresetsStyle(2));

        static TipStyle _red;
        /// <summary>
        /// 预置的红色样式
        /// </summary>
        public static TipStyle Red => _red ?? (_red = CreatePresetsStyle(3));

        private static TipStyle CreatePresetsStyle(int index)
        {
            var style = new TipStyle
            {
                Icon = PresetsResources.Icons[index],
                BorderColor = PresetsResources.Colors[index, 0],
                ShadowColor = PresetsResources.Colors[index, 2],
                _isPresets = true
            };
            style.BackBrush = r =>
            {
                var brush = new LinearGradientBrush(r,
                    PresetsResources.Colors[index, 1],
                    Color.White,
                    LinearGradientMode.Horizontal);
                brush.SetBlendTriangularShape(0.5f);
                return brush;
            };
            return style;
        }

        bool _disposed;
        [Obsolete("请改用Clear指定是否清理字体和图标")]
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDisposable.Dispose()
        {
            if (_disposed || _isPresets)//不销毁预置对象
            {
                return;
            }

            _border.Dispose();
            BackBrush = null;
            if (!_keepFont && TextFont != null && !TextFont.IsSystemFont)
            {
                TextFont.Dispose();
                TextFont = null;
            }
            if (!_keepIcon && Icon != null)
            {
                Icon.Dispose();
                Icon = null;
            }

            _disposed = true;
        }

        /// <summary>
        /// 预置资源
        /// </summary>
        private static class PresetsResources
        {
            public static readonly Color[,] Colors =
            {
                    //边框色、背景色、阴影色
             /*灰*/ {Color.FromArgb(150,150,150), Color.FromArgb(245,245,245), Color.FromArgb(110,  0,  0,  0)},
             /*绿*/ {Color.FromArgb(  0,189,  0), Color.FromArgb(232,255,232), Color.FromArgb(150,  0,150,  0)},
             /*橙*/ {Color.FromArgb(255,150,  0), Color.FromArgb(255,250,240), Color.FromArgb(150,250,100,  0)},
             /*红*/ {Color.FromArgb(255, 79, 79), Color.FromArgb(255,245,245), Color.FromArgb(140,255, 30, 30)}
            };

            //CreateIcon依赖Colors，所以需在Colors后初始化
            public static readonly Bitmap[] Icons =
            {
                CreateIcon(0),
                CreateIcon(1),
                CreateIcon(2),
                CreateIcon(3)
            };

            /// <summary>
            /// 创建图标
            /// </summary>
            /// <param name="index">0=i；1=√；2=！；3=×；其余=null</param>
            private static Bitmap CreateIcon(int index)
            {
                if (index < 0 || index > 3)
                {
                    return null;
                }
                Graphics g = null;
                Pen pen = null;
                Brush brush = null;
                Bitmap bmp = null;
                try
                {
                    bmp = new Bitmap(24, 24);
                    g = Graphics.FromImage(bmp);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    var color = Colors[index, 0];
                    if (index == 0) //i
                    {
                        brush = new SolidBrush(Color.FromArgb(103, 148, 186));
                        g.FillEllipse(brush, 3, 3, 18, 18);

                        pen = new Pen(Colors[index, 1], 2);
                        g.DrawLine(pen, new Point(12, 6), new Point(12, 8));
                        g.DrawLine(pen, new Point(12, 10), new Point(12, 18));
                    }
                    else if (index == 1) //√
                    {
                        pen = new Pen(color, 4);
                        g.DrawLines(pen, new[] { new Point(3, 11), new Point(10, 18), new Point(20, 5) });
                    }
                    else if (index == 2) //！
                    {
                        var points = new[] { new Point(12, 3), new Point(3, 20), new Point(21, 20) };
                        pen = new Pen(color, 2) { LineJoin = LineJoin.Bevel };
                        g.DrawPolygon(pen, points);

                        brush = new SolidBrush(color);
                        g.FillPolygon(brush, points);

                        pen.Color = Colors[index, 1];
                        g.DrawLine(pen, new Point(12, 8), new Point(12, 15));
                        g.DrawLine(pen, new Point(12, 17), new Point(12, 19));
                    }
                    else //×
                    {
                        pen = new Pen(color, 4);
                        g.DrawLine(pen, 5, 5, 19, 19);
                        g.DrawLine(pen, 5, 19, 19, 5);
                    }
                    return bmp;
                }
                catch
                {
                    bmp?.Dispose();
                    throw;
                }
                finally
                {
                    pen?.Dispose();
                    brush?.Dispose();
                    g?.Dispose();
                }
            }
        }
    }

    //干脆自建一个委托，不依赖Func了
    /// <summary>
    /// 画刷选择器委托
    /// </summary>
    public delegate Brush BrushSelector<in T>(T arg);
}

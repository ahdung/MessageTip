using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AhDung.Drawing
{
    internal static class GraphicsUtils
    {
        /// <summary>
        /// 计算指定边界添加描边和阴影后的边界
        /// </summary>
        public static Rectangle GetBounds(Rectangle rectangle, Border border = null, int shadowRadius = 0, int offsetX = 0, int offsetY = 0)
        {
            if (border != null)
            {
                rectangle = border.GetBounds(rectangle);
            }
            var boundsShadow = DropShadow.GetBounds(rectangle, shadowRadius);
            boundsShadow.Offset(offsetX, offsetY);
            return Rectangle.Union(rectangle, boundsShadow);
        }

        /// <summary>
        /// 测量文本区尺寸
        /// </summary>
        public static SizeF MeasureString(string text, Font font, int width = 0, StringFormat stringFormat = null) =>
            MeasureString(text, font, new SizeF(width, width > 0 ? 999999 : 0), stringFormat);

        /// <summary>
        /// 测量文本区尺寸
        /// </summary>
        public static SizeF MeasureString(string text, Font font, SizeF area, StringFormat stringFormat = null)
        {
            IntPtr dcScreen = IntPtr.Zero;
            Graphics g = null;
            try
            {
                dcScreen = GetDC(IntPtr.Zero);
                g = Graphics.FromHdc(dcScreen);
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                return g.MeasureString(text, font, area, stringFormat);
            }
            finally
            {
                g?.Dispose();
                if (dcScreen != IntPtr.Zero)
                {
                    ReleaseDC(IntPtr.Zero, dcScreen);
                }
            }
        }

        /// <summary>
        /// 构造圆角矩形
        /// </summary>
        private static GraphicsPath GetRoundedRectangle(Rectangle rectangle, int radius)
        {
            //合理化圆角半径
            radius = Math.Min(radius, Math.Min(rectangle.Width, rectangle.Height) / 2);

            var path = new GraphicsPath();
            if (radius < 1)
            {
                path.AddRectangle(rectangle);
                return path;
            }

            int d = radius * 2;
            var arc = new Rectangle(rectangle.X, rectangle.Y, d, d);
            path.AddArc(arc, 180, 90);
            arc.X = rectangle.X + rectangle.Width - d;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Y + rectangle.Height - d;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.X;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 绘制矩形。可带圆角、阴影
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle">矩形</param>
        /// <param name="brush">用于填充的画刷。为null则不填充</param>
        /// <param name="border">边框描述对象。对象无效则不描边</param>
        /// <param name="radius">圆角半径</param>
        /// <param name="shadowColor">阴影颜色</param>
        /// <param name="shadowRadius">阴影羽化半径</param>
        /// <param name="offsetX">阴影横向偏移</param>
        /// <param name="offsetY">阴影纵向偏移</param>
        public static void DrawRectangle(Graphics g, Rectangle rectangle, Brush brush, Border border, int radius, Color shadowColor, int shadowRadius = 0, int offsetX = 0, int offsetY = 0)
        {
            if (shadowColor.A == 0 || (shadowRadius == 0 && offsetX == 0 && offsetY == 0))
            {
                DrawRectangle(g, rectangle, brush, border, radius);
                return;
            }

            GraphicsPath path = null;
            Bitmap shadow = null;
            try
            {
                path = GetRoundedRectangle(rectangle, radius);
                shadow = DropShadow.Create(path, shadowColor, shadowRadius);

                var shadowBounds = DropShadow.GetBounds(rectangle, shadowRadius);
                shadowBounds.Offset(offsetX, offsetY);

                g.DrawImageUnscaled(shadow, shadowBounds.Location);
                DrawPath(g, path, brush, border);
            }
            finally
            {
                path?.Dispose();
                shadow?.Dispose();
            }
        }

        /// <summary>
        /// 画矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle">矩形</param>
        /// <param name="brush">用于填充的画刷。为null则不填充</param>
        /// <param name="border">边框描述对象。对象无效则不描边</param>
        /// <param name="radius">圆角半径</param>
        public static void DrawRectangle(Graphics g, Rectangle rectangle, Brush brush = null, Border border = null, int radius = 0)
        {
            using (var path = GetRoundedRectangle(rectangle, radius))
            {
                DrawPath(g, path, brush, border);
            }
        }

        /// <summary>
        /// 画路径
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path">路径</param>
        /// <param name="brush">用于填充的画刷。为null则不填充</param>
        /// <param name="border">边框描述对象。对象无效则不描边</param>
        public static void DrawPath(Graphics g, GraphicsPath path, Brush brush = null, Border border = null)
        {
            if (Border.IsValid(border) && border.Behind)
            {
                g.DrawPath(border.Pen, path);
            }
            if (brush != null)
            {
                g.FillPath(brush, path);
            }
            if (Border.IsValid(border) && !border.Behind)
            {
                g.DrawPath(border.Pen, path);
            }
        }


        /// <summary>
        /// 阴影生成类
        /// </summary>
        /* =================================================================================
         * 算法源于：http://blog.ivank.net/fastest-gaussian-blur.html
         * C#版实现取自：http://stackoverflow.com/questions/7364026/algorithm-for-fast-drop-shadow-in-gdi
         * 修改+优化：AhDung
         * - unsafe转safe
         * - RGB色值处理。解决边缘羽化区域黑化问题
         * - 减少运算量
         * =================================================================================
         */
        private static class DropShadow
        {
            const int CHANNELS = 4;
            const int InflateMultiple = 2;//单边外延radius的倍数

            /// <summary>
            /// 获取阴影边界。供外部定位阴影用
            /// </summary>
            /// <param name="path">形状</param>
            /// <param name="radius">模糊半径</param>
            /// <param name="pathBounds">形状边界</param>
            /// <param name="inflate">单边外延像素</param>
            public static Rectangle GetBounds(GraphicsPath path, int radius, out Rectangle pathBounds, out int inflate)
            {
                var bounds = pathBounds = Rectangle.Ceiling(path.GetBounds());
                inflate = radius * InflateMultiple;
                bounds.Inflate(inflate, inflate);
                return bounds;
            }

            /// <summary>
            /// 获取阴影边界
            /// </summary>
            /// <param name="source">原边界</param>
            /// <param name="radius">模糊半径</param>
            public static Rectangle GetBounds(Rectangle source, int radius)
            {
                var inflate = radius * InflateMultiple;
                source.Inflate(inflate, inflate);
                return source;
            }

            /// <summary>
            /// 创建阴影图片
            /// </summary>
            /// <param name="path">阴影形状</param>
            /// <param name="color">阴影颜色</param>
            /// <param name="radius">模糊半径</param>
            public static Bitmap Create(GraphicsPath path, Color color, int radius = 5)
            {
                var bounds = GetBounds(path, radius, out Rectangle pathBounds, out int inflate);
                var shadow = new Bitmap(bounds.Width, bounds.Height);

                if (color.A == 0)
                {
                    return shadow;
                }

                //将形状用color色画在阴影区中心
                Graphics g = null;
                GraphicsPath pathCopy = null;
                Matrix matrix = null;
                SolidBrush brush = null;
                try
                {
                    matrix = new Matrix();
                    matrix.Translate(-pathBounds.X + inflate, -pathBounds.Y + inflate);//先清除形状原有偏移再向中心偏移
                    pathCopy = (GraphicsPath)path.Clone();                             //基于形状副本操作
                    pathCopy.Transform(matrix);

                    brush = new SolidBrush(color);

                    g = Graphics.FromImage(shadow);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.FillPath(brush, pathCopy);
                }
                finally
                {
                    g?.Dispose();
                    brush?.Dispose();
                    pathCopy?.Dispose();
                    matrix?.Dispose();
                }

                if (radius <= 0)
                {
                    return shadow;
                }

                BitmapData data = null;
                try
                {
                    data = shadow.LockBits(new Rectangle(0, 0, shadow.Width, shadow.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    //两次方框模糊就能达到不错的效果
                    //var boxes = DetermineBoxes(radius, 3);
                    BoxBlur(data, radius, color);
                    BoxBlur(data, radius, color);
                    //BoxBlur(shadowData, radius);

                    return shadow;
                }
                finally
                {
                    shadow.UnlockBits(data);
                }
            }

            /// <summary>
            /// 方框模糊
            /// </summary>
            /// <param name="data">图像内存数据</param>
            /// <param name="radius">模糊半径</param>
            /// <param name="color">透明色值</param>
#if UNSAFE
            private static unsafe void BoxBlur(BitmapData data, int radius, Color color)
#else
            private static void BoxBlur(BitmapData data, int radius, Color color)
#endif
            {
#if UNSAFE //unsafe项目下请定义编译条件：UNSAFE
                IntPtr p1 = data1.Scan0;
#else
                byte[] p1 = new byte[data.Stride * data.Height];
                Marshal.Copy(data.Scan0, p1, 0, p1.Length);
#endif
                //色值处理
                //这步的意义在于让图片中的透明像素拥有color的色值（但仍然保持透明）
                //这样在混合时才能合出基于color的颜色（只是透明度不同），
                //否则它是与RGB(0,0,0)合，就会得到乌黑的渣特技
                byte R = color.R, G = color.G, B = color.B;
                for (int i = 3; i < p1.Length; i += 4)
                {
                    if (p1[i] == 0)
                    {
                        p1[i - 1] = R;
                        p1[i - 2] = G;
                        p1[i - 3] = B;
                    }
                }

                byte[] p2   = new byte[p1.Length];
                int radius2 = 2 * radius + 1;
                int First, Last, Sum;
                int stride  = data.Stride,
                    width   = data.Width,
                    height  = data.Height;

                //只处理Alpha通道

                //横向
                for (int r = 0; r < height; r++)
                {
                    int start = r * stride;
                    int left = start;
                    int right = start + radius * CHANNELS;

                    First = p1[start + 3];
                    Last = p1[start + stride - 1];
                    Sum = (radius + 1) * First;

                    for (int column = 0; column < radius; column++)
                    {
                        Sum += p1[start + column * CHANNELS + 3];
                    }
                    for (var column = 0; column <= radius; column++, right += CHANNELS, start += CHANNELS)
                    {
                        Sum += p1[right + 3] - First;
                        p2[start + 3] = (byte)(Sum / radius2);
                    }
                    for (var column = radius + 1; column < width - radius; column++, left += CHANNELS, right += CHANNELS, start += CHANNELS)
                    {
                        Sum += p1[right + 3] - p1[left + 3];
                        p2[start + 3] = (byte)(Sum / radius2);
                    }
                    for (var column = width - radius; column < width; column++, left += CHANNELS, start += CHANNELS)
                    {
                        Sum += Last - p1[left + 3];
                        p2[start + 3] = (byte)(Sum / radius2);
                    }
                }

                //纵向
                for (int column = 0; column < width; column++)
                {
                    int start = column * CHANNELS;
                    int top = start;
                    int bottom = start + radius * stride;

                    First = p2[start + 3];
                    Last = p2[start + (height - 1) * stride + 3];
                    Sum = (radius + 1) * First;

                    for (int row = 0; row < radius; row++)
                    {
                        Sum += p2[start + row * stride + 3];
                    }
                    for (int row = 0; row <= radius; row++, bottom += stride, start += stride)
                    {
                        Sum += p2[bottom + 3] - First;
                        p1[start + 3] = (byte)(Sum / radius2);
                    }
                    for (int row = radius + 1; row < height - radius; row++, top += stride, bottom += stride, start += stride)
                    {
                        Sum += p2[bottom + 3] - p2[top + 3];
                        p1[start + 3] = (byte)(Sum / radius2);
                    }
                    for (int row = height - radius; row < height; row++, top += stride, start += stride)
                    {
                        Sum += Last - p2[top + 3];
                        p1[start + 3] = (byte)(Sum / radius2);
                    }
                }
#if !UNSAFE
                Marshal.Copy(p1, 0, data.Scan0, p1.Length);
#endif
            }

            //private static int[] DetermineBoxes(double Sigma, int BoxCount)
            //{
            //    double IdealWidth = Math.Sqrt((12 * Sigma * Sigma / BoxCount) + 1);
            //    int Lower = (int)Math.Floor(IdealWidth);
            //    if (Lower % 2 == 0)
            //        Lower--;
            //    int Upper = Lower + 2;

            //    double MedianWidth = (12 * Sigma * Sigma - BoxCount * Lower * Lower - 4 * BoxCount * Lower - 3 * BoxCount) / (-4 * Lower - 4);
            //    int Median = (int)Math.Round(MedianWidth);

            //    int[] BoxSizes = new int[BoxCount];
            //    for (int i = 0; i < BoxCount; i++)
            //        BoxSizes[i] = (i < Median) ? Lower : Upper;
            //    return BoxSizes;
            //}
        }

        #region Win32 API

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        #endregion
    }
}

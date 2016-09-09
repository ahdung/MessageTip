using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AhDung.WinForm
{
    /// <summary>
    /// 内置图标枚举
    /// </summary>
    public enum TipIcon
    {
        /// <summary>
        /// 无图标
        /// </summary>
        None,
        /// <summary>
        /// 良好。绿勾 √
        /// </summary>
        Ok,
        /// <summary>
        /// 警告。黄色感叹号 ！
        /// </summary>
        Warning,
        /// <summary>
        /// 错误。红叉 ×
        /// </summary>
        Error
    }

    /// <summary>
    /// 轻快型消息框
    /// </summary>
    public static class MessageTip
    {
        /// <summary>
        /// 内置图标数组，顺序与TipIcon枚举对应
        /// </summary>
        static readonly Image[] _icons;

        /// <summary>
        /// 全局停留时长（毫秒），影响后续弹出的tip。默认500
        /// </summary>
        public static int DefaultDelay { get; set; }

        /// <summary>
        /// 是否允许上浮动画。默认true
        /// </summary>
        public static bool AllowFloating { get; set; }

        static MessageTip()
        {
            DefaultDelay = 500;
            AllowFloating = true;

            Bitmap spriteImage;
            using (var ms = new MemoryStream(Convert.FromBase64String(DefaultIconData)))
            {
                //不能直接用Img.FromMs得到的对象，怀疑因该方法得到的对象与源ms有瓜葛
                //ms释放后会导致莫名问题，比如下面的Clone会引发内存不足异常
                //而new Bitmap(Image)相当于基于Image重造了一个全新的bmp
                spriteImage = new Bitmap(Image.FromStream(ms));
            }

            using (spriteImage)
            {
#if SmallSize
                const int imageSize = 24;
#else
                const int imageSize = 32;
#endif
                _icons = new Image[4]; //[0]=null
                _icons[1] = spriteImage.Clone(new Rectangle(0, 0, imageSize, imageSize), spriteImage.PixelFormat);
                _icons[2] = spriteImage.Clone(new RectangleF(imageSize, 0, imageSize, imageSize), spriteImage.PixelFormat);
                _icons[3] = spriteImage.Clone(new RectangleF(2 * imageSize, 0, imageSize, imageSize), spriteImage.PixelFormat);
            }
        }

        /// <summary>
        /// 在指定控件附近显示良好消息，图标为绿勾 √。与传入TipIcon.Ok等价
        /// </summary>
        /// <param name="controlOrItem">控件或工具栏项</param>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        public static void ShowOk(Component controlOrItem, string text = null, int delay = -1)
        {
            Show(controlOrItem, text, _icons[1], delay);
        }

        /// <summary>
        /// 显示良好消息，图标为绿勾 √。与传入TipIcon.Ok等价
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        /// <param name="point">指定显示位置</param>
        public static void ShowOk(string text = null, int delay = -1, Point? point = null)
        {
            Show(text, _icons[1], delay, point);
        }

        /// <summary>
        /// 在指定控件附近显示警告消息，图标为黄色感叹号 ！。与传入TipIcon.Warning等价
        /// </summary>
        /// <param name="controlOrItem">控件或工具栏项</param>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        public static void ShowWarning(Component controlOrItem, string text = null, int delay = -1)
        {
            Show(controlOrItem, text, _icons[2], delay);
        }

        /// <summary>
        /// 显示警告消息，图标为黄色感叹号 ！。与传入TipIcon.Warning等价
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        /// <param name="point">指定显示位置</param>
        public static void ShowWarning(string text = null, int delay = -1, Point? point = null)
        {
            Show(text, _icons[2], delay, point);
        }

        /// <summary>
        /// 在指定控件附近显示出错消息，图标为红叉 X。与传入TipIcon.Error等价
        /// </summary>
        /// <param name="controlOrItem">控件或工具栏项</param>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        public static void ShowError(Component controlOrItem, string text = null, int delay = -1)
        {
            Show(controlOrItem, text, _icons[3], delay);
        }

        /// <summary>
        /// 显示出错消息，图标为红叉 X。与传入TipIcon.Error等价
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        /// <param name="point">指定显示位置</param>
        public static void ShowError(string text = null, int delay = -1, Point? point = null)
        {
            Show(text, _icons[3], delay, point);
        }

        /// <summary>
        /// 在指定控件附近显示消息
        /// </summary>
        /// <param name="controlOrItem">控件或工具栏项</param>
        /// <param name="text">消息文本</param>
        /// <param name="tipIcon">内置图标</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        public static void Show(Component controlOrItem, string text, TipIcon tipIcon = TipIcon.None, int delay = -1)
        {
            if (controlOrItem == null)
            {
                throw new ArgumentNullException("controlOrItem");
            }
            Show(text, CheckAndConvertTipIconValue(tipIcon), delay, GetCenterPosition(controlOrItem));
        }

        /// <summary>
        /// 在指定控件附近显示消息
        /// </summary>
        /// <param name="controlOrItem">控件或工具栏项</param>
        /// <param name="text">消息文本</param>
        /// <param name="icon">图标</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        public static void Show(Component controlOrItem, string text, Image icon, int delay = -1)
        {
            if (controlOrItem == null)
            {
                throw new ArgumentNullException("controlOrItem");
            }
            Show(text, icon, delay, GetCenterPosition(controlOrItem));
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="tipIcon">内置图标</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        /// <param name="point">指定显示位置。为null则按活动控件</param>
        public static void Show(string text, TipIcon tipIcon = TipIcon.None, int delay = -1, Point? point = null)
        {
            Show(text, CheckAndConvertTipIconValue(tipIcon), delay, point);
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="icon">图标</param>
        /// <param name="delay">消息停留时长（毫秒）。指定负数则使用 DefaultDelay</param>
        /// <param name="point">指定显示位置。为null则按活动控件</param>
        public static void Show(string text, Image icon, int delay = -1, Point? point = null)
        {
            if (point == null)
            {
                //确定基准点
                var focusControl = Control.FromHandle(NativeMethods.GetFocus());

                if (focusControl is TextBoxBase)//若焦点是文本框，取光标位置
                {
                    var pt = GetCaretPosition();
                    pt.Y += focusControl.Font.Height / 2;
                    point = focusControl.PointToScreen(pt);
                }
                else if (focusControl is ButtonBase)//若焦点是按钮，取按钮中心点
                {
                    point = GetCenterPosition(focusControl);
                }
                else //其余情况在鼠标附近显示
                {
                    point = Control.MousePosition;
                }
            }

            //异步Show
            ThreadPool.QueueUserWorkItem(obj => new TipForm
            {
                TipText = text,
                TipIcon = icon,
                Delay = delay < 0 ? DefaultDelay : delay,
                Floating = AllowFloating,
                BasePoint = point.Value
            }.ShowDialog());//要让创建浮动窗体的线程具有消息循环，所以要用ShowDialog
        }

        /// <summary>
        /// 检测枚举值合法性并转换为Image
        /// </summary>
        private static Image CheckAndConvertTipIconValue(TipIcon tipIcon)
        {
            int i = (int)tipIcon;
            if (i < 0 || i > 3)
            {
                throw new InvalidEnumArgumentException("tipIcon", i, typeof(TipIcon));
            }
            return _icons[i];
        }

        /// <summary>
        /// 获取控件中心点的屏幕坐标
        /// </summary>
        private static Point GetCenterPosition(Component controlOrItem)
        {
            Control c = controlOrItem as Control;
            if (c != null)
            {
                return c.PointToScreen(new Point(c.Width / 2, c.Height / 2));
            }
            var item = controlOrItem as ToolStripItem;
            if (item != null)
            {
                var pos = item.Bounds.Location;
                pos.X += item.Width / 2;
                pos.Y += item.Height / 2;
                return item.Owner.PointToScreen(pos);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// 获取输入光标位置，文本框内坐标
        /// </summary>
        private static Point GetCaretPosition()
        {
            Point pt;
            NativeMethods.GetCaretPos(out pt);
            return pt;
        }

        /// <summary>
        /// 内置图标数据：√ ! X
        /// </summary>
        /// <remarks>GIF文件</remarks>
        const string DefaultIconData =
#if SmallSize
 @"R0lGODlhSAAYAMQAAOjbK9eZmejy4uVdXSUkELHetssRESmbM1XYZfPqk9XHZdbROrk1NcSZmfbx
vhbFJqeXMmtrWzawRPPkdODTaLMSEoozM8u9St/WTr+0LuDXm0vGWdy3t4nFjqETE////ywAAAAA
SAAYAAAF/+AnjmRpnmiqrmzrviPHBGzAcHApSEXrYAoHSzaYqWxFXO4j2CB4LEoiQVkRB0XaCYm9
LZ2IZy/loDjKwhQDyzaWuO1cBxyWqKRT6nFd7GpFXHxuLgUSYXVjJ1QOCRNlCXt9a25IDJODLYVh
GxsSHSoKUxMTUworlZZZqJaYLDtOnBIbKhpSGhERoRQap5esv6x/La8IDw8SEgIpAhiluBqkCsqR
wMDCJIWfJZ2dxwfTKLWNCrgKpBi8Kw3Vv9cjmp4kHYYH3oknAheNE+S5FKQX0qhYZ6mCh4MMGqCA
90RbhwMI6kk4oC0FBSkTLhIgcGHBAgoK0h2xwOCgSQvuRP8YQkavw7wn3mapcKBvAgYMFzYu6Pgx
4ECSBk0eRHniIYJunQ5sMPbgQLIVCv5h8LgAQgSqC4CYQkGwgkGvHrxWsKDQxLylTGE2/baCZoKp
VEdhzerTRAMLYr1awJuXqFmlTAMfuIciKlyPGTZmAEAVwIWtJO4a6NsgAN8Kk8kWrRe4acUUGi5M
AECaNISNEUozdizyg2QDsA1odm0BNmbZZUs8FPw5xQUMqk0rDs74wojXsWeLQA5buTzOTcGBzgCc
+CjipDOkqx0bt13uyVHsjiA9BYTq2NMzhiCCA3jnkd8r2UwYNIT7+PPr1y/SvfcUklkw3xIEtuBe
bgAKWOAFggyyEAIAOw==";
#else
 @"R0lGODlhYAAgANUAAOrcJ9LORebm5tJKShPLJLczM/z3s/XrkNfSOhS2JKaYMezeaMoREfhwcNS3
t7IREVSkWpWQZjCpPfz8+zS4RN3PZdTU1EjLWG9uaO/w7/Dke1HVYfHsx5UzM8Q8PLjYuqmmn8bl
yeO3txsbD+fck9DAUeDaVV1cHuHXMDGTOO3iSeHy4+BYWDvGTPn25O/hN5PIl8K1Pbfhu/z78nW/
fPb39lbeZ5/WpIAzM2HpcqITExGiIjV/N8S9luLbqf///yH5BAAAAAAALAAAAABgACAAAAb/wJ9w
SCwaj8ikcslsOp/QKFNUEEGpVqm2OQtUZlJqo+oUk7fHFUWmJWk0pKi4Mc4qxaw6uli7bNZRMxUG
BhUuT3gseWdIc4p6e0I0Fzk5EmxPboQHcU2Jiot2RZ+PjFs3LTk2G5aYTC6DBweFh0wFDaC5kERi
Ayy+v4oFeyEUNqsbrJdNmm8GcJ63A9PUA7s/VL/V0yymUWobNhfj4xsSTByxb7MVHNEN29xn2fG+
3lB9F+LkfstLFbMWaBAoq0KZWx4SJhzgoZuIbB4YLox4DwqNFuJaaGxxgQIMdIM0RMCAIYKGQu6a
OCjAQqHLAQUKSHSZsOKTG8YubNRIgUYT/4AHFpAkqaHghIMtPRRQuhAm06VQHSiRAQhJCAl/JCSQ
wJUChBogDQgcimHBggPtnqyEGbOt27c2hciQYMNfkRUSOu7Yu1fCVyYTAA5cMJKk2aJfEHVgC9et
UqlJqNowZlfIBAgULvDtK2FFEx+DzArFcCKCaLQ+rixVCpd1B1FG5k6mQNnVDxoUNmzewTUElxIH
BhMeMeJEiQBeFpQAo7YDa9YPdEiP+0M27evmMN2QQCNBgs0SbjgBHXSBCQQniJ9AjnxBhU4H2Uqf
L/21ErrXrydoIeHD1Q3e7eDdVh814QJwGpjAngLEKcBeeyWA5cli9FVYH2xEzPUHBS0MuP+fBClc
4KGA3B3FTAUaBICAigEocMIJCqy4ohcBwHeHczpEZyF99iGhIQUeetfCH0H6lYETByaIwJJLxvBi
DEwyaUIJtSThQAcFPBCdljl2uWV0PR7xQV4EDFhmAmWe6R0EnjlRQQULRLmkCW+cJ6eCBiUhApZa
9unnn312AJmYErRAwKFopplmAin45gQHJWgg56SUTpmSEVcyAKiWHXSwKaeDGjGmooeWemgKHzwR
mAkmoODqqygwOIICsL66Yp6j4KDpA7vuKuiVWu7KK6hJfJCCqcimUOCjJSxQ66vEEffsqyVcKoQD
umqqrbY4QJYprwxs+4CgxR6LLAEp+KT/agkmqAArANBG+yy8KARQgok/YBvuvvx2S4S+4PLLgL9I
GIvsDhDg+1kM7gLgMLwPR0BcBA8/jMLDMaQmRLYCh0vwvxx3jIMSBiMKwZGqxmDCCxW3rIIsKrTc
MgIxmKivyKGC3PHAOf8wQQ0ZZCAACObyYIEAQWdQw9ITNH2EDwzLLPXUFWc8xM37fnwE1h73/APQ
AlggNgg88NCD2EcLgLTSThtB6wsqvCD33HIDwDLLVDsc678dZO11EZl2jcTPQattuNpJK11D020T
MUEPCkQu+eSUV255DwrrS65Kum4eyRJNAy304aSXXrriTk8wOgg4gIB22okbjjbrICC9E/jnuEcR
+uiHB73076KT7rvTQQAAOw==";
#endif

        /// <summary>
        /// 浮动消息层
        /// </summary>
        private class TipForm : Form
        {
            /// <summary>
            /// 图标和文本之间的间距（像素）
            /// </summary>
            const int IconTextSpacing = 3;

            /// <summary>
            /// 是否向下浮动
            /// </summary>
            bool _floatDown;

            /// <summary>
            /// 基准点。用于指导本窗体显示位置
            /// </summary>
            public Point BasePoint { get; set; }

            /// <summary>
            /// 提示图标
            /// </summary>
            public Image TipIcon
            {
                //有零星反映访问TipIcon会抛异常，姑且看看独占后能否解决
                [MethodImpl(MethodImplOptions.Synchronized)]
                get;
                set;
            }

            string _tipText;
            /// <summary>
            /// 提示文本
            /// </summary>
            public string TipText
            {
                get { return _tipText ?? string.Empty; }
                set { _tipText = value; }
            }

            /// <summary>
            /// 停留时长（毫秒）
            /// </summary>
            [DefaultValue(500)]
            public int Delay { get; set; }

            /// <summary>
            /// 是否浮动
            /// </summary>
            [DefaultValue(true)]
            public bool Floating { get; set; }

            //显示后不激活，即不抢焦点
            protected override bool ShowWithoutActivation
            {
                get { return true; }
            }

            public TipForm()
            {
                //双缓冲。有必要
                SetStyle(ControlStyles.UserPaint, true);
                DoubleBuffered = true;

                InitializeComponent();

                Delay = 500;
                Floating = true;

                this._timer.Tick += timer_Tick;
                this.Load += TipForm_Load;
                this.Shown += TipForm_Shown;
                this.FormClosing += TipForm_FormClosing;
            }

            /// <summary>
            /// 根据图标和文字处理窗体尺寸
            /// </summary>
            private void ProcessClientSize()
            {
                Size size = Size.Empty;
                if (TipIcon != null)
                {
                    size += TipIcon.Size;
                }
                if (TipText.Length != 0)
                {
                    if (TipIcon != null)
                    {
                        size.Width += IconTextSpacing;
                    }
                    var textSize = TextRenderer.MeasureText(TipText, this.Font);
                    size.Width += textSize.Width;
                    if (size.Height < textSize.Height) { size.Height = textSize.Height; }
                }
                this.ClientSize = size + Padding.Size;
            }

            /// <summary>
            /// 根据基准点处理窗体显示位置
            /// </summary>
            private void ProcessLocation()
            {
                var p = BasePoint;
                p.X -= this.Width / 2;

                //以基准点所在屏为界
                var screen = Screen.FromPoint(BasePoint).Bounds;

                //横向处理。距离屏幕左右两边太近时的处理
                //多屏下left可能为负，所以right = width - (-left) = width + left
                int dist = 10; //至少距离边缘多少像素
                int left, right;
                if (p.X < (left = screen.Left + dist))
                {
                    p.X = left;
                }
                else if (p.X > (right = screen.Width + screen.Left - dist - this.Width))
                {
                    p.X = right;
                }

                //纵向处理。默认在上方显示
                dist = 20;//错开基准点上下20像素
                p.Y -= this.Height + dist;
                if (p.Y < screen.Top + 50)//若太靠屏幕上方，往下显示
                {
                    p.Y += this.Height + 2 * dist;
                    _floatDown = true;//动画改为下降
                }

                this.Location = p;
            }

            void TipForm_Load(object sender, EventArgs e)
            {
                //这俩顺序不能乱
                ProcessClientSize();
                ProcessLocation();

                //浮动动画。采用异步，以不阻塞透明渐变动画的进行
                if (Floating)
                {
                    new Thread(() => //用线程池偶尔会忙不过来
                    {
                        int adj = _floatDown ? 1 : -1;
                        while (this.IsHandleCreated)
                        {
                            this.BeginInvoke(new Action<object>(arg =>
                            {
                                this.Top += adj;
                                Application.DoEvents();
                            }), (object)null);

                            Thread.Sleep(30);
                        }
                    }) { IsBackground = true }.Start();
                }

                //透明渐入动画。之所以不用异步是为了在完全显示后再开始Delay的计时
                //不然如果Delay设置过低，还没等看清就渐隐了
                this.Opacity = 0;
                while (this.Opacity < 1)
                {
                    this.Opacity += 0.1;
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }

            void TipForm_Shown(object sender, EventArgs e)
            {
                //timer.Interval不能为0
                if (Delay > 0)
                {
                    _timer.Interval = Delay;
                    _timer.Start();
                }
                else
                {
                    this.Close();
                }
            }

            void timer_Tick(object sender, EventArgs e)
            {
                _timer.Stop();
                this.Close();
            }

            void TipForm_FormClosing(object sender, FormClosingEventArgs e)
            {
                //透明渐隐动画
                while (this.Opacity > 0)
                {
                    this.Opacity -= 0.1;
                    Application.DoEvents();
                    Thread.Sleep(20);
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                var clip = GetPaddedRectangle();//得到作图区域
                var g = e.Graphics;

                //画图标
                if (TipIcon != null)
                {
                    g.DrawImageUnscaled(TipIcon, clip.Location);
                }
                //画文本
                if (TipText.Length != 0)
                {
                    if (TipIcon != null)
                    {
                        clip.X += TipIcon.Width + IconTextSpacing;
                    }
                    TextRenderer.DrawText(g, TipText, this.Font, clip, this.ForeColor, TextFormatFlags.VerticalCenter);
                }
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                base.OnPaintBackground(e);

                //画边框
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, SystemColors.ControlDark, ButtonBorderStyle.Solid);
            }

            /// <summary>
            /// 获取刨去Padding的内容区
            /// </summary>
            private Rectangle GetPaddedRectangle()
            {
                Rectangle r = this.ClientRectangle;
                r.X += this.Padding.Left;
                r.Y += this.Padding.Top;
                r.Width -= this.Padding.Horizontal;
                r.Height -= this.Padding.Vertical;
                return r;
            }

            #region 设计器内容

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _timer.Dispose();//注意释放这货
                }
                base.Dispose(disposing);
            }

            private void InitializeComponent()
            {
                this._timer = new System.Windows.Forms.Timer();
                this.SuspendLayout();

                this.AutoScaleMode = AutoScaleMode.None;
                this.BackColor = Color.White;
#if SmallSize
                this.Font = SystemFonts.MessageBoxFont;
                this.Padding = new Padding(10, 5, 10, 5);
#else
                this.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12);
                this.Padding = new Padding(20, 10, 20, 10);
#endif
                this.FormBorderStyle = FormBorderStyle.None;
                this.Name = "TipForm";
                this.ShowInTaskbar = false;

                this.ResumeLayout(false);
            }

            private System.Windows.Forms.Timer _timer;

            #endregion
        }

        /// <summary>
        /// Win32 API
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("User32.dll", SetLastError = true)]
            public static extern bool GetCaretPos(out Point pt);

            [DllImport("user32.dll")]
            public static extern IntPtr GetFocus();
        }
    }
}

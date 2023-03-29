// Copyright (c) AhDung. All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AhDung.Drawing;

namespace AhDung;

//求更高效的阴影画法

/// <summary>
/// 轻便消息窗
/// </summary>
public static class MessageTip
{
    //默认字体。当样式中的Font==null时用该字体替换
    static readonly Font DefaultFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 12);

    //文本格式。用于测量和绘制
    static readonly StringFormat DefStringFormat = StringFormat.GenericTypographic;

    /// <summary>
    /// 获取或设置默认消息样式
    /// </summary>
    public static TipStyle DefaultStyle { get; set; }

    /// <summary>
    /// 获取或设置良好消息样式
    /// </summary>
    public static TipStyle OkStyle { get; set; }

    /// <summary>
    /// 获取或设置警告消息样式
    /// </summary>
    public static TipStyle WarningStyle { get; set; }

    /// <summary>
    /// 获取或设置出错消息样式
    /// </summary>
    public static TipStyle ErrorStyle { get; set; }

    /// <summary>
    /// 获取或设置全局淡入淡出时长（毫秒）。默认100
    /// </summary>
    public static int Fade { get; set; }

    /// <summary>
    /// 是否使用漂浮动画。默认true
    /// </summary>
    public static bool Floating { get; set; }

    /// <summary>
    /// 全局消息停留时长（毫秒）。默认600
    /// </summary>
    public static int Delay { get; set; }

    static MessageTip()
    {
        Fade         = 100;
        Floating     = true;
        Delay        = 600;
        DefaultStyle = TipStyle.Gray;
        OkStyle      = TipStyle.Green;
        WarningStyle = TipStyle.Orange;
        ErrorStyle   = TipStyle.Red;
    }

    /// <summary>
    /// 在指定控件附近显示良好消息
    /// </summary>
    /// <param name="controlOrItem">控件或工具栏项</param>
    /// <param name="text">消息文本</param>
    /// <param name="delay">消息停留时长(ms)。为负时使用全局时长</param>
    /// <param name="floating">是否漂浮，不指定则使用全局设置</param>
    /// <param name="centerInControl">是否在控件中央显示，不指定则自动判断</param>
    public static void ShowOk(Component controlOrItem, string text = null, int delay = -1, bool? floating = null, bool? centerInControl = null) =>
        Show(controlOrItem, text, OkStyle ?? TipStyle.Green, delay, floating, centerInControl);

    /// <summary>
    /// 显示良好消息
    /// </summary>
    /// <param name="text">消息文本</param>
    /// <param name="delay">消息停留时长(ms)。为负时使用全局时长</param>
    /// <param name="floating">是否漂浮，不指定则使用全局设置</param>
    /// <param name="point">消息窗显示位置。不指定则智能判定，当由工具栏项(ToolStripItem)弹出时，请指定该参数或使用接收控件的重载</param>
    /// <param name="centerByPoint">是否以point参数为中心进行呈现。为false则是在其附近呈现</param>
    public static void ShowOk(string text = null, int delay = -1, bool? floating = null, Point? point = null, bool centerByPoint = false) =>
        Show(text, OkStyle ?? TipStyle.Green, delay, floating, point, centerByPoint);

    /// <summary>
    /// 在指定控件附近显示警告消息
    /// </summary>
    /// <param name="controlOrItem">控件或工具栏项</param>
    /// <param name="text">消息文本</param>
    /// <param name="delay">消息停留时长(ms)。默认1秒，若要使用全局时长请设为-1</param>
    /// <param name="floating">是否漂浮。默认不漂浮。若要使用全局设置请设为null</param>
    /// <param name="centerInControl">是否在控件中央显示，不指定则自动判断</param>
    public static void ShowWarning(Component controlOrItem, string text = null, int delay = 1000, bool? floating = false, bool? centerInControl = null) =>
        Show(controlOrItem, text, WarningStyle ?? TipStyle.Orange, delay, floating, centerInControl);

    /// <summary>
    /// 显示警告消息
    /// </summary>
    /// <param name="text">消息文本</param>
    /// <param name="delay">消息停留时长(ms)。默认1秒，若要使用全局时长请设为-1</param>
    /// <param name="floating">是否漂浮。默认不漂浮。若要使用全局设置请设为null</param>
    /// <param name="point">消息窗显示位置。不指定则智能判定，当由工具栏项(ToolStripItem)弹出时，请指定该参数或使用接收控件的重载</param>
    /// <param name="centerByPoint">是否以point参数为中心进行呈现。为false则是在其附近呈现</param>
    public static void ShowWarning(string text = null, int delay = 1000, bool? floating = false, Point? point = null, bool centerByPoint = false) =>
        Show(text, WarningStyle ?? TipStyle.Orange, delay, floating, point, centerByPoint);

    /// <summary>
    /// 在指定控件附近显示出错消息
    /// </summary>
    /// <param name="controlOrItem">控件或工具栏项</param>
    /// <param name="text">消息文本</param>
    /// <param name="delay">消息停留时长(ms)。默认1秒，若要使用全局时长请设为-1</param>
    /// <param name="floating">是否漂浮。默认不漂浮。若要使用全局设置请设为null</param>
    /// <param name="centerInControl">是否在控件中央显示，不指定则自动判断</param>
    public static void ShowError(Component controlOrItem, string text = null, int delay = 1000, bool? floating = false, bool? centerInControl = null) =>
        Show(controlOrItem, text, ErrorStyle ?? TipStyle.Red, delay, floating, centerInControl);

    /// <summary>
    /// 显示出错消息
    /// </summary>
    /// <param name="text">消息文本</param>
    /// <param name="delay">消息停留时长(ms)。默认1秒，若要使用全局时长请设为-1</param>
    /// <param name="floating">是否漂浮。默认不漂浮。若要使用全局设置请设为null</param>
    /// <param name="point">消息窗显示位置。不指定则智能判定，当由工具栏项(ToolStripItem)弹出时，请指定该参数或使用接收控件的重载</param>
    /// <param name="centerByPoint">是否以point参数为中心进行呈现。为false则是在其附近呈现</param>
    public static void ShowError(string text = null, int delay = 1000, bool? floating = false, Point? point = null, bool centerByPoint = false) =>
        Show(text, ErrorStyle ?? TipStyle.Red, delay, floating, point, centerByPoint);

    /// <summary>
    /// 在指定控件附近显示消息
    /// </summary>
    /// <param name="controlOrItem">控件或工具栏项</param>
    /// <param name="text">消息文本</param>
    /// <param name="style">消息样式。不指定则使用默认样式</param>
    /// <param name="delay">消息停留时长(ms)。为负时使用全局时长</param>
    /// <param name="floating">是否漂浮，不指定则使用全局设置</param>
    /// <param name="centerInControl">是否在控件中央显示，不指定则自动判断</param>
    public static void Show(Component controlOrItem, string text, TipStyle style = null, int delay = -1, bool? floating = null, bool? centerInControl = null)
    {
        if (controlOrItem == null)
        {
            throw new ArgumentNullException(nameof(controlOrItem));
        }

        Show(text, style, delay, floating, GetCenterPosition(controlOrItem), centerInControl ?? IsContainerLike(controlOrItem));
    }

    /// <summary>
    /// 显示消息
    /// </summary>
    /// <param name="text">消息文本</param>
    /// <param name="style">消息样式。不指定则使用默认样式</param>
    /// <param name="delay">消息停留时长(ms)。为负时使用全局时长</param>
    /// <param name="floating">是否漂浮，不指定则使用全局设置</param>
    /// <param name="point">消息窗显示位置。不指定则智能判定，当由工具栏项(ToolStripItem)弹出时，请指定该参数或使用接收控件的重载</param>
    /// <param name="centerByPoint">是否以point参数为中心进行呈现。为false则是在其附近呈现</param>
    public static void Show(string text, TipStyle style = null, int delay = -1, bool? floating = null, Point? point = null, bool centerByPoint = false)
    {
        var basePoint = point ?? DetermineHotPoint();

        new Thread(_ =>
        {
            LayeredWindow layer = null;
            try
            {
                layer = new LayeredWindow
                {
                    BackgroundImage = CreateTipImage(text ?? string.Empty, style ?? DefaultStyle ?? TipStyle.Gray, out Rectangle contentBounds),
                    Alpha           = 0,
                    Location        = GetLocation(contentBounds, basePoint, centerByPoint, out var floatDown),
                    MouseThrough    = true,
                    TopMost         = true,
                    Tag             = new object[] { delay < 0 ? Delay : delay, floating ?? Floating, floatDown }
                };
                layer.Showing += Layer_Showing;
                layer.Closing += Layer_Closing;
                layer.Show();
            }
            finally
            {
                if (layer != null)
                {
                    layer.Showing -= Layer_Showing;
                    layer.Closing -= Layer_Closing;
                    layer.Dispose();
                }
            }
        }) { IsBackground = true, Name = "T_Showing" }.Start();
    }

    static void Layer_Showing(object sender, EventArgs e)
    {
        var layer = (LayeredWindow)sender;
        var args = layer.Tag as object[];
        var delay = (int)args[0];
        var floating = (bool)args[1];
        var floatDown = (bool)args[2];

        if (floating)
        {
            //另起线程浮动窗体
            new Thread(_ =>
            {
                int adj = floatDown ? 1 : -1;

                while (layer.Visible) //layer.IsDisposed有lock，不适合此循环
                {
                    layer.Top += adj;
                    Thread.Sleep(30);
                }
            }) { IsBackground = true, Name = "T_Floating" }.Start();
        }

        FadeEffect(layer, true);
        Thread.Sleep(delay < 0 ? 0 : delay);
        layer.Close();
    }

    static void Layer_Closing(object sender, CancelEventArgs e) =>
        FadeEffect((LayeredWindow)sender, false);

    /// <summary>
    /// 淡入淡出处理
    /// </summary>
    static void FadeEffect(LayeredWindow window, bool fadeIn)
    {
        byte target = fadeIn ? byte.MaxValue : byte.MinValue;
        const int Updateinterval = 10; //动画更新间隔（毫秒）
        int step = Fade < Updateinterval ? 0 : (Fade / Updateinterval);

        for (int i = 1; i <= step; i++)
        {
            Thread.Sleep(Updateinterval);

            if (i == step)
            {
                break;
            }

            var tmp = (double)(fadeIn ? i : (step - i));
            window.Alpha = (byte)(tmp / step * 255);
        }

        window.Alpha = target;
    }

    /// <summary>
    /// 判定活动点
    /// </summary>
    static Point DetermineHotPoint()
    {
        var point = Control.MousePosition;

        var focusControl = Control.FromHandle(GetFocus());
        if (focusControl is TextBoxBase) //若焦点是文本框，取光标位置
        {
            var pt = GetCaretPosition();
            pt.Y  += focusControl.Font.Height / 2;
            point =  focusControl.PointToScreen(pt);
        }
        else if (focusControl is ButtonBase) //若焦点是按钮，取按钮中心点
        {
            point = GetCenterPosition(focusControl);
        }

        return point;
    }

    /// <summary>
    /// 创建消息窗图像，同时输出内容区，用于外部定位
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] //避免争用style中的图标画笔等资源，否则在同时调用时容易引发资源占用异常
    static Bitmap CreateTipImage(string text, TipStyle style, out Rectangle contentBounds)
    {
        var size = Size.Empty;
        var iconBounds = Rectangle.Empty;
        var textBounds = Rectangle.Empty;

        if (style.Icon != null)
        {
            size            = style.Icon.Size;
            iconBounds.Size = size;
            textBounds.X    = size.Width;
        }

        if (text.Length != 0)
        {
            if (style.Icon != null)
            {
                size.Width   += style.IconSpacing;
                textBounds.X += style.IconSpacing;
            }

            textBounds.Size =  Size.Truncate(GraphicsUtils.MeasureString(text, style.TextFont ?? DefaultFont, 0, DefStringFormat));
            size.Width      += textBounds.Width;

            if (size.Height < textBounds.Height)
            {
                size.Height = textBounds.Height;
            }
            else if (size.Height > textBounds.Height) //若文字没有图标高，令文字与图标垂直居中，否则与图标平齐
            {
                textBounds.Y += (size.Height - textBounds.Height) / 2;
            }

            textBounds.Offset(style.TextOffset);
        }

        size += style.Padding.Size;
        iconBounds.Offset(style.Padding.Left, style.Padding.Top);
        textBounds.Offset(style.Padding.Left, style.Padding.Top);

        contentBounds = new Rectangle(Point.Empty, size);
        var fullBounds = GraphicsUtils.GetBounds(contentBounds, style.Border, style.ShadowRadius, style.ShadowOffset.X, style.ShadowOffset.Y);
        contentBounds.Offset(-fullBounds.X, -fullBounds.Y);
        iconBounds.Offset(-fullBounds.X, -fullBounds.Y);
        textBounds.Offset(-fullBounds.X, -fullBounds.Y);

        var bmp = new Bitmap(fullBounds.Width, fullBounds.Height);

        Graphics g = null;
        Brush backBrush = null;
        Brush textBrush = null;
        try
        {
            g                 = Graphics.FromImage(bmp);
            g.SmoothingMode   = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            backBrush = (style.BackBrush ?? (_ => new SolidBrush(style.BackColor)))(contentBounds);
            GraphicsUtils.DrawRectangle(g, contentBounds,
                backBrush,
                style.Border,
                style.CornerRadius,
                style.ShadowColor,
                style.ShadowRadius,
                style.ShadowOffset.X,
                style.ShadowOffset.Y);

            if (style.Icon != null)
            {
                //DEBUG: g.DrawRectangle(new Border(Color.Red) { Width = 1, Direction = Direction.Inner }.Pen, iconBounds);
                g.DrawImageUnscaled(style.Icon, iconBounds.Location);
            }

            if (text.Length != 0)
            {
                textBrush = new SolidBrush(style.TextColor);
                //DEBUG: g.DrawRectangle(new Border(Color.Red){ Width=1, Direction= Direction.Inner}.Pen, textBounds);
                g.DrawString(text, style.TextFont ?? DefaultFont, textBrush, textBounds.Location, DefStringFormat);
            }

            g.Flush(FlushIntention.Sync);
            return bmp;
        }
        finally
        {
            g?.Dispose();
            backBrush?.Dispose();
            textBrush?.Dispose();
        }
    }

    /// <summary>
    /// 根据基准点处理窗体显示位置
    /// </summary>
    /// <param name="contentBounds">内容区。依据该区域处理定位，而不是根据整个消息窗图像，因为阴影也许偏移很大</param>
    /// <param name="basePoint">定位参考点</param>
    /// <param name="centerByBasePoint">是否以参考点为中心呈现。false则是在参考点附近呈现</param>
    /// <param name="floatDown">指示是否应当向下浮动（当太靠近屏幕顶部时）。默认是向上</param>
    static Point GetLocation(Rectangle contentBounds, Point basePoint, bool centerByBasePoint, out bool floatDown)
    {
        //以基准点所在屏为界
        var screen = Screen.FromPoint(basePoint).Bounds;

        var p = basePoint;
        p.X -= contentBounds.Width / 2;

        //横向处理。距离屏幕左右两边太近时的处理
        //多屏下left可能为负，所以right = width - (-left) = width + left
        int spacing = 10; //至少距离边缘多少像素
        int left, right;
        if (p.X < (left = screen.Left + spacing))
        {
            p.X = left;
        }
        else if (p.X > (right = screen.Width + screen.Left - spacing - contentBounds.Width))
        {
            p.X = right;
        }

        //纵向处理
        if (centerByBasePoint)
        {
            p.Y -= contentBounds.Height / 2;
        }
        else
        {
            spacing =  20; //错开基准点上下20像素
            p.Y     -= contentBounds.Height + spacing;
        }

        floatDown = false;
        if (p.Y < screen.Top + 50) //若太靠屏幕顶部
        {
            if (!centerByBasePoint)
            {
                p.Y += contentBounds.Height + 2 * spacing; //在下方错开
            }

            floatDown = true; //动画改为下降
        }

        p.Offset(-contentBounds.X, -contentBounds.Y);
        return p;
    }

    /// <summary>
    /// 获取控件中心点的屏幕坐标
    /// </summary>
    static Point GetCenterPosition(Component controlOrItem)
    {
        if (controlOrItem is Control c)
        {
            var size = c.ClientSize;
            return c.PointToScreen(new Point(size.Width / 2, size.Height / 2));
        }

        if (controlOrItem is ToolStripItem item)
        {
            var pos = item.Bounds.Location;
            pos.X += item.Width / 2;
            pos.Y += item.Height / 2;
            return item.Owner.PointToScreen(pos);
        }

        throw new ArgumentException("参数只能是Control或ToolStripItem！");
    }

    /// <summary>
    /// 判断控件看起来是否像容器（占一定面积那种）
    /// </summary>
    static bool IsContainerLike(Component controlOrItem)
    {
        if (controlOrItem is ContainerControl
            or GroupBox
            or Panel
            or TabControl
#if !NET
            or DataGrid
#endif
            or DataGridView
            or ListBox
            or ListView)
        {
            return true;
        }

        if (controlOrItem is TextBox { Multiline: true })
        {
            return true;
        }

        if (controlOrItem is RichTextBox { Multiline: true })
        {
            return true;
        }

        return false;
    }

    #region Win32 API

    /// <summary>
    /// 获取输入光标位置，文本框内坐标
    /// </summary>
    static Point GetCaretPosition()
    {
        GetCaretPos(out Point pt);
        return pt;
    }

    [DllImport("User32.dll", SetLastError = true)]
    static extern bool GetCaretPos(out Point pt);

    [DllImport("user32.dll")]
    static extern IntPtr GetFocus();

    #endregion
}
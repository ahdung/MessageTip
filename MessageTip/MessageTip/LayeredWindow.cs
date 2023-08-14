using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace AhDung;

/// <summary>
/// 简易层窗体
/// </summary>
[SuppressUnmanagedCodeSecurity]
public class LayeredWindow : IDisposable
{
    const  string          ClassName     = "AhDungLayeredWindow";
    const  int             DefaultWidth  = 200;
    const  int             DefaultHeight = 200;
    static WndProcDelegate _wndProc;

    readonly Color  _backgroundColor = SystemColors.Control;
    int             _top;
    int             _left;
    Image           _backgroundImage;

    IntPtr _defaultHBmp;
    IntPtr _dcMemory;
    IntPtr _hBmp;
    IntPtr _oldObj;

    IntPtr _activeWindow; //用于模式显示时，记录并disable原窗体，然后在本类关闭后enable它
    bool   _continueLoop = true;
    short  _wndClass;
    IntPtr _hWnd;

    BLENDFUNCTION _blend = new()
    {
        BlendOp             = 0, //AC_SRC_OVER
        BlendFlags          = 0,
        SourceConstantAlpha = 255, //透明度
        AlphaFormat         = 1    //AC_SRC_ALPHA
    };

    bool _visible;
    bool _layoutSuspended;

    /// <summary>
    /// 窗体显示时
    /// </summary>
    public event EventHandler Showing;

    /// <summary>
    /// 窗体关闭时
    /// </summary>
    public event CancelEventHandler Closing;

    static IntPtr _hInstance;

    static IntPtr HInstance
    {
        get
        {
            if (_hInstance == IntPtr.Zero)
                _hInstance = Marshal.GetHINSTANCE(Assembly.GetEntryAssembly()!.ManifestModule);

            return _hInstance;
        }
    }

    /// <summary>
    /// 获取窗体位置。内部用
    /// </summary>
    PointOrSize LocationInternal => new(_left, _top);

    /// <summary>
    /// 获取窗体尺寸。内部用
    /// </summary>
    PointOrSize SizeInternal => new(Width, Height);

    /// <summary>
    /// 窗体句柄
    /// </summary>
    public IntPtr Handle => _hWnd;

    /// <summary>
    /// 获取或设置窗体可见性
    /// </summary>
    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visible == value)
                return;

            _visible = value; //需在Update前设置，不然Update中检测到未显示也不会更新

            if (value)
            {
                TryUpdate();
                ShowWindow(_hWnd, Activation ? 5 /*SW_SHOW*/ : 8 /*SW_SHOWNA*/);
            }
            else
                ShowWindow(_hWnd, 0 /*SW_HIDE*/);
        }
    }

    /// <summary>
    /// 获取或设置左边缘坐标
    /// </summary>
    public int Left
    {
        get => _left;
        set
        {
            if (_left != value)
            {
                _left = value;
                TryUpdate();
            }
        }
    }

    /// <summary>
    /// 获取或设置上边缘坐标
    /// </summary>
    public int Top
    {
        get => _top;
        set
        {
            if (_top != value)
            {
                _top = value;
                TryUpdate();
            }
        }
    }

    /// <summary>
    /// 获取或设置定位
    /// </summary>
    public Point Location
    {
        get => new(_left, _top);
        set
        {
            if (_left != value.X || _top != value.Y)
            {
                _left = value.X;
                _top  = value.Y;
                TryUpdate();
            }
        }
    }

    /// <summary>
    /// 获取窗体尺寸。尺寸与<see cref="BackgroundImage"/>匹配，只能通过修改背景图修改
    /// </summary>
    public Size Size => new(Width, Height);

    /// <summary>
    /// 获取窗体矩形
    /// </summary>
    public Rectangle ClientRectangle => new(Point.Empty, Size);

    /// <summary>
    /// 获取窗体在桌面上的边界
    /// </summary>
    public Rectangle DesktopBounds => new(Location, Size);

    /// <summary>
    /// 获取窗体宽度
    /// </summary>
    public int Width { get; private set; } = DefaultWidth;

    /// <summary>
    /// 获取窗体高度
    /// </summary>
    public int Height { get; private set; } = DefaultHeight;

    /// <summary>
    /// 获取或设置窗体透明度。0=完全透明；1=不透明
    /// </summary>
    /// <remarks>系<see cref="Alpha"/>的包装，建议优先用Alpha。</remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public float Opacity
    {
        get => Alpha / 255f;
        set
        {
            if (value is < 0 or > 1)
                throw new ArgumentOutOfRangeException();

            Alpha = (byte)(value * 255);
        }
    }

    /// <summary>
    /// 获取或设置窗体透明度。0=完全透明；255=不透明
    /// </summary>
    public byte Alpha
    {
        get => _blend.SourceConstantAlpha;
        set
        {
            if (_blend.SourceConstantAlpha != value)
            {
                _blend.SourceConstantAlpha = value;
                TryUpdate();
            }
        }
    }

    /// <summary>
    /// 获取或设置名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 指示窗体是否以模式状态打开
    /// </summary>
    public bool IsModal { get; private set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool TopMost { get; set; }

    /// <summary>
    /// 是否在显示后激活本窗体。模式显示时强制为true
    /// </summary>
    public bool Activation { get; set; }

    /// <summary>
    /// 是否在任务栏显示
    /// </summary>
    public bool ShowInTaskbar { get; set; }

    /// <summary>
    /// 是否让鼠标事件穿透本窗体
    /// </summary>
    public bool MouseThrough { get; set; }

    /// <summary>
    /// 指示窗体是否已释放
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// 获取或设置自定对象
    /// </summary>
    public object Tag { get; set; }

    /// <summary>
    /// 获取或设置背景图片。设置图片后窗体尺寸将调整为图片大小
    /// </summary>
    public Image BackgroundImage
    {
        get => _backgroundImage;
        set
        {
            if (_backgroundImage == value)
                return;

            if (value is not null and not Bitmap)
                throw new ArgumentException("目前只接受位图！");

            //清理上个图的资源
            if (_oldObj != IntPtr.Zero)
                SelectObject(_dcMemory, _oldObj);

            if (_hBmp != IntPtr.Zero)
            {
                DeleteObject(_hBmp);
                _hBmp = IntPtr.Zero;
            }

            if (value == null)
            {
                Width   = DefaultWidth;
                Height  = DefaultHeight;
                _oldObj = SelectObject(_dcMemory, _defaultHBmp);
            }
            else
            {
                Width   = value.Width;
                Height  = value.Height;
                _hBmp   = ((Bitmap)value).GetHbitmap(Color.Empty);
                _oldObj = SelectObject(_dcMemory, _hBmp);
            }

            _backgroundImage = value;
            TryUpdate();
        }
    }

    /// <summary>
    /// 创建层窗体
    /// </summary>
    public LayeredWindow()
    {
        RegisterWindowClass();
        _dcMemory = CreateCompatibleDC(IntPtr.Zero);

        using var bmp = new Bitmap(DefaultWidth, DefaultHeight);
        using var g = Graphics.FromImage(bmp);

        g.Clear(_backgroundColor);
        g.Flush(System.Drawing.Drawing2D.FlushIntention.Flush);
        _defaultHBmp = bmp.GetHbitmap();
        _oldObj      = SelectObject(_dcMemory, _defaultHBmp);
    }

    /// <summary>
    /// 注册私有窗口类
    /// </summary>
    void RegisterWindowClass()
    {
        _wndProc ??= WndProc;

        var wc = new WNDCLASS
        {
            hInstance     = HInstance,
            lpszClassName = ClassName,
            lpfnWndProc   = _wndProc,
            hCursor       = LoadCursor(IntPtr.Zero, 32512 /*IDC_ARROW*/),
        };

        _wndClass = RegisterClass(wc);

        if (_wndClass == 0 && Marshal.GetLastWin32Error() != 0x582) //ERROR_CLASS_ALREADY_EXISTS
            throw new Win32Exception();
    }

    /// <summary>
    /// 创建窗口
    /// </summary>
    void CreateWindow()
    {
        var exStyle = 0x80000; //WS_EX_LAYERED

        if (TopMost)
            exStyle |= 0x8; //WS_EX_TOPMOST

        if (!Activation)
            exStyle |= 0x08000000; //WS_EX_NOACTIVATE

        if (MouseThrough)
            exStyle |= 0x20; //WS_EX_TRANSPARENT

        if (ShowInTaskbar)
            exStyle |= 0x40000; //WS_EX_APPWINDOW

        var style = unchecked((int)0x80000000) //WS_POPUP。不能加WS_VISIBLE，会抢焦点，改用ShowWindow显示
                    | 0x80000;                 //WS_SYSMENU

        _hWnd = CreateWindowEx(exStyle, ClassName, null, style,
            0, 0, 0, 0, //坐标尺寸全由UpdateLayeredWindow接管，这里无所谓
            IntPtr.Zero, IntPtr.Zero, HInstance, IntPtr.Zero);

        if (_hWnd == IntPtr.Zero)
            throw new Win32Exception();
    }

    int DoMessageLoop()
    {
        var m = new MSG();
        int result;

        while (_continueLoop && (result = GetMessage(ref m, IntPtr.Zero, 0, 0)) != 0)
        {
            if (result == -1)
                return Marshal.GetLastWin32Error();

            TranslateMessage(ref m);
            DispatchMessage(ref m);
        }

        return 0;
    }

    protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
        //Debug.WriteLine((hWnd == _hWnd) + "：0x" + Convert.ToString(msg, 16), "WndProc");

        switch (msg)
        {
            case 0x10: //WM_CLOSE
                Close();
                break;
            case 0x2: //WM_DESTROY
                EnableWindow(_activeWindow, true);
                _continueLoop = false;
                break;
        }

        return DefWndProc(hWnd, msg, wParam, lParam);
    }

    protected virtual IntPtr DefWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam) =>
        DefWindowProc(hWnd, msg, wParam, lParam);

    void ShowCore(bool modal)
    {
        if (IsDisposed)
            throw new ObjectDisposedException(Name ?? string.Empty);

        if (_visible)
            return;

        if (modal)
        {
            IsModal       = true;
            Activation    = true;
            _activeWindow = GetActiveWindow();
            EnableWindow(_activeWindow, false);
        }

        CreateWindow();
        ShowWindow(_hWnd, Activation ? 5 /*SW_SHOW*/ : 8 /*SW_SHOWNA*/);
        _visible = true;

        OnShowing(EventArgs.Empty);

        //Showing事件中也许会关闭窗体
        if (IsDisposed)
            return;

        TryUpdate();

        if (modal)
        {
            var result = DoMessageLoop();
            SetActiveWindow(_activeWindow);
            if (result != 0)
                throw new Win32Exception(result);
        }
    }

    /// <summary>
    /// 显示窗体
    /// </summary>
    public void Show() => ShowCore(false);

    /// <summary>
    /// 显示模式窗体
    /// </summary>
    public void ShowDialog() => ShowCore(true);

    /// <summary>
    /// 隐藏窗体
    /// </summary>
    public void Hide() => Visible = false;

    /// <summary>
    /// 挂起更新
    /// </summary>
    public void SuspendLayout() => _layoutSuspended = true;

    /// <summary>
    /// 取消挂起状态并立即执行一次更新
    /// </summary>
    public void ResumeLayout() => ResumeLayout(true);

    /// <summary>
    /// 取消挂起状态
    /// </summary>
    /// <param name="performLayout">是否立即执行一次更新</param>
    public void ResumeLayout(bool performLayout)
    {
        _layoutSuspended = false;
        if (performLayout)
            Update();
    }

    protected virtual void OnShowing(EventArgs e) => Showing?.Invoke(this, e);

    protected virtual void OnClosing(CancelEventArgs e) => Closing?.Invoke(this, e);

    void TryUpdate()
    {
        if (!_layoutSuspended)
            Update();
    }

    /// <summary>
    /// 更新窗体
    /// </summary>
    /// <exception cref="Win32Exception"/>
    public virtual void Update()
    {
        if (!_visible)
            return;

        //后续更新其实在nt6开启桌面主题的情况下，一干参数可以为null，
        //但是为了兼容其他情况，还是都指定
        if (!UpdateLayeredWindow(_hWnd,
                IntPtr.Zero, LocationInternal, SizeInternal, //注意这个尺寸只能小于等于图片尺寸，否则不会显示
                _dcMemory, PointOrSize.Empty,
                0, ref _blend, 2 /*ULW_ALPHA*/))
        {
            //忽略窗体句柄无效ERROR_INVALID_WINDOW_HANDLE
            if (Marshal.GetLastWin32Error() is { } errorCode && errorCode != 0x578)
                throw new Win32Exception(errorCode);
        }
    }

    /// <summary>
    /// 关闭并销毁窗体。须与Show系方法处于同一线程
    /// </summary>
    public void Close()
    {
        var e = new CancelEventArgs();
        OnClosing(e);
        if (!e.Cancel)
        {
            _visible = false;
            Dispose();
        }
    }

    /// <summary>
    /// 释放窗体。须与Show系方法处于同一线程
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        Tag = null;

        //销毁窗体
        DestroyWindow(_hWnd);
        _hWnd = IntPtr.Zero;

        //注销窗口类
        //窗口类是共用的，每个实例都尝试注册和注销
        //实际效果就是先开的注册，后关的注销
        if (_wndClass != 0)
        {
            if (UnregisterClass(ClassName, HInstance))
                _wndProc = null; //只有注销成功时才解绑窗口过程

            _wndClass = 0;
        }

        if (_oldObj != IntPtr.Zero)
            SelectObject(_dcMemory, _oldObj);

        DeleteDC(_dcMemory);

        if (_hBmp != IntPtr.Zero)
            DeleteObject(_hBmp);

        if (_defaultHBmp != IntPtr.Zero)
            DeleteObject(_defaultHBmp);

        _oldObj      = IntPtr.Zero;
        _dcMemory    = IntPtr.Zero;
        _hBmp        = IntPtr.Zero;
        _defaultHBmp = IntPtr.Zero;

        IsDisposed = true;
    }

    ~LayeredWindow()
    {
        Dispose(false);
    }

    //窗口过程委托
    delegate IntPtr WndProcDelegate(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    #region Win32 API

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetActiveWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr LoadCursor(IntPtr hInstance, int iconId);

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

    [DllImport("user32.dll")]
    static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern short RegisterClass(WNDCLASS wc);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetMessage(ref MSG msg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);

    [DllImport("user32.dll")]
    static extern IntPtr DispatchMessage(ref MSG msg);

    [DllImport("user32.dll")]
    static extern bool TranslateMessage(ref MSG msg);

    [DllImport("gdi32.dll")]
    static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll", SetLastError = true)]
    static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, PointOrSize pptDst, PointOrSize pSizeDst, IntPtr hdcSrc, PointOrSize pptSrc, int crKey, ref BLENDFUNCTION pBlend, int dwFlags);

    [DllImport("gdi32.dll", SetLastError = true)]
    static extern IntPtr CreateCompatibleDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    static extern bool DeleteDC(IntPtr hdc);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool DestroyWindow(IntPtr hWnd);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    class WNDCLASS
    {
        public int             style;
        public WndProcDelegate lpfnWndProc;
        public int             cbClsExtra;
        public int             cbWndExtra;
        public IntPtr          hInstance;
        public IntPtr          hIcon;
        public IntPtr          hCursor;
        public IntPtr          hbrBackground;
        public string          lpszMenuName;
        public string          lpszClassName;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MSG
    {
        public IntPtr HWnd;
        public int    Message;
        public IntPtr WParam;
        public IntPtr LParam;
        public int    Time;
        public int    X;
        public int    Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    class PointOrSize
    {
        public int XOrWidth, YOrHeight;

        public static readonly PointOrSize Empty = new();

        public PointOrSize()
        {
            XOrWidth  = 0;
            YOrHeight = 0;
        }

        public PointOrSize(int xOrWidth, int yOrHeight)
        {
            XOrWidth  = xOrWidth;
            YOrHeight = yOrHeight;
        }
    }

    #endregion
}
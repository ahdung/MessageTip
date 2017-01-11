using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace AhDung
{
    /// <summary>
    /// 简易层窗体
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public class LayeredWindow : IDisposable
    {
        const string ClassName = "AhDungLayeredWindow";

        static WndProcDelegate _wndProc;

        readonly PointOrSize _size;
        readonly IntPtr _dcMemory;
        readonly IntPtr _hBmp;

        //用于模式显示时，记录并disable原窗体，然后在本类关闭后enable它
        IntPtr _activeWindow;
        bool _continueLoop = true;
        short _wndClass;
        IntPtr _hWnd;
        BLENDFUNCTION _blend = new BLENDFUNCTION
        {
            BlendOp = 0,               //AC_SRC_OVER
            BlendFlags = 0,
            SourceConstantAlpha = 255, //透明度
            AlphaFormat = 1            //AC_SRC_ALPHA
        };

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
                {
                    _hInstance = Marshal.GetHINSTANCE(Assembly.GetEntryAssembly().ManifestModule);
                }
                return _hInstance;
            }
        }

        /// <summary>
        /// 获取窗体位置。内部用
        /// </summary>
        PointOrSize LocationInternal
        {
            get { return new PointOrSize(_left, _top); }
        }

        /// <summary>
        /// 窗体句柄
        /// </summary>
        public IntPtr Handle { get { return _hWnd; } }

        /// <summary>
        /// 指示窗体是否已显示
        /// </summary>
        public bool Visible { get; private set; }

        int _left;
        /// <summary>
        /// 获取或设置左边缘坐标
        /// </summary>
        public int Left
        {
            get { return _left; }
            set
            {
                if (_left == value) { return; }
                _left = value;
                Update();
            }
        }

        int _top;
        /// <summary>
        /// 获取或设置上边缘坐标
        /// </summary>
        public int Top
        {
            get { return _top; }
            set
            {
                if (_top == value) { return; }
                _top = value;
                Update();
            }
        }

        /// <summary>
        /// 获取或设置定位
        /// </summary>
        public Point Location
        {
            get { return new Point(_left, _top); }
            set
            {
                if (_left == value.X && _top == value.Y) { return; }
                _left = value.X;
                _top = value.Y;
                Update();
            }
        }

        /// <summary>
        /// 获取窗体宽度
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// 获取窗体高度
        /// </summary>
        public int Height { get; private set; }

        float _opacity;
        /// <summary>
        /// 获取或设置窗体透明度（建议优先用Alpha）。0=完全透明；1=不透明
        /// </summary>
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                if (_opacity.Equals(value)) { return; }
                if (value < 0) { _opacity = 0; }
                else if (value > 1) { _opacity = 1; }
                else { _opacity = value; }
                _blend.SourceConstantAlpha = (byte)(_opacity * 255);
                Update();
            }
        }

        /// <summary>
        /// 获取或设置窗体透明度。0=完全透明；255=不透明
        /// </summary>
        public byte Alpha
        {
            get { return _blend.SourceConstantAlpha; }
            set
            {
                if (_blend.SourceConstantAlpha == value) { return; }
                _blend.SourceConstantAlpha = value;
                Update();
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
        public bool IsDisposed
        {
            get { lock (this) { return _disposed; } }
        }

        /// <summary>
        /// 获取或设置自定对象
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 创建层窗体
        /// </summary>
        /// <param name="bmp">窗体图像</param>
        /// <param name="keepBmp">是否保留图像，为false会销毁图像</param>
        public LayeredWindow(Bitmap bmp, bool keepBmp = false)
        {
            try
            {
                RegisterWindowClass();

                //初始化绘制相关
                _dcMemory = CreateCompatibleDC(IntPtr.Zero);
                _hBmp = bmp.GetHbitmap(Color.Empty);
                SelectObject(_dcMemory, _hBmp);

                Width = bmp.Width;
                Height = bmp.Height;
                _size = new PointOrSize(Width, Height);
            }
            finally
            {
                if (!keepBmp)
                {
                    bmp.Dispose();
                }
            }
        }

        /// <summary>
        /// 注册私有窗口类
        /// </summary>
        private void RegisterWindowClass()
        {
            if (_wndProc == null)
            {
                _wndProc = WndProc;
            }

            var wc = new WNDCLASS
            {
                hInstance = HInstance,
                lpszClassName = ClassName,
                lpfnWndProc = _wndProc,
                hCursor = LoadCursor(IntPtr.Zero, 32512/*IDC_ARROW*/),
            };

            _wndClass = RegisterClass(wc);

            if (_wndClass == 0)
            {
                //ERROR_CLASS_ALREADY_EXISTS
                if (Marshal.GetLastWin32Error() != 0x582)
                {
                    throw new Win32Exception();
                }
            }
        }

        /// <summary>
        /// 创建窗口
        /// </summary>
        private void CreateWindow()
        {
            int exStyle = 0x80000;                      //WS_EX_LAYERED
            if (TopMost) { exStyle |= 0x8; }            //WS_EX_TOPMOST
            if (!Activation) { exStyle |= 0x08000000; } //WS_EX_NOACTIVATE
            if (MouseThrough) { exStyle |= 0x20; }      //WS_EX_TRANSPARENT
            if (ShowInTaskbar) { exStyle |= 0x40000; }  //WS_EX_APPWINDOW

            int style = unchecked((int)0x80000000)      //WS_POPUP。不能加WS_VISIBLE，会抢焦点，改用ShowWindow显示
                | 0x80000;                              //WS_SYSMENU

            _hWnd = CreateWindowEx(exStyle, ClassName, null, style,
                0, 0, 0, 0, //坐标尺寸全由UpdateLayeredWindow接管，这里无所谓
                IntPtr.Zero, IntPtr.Zero, HInstance, IntPtr.Zero);

            if (_hWnd == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            Debug.WriteLineIf(_hWnd == IntPtr.Zero, "Failed", "CreateWindowEx");
        }

        private int RunMessageLoop()
        {
            var m = new MSG();
            int result;

            while (_continueLoop && (result = GetMessage(ref m, IntPtr.Zero, 0, 0)) != 0)
            {
                if (result == -1)
                {
                    return Marshal.GetLastWin32Error();
                }
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
                case 0x10://WM_CLOSE
                    Close();
                    break;
                case 0x2://WM_DESTROY
                    EnableWindow(_activeWindow, true);
                    _continueLoop = false;
                    break;
                default: break;
            }
            return DefWndProc(hWnd, msg, wParam, lParam);
        }

        protected virtual IntPtr DefWndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private void ShowCore(bool modal)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(Name ?? string.Empty);
            }
            lock (this)
            {
                if (Visible) { return; }
                if (modal)
                {
                    IsModal = true;
                    Activation = true;
                    _activeWindow = GetActiveWindow();
                    EnableWindow(_activeWindow, false);
                }
                CreateWindow();
                ShowWindow(_hWnd, Activation ? 5/*SW_SHOW*/: 8/*SW_SHOWNA*/);
                Visible = true;
            }
            OnShowing(EventArgs.Empty);
            if (IsDisposed) { return; }//允许Showing事件中关闭窗体
            Update();
            if (modal)
            {
                var result = RunMessageLoop();
                SetActiveWindow(_activeWindow);
                if (result != 0)
                {
                    throw new Win32Exception(result);
                }
            }
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        public void Show()
        {
            ShowCore(false);
        }

        /// <summary>
        /// 显示模式窗体
        /// </summary>
        public void ShowDialog()
        {
            ShowCore(true);
        }

        protected virtual void OnShowing(EventArgs e)
        {
            var handle = Showing;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            var handle = Closing;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        /// <summary>
        /// 更新窗体
        /// </summary>
        protected virtual void Update()
        {
            if (!Visible) { return; }

            //后续更新其实在nt6开启桌面主题的情况下，一干参数可以为null，
            //但是为了兼容其他情况，还是都指定
            if (!UpdateLayeredWindow(_hWnd,
                IntPtr.Zero, LocationInternal, _size,
                _dcMemory, PointOrSize.Empty,
                0, _blend, 2/*ULW_ALPHA*/))
            {
                //忽略窗体句柄无效ERROR_INVALID_WINDOW_HANDLE
                if (Marshal.GetLastWin32Error() == 0x578) { return; }

                throw new Win32Exception();
            }
        }

        /// <summary>
        /// 关闭并销毁窗体
        /// </summary>
        public void Close()
        {
            var e = new CancelEventArgs();
            OnClosing(e);
            if (!e.Cancel)
            {
                Visible = false;
                this.Dispose();
            }
        }

        bool _disposed;
        /// <summary>
        /// 释放窗体
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_disposed) { return; }

                //if (disposing) { }

                //销毁窗体
                Debug.WriteLineIf(!DestroyWindow(_hWnd), "Failed", "DestroyWindow");
                _hWnd = IntPtr.Zero;

                //注销窗口类
                //窗口类是共用的，每个实例都尝试注册和注销
                //实际效果就是先开的注册，后关的注销
                if (UnregisterClass(ClassName, HInstance))
                {
                    _wndProc = null;//只有注销成功时才解绑窗口过程
                }
                _wndClass = 0;

                Debug.WriteLineIf(!DeleteObject(_hBmp), "Failed", "Delete _hBmp");
                Debug.WriteLineIf(!DeleteDC(_dcMemory), "Failed", "Delete _dcMemory");

                _disposed = true;
            }
        }

        ~LayeredWindow()
        {
            Dispose(false);
        }

        //窗口过程委托
        private delegate IntPtr WndProcDelegate(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        #region Win32 API

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int iconId);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern short RegisterClass(WNDCLASS wc);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetMessage(ref MSG msg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);

        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage(ref MSG msg);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage(ref MSG msg);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, PointOrSize pptDst, PointOrSize pSizeDst, IntPtr hdcSrc, PointOrSize pptSrc, int crKey, BLENDFUNCTION pBlend, int dwFlags);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyWindow(IntPtr hWnd);

        // ReSharper disable NotAccessedField.Local
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedField.Compiler
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private class WNDCLASS
        {
            public int style;
            public WndProcDelegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr HWnd;
            public int Message;
            public IntPtr WParam;
            public IntPtr LParam;
            public int Time;
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class PointOrSize
        {
            public int XOrWidth, YOrHeight;

            public static readonly PointOrSize Empty = new PointOrSize();

            public PointOrSize() { XOrWidth = 0; YOrHeight = 0; }

            public PointOrSize(int xOrWidth, int yOrHeight)
            {
                XOrWidth = xOrWidth;
                YOrHeight = yOrHeight;
            }
        }

        // ReSharper restore UnusedField.Compiler
        // ReSharper restore MemberCanBePrivate.Local
        // ReSharper restore FieldCanBeMadeReadOnly.Local
        // ReSharper restore NotAccessedField.Local
        #endregion
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AhDung.Drawing
{
    /// <summary>
    /// 描边位置
    /// </summary>
    internal enum Direction
    {
        /// <summary>
        /// 居中
        /// </summary>
        Middle,
        /// <summary>
        /// 内部
        /// </summary>
        Inner,
        /// <summary>
        /// 外部
        /// </summary>
        Outer
    }

    /// <summary>
    /// 存储一系列边框要素并产生合适的画笔
    /// <para>- 边框居中+奇数粗度时是介于两像素之间画，所以粗细在视觉上不精确，建议错开任一条件</para>
    /// </summary>
    internal class Border : IDisposable
    {
        //编写本类除了整合边框信息外，更重要的原因是如果不对画笔做额外处理，
        //Draw出来的边框是不理想的。本类的原理是：
        // - 偶数边框（这是得到理想效果的前提）
        // - 再利用画笔的CompoundArray属性将边框裁切掉一半，
        //   同时根据不同参数偏移描边位置，达到可内可外可居中的效果

        float[] _compoundArray;
        /// <summary>
        /// 根据_direction处理线段
        /// </summary>
        float[] CompoundArray
        {
            get
            {
                if (_compoundArray == null)
                {
                    _compoundArray = new float[2];
                }
                switch (_direction)
                {
                    case Direction.Middle: goto default;
                    case Direction.Inner: _compoundArray[0] = 0.5f; _compoundArray[1] = 1f; break;
                    case Direction.Outer: _compoundArray[0] = 0f; _compoundArray[1] = 0.5f; break;
                    default: _compoundArray[0] = 0.25f; _compoundArray[1] = 0.75f; break;
                }
                return _compoundArray;
            }
        }

        readonly Pen _pen;
        /// <summary>
        /// 获取用于画本边框的画笔。建议销毁本类而不是该画笔
        /// </summary>
        public Pen Pen { get { return _pen; } }

        /// <summary>
        /// 边框宽度。默认1
        /// </summary>
        public int Width
        {
            get { return (int)_pen.Width / 2; }
            set { _pen.Width = value * 2; }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color Color
        {
            get { return _pen.Color; }
            set { _pen.Color = value; }
        }

        Direction _direction;
        /// <summary>
        /// 边框位置。默认居中
        /// </summary>
        public Direction Direction
        {
            get { return _direction; }
            set
            {
                if (_direction == value) { return; }
                _direction = value;
                _pen.CompoundArray = CompoundArray;
            }
        }

        /// <summary>
        /// 描边是否躲在填充之后。默认false
        /// <para>- 如果躲，则处于内部的部分会被填充遮挡，反之则是填充被这部分边框遮挡</para>
        /// <para>- 此属性仅供外部在绘制时确定描边和填充的次序</para>
        /// </summary>
        public bool Behind { get; set; }

        /// <summary>
        /// 获取指定矩形加上本边框后的边界
        /// </summary>
        public Rectangle GetBounds(Rectangle rectangle)
        {
            if (!IsValid() || _direction == Direction.Inner)
            {
                return rectangle;
            }
            var inflate = _direction == Direction.Middle
                ? (int)Math.Ceiling(Width / 2d)
                : Width;
            rectangle.Inflate(inflate, inflate);
            return rectangle;
        }

        public Border(Color color) : this(new Pen(color), false) { }

        /// <summary>
        /// 基于现有画笔的副本构造
        /// </summary>
        public Border(Pen pen) : this(pen, true) { }

        protected Border(Pen pen, bool useCopy)
        {
            _pen = useCopy ? (Pen)pen.Clone() : pen;
            _pen.Alignment = PenAlignment.Center;
            _pen.Width = Convert.ToInt32(_pen.Width * 2);
            _pen.CompoundArray = CompoundArray;
        }

        /// <summary>
        /// 是否有效边框。无宽度或完全透明视为无效
        /// </summary>
        public bool IsValid()
        {
            return Width > 0 && (_pen.PenType != PenType.SolidColor || Color.A > 0);
        }

        /// <summary>
        /// 是否有效边框。无宽度或完全透明视为无效
        /// </summary>
        public static bool IsValid(Border border)
        {
            return border != null && border.IsValid();
        }

        public void Dispose()
        {
            if (_pen != null) { _pen.Dispose(); }
        }
    }
}

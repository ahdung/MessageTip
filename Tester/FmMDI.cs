using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
#if NET30_OR_GREATER
using System.Linq;
#endif
using System.Windows.Forms;

namespace AhDung;

public partial class FmMDI : Form
{
    public FmMDI()
    {
        InitializeComponent();
    }

    void btnNewChild_Click(object sender, EventArgs e)
    {
        new FmTester
        {
            Text      = "Form " + (MdiChildren.Length + 1),
            MdiParent = this
        }.Show();
    }

    void btnNewForm_Click(object sender, EventArgs e)
    {
        new FmTester().Show();
    }

    void btnTestItem_Click(object sender, EventArgs e)
    {
        MessageTip.ShowOk((ToolStripItem)sender, txbText.Text);
    }

    void txbText_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            btnShow.PerformClick();
        }
    }

    void btnShow_Click(object sender, EventArgs e)
    {
//        var       text = "The 消息";
//        using var font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12);

//#if NET30_OR_GREATER

//        foreach (var doClear in System.Linq.Enumerable.Range(0, 2).Select(Convert.ToBoolean))
//        {
//            foreach (SmoothingMode smoothingMode in Enum.GetValues(typeof(SmoothingMode)))
//            {
//                foreach (PixelOffsetMode pixelOffsetMode in Enum.GetValues(typeof(PixelOffsetMode)))
//                {
//                    foreach (CompositingMode compositingMode in Enum.GetValues(typeof(CompositingMode)))
//                    {
//                        foreach (CompositingQuality compositingQuality in Enum.GetValues(typeof(CompositingQuality)))
//                        {
//                            foreach (TextRenderingHint textRenderingHint in Enum.GetValues(typeof(TextRenderingHint)))
//                            {
//                                try
//                                {
//                                    DrawString(text, font, doClear, smoothingMode, pixelOffsetMode, compositingMode, compositingQuality, textRenderingHint);
//                                }
//                                catch
//                                {
//                                    Debug.WriteLine($"""
//                                        {nameof(doClear)}           : {doClear},
//                                        {nameof(smoothingMode)}     : {smoothingMode},
//                                        {nameof(pixelOffsetMode)}   : {pixelOffsetMode},
//                                        {nameof(compositingMode)}   : {compositingMode},
//                                        {nameof(compositingQuality)}: {compositingQuality},
//                                        {nameof(textRenderingHint)} : {textRenderingHint}
//                                        ==================================================
//                                        """);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        MessageBox.Show("Completion.");

//#endif

        //using var style = new TipStyle();
        //style.BackColor = Color.FromArgb(150, Color.Red);
        //MessageTip.Show(txbText.Text, style);

        MessageTip.ShowOk(txbText.Text);
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
        //ThreadPool.QueueUserWorkItem(_ => MessageTip.ShowOk("并行测试"));
    }

    void DrawString(string text, Font font, bool doClear, SmoothingMode smoothingMode,
        PixelOffsetMode pixelOffsetMode,
        CompositingMode compositingMode,
        CompositingQuality compositingQuality,
        TextRenderingHint textRenderingHint)
    {
        using var bmp = new Bitmap(130, 30);
        using var g   = Graphics.FromImage(bmp);

        if (doClear)
            g.Clear(Color.WhiteSmoke);

        g.SmoothingMode      = smoothingMode;
        g.PixelOffsetMode    = pixelOffsetMode;
        g.CompositingMode    = compositingMode;
        g.CompositingQuality = compositingQuality;
        g.TextRenderingHint  = textRenderingHint;
        g.DrawString(text, font, Brushes.Blue, Point.Empty);
        g.Flush();

        bmp.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"新建文件夹\\{nameof(doClear)}({Convert.ToInt32(doClear)})-{nameof(SmoothingMode)}({(int)smoothingMode})-{nameof(PixelOffsetMode)}({(int)pixelOffsetMode})-{nameof(CompositingMode)}({(int)compositingMode})-{nameof(CompositingQuality)}({(int)compositingQuality})-{nameof(TextRenderingHint)}({(int)textRenderingHint}).bmp"));
    }
}
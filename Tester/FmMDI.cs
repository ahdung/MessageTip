using System;
using System.Threading;
using System.Windows.Forms;

namespace AhDung
{
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
                Text = "Form " + (MdiChildren.Length + 1),
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
    }
}
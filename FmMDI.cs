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

        private void btnNewChild_Click(object sender, EventArgs e)
        {
            new FmTester
            {
                Text = "Form " + (MdiChildren.Length + 1),
                MdiParent = this
            }.Show();
        }

        private void btnNewForm_Click(object sender, EventArgs e)
        {
            new FmTester().Show();
        }

        private void btnTestItem_Click(object sender, EventArgs e)
        {
            MessageTip.ShowOk((ToolStripItem)sender, txbText.Text);
        }

        private void txbText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnShow.PerformClick();
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            MessageTip.ShowOk(txbText.Text);
            //ThreadPool.QueueUserWorkItem(obj => MessageTip.ShowOk("并行测试"));
            //ThreadPool.QueueUserWorkItem(obj => MessageTip.ShowOk("并行测试"));
            //ThreadPool.QueueUserWorkItem(obj => MessageTip.ShowOk("并行测试"));
            //ThreadPool.QueueUserWorkItem(obj => MessageTip.ShowOk("并行测试"));
            //ThreadPool.QueueUserWorkItem(obj => MessageTip.ShowOk("并行测试"));
        }
    }
}

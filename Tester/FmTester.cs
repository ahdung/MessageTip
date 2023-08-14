using System;
using System.Windows.Forms;

namespace AhDung
{
    public partial class FmTester : Form
    {
        TipStyle _style;

        public FmTester()
        {
            DoubleBuffered = true;
            InitializeComponent();
            _style = new TipStyle();
            propertyGrid1.SelectedObject = _style;
        }

        private void ckbFloating_CheckedChanged(object sender, EventArgs e)
        {
            MessageTip.Floating = ckbFloating.Checked;
        }

        private void nudDelay_ValueChanged(object sender, EventArgs e)
        {
            MessageTip.Delay = decimal.ToInt32(nudDelay.Value);
        }

        private void nudFade_ValueChanged(object sender, EventArgs e)
        {
            MessageTip.Fade = decimal.ToInt32(nudFade.Value);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MessageTip.ShowOk(txbMultiline.Text);
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            MessageTip.ShowWarning(txbMultiline.Text);
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            MessageTip.ShowError(txbMultiline.Text);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                MessageTip.Show(txbMultiline.Text, _style);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowInPanel_Click(object sender, EventArgs e)
        {
            MessageTip.Show(panel1, txbMultiline.Text);
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            MessageTip.Show((ToolStripItem)sender, txbMultiline.Text);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            _style.Clear();
            _style = new TipStyle();
            propertyGrid1.SelectedObject = _style;
        }

    }
}

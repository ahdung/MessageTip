using System.IO;
using AhDung.WinForm;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AhDung
{
    public partial class FmTester : Form
    {
        OpenFileDialog _ofd;

        public FmTester()
        {
            InitializeComponent();
        }

        private void btnSelectIcon_Click(object sender, EventArgs e)
        {
            if (_ofd == null)
            {
                _ofd = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Multiselect = false,
                    Filter = "*.bmp;*.jpg;*.gif;*.png;*.tif|*.bmp;*.jpg;*.gif;*.png;*.tif|*.*|*.*"
                };
            }
            if (_ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            txbIcon.Text = _ofd.FileName;
        }

        private void ckbFloating_CheckedChanged(object sender, EventArgs e)
        {
            MessageTip.AllowFloating = ckbFloating.Checked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MessageTip.ShowOk(txbText.Text, decimal.ToInt32(nudDelay.Value));
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            MessageTip.ShowWarning(txbText.Text, decimal.ToInt32(nudDelay.Value));
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            MessageTip.ShowError(txbText.Text, decimal.ToInt32(nudDelay.Value));
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (txbIcon.TextLength != 0 && !File.Exists(txbIcon.Text))
            {
                MessageBox.Show("图片不存在！");
                return;
            }

            try
            {
                MessageTip.Show(txbText.Text,
                    txbIcon.TextLength == 0 ? null : Image.FromFile(txbIcon.Text),
                    decimal.ToInt32(nudDelay.Value));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

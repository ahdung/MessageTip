// Copyright (c) AhDung. All Rights Reserved.

using System;
using System.Windows.Forms;

namespace AhDung;

public sealed partial class FmTester : Form
{
    TipStyle _style;

    public FmTester()
    {
        DoubleBuffered = true;
        InitializeComponent();
        nudDelay.Value = MessageTip.Delay;
        nudFade.Value = MessageTip.Fade;
        _style = new TipStyle();
        propertyGrid1.SelectedObject = _style;
    }

    void nudDelay_ValueChanged(object sender, EventArgs e)
    {
        MessageTip.Delay = decimal.ToInt32(nudDelay.Value);
    }

    void nudFade_ValueChanged(object sender, EventArgs e)
    {
        MessageTip.Fade = decimal.ToInt32(nudFade.Value);
    }

    void btnOk_Click(object sender, EventArgs e)
    {
        MessageTip.ShowOk(txbMultiline.Text);
    }

    void btnWarning_Click(object sender, EventArgs e)
    {
        MessageTip.ShowWarning(txbMultiline.Text);
    }

    void btnError_Click(object sender, EventArgs e)
    {
        MessageTip.ShowError(txbMultiline.Text);
    }

    void btnShow_Click(object sender, EventArgs e)
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

    void btnShowInPanel_Click(object sender, EventArgs e)
    {
        MessageTip.Show(panel1, txbMultiline.Text);
    }

    void btnEnter_Click(object sender, EventArgs e)
    {
        MessageTip.Show((ToolStripItem)sender, txbMultiline.Text);
    }

    void btnRestore_Click(object sender, EventArgs e)
    {
        _style.Clear();
        _style = new TipStyle();
        propertyGrid1.SelectedObject = _style;
    }
}

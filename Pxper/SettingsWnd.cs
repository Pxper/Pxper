// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Forms;

namespace Pxper
{
  public partial class SettingsWnd : Form
  {
    public SettingsWnd()
    {
      this.InitializeComponent();

      if (!DllSupportHelper.Check("Magnification.dll"))
        this.tc.TabPages.Remove(this.tpMagnification);
    }

    private void chbHideOverlaysIfActiveWindowTitleContains_CheckedChanged(object sender, System.EventArgs e)
    {
      this.tbActiveWindowTitlePart.Enabled = this.chbHideOverlaysIfActiveWindowTitleContains.Checked;
    }
  }
}
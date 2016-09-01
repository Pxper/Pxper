// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Windows.Forms;

namespace Pxper
{
  public partial class ShaderWnd : LayeredWndBase
  {
    public ShaderWnd()
    {
      this.InitializeComponent();
      this.SetLayered();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      if (e is FormClosingEventArgs)
      {
        CloseReason closeReason = (e as FormClosingEventArgs).CloseReason;

        if (closeReason == CloseReason.UserClosing)
          e.Cancel = true;

        else e.Cancel = false;
      }

      base.OnClosing(e);
    }
  }
}
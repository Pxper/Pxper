// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Windows.Forms;
using Pxper.Properties;

namespace Pxper
{
  public partial class HelpWnd : LayeredWndBase
  {
    public HelpWnd()
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

    protected override void OnPaint(PaintEventArgs e)
    {
      if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "uk")
        e.Graphics.DrawImage(
          Resources.Help_uk, 0, 0, Resources.Help_uk.Width, Resources.Help_uk.Height
        );

      else if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ru")
        e.Graphics.DrawImage(
          Resources.Help_ru, 0, 0, Resources.Help_ru.Width, Resources.Help_ru.Height
        );

      else e.Graphics.DrawImage(
        Resources.Help_en, 0, 0, Resources.Help_en.Width, Resources.Help_en.Height
      );
    }
  }
}
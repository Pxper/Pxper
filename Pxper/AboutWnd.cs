// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Windows.Forms;

namespace Pxper
{
  public partial class AboutWnd : Form
  {
    public AboutWnd()
    {
      this.InitializeComponent();
    }

    private void llProgramWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://pxper.com");
    }

    private void llEMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("mailto:dmitry@sikorsky.pro");
    }

    private void llWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://sikorsky.pro");
    }
  }
}

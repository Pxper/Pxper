// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Forms;

namespace Pxper
{
  public partial class HorizontalsWnd : Form
  {
    public int VerticalInterval
    {
      get
      {
        return (int)this.nudVerticalInterval.Value;
      }
    }

    public HorizontalsWnd()
    {
      this.InitializeComponent();
    }
  }
}
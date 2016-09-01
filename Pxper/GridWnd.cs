// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows.Forms;

namespace Pxper
{
  public partial class GridWnd : Form
  {
    public int HorizontalInterval
    {
      get
      {
        return (int)this.nudHorizontalInterval.Value;
      }
    }

    public int VerticalInterval
    {
      get
      {
        return (int)this.nudVerticalInterval.Value;
      }
    }

    public GridWnd()
    {
      this.InitializeComponent();
    }
  }
}
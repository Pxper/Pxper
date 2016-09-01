// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pxper
{
  public enum Control
  {
    None,
    Grip
  }

  public class ControlEventArgs : EventArgs
  {
    public Control Control { get; set; }

    public ControlEventArgs(Control control)
    {
      this.Control = control;
    }
  }
}
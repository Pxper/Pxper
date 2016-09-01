// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pxper
{
  public enum Tool
  {
    None = -1,
    HorizontalGuide,
    VerticalGuide,
    Image,
    Default,
    Measure,
    Move,
    Remove,
    Lock,
    Visibility
  }

  public class ToolEventArgs : EventArgs
  {
    public Tool Tool { get; set; }

    public ToolEventArgs(Tool tool)
    {
      this.Tool = tool;
    }
  }
}
// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pxper
{
  public enum GuideColor
  {
    None = -1,
    Blue,
    Purple,
    Yellow
  }

  public class GuideColorEventArgs : EventArgs
  {
    public GuideColor GuideColor { get; set; }

    public GuideColorEventArgs(GuideColor guideColor)
    {
      this.GuideColor = guideColor;
    }
  }
}
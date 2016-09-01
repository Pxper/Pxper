// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;

namespace Pxper
{
  [Serializable]
  public class Image : OverlayBase
  {
    public System.Drawing.Image Normal { get; set; }
    public System.Drawing.Image Hovered { get; set; }

    public Point Position { get; set; }
    public Point TempPosition { get; set; }
  }
}
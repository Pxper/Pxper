// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pxper
{
  public enum Orienation
  {
    Horizontal,
    Vertical
  }

  [Serializable]
  public class Guide : OverlayBase
  {
    public Orienation Orienation { get; set; }
    public GuideColor GuideColor { get; set; }
    public int Position { get; set; }
    public int TempPosition { get; set; }
  }
}
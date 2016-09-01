// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace Pxper
{
	public class WinGdi
	{
		public const int AC_SRC_OVER  = 0;
		public const int AC_SRC_ALPHA = 1;

    [StructLayout(LayoutKind.Sequential)]
    public struct BLENDFUNCTION
    {
      public byte BlendOp;
      public byte BlendFlags;
      public byte SourceConstantAlpha;
      public byte AlphaFormat;
    }
	}
}
// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Pxper
{
  public static class DllSupportHelper
  {
    public static bool Check(string fileName)
    {
      return Kernel32.LoadLibrary(fileName) != IntPtr.Zero;
    }
  }
}
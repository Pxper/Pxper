// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Pxper
{
  public class LayeredWndBase : Form
  {
    public new virtual void Invalidate()
    {
      if (this.Visible)
        this.Invalidate(this.Bounds);
    }

    protected override void OnShown(EventArgs e)
    {
      this.Invalidate();
    }

    protected void SetLayered()
    {
      UInt32 dwExStyle = User32.GetWindowLong(this.Handle, WinUser.GWL_EXSTYLE);

      dwExStyle = dwExStyle | WinUser.WS_EX_TOOLWINDOW | WinUser.WS_EX_NOACTIVATE | WinUser.WS_EX_LAYERED;
      User32.SetWindowLong(this.Handle, WinUser.GWL_EXSTYLE, dwExStyle);
    }

    protected void SetTransparent()
    {
      UInt32 dwExStyle = User32.GetWindowLong(this.Handle, WinUser.GWL_EXSTYLE);

      dwExStyle = dwExStyle | WinUser.WS_EX_TRANSPARENT;
      User32.SetWindowLong(this.Handle, WinUser.GWL_EXSTYLE, dwExStyle);
    }

    private new void Invalidate(Rectangle rect)
    {
      Bitmap memoryBitmap = new Bitmap(
        rect.Size.Width,
        rect.Size.Height,
        PixelFormat.Format32bppArgb
      );

      using (Graphics g = Graphics.FromImage(memoryBitmap))
      {
        Rectangle area = new Rectangle(0, 0, rect.Size.Width, rect.Size.Height);

        this.OnPaint(new PaintEventArgs(g, area));

        IntPtr hDC = User32.GetDC(IntPtr.Zero);
        IntPtr memoryDC = Gdi32.CreateCompatibleDC(hDC);
        IntPtr hBitmap = memoryBitmap.GetHbitmap(Color.FromArgb(0));
        IntPtr oldBitmap = Gdi32.SelectObject(memoryDC, hBitmap);

        POINT dst;
        dst.x = rect.Location.X;
        dst.y = rect.Location.Y;

        SIZE size;
        size.cx = rect.Size.Width;
        size.cy = rect.Size.Height;

        POINT src;
        src.x = 0;
        src.y = 0;

        WinGdi.BLENDFUNCTION blend;
        blend.BlendOp = (byte)WinGdi.AC_SRC_OVER;
        blend.BlendFlags = 0;
        blend.SourceConstantAlpha = (byte)255;
        blend.AlphaFormat = (byte)WinGdi.AC_SRC_ALPHA;

        User32.UpdateLayeredWindow(
          this.Handle, hDC, ref dst, ref size, memoryDC, ref src, 0, ref blend, WinUser.ULW_ALPHA
        );

        Gdi32.SelectObject(memoryDC, oldBitmap);
        User32.ReleaseDC(IntPtr.Zero, hDC);
        Gdi32.DeleteObject(hBitmap);
        Gdi32.DeleteDC(memoryDC);
        Gdi32.DeleteDC(hDC);
        memoryBitmap.Dispose();
      }
    }
  }
}
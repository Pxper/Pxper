// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pxper
{
  public class Magnifier : IDisposable
  {
    private Form form;
    private Timer updateTimer;
    private Timer animateTimer;
    private float magnificationFactor;
    private IntPtr hwndMagnificator;
    private RECT magnificatorWindowRect;
    private bool isInitialized;

    public static int X { get; set; }
    public static int Y { get; set; }

    public float MagnificationFactor
    {
      get { return this.magnificationFactor; }
      set
      {
        if (this.magnificationFactor != value)
        {
          this.magnificationFactor = value;
          
          Magnification.Transformation matrix = new Magnification.Transformation(this.magnificationFactor);

          Magnification.MagSetWindowTransform(this.hwndMagnificator, ref matrix);
        }
      }
    }

    public Magnifier(Form form)
    {
      if (form == null)
        throw new ArgumentNullException("form");

      this.form = form;
      this.form.Resize += new EventHandler(form_Resize);
      this.form.FormClosing += new FormClosingEventHandler(form_FormClosing);
      this.updateTimer = new Timer();
      this.updateTimer.Interval = WinUser.USER_TIMER_MINIMUM;
      this.updateTimer.Tick += new EventHandler(updateTimer_Tick);
      this.animateTimer = new Timer();
      this.animateTimer.Interval = 50;
      this.animateTimer.Tick += new EventHandler(animateTimer_Tick);
      this.magnificationFactor = 2.0f;
      this.magnificatorWindowRect = new RECT();
      this.isInitialized = Magnification.MagInitialize();

      if (this.isInitialized)
      {
        this.CreateMagnifier();
        this.updateTimer.Start();
      }
    }

    ~Magnifier()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      this.form.Resize -= form_Resize;
      this.form.FormClosing -= form_FormClosing;
      this.updateTimer.Stop();
      this.animateTimer.Stop();

      if (disposing)
        this.updateTimer.Dispose();

      this.updateTimer = null;
      this.DestroyMagnifier();
    }

    private void form_Resize(object sender, EventArgs e)
    {
      this.ResizeMagnifier();
    }

    private void form_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.updateTimer.Stop();
      this.animateTimer.Stop();
    }

    private void updateTimer_Tick(object sender, EventArgs e)
    {
      this.UpdateMaginifier();
    }

    private void animateTimer_Tick(object sender, EventArgs e)
    {
      if (this.form != null && this.form.Visible && this.form.Opacity < 1)
        this.form.Opacity += 0.25;

      else this.animateTimer.Stop();
    }

    private void CreateMagnifier()
    {
      if (!this.isInitialized)
        return;

      IntPtr hInst = Kernel32.GetModuleHandle(null);

      this.form.AllowTransparency = true;
      this.form.TransparencyKey = Color.Empty;
      this.form.Opacity = 0;
      User32.GetClientRect(this.form.Handle, ref this.magnificatorWindowRect);
      this.hwndMagnificator = User32.CreateWindow(
        0,
        Magnification.WC_MAGNIFIER,
        "MagnifierWindow",
        (int)WinUser.WS_VISIBLE | (int)WinUser.WS_CHILD | (int)Magnification.MagnifierStyle.MS_SHOWMAGNIFIEDCURSOR,
        this.magnificatorWindowRect.left,
        this.magnificatorWindowRect.top,
        this.magnificatorWindowRect.right,
        this.magnificatorWindowRect.bottom,
        this.form.Handle,
        IntPtr.Zero,
        hInst,
        IntPtr.Zero
      );

      if (this.hwndMagnificator == IntPtr.Zero)
        return;

      Magnification.Transformation matrix = new Magnification.Transformation(this.magnificationFactor);

      Magnification.MagSetWindowTransform(this.hwndMagnificator, ref matrix);
    }

    private void UpdateMaginifier()
    {
      if (!this.isInitialized || this.hwndMagnificator == IntPtr.Zero)
        return;

      //POINT mousePosition = new POINT();
      RECT rect = new RECT();

      //User32.GetCursorPos(ref mousePosition);

      int width = (int)((this.magnificatorWindowRect.right - this.magnificatorWindowRect.left) / this.magnificationFactor);
      int height = (int)((this.magnificatorWindowRect.bottom - this.magnificatorWindowRect.top) / this.magnificationFactor);

      rect.left = Magnifier.X/*mousePosition.x*/ - width / 2;
      rect.top = Magnifier.Y/*mousePosition.y*/ - height / 2;

      if (rect.left < 0)
        rect.left = 0;

      if (rect.left > User32.GetSystemMetrics(WinUser.SM_CXSCREEN) - width)
        rect.left = User32.GetSystemMetrics(WinUser.SM_CXSCREEN) - width;

      if (rect.top < 0)
        rect.top = 0;

      if (rect.top > User32.GetSystemMetrics(WinUser.SM_CYSCREEN) - height)
        rect.top = User32.GetSystemMetrics(WinUser.SM_CYSCREEN) - height;

      rect.right = rect.left + width;
      rect.bottom = rect.top + height;

      if (this.form == null)
      {
        this.updateTimer.Stop();
        this.animateTimer.Stop();
        return;
      }

      if (this.form.IsDisposed)
      {
        this.updateTimer.Enabled = false;
        return;
      }

      Magnification.MagSetWindowSource(this.hwndMagnificator, rect);
      User32.SetWindowPos(
        this.form.Handle,
        Magnification.HWND_TOPMOST,
        0,
        0,
        0,
        0,
        (int)WinUser.SWP_NOSIZE | (int)WinUser.SWP_NOMOVE | (int)WinUser.SWP_NOACTIVATE
      );

      User32.InvalidateRect(this.hwndMagnificator, IntPtr.Zero, true);
      this.animateTimer.Start();
    }

    private void ResizeMagnifier()
    {
      if (this.isInitialized && this.hwndMagnificator != IntPtr.Zero)
      {
        User32.GetClientRect(this.form.Handle, ref this.magnificatorWindowRect);
        User32.SetWindowPos(
          this.hwndMagnificator,
          IntPtr.Zero,
          this.magnificatorWindowRect.left,
          this.magnificatorWindowRect.top,
          this.magnificatorWindowRect.right,
          this.magnificatorWindowRect.bottom,
          0
        );
      }
    }

    private void DestroyMagnifier()
    {
      if (isInitialized)
        Magnification.MagUninitialize();
    }
  }
}
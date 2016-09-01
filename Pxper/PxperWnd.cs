// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pxper.Properties;

namespace Pxper
{
  public partial class PxperWnd : LayeredWndBase
  {
    public readonly Color colorHovered = Color.Red;
    public readonly Color colorBlue = Color.FromArgb(0, 255, 255);
    public readonly Color colorPurple = Color.FromArgb(255, 0, 255);
    public readonly Color colorYellow = Color.FromArgb(255, 255, 0);
    public readonly Color colorAbsRuler = Color.FromArgb(200, 0, 255, 0);
    public readonly Color colorAbsRulerLabel = Color.FromArgb(255, 255, 255);
    public readonly Color colorRelRuler = Color.FromArgb(200, 255, 0, 0);
    public readonly Color colorRelRulerLabel = Color.FromArgb(255, 255, 255);
    public const int helpWndOffsetX = -281;
    public const int helpWndOffsetY = -172;
    public const int rulerSpacing = 50;
    public const int rulerPadding = 10;
    public const int precision = 1;

    private bool invalidateRequested;
    private bool hideOverlays;
    private List<OverlayBase> overlays;
    private OverlayBase overlay;
    private Image activeImage;
    private bool showOfdImage;
    private HookHelper hookHelper;
    private bool isControlPressed;
    private int mouseX;
    private int mouseY;
    private bool mouseButtonPressed;
    private int startX;
    private int startY;
    private int controlKeyStartX;
    private int controlKeyStartY;
    private int toolbarWndX;
    private int toolbarWndY;
    private ShaderWnd shaderWnd;
    private ToolbarWnd toolbarWnd;
    private HelpWnd helpWnd;
    private MagnifierWnd magnifierWnd;
    private Control activeControl;
    private GuideColor activeColor;
    private Tool tool;
    private Tool activeTool;
    private Tool mode1Tool;
    private Tool mode2Tool;
    private Tool mode3Tool;
    
    public PxperWnd()
    {
      this.overlays = new List<OverlayBase>();
      this.hookHelper = new HookHelper();
      this.shaderWnd = new ShaderWnd();
      this.toolbarWnd = new ToolbarWnd();
      this.helpWnd = new HelpWnd();
      this.tool = Tool.None;
      this.activeTool = Tool.None;
      this.mode1Tool = Tool.Default;
      this.mode2Tool = Tool.None;
      this.mode3Tool = Tool.Visibility;
      this.InitializeComponent();
      this.SetLayered();
      this.SetTransparent();
    }

    protected override void OnShown(EventArgs e)
    {
      base.OnShown(e);
      this.SetHook();
      this.ShowToolbarWnd();

      if (!Settings.Default.HelpWndShown)
        this.ShowHelpWnd();

      this.StartInvalidateTimer();
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      this.ResetHook();
      this.SaveToolbarWndSettings();
      base.OnClosing(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (this.hideOverlays || this.mode3Tool == Tool.None)
        return;

      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

      foreach (OverlayBase overlay in this.overlays)
        this.DrawOverlay(e.Graphics, overlay);

      if (this.overlay != null)
      {
        if (!this.overlays.Contains(this.overlay))
          this.DrawOverlay(e.Graphics, this.overlay);

        if (this.overlay is Guide)
        {
          if (Settings.Default.ShowAbsRulers)
            this.DrawAbsRulers(e.Graphics, this.overlay as Guide);

          if (Settings.Default.ShowRelRulers)
            this.DrawRelRulers(e.Graphics, this.overlay as Guide);
        }
      }

      e.Graphics.SmoothingMode = SmoothingMode.Default;

      if (this.mode1Tool == Tool.Measure && this.mouseButtonPressed)
        this.DrawMeasure(e.Graphics);
    }

    private void ni_DoubleClick(object sender, EventArgs e)
    {
      if (this.helpWnd.Visible)
        this.HideHelpWnd();

      this.toolbarWnd.Visible = !this.toolbarWnd.Visible;
      this.toolbarWnd.Invalidate();
      this.BringToFront();
      this.toolbarWnd.BringToFront();
    }

    private void cmiClear_Click(object sender, EventArgs e)
    {
      if (this.overlays.Count != 0 && MessageBox.Show(Resources.AreYouSure, Resources.Confirmation, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        this.overlays = new List<OverlayBase>();
    }

    private void cmiSaveAs_Click(object sender, EventArgs e)
    {
      if (this.sfd.ShowDialog() == DialogResult.OK)
        ObjectUtils.Save(this.overlays, this.sfd.FileName);
    }

    private void cmiOpen_Click(object sender, EventArgs e)
    {
      if (this.ofd.ShowDialog() == DialogResult.OK)
      {
        this.overlays = (List<OverlayBase>)ObjectUtils.Load(this.ofd.FileName);
        this.RequestInvalidate();
      }
    }

    private void cmiOpenFromPsd_Click(object sender, EventArgs e)
    {
      if (this.ofdPsd.ShowDialog() == DialogResult.OK)
      {
        this.overlays = new PsdParser().GetGuides(this.ofdPsd.FileName);
        this.RequestInvalidate();
      }
    }

    private void cmiGrid_Click(object sender, EventArgs e)
    {
      GridWnd wnd = new GridWnd();

      if (wnd.ShowDialog() == DialogResult.OK)
      {
        this.overlays = new List<OverlayBase>();

        for (int i = wnd.VerticalInterval; i <= this.Height; i += wnd.VerticalInterval)
        {
          Guide guide = new Guide();

          guide.Orienation = Orienation.Horizontal;
          guide.Position = i;
          this.overlays.Add(guide);
        }

        for (int i = wnd.HorizontalInterval; i <= this.Width; i += wnd.HorizontalInterval)
        {
          Guide guide = new Guide();

          guide.Orienation = Orienation.Vertical;
          guide.Position = i;
          this.overlays.Add(guide);
        }

        this.RequestInvalidate();
      }
    }

    private void cmiVerticals_Click(object sender, EventArgs e)
    {
      VerticalsWnd wnd = new VerticalsWnd();

      if (wnd.ShowDialog() == DialogResult.OK)
      {
        this.overlays = new List<OverlayBase>();

        for (int i = wnd.HorizontalInterval; i <= this.Width; i += wnd.HorizontalInterval)
        {
          Guide guide = new Guide();

          guide.Orienation = Orienation.Vertical;
          guide.Position = i;
          this.overlays.Add(guide);
        }

        this.RequestInvalidate();
      }
    }

    private void cmiHorizontals_Click(object sender, EventArgs e)
    {
      HorizontalsWnd wnd = new HorizontalsWnd();

      if (wnd.ShowDialog() == DialogResult.OK)
      {
        this.overlays = new List<OverlayBase>();

        for (int i = wnd.VerticalInterval; i <= this.Height; i += wnd.VerticalInterval)
        {
          Guide guide = new Guide();

          guide.Orienation = Orienation.Horizontal;
          guide.Position = i;
          this.overlays.Add(guide);
        }

        this.RequestInvalidate();
      }
    }

    private void cmiSettings_Click(object sender, EventArgs e)
    {
      if (new SettingsWnd().ShowDialog() == DialogResult.OK)
      {
        Settings.Default.Save();
        this.StartInvalidateTimer();
        this.RequestInvalidate();
      }

      else Settings.Default.Reload();
    }

    private void cmiTooltips_Click(object sender, EventArgs e)
    {
      if (!this.toolbarWnd.Visible)
      {
        this.toolbarWnd.Visible = true;
        this.toolbarWnd.Invalidate();
        this.BringToFront();
        this.toolbarWnd.BringToFront();
      }

      this.ShowHelpWnd();
    }

    private void cmiAbout_Click(object sender, EventArgs e)
    {
      new AboutWnd().ShowDialog();
    }

    private void cmiExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void hookHelper_CtrlKeyDown(object sender, EventArgs e)
    {
      if (this.isControlPressed)
        return;

      this.controlKeyStartX = this.mouseX;
      this.controlKeyStartY = this.mouseY;
      this.isControlPressed = true;
    }

    private void hookHelper_CtrlKeyUp(object sender, EventArgs e)
    {
      this.isControlPressed = false;
    }

    private void hookManager_MouseMove(object sender, MouseEventArgs e)
    {
      this.mouseX = e.X;
      this.mouseY = e.Y;

      if (this.activeControl == Control.Grip && this.mouseButtonPressed)
      {
        int xDelta = this.mouseX - this.startX;
        int yDelta = this.mouseY - this.startY;

        this.toolbarWnd.Location = new Point(this.toolbarWndX + xDelta, this.toolbarWndY + yDelta);
        return;
      }

      if (this.overlay != null && this.mouseButtonPressed)
      {
        this.MoveOverlay(this.overlay);
        this.RequestInvalidate();
        return;
      }

      if (this.mode1Tool == Tool.Measure)
      {
        this.RequestInvalidate();
        return;
      }

      if (this.mode1Tool == Tool.Move || this.mode1Tool == Tool.Remove)
      {
        this.overlay = null;

        foreach (OverlayBase overlay in this.overlays)
        {
          overlay.IsHovered = false;

          if (this.mode2Tool == Tool.Lock)
          {
            overlay.IsHovered = true;

            if (this.mode1Tool == Tool.Move && this.mouseButtonPressed)
              this.MoveOverlay(overlay);
          }

          else if (this.overlay == null && this.IsHovered(overlay))
          {
            overlay.IsHovered = true;
            this.overlay = overlay;
          }
        }

        this.RequestInvalidate();
        return;
      }
    }

    private void hookHelper_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.mode1Tool != Tool.Default && this.PointInsideToolbarWnd(e.Location))
        return;

      this.mouseButtonPressed = true;
      this.startX = e.X;
      this.startY = e.Y;
      this.toolbarWndX = this.toolbarWnd.Location.X;
      this.toolbarWndY = this.toolbarWnd.Location.Y;
      this.activeTool = this.tool;

      foreach (OverlayBase item in this.overlays)
      {
        if (item is Guide)
          (item as Guide).TempPosition = (item as Guide).Position;

        else if (item is Image)
          (item as Image).TempPosition = (item as Image).Position;
      }

      if (this.activeTool == Tool.HorizontalGuide)
      {
        this.CreateHorizontalGuide();
        this.ShowMagnifierWnd();
        return;
      }

      if (this.activeTool == Tool.VerticalGuide)
      {
        this.CreateVerticalGuide();
        this.ShowMagnifierWnd();
        return;
      }

      if (this.activeTool == Tool.Image)
      {
        this.CreateImage();
        return;
      }

      if (this.mode1Tool == Tool.Move)
      {
        this.ShowMagnifierWnd();
        return;
      }

      if (this.mode1Tool == Tool.Remove)
      {
        if (this.mode2Tool == Tool.Lock)
          this.overlays.Clear();

        else if (this.overlay != null)
        {
          this.overlays.Remove(this.overlay);
          this.overlay = null;
        }

        this.RequestInvalidate();
        return;
      }
    }

    private void hookHelper_MouseUp(object sender, MouseEventArgs e)
    {
      this.HideMagnifierWnd();
      this.mouseButtonPressed = false;
      this.startX = 0;
      this.startY = 0;
      this.toolbarWndX = 0;
      this.toolbarWndY = 0;

      foreach (OverlayBase item in this.overlays)
      {
        if (item is Guide)
          (item as Guide).TempPosition = 0;

        else if (item is Image)
          (item as Image).TempPosition = Point.Empty;
      }

      if (this.activeTool == Tool.Image && this.overlay != null)
      {
        this.activeImage = this.overlay as Image;
        this.showOfdImage = true;
      }

      this.activeTool = Tool.None;

      if (this.overlay != null && !this.overlays.Contains(this.overlay))
        this.AppendOverlay();

      this.RequestInvalidate();
    }

    protected void toolbarWnd_ToolbarClicked(object sender, EventArgs e)
    {
      if (this.helpWnd.Visible)
        this.HideHelpWnd();
    }

    protected void toolbarWnd_ControlSelected(object sender, ControlEventArgs e)
    {
      this.activeControl = e.Control;
    }

    protected void toolbarWnd_ControlDeselected(object sender, ControlEventArgs e)
    {
      this.activeControl = Control.None;
    }

    protected void toolbarWnd_GuideColorChanged(object sender, GuideColorEventArgs e)
    {
      this.activeColor = e.GuideColor;
    }

    protected void toolbarWnd_ToolSelected(object sender, ToolEventArgs e)
    {
      this.tool = e.Tool;
    }

    protected void toolbarWnd_ToolDeselected(object sender, ToolEventArgs e)
    {
      this.tool = Tool.None;
    }

    protected void toolbarWnd_Mode1ToolChanged(object sender, ToolEventArgs e)
    {
      this.overlay = null;
      this.ResetHover();
      this.mode1Tool = e.Tool;
      this.RequestInvalidate();

      if (this.mode1Tool == Tool.Default)
        this.HideShaderWnd();

      else this.ShowShaderWnd();
    }

    protected void toolbarWnd_Mode2ToolChanged(object sender, ToolEventArgs e)
    {
      this.mode2Tool = e.Tool;
      this.RequestInvalidate();
    }

    protected void toolbarWnd_Mode3ToolChanged(object sender, ToolEventArgs e)
    {
      this.mode3Tool = e.Tool;
      this.RequestInvalidate();
    }

    private void tSystem_Tick(object sender, EventArgs e)
    {
      if (Settings.Default.HideOverlaysIfActiveWindowTitleContains)
      {
        if (this.GetActiveWindowTitle().Contains(Settings.Default.ActiveWindowTitlePart))
          this.hideOverlays = true;

        else this.hideOverlays = false;
      }

      else this.hideOverlays = false;

      if (this.showOfdImage)
        this.AskForImageFromUser();
    }

    private void tInvalidate_Tick(object sender, EventArgs e)
    {
      if (!this.invalidateRequested)
        return;

      this.invalidateRequested = false;
      this.Invalidate();
    }

    private void bwImageLoader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      try
      {
        using (Bitmap source = (Bitmap)Bitmap.FromFile(e.Argument as string))
        {
          this.activeImage.Normal = source.SetOpacity(this.GetImageOpacity());
          this.activeImage.Hovered = source.SetOpacity(this.GetImageOpacity()).SetRed();
        }
      }

      catch (Exception ex)
      {

      }

      finally
      {
        this.activeImage = null;
      }
    }

    private void bwImageLoader_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
    {
      this.RequestInvalidate();
    }

    private void SetHook()
    {
      if (!this.hookHelper.SetHook())
        return;

      this.hookHelper.CtrlKeyDown += hookHelper_CtrlKeyDown;
      this.hookHelper.CtrlKeyUp += hookHelper_CtrlKeyUp;
      this.hookHelper.MouseMove += hookManager_MouseMove;
      this.hookHelper.MouseDown += hookHelper_MouseDown;
      this.hookHelper.MouseUp += hookHelper_MouseUp;
    }

    private void ResetHook()
    {
      this.hookHelper.ResetHook();
    }

    private void ShowToolbarWnd()
    {
      this.toolbarWnd.ToolbarClicked += toolbarWnd_ToolbarClicked;
      this.toolbarWnd.ControlSelected += toolbarWnd_ControlSelected;
      this.toolbarWnd.ControlDeselected += toolbarWnd_ControlDeselected;
      this.toolbarWnd.GuideColorChanged += toolbarWnd_GuideColorChanged;
      this.toolbarWnd.ToolSelected += toolbarWnd_ToolSelected;
      this.toolbarWnd.ToolDeselected += toolbarWnd_ToolDeselected;
      this.toolbarWnd.Mode1ToolChanged += toolbarWnd_Mode1ToolChanged;
      this.toolbarWnd.Mode2ToolChanged += toolbarWnd_Mode2ToolChanged;
      this.toolbarWnd.Mode3ToolChanged += toolbarWnd_Mode3ToolChanged;

      if (Settings.Default.ToolbarWndLocation.X == -1 && Settings.Default.ToolbarWndLocation.Y == -1)
      {
        Rectangle screenRectangle = Screen.GetBounds(this);
        Point toolbarWndLocation = new Point(
          screenRectangle.Width / 2 - this.toolbarWnd.Width / 2,
          screenRectangle.Height / 3 - this.toolbarWnd.Height / 2
          );

        this.toolbarWnd.Location = toolbarWndLocation;
      }

      else this.toolbarWnd.Location = Settings.Default.ToolbarWndLocation;

      this.toolbarWnd.Show();
      this.toolbarWnd.Visible = Settings.Default.ToolbarWndVisibility;
      this.BringToFront();
      this.toolbarWnd.BringToFront();
    }

    private void SaveToolbarWndSettings()
    {
      Settings.Default.ToolbarWndLocation = this.toolbarWnd.Location;
      Settings.Default.ToolbarWndVisibility = this.toolbarWnd.Visible;
      Settings.Default.Save();
    }

    private void ShowShaderWnd()
    {
      this.shaderWnd.Show();
      this.shaderWnd.TopMost = true;
      this.shaderWnd.BringToFront();
      this.shaderWnd.TopMost = false;
      this.TopMost = true;
      this.BringToFront();
      this.TopMost = false;
      this.toolbarWnd.TopMost = true;
      this.toolbarWnd.BringToFront();
    }

    private void HideShaderWnd()
    {
      this.shaderWnd.Hide();
      this.TopMost = true;
      this.BringToFront();
      this.TopMost = false;
      this.toolbarWnd.TopMost = true;
      this.toolbarWnd.BringToFront();
    }

    private void ShowHelpWnd()
    {
      Settings.Default.HelpWndShown = true;
      Settings.Default.Save();
      this.ShowShaderWnd();

      Point location = this.toolbarWnd.Location;

      location.Offset(PxperWnd.helpWndOffsetX, PxperWnd.helpWndOffsetY);
      this.helpWnd.Location = location;
      this.helpWnd.Show();
      this.helpWnd.BringToFront();
    }

    private void HideHelpWnd()
    {
      this.helpWnd.Hide();
      this.HideShaderWnd();
    }

    private bool PointInsideToolbarWnd(Point location)
    {
      return this.toolbarWnd.Bounds.Contains(location);
    }

    private void ShowMagnifierWnd()
    {
      if (!DllSupportHelper.Check("Magnification.dll") || !Settings.Default.ShowMagnifyingGlass)
        return;

      this.magnifierWnd = new MagnifierWnd();
      this.magnifierWnd.Size = new Size(Settings.Default.MagnifyingGlassWidth, Settings.Default.MagnifyingGlassHeight);
      this.magnifierWnd.Show();

      Magnifier magnifier = new Magnifier(this.magnifierWnd);

      magnifier.MagnificationFactor = Settings.Default.MagnificationFactor;
    }

    private void HideMagnifierWnd()
    {
      if (!DllSupportHelper.Check("Magnification.dll") || !Settings.Default.ShowMagnifyingGlass)
        return;

      if (this.magnifierWnd != null && this.magnifierWnd.Visible)
        this.magnifierWnd.Close();
    }

    private void StartInvalidateTimer()
    {
      this.tInvalidate.Interval = 1000 / Settings.Default.RefreshRate;
      this.tInvalidate.Enabled = true;
    }

    private void RequestInvalidate()
    {
      this.invalidateRequested = true;
    }

    private void CreateHorizontalGuide()
    {
      Guide guide = new Guide();

      guide.GuideColor = this.activeColor;
      guide.Orienation = Orienation.Horizontal;
      guide.Position = guide.TempPosition = this.startY;
      this.overlay = guide;
      this.RequestInvalidate();
    }

    private void CreateVerticalGuide()
    {
      Guide guide = new Guide();

      guide.GuideColor = this.activeColor;
      guide.Orienation = Orienation.Vertical;
      guide.Position = guide.TempPosition = this.startX;
      this.overlay = guide;
      this.RequestInvalidate();
    }

    private void CreateImage()
    {
      Image image = new Image();

      using (Bitmap source = Resources.ImagePlaceholder)
      {
        image.Normal = source.SetOpacity(this.GetImageOpacity());
        image.Hovered = source.SetOpacity(this.GetImageOpacity()).SetRed();
      }

      image.Position = image.TempPosition = new Point(
        this.startX - Resources.ImagePlaceholder.Width / 2,
        this.startY - Resources.ImagePlaceholder.Height / 2
      );

      this.overlay = image;
      this.RequestInvalidate();
    }

    private float GetImageOpacity()
    {
      return 1 - Settings.Default.ImagesTransparency / 100f;
    }

    private void AppendOverlay()
    {
      List<OverlayBase> horizontalGuides = new List<OverlayBase>(this.overlays.Where(o => o is Guide && (o as Guide).Orienation == Orienation.Horizontal));

      if (this.overlay is Guide && (this.overlay as Guide).Orienation == Orienation.Horizontal)
        horizontalGuides.Add(this.overlay);

      List<OverlayBase> verticalGuides = new List<OverlayBase>(this.overlays.Where(o => o is Guide && (o as Guide).Orienation == Orienation.Vertical));

      if (this.overlay is Guide && (this.overlay as Guide).Orienation == Orienation.Vertical)
        verticalGuides.Add(this.overlay);

      List<OverlayBase> images = new List<OverlayBase>(this.overlays.Where(o => o is Image));

      if (this.overlay is Image)
        images.Add(this.overlay);

      List<OverlayBase> result = new List<OverlayBase>();

      result.AddRange(horizontalGuides);
      result.AddRange(verticalGuides);
      result.AddRange(images);
      this.overlays = result;
      this.overlay = null;
    }

    private bool IsHovered(OverlayBase overlay)
    {
      if (overlay is Guide)
      {
        if ((overlay as Guide).Orienation == Orienation.Horizontal && (overlay as Guide).Position >= this.mouseY - PxperWnd.precision && (overlay as Guide).Position <= this.mouseY + PxperWnd.precision)
          return true;

        if ((overlay as Guide).Orienation == Orienation.Vertical && (overlay as Guide).Position >= this.mouseX - PxperWnd.precision && (overlay as Guide).Position <= this.mouseX + PxperWnd.precision)
          return true;
      }

      else if (overlay is Image)
      {
        if (new Rectangle((overlay as Image).Position, (overlay as Image).Normal.Size).Contains(this.mouseX, this.mouseY))
          return true;
      }

      return false;
    }

    protected void ResetHover()
    {
      foreach (OverlayBase overlay in this.overlays)
        overlay.IsHovered = false;
    }

    private void MoveOverlay(OverlayBase overlay)
    {
      int xDelta;
      int yDelta;

      if (this.isControlPressed)
      {
        xDelta = this.controlKeyStartX - this.startX;
        yDelta = this.controlKeyStartY - this.startY;

        int xControlKeyDelta = this.mouseX - this.controlKeyStartX;
        int yControlKeyDelta = this.mouseY - this.controlKeyStartY;

        xControlKeyDelta = (int)Math.Round((double)xControlKeyDelta / 5.0);
        yControlKeyDelta = (int)Math.Round((double)yControlKeyDelta / 5.0);
        xDelta += xControlKeyDelta;
        yDelta += yControlKeyDelta;
      }

      else
      {
        xDelta = this.mouseX - this.startX;
        yDelta = this.mouseY - this.startY;
      }

      if (overlay is Guide)
      {
        if ((overlay as Guide).Orienation == Orienation.Horizontal)
        {
          (overlay as Guide).Position = (overlay as Guide).TempPosition + yDelta;
          Magnifier.X = this.mouseX;
          Magnifier.Y = (overlay as Guide).TempPosition + yDelta;
        }

        else if ((overlay as Guide).Orienation == Orienation.Vertical)
        {
          (overlay as Guide).Position = (overlay as Guide).TempPosition + xDelta;
          Magnifier.X = (overlay as Guide).TempPosition + xDelta;
          Magnifier.Y = this.mouseY;
        }
      }

      else if (overlay is Image)
      {
        (overlay as Image).Position = new Point(
          (overlay as Image).TempPosition.X + xDelta,
          (overlay as Image).TempPosition.Y + yDelta
        );

        Magnifier.X = (overlay as Image).TempPosition.X + xDelta;
        Magnifier.Y = (overlay as Image).TempPosition.Y + yDelta;
      }
    }

    private void DrawOverlay(Graphics g, OverlayBase overlay)
    {
      if (overlay is Image)
        this.DrawImage(g, overlay as Image);

      else if (overlay is Guide)
        this.DrawGuide(g, overlay as Guide);
    }

    private void DrawGuide(Graphics g, Guide guide)
    {
      using (Pen pen = new Pen(guide.IsHovered ? this.colorHovered : this.GetColorFromGuideColor(guide.GuideColor)))
      {
        if (guide.Orienation == Orienation.Horizontal)
          g.DrawLine(pen, 0, guide.Position, this.Width, guide.Position);

        else if (guide.Orienation == Orienation.Vertical)
          g.DrawLine(pen, guide.Position, 0, guide.Position, this.Height);
      }
    }

    private void DrawAbsRulers(Graphics g, Guide guide)
    {
      using (Pen pen = new Pen(this.colorAbsRuler))
      {
        pen.DashStyle = DashStyle.Dash;
        pen.DashPattern = new float[] { 5, 5 };
        pen.CustomStartCap = new AdjustableArrowCap(3, 3);
        pen.CustomEndCap = new AdjustableArrowCap(3, 3);

        if (guide.Orienation == Orienation.Horizontal)
        {
          g.DrawLine(pen, this.mouseX, 0, this.mouseX, guide.Position);
          g.DrawLine(pen, this.mouseX, guide.Position, this.mouseX, this.Height);
        }

        else if (guide.Orienation == Orienation.Vertical)
        {
          g.DrawLine(pen, 0, this.mouseY, guide.Position, this.mouseY);
          g.DrawLine(pen, guide.Position, this.mouseY, this.Width, this.mouseY);
        }
      }

      if (guide.Orienation == Orienation.Horizontal)
      {
        this.DrawSegmentLabel(g, this.colorAbsRuler, this.colorAbsRulerLabel, this.mouseX + PxperWnd.rulerPadding, PxperWnd.rulerPadding, guide.Position.ToString());
        this.DrawSegmentLabel(g, this.colorAbsRuler, this.colorAbsRulerLabel, this.mouseX + PxperWnd.rulerPadding, this.Height - PxperWnd.rulerPadding, (this.Height - guide.Position).ToString(), false, true);
      }

      else if (guide.Orienation == Orienation.Vertical)
      {
        this.DrawSegmentLabel(g, this.colorAbsRuler, this.colorAbsRulerLabel, PxperWnd.rulerPadding, this.mouseY + PxperWnd.rulerPadding, guide.Position.ToString());
        this.DrawSegmentLabel(g, this.colorAbsRuler, this.colorAbsRulerLabel, this.Width - PxperWnd.rulerPadding, this.mouseY + PxperWnd.rulerPadding, (this.Width - guide.Position).ToString(), true, false);
      }
    }

    private void DrawRelRulers(Graphics g, Guide guide)
    {
      Guide previousGuide = this.GetPreviousGuide();

      if (previousGuide != null)
        this.DrawPreviousRelRuler(g, guide, previousGuide);

      Guide nextGuide = this.GetNextGuide();

      if (nextGuide != null)
        this.DrawNextRelRuler(g, guide, nextGuide);
    }

    private void DrawPreviousRelRuler(Graphics g, Guide guide, Guide previousGuide)
    {
      using (Pen pen = new Pen(this.colorRelRuler))
      {
        pen.DashStyle = DashStyle.Dash;
        pen.DashPattern = new float[] { 5, 5 };
        pen.CustomStartCap = new AdjustableArrowCap(3, 3);
        pen.CustomEndCap = new AdjustableArrowCap(3, 3);

        if (guide.Orienation == Orienation.Horizontal)
        {
          g.DrawLine(pen, this.mouseX + PxperWnd.rulerSpacing, previousGuide.Position, this.mouseX + PxperWnd.rulerSpacing, guide.Position);
        }

        else if (guide.Orienation == Orienation.Vertical)
        {
          g.DrawLine(pen, previousGuide.Position, this.mouseY + PxperWnd.rulerSpacing, guide.Position, this.mouseY + PxperWnd.rulerSpacing);
        }
      }

      if (guide.Orienation == Orienation.Horizontal)
      {
        this.DrawSegmentLabel(g, this.colorRelRuler, this.colorRelRulerLabel, this.mouseX + PxperWnd.rulerSpacing + PxperWnd.rulerPadding, previousGuide.Position + PxperWnd.rulerPadding, (guide.Position - previousGuide.Position).ToString());
      }

      else if (guide.Orienation == Orienation.Vertical)
      {
        this.DrawSegmentLabel(g, this.colorRelRuler, this.colorRelRulerLabel, previousGuide.Position + PxperWnd.rulerPadding, this.mouseY + PxperWnd.rulerSpacing + PxperWnd.rulerPadding, (guide.Position - previousGuide.Position).ToString());
      }
    }

    private void DrawNextRelRuler(Graphics g, Guide guide, Guide nextGuide)
    {
      using (Pen pen = new Pen(this.colorRelRuler))
      {
        pen.DashStyle = DashStyle.Dash;
        pen.DashPattern = new float[] { 5, 5 };
        pen.CustomStartCap = new AdjustableArrowCap(3, 3);
        pen.CustomEndCap = new AdjustableArrowCap(3, 3);

        if (guide.Orienation == Orienation.Horizontal)
        {
          g.DrawLine(pen, this.mouseX + PxperWnd.rulerSpacing, guide.Position, this.mouseX + PxperWnd.rulerSpacing, nextGuide.Position);
        }

        else if (guide.Orienation == Orienation.Vertical)
        {
          g.DrawLine(pen, guide.Position, this.mouseY + PxperWnd.rulerSpacing, nextGuide.Position, this.mouseY + PxperWnd.rulerSpacing);
        }
      }

      if (guide.Orienation == Orienation.Horizontal)
      {
        this.DrawSegmentLabel(g, this.colorRelRuler, this.colorRelRulerLabel, this.mouseX + PxperWnd.rulerSpacing + PxperWnd.rulerPadding, nextGuide.Position - PxperWnd.rulerPadding, (nextGuide.Position - guide.Position).ToString(), false, true);
      }

      else if (guide.Orienation == Orienation.Vertical)
      {
        this.DrawSegmentLabel(g, this.colorRelRuler, this.colorRelRulerLabel, nextGuide.Position - PxperWnd.rulerPadding, this.mouseY + PxperWnd.rulerSpacing + PxperWnd.rulerPadding, (nextGuide.Position - guide.Position).ToString(), true, false);
      }
    }

    private void DrawImage(Graphics g, Image image)
    {
      if (image.Normal != null && image.Hovered != null)
        g.DrawImageUnscaled(image.IsHovered ? image.Hovered : image.Normal, image.Position);
    }

    private void DrawMeasure(Graphics g)
    {
      int width = this.mouseX - this.startX;

      if (width < 1)
        width = 1;

      int height = this.mouseY - this.startY;

      if (height < 1)
        height = 1;

      using (Brush brush = new SolidBrush(Color.FromArgb(100, this.colorBlue)))
        g.FillRectangle(brush, new Rectangle(this.startX, this.startY, width, height));

      this.DrawSegmentLabel(g, this.colorAbsRuler, this.colorAbsRulerLabel, Math.Max(this.startX, this.mouseX), this.startY, width.ToString(), false, false);
      this.DrawSegmentLabel(g, this.colorAbsRuler, this.colorAbsRulerLabel, this.startX, Math.Max(this.startY, this.mouseY), height.ToString(), false, false);
    }

    private Color GetColorFromGuideColor(GuideColor colorButton)
    {
      if (colorButton == GuideColor.Blue)
        return Color.FromArgb(255 - (int)(255f / 100f * Settings.Default.GuidesTransparency), 0, 255, 255);

      else if (colorButton == GuideColor.Purple)
        return Color.FromArgb(255 - (int)(255f / 100f * Settings.Default.GuidesTransparency), 255, 0, 255);

      else if (colorButton == GuideColor.Yellow)
        return Color.FromArgb(255 - (int)(255f / 100f * Settings.Default.GuidesTransparency), 255, 255, 0);

      return Color.White;
    }

    private void DrawSegmentLabel(Graphics g, Color color, Color colorLabel, int x, int y, string segmentLabel, bool invertedX = false, bool invertedY = false)
    {
      using (Font font = new Font("Verdana", 8f))
      {
        SizeF size = g.MeasureString(segmentLabel, font);

        g.SmoothingMode = SmoothingMode.None;

        using (Brush brush = new SolidBrush(color))
          g.FillRectangle(brush, invertedX ? x - size.Width : x, invertedY ? y - size.Height : y, size.Width, size.Height);

        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (Brush brush = new SolidBrush(colorLabel))
          g.DrawString(segmentLabel, font, brush, invertedX ? x - size.Width : x, invertedY ? y - size.Height : y);
      }
    }

    private Guide GetPreviousGuide()
    {
      if (!(this.overlay is Guide))
        return null;

      Guide previousGuide = null;

      foreach (OverlayBase item in this.overlays)
        if (item is Guide)
          if ((item as Guide).Orienation == (this.overlay as Guide).Orienation && (item as Guide).Position < (this.overlay as Guide).Position && (previousGuide == null || previousGuide.Position < (item as Guide).Position))
            previousGuide = item as Guide;

      return previousGuide;
    }

    private Guide GetNextGuide()
    {
      if (!(this.overlay is Guide))
        return null;

      Guide nextGuide = null;

      foreach (OverlayBase item in this.overlays)
        if (item is Guide)
          if ((item as Guide).Orienation == (this.overlay as Guide).Orienation && (item as Guide).Position > (this.overlay as Guide).Position && (nextGuide == null || nextGuide.Position > (item as Guide).Position))
            nextGuide = item as Guide;

      return nextGuide;
    }

    private string GetActiveWindowTitle()
    {
      try
      {
        IntPtr hWnd = User32.GetForegroundWindow();
        StringBuilder title = new StringBuilder(1024);

        User32.GetWindowText(hWnd, title, title.Capacity);

        return title.ToString();
      }

      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    private void AskForImageFromUser()
    {
      this.showOfdImage = false;

      DialogResult result = this.ofdImage.ShowDialog();

      this.activeImage.Normal.Dispose();
      this.activeImage.Normal = null;
      this.activeImage.Hovered.Dispose();
      this.activeImage.Hovered = null;

      if (result == DialogResult.OK)
        this.bwImageLoader.RunWorkerAsync(this.ofdImage.FileName);

      else
      {
        this.overlays.Remove(this.activeImage);
        this.activeImage = null;
      }
    }
  }
}
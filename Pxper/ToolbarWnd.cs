// Copyright © 2014 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Pxper.Properties;

namespace Pxper
{
  public delegate void ControlEventHandler(object sender, ControlEventArgs e);
  public delegate void GuildeColorEventHandler(object sender, GuideColorEventArgs e);
  public delegate void ToolEventHandler(object sender, ToolEventArgs e);

  public partial class ToolbarWnd : LayeredWndBase
  {
    private const int panelPadding = 8;
    private const int controlMargin = 4;
    private const int gripWidth = 6;
    private const int gripHeight = 42;
    private const int guideColorButtonSize = 14;
    private const int toolButtonSize = 42;
    private const int separatorWidth = 2;
    private const int separatorHeight = 50;

    private readonly Bitmap toolbar = Resources.Toolbar;
    private readonly Bitmap grip = Resources.Grip;
    private readonly Bitmap blue = Resources.Blue;
    private readonly Bitmap purple = Resources.Purple;
    private readonly Bitmap yellow = Resources.Yellow;
    private readonly Bitmap horizontalGuide = Resources.HorizontalGuide;
    private readonly Bitmap verticalGuide = Resources.VerticalGuide;
    private readonly Bitmap image = Resources.Image;
    private readonly Bitmap @default = Resources.Default;
    private readonly Bitmap measure = Resources.Measure;
    private readonly Bitmap move = Resources.Move;
    private readonly Bitmap remove = Resources.Remove;
    private readonly Bitmap @lock = Resources.Lock;
    private readonly Bitmap visibility = Resources.Visibility;
    private readonly Bitmap separator = Resources.Separator;

    private GuideColor hoveredGuideColor;
    private GuideColor pressedGuideColor;
    private GuideColor activeGuideColor;
    private Tool hoveredTool;
    private Tool pressedTool;
    private Tool mode1ActiveTool;
    private Tool mode2ActiveTool;
    private Tool mode3ActiveTool;

    public event EventHandler ToolbarClicked;
    public event ControlEventHandler ControlSelected;
    public event ControlEventHandler ControlDeselected;
    public event GuildeColorEventHandler GuideColorChanged;
    public event ToolEventHandler ToolSelected;
    public event ToolEventHandler ToolDeselected;
    public event ToolEventHandler Mode1ToolChanged;
    public event ToolEventHandler Mode2ToolChanged;
    public event ToolEventHandler Mode3ToolChanged;
    
    public ToolbarWnd()
    {
      this.hoveredGuideColor = GuideColor.None;
      this.pressedGuideColor = GuideColor.None;
      this.activeGuideColor = GuideColor.Blue;
      this.hoveredTool = Tool.None;
      this.pressedTool = Tool.None;
      this.mode1ActiveTool = Tool.Default;
      this.mode2ActiveTool = Tool.None;
      this.mode3ActiveTool = Tool.Visibility;
      this.InitializeComponent();
      this.SetLayered();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      if (e is FormClosingEventArgs)
      {
        CloseReason closeReason = (e as FormClosingEventArgs).CloseReason;

        if (closeReason == CloseReason.UserClosing)
          e.Cancel = true;

        else e.Cancel = false;
      }

      base.OnClosing(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);

      Cursor cursor = Cursors.Default;

      if (this.GetGripRectangle().Contains(e.Location))
        cursor = Cursors.SizeAll;

      this.hoveredGuideColor = GuideColor.None;

      for (int i = (int)GuideColor.None + 1; i != (int)GuideColor.Yellow + 1; i++)
        if (this.GetGuideColorButtonRectangle((GuideColor)i).Contains(e.Location) && this.mode1ActiveTool == Tool.Default)
        {
          this.hoveredGuideColor = (GuideColor)i;
          cursor = Cursors.Hand;
        }

      this.hoveredTool = Tool.None;

      for (int i = (int)Tool.None + 1; i != (int)Tool.Visibility + 1; i++)
        if (this.GetToolButtonRectangle((Tool)i).Contains(e.Location))
        {
          this.hoveredTool = (Tool)i;
          cursor = this.GetCursorByTool((Tool)i);

          if ((i == (int)Tool.HorizontalGuide || i == (int)Tool.VerticalGuide || i == (int)Tool.Image) && this.mode1ActiveTool == Tool.Default)
            if (this.ToolSelected != null)
              this.ToolSelected(this, new ToolEventArgs((Tool)i));
        }

      if (this.hoveredTool != Tool.HorizontalGuide && this.hoveredTool != Tool.VerticalGuide && this.hoveredTool != Tool.Image)
        if (this.ToolDeselected != null)
          this.ToolDeselected(this, new ToolEventArgs(Tool.None));

      this.Cursor = cursor;
      this.Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);

      if (this.ToolDeselected != null)
        this.ToolDeselected(this, new ToolEventArgs(Tool.None));

      this.Cursor = Cursors.Default;
      this.hoveredGuideColor = GuideColor.None;
      this.hoveredTool = Tool.None;
      this.Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      if (this.ToolbarClicked != null)
        this.ToolbarClicked(this, EventArgs.Empty);

      if (this.GetGripRectangle().Contains(e.Location))
        if (this.ControlSelected != null)
          this.ControlSelected(this, new ControlEventArgs(Control.Grip));

      this.pressedGuideColor = GuideColor.None;

      for (int i = (int)GuideColor.None + 1; i != (int)GuideColor.Yellow + 1; i++)
        if (this.GetGuideColorButtonRectangle((GuideColor)i).Contains(e.Location))
          this.pressedGuideColor = (GuideColor)i;

      this.pressedTool = Tool.None;

      for (int i = (int)Tool.None + 1; i != (int)Tool.Visibility + 1; i++)
        if (this.GetToolButtonRectangle((Tool)i).Contains(e.Location))
          this.pressedTool = (Tool)i;

      this.Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);

      if (this.ControlDeselected != null)
        this.ControlDeselected(this, new ControlEventArgs(Control.Grip));

      this.pressedGuideColor = GuideColor.None;

      for (int i = (int)GuideColor.Blue; i != (int)GuideColor.Yellow + 1; i++)
        if (this.GetGuideColorButtonRectangle((GuideColor)i).Contains(e.Location) && this.mode1ActiveTool == Tool.Default)
        {
          this.activeGuideColor = (GuideColor)i;

          if (this.GuideColorChanged != null)
            this.GuideColorChanged(this, new GuideColorEventArgs((GuideColor)i));
        }

      this.pressedTool = Tool.None;

      for (int i = (int)Tool.Default; i != (int)Tool.Remove + 1; i++)
        if (this.GetToolButtonRectangle((Tool)i).Contains(e.Location))
        {
          this.mode1ActiveTool = (Tool)i;

          if (this.Mode1ToolChanged != null)
            this.Mode1ToolChanged(this, new ToolEventArgs((Tool)i));
        }

      if (this.GetToolButtonRectangle(Tool.Lock).Contains(e.Location) && (this.mode1ActiveTool == Tool.Move || this.mode1ActiveTool == Tool.Remove))
      {
        this.mode2ActiveTool = this.mode2ActiveTool == Tool.None ? Tool.Lock : Tool.None;

        if (this.Mode2ToolChanged != null)
          this.Mode2ToolChanged(this, new ToolEventArgs(this.mode2ActiveTool));
      }

      if (this.GetToolButtonRectangle(Tool.Visibility).Contains(e.Location))
      {
        this.mode3ActiveTool = this.mode3ActiveTool == Tool.None ? Tool.Visibility : Tool.None;

        if (this.Mode3ToolChanged != null)
          this.Mode3ToolChanged(this, new ToolEventArgs(this.mode3ActiveTool));
      }

      this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      // Background
      e.Graphics.DrawImage(
        this.toolbar, 0, 0, this.Width, this.Height
      );

      // Grip
      e.Graphics.DrawImage(
        this.grip, this.GetGripClientRectangle()
      );

      // Color Buttons
      for (int i = (int)GuideColor.None + 1; i != (int)GuideColor.Yellow + 1; i++)
        this.DrawGuideColorButton(e.Graphics, (GuideColor)i);

      // Tool Buttons
      for (int i = (int)Tool.None + 1; i != (int)Tool.Visibility + 1; i++)
        this.DrawToolButton(e.Graphics, (Tool)i);

      // Separators
      this.DrawSeparator(e.Graphics, 0);
      this.DrawSeparator(e.Graphics, 1);
      this.DrawSeparator(e.Graphics, 2);
    }

    private Cursor GetCursorByTool(Tool tool)
    {
      if (tool == Tool.HorizontalGuide && this.mode1ActiveTool == Tool.Default)
        return Cursors.SizeNS;

      else if (tool == Tool.VerticalGuide && this.mode1ActiveTool == Tool.Default)
        return Cursors.SizeWE;

      else if (tool == Tool.Image && this.mode1ActiveTool == Tool.Default)
        return Cursors.SizeAll;

      else if (tool == Tool.Default || tool == Tool.Measure || tool == Tool.Move || tool == Tool.Remove || tool == Tool.Lock || tool == Tool.Visibility)
        return Cursors.Hand;

      return Cursors.Default;
    }

    private void DrawGuideColorButton(Graphics g, GuideColor guideColor)
    {
      g.DrawImage(this.GetGuideColorButtonImage(guideColor), this.GetGuideColorButtonRectangle(guideColor), this.GetGuideColorButtonClientRectangle(guideColor), GraphicsUnit.Pixel);
    }

    private void DrawToolButton(Graphics g, Tool tool)
    {
      g.DrawImage(this.GetToolButtonImage(tool), this.GetToolButtonRectangle(tool), this.GetToolButtonClientRectangle(tool), GraphicsUnit.Pixel);
    }

    private void DrawSeparator(Graphics g, int index)
    {
      g.DrawImage(this.separator, this.GetSeparatorRectangle(index));
    }

    private Rectangle GetGripRectangle()
    {
      return new Rectangle(
        ToolbarWnd.panelPadding,
        ToolbarWnd.panelPadding,
        ToolbarWnd.controlMargin * 2 + ToolbarWnd.gripWidth,
        ToolbarWnd.controlMargin * 2 + ToolbarWnd.gripHeight
      );
    }

    private Rectangle GetGripClientRectangle()
    {
      return new Rectangle(
        ToolbarWnd.panelPadding + ToolbarWnd.controlMargin,
        ToolbarWnd.panelPadding + ToolbarWnd.controlMargin,
        ToolbarWnd.gripWidth,
        ToolbarWnd.gripHeight
      );
    }

    private Rectangle GetGuideColorButtonRectangle(GuideColor guideColor)
    {
      return new Rectangle(
        ToolbarWnd.panelPadding + ToolbarWnd.controlMargin * 2 + ToolbarWnd.gripWidth,
        ToolbarWnd.panelPadding + ToolbarWnd.controlMargin + ToolbarWnd.guideColorButtonSize * (int)guideColor,
        ToolbarWnd.guideColorButtonSize,
        ToolbarWnd.guideColorButtonSize
      );
    }

    private Rectangle GetToolButtonRectangle(Tool tool)
    {
      int left = 0;

      if (tool == Tool.HorizontalGuide)
        left = ToolbarWnd.controlMargin * 3 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize;

      else if (tool == Tool.VerticalGuide)
        left = ToolbarWnd.controlMargin * 3 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize;

      else if (tool == Tool.Image)
        left = ToolbarWnd.controlMargin * 3 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 2;

      else if (tool == Tool.Default)
        left = ToolbarWnd.controlMargin * 5 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 3 + ToolbarWnd.separatorWidth;

      else if (tool == Tool.Measure)
        left = ToolbarWnd.controlMargin * 5 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 4 + ToolbarWnd.separatorWidth;

      else if (tool == Tool.Move)
        left = ToolbarWnd.controlMargin * 5 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 5 + ToolbarWnd.separatorWidth;

      else if (tool == Tool.Remove)
        left = ToolbarWnd.controlMargin * 5 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 6 + ToolbarWnd.separatorWidth;

      else if (tool == Tool.Lock)
        left = ToolbarWnd.controlMargin * 7 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 7 + ToolbarWnd.separatorWidth * 2;

      else if (tool == Tool.Visibility)
        left = ToolbarWnd.controlMargin * 9 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 8 + ToolbarWnd.separatorWidth * 3;

      return new Rectangle(
        ToolbarWnd.panelPadding + left,
        ToolbarWnd.panelPadding + ToolbarWnd.controlMargin,
        ToolbarWnd.toolButtonSize,
        ToolbarWnd.toolButtonSize
      );
    }

    private Rectangle GetGuideColorButtonClientRectangle(GuideColor guideColor)
    {
      if (this.mode1ActiveTool != Tool.Default)
        return new Rectangle(
          0,
          ToolbarWnd.guideColorButtonSize * 3,
          ToolbarWnd.guideColorButtonSize,
          ToolbarWnd.guideColorButtonSize
        );

      return new Rectangle(
        0,
        (this.pressedGuideColor == guideColor || this.activeGuideColor == guideColor) ? ToolbarWnd.guideColorButtonSize * 2 : this.hoveredGuideColor == guideColor ? ToolbarWnd.guideColorButtonSize : 0,
        ToolbarWnd.guideColorButtonSize,
        ToolbarWnd.guideColorButtonSize
      );
    }

    private Rectangle GetToolButtonClientRectangle(Tool tool)
    {
      if ((tool == Tool.HorizontalGuide || tool == Tool.VerticalGuide || tool == Tool.Image) && this.mode1ActiveTool != Tool.Default)
        return new Rectangle(
          0,
          ToolbarWnd.toolButtonSize * 3,
          ToolbarWnd.toolButtonSize,
          ToolbarWnd.toolButtonSize
        );

      if ((tool == Tool.Lock) && (this.mode1ActiveTool != Tool.Move && this.mode1ActiveTool != Tool.Remove))
        return new Rectangle(
          0,
          ToolbarWnd.toolButtonSize * 3,
          ToolbarWnd.toolButtonSize,
          ToolbarWnd.toolButtonSize
        );

      return new Rectangle(
        0,
        (this.pressedTool == tool || this.mode1ActiveTool == tool || this.mode2ActiveTool == tool || this.mode3ActiveTool == tool) ? ToolbarWnd.toolButtonSize * 2 : this.hoveredTool == tool ? ToolbarWnd.toolButtonSize : 0,
        ToolbarWnd.toolButtonSize,
        ToolbarWnd.toolButtonSize
      );
    }

    private Bitmap GetGuideColorButtonImage(GuideColor guideColor)
    {
      if (guideColor == GuideColor.Blue)
        return this.blue;

      if (guideColor == GuideColor.Purple)
        return this.purple;

      if (guideColor == GuideColor.Yellow)
        return this.yellow;

      return null;
    }

    private Bitmap GetToolButtonImage(Tool tool)
    {
      if (tool == Tool.HorizontalGuide)
        return this.horizontalGuide;

      if (tool == Tool.VerticalGuide)
        return this.verticalGuide;

      if (tool == Tool.Image)
        return this.image;

      if (tool == Tool.Default)
        return this.@default;

      if (tool == Tool.Measure)
        return this.measure;

      if (tool == Tool.Move)
        return this.move;

      if (tool == Tool.Remove)
        return this.remove;

      if (tool == Tool.Lock)
        return this.@lock;

      if (tool == Tool.Visibility)
        return this.visibility;

      return null;
    }

    private Rectangle GetSeparatorRectangle(int index)
    {
      int left = 0;

      if (index == 0)
        left = ToolbarWnd.controlMargin * 4 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 3;

      else if (index == 1)
        left = ToolbarWnd.controlMargin * 6 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 7 + ToolbarWnd.separatorWidth;

      else if (index == 2)
        left = ToolbarWnd.controlMargin * 8 + ToolbarWnd.gripWidth + ToolbarWnd.guideColorButtonSize + ToolbarWnd.toolButtonSize * 8 + ToolbarWnd.separatorWidth * 2;

      return new Rectangle(
        ToolbarWnd.panelPadding + left,
        ToolbarWnd.panelPadding,
        ToolbarWnd.separatorWidth,
        ToolbarWnd.separatorHeight
      );
    }
  }
}
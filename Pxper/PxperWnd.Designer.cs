namespace Pxper
{
  partial class PxperWnd
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PxperWnd));
      this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.cmiToggleToolbarWnd = new System.Windows.Forms.ToolStripMenuItem();
      this.cms0 = new System.Windows.Forms.ToolStripSeparator();
      this.cmiClear = new System.Windows.Forms.ToolStripMenuItem();
      this.cms1 = new System.Windows.Forms.ToolStripSeparator();
      this.cmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiOpen = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiOpenFromPsd = new System.Windows.Forms.ToolStripMenuItem();
      this.cms2 = new System.Windows.Forms.ToolStripSeparator();
      this.cmiGrid = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiHorizontals = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiVerticals = new System.Windows.Forms.ToolStripMenuItem();
      this.cms3 = new System.Windows.Forms.ToolStripSeparator();
      this.cmiSettings = new System.Windows.Forms.ToolStripMenuItem();
      this.cms4 = new System.Windows.Forms.ToolStripSeparator();
      this.cmiTooltips = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiAbout = new System.Windows.Forms.ToolStripMenuItem();
      this.cms5 = new System.Windows.Forms.ToolStripSeparator();
      this.cmiExit = new System.Windows.Forms.ToolStripMenuItem();
      this.ni = new System.Windows.Forms.NotifyIcon(this.components);
      this.ofdPsd = new System.Windows.Forms.OpenFileDialog();
      this.ofd = new System.Windows.Forms.OpenFileDialog();
      this.sfd = new System.Windows.Forms.SaveFileDialog();
      this.tSystem = new System.Windows.Forms.Timer(this.components);
      this.tInvalidate = new System.Windows.Forms.Timer(this.components);
      this.ofdImage = new System.Windows.Forms.OpenFileDialog();
      this.bwImageLoader = new System.ComponentModel.BackgroundWorker();
      this.cms.SuspendLayout();
      this.SuspendLayout();
      // 
      // cms
      // 
      this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiToggleToolbarWnd,
            this.cms0,
            this.cmiClear,
            this.cms1,
            this.cmiSaveAs,
            this.cmiOpen,
            this.cmiOpenFromPsd,
            this.cms2,
            this.cmiGrid,
            this.cmiHorizontals,
            this.cmiVerticals,
            this.cms3,
            this.cmiSettings,
            this.cms4,
            this.cmiTooltips,
            this.cmiAbout,
            this.cms5,
            this.cmiExit});
      this.cms.Name = "cms";
      resources.ApplyResources(this.cms, "cms");
      // 
      // cmiToggleToolbarWnd
      // 
      this.cmiToggleToolbarWnd.Name = "cmiToggleToolbarWnd";
      resources.ApplyResources(this.cmiToggleToolbarWnd, "cmiToggleToolbarWnd");
      this.cmiToggleToolbarWnd.Click += new System.EventHandler(this.ni_DoubleClick);
      // 
      // cms0
      // 
      this.cms0.Name = "cms0";
      resources.ApplyResources(this.cms0, "cms0");
      // 
      // cmiClear
      // 
      this.cmiClear.Name = "cmiClear";
      resources.ApplyResources(this.cmiClear, "cmiClear");
      this.cmiClear.Click += new System.EventHandler(this.cmiClear_Click);
      // 
      // cms1
      // 
      this.cms1.Name = "cms1";
      resources.ApplyResources(this.cms1, "cms1");
      // 
      // cmiSaveAs
      // 
      this.cmiSaveAs.Name = "cmiSaveAs";
      resources.ApplyResources(this.cmiSaveAs, "cmiSaveAs");
      this.cmiSaveAs.Click += new System.EventHandler(this.cmiSaveAs_Click);
      // 
      // cmiOpen
      // 
      this.cmiOpen.Name = "cmiOpen";
      resources.ApplyResources(this.cmiOpen, "cmiOpen");
      this.cmiOpen.Click += new System.EventHandler(this.cmiOpen_Click);
      // 
      // cmiOpenFromPsd
      // 
      this.cmiOpenFromPsd.Name = "cmiOpenFromPsd";
      resources.ApplyResources(this.cmiOpenFromPsd, "cmiOpenFromPsd");
      this.cmiOpenFromPsd.Click += new System.EventHandler(this.cmiOpenFromPsd_Click);
      // 
      // cms2
      // 
      this.cms2.Name = "cms2";
      resources.ApplyResources(this.cms2, "cms2");
      // 
      // cmiGrid
      // 
      this.cmiGrid.Name = "cmiGrid";
      resources.ApplyResources(this.cmiGrid, "cmiGrid");
      this.cmiGrid.Click += new System.EventHandler(this.cmiGrid_Click);
      // 
      // cmiHorizontals
      // 
      this.cmiHorizontals.Name = "cmiHorizontals";
      resources.ApplyResources(this.cmiHorizontals, "cmiHorizontals");
      this.cmiHorizontals.Click += new System.EventHandler(this.cmiHorizontals_Click);
      // 
      // cmiVerticals
      // 
      this.cmiVerticals.Name = "cmiVerticals";
      resources.ApplyResources(this.cmiVerticals, "cmiVerticals");
      this.cmiVerticals.Click += new System.EventHandler(this.cmiVerticals_Click);
      // 
      // cms3
      // 
      this.cms3.Name = "cms3";
      resources.ApplyResources(this.cms3, "cms3");
      // 
      // cmiSettings
      // 
      this.cmiSettings.Name = "cmiSettings";
      resources.ApplyResources(this.cmiSettings, "cmiSettings");
      this.cmiSettings.Click += new System.EventHandler(this.cmiSettings_Click);
      // 
      // cms4
      // 
      this.cms4.Name = "cms4";
      resources.ApplyResources(this.cms4, "cms4");
      // 
      // cmiTooltips
      // 
      this.cmiTooltips.Name = "cmiTooltips";
      resources.ApplyResources(this.cmiTooltips, "cmiTooltips");
      this.cmiTooltips.Click += new System.EventHandler(this.cmiTooltips_Click);
      // 
      // cmiAbout
      // 
      this.cmiAbout.Name = "cmiAbout";
      resources.ApplyResources(this.cmiAbout, "cmiAbout");
      this.cmiAbout.Click += new System.EventHandler(this.cmiAbout_Click);
      // 
      // cms5
      // 
      this.cms5.Name = "cms5";
      resources.ApplyResources(this.cms5, "cms5");
      // 
      // cmiExit
      // 
      this.cmiExit.Name = "cmiExit";
      resources.ApplyResources(this.cmiExit, "cmiExit");
      this.cmiExit.Click += new System.EventHandler(this.cmiExit_Click);
      // 
      // ni
      // 
      this.ni.ContextMenuStrip = this.cms;
      resources.ApplyResources(this.ni, "ni");
      this.ni.DoubleClick += new System.EventHandler(this.ni_DoubleClick);
      // 
      // ofdPsd
      // 
      this.ofdPsd.DefaultExt = "psd";
      resources.ApplyResources(this.ofdPsd, "ofdPsd");
      // 
      // ofd
      // 
      this.ofd.DefaultExt = "pxper";
      resources.ApplyResources(this.ofd, "ofd");
      // 
      // sfd
      // 
      this.sfd.DefaultExt = "pxper";
      resources.ApplyResources(this.sfd, "sfd");
      // 
      // tSystem
      // 
      this.tSystem.Enabled = true;
      this.tSystem.Interval = 40;
      this.tSystem.Tick += new System.EventHandler(this.tSystem_Tick);
      // 
      // tInvalidate
      // 
      this.tInvalidate.Tick += new System.EventHandler(this.tInvalidate_Tick);
      // 
      // ofdImage
      // 
      resources.ApplyResources(this.ofdImage, "ofdImage");
      // 
      // bwBitmapLoader
      // 
      this.bwImageLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwImageLoader_DoWork);
      this.bwImageLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwImageLoader_RunWorkerCompleted);
      // 
      // PxperWnd
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      resources.ApplyResources(this, "$this");
      this.ControlBox = false;
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PxperWnd";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.TopMost = true;
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.cms.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ContextMenuStrip cms;
    private System.Windows.Forms.NotifyIcon ni;
    private System.Windows.Forms.ToolStripMenuItem cmiExit;
    private System.Windows.Forms.ToolStripMenuItem cmiClear;
    private System.Windows.Forms.ToolStripSeparator cms1;
    private System.Windows.Forms.ToolStripMenuItem cmiSaveAs;
    private System.Windows.Forms.ToolStripMenuItem cmiOpen;
    private System.Windows.Forms.ToolStripSeparator cms2;
    private System.Windows.Forms.ToolStripMenuItem cmiGrid;
    private System.Windows.Forms.ToolStripSeparator cms3;
    private System.Windows.Forms.ToolStripMenuItem cmiAbout;
    private System.Windows.Forms.ToolStripSeparator cms4;
    private System.Windows.Forms.ToolStripMenuItem cmiVerticals;
    private System.Windows.Forms.ToolStripMenuItem cmiHorizontals;
    private System.Windows.Forms.ToolStripMenuItem cmiOpenFromPsd;
    private System.Windows.Forms.OpenFileDialog ofdPsd;
    private System.Windows.Forms.OpenFileDialog ofd;
    private System.Windows.Forms.SaveFileDialog sfd;
    private System.Windows.Forms.ToolStripMenuItem cmiSettings;
    private System.Windows.Forms.ToolStripSeparator cms5;
    private System.Windows.Forms.Timer tSystem;
    private System.Windows.Forms.Timer tInvalidate;
    private System.Windows.Forms.OpenFileDialog ofdImage;
    private System.Windows.Forms.ToolStripMenuItem cmiTooltips;
    private System.Windows.Forms.ToolStripMenuItem cmiToggleToolbarWnd;
    private System.Windows.Forms.ToolStripSeparator cms0;
    private System.ComponentModel.BackgroundWorker bwImageLoader;
  }
}
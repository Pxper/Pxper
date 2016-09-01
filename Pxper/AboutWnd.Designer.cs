namespace Pxper
{
  partial class AboutWnd
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWnd));
      this.bClose = new System.Windows.Forms.Button();
      this.lProgramName = new System.Windows.Forms.Label();
      this.lAuthorAndDeveloper = new System.Windows.Forms.Label();
      this.lPhone = new System.Windows.Forms.Label();
      this.lEMail = new System.Windows.Forms.Label();
      this.llEMail = new System.Windows.Forms.LinkLabel();
      this.lWebsite = new System.Windows.Forms.Label();
      this.llWebsite = new System.Windows.Forms.LinkLabel();
      this.lProgramWebsite = new System.Windows.Forms.Label();
      this.llProgramWebsite = new System.Windows.Forms.LinkLabel();
      this.SuspendLayout();
      // 
      // bClose
      // 
      this.bClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      resources.ApplyResources(this.bClose, "bClose");
      this.bClose.Name = "bClose";
      this.bClose.UseVisualStyleBackColor = true;
      // 
      // lProgramName
      // 
      resources.ApplyResources(this.lProgramName, "lProgramName");
      this.lProgramName.Name = "lProgramName";
      // 
      // lAuthorAndDeveloper
      // 
      resources.ApplyResources(this.lAuthorAndDeveloper, "lAuthorAndDeveloper");
      this.lAuthorAndDeveloper.Name = "lAuthorAndDeveloper";
      // 
      // lPhone
      // 
      resources.ApplyResources(this.lPhone, "lPhone");
      this.lPhone.Name = "lPhone";
      // 
      // lEMail
      // 
      resources.ApplyResources(this.lEMail, "lEMail");
      this.lEMail.Name = "lEMail";
      // 
      // llEMail
      // 
      resources.ApplyResources(this.llEMail, "llEMail");
      this.llEMail.Name = "llEMail";
      this.llEMail.TabStop = true;
      this.llEMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llEMail_LinkClicked);
      // 
      // lWebsite
      // 
      resources.ApplyResources(this.lWebsite, "lWebsite");
      this.lWebsite.Name = "lWebsite";
      // 
      // llWebsite
      // 
      resources.ApplyResources(this.llWebsite, "llWebsite");
      this.llWebsite.Name = "llWebsite";
      this.llWebsite.TabStop = true;
      this.llWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWebsite_LinkClicked);
      // 
      // lProgramWebsite
      // 
      resources.ApplyResources(this.lProgramWebsite, "lProgramWebsite");
      this.lProgramWebsite.Name = "lProgramWebsite";
      // 
      // llProgramWebsite
      // 
      resources.ApplyResources(this.llProgramWebsite, "llProgramWebsite");
      this.llProgramWebsite.Name = "llProgramWebsite";
      this.llProgramWebsite.TabStop = true;
      this.llProgramWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llProgramWebsite_LinkClicked);
      // 
      // AboutWnd
      // 
      this.AcceptButton = this.bClose;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.bClose;
      this.Controls.Add(this.llProgramWebsite);
      this.Controls.Add(this.lProgramWebsite);
      this.Controls.Add(this.llWebsite);
      this.Controls.Add(this.lWebsite);
      this.Controls.Add(this.llEMail);
      this.Controls.Add(this.lEMail);
      this.Controls.Add(this.lPhone);
      this.Controls.Add(this.lAuthorAndDeveloper);
      this.Controls.Add(this.lProgramName);
      this.Controls.Add(this.bClose);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutWnd";
      this.ShowInTaskbar = false;
      this.TopMost = true;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button bClose;
    private System.Windows.Forms.Label lProgramName;
    private System.Windows.Forms.Label lAuthorAndDeveloper;
    private System.Windows.Forms.Label lPhone;
    private System.Windows.Forms.Label lEMail;
    private System.Windows.Forms.LinkLabel llEMail;
    private System.Windows.Forms.Label lWebsite;
    private System.Windows.Forms.LinkLabel llWebsite;
    private System.Windows.Forms.Label lProgramWebsite;
    private System.Windows.Forms.LinkLabel llProgramWebsite;
  }
}
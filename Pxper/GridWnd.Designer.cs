namespace Pxper
{
  partial class GridWnd
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridWnd));
      this.lHorizontalInterval = new System.Windows.Forms.Label();
      this.nudHorizontalInterval = new System.Windows.Forms.NumericUpDown();
      this.lVerticalInterval = new System.Windows.Forms.Label();
      this.nudVerticalInterval = new System.Windows.Forms.NumericUpDown();
      this.bOk = new System.Windows.Forms.Button();
      this.bCancel = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.nudHorizontalInterval)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudVerticalInterval)).BeginInit();
      this.SuspendLayout();
      // 
      // lHorizontalInterval
      // 
      resources.ApplyResources(this.lHorizontalInterval, "lHorizontalInterval");
      this.lHorizontalInterval.Name = "lHorizontalInterval";
      // 
      // nudHorizontalInterval
      // 
      resources.ApplyResources(this.nudHorizontalInterval, "nudHorizontalInterval");
      this.nudHorizontalInterval.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.nudHorizontalInterval.Name = "nudHorizontalInterval";
      this.nudHorizontalInterval.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      // 
      // lVerticalInterval
      // 
      resources.ApplyResources(this.lVerticalInterval, "lVerticalInterval");
      this.lVerticalInterval.Name = "lVerticalInterval";
      // 
      // nudVerticalInterval
      // 
      resources.ApplyResources(this.nudVerticalInterval, "nudVerticalInterval");
      this.nudVerticalInterval.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.nudVerticalInterval.Name = "nudVerticalInterval";
      this.nudVerticalInterval.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
      // 
      // bOk
      // 
      resources.ApplyResources(this.bOk, "bOk");
      this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.bOk.Name = "bOk";
      this.bOk.UseVisualStyleBackColor = true;
      // 
      // bCancel
      // 
      resources.ApplyResources(this.bCancel, "bCancel");
      this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.bCancel.Name = "bCancel";
      this.bCancel.UseVisualStyleBackColor = true;
      // 
      // GridWnd
      // 
      this.AcceptButton = this.bOk;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.bCancel;
      this.Controls.Add(this.bCancel);
      this.Controls.Add(this.bOk);
      this.Controls.Add(this.nudVerticalInterval);
      this.Controls.Add(this.lVerticalInterval);
      this.Controls.Add(this.nudHorizontalInterval);
      this.Controls.Add(this.lHorizontalInterval);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GridWnd";
      this.ShowInTaskbar = false;
      this.TopMost = true;
      ((System.ComponentModel.ISupportInitialize)(this.nudHorizontalInterval)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudVerticalInterval)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lHorizontalInterval;
    private System.Windows.Forms.NumericUpDown nudHorizontalInterval;
    private System.Windows.Forms.Label lVerticalInterval;
    private System.Windows.Forms.NumericUpDown nudVerticalInterval;
    private System.Windows.Forms.Button bOk;
    private System.Windows.Forms.Button bCancel;
  }
}
namespace BrokenEvent.Match3Demo
{
  partial class MainForm
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
      BrokenEvent.Match3Demo.MeterSegment meterSegment1 = new BrokenEvent.Match3Demo.MeterSegment();
      BrokenEvent.Match3Demo.MeterSegment meterSegment2 = new BrokenEvent.Match3Demo.MeterSegment();
      BrokenEvent.Match3Demo.MeterSegment meterSegment3 = new BrokenEvent.Match3Demo.MeterSegment();
      BrokenEvent.Match3Demo.MeterSegment meterSegment4 = new BrokenEvent.Match3Demo.MeterSegment();
      BrokenEvent.Match3Demo.MeterSegment meterSegment5 = new BrokenEvent.Match3Demo.MeterSegment();
      BrokenEvent.Match3Demo.MeterSegment meterSegment6 = new BrokenEvent.Match3Demo.MeterSegment();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.slblComment = new System.Windows.Forms.ToolStripStatusLabel();
      this.tmAnimation = new System.Windows.Forms.Timer(this.components);
      this.lblPlayerHP = new System.Windows.Forms.Label();
      this.lblEnemyHP = new System.Windows.Forms.Label();
      this.lblPlayerDesc = new System.Windows.Forms.Label();
      this.lblEnemyDesc = new System.Windows.Forms.Label();
      this.mEnemy = new BrokenEvent.Match3Demo.Meter();
      this.mPlayer = new BrokenEvent.Match3Demo.Meter();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slblComment});
      this.statusStrip1.Location = new System.Drawing.Point(0, 321);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(578, 22);
      this.statusStrip1.TabIndex = 1;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // slblComment
      // 
      this.slblComment.Name = "slblComment";
      this.slblComment.Size = new System.Drawing.Size(118, 17);
      this.slblComment.Text = "toolStripStatusLabel1";
      // 
      // tmAnimation
      // 
      this.tmAnimation.Enabled = true;
      this.tmAnimation.Interval = 25;
      this.tmAnimation.Tick += new System.EventHandler(this.tmAnimation_Tick);
      // 
      // lblPlayerHP
      // 
      this.lblPlayerHP.AutoSize = true;
      this.lblPlayerHP.Location = new System.Drawing.Point(12, 38);
      this.lblPlayerHP.Name = "lblPlayerHP";
      this.lblPlayerHP.Size = new System.Drawing.Size(35, 13);
      this.lblPlayerHP.TabIndex = 4;
      this.lblPlayerHP.Text = "label1";
      // 
      // lblEnemyHP
      // 
      this.lblEnemyHP.AutoSize = true;
      this.lblEnemyHP.Location = new System.Drawing.Point(462, 38);
      this.lblEnemyHP.Name = "lblEnemyHP";
      this.lblEnemyHP.Size = new System.Drawing.Size(35, 13);
      this.lblEnemyHP.TabIndex = 5;
      this.lblEnemyHP.Text = "label1";
      // 
      // lblPlayerDesc
      // 
      this.lblPlayerDesc.AutoSize = true;
      this.lblPlayerDesc.Location = new System.Drawing.Point(11, 51);
      this.lblPlayerDesc.Name = "lblPlayerDesc";
      this.lblPlayerDesc.Size = new System.Drawing.Size(36, 13);
      this.lblPlayerDesc.TabIndex = 8;
      this.lblPlayerDesc.Text = "Player";
      // 
      // lblEnemyDesc
      // 
      this.lblEnemyDesc.AutoSize = true;
      this.lblEnemyDesc.Location = new System.Drawing.Point(462, 51);
      this.lblEnemyDesc.Name = "lblEnemyDesc";
      this.lblEnemyDesc.Size = new System.Drawing.Size(17, 13);
      this.lblEnemyDesc.TabIndex = 9;
      this.lblEnemyDesc.Text = "AI";
      // 
      // mEnemy
      // 
      this.mEnemy.BackColorBottom = System.Drawing.Color.Black;
      this.mEnemy.BackColorTop = System.Drawing.Color.Gray;
      this.mEnemy.Distance = 2;
      this.mEnemy.InnerPadding = new System.Windows.Forms.Padding(2);
      this.mEnemy.Invert = false;
      this.mEnemy.LineWidth = 2;
      this.mEnemy.Location = new System.Drawing.Point(432, 12);
      this.mEnemy.Name = "mEnemy";
      meterSegment1.ActiveColor = System.Drawing.Color.Red;
      meterSegment1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      meterSegment1.Percent = 0.3F;
      meterSegment2.ActiveColor = System.Drawing.Color.Yellow;
      meterSegment2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
      meterSegment2.Percent = 0.6F;
      meterSegment3.ActiveColor = System.Drawing.Color.Lime;
      meterSegment3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
      meterSegment3.Percent = 1F;
      this.mEnemy.Segments.Add(meterSegment1);
      this.mEnemy.Segments.Add(meterSegment2);
      this.mEnemy.Segments.Add(meterSegment3);
      this.mEnemy.Size = new System.Drawing.Size(24, 298);
      this.mEnemy.TabIndex = 7;
      this.mEnemy.Text = "meter2";
      this.mEnemy.Value = 50F;
      this.mEnemy.Vertical = true;
      // 
      // mPlayer
      // 
      this.mPlayer.BackColorBottom = System.Drawing.Color.Black;
      this.mPlayer.BackColorTop = System.Drawing.Color.Gray;
      this.mPlayer.Distance = 2;
      this.mPlayer.InnerPadding = new System.Windows.Forms.Padding(2);
      this.mPlayer.Invert = false;
      this.mPlayer.LineWidth = 2;
      this.mPlayer.Location = new System.Drawing.Point(86, 12);
      this.mPlayer.Name = "mPlayer";
      meterSegment4.ActiveColor = System.Drawing.Color.Red;
      meterSegment4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      meterSegment4.Percent = 0.3F;
      meterSegment5.ActiveColor = System.Drawing.Color.Yellow;
      meterSegment5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
      meterSegment5.Percent = 0.6F;
      meterSegment6.ActiveColor = System.Drawing.Color.Lime;
      meterSegment6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
      meterSegment6.Percent = 1F;
      this.mPlayer.Segments.Add(meterSegment4);
      this.mPlayer.Segments.Add(meterSegment5);
      this.mPlayer.Segments.Add(meterSegment6);
      this.mPlayer.Size = new System.Drawing.Size(24, 298);
      this.mPlayer.TabIndex = 6;
      this.mPlayer.Text = "meter1";
      this.mPlayer.Value = 50F;
      this.mPlayer.Vertical = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(578, 343);
      this.Controls.Add(this.lblEnemyDesc);
      this.Controls.Add(this.lblPlayerDesc);
      this.Controls.Add(this.mEnemy);
      this.Controls.Add(this.mPlayer);
      this.Controls.Add(this.lblEnemyHP);
      this.Controls.Add(this.lblPlayerHP);
      this.Controls.Add(this.statusStrip1);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.Text = "Match3Demo";
      this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel slblComment;
    private System.Windows.Forms.Timer tmAnimation;
    private System.Windows.Forms.Label lblPlayerHP;
    private System.Windows.Forms.Label lblEnemyHP;
    private BrokenEvent.Match3Demo.Meter mPlayer;
    private BrokenEvent.Match3Demo.Meter mEnemy;
    private System.Windows.Forms.Label lblPlayerDesc;
    private System.Windows.Forms.Label lblEnemyDesc;
  }
}


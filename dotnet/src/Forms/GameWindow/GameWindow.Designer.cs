namespace Pong
{
  partial class GameWindow<GameType, BallType, PaddleType> 
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      pictureBox = new PictureBox();
      tableLayoutPanel1 = new TableLayoutPanel();
      topMessage = new Label();
      bottomMessage = new Label();
      scoreLeft = new Label();
      scoreRight = new Label();
      timer1 = new System.Windows.Forms.Timer(components);
      ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
      pictureBox.SuspendLayout();
      tableLayoutPanel1.SuspendLayout();
      SuspendLayout();
      // 
      // pictureBox
      // 
      pictureBox.Controls.Add(tableLayoutPanel1);
      pictureBox.Dock = DockStyle.Fill;
      pictureBox.Location = new Point(0, 0);
      pictureBox.Name = "pictureBox";
      pictureBox.Size = new Size(784, 561);
      pictureBox.TabIndex = 0;
      pictureBox.TabStop = false;
      pictureBox.Paint += OnPaint;
      // 
      // tableLayoutPanel1
      // 
      tableLayoutPanel1.BackColor = Color.Transparent;
      tableLayoutPanel1.ColumnCount = 3;
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
      tableLayoutPanel1.Controls.Add(topMessage, 1, 0);
      tableLayoutPanel1.Controls.Add(bottomMessage, 1, 2);
      tableLayoutPanel1.Controls.Add(scoreLeft, 0, 1);
      tableLayoutPanel1.Controls.Add(scoreRight, 2, 1);
      tableLayoutPanel1.Dock = DockStyle.Fill;
      tableLayoutPanel1.Location = new Point(0, 0);
      tableLayoutPanel1.Name = "tableLayoutPanel1";
      tableLayoutPanel1.RowCount = 3;
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      tableLayoutPanel1.Size = new Size(784, 561);
      tableLayoutPanel1.TabIndex = 1;
      // 
      // topMessage
      // 
      topMessage.Anchor = AnchorStyles.Bottom;
      topMessage.AutoSize = true;
      topMessage.Font = new Font("Cascadia Code", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
      topMessage.ForeColor = Color.DeepPink;
      topMessage.Location = new Point(284, 212);
      topMessage.Name = "topMessage";
      topMessage.Size = new Size(216, 28);
      topMessage.TabIndex = 2;
      topMessage.Text = "Some message here";
      // 
      // bottomMessage
      // 
      bottomMessage.Anchor = AnchorStyles.Top;
      bottomMessage.AutoSize = true;
      bottomMessage.Font = new Font("Cascadia Code", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
      bottomMessage.ForeColor = Color.DeepPink;
      bottomMessage.Location = new Point(284, 212);
      bottomMessage.Name = "bottomMessage";
      bottomMessage.Size = new Size(216, 28);
      bottomMessage.TabIndex = 3;
      bottomMessage.Text = "Some message here";
      // 
      // scoreLeft
      // 
      scoreLeft.Anchor = AnchorStyles.None;
      scoreLeft.AutoSize = true;
      scoreLeft.Font = new Font("Cascadia Code", 24F, FontStyle.Regular, GraphicsUnit.Point);
      scoreLeft.ForeColor = Color.DeepPink;
      scoreLeft.Location = new Point(21, 258);
      scoreLeft.Name = "scoreLeft";
      scoreLeft.Size = new Size(38, 43);
      scoreLeft.TabIndex = 0;
      scoreLeft.Text = "0";
      // 
      // scoreRight
      // 
      scoreRight.Anchor = AnchorStyles.None;
      scoreRight.AutoSize = true;
      scoreRight.Font = new Font("Cascadia Code", 24F, FontStyle.Regular, GraphicsUnit.Point);
      scoreRight.ForeColor = Color.DeepPink;
      scoreRight.Location = new Point(725, 258);
      scoreRight.Name = "scoreRight";
      scoreRight.Size = new Size(38, 43);
      scoreRight.TabIndex = 1;
      scoreRight.Text = "0";
      // 
      // timer1
      // 
      timer1.Enabled = true;
      timer1.Interval = 50;
      timer1.Tick += OnTick;
      // 
      // GameWindow
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(784, 561);
      Controls.Add(pictureBox);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      Name = "GameWindow";
      Text = "Pong";
      FormClosed += OnClose;
      KeyDown += OnKeyDown;
      KeyUp += OnKeyUp;
      ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
      pictureBox.ResumeLayout(false);
      tableLayoutPanel1.ResumeLayout(false);
      tableLayoutPanel1.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    protected PictureBox pictureBox;
    protected System.Windows.Forms.Timer timer1;
    protected TableLayoutPanel tableLayoutPanel1;
    protected Label scoreLeft;
    protected Label scoreRight;
    protected Label topMessage;
    protected Label bottomMessage;
  }
}
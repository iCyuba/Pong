namespace Pong
{
  partial class Menu
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
      components = new System.ComponentModel.Container();
      nameLabel = new Label();
      tableLayoutPanel1 = new TableLayoutPanel();
      tableLayoutPanel2 = new TableLayoutPanel();
      botEasy = new Button();
      botMedium = new Button();
      botHard = new Button();
      tableLayoutPanel3 = new TableLayoutPanel();
      multiplayerLocal = new Button();
      multiplayerOnline = new Button();
      backgroundGame = new PictureBox();
      backgroundGameTimer = new System.Windows.Forms.Timer(components);
      tableLayoutPanel1.SuspendLayout();
      tableLayoutPanel2.SuspendLayout();
      tableLayoutPanel3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)backgroundGame).BeginInit();
      backgroundGame.SuspendLayout();
      SuspendLayout();
      // 
      // nameLabel
      // 
      nameLabel.AutoSize = true;
      nameLabel.BackColor = Color.Transparent;
      nameLabel.Dock = DockStyle.Fill;
      nameLabel.Font = new Font("Cascadia Code", 96F, FontStyle.Bold, GraphicsUnit.Point);
      nameLabel.ForeColor = Color.DeepPink;
      nameLabel.Location = new Point(43, 0);
      nameLabel.Name = "nameLabel";
      nameLabel.Size = new Size(698, 321);
      nameLabel.TabIndex = 0;
      nameLabel.Text = "PONG";
      nameLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // tableLayoutPanel1
      // 
      tableLayoutPanel1.BackColor = Color.Transparent;
      tableLayoutPanel1.ColumnCount = 3;
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
      tableLayoutPanel1.Controls.Add(nameLabel, 1, 0);
      tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
      tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 2);
      tableLayoutPanel1.Dock = DockStyle.Fill;
      tableLayoutPanel1.Location = new Point(0, 0);
      tableLayoutPanel1.Name = "tableLayoutPanel1";
      tableLayoutPanel1.RowCount = 4;
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
      tableLayoutPanel1.Size = new Size(784, 561);
      tableLayoutPanel1.TabIndex = 0;
      // 
      // tableLayoutPanel2
      // 
      tableLayoutPanel2.BackColor = Color.Transparent;
      tableLayoutPanel2.ColumnCount = 3;
      tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
      tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
      tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
      tableLayoutPanel2.Controls.Add(botEasy, 0, 0);
      tableLayoutPanel2.Controls.Add(botMedium, 1, 0);
      tableLayoutPanel2.Controls.Add(botHard, 2, 0);
      tableLayoutPanel2.Dock = DockStyle.Fill;
      tableLayoutPanel2.Location = new Point(43, 324);
      tableLayoutPanel2.Name = "tableLayoutPanel2";
      tableLayoutPanel2.RowCount = 1;
      tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
      tableLayoutPanel2.Size = new Size(698, 74);
      tableLayoutPanel2.TabIndex = 4;
      // 
      // botEasy
      // 
      botEasy.Anchor = AnchorStyles.None;
      botEasy.ForeColor = Color.DeepPink;
      botEasy.Location = new Point(51, 25);
      botEasy.Name = "botEasy";
      botEasy.Size = new Size(130, 24);
      botEasy.TabIndex = 1;
      botEasy.Text = "Bot - Easy";
      botEasy.UseVisualStyleBackColor = true;
      botEasy.Click += BotEasyClick;
      // 
      // botMedium
      // 
      botMedium.Anchor = AnchorStyles.None;
      botMedium.ForeColor = Color.DeepPink;
      botMedium.Location = new Point(283, 25);
      botMedium.Name = "botMedium";
      botMedium.Size = new Size(130, 24);
      botMedium.TabIndex = 5;
      botMedium.Text = "Bot - Medium";
      botMedium.UseVisualStyleBackColor = true;
      botMedium.Click += BotMediumClick;
      // 
      // botHard
      // 
      botHard.Anchor = AnchorStyles.None;
      botHard.ForeColor = Color.DeepPink;
      botHard.Location = new Point(516, 25);
      botHard.Name = "botHard";
      botHard.Size = new Size(130, 24);
      botHard.TabIndex = 6;
      botHard.Text = "Bot - Hard";
      botHard.UseVisualStyleBackColor = true;
      botHard.Click += BotHardClick;
      // 
      // tableLayoutPanel3
      // 
      tableLayoutPanel3.BackColor = Color.Transparent;
      tableLayoutPanel3.ColumnCount = 2;
      tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel3.Controls.Add(multiplayerLocal, 0, 0);
      tableLayoutPanel3.Controls.Add(multiplayerOnline, 1, 0);
      tableLayoutPanel3.Dock = DockStyle.Fill;
      tableLayoutPanel3.Location = new Point(43, 404);
      tableLayoutPanel3.Name = "tableLayoutPanel3";
      tableLayoutPanel3.RowCount = 1;
      tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
      tableLayoutPanel3.Size = new Size(698, 74);
      tableLayoutPanel3.TabIndex = 5;
      // 
      // multiplayerLocal
      // 
      multiplayerLocal.Anchor = AnchorStyles.None;
      multiplayerLocal.ForeColor = Color.DeepPink;
      multiplayerLocal.Location = new Point(109, 25);
      multiplayerLocal.Name = "multiplayerLocal";
      multiplayerLocal.Size = new Size(130, 24);
      multiplayerLocal.TabIndex = 2;
      multiplayerLocal.Text = "Local multiplayer";
      multiplayerLocal.UseVisualStyleBackColor = true;
      multiplayerLocal.Click += LocalMultiplayerClick;
      // 
      // multiplayerOnline
      // 
      multiplayerOnline.Anchor = AnchorStyles.None;
      multiplayerOnline.ForeColor = Color.DeepPink;
      multiplayerOnline.Location = new Point(458, 25);
      multiplayerOnline.Name = "multiplayerOnline";
      multiplayerOnline.Size = new Size(130, 24);
      multiplayerOnline.TabIndex = 3;
      multiplayerOnline.Text = "Online multiplayer";
      multiplayerOnline.UseVisualStyleBackColor = true;
      multiplayerOnline.Click += OnlineMultiplayerClick;
      // 
      // backgroundGame
      // 
      backgroundGame.BackColor = Color.Transparent;
      backgroundGame.Controls.Add(tableLayoutPanel1);
      backgroundGame.Dock = DockStyle.Fill;
      backgroundGame.Location = new Point(0, 0);
      backgroundGame.Name = "backgroundGame";
      backgroundGame.Size = new Size(784, 561);
      backgroundGame.TabIndex = 6;
      backgroundGame.TabStop = false;
      backgroundGame.Paint += BackgroundGamePaint;
      // 
      // backgroundGameTimer
      // 
      backgroundGameTimer.Enabled = true;
      backgroundGameTimer.Interval = 50;
      backgroundGameTimer.Tick += BackgroundGameTick;
      // 
      // Menu
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(784, 561);
      Controls.Add(backgroundGame);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      Name = "Menu";
      Text = "Menu";
      VisibleChanged += MenuVisibilityChanged;
      tableLayoutPanel1.ResumeLayout(false);
      tableLayoutPanel1.PerformLayout();
      tableLayoutPanel2.ResumeLayout(false);
      tableLayoutPanel3.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)backgroundGame).EndInit();
      backgroundGame.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private Label nameLabel;
    private TableLayoutPanel tableLayoutPanel1;
    private Button botEasy;
    private Button multiplayerLocal;
    private Button multiplayerOnline;
    private TableLayoutPanel tableLayoutPanel2;
    private Button botMedium;
    private Button botHard;
    private TableLayoutPanel tableLayoutPanel3;
    private PictureBox backgroundGame;
    private System.Windows.Forms.Timer backgroundGameTimer;
  }
}
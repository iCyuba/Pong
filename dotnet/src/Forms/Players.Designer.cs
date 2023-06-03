namespace Pong
{
  partial class Players
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
      barLayout = new TableLayoutPanel();
      username = new Label();
      refresh = new Button();
      topLayout = new TableLayoutPanel();
      dataGrid = new DataGridView();
      barLayout.SuspendLayout();
      topLayout.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
      SuspendLayout();
      // 
      // barLayout
      // 
      barLayout.ColumnCount = 2;
      barLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
      barLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      barLayout.Controls.Add(username, 0, 0);
      barLayout.Controls.Add(refresh, 1, 0);
      barLayout.Location = new Point(3, 3);
      barLayout.Name = "barLayout";
      barLayout.RowCount = 1;
      barLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
      barLayout.Size = new Size(778, 50);
      barLayout.TabIndex = 0;
      // 
      // username
      // 
      username.Anchor = AnchorStyles.Left;
      username.AutoSize = true;
      username.Location = new Point(3, 17);
      username.Name = "username";
      username.Size = new Size(79, 15);
      username.TabIndex = 0;
      username.Text = "Registered as:";
      // 
      // refresh
      // 
      refresh.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      refresh.Location = new Point(625, 13);
      refresh.Name = "refresh";
      refresh.Size = new Size(150, 23);
      refresh.TabIndex = 1;
      refresh.Text = "Refresh player list";
      refresh.UseVisualStyleBackColor = true;
      refresh.Click += refreshList;
      // 
      // topLayout
      // 
      topLayout.ColumnCount = 1;
      topLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      topLayout.Controls.Add(barLayout, 0, 0);
      topLayout.Controls.Add(dataGrid, 0, 1);
      topLayout.Dock = DockStyle.Fill;
      topLayout.Location = new Point(0, 0);
      topLayout.Name = "topLayout";
      topLayout.RowCount = 2;
      topLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
      topLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
      topLayout.Size = new Size(784, 561);
      topLayout.TabIndex = 1;
      // 
      // dataGrid
      // 
      dataGrid.AllowUserToAddRows = false;
      dataGrid.AllowUserToDeleteRows = false;
      dataGrid.AllowUserToResizeColumns = false;
      dataGrid.AllowUserToResizeRows = false;
      dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      dataGrid.BackgroundColor = SystemColors.Window;
      dataGrid.BorderStyle = BorderStyle.None;
      dataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGrid.ColumnHeadersVisible = false;
      dataGrid.Dock = DockStyle.Fill;
      dataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
      dataGrid.GridColor = SystemColors.Window;
      dataGrid.ImeMode = ImeMode.NoControl;
      dataGrid.Location = new Point(3, 59);
      dataGrid.MultiSelect = false;
      dataGrid.Name = "dataGrid";
      dataGrid.ReadOnly = true;
      dataGrid.RowHeadersVisible = false;
      dataGrid.RowTemplate.Height = 25;
      dataGrid.ShowCellToolTips = false;
      dataGrid.ShowEditingIcon = false;
      dataGrid.Size = new Size(778, 499);
      dataGrid.TabIndex = 1;
      dataGrid.CellClick += inviteClicked;
      // 
      // Players
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(784, 561);
      Controls.Add(topLayout);
      Name = "Players";
      Text = "Players";
      barLayout.ResumeLayout(false);
      barLayout.PerformLayout();
      topLayout.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel barLayout;
    private Label username;
    private Button refresh;
    private TableLayoutPanel topLayout;
    private DataGridView dataGrid;
  }
}
using System.Data;

namespace Pong
{
  public partial class Players : Form
  {
    public DataTable Data { get; set; }
    public Connection Connection { get; set; }

    public Players(Connection connection)
    {
      InitializeComponent();

      // Create the data table for the players
      Data = new();
      Data.Columns.Add("Name", typeof(string));

      // Set the data source of the data grid view to the data table
      dataGrid.DataSource = Data;

      // Add a column with a button for inviting players
      DataGridViewButtonColumn inviteColumn = new()
      {
        Name = "Invite",
        Text = "Invite",
        // Initially we want to show the default text which is "Invite". This will be changed to false when the button is clicked so the cell value will be used instead
        UseColumnTextForButtonValue = true
      };
      dataGrid.Columns.Add(inviteColumn);

      // Make the cells not selectable
      dataGrid.DefaultCellStyle.SelectionBackColor = dataGrid.DefaultCellStyle.BackColor;
      dataGrid.DefaultCellStyle.SelectionForeColor = dataGrid.DefaultCellStyle.ForeColor;

      // Store the connection
      Connection = connection;

      // Add an event listener for when the connection receives a list of players
      Connection.OnListHandler += (_, listEvent) => SetPlayers(listEvent.Players);
      // Add a player to the data table when the connection receives a registration (ignore ourselves tho)
      Connection.OnRegisterHandler += (_, registration) =>
      {
        if (registration.Name != Connection.Name)
          AddPlayer(registration.Name);
      };

      // Set the label to the player's name
      username.Text = $"Registered as: {Connection.Name}";
    }

    /// <summary>
    /// Adds players to the data table
    /// </summary>
    /// <param name="players">The list of players</param>
    public void SetPlayers(List<string> players)
    {
      // Clear the data table
      Data.Clear();

      foreach (string player in players)
      {
        // Add the player to the data table (without updating the data grid view)
        AddPlayer(player, false);
      }

      // Update the data grid view
      dataGrid.Update();
    }

    /// <summary>
    /// Adds a player to the data table
    /// </summary>
    /// <param name="player">The player to add</param>
    /// <param name="update">Whether the data grid view should be updated</param>
    public void AddPlayer(string player, bool update = true)
    {
      Data.Rows.Add(new object[] { player });

      if (update)
        dataGrid.Update();
    }

    /// <summary>
    /// Refreshes the list of players when the refresh button is clicked
    /// </summary>
    private void refreshList(object sender, EventArgs e)
    {
      // Idc about the task, just do it lmao
      _ = Connection.List();
    }

    /// <summary>
    /// Send an invitation to the selected player when the invite button is clicked
    /// </summary>
    private void inviteClicked(object sender, DataGridViewCellEventArgs e)
    {
      // Make sure the invite button was clicked and not the player name (column 0, no clue why it's 0 and not 1)
      if (e.ColumnIndex != 0)
        return;

      string? player = Data.Rows[e.RowIndex].Field<string>("Name");

      Console.WriteLine($"Invited player: {player}");

      // Replace the invite button with a text saying "Invited"
      DataGridViewTextBoxCell cell = new()
      {
        Value = "Invited"
      };
      dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] = cell;
    }
  }
}

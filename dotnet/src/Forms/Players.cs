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
      DataGridViewButtonColumn inviteColumn =
        new()
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

      // Connection event listeners
      Connection.OnListHandler += SetPlayers;
      Connection.OnRegisterHandler += AddPlayerRegistration;
      Connection.OnUnregisterHandler += RemovePlayer;
      Connection.OnInviteHandler += Invited;
      Connection.OnCreateHandler += SessionCreated;

      // Set the label to the player's name
      username.Text = $"Registered as: {Connection.Name}";
    }

    /// <summary>
    /// Show a game client window when a new session is created and we're in it
    /// </summary>
    private void SessionCreated(object? _, Connection.CreateEvent createEvent)
    {
      if (createEvent.Player1 == Connection.Name || createEvent.Player2 == Connection.Name)
      {
        // Create a new game client window and show it
        GameClientWindow gameWindow = new(Connection);

        Menu.ShowForm(this, gameWindow);
      }
      else
      {
        // Remove the players who are in the session from the data table
        RemovePlayer(null, new(createEvent.Player1));
        RemovePlayer(null, new(createEvent.Player2));
      }
    }

    /// <summary>
    /// Adds players to the data table when the connection receives a list of players
    /// </summary>
    private void SetPlayers(object? _, Connection.ListEvent listEvent)
    {
      // Clear the data table
      Data.Clear();

      foreach (string player in listEvent.Players)
      {
        // Add the player to the data table (without updating the data grid view)
        AddPlayer(player, false);
      }

      // Update the data grid view
      dataGrid.Update();
    }

    /// <summary>
    /// Adds a player when the connection receives a registration
    /// </summary>
    private void AddPlayerRegistration(object? _, Connection.RegisterEvent registerEvent)
    {
      if (registerEvent.Name != Connection.Name)
        AddPlayer(registerEvent.Name);
    }

    /// <summary>
    /// Adds a player to the data table
    /// </summary>
    /// <param name="player">The player to add</param>
    /// <param name="update">Whether the data grid view should be updated</param>
    private void AddPlayer(string player, bool update = true)
    {
      Data.Rows.Add(new object[] { player });

      if (update)
        dataGrid.Update();
    }

    /// <summary>
    /// Removes a player from the data table when the connection receives an unregistration
    /// </summary>
    private void RemovePlayer(object? _, Connection.UnregisterEvent unregisterEvent)
    {
      string player = unregisterEvent.Name;

      // Find the row with the player's name
      foreach (DataRow row in Data.Rows)
      {
        // Remove the row if the name matches
        if (row.Field<string>("Name") == player)
        {
          Data.Rows.Remove(row);
          break;
        }
      }

      // Update the data grid view
      dataGrid.Update();
    }

    /// <summary>
    /// Refreshes the list of players when the refresh button is clicked
    /// </summary>
    private void RefreshList(object sender, EventArgs e)
    {
      // Idc about the task, just do it lmao
      _ = Connection.List();
    }

    /// <summary>
    /// Send an invitation to the selected player when the invite button is clicked
    /// </summary>
    private void InviteClicked(object sender, DataGridViewCellEventArgs e)
    {
      // Make sure the invite button was clicked and not the player name (column 0, no clue why it's 0 and not 1)
      if (
        e.ColumnIndex != 0
        || dataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType()
          != typeof(DataGridViewButtonCell)
      )
        return;

      string? player = Data.Rows[e.RowIndex].Field<string>("Name");

      // Make sure the player is not null
      if (player == null)
        return;

      // Send an invite to the player
      _ = Connection.Invite(player);
    }

    /// <summary>
    /// Handles the invite event
    /// <br />
    /// If the invite is for this player, show a message box asking if the player wants to accept the invite
    /// <br />
    /// If the invite is by this player, replace the invite button with a text saying "Invited"
    /// </summary>
    private void Invited(object? _, Connection.InviteEvent inviteEvent)
    {
      // Check if the invite is for this player or by this player
      if (inviteEvent.By == Connection.Name)
      {
        // If it's by us. Replace the invite button with a text saying "Invited"
        // Find the row with the player's name
        foreach (DataGridViewRow row in dataGrid.Rows)
        {
          // The player's name is in the column with index 1 (column 0 is the invite button. In the rendered app it's different tho)
          if (row.Cells[1].Value?.ToString() == inviteEvent.To)
          {
            // Replace the invite button with a text saying "Invited"
            DataGridViewTextBoxCell cell = new() { Value = "Invited" };
            row.Cells[0] = cell;
            break;
          }
        }
      }
      else if (inviteEvent.To == Connection.Name)
      {
        // Change the invite button to "Accept"
        // Find the row with the player's name
        foreach (DataGridViewRow row in dataGrid.Rows)
        {
          // The player's name is in the column with index 1 (column 0 is the invite button. In the rendered app it's different tho)
          if (row.Cells[1].Value?.ToString() == inviteEvent.By)
          {
            // Replace the invite button with a text saying "Accept"
            var cell = (DataGridViewButtonCell)row.Cells[0];
            cell.Value = "Accept";
            cell.UseColumnTextForButtonValue = false; // Make sure the text is shown

            break;
          }
        }

        // The invite is for this player, so we want to show a message box asking if the player wants to accept the invite
        DialogResult result = MessageBox.Show(
          $"{inviteEvent.By} wants to play against you. Do you accept?",
          "Invitation",
          MessageBoxButtons.YesNo
        );

        // If the player accepted the invite, send an invite response (by sending a new invite)
        if (result == DialogResult.Yes)
          _ = Connection.Invite(inviteEvent.By);
      }
    }

    /// <summary>
    /// Unregisters the player when the form is closed (also unregisters the event listeners)
    /// </summary>
    private void ListClosed(object sender, FormClosedEventArgs e)
    {
      // Unregister the event listeners
      Connection.OnListHandler -= SetPlayers;
      Connection.OnRegisterHandler -= AddPlayerRegistration;
      Connection.OnUnregisterHandler -= RemovePlayer;
      Connection.OnInviteHandler -= Invited;
      Connection.OnCreateHandler -= SessionCreated;

      // Send an unregister event
      _ = Connection.Unregister();
    }
  }
}

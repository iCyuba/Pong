using System.Net.WebSockets;

namespace Pong
{
  // My handler methods are defined in the partial classes
  // Also the message loop is defined in the partial class (MessageLoop.cs)
  public partial class Connection
  {
    public Uri ServerURI { get; }
    private ClientWebSocket WS { get; }

    public string? Name { get; private set; }
    public bool Registered { get; private set; }

    // The event handlers
    /// <summary>
    /// Event handler for when the connection is closed
    /// </summary>
    public EventHandler? OnCloseHandler { get; set; }

    /// <summary>
    /// Event handler for when a message is received
    /// </summary>
    public EventHandler<string>? OnMessageHandler { get; set; }

    /// <summary>
    /// Event handler when an error occurs (sent by the server obv. not a real exception)
    /// </summary>
    public EventHandler<ErrorEvent>? OnErrorHandler { get; set; }

    /// <summary>
    /// Event handler for when the a player registers (including this player)
    /// </summary>
    public EventHandler<RegisterEvent>? OnRegisterHandler { get; set; }

    /// <summary>
    /// Event handler for when the a player unregisters (excluding this player)
    /// </summary>
    public EventHandler<UnregisterEvent>? OnUnregisterHandler { get; set; }

    /// <summary>
    /// Event handler for when the a list of all players is received
    /// </summary>
    public EventHandler<ListEvent>? OnListHandler { get; set; }

    /// <summary>
    /// Event handler for when an invitation is received (either for us or by us)
    /// </summary>
    public EventHandler<InviteEvent>? OnInviteHandler { get; set; }

    /// <summary>
    /// Event handler for when a player accepts an invitation and the session is created
    /// </summary>
    public EventHandler<CreateEvent>? OnCreateHandler { get; set; }

    /// <summary>
    /// Event handler for when a player is ready. This can be you or the other player. Only sent while the other player is not ready
    /// </summary>
    public EventHandler<ReadyEvent>? OnReadyHandler { get; set; }

    /// <summary>
    /// Event handler for when a session starts (this is a different event to OnCreateHandler)
    /// </summary>
    public EventHandler<StartEvent>? OnStartHandler { get; set; }

    /// <summary>
    /// Event handler for when the ball position is updated
    /// </summary>
    public EventHandler<UpdateEvent>? OnUpdateHandler { get; set; }

    /// <summary>
    /// Event handler for when a player moves their paddle
    /// </summary>
    public EventHandler<MoveEvent>? OnMoveHandler { get; set; }

    /// <summary>
    /// Event handler for when a player scores
    /// </summary>
    public EventHandler<ScoreEvent>? OnScoreHandler { get; set; }

    /// <summary>
    /// Event handler for when a player wins
    /// </summary>
    public EventHandler<WinEvent>? OnWinHandler { get; set; }

    /// <summary>
    /// Event handler for when a player leaves a session
    /// </summary>
    public EventHandler<EndEvent>? OnEndHandler { get; set; }

    /// <summary>
    /// Create a new connection to the server
    /// <br />
    /// By default, the connection will be automatically opened
    /// </summary>
    public Connection(string uri, bool autoConnect = true)
    {
      // Save the URI and create a new websocket
      ServerURI = new(uri);
      WS = new();

      // My lovely event handlers <3
      EventHandlers();

      // If autoConnect is true, connect to the server
      if (autoConnect)
        _ = Connect(); // don't wait for the connection to complete
    }

    /// <summary>
    /// Register all the event handlers. This is called in the constructor
    /// <br />
    /// There should be a better way to do this but I don't know what it is
    /// </summary>
    private void EventHandlers()
    {
      // On message
      OnMessageHandler += OnMessage;

      // On register
      OnRegisterHandler += OnRegister;

      // On close
      // OnCloseHandler += OnClose;
    }

    /// <summary>
    /// Connect to the server, this will also start the message loop
    /// </summary>
    public async Task Connect()
    {
      // Connect to the websocket with a timeout of 2 seconds
      CancellationTokenSource timeout = new(5000);
      await WS.ConnectAsync(ServerURI, timeout.Token);

      // Start the message loop (defined in MessageLoop.cs)
      _ = MessageLoop(); // don't wait for the message loop to complete (cuz it should never complete. you will have to play forever)
    }
  }
}

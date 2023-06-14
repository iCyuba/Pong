namespace Pong
{
  partial class Connection
  {
    public class ErrorEvent : GameEvent
    {
      public string Message { get; set; }

      public ErrorEvent(string message)
        : base("error")
      {
        Message = message;
      }

      /// <summary>
      /// Deserialize an ErrorEvent from json
      /// </summary>
      public static new ErrorEvent Deserialize(string json)
      {
        return Deserialize<ErrorEvent>(json);
      }
    }
  }
}

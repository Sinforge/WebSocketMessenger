namespace WebSocketMessenger.Core.Exceptions
{
    public class SharedException : Exception
    {
        public readonly int StatusCode;

        public SharedException(string? Message, int statusCode): base (Message){ 
            StatusCode = statusCode;
        }
        

    }
}

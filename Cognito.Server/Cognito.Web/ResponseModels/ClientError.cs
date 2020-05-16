namespace Cognito.Web.ResponseModels
{
    public class ClientError
    {
        public string Title { get; }

        public string Message { get; }

        public ClientError(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}

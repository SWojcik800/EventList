namespace EventList.WebApi.Common.Exceptions
{
    /// <summary>
    /// Represents application error
    /// </summary>
    public class ApplicationErrorException : Exception
    {
        public ApplicationErrorException(string? message) : base(message)
        {
        }
    }
}

using System.Text.Json.Serialization;

namespace EventList.WebApi.Web.Middleware
{
    /// <summary>
    /// Represents api response with error
    /// </summary>
    public class ErrorApiResponse
    {
        [JsonPropertyName("error")]
        public object Error { get; set; }

        public ErrorApiResponse(object error)
        {
            Error = error;
        }
    }
}

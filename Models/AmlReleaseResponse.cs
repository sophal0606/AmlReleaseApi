using System.Text.Json.Serialization;

namespace AmlReleaseApi.Models
{
    public class AmlReleaseResponse
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public AmlReleaseData? Data { get; set; }
    }

    public class AmlReleaseData
    {
        [JsonPropertyName("internalRef")]
        public string InternalRef { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("remark")]
        public string? Remark { get; set; }
    }
}
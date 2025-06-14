using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AmlReleaseApi.Models
{
    public class AmlReleaseRequest
    {
        [JsonPropertyName("ApprovalStatus")]
        [Required]
        public string ApprovalStatus { get; set; } = string.Empty;

        [JsonPropertyName("ExtRef")]
        [Required]
        public string ExtRef { get; set; } = string.Empty;

        [JsonPropertyName("SourceSystem")]
        [Required]
        public string SourceSystem { get; set; } = string.Empty;

        [JsonPropertyName("TrnChannel")]
        [Required]
        public string TrnChannel { get; set; } = string.Empty;

        [JsonPropertyName("ScanID")]
        [Required]
        public string ScanID { get; set; } = string.Empty;

        [JsonPropertyName("CurrentDateTime")]
        [Required]
        public string CurrentDateTime { get; set; } = string.Empty;

        [JsonPropertyName("Remark")]
        public string? Remark { get; set; }

        [JsonPropertyName("TotalScreening")]
        [Required]
        public int TotalScreening { get; set; }

        [JsonPropertyName("TotalHit")]
        [Required]
        public int TotalHit { get; set; }

        [JsonPropertyName("CurrentScreeningID")]
        [Required]
        public string CurrentScreeningID { get; set; } = string.Empty;

        [JsonPropertyName("PreviousScreeningID")]
        [Required]
        public string PreviousScreeningID { get; set; } = string.Empty;
    }
}
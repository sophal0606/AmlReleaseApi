using AmlReleaseApi.Models;
using System.Text;
using System.Text.Json;

namespace AmlReleaseApi.Services
{
    public class AmlReleaseService : IAmlReleaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AmlReleaseService> _logger;

        public AmlReleaseService(HttpClient httpClient, IConfiguration configuration, ILogger<AmlReleaseService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AmlReleaseResponse> ProcessAmlReleaseAsync(AmlReleaseRequest request)
        {
            try
            {
                //var apiUrl = _configuration["AmlReleaseApi:Url"] ?? "http://localhost:5019/v1/inhouse/api/PRD/amlrelease";
                //var apiKey = _configuration["AmlReleaseApi:ApiKey"] ?? "NkYUxNtvYmFk3AatLS7ZPQ==";
                var apiUrl = _configuration["AmlReleaseApi:Url"] ?? "http://localhost:8080/api/amlrelease";
                var apiKey = _configuration["AmlReleaseApi:ApiKey"] ?? "NkYUxNtvYmFk3AatLS7ZPQ==";
                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    WriteIndented = true
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Add API Key to headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                _logger.LogInformation("Sending AML Release request to {Url}", apiUrl);
                _logger.LogDebug("Request payload: {Json}", json);

                var response = await _httpClient.PostAsync(apiUrl, content);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Response content: {ResponseContent}", responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<AmlReleaseResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    _logger.LogInformation("AML Release request successful. Internal Ref: {InternalRef}", result?.Data?.InternalRef);
                    return result ?? new AmlReleaseResponse { StatusCode = 500, Status = "error" };
                }
                else
                {
                    _logger.LogError("AML Release request failed. Status: {StatusCode}, Content: {Content}",
                        response.StatusCode, responseContent);

                    return new AmlReleaseResponse
                    {
                        StatusCode = (int)response.StatusCode,
                        Status = "error",
                        Data = new AmlReleaseData
                        {
                            Code = "E" + (int)response.StatusCode,
                            Message = $"API call failed: {response.ReasonPhrase}",
                            Remark = responseContent
                        }
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error occurred while calling AML Release API");
                return new AmlReleaseResponse
                {
                    StatusCode = 500,
                    Status = "error",
                    Data = new AmlReleaseData
                    {
                        Code = "E5001",
                        Message = "Network error occurred",
                        Remark = ex.Message
                    }
                };
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timeout occurred while calling AML Release API");
                return new AmlReleaseResponse
                {
                    StatusCode = 408,
                    Status = "error",
                    Data = new AmlReleaseData
                    {
                        Code = "E4081",
                        Message = "Request timeout",
                        Remark = ex.Message
                    }
                };
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON serialization/deserialization error");
                return new AmlReleaseResponse
                {
                    StatusCode = 500,
                    Status = "error",
                    Data = new AmlReleaseData
                    {
                        Code = "E5002",
                        Message = "JSON processing error",
                        Remark = ex.Message
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while calling AML Release API");
                return new AmlReleaseResponse
                {
                    StatusCode = 500,
                    Status = "error",
                    Data = new AmlReleaseData
                    {
                        Code = "E5000",
                        Message = "Unexpected error occurred",
                        Remark = ex.Message
                    }
                };
            }
        }
    }
}
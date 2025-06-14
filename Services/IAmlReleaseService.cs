using AmlReleaseApi.Models;

namespace AmlReleaseApi.Services
{
    public interface IAmlReleaseService
    {
        Task<AmlReleaseResponse> ProcessAmlReleaseAsync(AmlReleaseRequest request);
    }
}
using AmlReleaseApi.Models;
using AmlReleaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AmlReleaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmlReleaseController : ControllerBase
    {
        private readonly IAmlReleaseService _amlReleaseService;
        private readonly ILogger<AmlReleaseController> _logger;

        public AmlReleaseController(IAmlReleaseService amlReleaseService, ILogger<AmlReleaseController> logger)
        {
            _amlReleaseService = amlReleaseService;
            _logger = logger;
        }

        /// <summary>
        /// Process AML Release request
        /// </summary>
        /// <param name="request">AML Release request payload</param>
        /// <returns>AML Release response</returns>
        [HttpPost("process")]
        public async Task<ActionResult<AmlReleaseResponse>> ProcessAmlRelease([FromBody] AmlReleaseRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for AML Release request");
                    return BadRequest(ModelState);
                }

                // Validate ApprovalStatus
                if (request.ApprovalStatus != "A" && request.ApprovalStatus != "R")
                {
                    _logger.LogWarning("Invalid ApprovalStatus: {ApprovalStatus}", request.ApprovalStatus);
                    return BadRequest(new { message = "ApprovalStatus must be 'A' (Approved) or 'R' (Rejected)" });
                }

                _logger.LogInformation("Processing AML Release request for ExtRef: {ExtRef}, Status: {ApprovalStatus}",
                    request.ExtRef, request.ApprovalStatus);

                var response = await _amlReleaseService.ProcessAmlReleaseAsync(request);

                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(response.StatusCode, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ProcessAmlRelease");
                return StatusCode(500, new AmlReleaseResponse
                {
                    StatusCode = 500,
                    Status = "error",
                    Data = new AmlReleaseData
                    {
                        Code = "E5000",
                        Message = "Internal server error",
                        Remark = "An unexpected error occurred"
                    }
                });
            }
        }

        ///// <summary>
        ///// Approve AML transaction
        ///// </summary>
        ///// <param name="extRef">External reference</param>
        ///// <param name="scanId">Scan ID</param>
        ///// <param name="screeningId">Screening ID</param>
        ///// <param name="remark">Optional remark</param>
        //[HttpPost("approve")]
        //public async Task<ActionResult<AmlReleaseResponse>> ApproveTransaction(
        //    [FromQuery] string extRef,
        //    [FromQuery] string scanId,
        //    [FromQuery] string screeningId,
        //    [FromQuery] string? remark = null)
        //{
        //    var request = new AmlReleaseRequest
        //    {
        //        ApprovalStatus = "A",
        //        ExtRef = extRef,
        //        SourceSystem = "TEST_OTT/OTCAPP",
        //        TrnChannel = "TEST_OTT/OTCAPP",
        //        ScanID = scanId,
        //        CurrentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //        Remark = remark ?? "Transaction approved",
        //        TotalScreening = 1,
        //        TotalHit = 1,
        //        CurrentScreeningID = screeningId,
        //        PreviousScreeningID = screeningId
        //    };

        //    return await ProcessAmlRelease(request);
        //}

        ///// <summary>
        ///// Reject AML transaction
        ///// </summary>
        ///// <param name="extRef">External reference</param>
        ///// <param name="scanId">Scan ID</param>
        ///// <param name="screeningId">Screening ID</param>
        ///// <param name="remark">Optional remark</param>
        //[HttpPost("reject")]
        //public async Task<ActionResult<AmlReleaseResponse>> RejectTransaction(
        //    [FromQuery] string extRef,
        //    [FromQuery] string scanId,
        //    [FromQuery] string screeningId,
        //    [FromQuery] string? remark = null)
        //{
        //    var request = new AmlReleaseRequest
        //    {
        //        ApprovalStatus = "R",
        //        ExtRef = extRef,
        //        SourceSystem = "TEST_OTT/OTCAPP",
        //        TrnChannel = "TEST_OTT/OTCAPP",
        //        ScanID = scanId,
        //        CurrentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //        Remark = remark ?? "Transaction rejected",
        //        TotalScreening = 1,
        //        TotalHit = 1,
        //        CurrentScreeningID = screeningId,
        //        PreviousScreeningID = screeningId
        //    };

        //    return await ProcessAmlRelease(request);
        //}
    }
}
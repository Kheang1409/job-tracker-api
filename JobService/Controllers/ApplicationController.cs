using Confluent.Kafka;
using JobService.DTOs;
using JobService.Models;
using JobService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace JobService.Controllers
{
    [ApiController]
    [Route("api/jobs/{jobId}/applications")]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IJobService _jobService;

        public ApplicationController(IJobService jobService)
        {
            _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        }

        [HttpPost]
        public async Task<IActionResult> AppliedJob(string jobId)
        {
            var authorizationResult = IsAuthorize();
            if (authorizationResult is IActionResult actionResult)
            {
                return actionResult;
            }
            if (!ObjectId.TryParse(jobId, out _))
            {
                return BadRequest("The provided ID is not a valid MongoDB ObjectId.");
            }

            var authorizUser = (Application)authorizationResult;

            var candidate = await _jobService.ApplyJobAsync(jobId, authorizUser);
            return Ok(candidate);
        }

        [HttpPatch("{applicationId}")]
        public async Task<IActionResult> UpdateApplicationStatus(string jobId, string applicationId, [FromBody] UpdateApplicationStatusDto statusDto)
        {
            try
            {
                var authorizationResult = IsAuthorize();
                if (authorizationResult is IActionResult actionResult)
                {
                    return actionResult;
                }
                if (!ObjectId.TryParse(jobId, out _))
                {
                    return BadRequest("The provided ID is not a valid MongoDB ObjectId.");
                }

                var application = (Application)authorizationResult;

                var job = await _jobService.GetJobByIdAsync(jobId);
                if (job == null || job.UserId != application.UserId)
                {
                    return NotFound("Job not found or unauthorized access.");
                }

                await _jobService.UpdateApplicationStatusAsync(jobId, applicationId, statusDto, application.UserId);

                return Ok(new { message = "Application's status has been set successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to set status: {ex.Message}" });
            }
        }

        private object IsAuthorize()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var username = User.FindFirst(ClaimTypes.GivenName)?.Value;

            var authorizUser = new Application();
            authorizUser.UserId = userId;
            authorizUser.Email = email;
            authorizUser.Username = username;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            return authorizUser;
        }
    }
}
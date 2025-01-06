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

        [HttpPatch("{applicationId}/interview")]
        public async Task<IActionResult> SetInterviewReminder(string jobId, string applicationId, [FromBody] SetInterviewReminderDto reminderDto)
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

                var authorizUser = (Application)authorizationResult;

                var job = await _jobService.GetJobByIdAsync(jobId);
                if (job == null || job.UserId != authorizUser.UserId)
                {
                    return NotFound("Job not found or unauthorized access.");
                }

                await _jobService.SetInterviewReminderAsync(jobId, applicationId, reminderDto, authorizUser.UserId);

                return Ok("Interview reminder has been set successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to set interview reminder: {ex.Message}");
            }
        }

        [HttpPatch("{applicationId}/status")]
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

                var authorizUser = (Application)authorizationResult;

                var job = await _jobService.GetJobByIdAsync(jobId);
                if (job == null || job.UserId != authorizUser.UserId)
                {
                    return NotFound("Job not found or unauthorized access.");
                }

                await _jobService.UpdateApplicationStatusAsync(jobId, applicationId, statusDto, authorizUser.UserId);

                return Ok("Application's status has been set successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to set status: {ex.Message}");
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
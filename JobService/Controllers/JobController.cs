using JobService.DTOs;
using JobService.Models;
using JobService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobService.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        }

        // Create a new job application
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var job = await _jobService.CreateJobAsync(dto, userId);
            return Ok(job);
        }

        // Fetch all jobs created by the logged-in user
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var jobs = await _jobService.GetJobsByUserIdAsync(userId);
            return Ok(jobs);
        }

        // Fetch a specific job by ID (only if it belongs to the logged-in user)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var job = await _jobService.GetJobByIdAsync(id, userId);

            if (job == null)
            {
                return NotFound("Job not found or does not belong to the user.");
            }

            return Ok(job);
        }

        // Update job status (only if it belongs to the logged-in user)
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateJobStatus(string id, [FromBody] UpdateJobStatusDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var updatedJob = await _jobService.UpdateJobStatusAsync(id, dto, userId);

            if (updatedJob == null)
            {
                return NotFound("Job not found or does not belong to the user.");
            }

            return Ok(updatedJob);
        }

        [HttpPut("{jobId}/reminder")]
        public async Task<IActionResult> SetInterviewReminder(string jobId, [FromBody] SetInterviewReminderDto reminderDto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in token.");
                }

                var job = await _jobService.GetJobByIdAsync(jobId, userId);
                if (job == null || job.UserId != userId)
                {
                    return NotFound("Job not found or unauthorized access.");
                }

                await _jobService.SetInterviewReminderAsync(jobId, reminderDto, userId);

                return Ok("Interview reminder has been set successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to set interview reminder: {ex.Message}");
            }
        }


        // Delete a job (only if it belongs to the logged-in user)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var deleted = await _jobService.DeleteJobAsync(id, userId);

            if (!deleted)
            {
                return NotFound("Job not found or does not belong to the user.");
            }

            return NoContent();
        }
    }
}
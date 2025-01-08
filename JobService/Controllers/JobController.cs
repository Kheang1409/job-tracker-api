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
    [Route("api/jobs")]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetJobs([FromQuery] int? PageNumber = 1, [FromQuery] string? Status = null)
        {
            var jobs = await _jobService.GetJobsAsync((int)PageNumber, Status);
            return Ok(jobs);
        }

        [Route("totals")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTotalJobsCount([FromQuery] string? Status = null)
        {
            var jobs = await _jobService.GetTotalJobsCountAsync(Status);
            return Ok(jobs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] JobDto dto)
        {
            var authorizationResult = IsAuthorize();
            if (authorizationResult is IActionResult actionResult)
            {
                return actionResult;
            }
            var authorizUser = (Application)authorizationResult;
            var job = await _jobService.CreateJobAsync(dto, authorizUser.Id);
            return Ok(job);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("The provided ID is not a valid MongoDB ObjectId.");
            }
            var job = await _jobService.GetJobByIdAsync(id);

            if (job == null)
            {
                return NotFound("Job not found.");
            }

            return Ok(job);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> FullUpdateJob(string id, [FromBody] JobDto updateJob)
        {

            var authorizationResult = IsAuthorize();
            if (authorizationResult is IActionResult actionResult)
            {
                return actionResult;
            }
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("The provided ID is not a valid MongoDB ObjectId.");
            }

            var authorizUser = (Application)authorizationResult;
            var job = await _jobService.FullUpdateJobAsync(id, updateJob, authorizUser.UserId);
            return Ok(job);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateJob(string id, [FromBody] JobDto updateJob)
        {
            var authorizationResult = IsAuthorize();
            if (authorizationResult is IActionResult actionResult)
            {
                return actionResult;
            }
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("The provided ID is not a valid MongoDB ObjectId.");
            }

            var authorizUser = (Application)authorizationResult;
            var job = await _jobService.PartialUpdateJobAsync(id, updateJob, authorizUser.UserId);
            return Ok(job);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateJobStatus(string id)
        {
            var authorizationResult = IsAuthorize();
            if (authorizationResult is IActionResult actionResult)
            {
                return actionResult;
            }
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("The provided ID is not a valid MongoDB ObjectId.");
            }
            var authorizUser = (Application)authorizationResult;
            var job = await _jobService.UpdateJobStatus(id, authorizUser.UserId);
            return Ok(job);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(string id)
        {
            var authorizationResult = IsAuthorize();
            if (authorizationResult is IActionResult actionResult)
            {
                return actionResult;
            }
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("The provided ID is not a valid MongoDB ObjectId.");
            }

            var authorizUser = (Application)authorizationResult;

            var deleted = await _jobService.DeleteJobAsync(id, authorizUser.Id);

            if (!deleted)
            {
                return NotFound("Job not found or does not belong to the user.");
            }

            return NoContent();
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
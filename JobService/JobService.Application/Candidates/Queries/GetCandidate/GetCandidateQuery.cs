using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Queries.GetCandidate;

public record GetCandidateQuery(string JobPostId, string CandidateId) : IRequest<Candidate>;
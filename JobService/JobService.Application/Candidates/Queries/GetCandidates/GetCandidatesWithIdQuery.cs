using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Queries.GetCandidates;

public record GetCandidatesWithIdQuery(string JobPostId, int PageNumber, int Limit) : IRequest<IEnumerable<Candidate>>;
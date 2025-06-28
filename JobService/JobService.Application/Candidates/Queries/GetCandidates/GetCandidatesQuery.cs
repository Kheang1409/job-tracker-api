using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.Candidates.Queries.GetCandidates;

public record GetCandidatesQuery(int PageNumber=1, int Limit=10) : IRequest<IEnumerable<Candidate>>;
using JobTracker.JobService.Application.Posts.Commands.Commons;
using JobTracker.JobService.Domain.Enums;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdatePost;

public record UpdatePostCommand(
    string Title,
    string CompanyName,
    WorkMode WorkMode,
    EmploymentType EmploymentType,
    int NumberOfOpenings,
    int MinExperience,
    int MaxExperience,
    decimal MinSalary,
    decimal MaxSalary,
    string Currency ,
    List<string> Skills,
    string Description,
    AddressCommand Address,
    DateTime? ExpirationDate,
    Status Status
) : IRequest<bool>;
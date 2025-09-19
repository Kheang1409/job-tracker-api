using JobTracker.JobService.Application.Posts.Commands.Commons;
using JobTracker.JobService.Domain.Enums;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.CreatePost;

public record CreatePostWithIdCommand(
    string AuthorId,
    string Title,
    string CompanyName,
    WorkMode WorkMode,
    EmploymentType EmploymentType,
    int NumberOfOpenings,
    int MinExperience,
    int MaxExperience,
    decimal MinSalary,
    decimal MaxSalary,
    string Currency,
    List<string> Skills,
    string Description,
    AddressCommand Address,
    DateTime? ExpirationDate
): IRequest<string>;
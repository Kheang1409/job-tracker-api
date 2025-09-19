using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using JobTracker.JobService.Domain.Factories;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostWithIdCommand, string>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostFactory _postFactory;

    public CreatePostCommandHandler(
        IPostRepository postRepository,
        IPostFactory postFactory
    )
    {
        _postRepository = postRepository;
        _postFactory = postFactory;
    }
    
    public async Task<string> Handle(CreatePostWithIdCommand command, CancellationToken cancellationToken)
    {
        var address = new Address.Builder()
                        .SetCountry(command.Address.Country)
                        .SetState(command.Address.State)
                        .SetCity(command.Address.City)
                        .SetStreet(command.Address.Street)
                        .SetPostalCode(command.Address.PostalCode)
                        .Build();

        var post = _postFactory.Create(
            command.AuthorId,
            command.Title,
            command.CompanyName,
            command.WorkMode,
            command.EmploymentType,
            command.NumberOfOpenings,
            command.MinExperience,
            command.MaxExperience,
            command.MinSalary,
            command.MaxSalary,
            command.Currency,
            command.Skills,
            command.Description,
            address,
            command.ExpirationDate
        );
        var jobPostId = await _postRepository.AddAsync(post);
        return jobPostId;
    }
}
using JobTracker.JobService.Application.Repositories;
using JobTracker.JobService.Domain.Entities;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdatePost;

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostWithIdCommand, bool>
{
    private readonly IPostRepository _postRepository;
    public UpdatePostCommandHandler(
        IPostRepository postRepository
    )
    {
        _postRepository = postRepository;
    }
    
    public async Task<bool> Handle(UpdatePostWithIdCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.JobPostId);
        if(post.AuthorId != command.AuthorId)
            throw new UnauthorizedAccessException();
        
        var address = new Address.Builder()
                        .SetCountry(command.Address.Country)
                        .SetState(command.Address.State)
                        .SetCity(command.Address.City)
                        .SetStreet(command.Address.Street)
                        .SetPostalCode(command.Address.PostalCode)
                        .Build();

        post.UpdatePost(
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
            command.ExpirationDate,
            command.Status
        );
        
        return await _postRepository.UpdateAsync(post);
    }
}
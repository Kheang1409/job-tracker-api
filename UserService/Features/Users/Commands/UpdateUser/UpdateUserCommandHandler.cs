using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using JobTracker.SharedKernel.Domain;
using MediatR;

namespace JobTracker.UserService.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserWithIdCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public UpdateUserCommandHandler(
        IUserRepository userRepository
    )
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(UpdateUserWithIdCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id);

        var address = new Address.Builder()
                        .SetCountry(command.Address.Country)
                        .SetState(command.Address.State)
                        .SetCity(command.Address.City)
                        .SetStreet(command.Address.Street)
                        .SetPostalCode(command.Address.PostalCode)
                        .Build();

        var experiences = command.Experiences.Select(
                        experience => new Experience.Builder()
                        .SetCompanyName(experience.CompanyName)
                        .SetPosition(experience.Position)
                        .SetBulletPoints(experience.BulletPoints)
                        .SetStartDate(experience.StartDate)
                        .SetEndDate(experience.EndDate)
                        .SetIsPresent(experience.IsPresent)
                        .Build()).ToList();


        var projects = command.Projects.Select(
                        project => new Project.Builder()
                        .SetName(project.Name)
                        .SetAbout(project.About)
                        .SetDuring(project.During)
                        .SetLink(project.Link)
                        .Build()).ToList();

        user.UpdateProfile(
            command.FirstName,
            command.LastName,
            command.Email,
            command.ContactNumber,
            command.ContactCountry,
            command.Bio,
            command.Skills,
            experiences,
            projects,
            address
        );
        
        var updated = await _userRepository.UpdateAsync(user);
        return updated;
    }
}

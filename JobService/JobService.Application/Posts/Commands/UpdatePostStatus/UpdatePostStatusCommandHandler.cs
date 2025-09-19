using JobTracker.JobService.Application.Repositories;
using MediatR;

namespace JobTracker.JobService.Application.JobLocations.Commands.UpdatePostStatus;

public class UpdatePostStatusCommandHandler : IRequestHandler<UpdatePostStatusWithIdCommand, bool>
{
    private readonly IPostRepository _postRepository;

    public UpdatePostStatusCommandHandler(
        IPostRepository postRepository
    )
    {
        _postRepository = postRepository;
    }
    
    
    public async Task<bool> Handle(UpdatePostStatusWithIdCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.JobPostId);
        post.SetStatus(command.Status);
        if (post.AuthorId != command.UserId)
            throw new UnauthorizedAccessException();
        return await _postRepository.UpdateAsync(post);
    }
}
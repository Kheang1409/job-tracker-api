using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.UserService.Application.Addresses.Commands.DeleteAddress;


public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required.")
            .Must(BeAValidObjectId).WithMessage("User Id must be a valid MongoDB ObjectId.");

        RuleFor(x => x.AddressId)
            .NotEmpty().WithMessage("Address Id is required.")
            .Must(BeAValidObjectId).WithMessage("Address Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}

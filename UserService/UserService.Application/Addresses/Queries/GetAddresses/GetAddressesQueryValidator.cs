using FluentValidation;
using MongoDB.Bson;

namespace JobTracker.UserService.Application.Addresses.Queries.GetAddresses;

public class GetAddressesQueryValidator : AbstractValidator<GetAddressesQuery>
{
    public GetAddressesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required.")
            .Must(BeAValidObjectId).WithMessage("User Id must be a valid MongoDB ObjectId.");
    }

    private bool BeAValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}
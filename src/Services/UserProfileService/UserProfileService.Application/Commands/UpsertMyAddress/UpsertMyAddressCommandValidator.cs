using FluentValidation;

namespace UserProfileService.Application.Commands.UpsertMyAddress;

public sealed class UpsertMyAddressCommandValidator : AbstractValidator<UpsertMyAddressCommand>
{
    public UpsertMyAddressCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(150);
        RuleFor(x => x.House).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20);
    }
}

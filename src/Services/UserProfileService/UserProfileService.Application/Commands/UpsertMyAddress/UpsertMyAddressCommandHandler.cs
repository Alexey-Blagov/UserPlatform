using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common;
using MediatR;
using UserProfileService.Contracts.Dtos;
using UserProfileService.Domain.Entities;
using UserProfileService.Domain.Repositories;
using UserProfileService.Domain.ValueObjects;

namespace UserProfileService.Application.Commands.UpsertMyAddress;

public sealed class UpsertMyAddressCommandHandler : IRequestHandler<UpsertMyAddressCommand, Result<UserProfileDto>>
{
    private readonly IUserProfileRepository _repository;
    private readonly ICurrentUser _currentUser;

    public UpsertMyAddressCommandHandler(IUserProfileRepository repository, ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Result<UserProfileDto>> Handle(UpsertMyAddressCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.Subject))
            return Result<UserProfileDto>.Failure("Unauthorized");

        var profile = await _repository.GetByExternalIdentityIdAsync(_currentUser.Subject!, cancellationToken);

        var address = new Address(request.Country, request.City, request.Street, request.House, request.PostalCode);

        if (profile is null)
        {
            profile = new UserProfile(
                Guid.NewGuid(),
                _currentUser.Subject!,
                request.FirstName,
                request.LastName,
                address);

            await _repository.AddAsync(profile, cancellationToken);
        }
        else
        {
            profile.UpdateAddress(address);
        }

        await _repository.SaveChangesAsync(cancellationToken);

        return Result<UserProfileDto>.Success(new UserProfileDto(
            profile.Id,
            profile.ExternalIdentityId,
            profile.FirstName,
            profile.LastName,
            profile.Address.Country,
            profile.Address.City,
            profile.Address.Street,
            profile.Address.House,
            profile.Address.PostalCode));
    }
}

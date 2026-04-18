using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common;
using MediatR;
using UserProfileService.Contracts.Dtos;
using UserProfileService.Domain.Repositories;

namespace UserProfileService.Application.Queries.GetMyProfile;

public sealed class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, Result<UserProfileDto>>
{
    private readonly IUserProfileRepository _repository;
    private readonly ICurrentUser _currentUser;

    public GetMyProfileQueryHandler(IUserProfileRepository repository, ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Result<UserProfileDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.Subject))
            return Result<UserProfileDto>.Failure("Unauthorized");

        var profile = await _repository.GetByExternalIdentityIdAsync(_currentUser.Subject!, cancellationToken);
        if (profile is null)
            return Result<UserProfileDto>.Failure("Profile not found");

        var dto = new UserProfileDto(
            profile.Id,
            profile.ExternalIdentityId,
            profile.FirstName,
            profile.LastName,
            profile.Address.Country,
            profile.Address.City,
            profile.Address.Street,
            profile.Address.House,
            profile.Address.PostalCode);

        return Result<UserProfileDto>.Success(dto);
    }
}

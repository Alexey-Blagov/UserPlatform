using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common;
using UserProfileService.Contracts.Dtos;

namespace UserProfileService.Application.Queries.GetMyProfile;

public sealed record GetMyProfileQuery() : IQuery<Result<UserProfileDto>>;

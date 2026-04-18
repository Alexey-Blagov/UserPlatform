using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Common;
using UserProfileService.Contracts.Dtos;

namespace UserProfileService.Application.Commands.UpsertMyAddress;

public sealed record UpsertMyAddressCommand(
    string FirstName,
    string LastName,
    string Country,
    string City,
    string Street,
    string House,
    string PostalCode) : ICommand<Result<UserProfileDto>>;

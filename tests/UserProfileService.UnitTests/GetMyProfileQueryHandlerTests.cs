using BuildingBlocks.Application.Abstractions;
using FluentAssertions;
using NSubstitute;
using UserProfileService.Application.Queries.GetMyProfile;
using UserProfileService.Domain.Entities;
using UserProfileService.Domain.Repositories;
using UserProfileService.Domain.ValueObjects;
using Xunit;

namespace UserProfileService.UnitTests;

public sealed class GetMyProfileQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Profile_For_Authenticated_User()
    {
        var repository = Substitute.For<IUserProfileRepository>();
        var currentUser = Substitute.For<ICurrentUser>();

        currentUser.IsAuthenticated.Returns(true);
        currentUser.Subject.Returns("kc-user-123");

        repository.GetByExternalIdentityIdAsync("kc-user-123", Arg.Any<CancellationToken>())
            .Returns(new UserProfile(
                Guid.NewGuid(),
                "kc-user-123",
                "Alexey",
                "Blagov",
                new Address("Finland", "Turku", "Main Street", "10A", "20100")));

        var handler = new GetMyProfileQueryHandler(repository, currentUser);

        var result = await handler.Handle(new GetMyProfileQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.City.Should().Be("Turku");
    }
}

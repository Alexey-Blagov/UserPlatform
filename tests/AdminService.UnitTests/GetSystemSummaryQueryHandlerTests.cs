using AdminService.Application.Queries.GetSystemSummary;
using FluentAssertions;
using Xunit;

namespace AdminService.UnitTests;

public sealed class GetSystemSummaryQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Summary()
    {
        var handler = new GetSystemSummaryQueryHandler();
        var result = await handler.Handle(new GetSystemSummaryQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ServiceName.Should().Be("AdminService");
    }
}

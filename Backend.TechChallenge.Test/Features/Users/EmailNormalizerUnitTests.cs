using Backend.TechChallenge.Api.Features.Users.Helpers;
using FluentAssertions;
using Xunit;

namespace Backend.TechChallenge.Test.Features.Users;

public class EmailNormalizerUnitTests
{
    [Theory]
    [InlineData("test@test.com", "test@test.com")]
    [InlineData("test.01+100@test.com", "test01+@test.com")]
    [InlineData("TEST@TEST.COM", "TEST@TEST.COM")]
    public void ShouldReturnNormalizedEmail(string input, string expectedResult)
    {
        // act
        var actualResult = EmailNormalizer.Normalize(input);

        // assert
        actualResult.Should().Be(expectedResult);
    }
}

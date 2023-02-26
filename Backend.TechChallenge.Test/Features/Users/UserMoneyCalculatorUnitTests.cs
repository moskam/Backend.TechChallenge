using Backend.TechChallenge.Api.DAL;
using Backend.TechChallenge.Api.Features.Users.Services;
using FluentAssertions;
using Xunit;

namespace Backend.TechChallenge.Test.Features.Users;

public class UserMoneyCalculatorUnitTests
{
    private readonly UserMoneyCalculator _sut;

    public UserMoneyCalculatorUnitTests()
    {
        _sut = new UserMoneyCalculator();
    }

    [Theory]
    [InlineData(200, 224)]
    [InlineData(100, 100)]
    [InlineData(20, 36)]
    [InlineData(10, 10)]
    [InlineData(5, 5)]
    [InlineData(0, 0)]
    public void ShouldCalculateNomalUserMoney(decimal money, decimal expectedResult)
    {
        // act
        var actualResult = _sut.Calculate(money, UserType.Normal);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(200, 240)]
    [InlineData(100, 100)]
    [InlineData(20, 20)]
    [InlineData(0, 0)]
    public void ShouldCalculateSuperUserMoney(decimal money, decimal expectedResult)
    {
        // act
        var actualResult = _sut.Calculate(money, UserType.SuperUser);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(200, 600)]
    [InlineData(100, 100)]
    [InlineData(20, 20)]
    [InlineData(0, 0)]
    public void ShouldCalculatePremiumUserMoney(decimal money, decimal expectedResult)
    {
        // act
        var actualResult = _sut.Calculate(money, UserType.Premium);

        // assert
        actualResult.Should().Be(expectedResult);
    }
}

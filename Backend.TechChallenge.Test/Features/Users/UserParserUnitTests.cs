using Backend.TechChallenge.Api.DAL;
using Backend.TechChallenge.Api.Features.Users.Helpers;
using FluentAssertions;
using Xunit;

namespace Backend.TechChallenge.Test.Features.Users;

public class UserParserUnitTests
{
    [Theory]
    [InlineData(
        "Agustina,Agustina@gmail.com,+534645213542,Garay y Otra Calle,SuperUser,112234",
        "Agustina",
        "Agustina@gmail.com",
        "+534645213542",
        "Garay y Otra Calle",
        UserType.SuperUser,
        112234)]
    public void ShouldReturnParsedUser(string line, string name, string email, string phone, string address, UserType userType, decimal money)
    {
        // act
        var actualResult = UserParser.Parse(line);

        // assert
        actualResult.Name.Should().Be(name);
        actualResult.Email.Should().Be(email);
        actualResult.Phone.Should().Be(phone);
        actualResult.Address.Should().Be(address);
        actualResult.UserType.Should().Be(userType);
        actualResult.Money.Should().Be(money);
    }
}

using Backend.TechChallenge.Api.DAL;
using Backend.TechChallenge.Api.Features.Users.Helpers;
using Backend.TechChallenge.Contracts.Features.Users;
using Backend.TechChallenge.Test.Helpers;
using Backend.TechChallenge.Test.Infrastructure;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace Backend.TechChallenge.Test.Features.Users;

public class CreateUserTests : BaseTests
{
    private const string TestUsersFilePath = "TestData/Users.txt";

    private readonly string _userFile;

    public CreateUserTests()
    {
        _userFile = Path.Combine(Directory.GetCurrentDirectory(), TestServer.AppConfig.UsersFileRelativePath);
        PrepareDataFiles();
    }

    [Fact]
    public async Task ShouldReturnNotSuccess_WhenRequestIsEmpty()
    {
        //arrange

        //act
        var response = await TestServer.HttpClient.PostAsync("create-user", null);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.ReadContent<ValidationResult>();

        result.Title.Should().Be("One or more validation errors occurred.");
        result.Errors.Should().HaveCount(4);
        result.Errors["Name"].Should().HaveCount(1).And.AllBe("The name is required");
        result.Errors["Email"].Should().HaveCount(1).And.AllBe("The email is required");
        result.Errors["Phone"].Should().HaveCount(1).And.AllBe("The phone is required");
        result.Errors["Address"].Should().HaveCount(1).And.AllBe("The address is required");

        var users = ReadUsers();
        users.Should().HaveCount(3);
    }

    [Theory]
    [InlineData("Not a number", "InvalidUserType", "InvalidEmail", "349112235421a")]
    [InlineData("-123", "InvalidUserType", "test,test", "+349-1122354215")]
    public async Task ShouldReturnNotSuccess_WhenMoneyValueIsNotANumber(string money, string userType, string email, string phone)
    {
        //arrange
        var name = "Mike";
        var address = "Av. Juan G";

        //act
        var response = await TestServer.HttpClient.PostAsync(BuildUrl(name, email, address, phone, userType, money), null);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var result = await response.ReadContent<ValidationResult>();

        result.Title.Should().Be("One or more validation errors occurred.");
        result.Errors.Should().HaveCount(4);
        result.Errors["Money"].Should().HaveCount(1).And.AllBe($"Invalid money value {money}");
        result.Errors["UserType"].Should().HaveCount(1).And.AllBe($"Invalid user type value {userType}");
        result.Errors["Email"].Should().HaveCount(1).And.AllBe($"Invalid email value {email}");

        var users = ReadUsers();
        users.Should().HaveCount(3);
    }

    [Theory]
    [InlineData("Agustina", "Agustina@gmail.com", "+349 1122354215", "Av. Juan G", false)]
    [InlineData("new", "Juan@marmol.com", "+349 112", "new", false)]
    [InlineData("new", "new@test.com", "+534645213542", "new", false)]
    [InlineData("Agustina", "new@test.com", "+534890", "new", true)]
    [InlineData("Agustina", "new@test.com", "+534890", "Garay y Otra Calle", false)]
    public async Task ShouldReturnNotSuccess_WhenUserIsDuplicated(string name, string email, string phone, string address, bool successfulResult)
    {
        //arrange
        var userType = "Normal";
        var money = "124";

        //act
        var response = await TestServer.HttpClient.PostAsync(BuildUrl(name, email, address, phone, userType, money), null);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.ReadContent<CreateUserResult>();
        var users = ReadUsers();

        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(successfulResult);

        if (!successfulResult)
        {
            result.Errors.Should().Be("The user is duplicated");
            users.Should().HaveCount(3);
        }
        else
        {
            users.Should().HaveCount(4);
        }

    }

    [Fact]
    public async Task ShouldAddNewUser_WhenDataIsValid()
    {
        //arrange
        var name = "Mike";
        var email = "mike@gmail.com";
        var address = "Av. Juan G";
        var phone = "349 1122354215";
        var userType = "Normal";
        var money = "124";

        //act
        var response = await TestServer.HttpClient.PostAsync(BuildUrl(name, email, address, phone, userType, money), null);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.ReadContent<CreateUserResult>();
        var users = ReadUsers();
        var addedUser = users.Last();

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().Be("User Created");

        users.Should().HaveCount(4);

        addedUser.Name.Should().Be(name);
        addedUser.Email.Should().Be(email);
        addedUser.Address.Should().Be(address);
        addedUser.Phone.Should().Be(phone);
        addedUser.UserType.Should().Be(UserType.Normal);
        addedUser.Money.Should().Be(138.88M);
    }


    [Fact]
    public async Task ShouldAddNewUserWithoutCommaInValues_WhenDataIsValid()
    {
        //arrange
        var name = "Mi,k,e";
        var email = "mike@gmail.com";
        var address = "Av. Juan,G";
        var phone = "+349 1122354215";
        var userType = "Normal";
        var money = "124";

        //act
        var response = await TestServer.HttpClient.PostAsync(BuildUrl(name, email, address, phone, userType, money), null);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.ReadContent<CreateUserResult>();
        var users = ReadUsers();
        var addedUser = users.Last();

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().Be("User Created");

        users.Should().HaveCount(4);

        addedUser.Name.Should().Be("Mi k e");
        addedUser.Email.Should().Be(email);
        addedUser.Address.Should().Be("Av. Juan G");
        addedUser.Phone.Should().Be(phone);
        addedUser.UserType.Should().Be(UserType.Normal);
        addedUser.Money.Should().Be(138.88M);
    }

    private static string BuildUrl(string name, string email, string address, string phone, string userType, string money)
    {
        return $"create-user" +
            $"?name={HttpUtility.UrlEncode(name)}" +
            $"&email={HttpUtility.UrlEncode(email)}" +
            $"&address={HttpUtility.UrlEncode(address)}" +
            $"&phone={HttpUtility.UrlEncode(phone)}" +
            $"&userType={HttpUtility.UrlEncode(userType)}" +
            $"&money={HttpUtility.UrlEncode(money)}";
    }

    private IEnumerable<User> ReadUsers()
    {
        var lines = File.ReadAllLines(_userFile);

        return lines.Select(x => UserParser.Parse(x)).ToList();
    }

    private void PrepareDataFiles()
    {
        var testDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), TestUsersFilePath);

        File.Delete(_userFile);
        File.Copy(testDataFilePath, _userFile);
    }
}

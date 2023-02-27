using Backend.TechChallenge.Api.DAL;
using Backend.TechChallenge.Api.Features.Users.Helpers;
using Backend.TechChallenge.Api.Features.Users.Services;
using Backend.TechChallenge.Contracts.Features.Users;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.TechChallenge.Api.Features.Users;

[ApiController]
[Route("[controller]")]
public partial class UsersController : ControllerBase
{
    private readonly IUserMoneyCalculator _userMoneyCalculator;
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public UsersController(IUserMoneyCalculator userMoneyCalculator, IUserRepository userRepository, ILogger logger)
    {
        _userMoneyCalculator = userMoneyCalculator;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpPost]
    [Route("/create-user")]
    public async Task<CreateUserResult> CreateUser([FromQuery] CreateUserRequest request)
    {
        var newUser = BuildUser(request);

        var users = await _userRepository.ReadAllUsers();

        var isDuplicated = users.Any(x =>
            string.Equals(x.Email, newUser.Email, StringComparison.InvariantCultureIgnoreCase)
            || string.Equals(x.Phone, newUser.Phone, StringComparison.InvariantCultureIgnoreCase)
            || string.Equals(x.Name, newUser.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(x.Address, newUser.Address, StringComparison.InvariantCultureIgnoreCase));

        if (isDuplicated)
        {
            _logger
                .ForContext("User", newUser, true)
                .Information("The user is duplicated");

            return new CreateUserResult()
            {
                IsSuccess = false,
                Errors = "The user is duplicated"
            };
        }

        await _userRepository.WriteUserToFile(newUser);

        _logger.Information($"User '{newUser.Name}' has been successfully addedd");

        return new CreateUserResult()
        {
            IsSuccess = true,
            Errors = "User Created"
        };
    }

    private User BuildUser(CreateUserRequest request)
    {
        var userType = string.IsNullOrEmpty(request.UserType) ? UserType.Normal : (UserType)Enum.Parse(typeof(UserType), request.UserType);
        var money = string.IsNullOrEmpty(request.Money) ? 0M : decimal.Parse(request.Money);

        var newUser = new User
        {
            Name = RemoveCommaSplitter(request.Name),
            Email = EmailNormalizer.Normalize(request.Email),
            Address = RemoveCommaSplitter(request.Address),
            Phone = RemoveCommaSplitter(request.Phone),
            UserType = userType,
            Money = _userMoneyCalculator.Calculate(money, userType)
        };

        return newUser;
    }

    private static string RemoveCommaSplitter(string input)
    {
        return input?.Replace(',', ' ');
    }
}

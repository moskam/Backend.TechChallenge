using Backend.TechChallenge.Api.DAL;
using System;

namespace Backend.TechChallenge.Api.Features.Users.Helpers;

public static class UserParser
{
    public static User Parse(string line)
    {
        var values = line.Split(',');
        if (values.Length != 6)
        {
            throw new ArgumentException($"Line with user data is ivalid '{line}'");
        }

        return new User
        {
            Name = values[0],
            Email = values[1],
            Phone = values[2],
            Address = values[3],
            UserType = (UserType)Enum.Parse(typeof(UserType), values[4]),
            Money = decimal.Parse(values[5])
        };
    }
}

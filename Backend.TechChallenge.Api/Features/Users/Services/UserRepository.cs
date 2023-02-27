using Backend.TechChallenge.Api.Configuration;
using Backend.TechChallenge.Api.DAL;
using Backend.TechChallenge.Api.Features.Users.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.TechChallenge.Api.Features.Users.Services;

public interface IUserRepository
{
    Task<IEnumerable<User>> ReadAllUsers();
    Task WriteUserToFile(User user);
}

public class UserRepository : IUserRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOptions<AppConfig> _config;

    public UserRepository(IOptions<AppConfig> config, IMemoryCache memoryCache)
    {
        _config = config;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<User>> ReadAllUsers()
    {
        return await _memoryCache.GetOrCreateAsync(GetCacheKey(), async x =>
        {
            x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_config.Value.UsersCacheTimeInMinutes);

            return await ReadAllUsersFromFile();
        });
    }
    public async Task WriteUserToFile(User user)
    {
        var path = GetFilePath();

        using var outputFile = new StreamWriter(path, append: true);
        await outputFile.WriteLineAsync($"{user.Name},{user.Email},{user.Phone},{user.Address},{user.UserType},{user.Money}");

        _memoryCache.Remove(GetCacheKey()); // clean the cache after changing file
    }

    private async Task<IEnumerable<User>> ReadAllUsersFromFile()
    {
        var result = new List<User>();
        var path = GetFilePath();

        using var fileStream = new FileStream(path, FileMode.Open);
        using var reader = new StreamReader(fileStream);

        while ((await reader.ReadLineAsync()) is { } line)
        {
            result.Add(UserParser.Parse(line));
        }

        return result;
    }

    private string GetFilePath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), _config.Value.UsersFileRelativePath);
    }

    private static string GetCacheKey()
    {
        return "Backend.TechChallenge.Api_Users";
    }
}

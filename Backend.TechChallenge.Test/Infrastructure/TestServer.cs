using Backend.TechChallenge.Api;
using Backend.TechChallenge.Api.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace Backend.TechChallenge.Test.Infrastructure;

public class TestServer : IDisposable
{
   public HttpClient HttpClient { get; }
    public AppConfig AppConfig { get; }

    private readonly Microsoft.AspNetCore.TestHost.TestServer _testServer;

    public TestServer()
    {
        var configuration = TestConfigurationBuilder.Build();
        AppConfig = configuration.GetSection("AppConfig").Get<AppConfig>();

        var webHostBuilder = new WebHostBuilder()
          .UseConfiguration(configuration)
          .ConfigureTestServices(
              services =>
              {
                  services.ConfigureAppSettings(configuration);
              })
          .UseStartup<Startup>();

        _testServer = new Microsoft.AspNetCore.TestHost.TestServer(webHostBuilder);

        HttpClient = _testServer.CreateClient();
    }

    public void Dispose()
    {
        _testServer?.Dispose();
        HttpClient?.Dispose();
    }
}

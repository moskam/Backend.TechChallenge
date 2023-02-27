using System;

namespace Backend.TechChallenge.Test.Infrastructure;

public abstract class BaseTests : IDisposable
{
    protected TestServer TestServer { get; }

    protected BaseTests()
    {
        TestServer = new TestServer();
    }

    public virtual void Dispose()
    {
        TestServer?.Dispose();
    }
}

using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;

namespace Kyle.Infrastructure.CAP.Test;

[TestClass]
public class UnitTest1: TestBase.TestBase
{
    private readonly ICapPublisher _publisher;

    public UnitTest1()
    {
        _publisher = Provider.GetRequiredService<ICapPublisher>();
    }

    [TestMethod]
    public async Task TestMethod1()
    {
      await  _publisher.PublishAsync("Q-Test1", new PublisherData());
    }
}
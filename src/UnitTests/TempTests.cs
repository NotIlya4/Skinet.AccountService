using StackExchange.Redis;

namespace UnitTests;

public class TempTests
{
    // [Fact]
    public void Test()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        IDatabase db = redis.GetDatabase();
    }
}
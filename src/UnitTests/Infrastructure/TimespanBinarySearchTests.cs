using Infrastructure.RefreshTokenService.Helpers;

namespace UnitTests.Infrastructure;

public class TimespanBinarySearchTests
{
    private readonly TimeSpan _expires = TimeSpan.FromMinutes(15);
    
    [Fact]
    public void DeleteCount_OneValidOneInvalid_One()
    {
        int result = Call(new List<DateTime>()
        {
            FromMinutes(-16),
            FromMinutes(-14)
        });
        
        Assert.Equal(1, result);
    }

    [Fact]
    public void DeleteCount_OneInvalid_One()
    {
        int result = Call(new List<DateTime>()
        {
            FromMinutes(-16)
        });
        
        Assert.Equal(1, result);
    }

    [Fact]
    public void DeleteCount_OneValid_Zero()
    {
        int result = Call(new List<DateTime>()
        {
            FromMinutes(-14)
        });
        
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void DeleteCount_TwoValid_Zero()
    {
        int result = Call(new List<DateTime>()
        {
            FromMinutes(-14),
            FromMinutes(-13)
        });
        
        Assert.Equal(0, result);
    }
    
    [Fact]
    public void DeleteCount_TwoInvalidOneValid_One()
    {
        int result = Call(new List<DateTime>()
        {
            FromMinutes(-17),
            FromMinutes(-16),
            FromMinutes(-14)
        });
        
        Assert.Equal(2, result);
    }

    [Fact]
    public void DeleteCount_EmptyList_Zero()
    {
        int result = Call(new List<DateTime>());
        
        Assert.Equal(0, result);
    }

    private int Call(List<DateTime> dates)
    {
        return TimespanBinarySearch.DeleteCount(dates, _expires);
    }

    private DateTime FromMinutes(int minutes)
    {
        return DateTime.UtcNow.AddMinutes(minutes);
    }
}
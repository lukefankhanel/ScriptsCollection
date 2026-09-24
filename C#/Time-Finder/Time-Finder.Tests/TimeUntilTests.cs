namespace Time_Finder.Tests;

public class TimeUntilTests
{
    [Theory]
    [InlineData("22:00", "14:00", "16:00")] // Example from the original request: rolls over to tomorrow.
    [InlineData("10:00", "14:00", "04:00")]
    [InlineData("10:00", "10:30", "00:30")]
    [InlineData("23:59", "00:00", "00:01")]
    [InlineData("00:00", "23:59", "23:59")]
    [InlineData("10:00", "09:59", "23:59")]
    [InlineData("10:00", "10:00", "00:00")]
    public void TimeOnly_ReturnsTimeUntilNextOccurrence(string now, string target, string expected)
    {
        var result = Program.TimeUntil(TimeOnly.Parse(now), TimeOnly.Parse(target));
        Assert.Equal(TimeSpan.Parse(expected), result);
    }

    [Fact]
    public void TimeOnly_IgnoresSecondsOnCurrentTime()
    {
        var result = Program.TimeUntil(new TimeOnly(10, 0, 59), new TimeOnly(11, 0));
        Assert.Equal(TimeSpan.FromHours(1), result);
    }

    [Fact]
    public void DateTime_ReturnsTimeUntilTarget()
    {
        var result = Program.TimeUntil(new DateTime(2026, 9, 24, 22, 0, 0), new DateTime(2026, 9, 26, 14, 0, 0));
        Assert.Equal(new TimeSpan(1, 16, 0, 0), result);
    }

    [Fact]
    public void DateTime_SpansMonthsAndYears()
    {
        var result = Program.TimeUntil(new DateTime(2026, 12, 31, 23, 30, 0), new DateTime(2027, 1, 1, 0, 15, 0));
        Assert.Equal(TimeSpan.FromMinutes(45), result);
    }

    [Fact]
    public void DateTime_IsNegativeForPastTarget()
    {
        var result = Program.TimeUntil(new DateTime(2026, 9, 24, 10, 0, 0), new DateTime(2026, 9, 24, 9, 0, 0));
        Assert.Equal(TimeSpan.FromHours(-1), result);
    }

    [Fact]
    public void DateTime_IgnoresSecondsOnCurrentTime()
    {
        var result = Program.TimeUntil(new DateTime(2026, 9, 24, 10, 0, 59), new DateTime(2026, 9, 24, 11, 0, 0));
        Assert.Equal(TimeSpan.FromHours(1), result);
    }
}

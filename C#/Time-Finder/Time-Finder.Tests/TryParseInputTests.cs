namespace Time_Finder.Tests;

public class TryParseInputTests
{
    [Theory]
    [InlineData("14:00", 14, 0)]
    [InlineData("09:05", 9, 5)]
    [InlineData("9:05", 9, 5)]
    [InlineData("00:00", 0, 0)]
    [InlineData("23:59", 23, 59)]
    [InlineData("2PM", 14, 0)]
    [InlineData("2pm", 14, 0)]
    [InlineData("2 PM", 14, 0)]
    [InlineData("2:30 pm", 14, 30)]
    [InlineData("2:30PM", 14, 30)]
    [InlineData("12AM", 0, 0)]
    [InlineData("12PM", 12, 0)]
    [InlineData("  14:00  ", 14, 0)]
    public void TimeOnly_ParsesWithoutDate(string input, int hour, int minute)
    {
        Assert.True(Program.TryParseInput(input, out var date, out var time));
        Assert.Null(date);
        Assert.Equal(new TimeOnly(hour, minute), time);
    }

    [Theory]
    [InlineData("2026-09-26 14:00", 14, 0)]
    [InlineData("2026-09-26 2PM", 14, 0)]
    [InlineData("2026-09-26 2:30 pm", 14, 30)]
    [InlineData("2026-09-26T14:00", 14, 0)]
    [InlineData("2026-09-26t14:00", 14, 0)]
    [InlineData("2026-09-26   14:00", 14, 0)]
    public void DateAndTime_ParsesBoth(string input, int hour, int minute)
    {
        Assert.True(Program.TryParseInput(input, out var date, out var time));
        Assert.Equal(new DateOnly(2026, 9, 26), date);
        Assert.Equal(new TimeOnly(hour, minute), time);
    }

    [Theory]
    [InlineData("2026-09-26")]
    [InlineData("2026-09-26 ")]
    [InlineData("2026-09-26T")]
    public void DateOnly_MeansMidnight(string input)
    {
        Assert.True(Program.TryParseInput(input, out var date, out var time));
        Assert.Equal(new DateOnly(2026, 9, 26), date);
        Assert.Equal(TimeOnly.MinValue, time);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("24:00")]
    [InlineData("25:00")]
    [InlineData("14:60")]
    [InlineData("13PM")]
    [InlineData("14:00:00")]
    [InlineData("2026-13-01 14:00")]
    [InlineData("2026-02-30 14:00")]
    [InlineData("2026-09-26 25:00")]
    [InlineData("2026-09-26X14:00")]
    [InlineData("2026-09-2614:00")]
    [InlineData("26-09-2026 14:00")]
    [InlineData("2026/09/26 14:00")]
    [InlineData("14:00 2026-09-26")]
    public void InvalidInput_ReturnsFalse(string input)
    {
        Assert.False(Program.TryParseInput(input, out _, out _));
    }
}

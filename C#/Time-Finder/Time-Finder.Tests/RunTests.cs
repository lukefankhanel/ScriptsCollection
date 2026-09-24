namespace Time_Finder.Tests;

public class RunTests
{
    private static readonly DateTime Now = new(2026, 9, 24, 22, 0, 0);

    private static (int ExitCode, string Output, string Error) Run(string input, DateTime now)
    {
        var output = new StringWriter();
        var error = new StringWriter();
        var exitCode = Program.Run(input, now, output, error);
        return (exitCode, output.ToString().Trim(), error.ToString().Trim());
    }

    [Theory]
    [InlineData("14:00", "16:00")]
    [InlineData("2PM", "16:00")]
    [InlineData("23:15", "01:15")]
    [InlineData("22:00", "00:00")]
    [InlineData("21:59", "23:59")]
    public void TimeOnly_OutputsHoursAndMinutes(string input, string expected)
    {
        var (exitCode, output, error) = Run(input, Now);

        Assert.Equal(0, exitCode);
        Assert.Equal(expected, output);
        Assert.Empty(error);
    }

    [Theory]
    [InlineData("2026-09-26 2PM", "01:16:00")]
    [InlineData("2026-09-26T14:00", "01:16:00")]
    [InlineData("2026-09-24 23:30", "00:01:30")]
    [InlineData("2026-09-24 22:00", "00:00:00")]
    [InlineData("2026-09-25", "00:02:00")]
    [InlineData("2026-10-04 22:00", "10:00:00")]
    [InlineData("2027-09-24 22:00", "365:00:00")]
    public void DateAndTime_OutputsDaysHoursAndMinutes(string input, string expected)
    {
        var (exitCode, output, error) = Run(input, Now);

        Assert.Equal(0, exitCode);
        Assert.Equal(expected, output);
        Assert.Empty(error);
    }

    [Fact]
    public void DateAndTime_IgnoresSecondsOnCurrentTime()
    {
        var (_, output, _) = Run("2026-09-24 23:00", new DateTime(2026, 9, 24, 22, 0, 59));
        Assert.Equal("00:01:00", output);
    }

    [Theory]
    [InlineData("2026-09-24 21:59")]
    [InlineData("2026-09-20 14:00")]
    [InlineData("2026-09-24")]
    public void PastDate_ReportsError(string input)
    {
        var (exitCode, output, error) = Run(input, Now);

        Assert.Equal(1, exitCode);
        Assert.Empty(output);
        Assert.Contains("is in the past", error);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("25:00")]
    [InlineData("2026-13-01 14:00")]
    public void InvalidInput_ReportsError(string input)
    {
        var (exitCode, output, error) = Run(input, Now);

        Assert.Equal(1, exitCode);
        Assert.Empty(output);
        Assert.Contains($"Invalid input \"{input}\"", error);
    }
}

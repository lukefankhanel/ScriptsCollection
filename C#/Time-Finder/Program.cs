using System.Globalization;

class Program
{
    // 24-hour HH:MM is the primary format; 12-hour forms like "2PM" or "2:30 PM" are also accepted.
    private static readonly string[] AcceptedFormats =
    [
        "H:mm", "HH:mm",
        "h:mm tt", "h:mmtt", "h tt", "htt",
    ];

    public static int Main(string[] args)
    {
        var input = args.Length > 0 ? string.Join(" ", args) : Prompt();

        if (!TryParseTime(input, out var target))
        {
            Console.Error.WriteLine($"Invalid time \"{input}\". Enter a time in HH:MM format (e.g. 14:00).");
            return 1;
        }

        var now = TimeOnly.FromDateTime(DateTime.Now);
        var remaining = TimeUntil(now, target);

        Console.WriteLine($"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}");
        return 0;
    }

    private static string Prompt()
    {
        Console.Write("Enter a future time (HH:MM): ");
        return Console.ReadLine() ?? "";
    }

    public static bool TryParseTime(string input, out TimeOnly time) =>
        TimeOnly.TryParseExact(input.Trim().ToUpperInvariant(), AcceptedFormats,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out time);

    // Returns the time from now until the next occurrence of target. TimeOnly subtraction
    // wraps around midnight, so a target earlier than now is treated as tomorrow.
    // Seconds on the current time are ignored.
    public static TimeSpan TimeUntil(TimeOnly now, TimeOnly target) =>
        target - new TimeOnly(now.Hour, now.Minute);
}

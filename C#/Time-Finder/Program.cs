using System.Globalization;

public class Program
{
    // 24-hour HH:MM is the primary format; 12-hour forms like "2PM" or "2:30 PM" are also accepted.
    private static readonly string[] AcceptedFormats =
    [
        "H:mm", "HH:mm",
        "h:mm tt", "h:mmtt", "h tt", "htt",
    ];

    private const string DateFormat = "yyyy-MM-dd";

    public static int Main(string[] args)
    {
        var input = args.Length > 0 ? string.Join(" ", args) : Prompt();
        return Run(input, DateTime.Now, Console.Out, Console.Error);
    }

    // Writes the time remaining until input to output, or an error message to error.
    // Returns the process exit code.
    public static int Run(string input, DateTime now, TextWriter output, TextWriter error)
    {
        if (!TryParseInput(input, out var date, out var time))
        {
            error.WriteLine($"Invalid input \"{input}\". Enter a time in HH:MM format (e.g. 14:00), " +
                "optionally preceded by a YYYY-MM-DD date (e.g. 2026-09-26 14:00).");
            return 1;
        }

        if (date is null)
        {
            var remaining = TimeUntil(TimeOnly.FromDateTime(now), time);
            output.WriteLine($"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}");
            return 0;
        }

        var remainingWithDate = TimeUntil(now, date.Value.ToDateTime(time));
        if (remainingWithDate < TimeSpan.Zero)
        {
            error.WriteLine($"\"{input}\" is in the past.");
            return 1;
        }

        output.WriteLine($"{remainingWithDate.Days:D2}:{remainingWithDate.Hours:D2}:{remainingWithDate.Minutes:D2}");
        return 0;
    }

    private static string Prompt()
    {
        Console.Write("Enter a future time (HH:MM or YYYY-MM-DD HH:MM): ");
        return Console.ReadLine() ?? "";
    }

    // Parses "HH:MM" or "YYYY-MM-DD HH:MM". The date may also be joined to the time with
    // a "T" (e.g. 2026-09-26T14:00). A date on its own means midnight at the start of that day.
    public static bool TryParseInput(string input, out DateOnly? date, out TimeOnly time)
    {
        input = input.Trim();
        date = null;
        time = default;

        if (input.Length >= DateFormat.Length &&
            DateOnly.TryParseExact(input[..DateFormat.Length], DateFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            date = parsedDate;
            input = input[DateFormat.Length..];
            if (input.StartsWith('T') || input.StartsWith('t'))
            {
                input = input[1..];
            }
            else if (input.Length > 0 && !char.IsWhiteSpace(input[0]))
            {
                return false;
            }

            if (input.Trim().Length == 0)
            {
                return true;
            }
        }

        return TryParseTime(input, out time);
    }

    public static bool TryParseTime(string input, out TimeOnly time) =>
        TimeOnly.TryParseExact(input.Trim().ToUpperInvariant(), AcceptedFormats,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out time);

    // Returns the time from now until the next occurrence of target. TimeOnly subtraction
    // wraps around midnight, so a target earlier than now is treated as tomorrow.
    // Seconds on the current time are ignored.
    public static TimeSpan TimeUntil(TimeOnly now, TimeOnly target) =>
        target - new TimeOnly(now.Hour, now.Minute);

    // Returns the time from now until target, which is negative if target is in the past.
    // Seconds on the current time are ignored.
    public static TimeSpan TimeUntil(DateTime now, DateTime target) =>
        target - new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
}

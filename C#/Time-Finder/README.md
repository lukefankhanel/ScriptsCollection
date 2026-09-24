A simple command-line application that calculates how many hours and minutes remain until a given future time, based on the current time.

## Usage

```
dotnet run -- 14:00
```

Or run with no arguments to be prompted for a time. Input and output are in `HH:MM` format (24-hour). 12-hour input such as `2PM` or `2:30 PM` is also accepted.

If the entered time is earlier than the current time, it is treated as that time tomorrow. For example, at 22:00 an input of `14:00` outputs `16:00`.

### Future dates

Put an ISO date (`YYYY-MM-DD`) in front of the time to count down to a specific day. The output is then in `DD:HH:MM` format.

```
dotnet run -- 2026-09-26 2PM
dotnet run -- 2026-09-26T14:00
```

A date on its own (`2026-09-26`) means midnight at the start of that day. Dates in the past are rejected.

## Tests

```
dotnet test
```
